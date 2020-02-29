using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class user_typeController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// mengambil semua User Type untuk DDL
        /// </summary>
        /// <returns>List</returns>
        public JsonResult GetUserType()
        {
            var _qry = from ut in db.ms_user_type
                       where (ut.deleted_date == null && ut.fl_active == true)
                       select new
                       {
                           ut.user_type_id,
                           user_type_name = "[" + ut.user_type_code + "] " + ut.user_type_name
                       };

            return Json(_qry, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var _list = from ut in db.ms_user_type
                        where (ut.deleted_date == null)
                        join u in db.ms_user on ut.updated_by equals u.user_id
                        into t_joined
                        from row_join in t_joined.DefaultIfEmpty()
                        from usr in db.ms_user.Where(rec_udated_by => (rec_udated_by == null) ? false : rec_udated_by.user_id == row_join.user_id).DefaultIfEmpty()
                        select new
                        {
                            ut.user_type_id,
                            ut.user_type_code,
                            ut.user_type_name,
                            ut.fl_active,
                            rec_isactive = (ut.fl_active == true) ? "Yes" : "No",
                            updated_date = ut.updated_date,
                            updated_by = (usr == null) ? string.Empty : usr.user_name
                        };

            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "user_type_code":
                        _list = _list.Where(m => m.user_type_code.Contains(searchString));
                        break;
                    case "user_type_name":
                        _list = _list.Where(m => m.user_type_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = _list.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                _list = _list.OrderByDescending(t => t.user_type_code);
                _list = _list.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                _list = _list.OrderBy(t => t.user_type_code);
                _list = _list.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _list
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CRUDUserType()
        {
            if (Request.HttpMethod == "POST")
            {
                ms_user_type _o = null;
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
                            _o = db.ms_user_type.Find(id);
                            _o.fl_active = false;
                            _o.deleted_by = UserProfile.UserId; //userid
                            _o.deleted_date = DateTime.Now;
                            db.Entry(_o).State = EntityState.Modified;
                            db.SaveChanges();
                            //return Json("Records deleted successfully.", JsonRequestBehavior.AllowGet);
                        }
                        break;
                    case "add":
                        _o = new ms_user_type();
                        _o.user_type_code = Request.Form["user_type_code"];
                        _o.user_type_name = Request.Form["user_type_name"];
                        _o.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");
                        _o.created_by = UserProfile.UserId;
                        _o.created_date = DateTime.Now;
                        _o.updated_by = UserProfile.UserId;
                        _o.updated_date = DateTime.Now;
                        _o.org_id = UserProfile.OrgId;
                        _o.deleted_by = null;
                        _o.deleted_date = null;
                        db.Entry(_o).State = EntityState.Added;
                        db.SaveChanges();
                        //return Json("Records deleted successfully.", JsonRequestBehavior.AllowGet);
                        break;
                    case "edit":
                        bool isNumeric = int.TryParse(Request.Form["user_type_id"].ToString(), out id);
                        if (!isNumeric) //add
                        {
                            _o = new ms_user_type();
                            _o.user_type_code = Request.Form["user_type_code"];
                            _o.user_type_name = Request.Form["user_type_name"];
                            _o.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");

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
                        else //edit
                        {
                            _o = db.ms_user_type.Find(id);
                            _o.user_type_code = Request.Form["user_type_code"];
                            _o.user_type_name = Request.Form["user_type_name"];
                            _o.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");

                            _o.updated_by = UserProfile.UserId;
                            _o.updated_date = DateTime.Now;
                            _o.org_id = UserProfile.OrgId;
                            _o.deleted_by = null;
                            _o.deleted_date = null;
                            db.Entry(_o).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        //return Json("Records deleted successfully.", JsonRequestBehavior.AllowGet);
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
            ms_user_type _o = db.ms_user_type.Find(id);
            db.ms_user_type.Remove(_o);
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
