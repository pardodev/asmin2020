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
    public class employee_detailController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: employee_detail
        public ActionResult Index()
        {
            var ms_employee_detail = db.ms_employee_detail.Include(m => m.ms_approval_range).Include(m => m.ms_asmin_company).Include(m => m.ms_department).Include(m => m.ms_employee).Include(m => m.ms_job_level).Include(m => m.ms_user_type);
            return View(ms_employee_detail.ToList());
        }

        // GET: employee_detail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_employee_detail ms_employee_detail = db.ms_employee_detail.Find(id);
            if (ms_employee_detail == null)
            {
                return HttpNotFound();
            }
            return View(ms_employee_detail);
        }

        // GET: employee_detail/Create
        public ActionResult Create()
        {
            ViewBag.range_id = new SelectList(db.ms_approval_range, "range_id", "range_type");
            ViewBag.company_id = new SelectList(db.ms_asmin_company, "company_id", "company_code");
            ViewBag.department_id = new SelectList(db.ms_department, "department_id", "department_code");
            ViewBag.employee_id = new SelectList(db.ms_employee, "employee_id", "employee_nik");
            ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code");
            ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code");
            return View();
        }

        // POST: employee_detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "emp_det_id,employee_id,company_id,department_id,job_level_id,user_type_id,fl_approver,range_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_employee_detail ms_employee_detail)
        {
            if (ModelState.IsValid)
            {
                db.ms_employee_detail.Add(ms_employee_detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.range_id = new SelectList(db.ms_approval_range, "range_id", "range_type", ms_employee_detail.range_id);
            ViewBag.company_id = new SelectList(db.ms_asmin_company, "company_id", "company_code", ms_employee_detail.company_id);
            ViewBag.department_id = new SelectList(db.ms_department, "department_id", "department_code", ms_employee_detail.department_id);
            ViewBag.employee_id = new SelectList(db.ms_employee, "employee_id", "employee_nik", ms_employee_detail.employee_id);
            ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code", ms_employee_detail.job_level_id);
            ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code", ms_employee_detail.user_type_id);
            return View(ms_employee_detail);
        }

        // GET: employee_detail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_employee_detail ms_employee_detail = db.ms_employee_detail.Find(id);
            if (ms_employee_detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.range_id = new SelectList(db.ms_approval_range, "range_id", "range_type", ms_employee_detail.range_id);
            ViewBag.company_id = new SelectList(db.ms_asmin_company, "company_id", "company_code", ms_employee_detail.company_id);
            ViewBag.department_id = new SelectList(db.ms_department, "department_id", "department_code", ms_employee_detail.department_id);
            ViewBag.employee_id = new SelectList(db.ms_employee, "employee_id", "employee_nik", ms_employee_detail.employee_id);
            ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code", ms_employee_detail.job_level_id);
            ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code", ms_employee_detail.user_type_id);
            return View(ms_employee_detail);
        }

        // POST: employee_detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emp_det_id,employee_id,company_id,department_id,job_level_id,user_type_id,fl_approver,range_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_employee_detail ms_employee_detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_employee_detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.range_id = new SelectList(db.ms_approval_range, "range_id", "range_type", ms_employee_detail.range_id);
            ViewBag.company_id = new SelectList(db.ms_asmin_company, "company_id", "company_code", ms_employee_detail.company_id);
            ViewBag.department_id = new SelectList(db.ms_department, "department_id", "department_code", ms_employee_detail.department_id);
            ViewBag.employee_id = new SelectList(db.ms_employee, "employee_id", "employee_nik", ms_employee_detail.employee_id);
            ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code", ms_employee_detail.job_level_id);
            ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code", ms_employee_detail.user_type_id);
            return View(ms_employee_detail);
        }

        // GET: employee_detail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_employee_detail ms_employee_detail = db.ms_employee_detail.Find(id);
            if (ms_employee_detail == null)
            {
                return HttpNotFound();
            }
            return View(ms_employee_detail);
        }

        // POST: employee_detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_employee_detail ms_employee_detail = db.ms_employee_detail.Find(id);
            db.ms_employee_detail.Remove(ms_employee_detail);
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
