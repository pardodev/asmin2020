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
    public class departmentController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: department
        public ActionResult Index()
        {
            return View(db.ms_department.ToList());
        }

        // GET: department/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_department ms_department = db.ms_department.Find(id);
            if (ms_department == null)
            {
                return HttpNotFound();
            }
            return View(ms_department);
        }

        // GET: department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "department_code,department_name,department_email")] ms_department ms_department)
        {
            if (ModelState.IsValid)
            {
                db.ms_department.Add(ms_department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_department);
        }

        // GET: department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_department ms_department = db.ms_department.Find(id);
            if (ms_department == null)
            {
                return HttpNotFound();
            }
            return View(ms_department);
        }

        // POST: department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "department_id,department_code,department_name,department_email,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_department ms_department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_department);
        }

        // GET: department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_department ms_department = db.ms_department.Find(id);
            if (ms_department == null)
            {
                return HttpNotFound();
            }
            return View(ms_department);
        }

        // POST: department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_department ms_department = db.ms_department.Find(id);
            db.ms_department.Remove(ms_department);
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
