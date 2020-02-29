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
    public class mutation_approvalController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: mutation_approval
        public ActionResult Index()
        {
            return View(db.tr_mutation_approval.ToList());
        }

        // GET: mutation_approval/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_mutation_approval tr_mutation_approval = db.tr_mutation_approval.Find(id);
            if (tr_mutation_approval == null)
            {
                return HttpNotFound();
            }
            return View(tr_mutation_approval);
        }

        // GET: mutation_approval/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mutation_approval/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "approval_id,request_id,approval_date,approval_location_id,approval_dept_id,approval_employee_id,approval_level_id,approval_status_id,approval_noted,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deteled_by,org_id")] tr_mutation_approval tr_mutation_approval)
        {
            if (ModelState.IsValid)
            {
                db.tr_mutation_approval.Add(tr_mutation_approval);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_mutation_approval);
        }

        // GET: mutation_approval/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_mutation_approval tr_mutation_approval = db.tr_mutation_approval.Find(id);
            if (tr_mutation_approval == null)
            {
                return HttpNotFound();
            }
            return View(tr_mutation_approval);
        }

        // POST: mutation_approval/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "approval_id,request_id,approval_date,approval_location_id,approval_dept_id,approval_employee_id,approval_level_id,approval_status_id,approval_noted,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deteled_by,org_id")] tr_mutation_approval tr_mutation_approval)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_mutation_approval).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_mutation_approval);
        }

        // GET: mutation_approval/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_mutation_approval tr_mutation_approval = db.tr_mutation_approval.Find(id);
            if (tr_mutation_approval == null)
            {
                return HttpNotFound();
            }
            return View(tr_mutation_approval);
        }

        // POST: mutation_approval/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_mutation_approval tr_mutation_approval = db.tr_mutation_approval.Find(id);
            db.tr_mutation_approval.Remove(tr_mutation_approval);
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
