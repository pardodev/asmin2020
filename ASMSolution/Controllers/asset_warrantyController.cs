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
    public class asset_warrantyController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_warranty
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// list utk warranty index
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult GetWarrantyList()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var _qry = (from aw in db.tr_asset_warranty
                        where (aw.fl_active == true && aw.deleted_date == null)
                        group aw by aw.asset_id into awg

                        join a in db.tr_asset_registration on awg.FirstOrDefault().asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null
                        && a.company_id == UserProfile.company_id
                        //&& a.current_location_id == UserProfile.location_id
                        )

                        join b in db.ms_vendor on a.vendor_id equals b.vendor_id
                        where (b.fl_active == true && b.deleted_date == null)

                        select new asset_warrantyViewModel
                        {
                            asset_parent_id = a.asset_id,
                            asset_number = a.asset_number,
                            asset_name = a.asset_name,
                            ms_vendor = b

                        }).ToList<asset_warrantyViewModel>();

            return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
        }

        // GET: asset_warranty/Details/5
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

            asset_warrantyViewModel asset_model = new asset_warrantyViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,

                asset_parent_id = (int)ass_reg.asset_id,
                asset_parent = ass_reg

            };

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_model.vendor_name = msvendor.vendor_name;

            //Data view
            asset_model.asset_warranty_list = (from aw in db.tr_asset_warranty
                                               where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                                               select aw).ToList<tr_asset_warranty>();

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

        // GET: asset_warranty/Create
        public ActionResult Create(int? id)
        {
            asset_warrantyViewModel asset = new asset_warrantyViewModel()
            {
                FormMode = EnumFormModeKey.Form_New,
                asset_parent_id = 0,
                asset_registration_list = db.tr_asset_registration.Where(r => r.fl_active == true && r.deleted_date == null).ToList()
            };

            //Data view
            asset.asset_warranty_list = (from aw in db.tr_asset_warranty
                                         where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                                         select aw).ToList<tr_asset_warranty>();

            return View(asset);
        }

        // POST: asset_warranty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "asset_parent_id, warranty_number, warranty_item_name, warranty_date, warranty_exp_date, warranty_description")] asset_warrantyViewModel asset_wrt)
        {
            try
            {
                if (asset_wrt.warranty_date > DateTime.Today)
                    ModelState.AddModelError("warranty_date", "Please enter a valid date.");

                if (asset_wrt.warranty_date > asset_wrt.warranty_exp_date)
                    ModelState.AddModelError("warranty_exp_date", "Warranty expired date must be after warranty date.");

                tr_asset_warranty existWarranty = (from exw in db.tr_asset_warranty.Where(exw => exw.warranty_number == asset_wrt.warranty_number && exw.asset_id == asset_wrt.asset_parent_id) select exw).FirstOrDefault<tr_asset_warranty>();

                if (existWarranty == null)
                {
                    if (ModelState.IsValid)
                    {
                        tr_asset_warranty ass_warranty = new tr_asset_warranty();
                        ass_warranty.asset_id = asset_wrt.asset_parent_id;
                        ass_warranty.warranty_number = asset_wrt.warranty_number;
                        ass_warranty.warranty_item_name = asset_wrt.warranty_item_name;
                        ass_warranty.warranty_date = asset_wrt.warranty_date;
                        ass_warranty.warranty_exp_date = asset_wrt.warranty_exp_date;
                        ass_warranty.warranty_description = asset_wrt.warranty_description;

                        ass_warranty.fl_active = true;
                        ass_warranty.created_date = DateTime.Now;
                        ass_warranty.created_by = UserProfile.UserId;
                        ass_warranty.updated_date = DateTime.Now;
                        ass_warranty.updated_by = UserProfile.UserId;
                        ass_warranty.deleted_date = null;
                        ass_warranty.deleted_by = null;
                        ass_warranty.org_id = UserProfile.OrgId;

                        ass_warranty = db.tr_asset_warranty.Add(ass_warranty);
                        db.SaveChanges();

                        ModelState.Clear();
                        asset_wrt.warranty_number = string.Empty;
                        asset_wrt.warranty_item_name = string.Empty;
                        asset_wrt.warranty_date = null;
                        asset_wrt.warranty_exp_date = null;
                    }
                }
                else
                {
                    //Data view
                    List<tr_asset_warranty> assetwrtlist = asset_wrt.asset_warranty_list = (from aw in db.tr_asset_warranty
                                                                                            where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == asset_wrt.asset_parent_id)
                                                                                            select aw).ToList<tr_asset_warranty>();
                    ModelState.Clear();
                    asset_wrt = new asset_warrantyViewModel();
                    asset_wrt.asset_warranty_list = assetwrtlist;
                    return View(asset_wrt);
                }
                //Data view
                asset_wrt.asset_warranty_list = (from aw in db.tr_asset_warranty
                                                 where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == asset_wrt.asset_parent_id)
                                                 select aw).ToList<tr_asset_warranty>();
            }
            catch (Exception ex)
            {
                string msgErr = string.Format("An unknown error has occurred , Please contact your system administrator. {0}", ex.Message);
                if (ex.InnerException != null)
                    msgErr += string.Format(" Inner Exception: {0}", ex.InnerException.Message);
                ModelState.AddModelError("", msgErr);
            }
            return View(asset_wrt);
        }

        // GET: asset_warranty/Edit/5
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

            asset_warrantyViewModel asset_model = new asset_warrantyViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,

                asset_parent_id = (int)ass_reg.asset_id,
                asset_parent = ass_reg
            };

            tr_asset_warranty ass_warranty = db.tr_asset_warranty.Find(idw);
            if (ass_warranty != null)
            {
                asset_model.warranty_id = ass_warranty.warranty_id;
                asset_model.warranty_number = ass_warranty.warranty_number;
                asset_model.warranty_item_name = ass_warranty.warranty_item_name;
                asset_model.warranty_date = ass_warranty.warranty_date;
                asset_model.warranty_exp_date = ass_warranty.warranty_exp_date;
                asset_model.warranty_description = ass_warranty.warranty_description;
            }

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_model.vendor_name = msvendor.vendor_name;

            //Data view edit
            asset_model.asset_warranty_list = (from aw in db.tr_asset_warranty
                                               where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                                               select aw).ToList<tr_asset_warranty>();

            return View(asset_model);
        }

        // POST: asset_warranty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "warranty_id, asset_parent_id, warranty_number, warranty_item_name, warranty_date, warranty_exp_date, warranty_description")] asset_warrantyViewModel asset_wrt)
        {
            if (asset_wrt.warranty_date > DateTime.Today)
                ModelState.AddModelError("warranty_date", "Please enter a valid date.");

            if (asset_wrt.warranty_date > asset_wrt.warranty_exp_date)
                ModelState.AddModelError("warranty_exp_date", "Warranty expired date must be after warranty date.");

            if (ModelState.IsValid && asset_wrt.warranty_id > 0)
            {
                tr_asset_warranty ass_warranty = db.tr_asset_warranty.Find(asset_wrt.warranty_id);
                ass_warranty.warranty_number = asset_wrt.warranty_number;
                ass_warranty.warranty_item_name = asset_wrt.warranty_item_name;
                ass_warranty.warranty_date = asset_wrt.warranty_date;
                ass_warranty.warranty_exp_date = asset_wrt.warranty_exp_date;
                ass_warranty.warranty_description = asset_wrt.warranty_description;

                ass_warranty.fl_active = true;
                ass_warranty.updated_date = DateTime.Now;
                ass_warranty.updated_by = UserProfile.UserId;
                ass_warranty.deleted_date = null;
                ass_warranty.deleted_by = null;
                ass_warranty.org_id = UserProfile.OrgId;

                db.Entry(ass_warranty).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            tr_asset_registration ass_reg = db.tr_asset_registration.Find(asset_wrt.asset_parent_id);
            asset_wrt.FormMode = EnumFormModeKey.Form_Edit;

            asset_wrt.asset_parent_id = (int)ass_reg.asset_id;
            asset_wrt.asset_parent = ass_reg;

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_wrt.vendor_name = msvendor.vendor_name;

            //Data view edit
            asset_wrt.asset_warranty_list = (from aw in db.tr_asset_warranty
                                             where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == asset_wrt.asset_parent_id)
                                             select aw).ToList<tr_asset_warranty>();

            return View(asset_wrt);
        }

        // GET: asset_warranty/Delete/5
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

            asset_warrantyViewModel asset_model = new asset_warrantyViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,

                asset_parent_id = (int)ass_reg.asset_id,
                asset_parent = ass_reg
            };

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_model.vendor_name = msvendor.vendor_name;

            //Data view
            asset_model.asset_warranty_list = (from aw in db.tr_asset_warranty
                                               where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                                               select aw).ToList<tr_asset_warranty>();

            return View(asset_model);
        }

        // POST: asset_warranty/Delete/5
        [HttpGet, ActionName("DeleteItem")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteItem(int id)
        {
            tr_asset_warranty tr_asset_warranty = db.tr_asset_warranty.Find(id);
            if (tr_asset_warranty != null)
            {
                tr_asset_warranty.fl_active = false;
                tr_asset_warranty.deleted_by = UserProfile.UserId;
                tr_asset_warranty.deleted_date = DateTime.Now;
                db.Entry(tr_asset_warranty).State = EntityState.Modified;
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
            var _qry = (from aw in db.tr_asset_warranty
                        where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                        select new asset_warrantyViewModel()
                        {
                            warranty_id = aw.warranty_id
                        }).ToList<asset_warrantyViewModel>();

            if (_qry != null)
            {
                foreach (asset_warrantyViewModel ass_wrt in _qry)
                {
                    tr_asset_warranty tr_asset_warranty = db.tr_asset_warranty.Find(ass_wrt.warranty_id);
                    if (tr_asset_warranty != null)
                    {
                        tr_asset_warranty.fl_active = false;
                        tr_asset_warranty.deleted_by = UserProfile.UserId;
                        tr_asset_warranty.deleted_date = DateTime.Now;

                        db.Entry(tr_asset_warranty).State = EntityState.Modified;
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