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
    public class vendorController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        public class VendorAsset
        {
            public int asset_id { get; set; }
            public string asset_number { get; set; }
            public string asset_name { get; set; }
            public decimal asset_price_fiskal { get; set; }
            public decimal asset_price_market { get; set; }
            public string usage_life_time_fiskal { get; set; }
            public string usage_life_time_market { get; set; }
        }

        // GET: vendor
        public ActionResult Index()
        {
            //var ms_vendor = db.ms_vendor.Include(m => m.ms_country);
            //return View(ms_vendor.ToList());
            return View();
        }

        public ActionResult Modal()
        {
            return PartialView();
        }
        public ActionResult ModalAsset()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var VendorList = (from ven in db.ms_vendor
                                 join cty in db.ms_country
                                 on ven.country_id equals cty.country_id
                                 where (ven.deleted_date == null)
                                 join u in db.ms_user on ven.updated_by equals u.user_id
                                 into t_joined
                                 from row_join in t_joined.DefaultIfEmpty()
                                 from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                                 select new
                                 {
                                     ven.vendor_id,
                                     ven.vendor_code,
                                     ven.vendor_name,
                                     ven.vendor_address,
                                     ven.country_id,
                                     cty.country_name,
                                     ven.vendor_phone,
                                     ven.vendor_mail,
                                     ven.vendor_cp_name,
                                     ven.vendor_cp_phone,
                                     ven.vendor_cp_mail,
                                     ven.vendor_description,
                                     ven.fl_active,
                                     rec_isactive = (ven.fl_active == true) ? "Yes" : "No",
                                     ven.created_by,
                                     ven.created_date,
                                     updated_by = (usr == null) ? string.Empty : usr.user_name,
                                     ven.updated_date,
                                     ven.deleted_by,
                                     ven.deleted_date,
                                 });
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "vendor_code":
                        VendorList = VendorList.Where(ven => ven.vendor_code.Contains(searchString));
                        break;
                    case "vendor_name":
                        VendorList = VendorList.Where(ven => ven.vendor_name.Contains(searchString));
                        break;
                    case "vendor_address":
                        VendorList = VendorList.Where(ven => ven.vendor_address.Contains(searchString));
                        break;
                    case "insntry_name":
                        VendorList = VendorList.Where(cty => cty.country_name.Contains(searchString));
                        break;
                    case "vendor_phone":
                        VendorList = VendorList.Where(ven => ven.vendor_phone.Contains(searchString));
                        break;
                    case "vendor_mail":
                        VendorList = VendorList.Where(ven => ven.vendor_mail.Contains(searchString));
                        break;
                    case "vendor_cp_name":
                        VendorList = VendorList.Where(ven => ven.vendor_cp_name.Contains(searchString));
                        break;
                    case "vendor_cp_phone":
                        VendorList = VendorList.Where(ven => ven.vendor_cp_phone.Contains(searchString));
                        break;
                    case "vendor_cp_mail":
                        VendorList = VendorList.Where(ven => ven.vendor_cp_mail.Contains(searchString));
                        break;
                    case "vendor_description":
                        VendorList = VendorList.Where(ven => ven.vendor_description.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = VendorList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                VendorList = VendorList.OrderByDescending(t => t.vendor_name);
                VendorList = VendorList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                VendorList = VendorList.OrderBy(t => t.vendor_name);
                VendorList = VendorList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = VendorList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public ActionResult SaveData(string NewName)
        {
            int idC;
            int.TryParse(Request.Form["vendorID"], out idC);

            ms_vendor ms_vendor = new ms_vendor();
            ms_vendor.vendor_id = idC;
            ms_vendor.vendor_code = Request.Form["vendorCode"];
            ms_vendor.vendor_name = Request.Form["vendorName"];
            ms_vendor.vendor_address = Request.Form["vendorAddress"];
            ms_vendor.country_id = Convert.ToInt32(Request.Form["Country"]);
            ms_vendor.vendor_phone = Request.Form["vendorPhone"];
            ms_vendor.vendor_mail = Request.Form["vendorMail"];
            ms_vendor.vendor_cp_name = Request.Form["vendorCPName"];
            ms_vendor.vendor_cp_phone = Request.Form["vendorCPPhone"];
            ms_vendor.vendor_cp_mail = Request.Form["vendorCPMail"];
            ms_vendor.vendor_description = Request.Form["vendorDescription"];
            ms_vendor.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
            ms_vendor.deleted_by = null;
            ms_vendor.deleted_date = null;

            if (idC == 0)
            {
                ms_vendor.created_by = UserProfile.UserId;
                ms_vendor.created_date = DateTime.Now;
                ms_vendor.org_id = UserProfile.OrgId;
                db.Entry(ms_vendor).State = EntityState.Added;
                db.SaveChanges();
                return Json("Insert", JsonRequestBehavior.AllowGet);
            }
            else
            {
                ms_vendor.updated_by = UserProfile.UserId;
                ms_vendor.updated_date = DateTime.Now;
                db.Entry(ms_vendor).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Update", JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult CrudVendor()
        {
            if (Request.Form["oper"] == "add")
            {
                //prepare for insert data
                ms_vendor ms_vendor = new ms_vendor();
                ms_vendor.vendor_code = Request.Form["vendor_code"];
                ms_vendor.vendor_name = Request.Form["vendor_name"];
                ms_vendor.vendor_address = Request.Form["vendor_address"];
                ms_vendor.country_id = Convert.ToInt32(Request.Form["country_id"]);
                ms_vendor.vendor_phone = Request.Form["vendor_phone"];
                ms_vendor.vendor_mail = Request.Form["vendor_mail"];
                ms_vendor.vendor_cp_name = Request.Form["vendor_cp_name"];
                ms_vendor.vendor_cp_phone = Request.Form["vendor_cp_phone"];
                ms_vendor.vendor_cp_mail = Request.Form["vendor_cp_mail"];
                ms_vendor.vendor_description = Request.Form["vendor_description"];
                ms_vendor.created_by = 1;
                ms_vendor.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                ms_vendor.created_date = DateTime.Now;
                ms_vendor.updated_by = 1;
                ms_vendor.updated_date = DateTime.Now;
                ms_vendor.org_id = 0;
                db.ms_vendor.Add(ms_vendor);
                db.SaveChanges();
                return Json("Insert Insurance Data Success!", JsonRequestBehavior.AllowGet);
            }
            else if (Request.Form["oper"] == "edit")
            {
                if (IsNumeric(Request.Form["vendor_id"].ToString()))
                {
                    //prepare for update data
                    int id = Convert.ToInt32(Request.Form["vendor_id"]);
                    ms_vendor ms_vendor = db.ms_vendor.Find(id);
                    ms_vendor.vendor_code = Request.Form["vendor_code"];
                    ms_vendor.vendor_name = Request.Form["vendor_name"];
                    ms_vendor.vendor_address = Request.Form["vendor_address"];
                    ms_vendor.country_id = Convert.ToInt32(Request.Form["country_id"]);
                    ms_vendor.vendor_phone = Request.Form["vendor_phone"];
                    ms_vendor.vendor_mail = Request.Form["vendor_mail"];
                    ms_vendor.vendor_cp_name = Request.Form["vendor_cp_name"];
                    ms_vendor.vendor_cp_phone = Request.Form["vendor_cp_phone"];
                    ms_vendor.vendor_cp_mail = Request.Form["vendor_cp_mail"];
                    ms_vendor.vendor_description = Request.Form["vendor_description"];
                    ms_vendor.updated_by = 1;
                    ms_vendor.updated_date = DateTime.Now;
                    db.SaveChanges();
                    return Json("Update Insurance Data Success!", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //prepare for insert data
                    ms_vendor ms_vendor = new ms_vendor();
                    ms_vendor.vendor_code = Request.Form["vendor_code"];
                    ms_vendor.vendor_name = Request.Form["vendor_name"];
                    ms_vendor.vendor_address = Request.Form["vendor_address"];
                    ms_vendor.country_id = Convert.ToInt32(Request.Form["country_id"]);
                    ms_vendor.vendor_phone = Request.Form["vendor_phone"];
                    ms_vendor.vendor_mail = Request.Form["vendor_mail"];
                    ms_vendor.vendor_cp_name = Request.Form["vendor_cp_name"];
                    ms_vendor.vendor_cp_phone = Request.Form["vendor_cp_phone"];
                    ms_vendor.vendor_cp_mail = Request.Form["vendor_cp_mail"];
                    ms_vendor.vendor_description = Request.Form["vendor_description"];
                    ms_vendor.created_by = 1;
                    ms_vendor.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                    ms_vendor.created_date = DateTime.Now;
                    ms_vendor.updated_by = 1;
                    ms_vendor.updated_date = DateTime.Now;
                    ms_vendor.org_id = 0;
                    db.ms_vendor.Add(ms_vendor);
                    db.SaveChanges();
                    return Json("Insert Insurance Data Success!", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (Request.Form["oper"] == "del")
                {
                    //for delete process
                    string ids = Request.Form["id"];
                    string[] values = ids.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        //prepare for soft delete data
                        int id = Convert.ToInt32(values[i]);
                        ms_vendor cou = db.ms_vendor.Find(id);
                        cou.deleted_by = 1;
                        cou.deleted_date = DateTime.Now;
                        db.SaveChanges();
                    }

                }
                return Json("Deleted Success!");
            }
        }

        public JsonResult GetCountry()
        {
            var CountryList = db.ms_country.Where(t => t.deleted_date == null).Select(
                t => new
                {
                    t.country_id,
                    t.country_name,
                }).ToList();
            return Json(CountryList, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetVendorByID(string id)
        {
            int idC;
            int.TryParse(id, out idC);
            var VendorList = (from ven in db.ms_vendor
                              join cty in db.ms_country
                              on ven.country_id equals cty.country_id
                              where (ven.vendor_id == idC)
                              select new
                              {
                                  ven.vendor_id,
                                  ven.vendor_code,
                                  ven.vendor_name,
                                  ven.vendor_address,
                                  ven.country_id,
                                  cty.country_name,
                                  ven.vendor_phone,
                                  ven.vendor_mail,
                                  ven.vendor_cp_name,
                                  ven.vendor_cp_phone,
                                  ven.vendor_cp_mail,
                                  ven.vendor_description,
                                  ven.fl_active,
                                  rec_isactive = (ven.fl_active == true) ? "Yes" : "No"
                              });
            return Json(VendorList.ToList(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAssetByVendor(string id)
        {
            int idC;
            int.TryParse(id, out idC);
            //var VendorList = (from asstReg in db.tr_asset_registration
            //                  join dept in db.tr_depreciation
            //                  on asstReg.asset_id equals dept.asset_id
            //                  where (asstReg.vendor_id == idC)
            //                  select new
            //                  {
            //                      asstReg.asset_number
            //                      , asstReg.asset_name
            //                      , AssetValue = 0 
            //                      , AssetLifeTime = ""
            //                  });
            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);
            
            var cmd = new SqlCommand("getvendorasset", conn);
            cmd.CommandText = "select * from dbo.FN_GET_ASSET_VALUE_BY_DEPTYPE(@vendor_id,@asset_id,@depreciation_type_id)";
            cmd.Parameters.AddWithValue("@vendor_id", idC);
            cmd.Parameters.AddWithValue("@asset_id", 0);
            cmd.Parameters.AddWithValue("@depreciation_type_id", 1);

            List<VendorAssetViewModel> VendorAssetList = new List<VendorAssetViewModel>();

            conn.Open();
            //var vendlist = cmd.ExecuteNonQuery();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(dr);
                
                foreach (DataRow row in tb.Rows)
                {
                    VendorAssetViewModel vn = new VendorAssetViewModel();

                    vn.asset_number = row["asset_number"].ToString();
                    vn.asset_name = row["asset_name"].ToString();
                    vn.asset_price_fiskal = decimal.Parse(row["asset_price_fiskal"].ToString());
                    vn.asset_price_market = decimal.Parse(row["asset_price_market"].ToString());
                    vn.usage_life_time_fiskal = row["usage_life_time_fiskal"].ToString();
                    vn.usage_life_time_market = row["usage_life_time_market"].ToString();

                    VendorAssetList.Add(vn);
                }
                
            }

            conn.Close();
            
            return Json(new { data = VendorAssetList.ToList() }, JsonRequestBehavior.AllowGet);
        }
    }
}
