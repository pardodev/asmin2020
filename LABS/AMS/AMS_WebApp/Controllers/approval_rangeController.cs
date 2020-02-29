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
    public class approval_rangeController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: approval_range
        public ActionResult Index()
        {
            return View(db.ms_approval_range.ToList());
        }

        // GET: approval_range/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_approval_range ms_approval_range = db.ms_approval_range.Find(id);
            if (ms_approval_range == null)
            {
                return HttpNotFound();
            }
            return View(ms_approval_range);
        }

        // GET: approval_range/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: approval_range/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "range_id,range_type,range_code,range_min,range_max,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_approval_range ms_approval_range)
        {
            if (ModelState.IsValid)
            {
                db.ms_approval_range.Add(ms_approval_range);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_approval_range);
        }

        // GET: approval_range/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_approval_range ms_approval_range = db.ms_approval_range.Find(id);
            if (ms_approval_range == null)
            {
                return HttpNotFound();
            }
            return View(ms_approval_range);
        }

        // POST: approval_range/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "range_id,range_type,range_code,range_min,range_max,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_approval_range ms_approval_range)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_approval_range).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_approval_range);
        }

        // GET: approval_range/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_approval_range ms_approval_range = db.ms_approval_range.Find(id);
            if (ms_approval_range == null)
            {
                return HttpNotFound();
            }
            return View(ms_approval_range);
        }

        // POST: approval_range/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_approval_range ms_approval_range = db.ms_approval_range.Find(id);
            db.ms_approval_range.Remove(ms_approval_range);
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
