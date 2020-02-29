using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using ASM_UI.Models;
using ASM_UI.App_Helpers;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class mutationapprovalController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: mutationApproval
        public ActionResult Index()
        {
            //return View();
            
            var _qry = (from dr in db.tr_mutation_request
                        where (dr.fl_active == true && dr.deleted_date == null) && dr.request_status != 3
                               //&& dr.request_dept_id == UserProfile.department_id
                        //dr.org_id == UserProfile.OrgId &&
                        //&& dr.request_location_id == UserProfile.asset_reg_location_id

                        join da in db.tr_mutation_approval on dr.request_id equals da.request_id
                        where (da.fl_active == true && da.deleted_date == null) && da.approval_employee_id == UserProfile.employee_id

                        //mengambil job level terakhir yg belum di approve
                        join app in (from app in db.tr_mutation_approval
                                     where (app.approval_date == null && app.fl_active == true && app.deleted_date == null)
                                     orderby app.approval_id ascending
                                     group app by app.request_id into appsort
                                     select appsort.FirstOrDefault())
                                    on da.request_id equals app.request_id
                        where da.approval_employee_id == app.approval_employee_id

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        //where (a.fl_active == true && a.deleted_date == null)
                        into t_a
                        from ta in t_a.DefaultIfEmpty()

                        join b in db.ms_asmin_company on ta.company_id equals b.company_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_department on dr.transfer_to_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.transfer_to_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_register_location on dr.transfer_to_location_id equals e.asset_reg_location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_request_status on da.approval_status_id equals f.request_status_id
                        into t_f
                        from tf in t_f.DefaultIfEmpty()

                        select new AssetMutationViewModel()
                        {
                            asset_id = dr.asset_id,
                            asset_parent = ta,

                            request_code = dr.request_code,
                            request_id = dr.request_id,
                            request_date = dr.request_date,
                            request_status_name = tf.request_status_name,
                            fl_approval = dr.fl_approval,
                            approval_date = dr.approval_date,
                            approval_status_id = da.approval_status_id,

                            company = b,
                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.asset_reg_location_name
                        }).ToList<AssetMutationViewModel>();

            var _qry2 = (from dr in db.tr_mutation_request
                        where (dr.fl_active == true && dr.deleted_date == null) && dr.request_status != 3
                               //&& dr.request_dept_id == UserProfile.department_id
                        //dr.org_id == UserProfile.OrgId &&
                        //&& dr.request_location_id == UserProfile.asset_reg_location_id

                        join da in db.tr_mutation_approval on dr.request_id equals da.request_id
                        where (da.fl_active == true && da.deleted_date == null) 
                        && da.approval_employee_id == UserProfile.employee_id && da.approval_date != null

                        ////mengambil job level terakhir yg belum di approve
                        //join app in (from app in db.tr_mutation_approval
                        //             where (app.approval_date == null && app.fl_active == true && app.deleted_date == null)
                        //             orderby app.approval_id ascending
                        //             group app by app.request_id into appsort
                        //             select appsort.FirstOrDefault())
                        //            on da.request_id equals app.request_id
                        //where da.approval_employee_id == app.approval_employee_id

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        //where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_asmin_company on a.company_id equals b.company_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_department on dr.transfer_to_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.transfer_to_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_register_location on dr.transfer_to_location_id equals e.asset_reg_location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_request_status on da.approval_status_id equals f.request_status_id

                        select new AssetMutationViewModel()
                        {
                            asset_id = dr.asset_id,
                            asset_parent = a,

                            request_code = dr.request_code,
                            request_id = dr.request_id,
                            request_date = dr.request_date,
                            request_status_name = f.request_status_name,
                            fl_approval = dr.fl_approval,
                            approval_date = dr.approval_date,
                            approval_status_id = da.approval_status_id,

                            company = b,
                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.asset_reg_location_name
                        }).ToList<AssetMutationViewModel>();

            _qry.AddRange(_qry2);

            


            return View(_qry);

        }

        public ActionResult approval(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_mutation_request mutation_req = db.tr_mutation_request.Find(id);
            if (mutation_req == null)
            {
                return HttpNotFound("Assset not found.");
            }
            AssetMutationViewModel mutationmodel = new AssetMutationViewModel();
            mutationmodel = DataMutationView(id, mutationmodel);

            return View(mutationmodel);
        }

        public ActionResult details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_mutation_request mutation_req = db.tr_mutation_request.Find(id);
            if (mutation_req == null)
            {
                return HttpNotFound("Assset not found.");
            }
            AssetMutationViewModel mutationmodel = new AssetMutationViewModel();
            mutationmodel = DataMutationView(id, mutationmodel);

            return View(mutationmodel);
        }

        private AssetMutationViewModel DataMutationView(int? id, AssetMutationViewModel mutationmodel)
        {
            var _qry = (from t in db.tr_mutation_request
                        where t.fl_active == true && t.deleted_date == null && t.request_id == id
                        //&& t.request_dept_id == UserProfile.department_id
                        //&& t.org_id == UserProfile.OrgId 

                        join da in db.tr_mutation_approval on t.request_id equals da.request_id
                        where da.fl_active == true && da.deleted_date == null && da.approval_employee_id == UserProfile.employee_id
                                //&& da.approval_date == null && da.approval_status_id == 1

                        join a in db.tr_depreciation on t.asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_currency on a.asset_original_currency_id equals b.currency_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_asmin_company on t.org_id equals c.company_id
                        where (c.fl_active == true && c.deleted_date == null)
                        #region Request
                        join d in db.ms_department on t.request_dept_id equals d.department_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_employee on t.request_emp_id equals e.employee_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_asset_location on t.request_location_id equals f.location_id
                        where (f.fl_active == true && f.deleted_date == null)
                        #endregion
                        #region Transfer To
                        join h in db.ms_department on t.transfer_to_dept_id equals h.department_id
                        where (h.fl_active == true && h.deleted_date == null)

                        join i in db.ms_employee on t.transfer_to_emp_id equals i.employee_id
                        where (i.fl_active == true && i.deleted_date == null)

                        join j in db.ms_asset_location on t.transfer_to_location_id equals j.location_id
                        where (j.fl_active == true && j.deleted_date == null)
                        #endregion
                        join g in db.tr_asset_registration on t.asset_id equals g.asset_id
                        //where (g.fl_active == true && g.deleted_date == null)


                        select new AssetMutationViewModel()
                        {
                            tr_mutation_request = t,
                            tr_mutation_approval = da,
                            asset_parent = g,
                            ms_currency = b,
                            tr_depreciation = a,
                            //current location
                            location_name = f.location_name,
                            department_name = d.department_name,
                            employee_name = e.employee_name,
                            //transfer to
                            transfer_to_dept_name = h.department_name,
                            transfer_to_emp_name = i.employee_name,
                            transfer_to_location_name = j.location_name,
                            approval_status_id = da.approval_status_id,
                            fl_approval = t.fl_approval
                        }).First<AssetMutationViewModel>();


            mutationmodel.FormMode = EnumFormModeKey.Form_Edit;
            mutationmodel.request_id = _qry.tr_mutation_request.request_id;
            mutationmodel.request_code = _qry.tr_mutation_request.request_code;
            mutationmodel.approval_id = _qry.tr_mutation_approval.approval_id;
            mutationmodel.asset_id = _qry.tr_mutation_request.asset_id;
            mutationmodel.asset_number = _qry.asset_parent.asset_number;
            mutationmodel.asset_name = _qry.asset_parent.asset_name;
            mutationmodel.asset_receipt_date = _qry.asset_parent.asset_receipt_date;
            //mutationmodel.asset_img_address = _qry.tr_disposal_image.asset_img_address;
            mutationmodel.currency_code = _qry.ms_currency.currency_code;
            mutationmodel.asset_book_value = _qry.tr_depreciation.asset_book_value;
            mutationmodel.currency_kurs = _qry.tr_depreciation.usd_kurs;
            mutationmodel.asset_original_value = _qry.tr_depreciation.asset_original_value;
            #region request
            mutationmodel.request_location_id = _qry.tr_mutation_request.request_location_id;
            mutationmodel.location_name = _qry.location_name;
            mutationmodel.request_dept_id = _qry.tr_mutation_request.request_dept_id;
            mutationmodel.department_name = _qry.department_name;
            mutationmodel.request_emp_id = _qry.tr_mutation_request.request_emp_id;
            mutationmodel.employee_name = _qry.employee_name;
            #endregion
            #region transfer
            mutationmodel.transfer_to_location_id = _qry.tr_mutation_request.transfer_to_location_id;
            mutationmodel.transfer_to_location_name = _qry.transfer_to_location_name;
            mutationmodel.transfer_to_dept_id = _qry.tr_mutation_request.transfer_to_dept_id;
            mutationmodel.transfer_to_dept_name = _qry.transfer_to_dept_name;
            mutationmodel.transfer_to_emp_id = _qry.tr_mutation_request.transfer_to_emp_id;
            mutationmodel.transfer_to_emp_name = _qry.transfer_to_emp_name;
            #endregion
            //mutationmodel.request_description = _qry.tr_mutation_request.request_description;
            if (_qry.approval_status_id == 1)
                mutationmodel.fl_approval = null;
            else
                mutationmodel.fl_approval = _qry.fl_approval;
            mutationmodel.approval_noted = _qry.tr_mutation_approval.approval_noted;

            //Data Approval view
            mutationmodel.mutation_Approval_list = (from da in db.tr_mutation_approval
                                                    where (da.fl_active == true && da.deleted_date == null && da.request_id == id)

                                                    join a in db.ms_employee on da.approval_employee_id equals a.employee_id
                                                    where (a.fl_active == true && a.deleted_date == null)

                                                    join b in db.ms_job_level on da.approval_level_id equals b.job_level_id
                                                    where (b.fl_active == true && b.deleted_date == null)

                                                    join c in db.ms_request_status on da.approval_status_id equals c.request_status_id

                                                    //join d in db.ms_disposal_type on da.approval_suggestion_id equals d.disposal_type_id
                                                    //into d_temp
                                                    //from subd in d_temp.DefaultIfEmpty()

                                                    select new mutationappViewModel()
                                                    {
                                                        approval_employee_name = a.employee_name,
                                                        approval_level_name = b.job_level_name,
                                                        approval_status_Name = c.request_status_name,
                                                        approval_date = da.approval_date
                                                        //approval_suggestion_id = da.approval_suggestion_id,
                                                        //approval_suggestion_name = d_temp.FirstOrDefault().disposal_type_name
                                                    }).ToList<mutationappViewModel>();
            return mutationmodel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approval([Bind(Include = "request_id, asset_id, approval_id, fl_approval, approval_noted, asset_number,asset_name, location_name, department_name, employee_name, transfer_to_location_name, transfer_to_dept_name, transfer_to_emp_name, transfer_to_location_id, transfer_to_dept_id, transfer_to_emp_id")] AssetMutationViewModel mutation_req)
        {
            if (mutation_req.fl_approval != null)
            {
                if (mutation_req.fl_approval == true)
                {
                    //if (mutation_req.approval_suggestion_id == null || mutation_req.approval_suggestion_id == 0)
                    //    ModelState.AddModelError("approval_suggestion_id", "Suggestion is Mandatory.");
                }
            }
            else if (mutation_req.approval_noted == null || mutation_req.approval_noted.Trim() == string.Empty)
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
                        #region "Save update Request Asset Mutation"
                        tr_mutation_request mutation_request = db.tr_mutation_request.Find(mutation_req.request_id);
                        mutation_request.fl_approval = mutation_req.fl_approval;
                        if (mutation_req.fl_approval == true)
                        {
                            var doneapp = (from app in db.tr_mutation_approval
                                           where (app.approval_date == null && app.fl_active == true
                                                    && app.deleted_date == null && app.request_id == mutation_req.request_id)
                                           select app).ToList();

                            if (doneapp.Count == 1)
                            {
                                mutation_request.request_status = 5;//complete
                                iscomplete = true;

                                tr_mutation_process tr = new tr_mutation_process();
                                tr.request_id = mutation_req.request_id;
                                tr.org_id = UserProfile.OrgId;
                                tr.fl_active = true;

                                tr = db.tr_mutation_process.Add(tr);
                                db.SaveChanges();
                            }
                            else
                                mutation_request.request_status = 2;//approve
                        }
                        else
                        {
                            mutation_request.request_status = 3; //reject
                        }

                        mutation_request.approval_date = DateTime.Now;
                        mutation_request.updated_date = DateTime.Now;
                        mutation_request.updated_by = UserProfile.UserId;
                        mutation_request.deleted_date = null;
                        mutation_request.deleted_by = null;

                        db.Entry(mutation_request).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion

                        #region "Save update Approval Mutation"
                        tr_mutation_approval mutation_approval = db.tr_mutation_approval.Find(mutation_req.approval_id);

                        if (mutation_req.fl_approval == true)
                        {
                            mutation_approval.approval_status_id = 2;//approve
                        }
                        else
                        {
                            mutation_approval.approval_status_id = 3; //reject
                            mutation_approval.approval_noted = mutation_req.approval_noted;
                        }
                        mutation_approval.approval_date = DateTime.Now;
                        //mutation_approval.approval_location_id = UserProfile.location_id; --> location_id bukan dari login
                        mutation_approval.updated_date = DateTime.Now;
                        mutation_approval.updated_by = UserProfile.UserId;
                        mutation_approval.deleted_date = null;
                        mutation_approval.deteled_by = null;

                        db.Entry(mutation_approval).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion

                        if (mutation_req.fl_approval == true)
                        {
                            if (!iscomplete)
                            {
                                #region "kirim email ke approval"

                                var next_approval = (from app in db.tr_mutation_approval
                                                     where (app.approval_date == null && app.fl_active == true && app.deleted_date == null)
                                                            && app.request_id == mutation_req.request_id
                                                     orderby app.approval_id ascending

                                                     join a in db.ms_employee on app.approval_employee_id equals a.employee_id
                                                     where a.fl_active == true && a.deleted_date == null
                                                     select a).FirstOrDefault<ms_employee>();

                                if (next_approval != null)
                                {
                                    sy_email_log sy_email_log = new sy_email_log();
                                    sy_email_log.elog_to = next_approval.employee_email;
                                    sy_email_log.elog_subject = string.Format("Asset Mutation Need Approval");
                                    sy_email_log.elog_template = "EMAIL_TEMPLATE_02";

                                    #region "body mail"
                                    var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_02"));
                                    string strBodyMail = _bodymail.FirstOrDefault().app_value;
                                    strBodyMail = strBodyMail.Replace("[to]", next_approval.employee_name);
                                    strBodyMail = strBodyMail.Replace("[assetnumber]", mutation_req.asset_number);
                                    strBodyMail = strBodyMail.Replace("[assetname]", mutation_req.asset_name);
                                    strBodyMail = strBodyMail.Replace("[assetlocation]", mutation_req.location_name);
                                    strBodyMail = strBodyMail.Replace("[department]", mutation_req.department_name);
                                    strBodyMail = strBodyMail.Replace("[employee]", mutation_req.employee_name);
                                    //strBodyMail = strBodyMail.Replace("[link]", "");
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
                                if (next_approval != null)
                                {
                                    int empid = Convert.ToInt32(next_approval.employee_id);
                                    ms_user msuser = (from m in db.ms_user
                                                      where m.employee_id == empid
                                                      select m).FirstOrDefault();

                                    sy_message_notification msg = new sy_message_notification();
                                    msg.notif_group = "BALOON_RECEIPT_03";
                                    msg.notify_user = msuser.user_name;
                                    msg.notify_ip = next_approval.ip_address;
                                    msg.notify_message = "Ada permintaan approval untuk asset mutasi.";
                                    msg.fl_active = true;
                                    msg.created_date = DateTime.Now;
                                    msg.created_by = UserProfile.UserId;
                                    msg.fl_shown = 0;

                                    db.sy_message_notification.Add(msg);
                                    db.SaveChanges();
                                }
                                #endregion
                            }
                            else
                            {
                                //kl dh complete???
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
            mutation_req = DataMutationView(mutation_req.request_id, mutation_req);
            return View(mutation_req);
        }

    }
}