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
    public class asset_registrationController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_registration
        public ActionResult Index()
        {
            return View(db.tr_asset_registration.ToList());
        }

        // GET: asset_registration/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_registration tr_asset_registration = db.tr_asset_registration.Find(id);
            if (tr_asset_registration == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_registration);
        }

        // GET: asset_registration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asset_registration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "asset_id,asset_type_id,asset_number,company_id,asset_reg_location_id,asset_reg_pic_id,category_id,asset_po_number,asset_do_number,asset_name,asset_merk,asset_serial_number,vendor_id,asset_receipt_date,location_id,department_id,asset_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_asset_registration tr_asset_registration)
        {
            if (ModelState.IsValid)
            {
                db.tr_asset_registration.Add(tr_asset_registration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_asset_registration);
        }

        // GET: asset_registration/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_registration tr_asset_registration = db.tr_asset_registration.Find(id);
            if (tr_asset_registration == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_registration);
        }

        // POST: asset_registration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "asset_id,asset_type_id,asset_number,company_id,asset_reg_location_id,asset_reg_pic_id,category_id,asset_po_number,asset_do_number,asset_name,asset_merk,asset_serial_number,vendor_id,asset_receipt_date,location_id,department_id,asset_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_asset_registration tr_asset_registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_asset_registration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_asset_registration);
        }

        // GET: asset_registration/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_registration tr_asset_registration = db.tr_asset_registration.Find(id);
            if (tr_asset_registration == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_registration);
        }

        // POST: asset_registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_asset_registration tr_asset_registration = db.tr_asset_registration.Find(id);
            db.tr_asset_registration.Remove(tr_asset_registration);
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
