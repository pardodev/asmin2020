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
    public class moduleController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: module
        public ActionResult Index()
        {
            return View(db.ms_module.ToList());
        }

        // GET: module/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_module ms_module = db.ms_module.Find(id);
            if (ms_module == null)
            {
                return HttpNotFound();
            }
            return View(ms_module);
        }

        // GET: module/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: module/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "module_id,module_code,module_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id,rec_order")] ms_module ms_module)
        {
            if (ModelState.IsValid)
            {
                db.ms_module.Add(ms_module);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_module);
        }

        // GET: module/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_module ms_module = db.ms_module.Find(id);
            if (ms_module == null)
            {
                return HttpNotFound();
            }
            return View(ms_module);
        }

        // POST: module/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "module_id,module_code,module_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id,rec_order")] ms_module ms_module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_module);
        }

        // GET: module/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_module ms_module = db.ms_module.Find(id);
            if (ms_module == null)
            {
                return HttpNotFound();
            }
            return View(ms_module);
        }

        // POST: module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_module ms_module = db.ms_module.Find(id);
            db.ms_module.Remove(ms_module);
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
