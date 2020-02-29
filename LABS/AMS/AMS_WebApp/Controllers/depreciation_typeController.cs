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
    public class depreciation_typeController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: depreciation_type
        public ActionResult Index()
        {
            return View(db.ms_depreciation_type.ToList());
        }

        // GET: depreciation_type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_depreciation_type ms_depreciation_type = db.ms_depreciation_type.Find(id);
            if (ms_depreciation_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_depreciation_type);
        }

        // GET: depreciation_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: depreciation_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "depreciation_type_id,depreciation_type_code,depreciation_type_name,fl_active,created_date,created_by,update_date,update_by,deleted_date,deleted_by,org_id")] ms_depreciation_type ms_depreciation_type)
        {
            if (ModelState.IsValid)
            {
                db.ms_depreciation_type.Add(ms_depreciation_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_depreciation_type);
        }

        // GET: depreciation_type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_depreciation_type ms_depreciation_type = db.ms_depreciation_type.Find(id);
            if (ms_depreciation_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_depreciation_type);
        }

        // POST: depreciation_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "depreciation_type_id,depreciation_type_code,depreciation_type_name,fl_active,created_date,created_by,update_date,update_by,deleted_date,deleted_by,org_id")] ms_depreciation_type ms_depreciation_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_depreciation_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_depreciation_type);
        }

        // GET: depreciation_type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_depreciation_type ms_depreciation_type = db.ms_depreciation_type.Find(id);
            if (ms_depreciation_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_depreciation_type);
        }

        // POST: depreciation_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_depreciation_type ms_depreciation_type = db.ms_depreciation_type.Find(id);
            db.ms_depreciation_type.Remove(ms_depreciation_type);
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
