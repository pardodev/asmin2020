namespace ASM_UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using ASM_UI.Models;

    [Authorize]
    public class asset_locationController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_location
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAssetLocation()
        {

            var CountryList = db.ms_asset_location.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                t => new
                {
                    t.location_id,
                    t.location_code,
                    t.location_name
                }).ToList();
            return Json(CountryList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegisterLocation()
        {
            var RegisterLocationList = db.ms_asset_register_location.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                t => new
                {
                    t.asset_reg_location_id,
                    t.asset_reg_location_name,
                }).ToList();
            return Json(RegisterLocationList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            //ms_asset_location AssetRegLoc = new ms_asset_location();
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var AssetLocList = from loc in db.ms_asset_location
                               join reg in db.ms_asset_register_location 
                                    on loc.asset_reg_location_id equals reg.asset_reg_location_id
                               where (loc.deleted_date == null)

                               join u in db.ms_user on loc.updated_by equals u.user_id
                               into t_joined
                               from row_join in t_joined.DefaultIfEmpty()
                               from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                               select new
                               {
                                   loc.location_id,
                                   loc.asset_reg_location_id,
                                   reg.asset_reg_location_name,
                                   loc.location_code,
                                   loc.location_name,
                                   loc.fl_active,
                                   rec_isactive = (loc.fl_active == true) ? "Yes" : "No",
                                   loc.created_by,
                                   loc.created_date,
                                   updated_by = (usr == null) ? string.Empty : usr.user_name,
                                   updated_date = loc.updated_date,
                                   loc.deleted_by,
                                   loc.deleted_date
                               };
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "location_code":
                        AssetLocList = AssetLocList.Where(t => t.location_code.Contains(searchString));
                        break;
                    case "location_name":
                        AssetLocList = AssetLocList.Where(t => t.location_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = AssetLocList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                AssetLocList = AssetLocList.OrderByDescending(t => t.location_name);
                AssetLocList = AssetLocList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                AssetLocList = AssetLocList.OrderBy(t => t.location_name);
                AssetLocList = AssetLocList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = AssetLocList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudAssetLoc()
        {
            if (UserProfile.UserId != 0)
            {
                if (Request.Form["oper"] == "add")
                {
                    //prepare for insert data
                    ms_asset_location ms_loc = new ms_asset_location();
                    ms_loc.asset_reg_location_id = Convert.ToInt32(Request.Form["asset_reg_location_name"]);
                    ms_loc.location_code = Request.Form["location_code"];
                    ms_loc.location_name = Request.Form["location_name"];
                    ms_loc.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                    //ms_loc.fl_active = true;

                    ms_loc.created_by = UserProfile.UserId;
                    ms_loc.created_date = DateTime.Now;
                    ms_loc.updated_by = UserProfile.UserId;
                    ms_loc.updated_date = DateTime.Now;
                    ms_loc.org_id = UserProfile.OrgId;
                    ms_loc.deleted_by = null;
                    ms_loc.deleted_date = null;

                    //db.ms_asset_location.Add(ms_loc);
                    db.Entry(ms_loc).State = EntityState.Added;
                    db.SaveChanges();
                    return Json("Insert", JsonRequestBehavior.AllowGet);
                }
                else if (Request.Form["oper"] == "edit")
                {
                    if (IsNumeric(Request.Form["location_id"].ToString()))
                    {
                        //prepare for update data
                        int id = Convert.ToInt32(Request.Form["location_id"]);
                        ms_asset_location ms_loc = db.ms_asset_location.Find(id);
                        ms_loc.asset_reg_location_id = Convert.ToInt32(Request.Form["asset_reg_location_name"]);
                        ms_loc.location_code = Request.Form["location_code"];
                        ms_loc.location_name = Request.Form["location_name"];
                        ms_loc.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                        //ms_loc.fl_active = true;

                        ms_loc.updated_by = UserProfile.UserId;
                        ms_loc.updated_date = DateTime.Now;
                        ms_loc.org_id = UserProfile.OrgId;
                        ms_loc.deleted_by = null;
                        ms_loc.deleted_date = null;

                        db.Entry(ms_loc).State = EntityState.Modified;
                        db.SaveChanges();

                        return Json("Update", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //prepare for insert data
                        ms_asset_location ms_loc = new ms_asset_location();
                        ms_loc.location_code = Request.Form["location_code"];
                        ms_loc.location_name = Request.Form["location_name"];
                        ms_loc.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                        //ms_loc.fl_active = true;

                        ms_loc.created_by = UserProfile.UserId;
                        ms_loc.created_date = DateTime.Now;
                        ms_loc.updated_by = UserProfile.UserId;
                        ms_loc.updated_date = DateTime.Now;
                        ms_loc.org_id = UserProfile.OrgId;
                        ms_loc.deleted_by = null;
                        ms_loc.deleted_date = null;

                        //db.ms_asset_location.Add(ms_loc);
                        db.Entry(ms_loc).State = EntityState.Added;
                        db.SaveChanges();
                        return Json("Insert", JsonRequestBehavior.AllowGet);
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
                            ms_asset_location ms_loc = db.ms_asset_location.Find(id);

                            ms_loc.fl_active = false;
                            ms_loc.deleted_by = UserProfile.UserId; //userid
                            ms_loc.deleted_date = DateTime.Now;
                            db.Entry(ms_loc).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return Json("Delete", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json("Session", JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
