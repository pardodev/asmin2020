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
    [Authorize]
    public class appsettingController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: appsetting
        public ActionResult Index()
        {
            //return View(db.sy_app_setting.ToList());
            return View();
        }

        public JsonResult List()
        {
            var _list = (from t in db.sy_app_setting
                         select t).ToList<sy_app_setting>();

            return Json(new { data = _list }, JsonRequestBehavior.AllowGet);
        }


        // GET: appsetting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sy_app_setting sy_app_setting = db.sy_app_setting.Find(id);
            if (sy_app_setting == null)
            {
                return HttpNotFound();
            }
            return View(sy_app_setting);
        }

        // GET: appsetting/Create
        public ActionResult Create()
        {
            sy_app_setting sy_app_setting = new sy_app_setting()
            {
                app_id = 0,
                fl_active = true,
                created_by = UserProfile.UserId,
                created_date = DateTime.Now
            };
            return View(sy_app_setting);
        }

        // POST: appsetting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "app_id,app_key,app_value,app_desc")] sy_app_setting sy_app_setting)
        {
            if (ModelState.IsValid)
            {
                sy_app_setting.fl_active = true;
                sy_app_setting.created_date = DateTime.Now;
                sy_app_setting.created_by = UserProfile.UserId;

                db.sy_app_setting.Add(sy_app_setting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sy_app_setting);
        }

        // GET: appsetting/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sy_app_setting sy_app_setting = db.sy_app_setting.Find(id);
            if (sy_app_setting == null)
            {
                return HttpNotFound();
            }
            return View(sy_app_setting);
        }

        // POST: appsetting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "app_id,app_key,app_value,app_desc")] sy_app_setting sy_app_setting)
        {
            if (ModelState.IsValid)
            {

                sy_app_setting.fl_active = true;
                sy_app_setting.created_date = DateTime.Now;
                sy_app_setting.created_by = UserProfile.UserId;

                db.Entry(sy_app_setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sy_app_setting);
        }

        // GET: appsetting/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sy_app_setting sy_app_setting = db.sy_app_setting.Find(id);
            if (sy_app_setting == null)
            {
                return HttpNotFound();
            }
            return View(sy_app_setting);
        }

        // POST: appsetting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sy_app_setting sy_app_setting = db.sy_app_setting.Find(id);
            db.sy_app_setting.Remove(sy_app_setting);
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
