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
    public class menuController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        public ActionResult Index()
        {
            //var ms_menu = db.ms_menu.Include(m => m.ms_module);
            //return View(ms_menu.ToList());
            return View();
        }

        public ActionResult GetMenuList()
        {
            try
            {
                var result = (from m in db.ms_menu
                              join n in db.ms_module
                              on m.module_id equals n.module_id
                              where (m.deleted_date == null && m.fl_active == true)
                              select new
                              {
                                  m.menu_id,
                                  n.module_name,
                                  m.module_id,
                                  m.menu_code,
                                  m.menu_name,
                                  m.menu_url,
                              }).ToList();
                return View("menu_list", result);
                //ms_menu_list menu = new ms_menu_list();
                //return View(result);
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
                return Content("Error");
            }
        }
        

        [HttpGet]
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var _menuList = from mn in db.ms_menu
                            where (mn.deleted_date == null)
                            join md in db.ms_module on mn.module_id equals md.module_id
                            join u in db.ms_user on mn.updated_by equals u.user_id
                            into t_joined
                            from row_join in t_joined.DefaultIfEmpty()
                            from usr in db.ms_user.Where(rec_udated_by => (rec_udated_by == null) ? false : rec_udated_by.user_id == row_join.user_id).DefaultIfEmpty()
                            select new
                            {
                                menu_id = mn.menu_id,
                                module_name = md.module_name ?? string.Empty,
                                module_id = mn.module_id,
                                menu_code = mn.menu_code,
                                menu_name = mn.menu_name,
                                menu_url = mn.menu_url,
                                rec_order = mn.rec_order,
                                fl_active = (mn.fl_active == true) ? "Yes" : "No",
                                updated_date = mn.updated_date,
                                updated_by = (usr == null) ? string.Empty : usr.user_name
                            };

            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "menu_code":
                        _menuList = _menuList.Where(m => m.menu_code.Contains(searchString));
                        break;
                    case "menu_name":
                        _menuList = _menuList.Where(m => m.menu_name.Contains(searchString));
                        break;
                    case "menu_url":
                        _menuList = _menuList.Where(m => m.menu_url.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = _menuList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                _menuList = _menuList.OrderByDescending(t => t.menu_code);
                _menuList = _menuList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                _menuList = _menuList.OrderBy(t => t.menu_code);
                _menuList = _menuList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _menuList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetActiveMenu(int user_type_id, int job_level_id)
        {

            var _qry = from t in db.ms_menu
                       where (t.fl_active == true && t.deleted_date == null)
                       select new
                       {
                           t.menu_id,
                           t.menu_code,
                           t.menu_name,
                           t.module_id
                       };

            return Json(_qry, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CRUDMenu()
        {
            if (Request.HttpMethod == "POST")
            {
                ms_menu _o = null;
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
                            _o = db.ms_menu.Find(id);
                            _o.fl_active = false;
                            _o.deleted_by = UserProfile.UserId; //userid
                            _o.deleted_date = DateTime.Now;
                            db.Entry(_o).State = EntityState.Modified;
                            db.SaveChanges();
                            db.SaveChanges();
                        }
                        break;
                    case "add":
                        _o = new ms_menu();
                        _o.module_id = Convert.ToInt32(Request.Form["module_name"]);
                        _o.menu_code = Request.Form["menu_code"];
                        _o.menu_name = Request.Form["menu_name"];
                        _o.menu_url = Request.Form["menu_url"];
                        _o.fl_active = Request.Form["fl_active"].ToLower().Equals("yes");
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
                        bool boolNumeric = Int32.TryParse(Request.Form["menu_id"], out id);
                        if (boolNumeric)     //update
                        {
                            _o = db.ms_menu.Find(id);
                            _o.module_id = Convert.ToInt32(Request.Form["module_name"]);
                            _o.menu_code = Request.Form["menu_code"];
                            _o.menu_name = Request.Form["menu_name"];
                            _o.menu_url = Request.Form["menu_url"];
                            _o.fl_active = Request.Form["fl_active"].ToLower().Equals("yes");
                            _o.rec_order = Convert.ToInt32(Request.Form["rec_order"]);

                            _o.updated_by = UserProfile.UserId;
                            _o.updated_date = DateTime.Now;
                            _o.org_id = UserProfile.OrgId;
                            _o.deleted_by = null;
                            _o.deleted_date = null;
                            db.Entry(_o).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else  //add
                        {
                            _o = new ms_menu();
                            _o.module_id = Convert.ToInt32(Request.Form["module_name"]);
                            _o.menu_code = Request.Form["menu_code"];
                            _o.menu_name = Request.Form["menu_name"];
                            _o.menu_url = Request.Form["menu_url"];
                            _o.fl_active = Request.Form["fl_active"].ToLower().Equals("yes");
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
            ms_menu _o = db.ms_menu.Find(id);
            db.ms_menu.Remove(_o);
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
