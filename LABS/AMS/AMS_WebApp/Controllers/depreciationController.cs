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
    public class depreciationController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: depreciation
        public ActionResult Index()
        {
            return View(db.tr_depreciation.ToList());
        }

        // GET: depreciation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_depreciation tr_depreciation = db.tr_depreciation.Find(id);
            if (tr_depreciation == null)
            {
                return HttpNotFound();
            }
            return View(tr_depreciation);
        }

        // GET: depreciation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: depreciation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "depreciation_id,asset_id,depreciation_type_id,asset_original_value,asset_original_currency_id,Currency_kurs,asset_book_value,fis_asset_residu_value,fis_asset_usefull_life,fis_ddb_precentage,mkt_asset_residu_value,mkt_asset_usefull_life,mkt_ddb_percentage,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_depreciation tr_depreciation)
        {
            if (ModelState.IsValid)
            {
                db.tr_depreciation.Add(tr_depreciation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_depreciation);
        }

        // GET: depreciation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_depreciation tr_depreciation = db.tr_depreciation.Find(id);
            if (tr_depreciation == null)
            {
                return HttpNotFound();
            }
            return View(tr_depreciation);
        }

        // POST: depreciation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "depreciation_id,asset_id,depreciation_type_id,asset_original_value,asset_original_currency_id,Currency_kurs,asset_book_value,fis_asset_residu_value,fis_asset_usefull_life,fis_ddb_precentage,mkt_asset_residu_value,mkt_asset_usefull_life,mkt_ddb_percentage,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_depreciation tr_depreciation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_depreciation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_depreciation);
        }

        // GET: depreciation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_depreciation tr_depreciation = db.tr_depreciation.Find(id);
            if (tr_depreciation == null)
            {
                return HttpNotFound();
            }
            return View(tr_depreciation);
        }

        // POST: depreciation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_depreciation tr_depreciation = db.tr_depreciation.Find(id);
            db.tr_depreciation.Remove(tr_depreciation);
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
