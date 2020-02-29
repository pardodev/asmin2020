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
    public class mutation_requestController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: mutation_request
        public ActionResult Index()
        {
            return View(db.tr_mutation_request.ToList());
        }

        // GET: mutation_request/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_mutation_request tr_mutation_request = db.tr_mutation_request.Find(id);
            if (tr_mutation_request == null)
            {
                return HttpNotFound();
            }
            return View(tr_mutation_request);
        }

        // GET: mutation_request/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mutation_request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "request_id,asset_id,request_date,request_emp_id,request_dept_id,request_location_id,request_level_id,transfer_to_location_id,transfer_to_dept_id,transfer_to_emp_id,fl_approval,approval_date,request_status,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_mutation_request tr_mutation_request)
        {
            if (ModelState.IsValid)
            {
                db.tr_mutation_request.Add(tr_mutation_request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_mutation_request);
        }

        // GET: mutation_request/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_mutation_request tr_mutation_request = db.tr_mutation_request.Find(id);
            if (tr_mutation_request == null)
            {
                return HttpNotFound();
            }
            return View(tr_mutation_request);
        }

        // POST: mutation_request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "request_id,asset_id,request_date,request_emp_id,request_dept_id,request_location_id,request_level_id,transfer_to_location_id,transfer_to_dept_id,transfer_to_emp_id,fl_approval,approval_date,request_status,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_mutation_request tr_mutation_request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_mutation_request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_mutation_request);
        }

        // GET: mutation_request/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_mutation_request tr_mutation_request = db.tr_mutation_request.Find(id);
            if (tr_mutation_request == null)
            {
                return HttpNotFound();
            }
            return View(tr_mutation_request);
        }

        // POST: mutation_request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_mutation_request tr_mutation_request = db.tr_mutation_request.Find(id);
            db.tr_mutation_request.Remove(tr_mutation_request);
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
