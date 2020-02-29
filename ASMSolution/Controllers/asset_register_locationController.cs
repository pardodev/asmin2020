using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class asset_register_locationController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_register_location
        public ActionResult Index()
        {
            return View();
            //db.ms_asset_register_location.ToList()
        }

        //GET : /asset_register_location/list
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            //ms_asset_register_location AssetRegLoc = new ms_asset_register_location();
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var AssetRegLocList = from locReg in db.ms_asset_register_location
                                  where (locReg.deleted_date == null)
                                  join u in db.ms_user on locReg.updated_by equals u.user_id
                                  into t_joined
                                  from row_join in t_joined.DefaultIfEmpty()
                                  from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                                  select new
                                  {
                                      locReg.asset_reg_location_id,
                                      locReg.asset_reg_location_code,
                                      locReg.asset_reg_location_name,
                                      locReg.fl_active,
                                      rec_isactive = (locReg.fl_active == true) ? "Yes" : "No",
                                      locReg.created_by,
                                      locReg.created_date,
                                      updated_by = (usr == null) ? string.Empty : usr.user_name,
                                      updated_date = locReg.updated_date,
                                      locReg.deleted_by,
                                      locReg.deleted_date
                                  };
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "asset_reg_location_code":
                        AssetRegLocList = AssetRegLocList.Where(t => t.asset_reg_location_code.Contains(searchString));
                        break;
                    case "asset_reg_location_name":
                        AssetRegLocList = AssetRegLocList.Where(t => t.asset_reg_location_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = AssetRegLocList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                AssetRegLocList = AssetRegLocList.OrderByDescending(t => t.asset_reg_location_name);
                AssetRegLocList = AssetRegLocList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                AssetRegLocList = AssetRegLocList.OrderBy(t => t.asset_reg_location_name);
                AssetRegLocList = AssetRegLocList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = AssetRegLocList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudAssetRegLoc()
        {
            if (UserProfile.UserId != 0)
            {
                if (Request.Form["oper"] == "add")
                {
                    //prepare for insert data
                    ms_asset_register_location ms_asset_reg_loc = new ms_asset_register_location();
                    ms_asset_reg_loc.asset_reg_location_code = Request.Form["asset_reg_location_code"];
                    ms_asset_reg_loc.asset_reg_location_name = Request.Form["asset_reg_location_name"];
                    ms_asset_reg_loc.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                    ms_asset_reg_loc.created_by = UserProfile.UserId;
                    ms_asset_reg_loc.created_date = DateTime.Now;
                    ms_asset_reg_loc.updated_by = UserProfile.UserId;
                    ms_asset_reg_loc.updated_date = DateTime.Now;
                    ms_asset_reg_loc.org_id = UserProfile.OrgId;
                    ms_asset_reg_loc.deleted_by = null;
                    ms_asset_reg_loc.deleted_date = null;

                    db.Entry(ms_asset_reg_loc).State = EntityState.Added;

                    db.SaveChanges();
                    return Json("Insert", JsonRequestBehavior.AllowGet);
                }
                else if (Request.Form["oper"] == "edit")
                {
                    if (IsNumeric(Request.Form["asset_reg_location_id"].ToString()))
                    {
                        //prepare for update data
                        int id = Convert.ToInt32(Request.Form["asset_reg_location_id"]);
                        ms_asset_register_location ms_asset_reg_loc = db.ms_asset_register_location.Find(id);
                        ms_asset_reg_loc.asset_reg_location_code = Request.Form["asset_reg_location_code"];
                        ms_asset_reg_loc.asset_reg_location_name = Request.Form["asset_reg_location_name"];
                        ms_asset_reg_loc.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        ms_asset_reg_loc.updated_by = UserProfile.UserId;
                        ms_asset_reg_loc.updated_date = DateTime.Now;

                        db.Entry(ms_asset_reg_loc).State = EntityState.Modified;
                        db.SaveChanges();

                        return Json("Update", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //prepare for insert data
                        ms_asset_register_location ms_asset_reg_loc = new ms_asset_register_location();
                        ms_asset_reg_loc.asset_reg_location_code = Request.Form["asset_reg_location_code"];
                        ms_asset_reg_loc.asset_reg_location_name = Request.Form["asset_reg_location_name"];
                        ms_asset_reg_loc.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        ms_asset_reg_loc.created_by = UserProfile.UserId;
                        ms_asset_reg_loc.created_date = DateTime.Now;
                        ms_asset_reg_loc.updated_by = UserProfile.UserId;
                        ms_asset_reg_loc.updated_date = DateTime.Now;
                        ms_asset_reg_loc.org_id = UserProfile.OrgId;
                        ms_asset_reg_loc.deleted_by = null;
                        ms_asset_reg_loc.deleted_date = null;

                        db.Entry(ms_asset_reg_loc).State = EntityState.Added;
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
                            ms_asset_register_location ms_asset_reg_loc = db.ms_asset_register_location.Find(id);

                            ms_asset_reg_loc.fl_active = false;
                            ms_asset_reg_loc.deleted_by = UserProfile.UserId;
                            ms_asset_reg_loc.deleted_date = DateTime.Now;
                            db.Entry(ms_asset_reg_loc).State = EntityState.Modified;
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
    }
}
