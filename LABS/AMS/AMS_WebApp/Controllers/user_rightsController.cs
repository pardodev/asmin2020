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
    public class user_rightsController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: user_rights
        public ActionResult Index()
        {
            var ms_user_rights = db.ms_user_rights.Where(m=>m.fl_active == true && m.deleted_date == null)
                .Include(m => m.ms_job_level)
                .Include(m => m.ms_user_type);

            return View(ms_user_rights.ToList());
        }

        public ActionResult List()
        {
            //var _qry = (from t in db.ms_user_rights
            //           where (t.fl_active == true && t.deleted_date == null)                       
            //           group t by new { t.job_level_id, t.user_type_id } into g
            //           select new
            //           {
            //               t.key
            //               t.ms_user_type.user_type_code,
            //               t.ms_user_type.user_type_name,

            //               t.job_level_id,
            //               t.ms_job_level.job_level_code,
            //               t.ms_job_level.job_level_name
            //           })

            return View();
        }

        // GET: user_rights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_user_rights ms_user_rights = db.ms_user_rights.Find(id);
            if (ms_user_rights == null)
            {
                return HttpNotFound();
            }
            return View(ms_user_rights);
        }

        // GET: user_rights/Create
        public ActionResult Create()
        {
            ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code");
            ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(user_rightViewModel user_right)
        {
            if (ModelState.IsValid)
            {
                foreach (var mn in user_right.ms_menus)
                {
                    ms_user_rights rights = new ms_user_rights()
                    {
                        user_rights_id = user_right.user_type.user_type_id,
                        job_level_id = user_right.job_level.job_level_id,
                        menu_id = mn.menu_id,
                        fl_active = true,
                        created_date = DateTime.Now,
                        created_by = 0,
                        updated_date = DateTime.Now,
                        updated_by = 1,
                        deleted_date = null,
                        deleted_by = null,
                        org_id = 0
                    };

                    db.ms_user_rights.Add(rights);
                    db.SaveChanges();
                }
            }

            //ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code", user_right.job_level.job_level_id);
            //ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code", user_right.user_type.user_type_id);

            return RedirectToAction("Index");
        }

        // POST: user_rights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "menu_id,user_type_id,job_level_id")] ms_user_rights ms_user_rights)
        {
            if (ModelState.IsValid)
            {
                ms_user_rights.fl_active = true;
                ms_user_rights.created_date = DateTime.Now;
                ms_user_rights.created_by = 0;
                ms_user_rights.updated_date = DateTime.Now;
                ms_user_rights.updated_by = 1;
                ms_user_rights.deleted_date = null;
                ms_user_rights.deleted_by = null;
                ms_user_rights.org_id = 0;

                db.ms_user_rights.Add(ms_user_rights);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code", ms_user_rights.job_level_id);
            ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code", ms_user_rights.user_type_id);
            return View(ms_user_rights);
        }

        // GET: user_rights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_user_rights ms_user_rights = db.ms_user_rights.Find(id);
            if (ms_user_rights == null)
            {
                return HttpNotFound();
            }
            ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code", ms_user_rights.job_level_id);
            ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code", ms_user_rights.user_type_id);
            return View(ms_user_rights);
        }

        // POST: user_rights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "menu_id,user_type_id,job_level_id")] ms_user_rights ms_user_rights)
        {
            if (ModelState.IsValid)
            {
                ms_user_rights.fl_active = true;
                ms_user_rights.updated_date = DateTime.Now;
                ms_user_rights.updated_by = 1;
                ms_user_rights.deleted_date = null;
                ms_user_rights.deleted_by = null;
                ms_user_rights.org_id = 0;

                db.Entry(ms_user_rights).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.job_level_id = new SelectList(db.ms_job_level, "job_level_id", "job_level_code", ms_user_rights.job_level_id);
            ViewBag.user_type_id = new SelectList(db.ms_user_type, "user_type_id", "user_type_code", ms_user_rights.user_type_id);
            return View(ms_user_rights);
        }

        // GET: user_rights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ms_user_rights ms_user_rights = db.ms_user_rights.Find(id);
            if (ms_user_rights == null)
            {
                return HttpNotFound();
            }
            return View(ms_user_rights);
        }

        // POST: user_rights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ms_user_rights ms_user_rights = db.ms_user_rights.Find(id);
            db.ms_user_rights.Remove(ms_user_rights);
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
