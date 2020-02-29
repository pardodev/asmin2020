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
    public class asset_insuranceController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_insurance
        public ActionResult Index()
        {
            return View(db.tr_asset_insurance.ToList());
        }

        // GET: asset_insurance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_insurance tr_asset_insurance = db.tr_asset_insurance.Find(id);
            if (tr_asset_insurance == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_insurance);
        }

        // GET: asset_insurance/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asset_insurance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "insurance_activa_id,asset_id,insurance_activa_number,insurance_activa_name,insurance_activa_date,insurance_activa_exp_date,insurance_id,insurance_activa_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_int,org_id")] tr_asset_insurance tr_asset_insurance)
        {
            if (ModelState.IsValid)
            {
                db.tr_asset_insurance.Add(tr_asset_insurance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_asset_insurance);
        }

        // GET: asset_insurance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_insurance tr_asset_insurance = db.tr_asset_insurance.Find(id);
            if (tr_asset_insurance == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_insurance);
        }

        // POST: asset_insurance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "insurance_activa_id,asset_id,insurance_activa_number,insurance_activa_name,insurance_activa_date,insurance_activa_exp_date,insurance_id,insurance_activa_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_int,org_id")] tr_asset_insurance tr_asset_insurance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_asset_insurance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_asset_insurance);
        }

        // GET: asset_insurance/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_insurance tr_asset_insurance = db.tr_asset_insurance.Find(id);
            if (tr_asset_insurance == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_insurance);
        }

        // POST: asset_insurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_asset_insurance tr_asset_insurance = db.tr_asset_insurance.Find(id);
            db.tr_asset_insurance.Remove(tr_asset_insurance);
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
