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
    public class courierController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: courier
        public ActionResult Index()
        {
            var ms_courier = db.ms_courier.Include(m => m.ms_country);
            return View(ms_courier.ToList());
        }

        // GET: courier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_courier ms_courier = db.ms_courier.Find(id);
            if (ms_courier == null)
            {
                return HttpNotFound();
            }
            return View(ms_courier);
        }

        // GET: courier/Create
        public ActionResult Create()
        {
            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code");
            return View();
        }

        // POST: courier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "courier_id,courier_code,courier_name,courier_address,country_id,courier_phone,courier_mail,courier_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_courier ms_courier)
        {
            if (ModelState.IsValid)
            {
                db.ms_courier.Add(ms_courier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code", ms_courier.country_id);
            return View(ms_courier);
        }

        // GET: courier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_courier ms_courier = db.ms_courier.Find(id);
            if (ms_courier == null)
            {
                return HttpNotFound();
            }
            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code", ms_courier.country_id);
            return View(ms_courier);
        }

        // POST: courier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "courier_id,courier_code,courier_name,courier_address,country_id,courier_phone,courier_mail,courier_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_courier ms_courier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_courier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code", ms_courier.country_id);
            return View(ms_courier);
        }

        // GET: courier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_courier ms_courier = db.ms_courier.Find(id);
            if (ms_courier == null)
            {
                return HttpNotFound();
            }
            return View(ms_courier);
        }

        // POST: courier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_courier ms_courier = db.ms_courier.Find(id);
            db.ms_courier.Remove(ms_courier);
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
