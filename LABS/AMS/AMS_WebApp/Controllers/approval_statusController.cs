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
    public class approval_statusController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: approval_status
        public ActionResult Index()
        {
            return View(db.ms_approval_status.ToList());
        }

        // GET: approval_status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_approval_status ms_approval_status = db.ms_approval_status.Find(id);
            if (ms_approval_status == null)
            {
                return HttpNotFound();
            }
            return View(ms_approval_status);
        }

        // GET: approval_status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: approval_status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "approval_status_id,approval_status_code,approval_status_name")] ms_approval_status ms_approval_status)
        {
            if (ModelState.IsValid)
            {
                db.ms_approval_status.Add(ms_approval_status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_approval_status);
        }

        // GET: approval_status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_approval_status ms_approval_status = db.ms_approval_status.Find(id);
            if (ms_approval_status == null)
            {
                return HttpNotFound();
            }
            return View(ms_approval_status);
        }

        // POST: approval_status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "approval_status_id,approval_status_code,approval_status_name")] ms_approval_status ms_approval_status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_approval_status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_approval_status);
        }

        // GET: approval_status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_approval_status ms_approval_status = db.ms_approval_status.Find(id);
            if (ms_approval_status == null)
            {
                return HttpNotFound();
            }
            return View(ms_approval_status);
        }

        // POST: approval_status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_approval_status ms_approval_status = db.ms_approval_status.Find(id);
            db.ms_approval_status.Remove(ms_approval_status);
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
