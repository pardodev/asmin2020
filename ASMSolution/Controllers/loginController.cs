using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ASM_UI.Models;

namespace ASM_UI.Controllers
{
    public class loginController : BaseController
    {
        private ModelAsmRemote _db = new ModelAsmRemote();

        public ActionResult Index(string ReturnUrl)
        {
            SignOut();
            return View();
        }

        private ms_user GetUserByUsername(string username)
        {            
            return (from u in _db.ms_user where u.user_name == username select u).SingleOrDefault();
        }

        private bool IsValidAdmin(AccountLoginViewModel user)
        {
            bool booValid = (user.UserName == "admin" && user.Password == "asm123!");
            return booValid;
        }

        private bool IsValidUser(AccountLoginViewModel user)
        {
            bool booValid = false;
            ms_user _user = GetUserByUsername(user.UserName); //ambil dari db
            if (_user != null)
                booValid = user.Password.Equals(_user.user_password);
            else //user not found
                booValid = false;
            return booValid;
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(AccountLoginViewModel user, string ReturnUrl)
        {
            string _controller = "home";
            user.ReturnUrl = ReturnUrl;

            if(!String.IsNullOrWhiteSpace(user.UserName))
            {
                ms_user _user = null;
                if (IsValidAdmin(user))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, true);
                    _user = new ms_user()
                    {
                        user_id = 99999,
                        user_name = user.UserName,
                        user_password = user.Password
                    };
                    UserProfile.UserId = _user.user_id;
                    UserProfile.UserName = _user.user_name;
                    UserProfile.UserFullName = "System Admin";
                    UserProfile.CompanyName = "SYS";

                }
                else if (IsValidUser(user))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, true);
                    _user = GetUserByUsername(user.UserName);

                    UserProfile.UserId = _user.user_id;
                    UserProfile.UserName = _user.user_name;
                    UserProfile.UserFullName = _user.ms_employee.employee_name;
                    UserProfile.CompanyName = "ABB"; // _user.ms_employee.ms_employee_detail.FirstOrDefault().ms_asmin_company.company_name;
                    UserProfile.Employee = null;
                }

                ViewData["user"] = _user;
                if (ReturnUrl != null)
                    return Redirect(ReturnUrl);

                //Request.IsAuthenticated;
                //if (!Request.IsAuthenticated)
                //{
                //    //return RedirectToAction("index", "home");
                //}
            }
            else
            {
                _controller = "login";
                //return RedirectToAction("index", "login");
            }
            return RedirectToAction("Index", _controller);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

    }
}