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
    public class menuController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: menu
        public ActionResult Index()
        {
            return View(db.ms_menu.ToList());
        }

        // GET: menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_menu ms_menu = db.ms_menu.Find(id);
            if (ms_menu == null)
            {
                return HttpNotFound();
            }
            return View(ms_menu);
        }

        // GET: menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "menu_id,menu_code,menu_name,module_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id,menu_url,rec_order")] ms_menu ms_menu)
        {
            if (ModelState.IsValid)
            {
                db.ms_menu.Add(ms_menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_menu);
        }

        // GET: menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_menu ms_menu = db.ms_menu.Find(id);
            if (ms_menu == null)
            {
                return HttpNotFound();
            }
            return View(ms_menu);
        }

        // POST: menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "menu_id,menu_code,menu_name,module_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id,menu_url,rec_order")] ms_menu ms_menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_menu);
        }

        // GET: menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_menu ms_menu = db.ms_menu.Find(id);
            if (ms_menu == null)
            {
                return HttpNotFound();
            }
            return View(ms_menu);
        }

        // POST: menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_menu ms_menu = db.ms_menu.Find(id);
            db.ms_menu.Remove(ms_menu);
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
