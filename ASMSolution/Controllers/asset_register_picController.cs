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
    public class asset_register_picController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_register_pic
        public ActionResult Index()
        {
            return View();
            //db.ms_asset_register_pic.ToList()
        }

        //GET : /asset_register_pic/list
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var AssetRegPICList = from reg in db.ms_asset_register_pic
                                  where (reg.deleted_date == null)
                                  join u in db.ms_user on reg.updated_by equals u.user_id
                                  into t_joined
                                  from row_join in t_joined.DefaultIfEmpty()
                                  from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                                  select new
                                  {
                                      reg.asset_reg_pic_id,
                                      reg.asset_reg_pic_code,
                                      reg.asset_reg_pic_name,
                                      reg.fl_active,
                                      rec_isactive = (reg.fl_active == true) ? "Yes" : "No",
                                      reg.created_by,
                                      reg.created_date,
                                      updated_by = (usr == null) ? string.Empty : usr.user_name,
                                      updated_date = reg.updated_date,
                                      reg.deleted_by,
                                      reg.deleted_date
                                  };
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "asset_reg_pic_code":
                        AssetRegPICList = AssetRegPICList.Where(t => t.asset_reg_pic_code.Contains(searchString));
                        break;
                    case "asset_reg_pic_name":
                        AssetRegPICList = AssetRegPICList.Where(t => t.asset_reg_pic_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = AssetRegPICList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                AssetRegPICList = AssetRegPICList.OrderByDescending(t => t.asset_reg_pic_name);
                AssetRegPICList = AssetRegPICList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                AssetRegPICList = AssetRegPICList.OrderBy(t => t.asset_reg_pic_name);
                AssetRegPICList = AssetRegPICList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = AssetRegPICList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudAssetRegPIC()
        {
            if (UserProfile.UserId != 0)
            {
                if (Request.Form["oper"] == "add")
                {
                    //prepare for insert data
                    ms_asset_register_pic ms_asset_reg_pic = new ms_asset_register_pic();
                    ms_asset_reg_pic.asset_reg_pic_code = Request.Form["asset_reg_pic_code"];
                    ms_asset_reg_pic.asset_reg_pic_name = Request.Form["asset_reg_pic_name"];
                    ms_asset_reg_pic.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                    ms_asset_reg_pic.created_by = UserProfile.UserId;
                    ms_asset_reg_pic.created_date = DateTime.Now;
                    ms_asset_reg_pic.updated_by = UserProfile.UserId;
                    ms_asset_reg_pic.updated_date = DateTime.Now;
                    ms_asset_reg_pic.org_id = UserProfile.OrgId;
                    ms_asset_reg_pic.deleted_by = null;
                    ms_asset_reg_pic.deleted_date = null;

                    db.Entry(ms_asset_reg_pic).State = EntityState.Added;
                    db.SaveChanges();
                    return Json("Insert", JsonRequestBehavior.AllowGet);
                }
                else if (Request.Form["oper"] == "edit")
                {
                    if (IsNumeric(Request.Form["asset_reg_pic_id"].ToString()))
                    {
                        //prepare for update data
                        int id = Convert.ToInt32(Request.Form["asset_reg_pic_id"]);
                        ms_asset_register_pic ms_asset_reg_pic = db.ms_asset_register_pic.Find(id);
                        ms_asset_reg_pic.asset_reg_pic_code = Request.Form["asset_reg_pic_code"];
                        ms_asset_reg_pic.asset_reg_pic_name = Request.Form["asset_reg_pic_name"];
                        ms_asset_reg_pic.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        ms_asset_reg_pic.updated_by = UserProfile.UserId;
                        ms_asset_reg_pic.updated_date = DateTime.Now;

                        db.Entry(ms_asset_reg_pic).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json("Update", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //prepare for insert data
                        ms_asset_register_pic ms_asset_reg_pic = new ms_asset_register_pic();
                        ms_asset_reg_pic.asset_reg_pic_code = Request.Form["asset_reg_pic_code"];
                        ms_asset_reg_pic.asset_reg_pic_name = Request.Form["asset_reg_pic_name"];
                        ms_asset_reg_pic.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        ms_asset_reg_pic.created_by = UserProfile.UserId;
                        ms_asset_reg_pic.created_date = DateTime.Now;
                        ms_asset_reg_pic.updated_by = UserProfile.UserId;
                        ms_asset_reg_pic.updated_date = DateTime.Now;
                        ms_asset_reg_pic.org_id = UserProfile.OrgId;
                        ms_asset_reg_pic.deleted_by = null;
                        ms_asset_reg_pic.deleted_date = null;

                        db.Entry(ms_asset_reg_pic).State = EntityState.Added;
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
                            ms_asset_register_pic ms_asset_reg_pic = db.ms_asset_register_pic.Find(id);

                            ms_asset_reg_pic.fl_active = false;
                            ms_asset_reg_pic.deleted_by = UserProfile.UserId;
                            ms_asset_reg_pic.deleted_date = DateTime.Now;

                            db.Entry(ms_asset_reg_pic).State = EntityState.Modified;
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
