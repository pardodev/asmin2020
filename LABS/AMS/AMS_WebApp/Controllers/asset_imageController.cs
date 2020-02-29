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
    public class asset_imageController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_image
        public ActionResult Index()
        {
            return View(db.tr_asset_image.ToList());
        }

        // GET: asset_image/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_image tr_asset_image = db.tr_asset_image.Find(id);
            if (tr_asset_image == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_image);
        }

        // GET: asset_image/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asset_image/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "asset_id,asset_img_address")] tr_asset_image tr_asset_image)
        {
            if (ModelState.IsValid)
            {
                db.tr_asset_image.Add(tr_asset_image);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_asset_image);
        }

        // GET: asset_image/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_image tr_asset_image = db.tr_asset_image.Find(id);
            if (tr_asset_image == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_image);
        }

        // POST: asset_image/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "asset_id,asset_img_address")] tr_asset_image tr_asset_image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_asset_image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_asset_image);
        }

        // GET: asset_image/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_image tr_asset_image = db.tr_asset_image.Find(id);
            if (tr_asset_image == null)
            {
                return HttpNotFound();
            }
            return View(tr_asset_image);
        }

        // POST: asset_image/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_asset_image tr_asset_image = db.tr_asset_image.Find(id);
            db.tr_asset_image.Remove(tr_asset_image);
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
