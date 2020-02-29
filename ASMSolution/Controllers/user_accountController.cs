using System;
using System.IO;
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
    public class user_accountController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: user
        public ActionResult Index()
        {
            //var _list = (from t1 in db.ms_user
            //             where (t1.deleted_date == null)
            //             join t2 in db.ms_employee on t1.employee_id equals t2.employee_id
            //             join u in db.ms_user on t1.updated_by equals u.user_id
            //             into t_joined
            //             from row_join in t_joined.DefaultIfEmpty()
            //             from usr in db.ms_user.Where(rec => (rec == null) ? false : rec.user_id == row_join.user_id).DefaultIfEmpty()
            //             select new
            //             {
            //                 user_id = t1.user_id,
            //                 user_name = t1.user_name,
            //                 employee_id = t2.employee_id,
            //                 employee_nik = t2.employee_nik,
            //                 employee_name = t2.employee_name,
            //                 employee_email = t2.employee_email,
            //                 rec_isactive = (t1.fl_active == true) ? "Yes" : "No",
            //                 //rec_isactive_2 = Enum.GetName(typeof(EnumBooleanKey), t1.fl_active),
            //                 rec_modified_by = (usr == null) ? string.Empty : usr.user_name,
            //                 rec_modified_date = t1.updated_date
            //             }).ToList()
            //             .Select(c => new user_accountViewModel()
            //             {
            //                 user_id = c.user_id,
            //                 user_name = c.user_name,
            //                 employee_id = c.employee_id,
            //                 employee_nik = c.employee_nik,
            //                 employee_name = c.employee_name,
            //                 employee_email = c.employee_email,
            //                 rec_isactive = c.rec_isactive,
            //                 rec_modified_by = c.rec_modified_by,
            //                 rec_modified_date = c.rec_modified_date
            //             });

            //return View(_list);
            return View();
        }

        public JsonResult List(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var _list = (from u1 in db.ms_user
                         join u3 in db.ms_user_type on u1.user_type_id equals u3.user_type_id
                         where (u1.deleted_date == null)
                         join e in db.ms_employee on u1.employee_id equals e.employee_id
                         join u2 in db.ms_user on u1.updated_by equals u2.user_id
                         into t_joined
                         from row_join in t_joined.DefaultIfEmpty()
                         from usr in db.ms_user.Where(rec_udated_by => (rec_udated_by == null) ? false : rec_udated_by.user_id == row_join.user_id).DefaultIfEmpty()

                         select new
                         {
                             user_id = u1.user_id,
                             user_name = u1.user_name,
                             user_password = u1.user_password,
                             user_type_id = u1.user_type_id,
                             user_type_name = u3.user_type_name,

                             employee_id = e.employee_id,
                             employee_nik = e.employee_nik,
                             employee_name = e.employee_name,
                             employee_email = e.employee_email,
                             rec_isactive = (u1.fl_active == true) ? "Yes" : "No",
                             //rec_isactive_2 = Enum.GetName(typeof(EnumBooleanKey), u1.fl_active),
                             rec_modified_by = (usr == null) ? string.Empty : usr.user_name,
                             rec_modified_date = u1.updated_date
                         }).ToList()
                        .Select(c => new user_accountViewModel()
                        {
                            user_id = c.user_id,
                            user_name = c.user_name,
                            user_password = c.user_password,
                            user_type_id = c.user_type_id,
                            user_type_name = c.user_type_name,
                            employee_id = c.employee_id,
                            employee_nik = c.employee_nik,
                            employee_name = c.employee_name,
                            employee_email = c.employee_email,
                            rec_isactive = c.rec_isactive,
                            rec_modified_by = c.rec_modified_by,
                            rec_modified_date = c.rec_modified_date
                        });

            // search function
            if (_search)
            {
                switch (searchField)
                {
                    case "user_name":
                        _list = _list.Where(t => t.user_name.Contains(searchString));
                        break;
                    case "employee_nik":
                        _list = _list.Where(t => t.employee_nik.Contains(searchString));
                        break;
                    case "employee_name":
                        _list = _list.Where(t => t.employee_name.Contains(searchString));
                        break;
                    case "employee_email":
                        _list = _list.Where(t => t.employee_email.Contains(searchString));
                        break;
                }
            }
            //calc paging
            int totalRecords = _list.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            //default sorting
            if (sord.ToUpper() == "DESC")
            {
                _list = _list.OrderByDescending(t => t.user_name);
                _list = _list.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                _list = _list.OrderBy(t => t.user_name);
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


        ////GET: user/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ms_user ms_user = db.ms_user.Find(id);
        //    if (ms_user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ms_user);
        //}

        public ActionResult ModalFrm(int? id = 0)
        {
            user_accountViewModel usr_acc = null;
            int user_id = (int)id;
            ms_user ms_user = db.ms_user.Find(user_id);
            if (ms_user == null)
            {
                usr_acc = new user_accountViewModel()
                {
                    fl_active = true,
                    rec_isactive = "Yes",
                    user_id = 0,
                    user_type_id = 0,
                    employee_id = 0
                };
            }
            else
            {
                usr_acc = new user_accountViewModel()
                {
                    fl_active = true,
                    rec_isactive = "Yes",
                    user_id = ms_user.user_id,
                    user_name = ms_user.user_name,
                    user_password =  App_Helpers.CryptorHelper.Decrypt(ms_user.user_password, "MD5", true),
                    user_type_id = ms_user.user_type_id,
                    user_type_name = ms_user.ms_user_type.user_type_name,

                    employee_id = ms_user.ms_employee.employee_id,
                    employee_nik = ms_user.ms_employee.employee_nik,
                    employee_name = ms_user.ms_employee.employee_name,
                    employee_email = ms_user.ms_employee.employee_email
                };

            }

            List<SelectListItem> first = new List<SelectListItem> { new SelectListItem { Text = "- [Not Set] -", Value = "0", Selected = true } };
            usr_acc.sli_user_type_list = (from t in db.ms_user_type
                                          where (t.fl_active == true && t.deleted_date == null)
                                          select new SelectListItem
                                          {
                                              Text = t.user_type_name,
                                              Value = t.user_type_id.ToString()
                                          }
                          ).ToList<SelectListItem>().Union(first);

            usr_acc.FormMode = (user_id > 0) ? EnumFormModeKey.Form_Edit : EnumFormModeKey.Form_New;
            return PartialView(usr_acc);
        }


        public ActionResult EmployeeModal()
        {
            return PartialView();
        }

        /// <summary>
        /// data untuk employee modal, 
        /// 
        /// tampilkan hanya yang belum memiliki use id. user id yang di nonaktifkan, 
        /// masi bisa dipakai tinggal akifkan lagi aja
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeList(int? id = 0)
        {
            db.Configuration.ProxyCreationEnabled = false;
            int employee_id = (int)(id);
            if (employee_id == 0)
            {
                var _list = from t in db.ms_employee
                            where t.fl_active == true && t.deleted_date == null 
                            //&& !(from u in db.ms_user select u.employee_id).ToList().Contains(t.employee_id)
                            select new
                            {
                                t.employee_id,
                                t.employee_nik,
                                t.employee_name,
                                t.employee_email
                            };

                return Json(new { data = _list.ToList() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var _list = from t in db.ms_employee
                            where t.fl_active == true && t.deleted_date == null 
                            && t.employee_id == employee_id 
                            //&& !(from u in db.ms_user select u.employee_id).ToList().Contains(t.employee_id)
                            select new
                            {
                                t.employee_id,
                                t.employee_nik,
                                t.employee_name,
                                t.employee_email
                            };

                return Json(_list.ToList(), JsonRequestBehavior.AllowGet);
            }
        }


        //// GET: user/Create
        //public ActionResult Create()
        //{
        //    user_accountViewModel usr_acc = new user_accountViewModel()
        //    {
        //        fl_active = true,
        //        rec_isactive = "Yes",
        //        user_id = 0,
        //        employee_id = 0
        //    };
        //    //ViewBag.employee_id = new SelectList(db.ms_employee, "employee_id", "employee_nik");
        //    return View(usr_acc);
        //}

        // POST: user/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,user_name,user_password,user_type_id,employee_id,fl_active")] user_accountViewModel user_acc)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ms_employee emp = db.ms_employee.Find(user_acc.employee_id);
                        if (emp == null)
                        {
                            throw new Exception("Employee not found " + user_acc.employee_id.ToString());
                        }
                        else
                        {
                            user_acc.employee_nik = emp.employee_nik;
                            user_acc.employee_name = emp.employee_name;
                            user_acc.employee_email = emp.employee_email;

                            emp.fl_active = true;
                            emp.updated_by = UserProfile.UserId;
                            emp.updated_date = DateTime.Now;
                            emp.deleted_by = null;
                            emp.deleted_date = null;
                            db.Entry(emp).State = EntityState.Modified;

                            ms_user ms_user = new ms_user()
                            {
                                user_name = user_acc.user_name,
                                user_password = App_Helpers.CryptorHelper.Encrypt(user_acc.user_password, "MD5", true),
                                user_type_id = user_acc.user_type_id,
                                employee_id = emp.employee_id,


                                fl_active = user_acc.fl_active,
                                created_by = UserProfile.UserId,
                                created_date = DateTime.Now,
                                updated_by = UserProfile.UserId,
                                updated_date = DateTime.Now,
                                deleted_by = null,
                                deleted_date = null
                            };
                            db.ms_user.Add(ms_user);

                            db.SaveChanges();
                        }

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
            return View(user_acc);
        }

        // GET: user/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_user ms_user = db.ms_user.Find(id);
            if (ms_user == null)
            {
                return HttpNotFound();
            }

            user_accountViewModel usr_acc = new user_accountViewModel()
            {
                fl_active = ms_user.fl_active,
                user_id = ms_user.user_id,
                user_name = ms_user.user_name,
                user_password = ms_user.user_password,
                user_type_id = ms_user.user_type_id,

                employee_id = ms_user.employee_id,
                employee_nik = ms_user.ms_employee.employee_nik,
                employee_name = ms_user.ms_employee.employee_name,
                employee_email = ms_user.ms_employee.employee_email,
            };

            return View(usr_acc);
        }

        // POST: user/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,user_name,user_password,user_type_id,employee_id,fl_active")] user_accountViewModel user_acc)
        {
            if (ModelState.IsValid)
            {
                ms_user ms_user = db.ms_user.Find(user_acc.user_id);
                ms_user.user_name = user_acc.user_name;

                if (!string.IsNullOrWhiteSpace(user_acc.user_password))
                    ms_user.user_password = App_Helpers.CryptorHelper.Encrypt(user_acc.user_password, "MD5", true);

                ms_user.user_type_id = user_acc.user_type_id;
                ms_user.fl_active = true;
                ms_user.updated_by = UserProfile.UserId;
                ms_user.updated_date = DateTime.Now;
                ms_user.deleted_by = null;
                ms_user.deleted_date = null;

                ms_employee emp = db.ms_employee.Find(user_acc.employee_id);
                if (emp != null)
                {
                    ms_user.employee_id = emp.employee_id;
                    emp.fl_active = true;
                    emp.updated_by = UserProfile.UserId;
                    emp.updated_date = DateTime.Now;
                    emp.deleted_by = null;
                    emp.deleted_date = null;
                    db.Entry(emp).State = EntityState.Modified;
                }

                db.Entry(ms_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user_acc);
        }

        // GET: user/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_user ms_user = db.ms_user.Find(id);
            if (ms_user == null)
            {
                return HttpNotFound();
            }
            //return View(ms_user);
            ms_user.fl_active = false;
            ms_user.deleted_by = UserProfile.UserId;
            ms_user.deleted_date = DateTime.Now;
            db.Entry(ms_user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // POST: user/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //ms_user ms_user = db.ms_user.Find(id);
            //db.ms_user.Remove(ms_user);
            //db.SaveChanges();
            ms_user ms_user = db.ms_user.Find(id);
            ms_user.fl_active = false;
            ms_user.deleted_by = UserProfile.UserId;
            ms_user.deleted_date = DateTime.Now;
            db.Entry(ms_user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public JsonResult Cruduser_account()
        {
            if (UserProfile.UserId != 0)
            {
                if (Request.Form["oper"] == "del")
                {
                    //for delete process
                    string ids = Request.Form["id"];
                    int id = Convert.ToInt32(ids);
                    ms_user ms_user = db.ms_user.Find(id);
                    ms_user.fl_active = false;
                    ms_user.deleted_by = UserProfile.UserId;
                    ms_user.deleted_date = DateTime.Now;
                    db.Entry(ms_user).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("Delete", JsonRequestBehavior.AllowGet);
                   
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Session", JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDataUserAccount([Bind(Include = "user_id,user_name,user_password,user_type_id,employee_id,fl_active")] user_accountViewModel user_acc)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region INSERT_DATA
                        if (user_acc.FormMode == EnumFormModeKey.Form_New)
                        {
                            ms_employee emp = db.ms_employee.Find(user_acc.employee_id);
                            if (emp == null)
                            {
                                throw new Exception("Employee not found. Please create Employee first " + user_acc.employee_id.ToString());
                            }
                            else
                            {
                                user_acc.employee_nik = emp.employee_nik;
                                user_acc.employee_name = emp.employee_name;
                                user_acc.employee_email = emp.employee_email;

                                emp.fl_active = true;
                                emp.updated_by = UserProfile.UserId;
                                emp.updated_date = DateTime.Now;
                                emp.deleted_by = null;
                                emp.deleted_date = null;
                                db.Entry(emp).State = EntityState.Modified;

                                ms_user ms_user = db.ms_user.Find(user_acc.user_id);
                                if (ms_user == null)
                                {
                                    ms_user = new ms_user()
                                    {
                                        user_name = user_acc.user_name,
                                        user_password = App_Helpers.CryptorHelper.Encrypt(user_acc.user_password, "MD5", true),
                                        user_type_id = user_acc.user_type_id,
                                        employee_id = emp.employee_id,

                                        fl_active = user_acc.fl_active,
                                        created_by = UserProfile.UserId,
                                        created_date = DateTime.Now,
                                        updated_by = UserProfile.UserId,
                                        updated_date = DateTime.Now,
                                        deleted_by = null,
                                        deleted_date = null
                                    };
                                    db.ms_user.Add(ms_user);
                                }
                                else
                                {
                                    ms_user.user_name = user_acc.user_name;
                                    ms_user.user_password = App_Helpers.CryptorHelper.Encrypt(user_acc.user_password, "MD5", true);
                                    ms_user.user_type_id = user_acc.user_type_id;

                                    ms_user.employee_id = user_acc.employee_id;
                                    ms_user.fl_active = user_acc.fl_active;
                                    ms_user.created_by = UserProfile.UserId;
                                    ms_user.created_date = DateTime.Now;
                                    ms_user.updated_by = UserProfile.UserId;
                                    ms_user.updated_date = DateTime.Now;
                                    ms_user.deleted_by = null;
                                    ms_user.deleted_date = null;
                                    db.Entry(ms_user).State = EntityState.Modified;
                                }
                                db.SaveChanges();
                            }

                            ViewBag.ResultMessage = "Record inserted successfully.";

                        }
                        #endregion
                        
                        #region UPDATE_DATA
                        else
                        {
                            ms_user ms_user = db.ms_user.Find(user_acc.user_id);
                            ms_user.user_name = user_acc.user_name;

                            if (!string.IsNullOrWhiteSpace(user_acc.user_password))
                                ms_user.user_password = App_Helpers.CryptorHelper.Encrypt(user_acc.user_password, "MD5", true);

                            ms_user.user_type_id = user_acc.user_type_id;
                            ms_user.fl_active = true;
                            ms_user.updated_by = UserProfile.UserId;
                            ms_user.updated_date = DateTime.Now;
                            ms_user.deleted_by = null;
                            ms_user.deleted_date = null;

                            ms_employee emp = db.ms_employee.Find(user_acc.employee_id);
                            if (emp != null)
                            {
                                ms_user.employee_id = emp.employee_id;
                                emp.fl_active = true;
                                emp.updated_by = UserProfile.UserId;
                                emp.updated_date = DateTime.Now;
                                emp.deleted_by = null;
                                emp.deleted_date = null;
                                db.Entry(emp).State = EntityState.Modified;
                            }

                            db.Entry(ms_user).State = EntityState.Modified;

                            ViewBag.ResultMessage = "Record updated successfully.";
                        }
                        #endregion

                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ViewBag.ResultMessage = string.Format("Error occured, records rolledback. {0}", ex.Message);
                    }

                }
            }
            return View(user_acc);

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