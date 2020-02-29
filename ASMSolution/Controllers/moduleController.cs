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
    public class moduleController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        public ActionResult Index()
        {
            //return View(db.ms_module.ToList());
            return View();
        }

        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            ms_module module = new ms_module();
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var ModuleList = from m in db.ms_module
                             where (m.deleted_date == null)
                             join u in db.ms_user on m.updated_by equals u.user_id
                             into t_joined
                             from row_join in t_joined.DefaultIfEmpty()
                             from usr in db.ms_user.Where(rec_udated_by => (rec_udated_by == null) ? false : rec_udated_by.user_id == row_join.user_id).DefaultIfEmpty()
                             select new
                             {
                                 m.module_id,
                                 m.module_code,
                                 m.module_name,
                                 m.rec_order,
                                 m.fl_active,
                                 rec_isactive = (m.fl_active == true) ? "Yes" : "No",
                                 updated_by = (usr == null) ? string.Empty : usr.user_name,
                                 m.updated_date
                             };

            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "modul_code":
                        ModuleList = ModuleList.Where(t => t.module_code.Contains(searchString));
                        break;
                    case "module_name":
                        ModuleList = ModuleList.Where(t => t.module_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = ModuleList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default soring
            if (sord.ToUpper() == "DESC")
            {
                ModuleList = ModuleList.OrderByDescending(t => t.module_code);
                ModuleList = ModuleList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                ModuleList = ModuleList.OrderBy(t => t.module_code);
                ModuleList = ModuleList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = ModuleList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Getmodule()
        {
            var ModuleList = db.ms_module.Where(t => t.deleted_date == null).Select(
                t => new
                {
                    t.module_id,
                    t.module_code,
                    t.module_name,
                }).ToList();
            return Json(ModuleList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CRUDModule()
        {
            if (Request.HttpMethod == "POST")
            {
                ms_module _o = null;
                int id = 0;
                switch (Request.Form["oper"])
                {
                    case "del":
                        string ids = Request.Form["id"];
                        string[] values = ids.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = values[i].Trim();
                            //prepare for soft delete data
                            id = Convert.ToInt32(values[i]);
                            _o = db.ms_module.Find(id);
                            _o.fl_active = false;
                            _o.deleted_by = UserProfile.UserId; //userid
                            _o.deleted_date = DateTime.Now;
                            db.Entry(_o).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;
                    case "add":
                        _o = new ms_module();
                        _o.module_code = Request.Form["module_code"];
                        _o.module_name = Request.Form["module_name"];
                        _o.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");
                        _o.rec_order = Convert.ToInt32(Request.Form["rec_order"]);

                        _o.created_by = UserProfile.UserId;
                        _o.created_date = DateTime.Now;
                        _o.updated_by = UserProfile.UserId;
                        _o.updated_date = DateTime.Now;
                        _o.org_id = UserProfile.OrgId;
                        _o.deleted_by = null;
                        _o.deleted_date = null;
                        db.Entry(_o).State = EntityState.Added;
                        db.SaveChanges();
                        break;
                    case "edit":
                       bool boolNumeric = Int32.TryParse(Request.Form["module_id"], out id);
                        if (boolNumeric)     //update
                        {
                            //id = Convert.ToInt32(Request.Form["module_id"]);
                            _o = db.ms_module.Find(id);
                            _o.module_code = Request.Form["module_code"];
                            _o.module_name = Request.Form["module_name"];
                            _o.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");
                            _o.rec_order = Convert.ToInt32(Request.Form["rec_order"]);

                            _o.updated_by = UserProfile.UserId;
                            _o.updated_date = DateTime.Now;
                            _o.org_id = UserProfile.OrgId;
                            _o.deleted_by = null;
                            _o.deleted_date = null;
                            db.Entry(_o).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else //add
                        {
                            _o = new ms_module();
                            _o.module_code = Request.Form["module_code"];
                            _o.module_name = Request.Form["module_name"];
                            _o.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");
                            _o.rec_order = Convert.ToInt32(Request.Form["rec_order"]);

                            _o.created_by = UserProfile.UserId;
                            _o.created_date = DateTime.Now;
                            _o.updated_by = UserProfile.UserId;
                            _o.updated_date = DateTime.Now;
                            _o.org_id = UserProfile.OrgId;
                            _o.deleted_by = null;
                            _o.deleted_date = null;
                            db.Entry(_o).State = EntityState.Added;
                            db.SaveChanges();
                        }
                        break;
                    default:
                        break;
                }
                return Json("Records successfully saved.", JsonRequestBehavior.AllowGet);
            }
            return Json("Invalid requests");
        }


        [HttpPost]
        public JsonResult PackDelete(int id)
        {
            ms_module _o = db.ms_module.Find(id);
            db.ms_module.Remove(_o);
            db.SaveChanges();
            return Json("Records deleted successfully.", JsonRequestBehavior.AllowGet);
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
