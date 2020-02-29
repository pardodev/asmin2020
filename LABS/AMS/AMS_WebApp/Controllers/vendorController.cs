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
    public class vendorController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: vendor
        public ActionResult Index()
        {
            var ms_vendor = db.ms_vendor.Include(m => m.ms_country);
            return View(ms_vendor.ToList());
        }

        // GET: vendor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_vendor ms_vendor = db.ms_vendor.Find(id);
            if (ms_vendor == null)
            {
                return HttpNotFound();
            }
            return View(ms_vendor);
        }

        // GET: vendor/Create
        public ActionResult Create()
        {
            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code");
            return View();
        }

        // POST: vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "vendor_id,vendor_code,vendor_name,vendor_address,country_id,vendor_phone,vendor_mail,vendor_cp_name,vendor_cp_phone,vendor_cp_mail,vendor_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_vendor ms_vendor)
        {
            if (ModelState.IsValid)
            {
                db.ms_vendor.Add(ms_vendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code", ms_vendor.country_id);
            return View(ms_vendor);
        }

        // GET: vendor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_vendor ms_vendor = db.ms_vendor.Find(id);
            if (ms_vendor == null)
            {
                return HttpNotFound();
            }
            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code", ms_vendor.country_id);
            return View(ms_vendor);
        }

        // POST: vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "vendor_id,vendor_code,vendor_name,vendor_address,country_id,vendor_phone,vendor_mail,vendor_cp_name,vendor_cp_phone,vendor_cp_mail,vendor_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_vendor ms_vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code", ms_vendor.country_id);
            return View(ms_vendor);
        }

        // GET: vendor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_vendor ms_vendor = db.ms_vendor.Find(id);
            if (ms_vendor == null)
            {
                return HttpNotFound();
            }
            return View(ms_vendor);
        }

        // POST: vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_vendor ms_vendor = db.ms_vendor.Find(id);
            db.ms_vendor.Remove(ms_vendor);
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
