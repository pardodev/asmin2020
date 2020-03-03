using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;
using System.Data.SqlClient;
using System.Web.Configuration;
using ASM_UI.App_Helpers;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class mutationController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: mutation
        public ActionResult Index()
        {
            var _qry = new object();

            if (UserProfile.asset_reg_location_id == 2) //branchs
            {
                _qry = (from dr in db.tr_mutation_request
                        where (dr.fl_active == true && dr.deleted_date == null)
                        //&& dr.org_id == UserProfile.OrgId
                        //&& dr.request_dept_id == UserProfile.department_id
                        //&& dr.request_location_id == UserProfile.location_id

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null
                        && a.company_id == UserProfile.company_id
                        && a.current_location_id == UserProfile.location_id
                        )

                        //join b in db.ms_asmin_company on a.company_id equals b.company_id
                        //where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_department on dr.transfer_to_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.transfer_to_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        //join e in db.ms_asset_register_location on dr.request_location_id equals e.asset_reg_location_id
                        //where (e.fl_active == true && e.deleted_date == null)

                        join e in db.ms_asset_location on dr.transfer_to_location_id equals e.location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_request_status on dr.request_status equals f.request_status_id

                        select new AssetMutationViewModel()
                        {
                            asset_id = dr.asset_id,
                            asset_parent = a,

                            request_id = dr.request_id,
                            request_code = dr.request_code,
                            request_date = dr.request_date,
                            request_status_name = f.request_status_name,
                            fl_approval = dr.fl_approval,
                            approval_date = dr.approval_date,

                            //company = b,
                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.location_name
                        }).ToList<AssetMutationViewModel>();
            }
            else if (UserProfile.asset_reg_location_id == 1)
            {
                _qry = (from dr in db.tr_mutation_request
                        where (dr.fl_active == true && dr.deleted_date == null)
                        //&& dr.org_id == UserProfile.OrgId
                        //&& dr.request_dept_id == UserProfile.department_id
                        //&& dr.request_location_id == UserProfile.location_id

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null
                        && a.company_id == UserProfile.company_id
                        //&& a.current_location_id == UserProfile.location_id
                        )

                        //join b in db.ms_asmin_company on a.company_id equals b.company_id
                        //where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_department on dr.transfer_to_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.transfer_to_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        //join e in db.ms_asset_register_location on dr.request_location_id equals e.asset_reg_location_id
                        //where (e.fl_active == true && e.deleted_date == null)

                        join e in db.ms_asset_location on dr.transfer_to_location_id equals e.location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_request_status on dr.request_status equals f.request_status_id

                        select new AssetMutationViewModel()
                        {
                            asset_id = dr.asset_id,
                            asset_parent = a,

                            request_id = dr.request_id,
                            request_code = dr.request_code,
                            request_date = dr.request_date,
                            request_status_name = f.request_status_name,
                            fl_approval = dr.fl_approval,
                            approval_date = dr.approval_date,

                            //company = b,
                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.location_name
                        }).ToList<AssetMutationViewModel>();
            }
            return View(_qry);
        }

        [HttpGet]
        public JsonResult List()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var _qry = new object();
            if (UserProfile.asset_reg_location_id == 2) //branchs
            {
                _qry = (from dr in db.tr_mutation_request
                        where (dr.fl_active == true && dr.deleted_date == null)
                        //&& dr.org_id == UserProfile.OrgId
                        //&& dr.request_dept_id == UserProfile.department_id
                        //&& dr.request_location_id == UserProfile.location_id

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null
                        && a.company_id == UserProfile.company_id
                        && a.current_location_id == UserProfile.location_id
                        )

                        //join b in db.ms_asmin_company on a.company_id equals b.company_id
                        //where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_department on dr.transfer_to_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.transfer_to_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        //join e in db.ms_asset_register_location on dr.request_location_id equals e.asset_reg_location_id
                        //where (e.fl_active == true && e.deleted_date == null)

                        join e in db.ms_asset_location on dr.transfer_to_location_id equals e.location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_request_status on dr.request_status equals f.request_status_id

                        select new 
                        {
                            asset_id = dr.asset_id,
                            asset_number = a.asset_number,
                            asset_name = a.asset_name,

                            request_id = dr.request_id,
                            request_code = dr.request_code,
                            request_date = dr.request_date,
                            request_status_name = f.request_status_name,
                            fl_approval = dr.fl_approval,
                            approval_date = dr.approval_date,

                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.location_name
                        }).ToList();
            }
            else if (UserProfile.asset_reg_location_id == 1)
            {
                _qry = (from dr in db.tr_mutation_request
                        where (dr.fl_active == true && dr.deleted_date == null)
                        //&& dr.org_id == UserProfile.OrgId
                        //&& dr.request_dept_id == UserProfile.department_id
                        //&& dr.request_location_id == UserProfile.location_id

                        join a in db.tr_asset_registration on dr.asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null
                        && a.company_id == UserProfile.company_id
                        //&& a.current_location_id == UserProfile.location_id
                        )

                        //join b in db.ms_asmin_company on a.company_id equals b.company_id
                        //where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_department on dr.transfer_to_dept_id equals c.department_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_employee on dr.transfer_to_emp_id equals d.employee_id
                        where (d.fl_active == true && d.deleted_date == null)

                        //join e in db.ms_asset_register_location on dr.request_location_id equals e.asset_reg_location_id
                        //where (e.fl_active == true && e.deleted_date == null)

                        join e in db.ms_asset_location on dr.transfer_to_location_id equals e.location_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_request_status on dr.request_status equals f.request_status_id

                        select new 
                        {
                            asset_id = dr.asset_id,
                            asset_number = a.asset_number,
                            asset_name = a.asset_name,

                            request_id = dr.request_id,
                            request_code = dr.request_code,
                            request_date = dr.request_date,
                            request_status_name = f.request_status_name,
                            fl_approval = dr.fl_approval,
                            approval_date = dr.approval_date,

                            department_name = c.department_code,
                            employee_name = d.employee_name,
                            location_name = e.location_name
                        }).ToList();
            }


            return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RequestMutation()
        {
            AssetMutationViewModel mutation_req = new AssetMutationViewModel()
            {
                FormMode = EnumFormModeKey.Form_New,
                asset_id = 0
            };


            #region "for dropdown Employee"

            var _employeelist = db.ms_employee.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                t => new
                {
                    t.employee_id,
                    t.employee_nik,
                    t.employee_name
                }).ToList();

            SelectList itemsType = new SelectList(_employeelist, "employee_id", "employee_name");
            ViewBag.transfer_to_emp_id = itemsType;
            #endregion

            #region "for dropdown Department"
            var _departmentlist = db.ms_department.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                 t => new
                 {
                     t.department_id,
                     t.department_code,
                     t.department_name
                 }).ToList();
            SelectList itemsType2 = new SelectList(_departmentlist, "department_id", "department_name");
            ViewBag.transfer_to_dept_id = itemsType2;
            #endregion

            #region "for dropdown Location"
            var _locationlist = db.ms_asset_location.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                t => new
                {
                    t.location_id,
                    t.location_code,
                    t.location_name
                }).ToList();
            SelectList itemsType3 = new SelectList(_locationlist, "location_id", "location_name");
            ViewBag.transfer_to_location_id = itemsType3;
            #endregion

            return View("~/Views/mutation/Request.cshtml");
        }

        public ActionResult ModalAsset()
        {
            return PartialView();
        }

        public JsonResult GetAssetList(string id)
        {
            int AssetID = int.Parse(id);

            List<AssetMutationViewModel> VendorAssetList = new List<AssetMutationViewModel>();

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);

            var cmd2 = new SqlCommand("GetData", conn);
            cmd2.CommandText = "EXEC SP_GET_ALL_ASSET_AVAILABLE_FOR_MUTATION @Org_ID, @asset_id, @location_id, @department_id ";
            cmd2.Parameters.AddWithValue("@Org_ID", UserProfile.OrgId);
            cmd2.Parameters.AddWithValue("@asset_id", AssetID);
            cmd2.Parameters.AddWithValue("@location_id", UserProfile.location_id);
            cmd2.Parameters.AddWithValue("@department_id", UserProfile.department_id);

            conn.Open();
            using (SqlDataReader dr = cmd2.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(dr);

                foreach (DataRow row in tb.Rows)
                {
                    AssetMutationViewModel vn = new AssetMutationViewModel();

                    vn.asset_id = int.Parse(row["asset_id"].ToString());
                    vn.asset_number = row["asset_number"].ToString();
                    vn.asset_name = row["asset_name"].ToString();
                    vn.asset_receipt_date = DateTime.Parse(row["asset_receipt_date"].ToString());

                    vn.currency_code = row["currency_code"].ToString();
                    vn.asset_original_value = decimal.Parse(row["asset_original_value"].ToString());
                    vn.currency_kurs = decimal.Parse(row["currency_kurs"].ToString());
                    vn.asset_book_value = decimal.Parse(row["asset_book_value"].ToString());

                    vn.current_location_id = int.Parse(row["current_location_id"].ToString());
                    vn.location_name = row["location_name"].ToString();
                    vn.current_department_id = int.Parse(row["current_department_id"].ToString());
                    vn.department_name = row["department_name"].ToString();
                    vn.current_employee_id = int.Parse(row["current_employee_id"].ToString());
                    vn.employee_name = row["employee_name"].ToString();

                    VendorAssetList.Add(vn);
                }
            }
            conn.Close();

            if (AssetID == 0)
            {
                return Json(new { data = VendorAssetList.ToList() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(VendorAssetList.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmployeeList(int location_id = 0, int department_id = 0)
        {
            var _employeelist = (from emp in db.ms_employee
                                 where emp.deleted_date == null && emp.fl_active == true

                                 join empdetail in db.ms_employee_detail on emp.employee_id equals empdetail.employee_id
                                 where empdetail.deleted_date == null && empdetail.fl_active == true
                                        && empdetail.location_id == location_id && empdetail.department_id == department_id
                                        && empdetail.company_id == UserProfile.company_id
                                 select new AssetMutationViewModel
                                 {
                                     employee_id = emp.employee_id,
                                     employee_name = emp.employee_name
                                 }).ToList();

            return Json(_employeelist, JsonRequestBehavior.AllowGet);
        }

        #region Gadipke
        [HttpPost]
        public JsonResult Save()
        {
            #region Variable Declaration
            string mode;

            int AssetID, ToEmployeeID, ToDeptID, ToLocID;

            #endregion

            #region Variable Value Assign

            mode = Request.Form["mode"].ToString();

            int.TryParse(Request.Form["asset_id"], out AssetID);

            //FROM
            //int.TryParse(Request.Form["curLocation"], out ToLocID);
            //int.TryParse(Request.Form["curDepartment"], out ToDeptID);
            //int.TryParse(Request.Form["curEmployee"], out ToEmployeeID);

            //TO
            int.TryParse(Request.Form["location"], out ToLocID);
            int.TryParse(Request.Form["department"], out ToDeptID);
            int.TryParse(Request.Form["employee"], out ToEmployeeID);

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            #endregion

            var conn = new SqlConnection(constring);

            var cmd = new SqlCommand("Insert Mutation", conn);
            //cmd.CommandText = "Exec SP_INSERT_TR_MUTATION_REQUEST @asset_id,@request_emp_id, @request_dept_id,@request_location_id ,@request_level_id ,@transfer_to_location_id ,@transfer_to_dept_id ,@transfer_to_emp_id ,@created_by ,@org_id ";
            cmd.CommandText = "Exec SP_INSERT_TR_MUTATION_REQUEST @asset_id,@transfer_to_location_id ,@transfer_to_dept_id ,@transfer_to_emp_id ,@created_by ,@org_id ";
            cmd.Parameters.AddWithValue("@asset_id", AssetID);

            //cmd.Parameters.AddWithValue("@request_emp_id", DepreciationTypeID);
            //cmd.Parameters.AddWithValue("@request_dept_id", OriValue);
            //cmd.Parameters.AddWithValue("@request_location_id", OriCurrencyID);
            //cmd.Parameters.AddWithValue("@request_level_id", CurrencyKurs);

            cmd.Parameters.AddWithValue("@transfer_to_location_id", ToLocID);
            cmd.Parameters.AddWithValue("@transfer_to_dept_id", ToDeptID);
            cmd.Parameters.AddWithValue("@transfer_to_emp_id", ToEmployeeID);

            cmd.Parameters.AddWithValue("@created_by", UserProfile.UserId);
            cmd.Parameters.AddWithValue("@org_id", UserProfile.OrgId);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Json("Insert Mutation Request Success", JsonRequestBehavior.AllowGet);
        }
        #endregion

        private string MutationNumberNew(int? asset_id)
        {
            tr_asset_registration tr_asset = db.tr_asset_registration.Find(asset_id);
            ms_asmin_company company = db.ms_asmin_company.Find(tr_asset.company_id);
            ms_department department = db.ms_department.Find(tr_asset.department_id);
            ms_asset_category category = db.ms_asset_category.Find(tr_asset.category_id);
            string code = string.Empty;
            code += "M";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult request([Bind(Include = "asset_id, asset_number, asset_name, current_location_id, location_name, current_department_id, department_name, current_employee_id, employee_name, transfer_to_location_id, transfer_to_dept_id, transfer_to_emp_id, asset_original_value")] AssetMutationViewModel mutation_req)
        {
            //Cek apakah disposal sudah di proses sebelumnya (kecuali reject)
            var _MutationExist = (from dr in db.tr_mutation_request
                                  where (dr.fl_active == true && dr.deleted_date == null) && dr.asset_id == mutation_req.asset_id
                                  select dr).ToList<tr_mutation_request>();
            if (_MutationExist.Count > 0)
            {
                ModelState.AddModelError("asset_number", "Asset Already Exists in Disposal Request Data.");
            }

            //input data request disposal with transaction
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Save Request Disposal
                        tr_mutation_request mutation_request = new tr_mutation_request();
                        mutation_request.request_code = MutationNumberNew(mutation_req.asset_id);
                        mutation_request.asset_id = mutation_req.asset_id;

                        mutation_request.request_date = DateTime.Now;
                        //mutation_request.request_location_id = UserProfile.location_id;
                        //mutation_request.request_dept_id = UserProfile.department_id;
                        //mutation_request.request_emp_id = UserProfile.employee_id;
                        mutation_request.request_location_id = mutation_req.current_location_id;
                        mutation_request.request_dept_id = mutation_req.current_department_id;
                        mutation_request.request_emp_id = mutation_req.current_employee_id;
                        mutation_request.request_status = 1;
                        mutation_request.transfer_to_location_id = mutation_req.transfer_to_location_id;
                        mutation_request.transfer_to_dept_id = mutation_req.transfer_to_dept_id;
                        mutation_request.transfer_to_emp_id = mutation_req.transfer_to_emp_id;
                        mutation_request.fl_active = true;
                        mutation_request.created_date = DateTime.Now;
                        mutation_request.created_by = UserProfile.UserId;
                        mutation_request.updated_date = DateTime.Now;
                        mutation_request.updated_by = UserProfile.UserId;
                        mutation_request.deleted_date = null;
                        mutation_request.deleted_by = null;
                        mutation_request.org_id = UserProfile.OrgId;

                        mutation_request = db.tr_mutation_request.Add(mutation_request);
                        db.SaveChanges();
                        var x = UserProfile.OrgId;
                        //Save Approval List Mutation Untuk Dept Head
                        //Hendy 22 Feb 2020
                        var _qry = (from sa in db.sy_ref_approval_level
                                    where sa.asset_reg_location_id == mutation_req.current_location_id && sa.job_level_id == 2

                                    join a in db.ms_job_level on sa.job_level_id equals a.job_level_id
                                    where (a.fl_active == true && a.deleted_date == null)

                                    join b in db.ms_employee_detail on a.job_level_id equals b.job_level_id
                                    where (b.fl_active == true && b.deleted_date == null
                                            //&& b.department_id == UserProfile.department_id && b.org_id == UserProfile.OrgId)
                                            && b.department_id == mutation_req.current_department_id && b.company_id == UserProfile.company_id)

                                    join c in db.ms_employee on b.employee_id equals c.employee_id

                                    orderby sa.order_no ascending
                                    select new AssetMutationViewModel()
                                    {
                                        //request_location_id = b.loca
                                        request_dept_id = b.department_id,
                                        request_emp_id = b.employee_id,
                                        request_level_id = a.job_level_id,
                                        current_employee_id = c.employee_id,
                                        employee_email = c.employee_email,
                                        employee_name = c.employee_name,
                                        ip_address = c.ip_address
                                    }).ToList<AssetMutationViewModel>();

                        if (_qry != null)
                        {
                            foreach (AssetMutationViewModel refApproval in _qry)
                            {
                                tr_mutation_approval mutation_approval = new tr_mutation_approval();
                                mutation_approval.request_id = mutation_request.request_id;
                                mutation_approval.approval_date = null;
                                mutation_approval.approval_location_id = /*refApproval.request_location_id*/ 0;
                                mutation_approval.approval_dept_id = refApproval.request_dept_id;
                                mutation_approval.approval_employee_id = refApproval.request_emp_id;
                                mutation_approval.approval_level_id = refApproval.request_level_id;
                                mutation_approval.approval_status_id = 1;//waiting approval
                                mutation_approval.approval_noted = "";
                                mutation_approval.fl_active = true;
                                mutation_approval.created_date = DateTime.Now;
                                mutation_approval.created_by = UserProfile.UserId;
                                mutation_approval.updated_date = DateTime.Now;
                                mutation_approval.updated_by = UserProfile.UserId;
                                mutation_approval.deleted_date = null;
                                mutation_approval.deteled_by = null;
                                mutation_approval.org_id = UserProfile.OrgId;
                                mutation_approval = db.tr_mutation_approval.Add(mutation_approval);
                                db.SaveChanges();
                            }
                        }
                        //Check KTT

                        //check range approval
                        decimal ktt_asset_value = Convert.ToDecimal(mutation_req.asset_original_value);
                        bool isKTTApproval = false;
                        ms_approval_range range_ktt = db.ms_approval_range.Where(r => r.range_code == "M_lv2").FirstOrDefault();
                        if (range_ktt != null)
                        {
                            isKTTApproval = (range_ktt.range_min <= ktt_asset_value);
                        }

                        if (isKTTApproval)
                        {
                            var x_ktt = UserProfile.OrgId;
                            //Save Approval List Mutation Untuk KTT
                            //Hendy 22 Feb 2020
                            var _qry_ktt = (from sa in db.sy_ref_approval_level
                                            where sa.asset_reg_location_id == mutation_req.current_location_id
                                            && sa.job_level_id == 3

                                            join a in db.ms_job_level on sa.job_level_id equals a.job_level_id
                                            where (a.fl_active == true && a.deleted_date == null)

                                            join b in db.ms_employee_detail on a.job_level_id equals b.job_level_id
                                            where (b.fl_active == true && b.deleted_date == null)
                                            && b.company_id == UserProfile.company_id
                                            && b.asset_reg_location_id == UserProfile.asset_reg_location_id

                                            join c in db.ms_employee on b.employee_id equals c.employee_id


                                            orderby sa.order_no ascending
                                            select new AssetMutationViewModel()
                                            {
                                                //request_location_id = b.loca
                                                request_dept_id = b.department_id,
                                                request_emp_id = b.employee_id,
                                                request_level_id = a.job_level_id,
                                                current_employee_id = c.employee_id,
                                                employee_email = c.employee_email,
                                                employee_name = c.employee_name,
                                                ip_address = c.ip_address
                                            }).ToList<AssetMutationViewModel>();

                            if (_qry_ktt != null)
                            {
                                foreach (AssetMutationViewModel refApproval in _qry_ktt)
                                {
                                    tr_mutation_approval mutation_approval = new tr_mutation_approval();
                                    mutation_approval.request_id = mutation_request.request_id;
                                    mutation_approval.approval_date = null;
                                    mutation_approval.approval_location_id = /*refApproval.request_location_id*/ 0;
                                    mutation_approval.approval_dept_id = refApproval.request_dept_id;
                                    mutation_approval.approval_employee_id = refApproval.request_emp_id;
                                    mutation_approval.approval_level_id = refApproval.request_level_id;
                                    mutation_approval.approval_status_id = 1;//waiting approval
                                    mutation_approval.approval_noted = "";
                                    mutation_approval.fl_active = true;
                                    mutation_approval.created_date = DateTime.Now;
                                    mutation_approval.created_by = UserProfile.UserId;
                                    mutation_approval.updated_date = DateTime.Now;
                                    mutation_approval.updated_by = UserProfile.UserId;
                                    mutation_approval.deleted_date = null;
                                    mutation_approval.deteled_by = null;
                                    mutation_approval.org_id = UserProfile.OrgId;
                                    mutation_approval = db.tr_mutation_approval.Add(mutation_approval);
                                    db.SaveChanges();
                                }
                            }
                        }

                        decimal asset_value = Convert.ToDecimal(mutation_req.asset_original_value);
                        bool isBODApproval = false;
                        ms_approval_range range = db.ms_approval_range.Where(r => r.range_code == "M_BOD").FirstOrDefault();
                        if (range != null)
                        {
                            isBODApproval = (range.range_min <= asset_value);
                        }

                        if (isBODApproval)
                        {
                            //Approval BOD berdasarkan data ms_department >> employee_bod_id
                            ms_department dept = db.ms_department.Find(UserProfile.department_id);

                            if (dept != null)
                            {
                                var _qry_bod = (from sa in db.sy_ref_approval_level
                                                where sa.asset_reg_location_id == mutation_req.current_location_id && sa.job_level_id == 9

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
                                                select new AssetMutationViewModel()
                                                {
                                                    //request_location_id = b.loca
                                                    request_dept_id = b.department_id,
                                                    request_emp_id = d.employee_bod_id,
                                                    request_level_id = a.job_level_id,
                                                    current_employee_id = c.employee_id,
                                                    employee_email = c.employee_email,
                                                    employee_name = c.employee_name,
                                                    ip_address = c.ip_address
                                                }).ToList<AssetMutationViewModel>();

                                if (_qry_bod != null)
                                {
                                    foreach (AssetMutationViewModel refApproval in _qry_bod)
                                    {
                                        tr_mutation_approval mutation_approval = new tr_mutation_approval();
                                        mutation_approval.request_id = mutation_request.request_id;
                                        mutation_approval.approval_date = null;
                                        mutation_approval.approval_location_id = /*refApproval.request_location_id*/ 0;
                                        mutation_approval.approval_dept_id = refApproval.request_dept_id;
                                        mutation_approval.approval_employee_id = refApproval.request_emp_id;
                                        mutation_approval.approval_level_id = refApproval.request_level_id;
                                        mutation_approval.approval_status_id = 1;//waiting approval
                                        mutation_approval.approval_noted = "";
                                        mutation_approval.fl_active = true;
                                        mutation_approval.created_date = DateTime.Now;
                                        mutation_approval.created_by = UserProfile.UserId;
                                        mutation_approval.updated_date = DateTime.Now;
                                        mutation_approval.updated_by = UserProfile.UserId;
                                        mutation_approval.deleted_date = null;
                                        mutation_approval.deteled_by = null;
                                        mutation_approval.org_id = UserProfile.OrgId;
                                        mutation_approval = db.tr_mutation_approval.Add(mutation_approval);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                        #region "kirim email ke approval level 1"
                        sy_email_log sy_email_log = new sy_email_log();
                        sy_email_log.elog_to = _qry.FirstOrDefault().employee_email;
                        sy_email_log.elog_subject = string.Format("Asset Mutation Need Approval");
                        sy_email_log.elog_template = "EMAIL_TEMPLATE_02";

                        #region "body mail"
                        var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_02"));
                        string strBodyMail = _bodymail.FirstOrDefault().app_value;
                        strBodyMail = strBodyMail.Replace("[to]", _qry.FirstOrDefault().employee_name);
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
                        #endregion

                        #region "Save Sy_Message_notification ke approval"
                        int empid = Convert.ToInt32(_qry.FirstOrDefault().current_employee_id);
                        ms_user msuser = (from m in db.ms_user
                                          where m.employee_id == empid
                                          select m).FirstOrDefault();

                        sy_message_notification msg = new sy_message_notification();
                        msg.notif_group = "BALOON_RECEIPT_03";
                        msg.notify_user = msuser.user_name;
                        msg.notify_ip = _qry.FirstOrDefault().ip_address;
                        msg.notify_message = "Ada permintaan approval untuk asset mutasi.";
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
                        string msgErr = string.Format("Error occured, records rolledback. {0}", ex.Message);
                        if (ex.InnerException != null)
                            msgErr += string.Format(" Inner Exception: {0}", ex.InnerException.Message);
                        ModelState.AddModelError("", msgErr);
                    }
                }

            }

            #region "for dropdown Employee"
            //var _employeelist = from s in db.ms_disposal_type
            //                    select s;
            var _employeelist = db.ms_employee.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                t => new
                {
                    t.employee_id,
                    t.employee_nik,
                    t.employee_name
                }).ToList();
            SelectList itemsType = new SelectList(_employeelist, "employee_id", "employee_name");
            ViewBag.transfer_to_emp_id = itemsType;
            #endregion

            #region "for dropdown Department"
            var _departmentlist = db.ms_department.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                 t => new
                 {
                     t.department_id,
                     t.department_code,
                     t.department_name
                 }).ToList();
            SelectList itemsType2 = new SelectList(_departmentlist, "department_id", "department_name");
            ViewBag.transfer_to_dept_id = itemsType2;
            #endregion

            #region "for dropdown Location"
            var _locationlist = db.ms_asset_location.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                t => new
                {
                    t.location_id,
                    t.location_code,
                    t.location_name
                }).ToList();
            SelectList itemsType3 = new SelectList(_locationlist, "location_id", "location_name");
            ViewBag.transfer_to_location_id = itemsType3;
            #endregion

            return View(mutation_req);
        }

        // GET: Disposal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_mutation_request mutationreq = db.tr_mutation_request.Find(id);
            if (mutationreq == null)
            {
                return HttpNotFound("Assset not found.");
            }

            var _qry = (from t in db.tr_mutation_request
                        where t.fl_active == true && t.deleted_date == null && t.request_id == id
                        //&& t.org_id == UserProfile.OrgId && t.request_dept_id == UserProfile.department_id

                        join a in db.tr_depreciation on t.asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_currency on a.asset_original_currency_id equals b.currency_id
                        where (b.fl_active == true && b.deleted_date == null)

                        //join c in db.ms_asmin_company on t.org_id equals c.company_id
                        //where (c.fl_active == true && c.deleted_date == null)
                        join c in db.tr_asset_registration on t.asset_id equals c.asset_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_department on c.current_department_id equals d.department_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_employee on c.current_employee_id equals e.employee_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_asset_location on c.current_location_id equals f.location_id
                        where (f.fl_active == true && f.deleted_date == null)

                        join g in db.ms_department on t.transfer_to_dept_id equals g.department_id
                        where (g.fl_active == true && g.deleted_date == null)

                        join h in db.ms_employee on t.transfer_to_emp_id equals h.employee_id
                        where (h.fl_active == true && h.deleted_date == null)

                        join i in db.ms_asset_location on t.transfer_to_location_id equals i.location_id
                        where (i.fl_active == true && i.deleted_date == null)

                        select new AssetMutationViewModel()
                        {
                            request_code = t.request_code,
                            asset_id = t.asset_id,
                            asset_number = c.asset_number,
                            asset_name = c.asset_name,
                            asset_receipt_date = c.asset_receipt_date,
                            currency_code = b.currency_code,
                            asset_book_value = a.asset_book_value,
                            currency_kurs = a.usd_kurs,
                            asset_original_value = a.asset_original_value,
                            current_location_id = c.current_location_id,
                            current_department_id = c.current_department_id,
                            current_employee_id = c.current_employee_id,
                            location_name = f.location_name,
                            department_name = d.department_name,
                            employee_name = e.employee_name,
                            transfer_to_dept_id = t.transfer_to_dept_id,
                            transfer_to_dept_name = g.department_name,
                            transfer_to_location_id = t.transfer_to_location_id,
                            transfer_to_location_name = i.location_name,
                            transfer_to_emp_id = t.transfer_to_emp_id,
                            transfer_to_emp_name = h.employee_name

                        }).ToList<AssetMutationViewModel>();

            //Data Approval view
            var _qrylist = (from da in db.tr_mutation_approval
                            where (da.fl_active == true && da.deleted_date == null && da.request_id == id)

                            join a in db.ms_employee on da.approval_employee_id equals a.employee_id
                            where (a.fl_active == true && a.deleted_date == null)

                            join b in db.ms_job_level on da.approval_level_id equals b.job_level_id
                            where (b.fl_active == true && b.deleted_date == null)

                            join c in db.ms_request_status on da.approval_status_id equals c.request_status_id

                            //join d in db.ms_disposal_type on da.approval_suggestion_id equals d.disposal_type_id
                            //into d_temp
                            //from subd in d_temp.DefaultIfEmpty()

                            select new AssetMutationViewModel()
                            {
                                approval_employee_name = a.employee_name,
                                approval_level_name = b.job_level_name,
                                approval_status_Name = c.request_status_name,
                                approval_date = da.approval_date
                            }).ToList<AssetMutationViewModel>();

            var tuple = new Tuple<AssetMutationViewModel, IEnumerable<AssetMutationViewModel>>(_qry.First<AssetMutationViewModel>(), _qrylist);

            return View(tuple);
        }
    }
}