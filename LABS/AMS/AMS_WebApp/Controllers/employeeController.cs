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
    public class employeeController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: employee
        public ActionResult Index()
        {
            return View(db.ms_employee.ToList());
        }

        // GET: employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_employee ms_employee = db.ms_employee.Find(id);
            if (ms_employee == null)
            {
                return HttpNotFound();
            }
            return View(ms_employee);
        }

        // GET: employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "employee_id,employee_nik,employee_name,employee_email,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id,ip_address")] ms_employee ms_employee)
        {
            if (ModelState.IsValid)
            {
                db.ms_employee.Add(ms_employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_employee);
        }

        // GET: employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_employee ms_employee = db.ms_employee.Find(id);
            if (ms_employee == null)
            {
                return HttpNotFound();
            }
            return View(ms_employee);
        }

        // POST: employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "employee_id,employee_nik,employee_name,employee_email,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id,ip_address")] ms_employee ms_employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_employee);
        }

        // GET: employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_employee ms_employee = db.ms_employee.Find(id);
            if (ms_employee == null)
            {
                return HttpNotFound();
            }
            return View(ms_employee);
        }

        // POST: employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_employee ms_employee = db.ms_employee.Find(id);
            db.ms_employee.Remove(ms_employee);
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
