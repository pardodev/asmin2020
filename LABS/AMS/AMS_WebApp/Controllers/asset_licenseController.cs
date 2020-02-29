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
    public class asset_licenseController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_license
        public ActionResult Index()
        {
            return View(db.tr_asset_license.ToList());
        }

        // GET: asset_license/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_license tr_asset_license = db.tr_asset_license.Find(id);
            if (tr_asset_license == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_license);
        }

        // GET: asset_license/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asset_license/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "license_id,asset_id,license_number,license_name,license_issued_by,license_date,license_exp_date,license_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_asset_license tr_asset_license)
        {
            if (ModelState.IsValid)
            {
                db.tr_asset_license.Add(tr_asset_license);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_asset_license);
        }

        // GET: asset_license/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_license tr_asset_license = db.tr_asset_license.Find(id);
            if (tr_asset_license == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_license);
        }

        // POST: asset_license/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "license_id,asset_id,license_number,license_name,license_issued_by,license_date,license_exp_date,license_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_asset_license tr_asset_license)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_asset_license).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_asset_license);
        }

        // GET: asset_license/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_license tr_asset_license = db.tr_asset_license.Find(id);
            if (tr_asset_license == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_license);
        }

        // POST: asset_license/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_asset_license tr_asset_license = db.tr_asset_license.Find(id);
            db.tr_asset_license.Remove(tr_asset_license);
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
