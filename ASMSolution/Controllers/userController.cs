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
    public class userController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: user
        public ActionResult Index()
        {
            var ms_user = db.ms_user.Include(m => m.ms_employee);
            return View(ms_user.ToList());
        }


        // GET: user/Details/5
        public ActionResult Details(int? id)
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
            return View(ms_user);
        }

        // GET: user/Create
        public ActionResult Create()
        {
            ViewBag.employee_id = new SelectList(db.ms_employee, "employee_id", "employee_nik");
            return View();
        }

        public JsonResult GetUserTypeList()
        {
            var UserTypeList = db.ms_user_type.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                t => new
                {
                    t.user_type_id,
                    user_type_name = t.user_type_code + " - " + t.user_type_name,
                }).ToList();
            return Json(UserTypeList, JsonRequestBehavior.AllowGet);
        }



        // POST: user/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,user_name,user_password,user_type_id,employee_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_user ms_user)
        {
            if (ModelState.IsValid)
            {
                db.ms_user.Add(ms_user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.employee_id = new SelectList(db.ms_employee, "employee_id", "employee_nik", ms_user.employee_id);
            return View(ms_user);
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
            ViewBag.employee_id = new SelectList(db.ms_employee, "employee_id", "employee_nik", ms_user.employee_id);
            return View(ms_user);
        }

        // POST: user/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,user_name,user_password,user_type_id,employee_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_user ms_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.employee_id = new SelectList(db.ms_employee, "employee_id", "employee_nik", ms_user.employee_id);
            return View(ms_user);
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
            return View(ms_user);
        }

        // POST: user/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_user ms_user = db.ms_user.Find(id);
            db.ms_user.Remove(ms_user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
