using ASM_UI.CustomAuthentication;
using ASM_UI.Models;
using ASM_UI.App_Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ASM_UI.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private ModelAsmRemote _db = new ModelAsmRemote();

        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account", null);
            //return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "", string token = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            #region "Process Token from Email Notification"
            if (token.Trim() != string.Empty)
            {
                try
                {
                    token = token.Replace("plus", "+").Replace("equal", "=");
                    token = App_Helpers.CryptorHelper.Decrypt(token, "MD5", true);

                    string[] arrToken = token.Split('|');
                    if (arrToken.Count() > 0)
                    {
                        string controller = arrToken[0];
                        string actionname = arrToken[1];
                        string username = arrToken[2];
                        int companyid = Convert.ToInt32(arrToken[3]);
                        int locationregid = Convert.ToInt32(arrToken[4]);

                        //isi login form
                        var user = (CustomMembershipUser)Membership.GetUser(username, false);
                        if (user != null)
                        {
                            ASM_UI.Models.CustomSerializeViewModel userModel = new ASM_UI.Models.CustomSerializeViewModel()
                            {
                                user_id = user.user_id,
                                user_name = user.user_name,
                                user_password = user.user_password,
                                user_type_id = user.user_type_id,

                                employee_id = user.employee_id,
                                employee_nik = user.employee_nik,
                                employee_name = user.employee_name,
                                employee_email = user.employee_email,

                                fl_active = user.fl_active,

                                RoleCode = user.Roles.Select(r => r.RoleCode).ToList()
                            };

                            #region setelah login isi user profile 
                            USER_PROFILE UserProfile = Session["USER_PROFILE"] as USER_PROFILE;

                            UserProfile.UserId = userModel.user_id;
                            UserProfile.UserName = userModel.user_name;

                            UserProfile.user_type_id = userModel.user_type_id;
                            UserProfile.ms_user_type = _db.ms_user_type.Find(userModel.user_type_id);

                            UserProfile.company_id = companyid;
                            UserProfile.asset_reg_location_id = locationregid;
                            UserProfile.register_location = _db.ms_asset_register_location.Find(UserProfile.asset_reg_location_id);

                            UserProfile.employee_id = userModel.employee_id;
                            UserProfile.UserFullName = userModel.employee_name;
                            UserProfile.ms_employee = _db.ms_employee.Find(userModel.employee_id);

                            ms_employee_detail employee_detail = _db.ms_employee_detail.Where(w => w.employee_id == userModel.employee_id && w.company_id == companyid).FirstOrDefault<ms_employee_detail>();

                            if (employee_detail != null)
                            {
                                UserProfile.CompanyName = employee_detail.ms_asmin_company.company_name;
                                UserProfile.ms_department = employee_detail.ms_department;
                                UserProfile.department_id = UserProfile.ms_department.department_id;

                                UserProfile.ms_job_level = employee_detail.ms_job_level;
                                UserProfile.job_level_id = UserProfile.ms_job_level.job_level_id;

                                //UserProfile.ms_user_type = employee_detail.ms_user_type;
                                //UserProfile.user_type_id = UserProfile.ms_user_type.user_type_id;

                                if (employee_detail.ms_approval_range != null)
                                {
                                    UserProfile.ms_approval_range = employee_detail.ms_approval_range;
                                    UserProfile.range_id = UserProfile.ms_approval_range.range_id;
                                    UserProfile.fl_approver = (UserProfile.range_id > 0);
                                }
                            }
                            else //null tidak dapaat menu/role
                            {
                                UserProfile.CompanyName = "";
                                UserProfile.ms_department = new ms_department() { department_id = 0 };
                                UserProfile.department_id = UserProfile.ms_department.department_id;

                                UserProfile.ms_job_level = new ms_job_level() { job_level_id = 0 };
                                UserProfile.job_level_id = UserProfile.ms_job_level.job_level_id;

                                UserProfile.ms_user_type = new ms_user_type() { user_type_id = 0 };
                                UserProfile.user_type_id = UserProfile.ms_user_type.user_type_id;

                                UserProfile.ms_approval_range = new ms_approval_range() { range_id = 0 };
                                UserProfile.range_id = UserProfile.ms_approval_range.range_id;
                                UserProfile.fl_approver = false;
                            }
                            app_logwriter.ToLog(UserProfile.UserName + " logged-in with SessionID=" + Session.SessionID);
                            app_logwriter.ToLog(string.Format("UserName:{0}, Employee:{1}, UserType={2}, JobLevel={3}, Department={4}, Company={5}, Location={6}, Register={7}"
                                , UserProfile.UserName
                                , UserProfile.ms_employee.employee_name
                                , UserProfile.ms_user_type.user_type_name
                                , UserProfile.ms_job_level.job_level_name
                                , UserProfile.ms_department.department_name
                                , UserProfile.CompanyName
                                , UserProfile.location_name
                                , UserProfile.register_location.asset_reg_location_name));

                            #endregion
                            string userData = JsonConvert.SerializeObject(userModel);
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                                (
                                1, username.ToString(), DateTime.Now, DateTime.Now.AddHours(5), false, userData
                                );

                            string enTicket = FormsAuthentication.Encrypt(authTicket);
                            string cookie_name = app_setting.COOKIE_NAME + UserProfile.UserName;
                            //HttpCookie faCookie = new HttpCookie(cookie_name, enTicket);
                            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, enTicket);
                            Response.Cookies.Add(faCookie);
                        }

                        return RedirectToAction(actionname, controller);
                    }
                }
                catch (Exception ex)
                {
                    app_logwriter.ToLog("Token Invalid:" + ex.Message);
                    ModelState.AddModelError("", "Token Invalid.");
                }
            }
            #endregion

            string controllerName = RouteData.Values["controller"].ToString().ToLower();
            string actionName = RouteData.Values["action"].ToString().ToLower();
            ReturnUrl = (controllerName.Equals("account") && actionName.Equals("login")) ? "/" : ReturnUrl;
            var loginView = new AccountLoginViewModel()
            {
                company_id = 0,
                company_list = _db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                asset_reg_location_id = 0,
                asset_register_location_list = _db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                user_type_id = 0,
                user_type_list = _db.ms_user_type.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                remember_me = true,
                return_url = ReturnUrl,
            };
            ViewBag.ReturnUrl = loginView.return_url;
            return View(loginView);
        }

        [HttpPost]
        public ActionResult Login(AccountLoginViewModel loginView, string ReturnUrl = "")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Membership.ValidateUser(loginView.UserName, App_Helpers.CryptorHelper.Encrypt(loginView.Password, "MD5", true)))
                    {
                        var m_user = (CustomMembershipUser)Membership.GetUser(loginView, false);
                        if (m_user != null)
                        {
                            ASM_UI.Models.CustomSerializeViewModel userModel = new ASM_UI.Models.CustomSerializeViewModel()
                            {
                                user_id = m_user.user_id,
                                user_name = m_user.user_name,
                                user_password = m_user.user_password,
                                user_type_id = loginView.user_type_id,

                                employee_id = m_user.employee_id,
                                employee_nik = m_user.employee_nik,
                                employee_name = m_user.employee_name,
                                employee_email = m_user.employee_email,

                                fl_active = m_user.fl_active,

                                RoleCode = m_user.Roles.Select(r => r.RoleCode).ToList()
                            };

                            #region setelah login isi user profile 
                            USER_PROFILE UserProfile = Session["USER_PROFILE"] as USER_PROFILE;

                            UserProfile.UserId = userModel.user_id;
                            UserProfile.UserName = userModel.user_name;

                            UserProfile.user_type_id = userModel.user_type_id;
                            UserProfile.ms_user_type = _db.ms_user_type.Find(userModel.user_type_id);

                            UserProfile.company_id = loginView.company_id;
                            UserProfile.asset_reg_location_id = loginView.asset_reg_location_id;
                            UserProfile.register_location = _db.ms_asset_register_location.Find(UserProfile.asset_reg_location_id);

                            UserProfile.employee_id = userModel.employee_id;
                            UserProfile.UserFullName = userModel.employee_name;
                            UserProfile.ms_employee = _db.ms_employee.Find(userModel.employee_id);


                            ms_employee_detail employee_detail = _db.ms_employee_detail.Where(w => w.employee_id == userModel.employee_id
                            && w.company_id == UserProfile.company_id
                            && w.asset_reg_location_id == UserProfile.asset_reg_location_id
                            ).FirstOrDefault<ms_employee_detail>();

                            if (employee_detail != null)
                            {
                                UserProfile.CompanyName = employee_detail.ms_asmin_company.company_name;
                                UserProfile.ms_department = employee_detail.ms_department;
                                UserProfile.department_id = UserProfile.ms_department.department_id;

                                UserProfile.ms_job_level = employee_detail.ms_job_level;
                                UserProfile.job_level_id = UserProfile.ms_job_level.job_level_id;

                                //UserProfile.ms_user_type = employee_detail.ms_user_type;
                                //UserProfile.user_type_id = UserProfile.ms_user_type.user_type_id;

                                UserProfile.location_id = employee_detail.location_id;
                                ms_asset_location asset_location = _db.ms_asset_location.Where(w => w.location_id == UserProfile.location_id).FirstOrDefault<ms_asset_location>();
                                UserProfile.location_name = asset_location.location_name;

                                if (employee_detail.ms_approval_range != null)
                                {
                                    UserProfile.ms_approval_range = employee_detail.ms_approval_range;
                                    UserProfile.range_id = UserProfile.ms_approval_range.range_id;
                                    UserProfile.fl_approver = (UserProfile.range_id > 0);
                                }
                            }
                            else //null tidak dapaat menu/role
                            {
                                UserProfile.CompanyName = "";
                                UserProfile.ms_department = new ms_department() { department_id = 0 };
                                UserProfile.department_id = UserProfile.ms_department.department_id;

                                UserProfile.ms_job_level = new ms_job_level() { job_level_id = 0 };
                                UserProfile.job_level_id = UserProfile.ms_job_level.job_level_id;

                                UserProfile.ms_user_type = new ms_user_type() { user_type_id = 0 };
                                UserProfile.user_type_id = UserProfile.ms_user_type.user_type_id;

                                UserProfile.ms_approval_range = new ms_approval_range() { range_id = 0 };
                                UserProfile.range_id = UserProfile.ms_approval_range.range_id;
                                UserProfile.fl_approver = false;
                            }
                            app_logwriter.ToLog(UserProfile.UserName + " logged-in with SessionID=" + Session.SessionID);
                            app_logwriter.ToLog(string.Format("UserName:{0}, Employee:{1}, UserType={2}, JobLevel={3}, Department={4}, Company={5}, Location={6}, Register={7}"
                                , UserProfile.UserName
                                , UserProfile.ms_employee.employee_name
                                , UserProfile.ms_user_type.user_type_name
                                , UserProfile.ms_job_level.job_level_name
                                , UserProfile.ms_department.department_name
                                , UserProfile.CompanyName
                                , UserProfile.location_name
                                , UserProfile.register_location.asset_reg_location_name));
                            #endregion

                            string userData = JsonConvert.SerializeObject(userModel);
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                1, loginView.UserName, DateTime.Now, DateTime.Now.AddHours(5), false, userData
                                );

                            string enTicket = FormsAuthentication.Encrypt(authTicket);
                            string cookie_name = app_setting.COOKIE_NAME + UserProfile.UserName;
                            //HttpCookie faCookie = new HttpCookie(cookie_name, enTicket);
                            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, enTicket);
                            Response.Cookies.Add(faCookie);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Something Wrong : User not Found or user login not matched.");
                            //throw new Exception("Something Wrong : User not Found or user login not matched.");
                        }

                        string controllerName = RouteData.Values["controller"].ToString().ToLower();
                        string actionName = RouteData.Values["action"].ToString().ToLower();
                        ReturnUrl = (controllerName.Equals("account") && actionName.Equals("login")) ? "/" : ReturnUrl;

                        //if (Url.IsLocalUrl(ReturnUrl))
                        if (!string.IsNullOrWhiteSpace(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                        ModelState.AddModelError("", "Something Wrong : Username/nik or Password invalid.");
                }
                else
                    ModelState.AddModelError("", "Something Wrong : Username/nik or Password invalid.");
            }
            catch (Exception _ex)
            {
                //ModelState.AddModelError("", "Invalid Login." + ex.Message);
                App_Helpers.app_logwriter.ToLog("Invalid Login." + _ex.Message);
                ModelState.AddModelError("", "Invalid Login.");
            }

            if (loginView.company_list == null || loginView.company_list.Count == 0)
                loginView.company_list = _db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (loginView.asset_register_location_list == null || loginView.asset_register_location_list.Count == 0)
                loginView.asset_register_location_list = _db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (loginView.user_type_list == null || loginView.user_type_list.Count == 0)
                loginView.user_type_list = _db.ms_user_type.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            return View(loginView);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(AccountRegistrationViewModel registrationView)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;

            if (ModelState.IsValid)
            {
                // Email Verification
                string userName = Membership.GetUserNameByEmail(registrationView.employee_email);
                if (!string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
                    return View(registrationView);
                }

                //Save User Data 
                using (ModelAsmRemote _db = new ModelAsmRemote())
                {
                    //var user = new User()
                    //{
                    //    Username = registrationView.Username,
                    //    FirstName = registrationView.FirstName,
                    //    LastName = registrationView.LastName,
                    //    Email = registrationView.Email,
                    //    Password = registrationView.Password,
                    //    ActivationCode = Guid.NewGuid(),
                    //};
                    //dbContext.Users.Add(user);
                    var emp = new ms_employee()
                    {
                        employee_email = registrationView.employee_email,
                        employee_nik = registrationView.employee_nik,
                        employee_name = registrationView.employee_name,

                        fl_active = true,
                        created_by = UserProfile.UserId,
                        created_date = DateTime.Now,
                        updated_by = UserProfile.UserId,
                        updated_date = DateTime.Now,
                        deleted_by = null,
                        deleted_date = null,
                        org_id = UserProfile.OrgId
                    };

                    emp = _db.ms_employee.Add(emp);
                    //int emp_id = emp.employee_id;

                    var user = new ms_user()
                    {
                        user_name = registrationView.user_name,
                        user_password = App_Helpers.CryptorHelper.Encrypt(registrationView.user_password, "MD5", true),
                        employee_id = emp.employee_id,
                        fl_active = true,
                        created_by = UserProfile.UserId,
                        created_date = DateTime.Now,
                        updated_by = UserProfile.UserId,
                        updated_date = DateTime.Now,
                        deleted_by = null,
                        deleted_date = null,
                        org_id = UserProfile.OrgId
                    };
                    _db.ms_user.Add(user);

                    _db.SaveChanges();
                }

                //Verification Email:
                //TIDAK usah aktifkan by email -> admin saja yanag mengaktifkan sendiri krna hrus pilih role, job title dan company
                //VerificationEmail(registrationView.Email, registrationView.ActivationCode.ToString());
                messageRegistration = "Your account has been created successfully. ^_^";
                statusRegistration = true;
            }
            else
            {
                messageRegistration = "Something Wrong!";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;

            return View(registrationView);
        }

        [HttpGet]
        public ActionResult ActivationAccount(string id)
        {
            //bool statusAccount = false;
            //using (ModelAsmRemote dbContext = new ModelAsmRemote())
            //{
            //    var userAccount = dbContext.ms_user.Where(u => u.ActivationCode.ToString().Equals(id)).FirstOrDefault();

            //    if (userAccount != null)
            //    {
            //        userAccount.IsActive = true;
            //        dbContext.SaveChanges();
            //        statusAccount = true;
            //    }
            //    else
            //    {
            //        ViewBag.Message = "Something Wrong !!";
            //    }

            //}
            //ViewBag.Status = statusAccount;
            ViewBag.Status = true;
            return View();
        }

        public ActionResult LogOut()
        {
            try
            {
                app_logwriter.ToLog(UserProfile.UserName + " logged-out-SesssionID=" + Session.SessionID);
                //string _var_c = FormsAuthentication.FormsCookieName;

                string cookie_name = app_setting.COOKIE_NAME + ((USER_PROFILE)Session["USER_PROFILE"]).UserName;
                //HttpCookie authCookie = Request.Cookies[cookie_name];
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                authCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Remove(authCookie.Name);

                HttpCookie cookie = new HttpCookie("Cookie1", "");
                cookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie);
            }
            catch (Exception _ex)
            {
                app_logwriter.ToLog("LogOut" + _ex.Message.ToString());
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);

        }

        [NonAction]
        public void VerificationEmail(string email, string activationCode)
        {
            var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

            var fromEmail = new MailAddress(App_Helpers.app_setting.MAIL_FROM, "Activation Account - ASM");
            var toEmail = new MailAddress(email);

            var fromEmailPassword = App_Helpers.app_setting.MAIL_USERPASSWORD;
            string subject = "Activation Account !";

            string body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Activation Account ! </a>";

            var smtp = new SmtpClient
            {
                Host = App_Helpers.app_setting.MAIL_SMTPSERVER,
                Port = Convert.ToInt32(App_Helpers.app_setting.MAIL_SMTPPORT),
                EnableSsl = App_Helpers.app_setting.MAIL_ENABLE_SSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = App_Helpers.app_setting.MAIL_USE_DEFAULT_CREDENTIAL,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })

                smtp.Send(message);

        }


        [HttpGet]
        public ActionResult ForgotPassword()
        {

            return View();
        }


        [HttpPost]
        public ActionResult ForgotPassword(AccountForgotPasswordViewModel forgotPassView)
        {

            return RedirectToAction("Login", "Account", null);
            //return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            AccountChangePasswordViewModel changePwd = new AccountChangePasswordViewModel()
            {
                UserName = UserProfile.UserName,
                user_id = UserProfile.UserId
            };

            ViewBag.Status = true;
            return View(changePwd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(AccountChangePasswordViewModel model)
        {
            ms_user _user = (from t in _db.ms_user
                             where t.user_name == model.UserName && t.user_id == model.user_id
                             select t).SingleOrDefault<ms_user>();

            if (_user != null)
            {
                if (model.NewPassword.ToLower().Equals(model.ConfirmPassword.ToLower()))
                {
                    _user.user_password = App_Helpers.CryptorHelper.Encrypt(model.NewPassword, "MD5", true);
                    _user.fl_active = true;
                    _user.updated_by = UserProfile.UserId;
                    _user.updated_date = DateTime.Now;
                    _user.deleted_by = null;
                    _user.deleted_date = null;
                    _db.Entry(_user).State = EntityState.Modified;
                    _db.SaveChanges();
                    ViewBag.ErrMessage = "Your password has been successfully changed.";
                    RedirectToAction("Index", "Account", null);
                    //RedirectToAction("Logout", "Account", null);
                }
                else
                {
                    //beda confirm
                    ViewBag.ErrMessage = "[New Password] not matched to [Confirm Password].";
                }

            }
            else
            {
                ViewBag.ErrMessage = "User " + _user.user_name + " not found...";
            }
            return View(model);
        }
    }
}