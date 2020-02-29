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
    public class asset_warrantyController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_warranty
        public ActionResult Index()
        {
            return View(db.tr_asset_warranty.ToList());
        }

        // GET: asset_warranty/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_warranty tr_asset_warranty = db.tr_asset_warranty.Find(id);
            if (tr_asset_warranty == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_warranty);
        }

        // GET: asset_warranty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asset_warranty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "warranty_id,asset_id,warranty_number,warranty_item_name,warranty_date,warranty_exp_date,warranty_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_asset_warranty tr_asset_warranty)
        {
            if (ModelState.IsValid)
            {
                db.tr_asset_warranty.Add(tr_asset_warranty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_asset_warranty);
        }

        // GET: asset_warranty/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_warranty tr_asset_warranty = db.tr_asset_warranty.Find(id);
            if (tr_asset_warranty == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_warranty);
        }

        // POST: asset_warranty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "warranty_id,asset_id,warranty_number,warranty_item_name,warranty_date,warranty_exp_date,warranty_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_asset_warranty tr_asset_warranty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_asset_warranty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_asset_warranty);
        }

        // GET: asset_warranty/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_warranty tr_asset_warranty = db.tr_asset_warranty.Find(id);
            if (tr_asset_warranty == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_warranty);
        }

        // POST: asset_warranty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_asset_warranty tr_asset_warranty = db.tr_asset_warranty.Find(id);
            db.tr_asset_warranty.Remove(tr_asset_warranty);
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
