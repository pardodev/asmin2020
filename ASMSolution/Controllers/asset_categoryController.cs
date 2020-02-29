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
    public class asset_categoryController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        //private ModelAsm _db = new ModelAsm();

        // GET: asset_category
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var Category = from cat in db.ms_asset_category
                           where (cat.deleted_date == null)
                           join u in db.ms_user on cat.updated_by equals u.user_id
                           into t_joined
                           from row_join in t_joined.DefaultIfEmpty()
                           from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                           select new
                           {
                               cat.category_id,
                               cat.category_code,
                               cat.category_name,
                               cat.fl_active,
                               rec_isactive = (cat.fl_active == true) ? "Yes" : "No",
                               cat.created_by,
                               cat.created_date,
                               updated_by = (usr == null) ? string.Empty : usr.user_name,
                               updated_date = cat.updated_date,
                               cat.deleted_by,
                               cat.deleted_date
                           };
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "category_code":
                        Category = Category.Where(t => t.category_code.Contains(searchString));
                        break;
                    case "category_name":
                        Category = Category.Where(t => t.category_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = Category.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                Category = Category.OrderByDescending(t => t.category_name);
                Category = Category.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                Category = Category.OrderBy(t => t.category_name);
                Category = Category.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = Category
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudCategory()
        {
            if (UserProfile.UserId != 0)
            {
                if (Request.Form["oper"] == "add")
                {
                    //prepare for insert data
                    ms_asset_category ms_asset_category = new ms_asset_category();
                    ms_asset_category.category_code = Request.Form["category_code"];
                    ms_asset_category.category_name = Request.Form["category_name"];
                    ms_asset_category.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                    ms_asset_category.created_by = UserProfile.UserId;
                    ms_asset_category.created_date = DateTime.Now;
                    ms_asset_category.updated_by = UserProfile.UserId;
                    ms_asset_category.updated_date = DateTime.Now;
                    ms_asset_category.org_id = UserProfile.OrgId;
                    ms_asset_category.deleted_by = null;
                    ms_asset_category.deleted_date = null;

                    db.Entry(ms_asset_category).State = EntityState.Added;
                    db.SaveChanges();
                    return Json("Insert", JsonRequestBehavior.AllowGet);
                }
                else if (Request.Form["oper"] == "edit")
                {
                    if (IsNumeric(Request.Form["category_id"].ToString()))
                    {
                        //prepare for update data
                        int id = Convert.ToInt32(Request.Form["category_id"]);
                        ms_asset_category ms_asset_category = db.ms_asset_category.Find(id);
                        ms_asset_category.category_code = Request.Form["category_code"];
                        ms_asset_category.category_name = Request.Form["category_name"];
                        ms_asset_category.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                        ms_asset_category.updated_by = UserProfile.UserId;
                        ms_asset_category.updated_date = DateTime.Now;

                        db.Entry(ms_asset_category).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json("Update", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //prepare for insert data
                        ms_asset_category ms_asset_category = new ms_asset_category();
                        ms_asset_category.category_code = Request.Form["category_code"];
                        ms_asset_category.category_name = Request.Form["category_name"];
                        ms_asset_category.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        ms_asset_category.created_by = UserProfile.UserId;
                        ms_asset_category.created_date = DateTime.Now;
                        ms_asset_category.updated_by = UserProfile.UserId;
                        ms_asset_category.updated_date = DateTime.Now;
                        ms_asset_category.org_id = UserProfile.OrgId;
                        ms_asset_category.deleted_by = null;
                        ms_asset_category.deleted_date = null;

                        db.Entry(ms_asset_category).State = EntityState.Added;
                        //db.ms_asset_category.Add(ms_asset_category);
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
                            ms_asset_category ms_asset_category = db.ms_asset_category.Find(id);

                            ms_asset_category.fl_active = false;
                            ms_asset_category.deleted_by = UserProfile.UserId;
                            ms_asset_category.deleted_date = DateTime.Now;

                            db.Entry(ms_asset_category).State = EntityState.Modified;
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
