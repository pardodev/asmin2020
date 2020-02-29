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
    public class disposal_approvalController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: disposal_approval
        public ActionResult Index()
        {
            return View(db.tr_disposal_approval.ToList());
        }

        // GET: disposal_approval/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_approval tr_disposal_approval = db.tr_disposal_approval.Find(id);
            if (tr_disposal_approval == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_approval);
        }

        // GET: disposal_approval/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: disposal_approval/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "approval_id,request_id,approval_date,approval_location_id,approval_dept_id,approval_employee_id,approval_level_id,approval_status_id,approval_noted,approval_suggestion_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deteled_by,org_id")] tr_disposal_approval tr_disposal_approval)
        {
            if (ModelState.IsValid)
            {
                db.tr_disposal_approval.Add(tr_disposal_approval);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_disposal_approval);
        }

        // GET: disposal_approval/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_approval tr_disposal_approval = db.tr_disposal_approval.Find(id);
            if (tr_disposal_approval == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_approval);
        }

        // POST: disposal_approval/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "approval_id,request_id,approval_date,approval_location_id,approval_dept_id,approval_employee_id,approval_level_id,approval_status_id,approval_noted,approval_suggestion_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deteled_by,org_id")] tr_disposal_approval tr_disposal_approval)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_disposal_approval).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_disposal_approval);
        }

        // GET: disposal_approval/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_approval tr_disposal_approval = db.tr_disposal_approval.Find(id);
            if (tr_disposal_approval == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_approval);
        }

        // POST: disposal_approval/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_disposal_approval tr_disposal_approval = db.tr_disposal_approval.Find(id);
            db.tr_disposal_approval.Remove(tr_disposal_approval);
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
