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
    public class asset_locationController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_location
        public ActionResult Index()
        {
            return View(db.ms_asset_location.ToList());
        }

        // GET: asset_location/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_location ms_asset_location = db.ms_asset_location.Find(id);
            if (ms_asset_location == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_location);
        }

        // GET: asset_location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asset_location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "location_id,location_code,location_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asset_location ms_asset_location)
        {
            if (ModelState.IsValid)
            {
                db.ms_asset_location.Add(ms_asset_location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_asset_location);
        }

        // GET: asset_location/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_location ms_asset_location = db.ms_asset_location.Find(id);
            if (ms_asset_location == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_location);
        }

        // POST: asset_location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "location_id,location_code,location_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asset_location ms_asset_location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_asset_location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_asset_location);
        }

        // GET: asset_location/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_location ms_asset_location = db.ms_asset_location.Find(id);
            if (ms_asset_location == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_location);
        }

        // POST: asset_location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_asset_location ms_asset_location = db.ms_asset_location.Find(id);
            db.ms_asset_location.Remove(ms_asset_location);
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
