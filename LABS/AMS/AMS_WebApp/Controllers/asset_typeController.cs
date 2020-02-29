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
    public class asset_typeController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_type
        public ActionResult Index()
        {
            return View(db.ms_asset_type.ToList());
        }

        // GET: asset_type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_type ms_asset_type = db.ms_asset_type.Find(id);
            if (ms_asset_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_type);
        }

        // GET: asset_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asset_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "activa_type_id,asset_type_code,asset_type_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asset_type ms_asset_type)
        {
            if (ModelState.IsValid)
            {
                db.ms_asset_type.Add(ms_asset_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_asset_type);
        }

        // GET: asset_type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_type ms_asset_type = db.ms_asset_type.Find(id);
            if (ms_asset_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_type);
        }

        // POST: asset_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "activa_type_id,asset_type_code,asset_type_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asset_type ms_asset_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_asset_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_asset_type);
        }

        // GET: asset_type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_type ms_asset_type = db.ms_asset_type.Find(id);
            if (ms_asset_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_type);
        }

        // POST: asset_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_asset_type ms_asset_type = db.ms_asset_type.Find(id);
            db.ms_asset_type.Remove(ms_asset_type);
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
