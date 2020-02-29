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
    public class user_typeController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: user_type
        public ActionResult Index()
        {
            return View(db.ms_user_type.ToList());
        }

        // GET: user_type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_user_type ms_user_type = db.ms_user_type.Find(id);
            if (ms_user_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_user_type);
        }

        // GET: user_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: user_type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_type_id,user_type_code,user_type_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_user_type ms_user_type)
        {
            if (ModelState.IsValid)
            {
                db.ms_user_type.Add(ms_user_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_user_type);
        }

        // GET: user_type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_user_type ms_user_type = db.ms_user_type.Find(id);
            if (ms_user_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_user_type);
        }

        // POST: user_type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_type_id,user_type_code,user_type_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_user_type ms_user_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_user_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_user_type);
        }

        // GET: user_type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_user_type ms_user_type = db.ms_user_type.Find(id);
            if (ms_user_type == null)
            {
                return HttpNotFound();
            }
            return View(ms_user_type);
        }

        // POST: user_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_user_type ms_user_type = db.ms_user_type.Find(id);
            db.ms_user_type.Remove(ms_user_type);
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
