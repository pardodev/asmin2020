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
    public class job_levelController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: job_level
        public ActionResult Index()
        {
            return View(db.ms_job_level.ToList());
        }

        // GET: job_level/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_job_level ms_job_level = db.ms_job_level.Find(id);
            if (ms_job_level == null)
            {
                return HttpNotFound();
            }
            return View(ms_job_level);
        }

        // GET: job_level/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: job_level/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "job_level_id,job_level_code,job_level_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_job_level ms_job_level)
        {
            if (ModelState.IsValid)
            {
                db.ms_job_level.Add(ms_job_level);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_job_level);
        }

        // GET: job_level/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_job_level ms_job_level = db.ms_job_level.Find(id);
            if (ms_job_level == null)
            {
                return HttpNotFound();
            }
            return View(ms_job_level);
        }

        // POST: job_level/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "job_level_id,job_level_code,job_level_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_job_level ms_job_level)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_job_level).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_job_level);
        }

        // GET: job_level/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_job_level ms_job_level = db.ms_job_level.Find(id);
            if (ms_job_level == null)
            {
                return HttpNotFound();
            }
            return View(ms_job_level);
        }

        // POST: job_level/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_job_level ms_job_level = db.ms_job_level.Find(id);
            db.ms_job_level.Remove(ms_job_level);
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
