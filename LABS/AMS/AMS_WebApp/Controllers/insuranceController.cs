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
    public class insuranceController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: insurance
        public ActionResult Index()
        {
            var ms_insurance = db.ms_insurance.Include(m => m.ms_country);
            return View(ms_insurance.ToList());
        }

        // GET: insurance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_insurance ms_insurance = db.ms_insurance.Find(id);
            if (ms_insurance == null)
            {
                return HttpNotFound();
            }
            return View(ms_insurance);
        }

        // GET: insurance/Create
        public ActionResult Create()
        {
            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code");
            return View();
        }

        // POST: insurance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "insurance_id,insurance_code,insurance_name,insurance_address,country_id,insurance_phone,insurance_mail,insurance_cp_name,insurance_cp_phone,insurance_cp_mail,insurance_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_insurance ms_insurance)
        {
            if (ModelState.IsValid)
            {
                db.ms_insurance.Add(ms_insurance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code", ms_insurance.country_id);
            return View(ms_insurance);
        }

        // GET: insurance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_insurance ms_insurance = db.ms_insurance.Find(id);
            if (ms_insurance == null)
            {
                return HttpNotFound();
            }
            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code", ms_insurance.country_id);
            return View(ms_insurance);
        }

        // POST: insurance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "insurance_id,insurance_code,insurance_name,insurance_address,country_id,insurance_phone,insurance_mail,insurance_cp_name,insurance_cp_phone,insurance_cp_mail,insurance_description,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_insurance ms_insurance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_insurance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.country_id = new SelectList(db.ms_country, "country_id", "country_code", ms_insurance.country_id);
            return View(ms_insurance);
        }

        // GET: insurance/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_insurance ms_insurance = db.ms_insurance.Find(id);
            if (ms_insurance == null)
            {
                return HttpNotFound();
            }
            return View(ms_insurance);
        }

        // POST: insurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_insurance ms_insurance = db.ms_insurance.Find(id);
            db.ms_insurance.Remove(ms_insurance);
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
