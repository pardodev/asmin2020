using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;
using ASM_UI.App_Helpers;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class disposalCSRController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        private string app_root_path;
        private string base_file_path;

        // GET: disposalprocess
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// list utk Disposal Request index
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult GetDisposalList()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var _qry = (from dp in db.tr_disposal_announcement
                        where dp.deleted_date == null && dp.approval_disposal_type_id == 2
                        orderby dp.announcement_id descending

                        join dr in db.tr_disposal_request on dp.request_id equals dr.request_id
                        where (dr.fl_active == true && dr.deleted_date == null)

                        join md in db.ms_disposal_type on dp.approval_disposal_type_id equals md.disposal_type_id
                        where (md.disposal_by_dept_id == UserProfile.department_id)

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        //where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_asmin_company on a.company_id equals b.company_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_department on dr.request_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.request_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_register_location on dr.request_location_id equals e.asset_reg_location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_request_status on dr.request_status_id equals f.request_status_id

                        select new disposalViewModel()
                        {
                            announcement_id = dp.announcement_id,
                            announcement_date = dp.announcement_date,
                            fl_SuggestionChanges = dp.fl_suggestion_changes,
                            fl_fin_announcement = dp.fl_fin_announcement,
                            fin_announcement_date = dp.fin_announcement_date,
                            fin_announcement_dept_id = dp.fin_announcement_dept_id,

                            fl_remove_asset = dp.fl_remove_asset,
                            remove_asset_date = dp.remove_asset_date,

                            asset_id = dr.asset_id,
                            asset_number = a.asset_number,
                            asset_name = a.asset_name,

                            disposal_number = dr.disposal_number,
                            request_id = dr.request_id,
                            request_date = dr.request_date,
                            fl_approval = dr.fl_approval,
                            request_status_name = f.request_status_name,
                            approval_date = dr.approval_date,

                            company = b,
                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.asset_reg_location_name
                        }).ToList<disposalViewModel>();

            return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
        }

        // GET: DisposalProcess/details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_disposal_announcement disposal_req = db.tr_disposal_announcement.Find(id);
            if (disposal_req == null)
            {
                return HttpNotFound("Assset not found.");
            }

            disposalViewModel disposalprocess = new disposalViewModel();
            disposalprocess = DataDisposalView(id, disposalprocess);

            return View(disposalprocess);
        }

        // GET: DisposalProcess/process/5
        public ActionResult Process(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_disposal_announcement disposal_req = db.tr_disposal_announcement.Find(id);
            if (disposal_req == null)
            {
                return HttpNotFound("Assset not found.");
            }

            disposalViewModel disposalprocess = new disposalViewModel();
            disposalprocess = DataDisposalView(id, disposalprocess);

            return View(disposalprocess);
        }

        // POST: disposal/Approval
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Process")]
        public ActionResult DisposalFollowup([Bind(Include = "announcement_id, request_id, announcement_upload_address, announcement_description, asset_number, asset_name, location_name, department_name, fl_SuggestionChanges, approval_suggestion_id")] disposalViewModel disposal_prs)
        {
            tr_disposal_announcement disposal_announcement = db.tr_disposal_announcement.Find(disposal_prs.announcement_id);

            if (disposal_prs.fl_SuggestionChanges == null || disposal_prs.fl_SuggestionChanges == false)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["announcement_upload_address"];
                    if (file == null || file.ContentLength == 0)
                    {
                        ModelState.AddModelError("announcement_upload_address", "Berita Acara is Mandatory.");
                    }
                }
            }
            else
            {
                if (disposal_prs.approval_suggestion_id == null || disposal_prs.approval_suggestion_id == 0)
                    ModelState.AddModelError("approval_suggestion_id", "Suggestion is Mandatory.");

            }
            //update disposal request and approval data with transaction
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (disposal_prs.fl_SuggestionChanges == null || disposal_prs.fl_SuggestionChanges == false)
                        {
                            //Process Normal
                            #region "Save update Disposal Announcement"
                            if (Request.Files.Count > 0)
                            {
                                //var file = Request.Files[0];
                                app_root_path = Server.MapPath("~/");
                                if (string.IsNullOrWhiteSpace(base_file_path))
                                    base_file_path = asset_registrationViewModel.path_file_disposal;

                                string _file = Server.MapPath(base_file_path);
                                if (!Directory.Exists(_file))
                                    Directory.CreateDirectory(_file);

                                var file = Request.Files["announcement_upload_address"];
                                if (file != null && file.ContentLength > 0)
                                {
                                    var fileName = "dispose_precurement_" + disposal_prs.request_id.ToString() + "_" + Path.GetFileName(file.FileName);
                                    var path = Path.Combine(_file, fileName);
                                    file.SaveAs(path);

                                    disposal_announcement.announcement_upload_address = fileName;
                                    disposal_announcement.announcement_description = disposal_prs.announcement_description;
                                    disposal_announcement.announcement_date = DateTime.Now;
                                    disposal_announcement.announcement_emp_id = UserProfile.employee_id;

                                    disposal_announcement.fl_remove_asset = true;
                                    disposal_announcement.remove_asset_dept_id = 5;
                                }
                            }


                            disposal_announcement.updated_date = DateTime.Now;
                            disposal_announcement.updated_by = UserProfile.UserId;
                            disposal_announcement.deleted_date = null;
                            disposal_announcement.deleted_by = null;

                            db.Entry(disposal_announcement).State = EntityState.Modified;
                            db.SaveChanges();
                            #endregion

                            #region "kirim email ke PIC Process Disposal"
                            string emailsetting = string.Empty;
                            string to_name = string.Empty;
                            string freetext = string.Empty;


                            emailsetting = "EMAIL_TO_DISPOSAL_ACCOUNTING";
                            to_name = "Department Accounting";
                            freetext = "Dokumen <strong>Berita Acara</strong> sudah tersedia. Asset menunggu tindak lanjut (<strong>Dispose</strong>) oleh Accounting Department.";


                            var _emailto = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains(emailsetting));

                            sy_email_log sy_email_log = new sy_email_log();
                            sy_email_log.elog_to = _emailto.FirstOrDefault().app_value;
                            sy_email_log.elog_subject = string.Format("Asset Disposal Need Follow Up (Donation)");
                            sy_email_log.elog_template = "EMAIL_TEMPLATE_05";

                            #region "body mail"
                            var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_05"));
                            string strBodyMail = _bodymail.FirstOrDefault().app_value;
                            strBodyMail = strBodyMail.Replace("[to]", to_name);
                            strBodyMail = strBodyMail.Replace("[assetnumber]", disposal_prs.asset_number);
                            strBodyMail = strBodyMail.Replace("[aseetname]", disposal_prs.asset_name);
                            strBodyMail = strBodyMail.Replace("[assetlocation]", disposal_prs.location_name);
                            strBodyMail = strBodyMail.Replace("[department]", disposal_prs.department_name);
                            strBodyMail = strBodyMail.Replace("[suggestion]", "Donation");
                            strBodyMail = strBodyMail.Replace("[freetext]", freetext);

                            string linkapp = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~/Account/Login?ReturnUrl=%2f"));
                            string strImg = string.Format("http://{0}/Content/EmailImage/button_asmin.png", Request.Url.Authority);
                            linkapp = string.Format(@"<a href={0}><img src=""{1}"" alt=""click for process""/></a>", linkapp, strImg);
                            strBodyMail = strBodyMail.Replace("[link]", linkapp);

                            sy_email_log.elog_body = strBodyMail;
                            #endregion

                            var EmailHelper = new EmailHelper()
                            {
                                ToAddress = sy_email_log.elog_to,
                                Email_Template = sy_email_log.elog_template,
                                MailSubject = sy_email_log.elog_subject,
                                MailBody = sy_email_log.elog_body
                            };
                            EmailHelper.Send();
                            #endregion
                        }
                        else if (disposal_prs.fl_SuggestionChanges == true)
                        {
                            #region "Update Data Announcement"
                            //update flag active = false
                            disposal_announcement.fl_suggestion_changes = true;
                            disposal_announcement.fl_active = false;
                            disposal_announcement.updated_date = DateTime.Now;
                            disposal_announcement.updated_by = UserProfile.UserId;
                            disposal_announcement.deleted_date = null;
                            disposal_announcement.deleted_by = null;

                            db.Entry(disposal_announcement).State = EntityState.Modified;
                            db.SaveChanges();
                            #endregion

                            #region"Save create disposal_approval"
                            var refApproval = (from a in db.ms_job_level
                                               where (a.fl_active == true && a.deleted_date == null
                                                       && a.job_level_id == 2)

                                               join b in db.ms_employee_detail on a.job_level_id equals b.job_level_id
                                               where (b.fl_active == true && b.deleted_date == null
                                                       && b.department_id == 10 && b.company_id == UserProfile.company_id)

                                               join c in db.ms_employee on b.employee_id equals c.employee_id
                                               where c.fl_active == true && c.deleted_date == null

                                               select new disposalViewModel()
                                               {
                                                   department_id = b.department_id,
                                                   employee_id = b.employee_id,
                                                   job_level_id = a.job_level_id,
                                                   employee_email = c.employee_email,
                                                   employee_name = c.employee_name
                                               }).First<disposalViewModel>();

                            //approval disposal changes level 1
                            tr_disposal_approval disposal_approval = new tr_disposal_approval();
                            disposal_approval.request_id = disposal_prs.request_id;
                            disposal_approval.approval_date = null;
                            disposal_approval.approval_dept_id = refApproval.department_id;
                            disposal_approval.approval_employee_id = refApproval.employee_id;
                            disposal_approval.approval_level_id = refApproval.job_level_id;
                            disposal_approval.approval_status_id = 1;//waiting approval
                            disposal_approval.approval_suggestion_id = disposal_prs.approval_suggestion_id;
                            disposal_approval.fl_active = true;
                            disposal_approval.created_date = DateTime.Now;
                            disposal_approval.created_by = UserProfile.UserId;
                            disposal_approval.updated_date = DateTime.Now;
                            disposal_approval.updated_by = UserProfile.UserId;
                            disposal_approval.deleted_date = null;
                            disposal_approval.deteled_by = null;
                            disposal_approval.org_id = UserProfile.OrgId;
                            disposal_approval = db.tr_disposal_approval.Add(disposal_approval);
                            db.SaveChanges();

                            int approval_id = disposal_approval.approval_id;

                            //approval disposal changes level 2
                            tr_disposal_request disposalrequest = db.tr_disposal_request.Find(disposal_prs.request_id);
                            tr_asset_registration assetreg = db.tr_asset_registration.Find(disposalrequest.asset_id);

                            if (assetreg.location_id != 1)
                            {
                                var refApproval2 = (from a in db.ms_job_level
                                                    where (a.fl_active == true && a.deleted_date == null
                                                            && a.job_level_id == 3)

                                                    join b in db.ms_employee_detail on a.job_level_id equals b.job_level_id
                                                    where (b.fl_active == true && b.deleted_date == null
                                                            && b.department_id == disposalrequest.request_dept_id && b.company_id == assetreg.company_id)

                                                    join c in db.ms_employee on b.employee_id equals c.employee_id
                                                    where c.fl_active == true && c.deleted_date == null

                                                    select new disposalViewModel()
                                                    {
                                                        department_id = b.department_id,
                                                        employee_id = b.employee_id,
                                                        job_level_id = a.job_level_id,
                                                        employee_email = c.employee_email,
                                                        employee_name = c.employee_name
                                                    }).First<disposalViewModel>();

                                disposal_approval = new tr_disposal_approval();
                                disposal_approval.request_id = disposal_prs.request_id;
                                disposal_approval.approval_date = null;
                                disposal_approval.approval_dept_id = refApproval2.department_id;
                                disposal_approval.approval_employee_id = refApproval2.employee_id;
                                disposal_approval.approval_level_id = refApproval2.job_level_id;
                                disposal_approval.approval_status_id = 1;//waiting approval
                                disposal_approval.approval_suggestion_id = disposal_prs.approval_suggestion_id;
                                disposal_approval.fl_active = true;
                                disposal_approval.created_date = DateTime.Now;
                                disposal_approval.created_by = UserProfile.UserId;
                                disposal_approval.updated_date = DateTime.Now;
                                disposal_approval.updated_by = UserProfile.UserId;
                                disposal_approval.deleted_date = null;
                                disposal_approval.deteled_by = null;
                                disposal_approval.org_id = UserProfile.OrgId;
                                disposal_approval = db.tr_disposal_approval.Add(disposal_approval);
                                db.SaveChanges();
                            }
                            #endregion

                            #region "Kirim email ke Procurement Head"
                            sy_email_log sy_email_log = new sy_email_log();
                            sy_email_log.elog_to = refApproval.employee_email;
                            sy_email_log.elog_subject = string.Format("Asset Disposal Changes Need Approval");
                            sy_email_log.elog_template = "EMAIL_TEMPLATE_04";

                            var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_04"));
                            string strBodyMail = _bodymail.FirstOrDefault().app_value;
                            strBodyMail = strBodyMail.Replace("[to]", refApproval.employee_name);
                            strBodyMail = strBodyMail.Replace("[action]", "Disposal Changes");
                            strBodyMail = strBodyMail.Replace("[assetnumber]", disposal_prs.asset_number);
                            strBodyMail = strBodyMail.Replace("[aseetname]", disposal_prs.asset_name);
                            strBodyMail = strBodyMail.Replace("[assetlocation]", disposal_prs.location_name);
                            strBodyMail = strBodyMail.Replace("[department]", disposal_prs.department_name);

                            int empid = Convert.ToInt32(refApproval.employee_id);
                            ms_user msuser = (from m in db.ms_user
                                              where m.employee_id == empid
                                              select m).FirstOrDefault();

                            //token untuk link approval di email
                            string token = string.Format("DisposalApproval|Approval/{0}|{1}|{2}|{3}", approval_id, msuser.user_name, UserProfile.company_id, UserProfile.asset_reg_location_id);
                            token = CryptorHelper.Encrypt(token, "MD5", true).Replace("+", "plus").Replace("=", "equal");
                            string linkapp = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~/Account/Login?ReturnUrl=%2f&token=" + token));
                            string strImg = string.Format("http://{0}/Content/EmailImage/button_approval_disposal.png", Request.Url.Authority);
                            linkapp = string.Format(@"<a href={0}><img src=""{1}"" alt=""click for approval""/></a>", linkapp, strImg);
                            strBodyMail = strBodyMail.Replace("[link]", linkapp);

                            sy_email_log.elog_body = strBodyMail;

                            var EmailHelper = new EmailHelper()
                            {
                                ToAddress = sy_email_log.elog_to,
                                Email_Template = sy_email_log.elog_template,
                                MailSubject = sy_email_log.elog_subject,
                                MailBody = sy_email_log.elog_body
                            };
                            EmailHelper.Send();
                            #endregion
                        }
                        transaction.Commit();
                        ViewBag.ResultMessage = "Update table successfully.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        // roll back all database operations, if any thing goes wrong
                        transaction.Rollback();
                        ViewBag.ResultMessage = "Error occured, records rolledback.";
                    }
                }
            }

            disposal_prs = DataDisposalView(disposal_prs.request_id, disposal_prs);

            return View(disposal_prs);
        }
        private disposalViewModel DataDisposalView(int? id, disposalViewModel disposalmodel)
        {
            var _qry = (from dp in db.tr_disposal_announcement
                        where dp.deleted_date == null && dp.announcement_id == id

                        join t in db.tr_disposal_request on dp.request_id equals t.request_id
                        where t.fl_active == true && t.deleted_date == null

                        join a in db.tr_depreciation on t.asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_currency on a.asset_original_currency_id equals b.currency_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_asmin_company on t.org_id equals c.company_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_department on t.request_dept_id equals d.department_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_employee on t.request_emp_id equals e.employee_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_asset_location on t.request_location_id equals f.location_id
                        where (f.fl_active == true && f.deleted_date == null)

                        join g in db.tr_asset_registration on t.asset_id equals g.asset_id
                        //where (g.fl_active == true && g.deleted_date == null)

                        join h in db.tr_disposal_image on t.request_id equals h.request_id

                        select new disposalViewModel()
                        {
                            tr_disposal_announcement = dp,
                            tr_disposal_request = t,
                            asset_parent = g,
                            tr_disposal_image = h,
                            ms_currency = b,
                            tr_depreciation = a,
                            location_name = f.location_name,
                            department_name = d.department_name,
                            employee_name = e.employee_name
                        }).First<disposalViewModel>();

            disposalmodel.FormMode = EnumFormModeKey.Form_Edit;
            disposalmodel.announcement_id = _qry.tr_disposal_announcement.announcement_id;
            disposalmodel.request_id = _qry.tr_disposal_request.request_id;
            disposalmodel.disposal_number = _qry.tr_disposal_request.disposal_number;
            disposalmodel.asset_id = _qry.tr_disposal_request.asset_id;
            disposalmodel.asset_number = _qry.asset_parent.asset_number;
            disposalmodel.asset_name = _qry.asset_parent.asset_name;
            disposalmodel.asset_receipt_date = _qry.asset_parent.asset_receipt_date;
            disposalmodel.asset_img_address = _qry.tr_disposal_image.asset_img_address;
            disposalmodel.currency_code = _qry.ms_currency.currency_code;
            disposalmodel.asset_book_value = _qry.tr_depreciation.asset_book_value;
            disposalmodel.Currency_kurs = _qry.tr_depreciation.usd_kurs;
            disposalmodel.asset_original_value = _qry.tr_depreciation.asset_original_value;
            disposalmodel.location_id = _qry.tr_disposal_request.request_location_id;
            disposalmodel.location_name = _qry.location_name;
            disposalmodel.department_name = _qry.department_name;
            disposalmodel.employee_name = _qry.employee_name;
            disposalmodel.request_description = _qry.tr_disposal_request.request_description;
            disposalmodel.fl_announcement_status = _qry.tr_disposal_announcement.fl_announcement_status;
            disposalmodel.announcement_upload_address = _qry.tr_disposal_announcement.announcement_upload_address;
            disposalmodel.announcement_description = _qry.tr_disposal_announcement.announcement_description;
            disposalmodel.fin_announcement_upload_address = _qry.tr_disposal_announcement.fin_announcement_upload_address;
            disposalmodel.path_address = asset_registrationViewModel.path_file_disposal;

            //Data Approval view
            disposalmodel.disposal_Approval_list = (from da in db.tr_disposal_approval
                                                    where (da.fl_active == true && da.deleted_date == null && da.request_id == disposalmodel.request_id)

                                                    join a in db.ms_employee on da.approval_employee_id equals a.employee_id
                                                    where (a.fl_active == true && a.deleted_date == null)

                                                    join b in db.ms_job_level on da.approval_level_id equals b.job_level_id
                                                    where (b.fl_active == true && b.deleted_date == null)

                                                    join c in db.ms_request_status on da.approval_status_id equals c.request_status_id

                                                    join d in db.ms_disposal_type on da.approval_suggestion_id equals d.disposal_type_id
                                                    into d_temp
                                                    from subd in d_temp.DefaultIfEmpty()

                                                    select new disposalappViewModel()
                                                    {
                                                        approval_employee_name = a.employee_name,
                                                        approval_level_name = b.job_level_name,
                                                        approval_status_Name = c.request_status_name,
                                                        approval_date = da.approval_date,
                                                        approval_suggestion_id = da.approval_suggestion_id,
                                                        approval_suggestion_name = d_temp.FirstOrDefault().disposal_type_name
                                                    }).ToList<disposalappViewModel>();
            //Data Sugesstion Changes
            disposalmodel.disposal_type_list = (from dt in db.ms_disposal_type
                                                where dt.disposal_type_id != 2
                                                select dt).ToList();
            return disposalmodel;
        }
    }
}