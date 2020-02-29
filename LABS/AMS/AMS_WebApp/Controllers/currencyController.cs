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
    public class currencyController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: currency
        public ActionResult Index()
        {
            return View(db.ms_currency.ToList());
        }

        // GET: currency/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_currency ms_currency = db.ms_currency.Find(id);
            if (ms_currency == null)
            {
                return HttpNotFound();
            }
            return View(ms_currency);
        }

        // GET: currency/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: currency/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "currency_id,currency_code,currency_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_currency ms_currency)
        {
            if (ModelState.IsValid)
            {
                db.ms_currency.Add(ms_currency);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_currency);
        }

        // GET: currency/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_currency ms_currency = db.ms_currency.Find(id);
            if (ms_currency == null)
            {
                return HttpNotFound();
            }
            return View(ms_currency);
        }

        // POST: currency/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "currency_id,currency_code,currency_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_currency ms_currency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_currency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_currency);
        }

        // GET: currency/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_currency ms_currency = db.ms_currency.Find(id);
            if (ms_currency == null)
            {
                return HttpNotFound();
            }
            return View(ms_currency);
        }

        // POST: currency/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_currency ms_currency = db.ms_currency.Find(id);
            db.ms_currency.Remove(ms_currency);
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
