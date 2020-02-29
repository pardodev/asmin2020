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
    public class mutationreceiveController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: mutationreceive
        public ActionResult Index()
        {
            //ms_employee_detail empCurrent = db.ms_employee_detail.Find(49);

            //return View();
            var _qry = (from dr in db.tr_mutation_request
                        where (dr.fl_active == true && dr.deleted_date == null && dr.request_status == 5) //cari yg udh di complete approval

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        //where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_asmin_company on a.company_id equals b.company_id
                        where (b.fl_active == true && b.deleted_date == null)

                        //request
                        join c in db.ms_department on dr.request_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.request_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_location on dr.request_location_id equals e.location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        //transfer to
                        join g in db.ms_department on dr.transfer_to_dept_id equals g.department_id
                        where (g.fl_active == true && g.deleted_date == null)

                        join h in db.ms_employee on dr.transfer_to_emp_id equals h.employee_id
                        where (h.fl_active == true && h.deleted_date == null)

                        join i in db.ms_asset_location on dr.transfer_to_location_id equals i.location_id
                        where (i.fl_active == true && i.deleted_date == null)

                        join f in db.ms_request_status on dr.request_status equals f.request_status_id

                        join mp in db.tr_mutation_process on dr.request_id equals mp.request_id
                        where (mp.fl_active == true && mp.deleted_date == null && mp.fl_pic_asset_comfirm == true)

                        //join emp_det in db.ms_employee_detail on h.employee_id equals emp_det.employee_id
                        //where (emp_det.department_id == UserProfile.department_id && emp_det.user_type_id == empCurrent.user_type_id
                        //    && emp_det.company_id == UserProfile.company_id
                        //    && emp_det.user_type_id == 1
                        //)

                        select new AssetMutationViewModel()
                        {

                            //department_id = UserProfile.department_id,

                            request_id = dr.request_id,
                            request_code = dr.request_code,
                            request_date = dr.request_date,
                            asset_id = dr.asset_id,
                            asset_parent = a,


                            fl_approval = dr.fl_approval,
                            request_status_name = f.request_status_name,
                            approval_date = dr.approval_date,

                            company = b,
                            //request
                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.location_name,

                            //transfer to
                            transfer_to_dept_id = g.department_id,
                            transfer_to_dept_name = g.department_name,
                            transfer_to_emp_id = h.employee_id,
                            transfer_to_emp_name = h.employee_name,
                            transfer_to_location_id = i.location_id,
                            transfer_to_location_name = i.location_name

                            ,
                            process = mp
                        }).ToList<AssetMutationViewModel>();

            return View(_qry);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //tr_mutation_request mutation_req = db.tr_mutation_request.Find(id);
            tr_mutation_process mutation_req = db.tr_mutation_process.Find(id); //changed by wisnu

            if (mutation_req == null)
            {
                return HttpNotFound("Assset not found.");
            }

            AssetMutationViewModel mutationreceive = new AssetMutationViewModel();
            mutationreceive = DataMutationView(id, mutationreceive);

            return View(mutationreceive);
        }

        public ActionResult Receive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //tr_mutation_request mutation_req = db.tr_mutation_request.Find(id);
            tr_mutation_process mutation_req = db.tr_mutation_process.Find(id); //changed by wisnu
            if (mutation_req == null)
            {
                return HttpNotFound("Assset not found.");
            }
            
            AssetMutationViewModel mutationreceive = new AssetMutationViewModel();
            mutationreceive = DataMutationView(id, mutationreceive);

            return View(mutationreceive);


            //return View();
        }

        private AssetMutationViewModel DataMutationView(int? id, AssetMutationViewModel mutationmodel)
        {
            var _qry = (from tp in db.tr_mutation_process
                        where tp.fl_active == true && tp.deleted_date == null && tp.mutation_id == id

                        join t in db.tr_mutation_request on tp.request_id equals t.request_id
                        where t.fl_active == true && t.deleted_date == null

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
                        join k in db.ms_courier on tp.courier_id equals k.courier_id

                        select new AssetMutationViewModel()
                        {
                            process = tp,
                            tr_mutation_request = t,
                            courier = k,
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
                            transfer_to_location_name = j.location_name

                        }).First<AssetMutationViewModel>();


            mutationmodel.FormMode = EnumFormModeKey.Form_Edit;
            mutationmodel.mutation_id = _qry.process.mutation_id;
            mutationmodel.request_id = _qry.tr_mutation_request.request_id;
            mutationmodel.request_code = _qry.tr_mutation_request.request_code;

            mutationmodel.asset_id = _qry.tr_mutation_request.asset_id;
            mutationmodel.asset_number = _qry.asset_parent.asset_number;
            mutationmodel.asset_name = _qry.asset_parent.asset_name;
            mutationmodel.asset_receipt_date = _qry.asset_parent.asset_receipt_date;
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

            mutationmodel.process = _qry.process;
            mutationmodel.courier = _qry.courier;

            //mutationmodel.request_description = _qry.tr_mutation_request.request_description;
            //mutationmodel.fl_approval = null;
            //mutationmodel.approval_noted = _qry.tr_mutation_approval.approval_noted;

            //Data Approval view
            mutationmodel.mutation_Approval_list = (from da in db.tr_mutation_approval
                                                    where (da.fl_active == true && da.deleted_date == null && da.request_id == mutationmodel.request_id)

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
        [ActionName("Receive")]
        public ActionResult SubmitReceiveMutation([Bind(Include = "mutation_id, asset_id, request_id, fl_pic_asset_receive, courier_description, asset_number, asset_name, transfer_to_dept_id, transfer_to_emp_id, transfer_to_location_id")] AssetMutationViewModel mutation_rcv)
        {
            tr_mutation_process mut_process = db.tr_mutation_process.Find(mutation_rcv.mutation_id);

            //update disposal request and approval data with transaction
            if (mutation_rcv.fl_pic_asset_receive == null)
            {
                ModelState.AddModelError("fl_pic_asset_receive", "Confirmation of receive asset is mandatory.");
            }
            else if (string.IsNullOrEmpty(mutation_rcv.courier_description))
            {
                ModelState.AddModelError("courier_description", "Feedback is mandatory.");
            }

            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region "Save update Mutation process"

                        if (mutation_rcv.fl_pic_asset_receive == null)
                        {
                            mut_process.fl_pic_asset_receive = false;
                        }
                        else if (mutation_rcv.fl_pic_asset_receive == true)
                        {
                            //ms_employee_detail ed = db.ms_employee_detail.Find(UserProfile.employee_id);
                            mut_process.fl_pic_asset_receive = true;
                            mut_process.pic_asset_received_date = DateTime.Now;
                            mut_process.courier_description = mutation_rcv.courier_description;
                            mut_process.pic_asset_received_employee_id = UserProfile.employee_id;
                            //mut_process.pic_asset_level_id = UserProfile.
                            mut_process.user_asset_received_date = DateTime.Now;
                            mut_process.user_asset_received_employee_id = mutation_rcv.transfer_to_emp_id;

                            tr_mutation_request mr = db.tr_mutation_request.Find(mutation_rcv.request_id);
                            tr_asset_registration ar = db.tr_asset_registration.Find(mr.asset_id);

                            ar.current_department_id = mr.transfer_to_dept_id;
                            ar.current_employee_id = mr.transfer_to_emp_id;
                            ar.current_location_id = mr.transfer_to_location_id;

                            db.Entry(ar).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        
                        mut_process.updated_date = DateTime.Now;
                        mut_process.updated_by = UserProfile.UserId;
                        mut_process.deleted_date = null;
                        mut_process.deleted_by = null;

                        db.Entry(mut_process).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion


                        #region "kirim email ke PIC Process Mutation"
                        //string emailsetting = string.Empty;
                        //string to_name = string.Empty;
                        //string freetext = string.Empty;

                        //ms_department depTransfer = db.ms_department.Find(mutation_rcv.transfer_to_dept_id);

                        //emailsetting = "EMAIL_TEMPLATE_03";
                        //to_name = "Department " + depTransfer.department_name;
                        //freetext = "PIC Asset Department telah mengirim <strong>" + mutation_rcv.asset_name + "(" + mutation_rcv.asset_number + ")</strong>.";

                        //var _emailto = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains(emailsetting));

                        //sy_email_log sy_email_log = new sy_email_log();
                        //sy_email_log.elog_to = _emailto.FirstOrDefault().app_value;
                        //sy_email_log.elog_subject = string.Format("Asset Mutation Ready to Process");
                        //sy_email_log.elog_template = "EMAIL_TEMPLATE_03";

                        //#region "body mail"
                        //var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_03"));
                        //string strBodyMail = _bodymail.FirstOrDefault().app_value;
                        //strBodyMail = strBodyMail.Replace("[to]", to_name);
                        //strBodyMail = strBodyMail.Replace("[assetnumber]", mutation_rcv.asset_number);
                        //strBodyMail = strBodyMail.Replace("[aseetname]", mutation_rcv.asset_name);
                        //strBodyMail = strBodyMail.Replace("[assetlocation]", mutation_rcv.location_name);
                        //strBodyMail = strBodyMail.Replace("[department]", disposal_prs.department_name);
                        //strBodyMail = strBodyMail.Replace("[suggestion]", "Resale");
                        //strBodyMail = strBodyMail.Replace("[freetext]", freetext);
                        ////strBodyMail = strBodyMail.Replace("[link]", "");
                        //sy_email_log.elog_body = strBodyMail;
                        //#endregion

                        //var EmailHelper = new EmailHelper()
                        //{
                        //    ToAddress = sy_email_log.elog_to,
                        //    Email_Template = sy_email_log.elog_template,
                        //    MailSubject = sy_email_log.elog_subject,
                        //    MailBody = sy_email_log.elog_body
                        //};
                        //EmailHelper.Send();
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
            mutation_rcv = DataMutationView(mutation_rcv.request_id, mutation_rcv);
            return View(mutation_rcv);
        }
    }
}