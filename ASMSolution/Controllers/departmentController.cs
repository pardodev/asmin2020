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
    public class departmentController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        //private ModelAsm _db = new ModelAsm();

        // GET: department
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDepartment()
        {

            var CountryList = db.ms_department.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                t => new
                {
                    t.department_id,
                    t.department_code,
                    t.department_name
                }).ToList();
            return Json(CountryList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            //var Department = db.ms_department.Where(t => t.deleted_date == null).Select(
            //        t => new
            //        {
            //            t.department_id,
            //            t.department_code,
            //            t.department_name,
            //            t.department_email,
            //            t.fl_active,
            //            rec_isactive = (t.fl_active == true) ? "Yes" : "No",
            //            t.created_by,
            //            t.created_date,
            //            t.updated_by,
            //            t.updated_date,
            //            t.deleted_by,
            //            t.deleted_date,
            //        });
            var Department = from dep in db.ms_department
                             where (dep.deleted_date == null)
                             join u in db.ms_user on dep.updated_by equals u.user_id
                             into t_joined
                             from row_join in t_joined.DefaultIfEmpty()
                             from usr in db.ms_user.Where(rec_updated_by => (rec_updated_by == null) ? false : rec_updated_by.user_id == row_join.user_id).DefaultIfEmpty()
                             join emp in db.ms_employee on dep.employee_bod_id equals emp.employee_id
                             into emp_join
                             from empdata in emp_join.DefaultIfEmpty()
                             select new
                             {
                                 dep.department_id,
                                 dep.department_code,
                                 dep.department_name,
                                 dep.department_email,
                                 empdata.employee_name,
                                 dep.employee_bod_id,
                                 dep.fl_active,
                                 rec_isactive = (dep.fl_active == true) ? "Yes" : "No",
                                 dep.created_by,
                                 dep.created_date,
                                 updated_by = (usr == null) ? string.Empty : usr.user_name,
                                 updated_date = dep.updated_date,
                                 dep.deleted_by,
                                 dep.deleted_date
                             };
            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "department_code":
                        Department = Department.Where(t => t.department_code.Contains(searchString));
                        break;
                    case "department_name":
                        Department = Department.Where(t => t.department_name.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = Department.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                Department = Department.OrderByDescending(t => t.department_name);
                Department = Department.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                Department = Department.OrderBy(t => t.department_name);
                Department = Department.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = Department
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        public JsonResult GetBOD()
        {
            var BODList = (from t in db.ms_employee.Where(t => t.deleted_date == null)
                           join a in db.ms_employee_detail.Where(a => a.deleted_date == null && a.job_level_id == 9 && a.company_id == UserProfile.company_id)
                           on t.employee_id equals a.employee_id
                           select new
                           {
                               t.employee_id,
                               t.employee_name
                           }).ToList();
            return Json(BODList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CrudDepartment()
        {
            if (UserProfile.UserId != 0)
            {
                if (Request.Form["oper"] == "add")
                {
                    //prepare for insert data
                    ms_department ms_department = new ms_department();
                    ms_department.department_code = Request.Form["department_code"];
                    ms_department.department_name = Request.Form["department_name"];
                    ms_department.department_email = Request.Form["department_email"];
                    if (Request.Form["employee_name"] != null && Request.Form["employee_name"] != "")
                        ms_department.employee_bod_id = Convert.ToInt32(Request.Form["employee_name"]);
                    ms_department.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;
                    //ms_department.fl_active = true;

                    ms_department.created_by = UserProfile.UserId;
                    ms_department.created_date = DateTime.Now;
                    ms_department.updated_by = UserProfile.UserId;
                    ms_department.updated_date = DateTime.Now;
                    ms_department.org_id = UserProfile.OrgId;
                    ms_department.deleted_by = null;
                    ms_department.deleted_date = null;

                    //db.ms_department.Add(ms_department);
                    db.Entry(ms_department).State = EntityState.Added;
                    db.SaveChanges();

                    return Json("Insert", JsonRequestBehavior.AllowGet);
                }
                else if (Request.Form["oper"] == "edit")
                {
                    if (IsNumeric(Request.Form["department_id"].ToString()))
                    {
                        //prepare for update data
                        int id = Convert.ToInt32(Request.Form["department_id"]);
                        ms_department ms_department = db.ms_department.Find(id);
                        ms_department.department_code = Request.Form["department_code"];
                        ms_department.department_name = Request.Form["department_name"];
                        ms_department.department_email = Request.Form["department_email"];
                        if (Request.Form["employee_name"] != null && Request.Form["employee_name"] != "")
                            ms_department.employee_bod_id = Convert.ToInt32(Request.Form["employee_name"]);
                        ms_department.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        ms_department.updated_by = UserProfile.UserId;
                        ms_department.updated_date = DateTime.Now;

                        db.Entry(ms_department).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json("Update", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //prepare for insert data
                        ms_department ms_department = new ms_department();
                        ms_department.department_code = Request.Form["department_code"];
                        ms_department.department_name = Request.Form["department_name"];
                        ms_department.department_email = Request.Form["department_email"];
                        if (Request.Form["employee_name"] != null && Request.Form["employee_name"] != "")
                            ms_department.employee_bod_id = Convert.ToInt32(Request.Form["employee_name"]);
                        ms_department.fl_active = Request.Form["rec_isactive"] == "Yes" ? true : false;

                        ms_department.created_by = UserProfile.UserId;
                        ms_department.created_date = DateTime.Now;
                        ms_department.updated_by = UserProfile.UserId;
                        ms_department.updated_date = DateTime.Now;
                        ms_department.org_id = UserProfile.OrgId;
                        ms_department.deleted_by = null;
                        ms_department.deleted_date = null;

                        db.Entry(ms_department).State = EntityState.Added;
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
                            ms_department ms_department = db.ms_department.Find(id);

                            ms_department.deleted_by = UserProfile.UserId;
                            ms_department.deleted_date = DateTime.Now;
                            ms_department.fl_active = false;

                            db.Entry(ms_department).State = EntityState.Modified;
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
