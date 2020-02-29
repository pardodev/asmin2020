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
    public class asset_register_picController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_register_pic
        public ActionResult Index()
        {
            return View(db.ms_asset_register_pic.ToList());
        }

        // GET: asset_register_pic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_register_pic ms_asset_register_pic = db.ms_asset_register_pic.Find(id);
            if (ms_asset_register_pic == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_register_pic);
        }

        // GET: asset_register_pic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: asset_register_pic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "asset_reg_pic_id,asset_reg_pic_code,asset_reg_pic_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asset_register_pic ms_asset_register_pic)
        {
            if (ModelState.IsValid)
            {
                db.ms_asset_register_pic.Add(ms_asset_register_pic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ms_asset_register_pic);
        }

        // GET: asset_register_pic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_register_pic ms_asset_register_pic = db.ms_asset_register_pic.Find(id);
            if (ms_asset_register_pic == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_register_pic);
        }

        // POST: asset_register_pic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "asset_reg_pic_id,asset_reg_pic_code,asset_reg_pic_name,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] ms_asset_register_pic ms_asset_register_pic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ms_asset_register_pic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ms_asset_register_pic);
        }

        // GET: asset_register_pic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_asset_register_pic ms_asset_register_pic = db.ms_asset_register_pic.Find(id);
            if (ms_asset_register_pic == null)
            {
                return HttpNotFound();
            }
            return View(ms_asset_register_pic);
        }

        // POST: asset_register_pic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_asset_register_pic ms_asset_register_pic = db.ms_asset_register_pic.Find(id);
            db.ms_asset_register_pic.Remove(ms_asset_register_pic);
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
