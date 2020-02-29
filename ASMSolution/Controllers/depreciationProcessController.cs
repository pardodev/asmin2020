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

namespace ASM_UI.Controllers
{
    [Authorize]
    public class depreciationProcessController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: depreciationProcess
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var ListDepre = from a in db.tr_depreciation
                            where (a.fl_active == true) && (a.org_id == UserProfile.OrgId) && (a.fl_depreciation == false || a.fl_depreciation == null)
                            join b in db.tr_asset_registration on a.asset_id equals b.asset_id
                            join c in db.ms_department on b.department_id equals c.department_id
                            join d in db.ms_employee on b.employee_id equals d.employee_id
                            select new
                            {
                                a.depreciation_id,
                                a.asset_id,
                                a.depreciation_type_id,
                                b.asset_number,
                                b.asset_name,
                                c.department_code,
                                d.employee_name,
                                b.asset_receipt_date
                            };

            //var Category = from cat in db.ms_asset_category
            //               where (cat.deleted_date == null)
            //               join u in db.ms_user on cat.updated_by equals u.user_id
            //               into t_joined
            //               from row_join in t_joined.DefaultIfEmpty()
            //               from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
            //               select new
            //               {
            //                   cat.category_id,
            //                   cat.category_code,
            //                   cat.category_name,
            //                   cat.fl_active,
            //                   rec_isactive = (cat.fl_active == true) ? "Yes" : "No",
            //                   cat.created_by,
            //                   cat.created_date,
            //                   updated_by = (usr == null) ? string.Empty : usr.user_name,
            //                   updated_date = cat.updated_date,
            //                   cat.deleted_by,
            //                   cat.deleted_date
            //               };
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "asset_number":
                        ListDepre = ListDepre.Where(t => t.asset_number.Contains(searchString));
                        break;
                    case "asset_name":
                        ListDepre = ListDepre.Where(t => t.asset_name.Contains(searchString));
                        break;
                    case "department_code":
                        ListDepre = ListDepre.Where(t => t.department_code.Contains(searchString));
                        break;
                    case "employee_name":
                        ListDepre = ListDepre.Where(t => t.employee_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = ListDepre.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                ListDepre = ListDepre.OrderByDescending(t => t.asset_name);
                ListDepre = ListDepre.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                ListDepre = ListDepre.OrderBy(t => t.asset_name);
                ListDepre = ListDepre.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = ListDepre
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProcessAllCheckedData(string id)
        {
            //string[] values = id.Split(',');
            if (id != null)
            {
                int CurrUser, OrgID;

                CurrUser = UserProfile.UserId;
                OrgID = UserProfile.OrgId;

                string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
                var conn = new SqlConnection(constring);
                var cmd = new SqlCommand("ProcessDepreciation", conn);
                cmd.CommandText = "Exec SP_INSERT_TR_DEPRECIATION_PROCESS_BY_BATCH @batch_depreciation_id ,@created_by ,@orgID ";

                cmd.Parameters.AddWithValue("@batch_depreciation_id", id);
                cmd.Parameters.AddWithValue("@created_by", CurrUser);
                cmd.Parameters.AddWithValue("@orgID", OrgID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                return Json("Process Data Success");
            }
            else
            {
                return Json("No Data Selected");
            }
        }
        [HttpPost]
        public JsonResult ReSetAllCheckedData(string id)
        {
            //string[] values = id.Split(',');
            if (id != null)
            {
                int CurrUser, OrgID;

                CurrUser = UserProfile.UserId;
                OrgID = UserProfile.OrgId;

                string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
                var conn = new SqlConnection(constring);
                var cmd = new SqlCommand("ReSetDepreciation", conn);
                cmd.CommandText = "Exec SP_DELETE_ASSET_DEPRECIATION_BY_BATCH @batch_depreciation_id ,@created_by ,@orgID ";

                cmd.Parameters.AddWithValue("@batch_depreciation_id", id);
                cmd.Parameters.AddWithValue("@created_by", CurrUser);
                cmd.Parameters.AddWithValue("@orgID", OrgID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                return Json("Process Data Success");
            }
            else
            {
                return Json("No Data Selected");
            }
        }
    }
}