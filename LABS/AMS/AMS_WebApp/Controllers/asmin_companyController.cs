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
    public class asmin_companyController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asmin_company
        public ActionResult Index()
        {
            return View(db.ms_asmin_company.ToList());
        }

        // GET: asmin_company/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asmin_company ms_asmin_company = db.ms_asmin_company.Find(id);
            if (ms_asmin_company == null)
            {
                return HttpNotFound();
            }
            return View(ms_asmin_company);
        }

        // GET: asmin_company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asmin_company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "company_id,company_code,company_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asmin_company ms_asmin_company)
        {
            if (ModelState.IsValid)
            {
                db.ms_asmin_company.Add(ms_asmin_company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_asmin_company);
        }

        // GET: asmin_company/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asmin_company ms_asmin_company = db.ms_asmin_company.Find(id);
            if (ms_asmin_company == null)
            {
                return HttpNotFound();
            }
            return View(ms_asmin_company);
        }

        // POST: asmin_company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "company_id,company_code,company_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asmin_company ms_asmin_company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_asmin_company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_asmin_company);
        }

        // GET: asmin_company/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asmin_company ms_asmin_company = db.ms_asmin_company.Find(id);
            if (ms_asmin_company == null)
            {
                return HttpNotFound();
            }
            return View(ms_asmin_company);
        }

        // POST: asmin_company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_asmin_company ms_asmin_company = db.ms_asmin_company.Find(id);
            db.ms_asmin_company.Remove(ms_asmin_company);
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
