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
    public class job_levelController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// list untuk DDL
        /// </summary>
        /// <returns>List</returns>
        public JsonResult GetJobLevel()
        {
            var _qry = from t in db.ms_job_level
                       where (t.deleted_date == null && t.fl_active == true)
                       select new
                       {
                           t.job_level_id,
                           job_level_name = "[" + t.job_level_code + "] " + t.job_level_name
                       };

            return Json(_qry, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            //var JobLevel = db.ms_job_level.Where(t => t.deleted_date == null).Select(
            //        t => new
            //        {
            //            t.job_level_id,
            //            t.job_level_code,
            //            t.job_level_name,
            //            t.fl_active,
            //            rec_isactive = (t.fl_active == true) ? "Yes" : "No",
            //            t.created_by,
            //            t.created_date,
            //            t.updated_by,
            //            t.updated_date,
            //            t.deleted_by,
            //            t.deleted_date,
            //        });
            var JobLevel = from jl in db.ms_job_level
                           where (jl.deleted_date == null)
                           join u in db.ms_user on jl.updated_by equals u.user_id
                           into t_joined
                           from row_join in t_joined.DefaultIfEmpty()
                           from usr in db.ms_user.Where(rec_udated_by => (rec_udated_by == null) ? false : rec_udated_by.user_id == row_join.user_id).DefaultIfEmpty()
                           select new
                           {
                               jl.job_level_id,
                               jl.job_level_code,
                               jl.job_level_name,
                               jl.fl_active,
                               rec_isactive = (jl.fl_active == true) ? "Yes" : "No",
                               updated_date = jl.updated_date,
                               updated_by = (usr == null) ? string.Empty : usr.user_name
                           };


            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "job_level_code":
                        JobLevel = JobLevel.Where(t => t.job_level_code.Contains(searchString));
                        break;
                    case "job_level_name":
                        JobLevel = JobLevel.Where(t => t.job_level_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = JobLevel.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                JobLevel = JobLevel.OrderByDescending(t => t.job_level_name);
                JobLevel = JobLevel.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                JobLevel = JobLevel.OrderBy(t => t.job_level_name);
                JobLevel = JobLevel.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = JobLevel
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudJobLevel()
        {
            if (Request.Form["oper"] == "add")
            {
                //prepare for insert data
                ms_job_level ms_job_level = new ms_job_level();
                ms_job_level.job_level_code = Request.Form["job_level_code"];
                ms_job_level.job_level_name = Request.Form["job_level_name"];
                ms_job_level.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");

                ms_job_level.created_by = UserProfile.UserId;
                ms_job_level.created_date = DateTime.Now;
                ms_job_level.updated_by = UserProfile.UserId;
                ms_job_level.updated_date = DateTime.Now;
                ms_job_level.org_id = UserProfile.OrgId;
                ms_job_level.deleted_by = null;
                ms_job_level.deleted_date = null;
                db.ms_job_level.Add(ms_job_level);
                db.SaveChanges();
                return Json("Insert Job Level Data Success!", JsonRequestBehavior.AllowGet);
            }
            else if (Request.Form["oper"] == "edit")
            {
                if (IsNumeric(Request.Form["job_level_id"].ToString()))
                {
                    //prepare for update data
                    int id = Convert.ToInt32(Request.Form["job_level_id"]);
                    ms_job_level ms_job_level = db.ms_job_level.Find(id);
                    ms_job_level.job_level_code = Request.Form["job_level_code"];
                    ms_job_level.job_level_name = Request.Form["job_level_name"];
                    ms_job_level.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                    ms_job_level.updated_by = UserProfile.UserId;
                    ms_job_level.updated_date = DateTime.Now;
                    ms_job_level.org_id = UserProfile.OrgId;
                    ms_job_level.deleted_by = null;
                    ms_job_level.deleted_date = null;
                    db.SaveChanges();
                    return Json("Update Job Level Data Success!", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //prepare for insert data
                    ms_job_level ms_job_level = new ms_job_level();
                    ms_job_level.job_level_code = Request.Form["job_level_code"];
                    ms_job_level.job_level_name = Request.Form["job_level_name"];
                    ms_job_level.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                    ms_job_level.created_by = UserProfile.UserId;
                    ms_job_level.created_date = DateTime.Now;
                    ms_job_level.updated_by = UserProfile.UserId;
                    ms_job_level.updated_date = DateTime.Now;
                    ms_job_level.org_id = UserProfile.OrgId;
                    ms_job_level.deleted_by = null;
                    ms_job_level.deleted_date = null;
                    db.ms_job_level.Add(ms_job_level);
                    db.SaveChanges();
                    return Json("Insert Job Level Data Success!", JsonRequestBehavior.AllowGet);
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
                        ms_job_level ms_job_level = db.ms_job_level.Find(id);
                        ms_job_level.fl_active = false;
                        ms_job_level.deleted_by = UserProfile.UserId; //userid
                        ms_job_level.deleted_date = DateTime.Now;
                        db.SaveChanges();
                    }

                }
                return Json("Deleted Success!");
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
