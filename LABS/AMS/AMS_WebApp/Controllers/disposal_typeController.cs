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
    public class disposal_typeController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: disposal_type
        public ActionResult Index()
        {
            return View(db.ms_disposal_type.ToList());
        }

        // GET: disposal_type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_disposal_type ms_disposal_type = db.ms_disposal_type.Find(id);
            if (ms_disposal_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_disposal_type);
        }

        // GET: disposal_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: disposal_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "disposal_type_id,disposal_type_code,disposal_type_name,disposal_by_dept_id")] ms_disposal_type ms_disposal_type)
        {
            if (ModelState.IsValid)
            {
                db.ms_disposal_type.Add(ms_disposal_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_disposal_type);
        }

        // GET: disposal_type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_disposal_type ms_disposal_type = db.ms_disposal_type.Find(id);
            if (ms_disposal_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_disposal_type);
        }

        // POST: disposal_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "disposal_type_id,disposal_type_code,disposal_type_name,disposal_by_dept_id")] ms_disposal_type ms_disposal_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_disposal_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_disposal_type);
        }

        // GET: disposal_type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_disposal_type ms_disposal_type = db.ms_disposal_type.Find(id);
            if (ms_disposal_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_disposal_type);
        }

        // POST: disposal_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_disposal_type ms_disposal_type = db.ms_disposal_type.Find(id);
            db.ms_disposal_type.Remove(ms_disposal_type);
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
