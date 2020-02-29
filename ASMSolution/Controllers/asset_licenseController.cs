using System;
using System.IO;
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
    public class asset_licenseController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_license
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// list utk license index
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult GetLicenseList()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var _qry = (from aw in db.tr_asset_license
                        where (aw.fl_active == true && aw.deleted_date == null)
                        group aw by aw.asset_id into awg

                        join a in db.tr_asset_registration on awg.FirstOrDefault().asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null
                        && a.company_id == UserProfile.company_id                        
                        && a.current_location_id == UserProfile.location_id)

                        join b in db.ms_vendor on a.vendor_id equals b.vendor_id
                        where (b.fl_active == true && b.deleted_date == null)

                        select new asset_licenseViewModel
                        {
                            asset_parent_id = a.asset_id,
                            asset_number = a.asset_number,
                            asset_name = a.asset_name,
                            ms_vendor = b
                        }).ToList<asset_licenseViewModel>();

            return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
        }

        // GET: asset_license/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_asset_registration ass_reg = db.tr_asset_registration.Find(id);
            if (ass_reg == null)
            {
                return HttpNotFound("Assset not found.");
            }

            asset_licenseViewModel asset_model = new asset_licenseViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,

                asset_parent_id = (int)ass_reg.asset_id,
                asset_parent = ass_reg
            };

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_model.vendor_name = msvendor.vendor_name;

            //Data view
            asset_model.asset_license_list = (from aw in db.tr_asset_license
                                              where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                                              select aw).ToList<tr_asset_license>();

            return View(asset_model);
        }

        /// <summary>
        /// list utk modal assetlist
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JSON</returns>
        public JsonResult GetAssetList(int? id = 0)
        {
            db.Configuration.ProxyCreationEnabled = false;
            int asset_id = (int)(id);
            if (asset_id == 0)
            {
                var _list = from t in db.tr_asset_registration
                            where t.fl_active == true && t.deleted_date == null 
                            //&& t.asset_type_id == 1
                            && t.company_id == UserProfile.company_id 
                            //&& t.department_id == UserProfile.department_id

                            join v in db.ms_vendor on t.vendor_id equals v.vendor_id
                            select new asset_warrantyViewModel
                            {
                                asset_parent_id = t.asset_id,
                                asset_name = t.asset_name,
                                asset_number = t.asset_number,
                                vendor_id = t.vendor_id,
                                vendor_name = v.vendor_name
                            };

                return Json(new { data = _list.ToList() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var _list = from t in db.tr_asset_registration
                            where t.fl_active == true && t.deleted_date == null 
                            //&& t.asset_type_id == 1 
                            && t.asset_id == asset_id
                            && t.company_id == UserProfile.company_id 
                            //&& t.department_id == UserProfile.department_id

                            join v in db.ms_vendor on t.vendor_id equals v.vendor_id
                            select new asset_warrantyViewModel
                            {
                                asset_parent_id = t.asset_id,
                                asset_name = t.asset_name,
                                asset_number = t.asset_number,
                                vendor_id = t.vendor_id,
                                vendor_name = v.vendor_name
                            };

                return Json(_list.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        // GET: asset_license/Create
        public ActionResult Create(int? id)
        {
            asset_licenseViewModel asset = new asset_licenseViewModel()
            {
                FormMode = EnumFormModeKey.Form_New,
                asset_parent_id = 0,
                asset_registration_list = db.tr_asset_registration.Where(r => r.fl_active == true && r.deleted_date == null).ToList()

            };

            //Data view
            asset.asset_license_list = (from aw in db.tr_asset_license
                                        where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                                        select aw).ToList<tr_asset_license>();

            return View(asset);
        }

        // POST: asset_license/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "asset_parent_id, license_number, license_name, license_issued_by, license_date, license_exp_date, license_description")] asset_licenseViewModel asset_lcs)
        {
            if (asset_lcs.license_date > DateTime.Today)
                ModelState.AddModelError("license_date", "Please enter a valid date.");

            if (asset_lcs.license_date > asset_lcs.license_exp_date)
                ModelState.AddModelError("license_exp_date", "license expired date must be after license date.");

            tr_asset_license existLicense = (from exw in db.tr_asset_license.Where(exw => exw.license_number == asset_lcs.license_number && exw.asset_id == asset_lcs.asset_parent_id) select exw).FirstOrDefault<tr_asset_license>();

            if (existLicense == null)
            {
                if (ModelState.IsValid)
                {
                    tr_asset_license ass_license = new tr_asset_license();
                    ass_license.asset_id = asset_lcs.asset_parent_id;
                    ass_license.license_number = asset_lcs.license_number;
                    ass_license.license_name = asset_lcs.license_name;
                    ass_license.license_issued_by = asset_lcs.license_issued_by;
                    ass_license.license_date = asset_lcs.license_date;
                    ass_license.license_exp_date = asset_lcs.license_exp_date;
                    ass_license.license_description = asset_lcs.license_description;

                    ass_license.fl_active = true;
                    ass_license.created_date = DateTime.Now;
                    ass_license.created_by = UserProfile.UserId;
                    ass_license.updated_date = DateTime.Now;
                    ass_license.updated_by = UserProfile.UserId;
                    ass_license.deleted_date = null;
                    ass_license.deleted_by = null;
                    ass_license.org_id = UserProfile.OrgId;

                    ass_license = db.tr_asset_license.Add(ass_license);
                    db.SaveChanges();

                    //clear
                    ModelState.Clear();
                    asset_lcs.license_id = 0;
                    asset_lcs.license_name = "";
                    asset_lcs.license_issued_by = "";
                    asset_lcs.license_number = "";
                    asset_lcs.license_date = null;
                    asset_lcs.license_exp_date = null;
                    asset_lcs.license_description = "";
                }
            }
            else
            {
                //Data view
                List<tr_asset_license> assetlcslist = (from aw in db.tr_asset_license
                                                                                      where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == asset_lcs.asset_parent_id)
                                                                                      select aw).ToList<tr_asset_license>();
                ModelState.Clear();
                asset_lcs = new asset_licenseViewModel();
                asset_lcs.asset_license_list = assetlcslist;
                return View(asset_lcs);
            }
            //Data view
            asset_lcs.asset_license_list = (from aw in db.tr_asset_license
                                            where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == asset_lcs.asset_parent_id)
                                            select aw).ToList<tr_asset_license>();

            return View(asset_lcs);
        }

        // GET: asset_license/Edit/5
        public ActionResult Edit(int? id, int? idw)
        {
            if (id == null || idw == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_asset_registration ass_reg = db.tr_asset_registration.Find(id);
            if (ass_reg == null)
            {
                return HttpNotFound("Assset not found.");
            }

            asset_licenseViewModel asset_model = new asset_licenseViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,

                asset_parent_id = (int)ass_reg.asset_id,
                asset_parent = ass_reg
            };

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_model.vendor_name = msvendor.vendor_name;

            tr_asset_license ass_license = db.tr_asset_license.Find(idw);
            if (ass_license != null)
            {
                asset_model.license_id = ass_license.license_id;
                asset_model.license_number = ass_license.license_number;
                asset_model.license_name = ass_license.license_name;
                asset_model.license_issued_by = ass_license.license_issued_by;
                asset_model.license_date = ass_license.license_date;
                asset_model.license_exp_date = ass_license.license_exp_date;
                asset_model.license_description = ass_license.license_description;
            }

            //Data view edit
            asset_model.asset_license_list = (from aw in db.tr_asset_license
                                              where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                                              select aw).ToList<tr_asset_license>();

            return View(asset_model);
        }

        // POST: asset_license/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "license_id, asset_parent_id, license_number, license_name, license_issued_by, license_date, license_exp_date, license_description")] asset_licenseViewModel asset_lcs)
        {
            if (asset_lcs.license_date > DateTime.Today)
                ModelState.AddModelError("license_date", "Please enter a valid date.");

            if (asset_lcs.license_date > asset_lcs.license_exp_date)
                ModelState.AddModelError("license_exp_date", "license expired date must be after license date.");

            if (ModelState.IsValid && asset_lcs.license_id > 0)
            {
                tr_asset_license ass_license = db.tr_asset_license.Find(asset_lcs.license_id);
                ass_license.license_number = asset_lcs.license_number;
                ass_license.license_name = asset_lcs.license_name;
                ass_license.license_issued_by = asset_lcs.license_issued_by;
                ass_license.license_date = asset_lcs.license_date;
                ass_license.license_exp_date = asset_lcs.license_exp_date;
                ass_license.license_description = asset_lcs.license_description;

                ass_license.fl_active = true;
                ass_license.updated_date = DateTime.Now;
                ass_license.updated_by = UserProfile.UserId;
                ass_license.deleted_date = null;
                ass_license.deleted_by = null;
                ass_license.org_id = UserProfile.OrgId;

                db.Entry(ass_license).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            tr_asset_registration ass_reg = db.tr_asset_registration.Find(asset_lcs.asset_parent_id);
            asset_lcs.FormMode = EnumFormModeKey.Form_Edit;

            asset_lcs.asset_parent_id = (int)ass_reg.asset_id;
            asset_lcs.asset_parent = ass_reg;

            //Data view edit
            asset_lcs.asset_license_list = (from aw in db.tr_asset_license
                                            where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == asset_lcs.asset_parent_id)
                                            select aw).ToList<tr_asset_license>();

            return View(asset_lcs);
        }

        // GET: asset_license/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tr_asset_registration ass_reg = db.tr_asset_registration.Find(id);
            if (ass_reg == null)
            {
                return HttpNotFound("Assset not found.");
            }

            asset_licenseViewModel asset_model = new asset_licenseViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,

                asset_parent_id = (int)ass_reg.asset_id,
                asset_parent = ass_reg
            };

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_model.vendor_name = msvendor.vendor_name;

            //Data view
            asset_model.asset_license_list = (from aw in db.tr_asset_license
                                              where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                                              select aw).ToList<tr_asset_license>();

            return View(asset_model);
        }

        // POST: asset_warranty/Delete/5
        [HttpGet, ActionName("DeleteItem")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteItem(int id)
        {
            tr_asset_license tr_asset_license = db.tr_asset_license.Find(id);
            if (tr_asset_license != null)
            {
                tr_asset_license.fl_active = false;
                tr_asset_license.deleted_by = UserProfile.UserId;
                tr_asset_license.deleted_date = DateTime.Now;

                db.Entry(tr_asset_license).State = EntityState.Modified;
                db.SaveChanges();

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
       // POST: asset_warranty/Delete/5
        [HttpGet, ActionName("DeleteItemAll")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteItemAll(int id)
        {
            var _qry = (from aw in db.tr_asset_license
                        where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                        select new asset_licenseViewModel()
                        {
                            license_id = aw.license_id
                        }).ToList<asset_licenseViewModel>();

            if (_qry != null)
            {
                foreach (asset_licenseViewModel ass_wrt in _qry)
                {
                    tr_asset_license tr_asset_license = db.tr_asset_license.Find(ass_wrt.license_id);
                    if (tr_asset_license != null)
                    {
                        tr_asset_license.fl_active = false;
                        tr_asset_license.deleted_by = UserProfile.UserId;
                        tr_asset_license.deleted_date = DateTime.Now;

                        db.Entry(tr_asset_license).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult ModalAsset()
        {
            return PartialView();
        }
    }
}