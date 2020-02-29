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
    public class depreciation_detailController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: depreciation_detail
        public ActionResult Index()
        {
            return View(db.tr_depreciation_detail.ToList());
        }

        // GET: depreciation_detail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_depreciation_detail tr_depreciation_detail = db.tr_depreciation_detail.Find(id);
            if (tr_depreciation_detail == null)
            {
                return HttpNotFound();
            }
            return View(tr_depreciation_detail);
        }

        // GET: depreciation_detail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: depreciation_detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "depreciation_detail_id,depreciation_id,depreciation_type_id,period,period_year,period_month,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_depreciation_detail tr_depreciation_detail)
        {
            if (ModelState.IsValid)
            {
                db.tr_depreciation_detail.Add(tr_depreciation_detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_depreciation_detail);
        }

        // GET: depreciation_detail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_depreciation_detail tr_depreciation_detail = db.tr_depreciation_detail.Find(id);
            if (tr_depreciation_detail == null)
            {
                return HttpNotFound();
            }
            return View(tr_depreciation_detail);
        }

        // POST: depreciation_detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "depreciation_detail_id,depreciation_id,depreciation_type_id,period,period_year,period_month,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_depreciation_detail tr_depreciation_detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_depreciation_detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_depreciation_detail);
        }

        // GET: depreciation_detail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_depreciation_detail tr_depreciation_detail = db.tr_depreciation_detail.Find(id);
            if (tr_depreciation_detail == null)
            {
                return HttpNotFound();
            }
            return View(tr_depreciation_detail);
        }

        // POST: depreciation_detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_depreciation_detail tr_depreciation_detail = db.tr_depreciation_detail.Find(id);
            db.tr_depreciation_detail.Remove(tr_depreciation_detail);
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
