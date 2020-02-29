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
    public class disposalapprovalController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: disposalapproval
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
            if (UserProfile.job_level_id != 9)
            {
                var _qry = (from dr in db.tr_disposal_request
                            where (dr.fl_active == true && dr.deleted_date == null)
                                   && dr.org_id == UserProfile.OrgId
                                   //&& dr.request_dept_id == UserProfile.department_id
                                   && dr.request_location_id == UserProfile.asset_reg_location_id
                            orderby dr.request_id descending

                            //mengambil job level terakhir yg belum di approve
                            join appnow in (from appnow in db.tr_disposal_approval
                                            where appnow.approval_date == null
                                            && appnow.fl_active == true && appnow.deleted_date == null

                                            orderby appnow.approval_id ascending
                                            group appnow by appnow.request_id into appnowsort
                                            select appnowsort.FirstOrDefault())
                                        on dr.request_id equals appnow.request_id

                            //mengambil approval user login
                            join app in (from app in db.tr_disposal_approval
                                         where app.fl_active == true && app.deleted_date == null
                                           && app.approval_employee_id == UserProfile.employee_id
                                           && app.approval_status_id == 1
                                         orderby app.approval_id ascending
                                         group app by app.request_id into appsort
                                         select appsort.FirstOrDefault())
                                        on appnow.request_id equals app.request_id
                            where (appnow.approval_employee_id == app.approval_employee_id)

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

                            join f in db.ms_request_status on app.approval_status_id equals f.request_status_id

                            join g in db.tr_disposal_announcement on dr.request_id equals g.request_id
                                   into leftg
                            from lftjoing in leftg.DefaultIfEmpty()

                            join h in db.ms_disposal_type on app.approval_suggestion_id equals h.disposal_type_id
                                    into lefth
                            from lftjoinh in lefth.DefaultIfEmpty()

                            join i in db.ms_request_status on app.approval_status_id equals i.request_status_id
                             into lefti
                            from lftjoini in lefti.DefaultIfEmpty()

                            join j in db.ms_job_level on app.approval_level_id equals j.job_level_id
                             into leftj
                            from lftjoinj in leftj.DefaultIfEmpty()

                            select new disposalViewModel()
                            {
                                asset_id = dr.asset_id,
                                asset_number = a.asset_number,
                                asset_name = a.asset_name,
                                disposal_number = dr.disposal_number,
                                request_id = dr.request_id,
                                request_date = dr.request_date,
                                request_status_id = f.request_status_id,
                                request_status_name = f.request_status_name,
                                fl_approval = dr.fl_approval,
                                approval_date = dr.approval_date,

                                company = b,
                                department_name = c.department_code,
                                employee_name = d.employee_name,
                                location_name = e.asset_reg_location_name,

                                fl_SuggestionChanges = lftjoing.fl_suggestion_changes,
                                fl_announcement_status = lftjoing.fl_announcement_status,
                                fl_fin_announcement = lftjoing.fl_fin_announcement,
                                fl_remove_asset = lftjoing.fl_remove_asset,

                                approval_id = app.approval_id,
                                approval_status_id = app.approval_status_id,
                                approval_status_Name = lftjoini.request_status_name,
                                approval_suggestion_name = lftjoinh.disposal_type_name,
                                approval_level_name = lftjoinj.job_level_name
                            }).ToList<disposalViewModel>();

                //Non approval
                var _qry2 = (from dr in db.tr_disposal_request
                             where (dr.fl_active == true && dr.deleted_date == null)
                                    && dr.org_id == UserProfile.OrgId
                                    //&& dr.request_dept_id == UserProfile.department_id
                                    && dr.request_location_id == UserProfile.asset_reg_location_id
                             orderby dr.request_id descending

                             //mengambil approval user login
                             join app in (from app in db.tr_disposal_approval
                                          where app.fl_active == true && app.deleted_date == null
                                            && app.approval_employee_id == UserProfile.employee_id && app.approval_status_id > 1
                                          orderby app.approval_id descending
                                          group app by app.request_id into appsort
                                          select appsort.FirstOrDefault())
                                         on dr.request_id equals app.request_id

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

                             join f in db.ms_request_status on app.approval_status_id equals f.request_status_id

                             join g in db.tr_disposal_announcement.Where(g => g.fl_active == true) on dr.request_id equals g.request_id
                                    into leftg
                             from lftjoing in leftg.DefaultIfEmpty()

                             join h in db.ms_disposal_type on app.approval_suggestion_id equals h.disposal_type_id
                                     into lefth
                             from lftjoinh in lefth.DefaultIfEmpty()

                             join i in db.ms_request_status on app.approval_status_id equals i.request_status_id
                              into lefti
                             from lftjoini in lefti.DefaultIfEmpty()

                             join j in db.ms_job_level on app.approval_level_id equals j.job_level_id
                              into leftj
                             from lftjoinj in leftj.DefaultIfEmpty()

                             select new disposalViewModel()
                             {
                                 asset_id = dr.asset_id,
                                 asset_number = a.asset_number,
                                 asset_name = a.asset_name,
                                 disposal_number = dr.disposal_number,
                                 request_id = dr.request_id,
                                 request_date = dr.request_date,
                                 request_status_id = f.request_status_id,
                                 request_status_name = f.request_status_name,
                                 fl_approval = dr.fl_approval,
                                 approval_date = dr.approval_date,

                                 company = b,
                                 department_name = c.department_code,
                                 employee_name = d.employee_name,
                                 location_name = e.asset_reg_location_name,

                                 fl_SuggestionChanges = lftjoing.fl_suggestion_changes,
                                 fl_announcement_status = lftjoing.fl_announcement_status,
                                 fl_fin_announcement = lftjoing.fl_fin_announcement,
                                 fl_remove_asset = lftjoing.fl_remove_asset,

                                 approval_id = app.approval_id,
                                 approval_status_id = app.approval_status_id,
                                 approval_status_Name = lftjoini.request_status_name,
                                 approval_suggestion_name = lftjoinh.disposal_type_name,
                                 approval_level_name = lftjoinj.job_level_name
                             }).ToList<disposalViewModel>();

                _qry.AddRange(_qry2);
                return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var _qry = (from dr in db.tr_disposal_request
                            where (dr.fl_active == true && dr.deleted_date == null)
                                   && dr.org_id == UserProfile.OrgId
                                   //&& dr.request_dept_id == UserProfile.department_id
                                   //&& dr.request_location_id == UserProfile.asset_reg_location_id
                            orderby dr.request_id descending

                            //mengambil job level terakhir yg belum di approve
                            join appnow in (from appnow in db.tr_disposal_approval
                                            where appnow.approval_date == null
                                            && appnow.fl_active == true && appnow.deleted_date == null

                                            orderby appnow.approval_id ascending
                                            group appnow by appnow.request_id into appnowsort
                                            select appnowsort.FirstOrDefault())
                                        on dr.request_id equals appnow.request_id

                            //mengambil approval user login
                            join app in (from app in db.tr_disposal_approval
                                         where app.fl_active == true && app.deleted_date == null
                                           && app.approval_employee_id == UserProfile.employee_id
                                           && app.approval_status_id == 1
                                         orderby app.approval_id ascending
                                         group app by app.request_id into appsort
                                         select appsort.FirstOrDefault())
                                        on appnow.request_id equals app.request_id
                            where (appnow.approval_employee_id == app.approval_employee_id)

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

                            join f in db.ms_request_status on app.approval_status_id equals f.request_status_id

                            join g in db.tr_disposal_announcement on dr.request_id equals g.request_id
                                   into leftg
                            from lftjoing in leftg.DefaultIfEmpty()

                            join h in db.ms_disposal_type on app.approval_suggestion_id equals h.disposal_type_id
                                    into lefth
                            from lftjoinh in lefth.DefaultIfEmpty()

                            join i in db.ms_request_status on app.approval_status_id equals i.request_status_id
                             into lefti
                            from lftjoini in lefti.DefaultIfEmpty()

                            join j in db.ms_job_level on app.approval_level_id equals j.job_level_id
                             into leftj
                            from lftjoinj in leftj.DefaultIfEmpty()

                            select new disposalViewModel()
                            {
                                asset_id = dr.asset_id,
                                asset_number = a.asset_number,
                                asset_name = a.asset_name,
                                disposal_number = dr.disposal_number,
                                request_id = dr.request_id,
                                request_date = dr.request_date,
                                request_status_id = f.request_status_id,
                                request_status_name = f.request_status_name,
                                fl_approval = dr.fl_approval,
                                approval_date = dr.approval_date,

                                company = b,
                                department_name = c.department_code,
                                employee_name = d.employee_name,
                                location_name = e.asset_reg_location_name,

                                fl_SuggestionChanges = lftjoing.fl_suggestion_changes,
                                fl_announcement_status = lftjoing.fl_announcement_status,
                                fl_fin_announcement = lftjoing.fl_fin_announcement,
                                fl_remove_asset = lftjoing.fl_remove_asset,

                                approval_id = app.approval_id,
                                approval_status_id = app.approval_status_id,
                                approval_status_Name = lftjoini.request_status_name,
                                approval_suggestion_name = lftjoinh.disposal_type_name,
                                approval_level_name = lftjoinj.job_level_name
                            }).ToList<disposalViewModel>();

                //Non approval
                var _qry2 = (from dr in db.tr_disposal_request
                             where (dr.fl_active == true && dr.deleted_date == null)
                                    && dr.org_id == UserProfile.OrgId
                                    //&& dr.request_dept_id == UserProfile.department_id
                                    //&& dr.request_location_id == UserProfile.asset_reg_location_id
                             orderby dr.request_id descending

                             //mengambil approval user login
                             join app in (from app in db.tr_disposal_approval
                                          where app.fl_active == true && app.deleted_date == null
                                            && app.approval_employee_id == UserProfile.employee_id && app.approval_status_id > 1
                                          orderby app.approval_id descending
                                          group app by app.request_id into appsort
                                          select appsort.FirstOrDefault())
                                         on dr.request_id equals app.request_id

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

                             join f in db.ms_request_status on app.approval_status_id equals f.request_status_id

                             join g in db.tr_disposal_announcement.Where(g => g.fl_active == true) on dr.request_id equals g.request_id
                                    into leftg
                             from lftjoing in leftg.DefaultIfEmpty()

                             join h in db.ms_disposal_type on app.approval_suggestion_id equals h.disposal_type_id
                                     into lefth
                             from lftjoinh in lefth.DefaultIfEmpty()

                             join i in db.ms_request_status on app.approval_status_id equals i.request_status_id
                              into lefti
                             from lftjoini in lefti.DefaultIfEmpty()

                             join j in db.ms_job_level on app.approval_level_id equals j.job_level_id
                              into leftj
                             from lftjoinj in leftj.DefaultIfEmpty()

                             select new disposalViewModel()
                             {
                                 asset_id = dr.asset_id,
                                 asset_number = a.asset_number,
                                 asset_name = a.asset_name,
                                 disposal_number = dr.disposal_number,
                                 request_id = dr.request_id,
                                 request_date = dr.request_date,
                                 request_status_id = f.request_status_id,
                                 request_status_name = f.request_status_name,
                                 fl_approval = dr.fl_approval,
                                 approval_date = dr.approval_date,

                                 company = b,
                                 department_name = c.department_code,
                                 employee_name = d.employee_name,
                                 location_name = e.asset_reg_location_name,

                                 fl_SuggestionChanges = lftjoing.fl_suggestion_changes,
                                 fl_announcement_status = lftjoing.fl_announcement_status,
                                 fl_fin_announcement = lftjoing.fl_fin_announcement,
                                 fl_remove_asset = lftjoing.fl_remove_asset,

                                 approval_id = app.approval_id,
                                 approval_status_id = app.approval_status_id,
                                 approval_status_Name = lftjoini.request_status_name,
                                 approval_suggestion_name = lftjoinh.disposal_type_name,
                                 approval_level_name = lftjoinj.job_level_name
                             }).ToList<disposalViewModel>();

                _qry.AddRange(_qry2);
                return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
            }
            //approval utama
            

            
        }

        // GET: DisposalApproval/details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_disposal_approval disposal_app = db.tr_disposal_approval.Find(id);
            if (disposal_app == null)
            {
                return HttpNotFound("Assset not found.");
            }
            disposalViewModel disposalmodel = new disposalViewModel();
            disposalmodel = DataDisposalView(id, disposalmodel);

            return View(disposalmodel);
        }

        // GET: Disposal/approval/5
        public ActionResult approval(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_disposal_approval disposal_app = db.tr_disposal_approval.Find(id);
            if (disposal_app == null)
            {
                return HttpNotFound("Assset not found.");
            }
            disposalViewModel disposalmodel = new disposalViewModel();
            disposalmodel = DataDisposalView(id, disposalmodel);

            return View(disposalmodel);
        }

        // POST: disposal/Approval
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approval([Bind(Include = "request_id, approval_id, fl_approval, approval_suggestion_id, approval_noted, asset_number,asset_name, location_name, department_name, fl_SuggestionChanges")] disposalViewModel disposal_req)
        {
            if (disposal_req.fl_approval != null)
            {
                if (disposal_req.fl_approval == true)
                {
                    if (disposal_req.approval_suggestion_id == null || disposal_req.approval_suggestion_id == 0)
                        ModelState.AddModelError("approval_suggestion_id", "Suggestion is Mandatory.");
                }
            }
            else if (disposal_req.approval_noted == null || disposal_req.approval_noted.Trim() == string.Empty)
            {
                ModelState.AddModelError("fl_approval", "Approval is Mandatory.");
                ModelState.AddModelError("approval_noted", "Reject Reason is Mandatory.");
            }

            //update disposal request and approval data with transaction
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool iscomplete = false;
                        #region "Save update Request Disposal"
                        tr_disposal_request disposal_request = db.tr_disposal_request.Find(disposal_req.request_id);
                        var doneapp = (from app in db.tr_disposal_approval
                                       where (app.approval_date == null && app.fl_active == true
                                                && app.deleted_date == null && app.request_id == disposal_req.request_id)
                                       select app).ToList();

                        if (disposal_req.fl_SuggestionChanges == false)
                        {
                            disposal_request.fl_approval = disposal_req.fl_approval;
                            if (disposal_req.fl_approval == true)
                            {
                                if (doneapp.Count == 1)
                                {
                                    disposal_request.request_status_id = 5;//complete
                                    iscomplete = true;
                                }
                                else
                                    disposal_request.request_status_id = 2;//approve
                            }
                            else
                            {
                                disposal_request.request_status_id = 3; //reject
                            }

                            disposal_request.approval_date = DateTime.Now;
                            disposal_request.updated_date = DateTime.Now;
                            disposal_request.update_by = UserProfile.UserId;
                            disposal_request.deleted_date = null;
                            disposal_request.deleted_by = null;

                            db.Entry(disposal_request).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        #endregion

                        #region "Save update Approval Disposal"
                        tr_disposal_approval disposal_approval = db.tr_disposal_approval.Find(disposal_req.approval_id);

                        if (disposal_req.fl_approval == true)
                        {
                            disposal_approval.approval_status_id = 2;//approve
                            disposal_approval.approval_suggestion_id = disposal_req.approval_suggestion_id;
                        }
                        else
                        {
                            disposal_approval.approval_status_id = 3; //reject
                            disposal_approval.approval_noted = disposal_req.approval_noted;
                            disposal_approval.fl_active = false;

                            //kembalikan status announcement ke suggestion terakhir
                            tr_disposal_announcement disposal_ann_old = (from tda in db.tr_disposal_announcement
                                                                         where tda.request_id == disposal_approval.request_id && tda.fl_suggestion_changes == true
                                                                         select tda).FirstOrDefault();
                            disposal_ann_old.fl_suggestion_changes = false;
                            disposal_ann_old.fl_active = true;
                            disposal_ann_old.updated_date = DateTime.Now;
                            disposal_ann_old.updated_by = UserProfile.UserId;
                            db.Entry(disposal_ann_old).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        disposal_approval.approval_date = DateTime.Now;
                        disposal_approval.approval_location_id = UserProfile.asset_reg_location_id;
                        disposal_approval.updated_date = DateTime.Now;
                        disposal_approval.updated_by = UserProfile.UserId;
                        disposal_approval.deleted_date = null;
                        disposal_approval.deteled_by = null;

                        db.Entry(disposal_approval).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion

                        if (disposal_req.fl_approval == true)
                        {
                            //cek untuk disposal changes: site atau ho
                            tr_asset_registration assetreg = db.tr_asset_registration.Find(disposal_request.asset_id);

                            if (doneapp.Count == 1)
                                iscomplete = true;

                            if (!iscomplete && ((assetreg.location_id != 1 && disposal_req.fl_SuggestionChanges == true) || (disposal_req.fl_SuggestionChanges == false)))
                            {
                                #region "kirim email ke approval"

                                var next_approval = (from app in db.tr_disposal_approval
                                                     where (app.approval_date == null && app.fl_active == true && app.deleted_date == null)
                                                            && app.request_id == disposal_req.request_id
                                                     orderby app.approval_id ascending

                                                     join a in db.ms_employee on app.approval_employee_id equals a.employee_id
                                                     where a.fl_active == true && a.deleted_date == null

                                                     join b in db.ms_user on a.employee_id equals b.employee_id
                                                     where b.fl_active == true && b.deleted_date == null

                                                     select new disposalViewModel
                                                     {
                                                         approval_id = app.approval_id,
                                                         ms_employee = a, 
                                                         ms_user = b
                                                     }).FirstOrDefault<disposalViewModel>();

                                if (next_approval != null)
                                {
                                    sy_email_log sy_email_log = new sy_email_log();
                                    sy_email_log.elog_to = next_approval.ms_employee.employee_email;
                                    sy_email_log.elog_subject = string.Format("Asset Disposal Need Approval");
                                    sy_email_log.elog_template = "EMAIL_TEMPLATE_04";

                                    #region "body mail"
                                    var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_04"));
                                    string strBodyMail = _bodymail.FirstOrDefault().app_value;
                                    strBodyMail = strBodyMail.Replace("[to]", next_approval.ms_employee.employee_name);
                                    strBodyMail = strBodyMail.Replace("[action]", "Dispose");
                                    strBodyMail = strBodyMail.Replace("[assetnumber]", disposal_req.asset_number);
                                    strBodyMail = strBodyMail.Replace("[aseetname]", disposal_req.asset_name);
                                    strBodyMail = strBodyMail.Replace("[assetlocation]", disposal_req.location_name);
                                    strBodyMail = strBodyMail.Replace("[department]", disposal_req.department_name);

                                    //token untuk link approval di email
                                    string token = string.Format("DisposalApproval|Approval/{0}|{1}|{2}|{3}", next_approval.approval_id, next_approval.ms_user.user_name, UserProfile.company_id, UserProfile.asset_reg_location_id);
                                    token = CryptorHelper.Encrypt(token, "MD5", true).Replace("+", "plus").Replace("=", "equal");
                                    string linkapp = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~/Account/Login?ReturnUrl=%2f&token=" + token));
                                    string strImg = string.Format("http://{0}/Content/EmailImage/button_approval_disposal.png", Request.Url.Authority);

                                    linkapp = string.Format(@"<a href={0}><img src=""{1}"" alt=""click for approval""/></a>", linkapp, strImg);

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
                                }
                                #endregion

                                #region "Save Sy_Message_notification ke approval"

                                sy_message_notification msg = new sy_message_notification();
                                msg.notif_group = "BALOON_RECEIPT_04";
                                msg.notify_user = next_approval.ms_user.user_name;
                                msg.notify_ip = next_approval.ms_employee.ip_address;
                                msg.notify_message = "Ada permintaan approval untuk Asset disposal.";
                                msg.fl_active = true;
                                msg.created_date = DateTime.Now;
                                msg.created_by = UserProfile.UserId;
                                msg.fl_shown = 0;

                                db.sy_message_notification.Add(msg);
                                db.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                #region "Save Update Process Disposal"
                                ms_disposal_type suggestion = db.ms_disposal_type.Find(disposal_req.approval_suggestion_id);

                                tr_disposal_announcement disposalproses = new tr_disposal_announcement();
                                disposalproses.request_id = disposal_req.request_id;
                                disposalproses.approval_disposal_type_id = disposal_req.approval_suggestion_id;
                                if (disposal_req.approval_suggestion_id != 3)
                                {
                                    disposalproses.fl_announcement_status = true;
                                }
                                else
                                {
                                    disposalproses.fl_remove_asset = true;
                                    disposalproses.remove_asset_dept_id = 5;
                                }
                                disposalproses.fl_active = true;
                                disposalproses.created_date = DateTime.Now;
                                disposalproses.created_by = UserProfile.UserId;
                                disposalproses.updated_date = DateTime.Now;
                                disposalproses.updated_by = UserProfile.UserId;
                                disposalproses.deleted_date = null;
                                disposalproses.deleted_by = null;
                                disposalproses.org_id = UserProfile.OrgId;
                                disposalproses = db.tr_disposal_announcement.Add(disposalproses);
                                db.SaveChanges();
                                #endregion

                                #region "kirim email ke PIC Process Disposal"
                                string emailsetting = string.Empty;
                                string to_name = string.Empty;


                                switch (disposal_req.approval_suggestion_id)
                                {
                                    //resale => procurement
                                    case 1:
                                        emailsetting = "EMAIL_TO_DISPOSAL_PROCUREMENT";
                                        to_name = "Department Procurement";
                                        break;
                                    //donation => CSR
                                    case 2:
                                        emailsetting = "EMAIL_TO_DISPOSAL_CSR";
                                        to_name = "Department CSR";
                                        break;
                                    //destroy => Acounting
                                    case 3:
                                        emailsetting = "EMAIL_TO_DISPOSAL_ACCOUNTING";
                                        to_name = "Department Accounting";
                                        break;
                                }
                                var _emailto = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains(emailsetting));

                                sy_email_log sy_email_log = new sy_email_log();
                                sy_email_log.elog_to = _emailto.FirstOrDefault().app_value;
                                sy_email_log.elog_subject = string.Format("Asset Disposal Need Follow Up ({0})", suggestion.disposal_type_name);
                                sy_email_log.elog_template = "EMAIL_TEMPLATE_05";

                                #region "body mail"
                                var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_05"));
                                string strBodyMail = _bodymail.FirstOrDefault().app_value;
                                strBodyMail = strBodyMail.Replace("[to]", to_name);
                                strBodyMail = strBodyMail.Replace("[assetnumber]", disposal_req.asset_number);
                                strBodyMail = strBodyMail.Replace("[aseetname]", disposal_req.asset_name);
                                strBodyMail = strBodyMail.Replace("[assetlocation]", disposal_req.location_name);
                                strBodyMail = strBodyMail.Replace("[department]", disposal_req.department_name);
                                strBodyMail = strBodyMail.Replace("[suggestion]", suggestion.disposal_type_name);
                                strBodyMail = strBodyMail.Replace("[freetext]", "");

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
            disposal_req = DataDisposalView(disposal_req.request_id, disposal_req);
            return View(disposal_req);
        }

        private disposalViewModel DataDisposalView(int? id, disposalViewModel disposalmodel)
        {
            var _qry = (from da in db.tr_disposal_approval
                        where da.fl_active == true && da.deleted_date == null 
                        && da.approval_id == id

                        join t in db.tr_disposal_request on da.request_id equals t.request_id
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

                        join i in db.tr_disposal_announcement.Where(i => (i.deleted_date == null))
                                  on t.request_id equals i.request_id
                                  into ileft
                        from i in ileft.DefaultIfEmpty()

                        select new disposalViewModel()
                        {
                            tr_disposal_request = t,
                            tr_disposal_approval = da,
                            asset_parent = g,
                            tr_disposal_image = h,
                            ms_currency = b,
                            tr_depreciation = a,
                            location_name = f.location_name,
                            department_name = d.department_name,
                            employee_name = e.employee_name,
                            tr_disposal_announcement = i
                        }).First<disposalViewModel>();


            disposalmodel.FormMode = EnumFormModeKey.Form_Edit;
            disposalmodel.request_id = _qry.tr_disposal_request.request_id;
            disposalmodel.disposal_number = _qry.tr_disposal_request.disposal_number;
            disposalmodel.approval_id = _qry.tr_disposal_approval.approval_id;
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
            if (_qry.tr_disposal_approval.approval_suggestion_id != null)
                disposalmodel.fl_approval = true;
            else
                disposalmodel.fl_approval = null;
            disposalmodel.approval_suggestion_id = _qry.tr_disposal_approval.approval_suggestion_id;
            disposalmodel.approval_noted = _qry.tr_disposal_approval.approval_noted;

            if (_qry.tr_disposal_announcement != null)
            {
                disposalmodel.disposal_type_list = (from mdt in db.ms_disposal_type
                                                    where mdt.disposal_type_id == _qry.tr_disposal_approval.approval_suggestion_id
                                                    select mdt).ToList();
                disposalmodel.fl_SuggestionChanges = true;
            }
            else
            {
                disposalmodel.disposal_type_list = db.ms_disposal_type.ToList();
                disposalmodel.fl_SuggestionChanges = false;
            }
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
            return disposalmodel;
        }
    }
}