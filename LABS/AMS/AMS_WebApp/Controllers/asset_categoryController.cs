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
    public class asset_categoryController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_category
        public ActionResult Index()
        {
            return View(db.ms_asset_category.ToList());
        }

        // GET: asset_category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_category ms_asset_category = db.ms_asset_category.Find(id);
            if (ms_asset_category == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_category);
        }

        // GET: asset_category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asset_category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "category_id,category_code,category_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asset_category ms_asset_category)
        {
            if (ModelState.IsValid)
            {
                db.ms_asset_category.Add(ms_asset_category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_asset_category);
        }

        // GET: asset_category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_category ms_asset_category = db.ms_asset_category.Find(id);
            if (ms_asset_category == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_category);
        }

        // POST: asset_category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "category_id,category_code,category_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asset_category ms_asset_category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_asset_category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_asset_category);
        }

        // GET: asset_category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_category ms_asset_category = db.ms_asset_category.Find(id);
            if (ms_asset_category == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_category);
        }

        // POST: asset_category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_asset_category ms_asset_category = db.ms_asset_category.Find(id);
            db.ms_asset_category.Remove(ms_asset_category);
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
