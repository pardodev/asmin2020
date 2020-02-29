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
    public class mutation_processController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: mutation_process
        public ActionResult Index()
        {
            return View(db.tr_mutation_process.ToList());
        }

        // GET: mutation_process/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_mutation_process tr_mutation_process = db.tr_mutation_process.Find(id);
            if (tr_mutation_process == null)
            {
                return HttpNotFound();
            }
            return View(tr_mutation_process);
        }

        // GET: mutation_process/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mutation_process/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mutation_id,request_id,fl_pic_asset_comfirm,pic_asset_confirm_date,pic_asset_employee_id,pic_asset_level_id,courier_id,courier_description,pic_asset_received_date,pic_asset_received_employee_id,pic_asset_received_level_id,user_asset_received_date,user_asset_received_employee_id,user_asset_received_level_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_mutation_process tr_mutation_process)
        {
            if (ModelState.IsValid)
            {
                db.tr_mutation_process.Add(tr_mutation_process);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_mutation_process);
        }

        // GET: mutation_process/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_mutation_process tr_mutation_process = db.tr_mutation_process.Find(id);
            if (tr_mutation_process == null)
            {
                return HttpNotFound();
            }
            return View(tr_mutation_process);
        }

        // POST: mutation_process/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mutation_id,request_id,fl_pic_asset_comfirm,pic_asset_confirm_date,pic_asset_employee_id,pic_asset_level_id,courier_id,courier_description,pic_asset_received_date,pic_asset_received_employee_id,pic_asset_received_level_id,user_asset_received_date,user_asset_received_employee_id,user_asset_received_level_id,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_mutation_process tr_mutation_process)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_mutation_process).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_mutation_process);
        }

        // GET: mutation_process/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_mutation_process tr_mutation_process = db.tr_mutation_process.Find(id);
            if (tr_mutation_process == null)
            {
                return HttpNotFound();
            }
            return View(tr_mutation_process);
        }

        // POST: mutation_process/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_mutation_process tr_mutation_process = db.tr_mutation_process.Find(id);
            db.tr_mutation_process.Remove(tr_mutation_process);
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
