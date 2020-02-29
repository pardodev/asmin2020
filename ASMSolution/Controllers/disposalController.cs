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
    public class disposalController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        private string app_root_path;
        private string base_image_path;

        // GET: disposal
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
            var _qry = new object();

            //mengambil job level terakhir yg belum di approve
            var x_list_last_approval = (from c in db.tr_disposal_approval
                                        where (c.approval_date != null && c.fl_active == true && c.deleted_date == null)
                                        group c by c.request_id into g
                                        select new
                                        {
                                            request_id = g.Key,
                                            approval_id = g.Max(a => a.approval_id)
                                        }).ToList().AsEnumerable();

            IEnumerable<LastApprovalDTO> x_list_last = (from a in x_list_last_approval
                                                        join b in db.tr_disposal_approval on a.approval_id equals b.approval_id
                                                        select new LastApprovalDTO
                                                        {
                                                            request_id = b.request_id,
                                                            approval_id = b.approval_id,
                                                            approval_suggestion_id = b.approval_suggestion_id,
                                                            approval_status_id = b.approval_status_id,
                                                            approval_level_id = b.approval_level_id
                                                        }).ToList<LastApprovalDTO>().AsEnumerable();

            List<int> _list_id = x_list_last.Select(c => c.approval_id).ToList<int>();

            if (UserProfile.asset_reg_location_id == 2)
            {


                _qry = (from dr in db.tr_disposal_request
                        where (dr.fl_active == true && dr.deleted_date == null)
                                && dr.request_dept_id == UserProfile.department_id
                                && dr.request_location_id == UserProfile.location_id
                        orderby dr.request_id descending

                        join app in (from app in db.tr_disposal_approval
                                     where _list_id.Contains(app.approval_id)
                                     group app by app.request_id 
                                     into appsort
                                     select appsort.FirstOrDefault()
                                     ) on dr.request_id equals app.request_id
                        into leftapp
                        from lftjoinapp in leftapp.DefaultIfEmpty()

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        //where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_asmin_company on dr.org_id equals b.company_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_department on dr.request_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.request_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_register_location on dr.request_location_id equals e.asset_reg_location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_request_status on dr.request_status_id equals f.request_status_id

                        join g in db.tr_disposal_announcement.Where(g => g.fl_active == true) on dr.request_id equals g.request_id
                                into leftg
                        from lftjoing in leftg.DefaultIfEmpty()

                        join h in db.ms_disposal_type on lftjoinapp.approval_suggestion_id equals h.disposal_type_id
                                into lefth
                        from lftjoinh in lefth.DefaultIfEmpty()

                        join i in db.ms_request_status on lftjoinapp.approval_status_id equals i.request_status_id
                         into lefti
                        from lftjoini in lefti.DefaultIfEmpty()

                        join j in db.ms_job_level on lftjoinapp.approval_level_id equals j.job_level_id
                         into leftj
                        from lftjoinj in leftj.DefaultIfEmpty()

                        select new disposalViewModel()
                        {
                            asset_id = dr.asset_id,
                            //asset_parent = a,
                            asset_number = a.asset_number,
                            asset_name = a.asset_name,
                            request_id = dr.request_id,
                            disposal_number = dr.disposal_number,
                            request_date = dr.request_date,
                            request_status_name = f.request_status_name,
                            fl_approval = dr.fl_approval,
                            approval_date = dr.approval_date,

                            company = b,
                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.asset_reg_location_name,
                            fl_announcement_status = lftjoing.fl_announcement_status,
                            fl_fin_announcement = lftjoing.fl_fin_announcement,
                            fl_remove_asset = lftjoing.fl_remove_asset,

                            approval_status_id = lftjoinapp.approval_status_id,
                            approval_status_Name = lftjoini.request_status_name,
                            approval_suggestion_name = lftjoinh.disposal_type_name,
                            approval_level_name = lftjoinj.job_level_name

                        }).ToList<disposalViewModel>().AsEnumerable();

            }
            else if (UserProfile.asset_reg_location_id == 1)
            {
                _qry = (from dr in db.tr_disposal_request
                        where (dr.fl_active == true && dr.deleted_date == null)
                        //&& dr.request_dept_id == UserProfile.department_id
                        //&& dr.request_location_id == UserProfile.location_id
                        orderby dr.request_id descending

                        join app in (from app in db.tr_disposal_approval
                                     where _list_id.Contains(app.approval_id)
                                     group app by app.request_id
                                     into appsort
                                     select appsort.FirstOrDefault()
                                     ) on dr.request_id equals app.request_id
                        into leftapp
                        from lftjoinapp in leftapp.DefaultIfEmpty()

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        //where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_asmin_company on dr.org_id equals b.company_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_department on dr.request_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.request_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_register_location on dr.request_location_id equals e.asset_reg_location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_request_status on dr.request_status_id equals f.request_status_id

                        join g in db.tr_disposal_announcement.Where(g => g.fl_active == true) on dr.request_id equals g.request_id
                                into leftg
                        from lftjoing in leftg.DefaultIfEmpty()

                        join h in db.ms_disposal_type on lftjoinapp.approval_suggestion_id equals h.disposal_type_id
                                into lefth
                        from lftjoinh in lefth.DefaultIfEmpty()

                        join i in db.ms_request_status on lftjoinapp.approval_status_id equals i.request_status_id
                         into lefti
                        from lftjoini in lefti.DefaultIfEmpty()

                        join j in db.ms_job_level on lftjoinapp.approval_level_id equals j.job_level_id
                         into leftj
                        from lftjoinj in leftj.DefaultIfEmpty()

                        select new disposalViewModel()
                        {
                            asset_id = dr.asset_id,
                            //asset_parent = a,
                            asset_number = a.asset_number,
                            asset_name = a.asset_name,
                            request_id = dr.request_id,
                            disposal_number = dr.disposal_number,
                            request_date = dr.request_date,
                            request_status_name = f.request_status_name,
                            fl_approval = dr.fl_approval,
                            approval_date = dr.approval_date,

                            company = b,
                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.asset_reg_location_name,
                            fl_announcement_status = lftjoing.fl_announcement_status,
                            fl_fin_announcement = lftjoing.fl_fin_announcement,
                            fl_remove_asset = lftjoing.fl_remove_asset,

                            approval_status_id = lftjoinapp.approval_status_id,
                            approval_status_Name = lftjoini.request_status_name,
                            approval_suggestion_name = lftjoinh.disposal_type_name,
                            approval_level_name = lftjoinj.job_level_name
                        }).ToList<disposalViewModel>();

            }

            return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
        }

        // GET: Disposal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_disposal_request disposal_req = db.tr_disposal_request.Find(id);
            if (disposal_req == null)
            {
                return HttpNotFound("Assset not found.");
            }

            var _qry = (from t in db.tr_disposal_request
                        where t.fl_active == true && t.deleted_date == null && t.request_id == id
                        && t.org_id == UserProfile.OrgId && t.request_dept_id == UserProfile.department_id

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
                            disposal_number = t.disposal_number,
                            asset_id = t.asset_id,
                            asset_number = g.asset_number,
                            asset_name = g.asset_name,
                            asset_receipt_date = g.asset_receipt_date,
                            currency_code = b.currency_code,
                            asset_book_value = a.asset_book_value,
                            Currency_kurs = a.usd_kurs,
                            asset_original_value = a.asset_original_value,
                            location_id = t.request_location_id,
                            location_name = f.location_name,
                            department_name = d.department_name,
                            employee_name = e.employee_name,
                            request_description = t.request_description,
                            asset_img_address = h.asset_img_address
                        }).ToList<disposalViewModel>();

            //Data Approval view
            var _qrylist = (from da in db.tr_disposal_approval
                            where (da.fl_active == true && da.deleted_date == null && da.request_id == id)

                            join a in db.ms_employee on da.approval_employee_id equals a.employee_id
                            where (a.fl_active == true && a.deleted_date == null)

                            join b in db.ms_job_level on da.approval_level_id equals b.job_level_id
                            where (b.fl_active == true && b.deleted_date == null)

                            join c in db.ms_request_status on da.approval_status_id equals c.request_status_id

                            join d in db.ms_disposal_type on da.approval_suggestion_id equals d.disposal_type_id
                            into d_temp
                            from subd in d_temp.DefaultIfEmpty()

                            select new disposalViewModel()
                            {
                                approval_employee_name = a.employee_name,
                                approval_level_name = b.job_level_name,
                                approval_status_Name = c.request_status_name,
                                approval_date = da.approval_date,
                                approval_suggestion_id = da.approval_suggestion_id,
                                approval_suggestion_name = d_temp.FirstOrDefault().disposal_type_name
                            }).ToList<disposalViewModel>();

            var tuple = new Tuple<disposalViewModel, IEnumerable<disposalViewModel>, string>(_qry.First<disposalViewModel>(), _qrylist, asset_registrationViewModel.path_file_disposal);

            return View(tuple);
        }

        /// <summary>
        /// list utk modal assetlist
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JSON</returns>
        public JsonResult GetAssetList(int? id = 0)
        {
            db.Configuration.ProxyCreationEnabled = false;
            int asset_id = (int)(id);
            if (asset_id == 0)
            {
                var _list = (from t in db.tr_asset_registration
                             where t.fl_active == true && t.deleted_date == null
                                    && t.company_id == UserProfile.company_id && t.department_id == UserProfile.department_id
                                    && t.asset_reg_location_id == UserProfile.asset_reg_location_id
                                    && t.location_id == UserProfile.location_id
                                    && !db.tr_disposal_request.Any(f => (f.asset_id == t.asset_id && f.request_status_id != 3))

                             join a in db.tr_depreciation on t.asset_id equals a.asset_id
                             where (a.fl_active == true && a.deleted_date == null)

                             join b in db.ms_currency on a.asset_original_currency_id equals b.currency_id
                             where (b.fl_active == true && b.deleted_date == null)

                             join c in db.ms_asmin_company on t.company_id equals c.company_id
                             where (c.fl_active == true && c.deleted_date == null)

                             join d in db.ms_department on t.department_id equals d.department_id
                             where (d.fl_active == true && d.deleted_date == null)

                             join e in db.ms_employee on t.employee_id equals e.employee_id
                             where (e.fl_active == true && e.deleted_date == null)

                             join f in db.ms_asset_register_location on t.asset_reg_location_id equals f.asset_reg_location_id
                             where (f.fl_active == true && f.deleted_date == null)

                             select new disposalViewModel()
                             {
                                 asset_id = t.asset_id,
                                 asset_number = t.asset_number,
                                 asset_name = t.asset_name,
                                 asset_receipt_date = t.asset_receipt_date,
                                 currency_code = b.currency_code,
                                 asset_book_value = a.asset_book_value,
                                 Currency_kurs = a.usd_kurs,
                                 asset_original_value = a.asset_original_value,
                                 location_id = t.asset_reg_location_id,
                                 location_name = f.asset_reg_location_name,
                                 department_name = d.department_name,
                                 employee_name = e.employee_name
                             }).ToList<disposalViewModel>();

                return Json(new { data = _list }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var _list = (from t in db.tr_asset_registration
                             where t.fl_active == true && t.deleted_date == null && t.asset_id == asset_id
                             && t.company_id == UserProfile.company_id && t.department_id == UserProfile.department_id
                             && t.asset_reg_location_id == UserProfile.asset_reg_location_id
                             && t.location_id == UserProfile.location_id
                             && !db.tr_disposal_request.Any(f => (f.asset_id == t.asset_id && f.request_status_id != 3))

                             join a in db.tr_depreciation on t.asset_id equals a.asset_id
                             where (a.fl_active == true && a.deleted_date == null)

                             join b in db.ms_currency on a.asset_original_currency_id equals b.currency_id
                             where (b.fl_active == true && b.deleted_date == null)

                             join c in db.ms_asmin_company on t.company_id equals c.company_id
                             where (c.fl_active == true && c.deleted_date == null)

                             join d in db.ms_department on t.department_id equals d.department_id
                             where (d.fl_active == true && d.deleted_date == null)

                             join e in db.ms_employee on t.employee_id equals e.employee_id
                             where (e.fl_active == true && e.deleted_date == null)

                             join f in db.ms_asset_location on t.location_id equals f.location_id
                             where (f.fl_active == true && f.deleted_date == null)

                             select new disposalViewModel()
                             {
                                 asset_id = t.asset_id,
                                 asset_number = t.asset_number,
                                 asset_name = t.asset_name,
                                 asset_receipt_date = t.asset_receipt_date,
                                 currency_code = b.currency_code,
                                 asset_book_value = a.asset_book_value,
                                 Currency_kurs = a.usd_kurs,
                                 asset_original_value = a.asset_original_value,
                                 location_id = t.asset_reg_location_id,
                                 location_name = f.location_name,
                                 department_name = d.department_name,
                                 employee_name = e.employee_name
                             }).ToList<disposalViewModel>();

                return Json(_list, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: disposal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("RequestDisposal")]
        public ActionResult Create([Bind(Include = "asset_id, asset_number,asset_name, location_id, location_name, department_name, request_description")] disposalViewModel disposal_req)
        {
            if (Request.Files.Count > 0)
            {
                var fileexist = Request.Files["asset_img_address"];
                if (fileexist == null || fileexist.ContentLength == 0)
                {
                    ModelState.AddModelError("asset_img_address", "Asset image is mandatory.");
                }
            }
            //input data request disposal with transaction
            if (ModelState.IsValid)
            {
                //Int32 z = Convert.ToInt32("a");
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region "Save Request Disposal"
                        tr_disposal_request disposal_request = new tr_disposal_request();
                        disposal_request.disposal_number = DisposalNumberNew(disposal_req.asset_id);
                        disposal_request.asset_id = disposal_req.asset_id;
                        disposal_request.request_date = DateTime.Now;
                        disposal_request.request_description = disposal_req.request_description;
                        disposal_request.request_location_id = disposal_req.location_id;
                        disposal_request.request_dept_id = UserProfile.department_id;
                        disposal_request.request_emp_id = UserProfile.employee_id;
                        disposal_request.request_status_id = 1;
                        disposal_request.fl_active = true;
                        disposal_request.created_date = DateTime.Now;
                        disposal_request.created_by = UserProfile.UserId;
                        disposal_request.updated_date = DateTime.Now;
                        disposal_request.update_by = UserProfile.UserId;
                        disposal_request.deleted_date = null;
                        disposal_request.deleted_by = null;
                        disposal_request.org_id = UserProfile.OrgId;

                        disposal_request = db.tr_disposal_request.Add(disposal_request);
                        db.SaveChanges();

                        //Save Approval Dept. Head
                        var _qry = (from sa in db.sy_ref_approval_level
                                    where sa.asset_reg_location_id == disposal_req.location_id && sa.job_level_id == 2

                                    join a in db.ms_job_level on sa.job_level_id equals a.job_level_id
                                    where (a.fl_active == true && a.deleted_date == null)

                                    join b in db.ms_employee_detail on a.job_level_id equals b.job_level_id
                                    where (b.fl_active == true && b.deleted_date == null
                                            && b.department_id == UserProfile.department_id && b.company_id == UserProfile.company_id)

                                    join c in db.ms_employee on b.employee_id equals c.employee_id
                                    where c.fl_active == true && c.deleted_date == null

                                    orderby sa.order_no ascending
                                    select new disposalViewModel()
                                    {
                                        department_id = b.department_id,
                                        employee_id = b.employee_id,
                                        job_level_id = a.job_level_id,
                                        employee_email = c.employee_email,
                                        employee_name = c.employee_name,
                                        ip_address = c.ip_address
                                    }).ToList<disposalViewModel>();
                        int count = 1;
                        int approval_id = 0;

                        if (_qry != null)
                        {
                            foreach (disposalViewModel refApproval in _qry)
                            {
                                tr_disposal_approval disposal_approval = new tr_disposal_approval();
                                disposal_approval.request_id = disposal_request.request_id;
                                disposal_approval.approval_date = null;
                                disposal_approval.approval_dept_id = refApproval.department_id;
                                disposal_approval.approval_employee_id = refApproval.employee_id;
                                disposal_approval.approval_level_id = refApproval.job_level_id;
                                disposal_approval.approval_status_id = 1;//waiting approval
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

                                if (count == 1)
                                {
                                    //untuk kebutuhan link email
                                    count++;
                                    approval_id = disposal_approval.approval_id;
                                }
                            }
                        }

                        //Save Approval Dept. ktt
                        var _qry_ktt = (from sa in db.sy_ref_approval_level
                                        where sa.asset_reg_location_id == disposal_req.location_id && sa.job_level_id == 3

                                        join a in db.ms_job_level on sa.job_level_id equals a.job_level_id
                                        where (a.fl_active == true && a.deleted_date == null)

                                        join b in db.ms_employee_detail on a.job_level_id equals b.job_level_id
                                        where (b.fl_active == true && b.deleted_date == null
                                                && b.company_id == UserProfile.company_id)

                                        join c in db.ms_employee on b.employee_id equals c.employee_id
                                        where c.fl_active == true && c.deleted_date == null

                                        orderby sa.order_no ascending
                                        select new disposalViewModel()
                                        {
                                            department_id = b.department_id,
                                            employee_id = b.employee_id,
                                            job_level_id = a.job_level_id,
                                            employee_email = c.employee_email,
                                            employee_name = c.employee_name,
                                            ip_address = c.ip_address
                                        }).ToList<disposalViewModel>();
                        int count_ktt = 1;
                        int approval_id_ktt = 0;

                        if (_qry_ktt != null)
                        {
                            foreach (disposalViewModel refApproval in _qry_ktt)
                            {
                                tr_disposal_approval disposal_approval = new tr_disposal_approval();
                                disposal_approval.request_id = disposal_request.request_id;
                                disposal_approval.approval_date = null;
                                disposal_approval.approval_dept_id = refApproval.department_id;
                                disposal_approval.approval_employee_id = refApproval.employee_id;
                                disposal_approval.approval_level_id = refApproval.job_level_id;
                                disposal_approval.approval_status_id = 1;//waiting approval
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

                                if (count_ktt == 1)
                                {
                                    //untuk kebutuhan link email
                                    count_ktt++;
                                    approval_id_ktt = disposal_approval.approval_id;
                                }
                            }
                        }

                        //Save Approval bod
                        var _qry_bod = (from sa in db.sy_ref_approval_level
                                        where sa.asset_reg_location_id == disposal_req.location_id && sa.job_level_id == 9

                                        join a in db.ms_job_level on sa.job_level_id equals a.job_level_id
                                        where (a.fl_active == true && a.deleted_date == null)

                                        join b in db.ms_employee_detail on a.job_level_id equals b.job_level_id
                                        where (b.fl_active == true && b.deleted_date == null
                                                && b.company_id == UserProfile.company_id)

                                        join c in db.ms_employee on b.employee_id equals c.employee_id
                                        where c.fl_active == true && c.deleted_date == null

                                        join d in db.ms_department on c.employee_id equals d.employee_bod_id
                                        where d.department_id == UserProfile.department_id

                                        orderby sa.order_no ascending
                                        select new disposalViewModel()
                                        {
                                            department_id = b.department_id,
                                            employee_id = d.employee_bod_id,
                                            job_level_id = a.job_level_id,
                                            employee_email = c.employee_email,
                                            employee_name = c.employee_name,
                                            ip_address = c.ip_address
                                        }).ToList<disposalViewModel>();
                        int count_bod = 1;
                        int approval_id_bod = 0;

                        if (_qry_ktt != null)
                        {
                            foreach (disposalViewModel refApproval in _qry_bod)
                            {
                                tr_disposal_approval disposal_approval = new tr_disposal_approval();
                                disposal_approval.request_id = disposal_request.request_id;
                                disposal_approval.approval_date = null;
                                disposal_approval.approval_dept_id = refApproval.department_id;
                                disposal_approval.approval_employee_id = refApproval.employee_id;
                                disposal_approval.approval_level_id = refApproval.job_level_id;
                                disposal_approval.approval_status_id = 1;//waiting approval
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

                                if (count_ktt == 1)
                                {
                                    //untuk kebutuhan link email
                                    count_ktt++;
                                    approval_id_ktt = disposal_approval.approval_id;
                                }
                            }
                        }

                        if (Request.Files.Count > 0)
                        {
                            //var file = Request.Files[0];
                            app_root_path = Server.MapPath("~/");
                            if (string.IsNullOrWhiteSpace(base_image_path))
                                base_image_path = asset_registrationViewModel.path_file_disposal;

                            string img_path = Server.MapPath(base_image_path);
                            if (!Directory.Exists(img_path))
                                Directory.CreateDirectory(img_path);

                            var file = Request.Files["asset_img_address"];
                            if (file != null && file.ContentLength > 0)
                            {
                                var fileName = "asset" + disposal_req.asset_id.ToString() + "_" + Path.GetFileName(file.FileName);
                                var path = Path.Combine(img_path, fileName);
                                file.SaveAs(path);
                                tr_disposal_image _ass_img = new tr_disposal_image()
                                {
                                    request_id = disposal_request.request_id,
                                    asset_id = Convert.ToInt32(disposal_req.asset_id),
                                    asset_img_address = fileName,
                                };
                                db.tr_disposal_image.Add(_ass_img);
                                db.SaveChanges();
                            }
                        }
                        #endregion

                        #region "kirim email ke approval level 1"
                        sy_email_log sy_email_log = new sy_email_log();
                        sy_email_log.elog_to = _qry.FirstOrDefault().employee_email;
                        sy_email_log.elog_subject = string.Format("Asset Disposal Need Approval");
                        sy_email_log.elog_template = "EMAIL_TEMPLATE_04";

                        var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_04"));
                        string strBodyMail = _bodymail.FirstOrDefault().app_value;
                        strBodyMail = strBodyMail.Replace("[to]", _qry.FirstOrDefault().employee_name);
                        strBodyMail = strBodyMail.Replace("[action]", "Dispose");
                        strBodyMail = strBodyMail.Replace("[assetnumber]", disposal_req.asset_number);
                        strBodyMail = strBodyMail.Replace("[aseetname]", disposal_req.asset_name);
                        strBodyMail = strBodyMail.Replace("[assetlocation]", disposal_req.location_name);
                        strBodyMail = strBodyMail.Replace("[department]", disposal_req.department_name);

                        int empid = Convert.ToInt32(_qry.FirstOrDefault().employee_id);
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

                        #region "Save Sy_Message_notification"
                        sy_message_notification msg = new sy_message_notification();
                        msg.notif_group = "BALOON_RECEIPT_04";
                        msg.notify_user = msuser.user_name;
                        msg.notify_ip = _qry.FirstOrDefault().ip_address;
                        msg.notify_message = "Ada permintaan approval untuk Asset disposal.";
                        msg.fl_active = true;
                        msg.created_date = DateTime.Now;
                        msg.created_by = UserProfile.UserId;
                        msg.fl_shown = 0;

                        db.sy_message_notification.Add(msg);
                        db.SaveChanges();
                        #endregion

                        transaction.Commit();

                        ViewBag.ResultMessage = "Record inserted into table successfully.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        // roll back all database operations, if any thing goes wrong
                        transaction.Rollback();

                        string msgErr = string.Format("An unknown error has occurred , Please contact your system administrator. {0}", ex.Message);
                        if (ex.InnerException != null)
                            msgErr += string.Format(" Inner Exception: {0}", ex.InnerException.Message);
                        ModelState.AddModelError("", msgErr);
                    }
                }

            }
            return View(disposal_req);
        }

        private string DisposalNumberNew(int? asset_id)
        {
            tr_asset_registration tr_asset = db.tr_asset_registration.Find(asset_id);
            ms_asmin_company company = db.ms_asmin_company.Find(tr_asset.company_id);
            ms_department department = db.ms_department.Find(tr_asset.department_id);
            ms_asset_category category = db.ms_asset_category.Find(tr_asset.category_id);
            string code = string.Empty;
            code += "D";
            code += company.company_code;
            code += department.department_code;
            code += category.category_code;
            code += DateTime.Today.Year;
            code += DateTime.Today.Month.ToString().PadLeft(2, '0');

            var _lastno = (from dr in db.tr_disposal_request
                           where dr.disposal_number.Contains(code)
                           orderby dr.request_id descending
                           select dr).ToList<tr_disposal_request>();
            if (_lastno.Count > 0)
            {
                tr_disposal_request lastno = _lastno.FirstOrDefault();
                string currentno = (Convert.ToInt32(lastno.disposal_number.Substring((lastno.disposal_number.Length - 2), 2)) + 1).ToString();
                code += currentno.PadLeft(2, '0');
            }
            else code += "01";
            code += "-";
            code += tr_asset.asset_number.Substring((tr_asset.asset_number.Length - 2), 2);

            return code;
        }

        // GET: disposal/request
        public ActionResult RequestDisposal()
        {
            disposalViewModel disposal_req = new disposalViewModel()
            {
                FormMode = EnumFormModeKey.Form_New,
                asset_id = 0
            };

            return View(disposal_req);
        }

        public ActionResult ModalAsset()
        {
            return PartialView();
        }
    }
}