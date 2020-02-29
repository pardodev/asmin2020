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
    public class disposalRemoveController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

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
                        where dp.fl_active == true && dp.deleted_date == null 
                        && dp.remove_asset_dept_id == UserProfile.department_id
                        orderby dp.announcement_id descending

                        join dr in db.tr_disposal_request on dp.request_id equals dr.request_id
                        where (dr.fl_active == true && dr.deleted_date == null)

                        join md in db.ms_disposal_type on dp.approval_disposal_type_id equals md.disposal_type_id

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
                            fl_fin_announcement = dp.fl_fin_announcement,
                            fin_announcement_date = dp.fin_announcement_date,
                            fin_announcement_dept_id = dp.fin_announcement_dept_id,

                            fl_remove_asset = dp.fl_remove_asset,
                            remove_asset_date = dp.remove_asset_date,

                            asset_id = dr.asset_id,
                            asset_number = a.asset_number,
                            asset_name = a.asset_name,

                            approval_suggestion_name = md.disposal_type_name,

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

            var _qry = (from dp in db.tr_disposal_announcement
                        where dp.fl_active == true && dp.deleted_date == null && dp.announcement_id == id

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

            disposalViewModel disposalprocess = new disposalViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,
                announcement_id = _qry.tr_disposal_announcement.announcement_id,
                request_id = _qry.tr_disposal_request.request_id,
                disposal_number = _qry.tr_disposal_request.disposal_number,
                asset_id = _qry.tr_disposal_request.asset_id,
                asset_number = _qry.asset_parent.asset_number,
                asset_name = _qry.asset_parent.asset_name,
                asset_receipt_date = _qry.asset_parent.asset_receipt_date,
                asset_img_address = _qry.tr_disposal_image.asset_img_address,
                currency_code = _qry.ms_currency.currency_code,
                asset_book_value = _qry.tr_depreciation.asset_book_value,
                Currency_kurs = _qry.tr_depreciation.usd_kurs,
                asset_original_value = _qry.tr_depreciation.asset_original_value,
                location_id = _qry.tr_disposal_request.request_location_id,
                location_name = _qry.location_name,
                department_name = _qry.department_name,
                employee_name = _qry.employee_name,
                request_description = _qry.tr_disposal_request.request_description,
                fl_announcement_status = _qry.tr_disposal_announcement.fl_announcement_status,
                announcement_upload_address = _qry.tr_disposal_announcement.announcement_upload_address,
                fl_fin_announcement = _qry.tr_disposal_announcement.fl_fin_announcement,
                fin_announcement_upload_address = _qry.tr_disposal_announcement.fin_announcement_upload_address,
                fl_remove_asset = _qry.tr_disposal_announcement.fl_remove_asset,
                remove_asset_date = _qry.tr_disposal_announcement.remove_asset_date,
                remove_asset_description = _qry.tr_disposal_announcement.remove_asset_description,
                path_address = asset_registrationViewModel.path_file_disposal
            };

            //Data Approval view
            disposalprocess.disposal_Approval_list = (from da in db.tr_disposal_approval
                                                      where (da.fl_active == true && da.deleted_date == null && da.request_id == disposal_req.request_id)

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

            var _qry = (from dp in db.tr_disposal_announcement
                        where dp.fl_active == true && dp.deleted_date == null && dp.announcement_id == id

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

            disposalViewModel disposalprocess = new disposalViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,
                announcement_id = _qry.tr_disposal_announcement.announcement_id,
                request_id = _qry.tr_disposal_request.request_id,
                disposal_number = _qry.tr_disposal_request.disposal_number,
                asset_id = _qry.tr_disposal_request.asset_id,
                asset_number = _qry.asset_parent.asset_number,
                asset_name = _qry.asset_parent.asset_name,
                asset_receipt_date = _qry.asset_parent.asset_receipt_date,
                asset_img_address = _qry.tr_disposal_image.asset_img_address,
                currency_code = _qry.ms_currency.currency_code,
                asset_book_value = _qry.tr_depreciation.asset_book_value,
                Currency_kurs = _qry.tr_depreciation.usd_kurs,
                asset_original_value = _qry.tr_depreciation.asset_original_value,
                location_id = _qry.tr_disposal_request.request_location_id,
                location_name = _qry.location_name,
                department_name = _qry.department_name,
                employee_name = _qry.employee_name,
                request_description = _qry.tr_disposal_request.request_description,
                fl_announcement_status = _qry.tr_disposal_announcement.fl_announcement_status,
                announcement_upload_address = _qry.tr_disposal_announcement.announcement_upload_address,
                fl_fin_announcement = _qry.tr_disposal_announcement.fl_fin_announcement,
                fin_announcement_upload_address = _qry.tr_disposal_announcement.fin_announcement_upload_address,
                fl_remove_asset = _qry.tr_disposal_announcement.fl_remove_asset,
                remove_asset_date = _qry.tr_disposal_announcement.remove_asset_date,
                remove_asset_description = _qry.tr_disposal_announcement.remove_asset_description,
                path_address = asset_registrationViewModel.path_file_disposal
            };

            //Data Approval view
            disposalprocess.disposal_Approval_list = (from da in db.tr_disposal_approval
                                                      where (da.fl_active == true && da.deleted_date == null && da.request_id == disposal_req.request_id)

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

            return View(disposalprocess);
        }

        // POST: disposal/Approval
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisposalFollowup([Bind(Include = "announcement_id, request_id, remove_asset_description, asset_id, asset_number, asset_name, location_name, department_name")] disposalViewModel disposal_prs)
        {
            //update disposal request and approval data with transaction
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region "Save update Disposal Announcement"
                        tr_disposal_announcement disposal_announcement = db.tr_disposal_announcement.Find(disposal_prs.announcement_id);

                        disposal_announcement.remove_asset_description = disposal_prs.remove_asset_description;
                        disposal_announcement.remove_asset_date = DateTime.Now;
                        disposal_announcement.remove_asset_emp_id = UserProfile.employee_id;

                        disposal_announcement.updated_date = DateTime.Now;
                        disposal_announcement.updated_by = UserProfile.UserId;
                        disposal_announcement.deleted_date = null;
                        disposal_announcement.deleted_by = null;

                        db.Entry(disposal_announcement).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion

                        tr_asset_registration asset_registration = db.tr_asset_registration.Find(disposal_prs.asset_id);
                        asset_registration.fl_active = false;
                        db.Entry(asset_registration).State = EntityState.Modified;
                        db.SaveChanges();

                        #region "kirim email ke All PIC"
                        string emailsetting = string.Empty;
                        string emailsettingcc = string.Empty;

                        var _emailapproval = (from app in db.tr_disposal_approval
                                              where app.request_id == disposal_prs.request_id && app.fl_active == true && app.deleted_date == null

                                              join a in db.ms_employee on app.approval_employee_id equals a.employee_id
                                              where a.fl_active == true && a.deleted_date == null

                                              select a).ToList<ms_employee>();

                        foreach (ms_employee employee_app in _emailapproval)
                        {
                            emailsettingcc += employee_app.employee_email + ";";
                        }

                        var _emailowner_asset = (from app in db.tr_asset_registration
                                                 where app.asset_id == disposal_prs.asset_id /*&& app.fl_active == true && app.deleted_date == null*/

                                                 join a in db.ms_employee on app.employee_id equals a.employee_id
                                                 where a.fl_active == true && a.deleted_date == null

                                                 select a).ToList<ms_employee>();
                        foreach (ms_employee employee_app in _emailowner_asset)
                        {
                            emailsetting += employee_app.employee_email + ";";
                        }

                        var _emailpic_asset = (from app in db.tr_disposal_request
                                               where app.request_id == disposal_prs.request_id && app.fl_active == true && app.deleted_date == null

                                               join a in db.ms_employee on app.request_emp_id equals a.employee_id
                                               where a.fl_active == true && a.deleted_date == null

                                               select a).ToList<ms_employee>();
                        foreach (ms_employee employee_app in _emailpic_asset)
                        {
                            emailsetting += employee_app.employee_email + ";";
                        }

                        var _email_announcement = (from app in db.tr_disposal_announcement
                                                   where app.announcement_id == disposal_prs.announcement_id && app.fl_active == true && app.deleted_date == null

                                                   join a in db.ms_employee on app.announcement_emp_id equals a.employee_id
                                                   where a.fl_active == true && a.deleted_date == null

                                                   select a).ToList<ms_employee>();
                        foreach (ms_employee employee_app in _email_announcement)
                        {
                            emailsetting += employee_app.employee_email + ";";
                        }

                        var _email_fin_announcement = (from app in db.tr_disposal_announcement
                                                       where app.announcement_id == disposal_prs.announcement_id && app.fl_active == true && app.deleted_date == null

                                                       join a in db.ms_employee on app.fin_announcement_emp_id equals a.employee_id
                                                       where a.fl_active == true && a.deleted_date == null

                                                       select a).ToList<ms_employee>();
                        foreach (ms_employee employee_app in _email_fin_announcement)
                        {
                            emailsetting += employee_app.employee_email + ";";
                        }

                        sy_email_log sy_email_log = new sy_email_log();
                        sy_email_log.elog_to = emailsetting;
                        sy_email_log.elog_cc = emailsettingcc;
                        sy_email_log.elog_subject = string.Format("Disposal Asset");
                        sy_email_log.elog_template = "EMAIL_TEMPLATE_06";

                        #region "body mail"
                        var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_06"));
                        string strBodyMail = _bodymail.FirstOrDefault().app_value;
                        strBodyMail = strBodyMail.Replace("[to]", "");
                        strBodyMail = strBodyMail.Replace("[assetnumber]", disposal_prs.asset_number);
                        strBodyMail = strBodyMail.Replace("[aseetname]", disposal_prs.asset_name);
                        strBodyMail = strBodyMail.Replace("[assetlocation]", disposal_prs.location_name);
                        strBodyMail = strBodyMail.Replace("[department]", disposal_prs.department_name);

                        string linkapp = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~/Account/Login?ReturnUrl=%2f"));
                        string strImg = string.Format("http://{0}/Content/EmailImage/button_asmin.png", Request.Url.Authority);
                        linkapp = string.Format(@"<a href={0}><img src=""{1}"" alt=""click for process""/></a>", linkapp, strImg);
                        strBodyMail = strBodyMail.Replace("[link]", linkapp);

                        ms_disposal_type suggestion = db.ms_disposal_type.Find(disposal_announcement.approval_disposal_type_id);
                        strBodyMail = strBodyMail.Replace("[suggestion]", suggestion.disposal_type_name);
                        sy_email_log.elog_body = strBodyMail;
                        #endregion

                        var EmailHelper = new EmailHelper()
                        {
                            ToAddress = sy_email_log.elog_to,
                            CcAddress = sy_email_log.elog_cc,
                            Email_Template = sy_email_log.elog_template,
                            MailSubject = sy_email_log.elog_subject,
                            MailBody = sy_email_log.elog_body
                        };
                        EmailHelper.Send();
                        #endregion

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
            return View(disposal_prs);
        }
    }
}