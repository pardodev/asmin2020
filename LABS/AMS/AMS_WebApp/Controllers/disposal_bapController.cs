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
    public class disposal_bapController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: disposal_bap
        public ActionResult Index()
        {
            return View(db.tr_disposal_bap.ToList());
        }

        // GET: disposal_bap/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_bap tr_disposal_bap = db.tr_disposal_bap.Find(id);
            if (tr_disposal_bap == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_bap);
        }

        // GET: disposal_bap/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: disposal_bap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "disposal_bap_id,disposal_id,disposal_dept_id,fl_disposal_process,disposal_bap_date,disposal_bap_description,disposal_upload_address,disposal_change_dept_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_disposal_bap tr_disposal_bap)
        {
            if (ModelState.IsValid)
            {
                db.tr_disposal_bap.Add(tr_disposal_bap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_disposal_bap);
        }

        // GET: disposal_bap/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_bap tr_disposal_bap = db.tr_disposal_bap.Find(id);
            if (tr_disposal_bap == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_bap);
        }

        // POST: disposal_bap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "disposal_bap_id,disposal_id,disposal_dept_id,fl_disposal_process,disposal_bap_date,disposal_bap_description,disposal_upload_address,disposal_change_dept_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_disposal_bap tr_disposal_bap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_disposal_bap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_disposal_bap);
        }

        // GET: disposal_bap/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_bap tr_disposal_bap = db.tr_disposal_bap.Find(id);
            if (tr_disposal_bap == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_bap);
        }

        // POST: disposal_bap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_disposal_bap tr_disposal_bap = db.tr_disposal_bap.Find(id);
            db.tr_disposal_bap.Remove(tr_disposal_bap);
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
