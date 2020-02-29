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
    public class employeeController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: employee
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEmployee()
        {

            var CountryList = db.ms_employee.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                t => new
                {
                    t.employee_id,
                    t.employee_nik,
                    t.employee_name
                }).ToList();
            return Json(CountryList, JsonRequestBehavior.AllowGet);
        }

        //GET : /country/list
        [HttpGet]
        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            //var EmployeeList = db.ms_employee.Where(t => t.deleted_date == null).Select(
            //        t => new
            //        {
            //            //fetch all data
            //            t.employee_id,
            //            t.employee_nik,
            //            t.employee_name,
            //            t.employee_email,
            //            t.ip_address,
            //            t.fl_active,
            //            rec_isactive = (t.fl_active == true) ? "Yes" : "No",
            //            t.created_by,
            //            t.created_date,
            //            t.updated_by,
            //            t.updated_date,
            //            t.deleted_by,
            //            t.deleted_date,
            //        });

            var EmployeeList = from emp in db.ms_employee
                               where (emp.deleted_date == null)
                               join u in db.ms_user on emp.updated_by equals u.user_id
                               into t_joined
                               from row_join in t_joined.DefaultIfEmpty()
                               from usr in db.ms_user.Where(rec_udated_by => (rec_udated_by == null) ? false : rec_udated_by.user_id == row_join.user_id).DefaultIfEmpty()
                               select new
                               {
                                   emp.employee_id,
                                   emp.employee_nik,
                                   emp.employee_name,
                                   emp.employee_email,
                                   emp.ip_address,
                                   emp.fl_active,
                                   rec_isactive = (emp.fl_active == true) ? "Yes" : "No",
                                   updated_date = emp.updated_date,
                                   updated_by = (usr == null) ? string.Empty : usr.user_name
                               };

            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "employee_nik":
                        EmployeeList = EmployeeList.Where(t => t.employee_nik.Contains(searchString));
                        break;
                    case "employee_name":
                        EmployeeList = EmployeeList.Where(t => t.employee_name.Contains(searchString));
                        break;
                    case "employee_email":
                        EmployeeList = EmployeeList.Where(t => t.employee_email.Contains(searchString));
                        break;
                    case "ip_address":
                        EmployeeList = EmployeeList.Where(t => t.ip_address.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = EmployeeList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                EmployeeList = EmployeeList.OrderByDescending(t => t.employee_name);
                EmployeeList = EmployeeList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                EmployeeList = EmployeeList.OrderBy(t => t.employee_name);
                EmployeeList = EmployeeList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = EmployeeList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool IsNumeric(string id)
        {
            int ID;
            return int.TryParse(id, out ID);
        }

        [HttpPost]
        public JsonResult CrudEmployee()
        {
            if (Request.Form["oper"] == "add")
            {
                //prepare for insert data
                ms_employee ms_emp = new ms_employee();
                ms_emp.employee_nik = Request.Form["employee_nik"];
                ms_emp.employee_name = Request.Form["employee_name"];
                ms_emp.employee_email = Request.Form["employee_email"];
                ms_emp.ip_address = Request.Form["ip_address"];
                ms_emp.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");

                ms_emp.created_by = UserProfile.UserId;
                ms_emp.created_date = DateTime.Now;
                ms_emp.updated_by = UserProfile.UserId;
                ms_emp.updated_date = DateTime.Now;
                ms_emp.org_id = UserProfile.OrgId;
                ms_emp.deleted_by = null;
                ms_emp.deleted_date = null;
                db.ms_employee.Add(ms_emp);
                db.SaveChanges();
                return Json("Employee successfully saved", JsonRequestBehavior.AllowGet);
            }
            else if (Request.Form["oper"] == "edit")
            {
                if (IsNumeric(Request.Form["employee_id"].ToString()))
                {
                    //prepare for update data
                    int id = Convert.ToInt32(Request.Form["employee_id"]);
                    ms_employee emp = db.ms_employee.Find(id);
                    emp.employee_nik = Request.Form["employee_nik"];
                    emp.employee_name = Request.Form["employee_name"];
                    emp.employee_email = Request.Form["employee_email"];
                    emp.ip_address = Request.Form["ip_address"];
                    emp.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");
                    emp.updated_by = UserProfile.UserId;
                    emp.updated_date = DateTime.Now;
                    db.SaveChanges();
                    return Json("Employee successfully saved", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //prepare for insert data

                    //check if nik exists
                    bool is_nik_exist = false;
                    string str_nik = Request.Form["employee_nik"];
                    ms_employee ms_emp = db.ms_employee.Where(c => c.employee_nik == str_nik).SingleOrDefault<ms_employee>();
                    is_nik_exist = (ms_emp != null);
                    if (!is_nik_exist)
                    {
                        ms_emp = new ms_employee();
                    }

                    ms_emp.employee_nik = Request.Form["employee_nik"];
                    ms_emp.employee_name = Request.Form["employee_name"];
                    ms_emp.employee_email = Request.Form["employee_email"];
                    ms_emp.ip_address = Request.Form["ip_address"];
                    ms_emp.fl_active = Request.Form["rec_isactive"].ToLower().Equals("yes");

                    if (!is_nik_exist)
                    {
                        ms_emp.created_by = UserProfile.UserId;
                        ms_emp.created_date = DateTime.Now;
                    }
                    ms_emp.updated_by = UserProfile.UserId;
                    ms_emp.updated_date = DateTime.Now;
                    ms_emp.org_id = UserProfile.OrgId;
                    ms_emp.deleted_by = null;
                    ms_emp.deleted_date = null;

                    //db.ms_employee.Add(ms_emp);
                    if (!is_nik_exist)
                        db.Entry(ms_emp).State = EntityState.Added;
                    else
                        db.Entry(ms_emp).State = EntityState.Modified;

                    db.SaveChanges();
                    return Json("Employee successfully saved", JsonRequestBehavior.AllowGet);
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
                        ms_employee emp = db.ms_employee.Find(id);
                        emp.fl_active = false;
                        emp.deleted_by = UserProfile.UserId; //userid
                        emp.deleted_date = DateTime.Now;
                        db.SaveChanges();
                    }

                }
                return Json("Deleted Success!");
            }

        }



        /*
         employee setup
         */
        public ActionResult Setup(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ms_employee emp_db = db.ms_employee.Single(a => a.employee_id == id);
            if (emp_db == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            employee_setupViewModel model = new employee_setupViewModel()
            {
                employee_id = emp_db.employee_id,
                ms_employee = emp_db
            };

            List<SelectListItem> first = new List<SelectListItem> { new SelectListItem { Text = "- [Not Set] -", Value = "0", Selected = true } };

            model.company_list = (from t in db.ms_asmin_company
                                  where (t.fl_active == true && t.deleted_date == null)
                                  select t).ToList();

            model.sli_register_list = (from t in db.ms_asset_register_location
                                       where (t.fl_active == true && t.deleted_date == null)
                                       select new SelectListItem
                                       {
                                           Text = t.asset_reg_location_name,
                                           Value = t.asset_reg_location_id.ToString()
                                       }
                                  ).ToList<SelectListItem>().Union(first);


            model.sli_location_list = (from t in db.ms_asset_location
                                       where (t.fl_active == true && t.deleted_date == null)
                                       //&& t.asset_reg_location_id == model.selected_register_id[0]
                                       select new SelectListItem
                                       {
                                           Text = t.location_name,
                                           Value = t.location_id.ToString()
                                       }
                                  ).ToList<SelectListItem>().Union(first);


            model.sli_department_list = (from t in db.ms_department
                                         where (t.fl_active == true && t.deleted_date == null)
                                         //from d in db.ms_employee_detail
                                         //where (d.employee_id == emp_db.employee_id && t.department_id == d.department_id)
                                         select new SelectListItem
                                         {
                                             Text = t.department_name,
                                             Value = t.department_id.ToString()
                                             //Selected = (d.employee_id>0)
                                         }
                                  ).ToList<SelectListItem>().Union(first);

            model.sli_job_level_list = (from t in db.ms_job_level
                                        where (t.fl_active == true && t.deleted_date == null)
                                        select new SelectListItem
                                        {
                                            Text = t.job_level_name,
                                            Value = t.job_level_id.ToString()
                                        }
                                  ).ToList<SelectListItem>().Union(first);


            model.sli_user_type_list = (from t in db.ms_user_type
                                        where (t.fl_active == true && t.deleted_date == null)
                                        select new SelectListItem
                                        {
                                            Text = t.user_type_name,
                                            Value = t.user_type_id.ToString()
                                        }
                                  ).ToList<SelectListItem>().Union(first);


            model.sli_range_list = (from t in db.ms_approval_range
                                    where (t.fl_active == true && t.deleted_date == null)
                                    select new SelectListItem
                                    {
                                        Text = t.range_code,
                                        Value = t.range_id.ToString()
                                    }
                                  ).ToList<SelectListItem>().Union(first);

            int i_pos = 0;
            int i_max = model.company_list.Count;

            foreach (ms_asmin_company company in model.company_list)
            {
                employee_detailViewModel _item = new employee_detailViewModel()
                {
                    employee_id = emp_db.employee_id,
                    ms_asmin_company = company,
                    sli_register_list = model.sli_register_list,
                    sli_location_list = model.sli_location_list,
                    sli_department_list = model.sli_department_list,
                    sli_job_level_list = model.sli_job_level_list,
                    sli_user_type_list = model.sli_user_type_list,
                    sli_range_list = model.sli_range_list
                };

                ms_employee_detail dtl_db = (from d in db.ms_employee_detail
                                             where (d.employee_id == emp_db.employee_id
                                             && d.company_id == company.company_id)
                                             select d).FirstOrDefault<ms_employee_detail>();
                if (dtl_db != null)
                {
                    dtl_db.range_id = 0;  //di hide
                    _item.selected_register_id = (dtl_db.asset_reg_location_id.HasValue) ? dtl_db.asset_reg_location_id.Value : 0;
                    _item.selected_location_id = (dtl_db.location_id.HasValue) ? dtl_db.location_id.Value : 0;
                    _item.selected_department_id = (dtl_db.department_id.HasValue) ? dtl_db.department_id.Value : 0;
                    _item.selected_job_level_id = (dtl_db.job_level_id.HasValue) ? dtl_db.job_level_id.Value : 0;
                    //_item.selected_user_type_id = (dtl_db.user_type_id.HasValue) ? dtl_db.user_type_id.Value : 0;
                    _item.selected_fl_approver = (dtl_db.range_id.HasValue) ? ((dtl_db.range_id.Value > 0) ? 1 : 0) : 0;
                    _item.selected_range_id = (dtl_db.range_id.HasValue) ? dtl_db.range_id.Value : 0;

                    model.checkbox_approver.Add(new SelectedApprover_CheckBoxes
                    {
                        Checked = (dtl_db.range_id > 0),
                        Value = "1"
                    });
                }
                else
                {

                    _item.selected_register_id = 0;
                    _item.selected_location_id = 0;
                    _item.selected_department_id = 0;
                    _item.selected_job_level_id = 0;
                    _item.selected_user_type_id = 0;
                    _item.selected_fl_approver = 0;   //di hide
                    _item.selected_range_id = 0;  //di hide

                    model.checkbox_approver.Add(new SelectedApprover_CheckBoxes
                    {
                        Checked = false,
                        Value = "1"
                    });
                }

                model.employee_details.Add(_item);
                i_pos += 1;
            }

            return View(model);
        }


        [HttpPost, ActionName("Setup")]
        [ValidateAntiForgeryToken]
        public ActionResult Setup(employee_setupViewModel emp_setup)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    emp_setup.ms_employee = db.ms_employee.Find(emp_setup.employee_id);
                    try
                    {
                        //List<ms_employee_detail> emp_detils = (from t in db.ms_employee_detail
                        //                                       where t.employee_id == emp_setup.employee_id
                        //                                       select t).ToList<ms_employee_detail>();
                        //if (emp_detils.Count > 0)
                        //{
                        //    foreach (var itm in emp_detils)
                        //    {
                        //        db.ms_employee_detail.Remove(itm);
                        //    }
                        //}

                        //emp_setup.company_list = (from t in db.ms_asmin_company where (t.fl_active == true && t.deleted_date == null) select t).ToList();
                        if (emp_setup.selected_company_id.Length > 0)
                        {
                            //karena di hide
                            if (emp_setup.selected_range_id == null)
                                emp_setup.selected_range_id = new int[emp_setup.selected_company_id.Length];

                            int i_loop = 0;
                            //foreach (var company_loop in emp_setup.company_list)
                            foreach (var _company_id in emp_setup.selected_company_id)
                            {
                                ms_employee_detail emp_detail = (from t in db.ms_employee_detail
                                                                 where (t.employee_id == emp_setup.employee_id && t.company_id == _company_id)
                                                                 select t).FirstOrDefault<ms_employee_detail>();

                                emp_setup.selected_range_id[i_loop] = 0;    //di hide

                                if (emp_detail != null) //update
                                {
                                    emp_detail.asset_reg_location_id = (emp_setup.selected_register_id[i_loop] > 0) ? emp_setup.selected_register_id[i_loop] : (int?)null;
                                    emp_detail.location_id = (emp_setup.selected_register_id[i_loop] > 0) ? emp_setup.selected_register_id[i_loop] : (int?)null;
                                    emp_detail.department_id = (emp_setup.selected_department_id[i_loop] > 0) ? emp_setup.selected_department_id[i_loop] : (int?)null;
                                    emp_detail.job_level_id = (emp_setup.selected_job_level_id[i_loop] > 0) ? emp_setup.selected_job_level_id[i_loop] : (int?)null;
                                    //emp_detail.user_type_id = (emp_setup.selected_user_type_id[i_loop] > 0) ? emp_setup.selected_user_type_id[i_loop] : (int?)null;

                                    emp_detail.range_id = (emp_setup.selected_range_id[i_loop] > 0) ? (int)emp_setup.selected_range_id[i_loop] : (int?)null;
                                    emp_detail.fl_approver = (emp_setup.selected_range_id[i_loop] > 0);

                                    emp_detail.fl_active = true;
                                    emp_detail.created_by = UserProfile.UserId;
                                    emp_detail.created_date = DateTime.Now;
                                    emp_detail.updated_by = UserProfile.UserId;
                                    emp_detail.updated_date = DateTime.Now;
                                    emp_detail.deleted_by = null;
                                    emp_detail.deleted_date = null;
                                    emp_detail.org_id = UserProfile.OrgId;

                                    db.Entry(emp_detail).State = EntityState.Modified;
                                }
                                else //insert
                                {
                                    db.ms_employee_detail.Add(new ms_employee_detail
                                    {
                                        employee_id = emp_setup.employee_id,
                                        company_id = _company_id,
                                        asset_reg_location_id = (emp_setup.selected_register_id[i_loop] > 0) ? emp_setup.selected_register_id[i_loop] : (int?)null,
                                        location_id = (emp_setup.selected_location_id[i_loop] > 0) ? emp_setup.selected_location_id[i_loop] : (int?)null,
                                        department_id = (emp_setup.selected_department_id[i_loop] > 0) ? emp_setup.selected_department_id[i_loop] : (int?)null,
                                        job_level_id = (emp_setup.selected_job_level_id[i_loop] > 0) ? emp_setup.selected_job_level_id[i_loop] : (int?)null,
                                        //user_type_id = (emp_setup.selected_user_type_id[i_loop] > 0) ? emp_setup.selected_user_type_id[i_loop] : (int?)null,

                                        range_id = (emp_setup.selected_range_id[i_loop] > 0) ? (int)emp_setup.selected_range_id[i_loop] : (int?)null,
                                        fl_approver = (emp_setup.selected_range_id[i_loop] > 0),

                                        fl_active = true,
                                        created_by = UserProfile.UserId,
                                        created_date = DateTime.Now,
                                        updated_by = UserProfile.UserId,
                                        updated_date = DateTime.Now,
                                        deleted_by = null,
                                        deleted_date = null,
                                        org_id = UserProfile.OrgId
                                    });
                                }
                                i_loop += 1;
                            }
                        }
                        db.SaveChanges();

                        transaction.Commit();
                        ViewBag.ResultMessage = "Record inserted into table successfully.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ViewBag.ResultMessage = string.Format("Error occured, records rolledback. {0}", ex.Message);
                    }
                }

            }
            return View(emp_setup);
        }

        [HttpGet]
        public JsonResult FetchRegisterLocation()
        {
            var data = db.ms_asset_register_location
                .Where(c => c.fl_active == true)
                .Select(l => new
                {
                    Value = l.asset_reg_location_id,
                    Text = l.asset_reg_location_name
                });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FetchAssetLocation(int reg_location_id)
        {
            var data = db.ms_asset_location
                .Where(l => l.asset_reg_location_id == reg_location_id && l.fl_active == true)
                .Select(l => new
                {
                    Value = l.location_id,
                    Text = l.location_name
                });
            return Json(data, JsonRequestBehavior.AllowGet);
        }



    }
}
