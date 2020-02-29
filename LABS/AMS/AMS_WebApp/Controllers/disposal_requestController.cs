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
    public class disposal_requestController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: disposal_request
        public ActionResult Index()
        {
            return View(db.tr_disposal_request.ToList());
        }

        // GET: disposal_request/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_request tr_disposal_request = db.tr_disposal_request.Find(id);
            if (tr_disposal_request == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_request);
        }

        // GET: disposal_request/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: disposal_request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "request_id,asset_id,request_date,request_emp_id,request_dept_id,request_location_id,fl_approval,approval_date,request_status,fl_active,created_date,created_by,updated_date,update_by,deleted_date,deleted_by,org_id")] tr_disposal_request tr_disposal_request)
        {
            if (ModelState.IsValid)
            {
                db.tr_disposal_request.Add(tr_disposal_request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_disposal_request);
        }

        // GET: disposal_request/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_request tr_disposal_request = db.tr_disposal_request.Find(id);
            if (tr_disposal_request == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_request);
        }

        // POST: disposal_request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "request_id,asset_id,request_date,request_emp_id,request_dept_id,request_location_id,fl_approval,approval_date,request_status,fl_active,created_date,created_by,updated_date,update_by,deleted_date,deleted_by,org_id")] tr_disposal_request tr_disposal_request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_disposal_request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_disposal_request);
        }

        // GET: disposal_request/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_request tr_disposal_request = db.tr_disposal_request.Find(id);
            if (tr_disposal_request == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_request);
        }

        // POST: disposal_request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_disposal_request tr_disposal_request = db.tr_disposal_request.Find(id);
            db.tr_disposal_request.Remove(tr_disposal_request);
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
