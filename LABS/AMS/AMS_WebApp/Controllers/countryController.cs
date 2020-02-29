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
    public class countryController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: country
        public ActionResult Index()
        {
            return View(db.ms_country.ToList());
        }

        // GET: country/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_country ms_country = db.ms_country.Find(id);
            if (ms_country == null)
            {
                return HttpNotFound();
            }
            return View(ms_country);
        }

        // GET: country/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "country_id,country_code,country_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_country ms_country)
        {
            if (ModelState.IsValid)
            {
                db.ms_country.Add(ms_country);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_country);
        }

        // GET: country/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_country ms_country = db.ms_country.Find(id);
            if (ms_country == null)
            {
                return HttpNotFound();
            }
            return View(ms_country);
        }

        // POST: country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "country_id,country_code,country_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_country ms_country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_country).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_country);
        }

        // GET: country/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_country ms_country = db.ms_country.Find(id);
            if (ms_country == null)
            {
                return HttpNotFound();
            }
            return View(ms_country);
        }

        // POST: country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_country ms_country = db.ms_country.Find(id);
            db.ms_country.Remove(ms_country);
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
