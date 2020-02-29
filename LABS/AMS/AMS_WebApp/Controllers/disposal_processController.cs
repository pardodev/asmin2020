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
    public class disposal_processController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: disposal_process
        public ActionResult Index()
        {
            return View(db.tr_disposal_process.ToList());
        }

        // GET: disposal_process/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_process tr_disposal_process = db.tr_disposal_process.Find(id);
            if (tr_disposal_process == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_process);
        }

        // GET: disposal_process/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: disposal_process/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "disposal_id,request_id,fl_pic_asset_comfirm,pic_asset_confirm_date,pic_asset_employee_id,pic_asset_level_id,pic_asset_received_date,pic_asset_received_employee_id,pic_asset_received_level_id,disposal_suggestion_id,disposal_sent_dept_id,disposal_sent_date,user_asset_received_employee_id,user_asset_received_level_id,user_asset_received_date,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_disposal_process tr_disposal_process)
        {
            if (ModelState.IsValid)
            {
                db.tr_disposal_process.Add(tr_disposal_process);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_disposal_process);
        }

        // GET: disposal_process/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_process tr_disposal_process = db.tr_disposal_process.Find(id);
            if (tr_disposal_process == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_process);
        }

        // POST: disposal_process/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "disposal_id,request_id,fl_pic_asset_comfirm,pic_asset_confirm_date,pic_asset_employee_id,pic_asset_level_id,pic_asset_received_date,pic_asset_received_employee_id,pic_asset_received_level_id,disposal_suggestion_id,disposal_sent_dept_id,disposal_sent_date,user_asset_received_employee_id,user_asset_received_level_id,user_asset_received_date,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_disposal_process tr_disposal_process)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_disposal_process).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_disposal_process);
        }

        // GET: disposal_process/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_disposal_process tr_disposal_process = db.tr_disposal_process.Find(id);
            if (tr_disposal_process == null)
            {
                return HttpNotFound();
            }
            return View(tr_disposal_process);
        }

        // POST: disposal_process/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_disposal_process tr_disposal_process = db.tr_disposal_process.Find(id);
            db.tr_disposal_process.Remove(tr_disposal_process);
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
