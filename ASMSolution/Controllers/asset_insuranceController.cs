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
    public class asset_insuranceController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: asset_insurance
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// list utk insurance index
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult GetInsuranceList()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var _qry = (from ai in db.tr_asset_insurance
                        where (ai.fl_active == true && ai.deleted_date == null)
                        group ai by ai.asset_id into aig

                        join a in db.tr_asset_registration on aig.FirstOrDefault().asset_id equals a.asset_id
                        where (a.fl_active == true && a.deleted_date == null
                        && a.company_id == UserProfile.company_id
                        && a.current_location_id == UserProfile.location_id

                        )

                        join b in db.ms_vendor on a.vendor_id equals b.vendor_id
                        where (b.fl_active == true && b.deleted_date == null)

                        select new asset_insuranceViewModel
                        {
                            asset_parent_id = a.asset_id,
                            asset_number = a.asset_number,
                            asset_name = a.asset_name,
                            ms_vendor = b
                        }).ToList<asset_insuranceViewModel>();

            return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
        }

        // GET: asset_insurance/Details/5
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

            asset_insuranceViewModel asset_model = new asset_insuranceViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,

                asset_parent_id = (int)ass_reg.asset_id,
                asset_parent = ass_reg
            };

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_model.vendor_name = msvendor.vendor_name;

            //Data view
            asset_model.detail_insurance_list = (from aw in db.tr_asset_insurance
                                                 where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)

                                                 join a in db.ms_insurance on aw.insurance_id equals a.insurance_id
                                                 where (a.fl_active == true && a.deleted_date == null)

                                                 select new asset_insurancedetailViewModel()
                                                 {
                                                     insurance_activa_id = aw.insurance_activa_id,
                                                     insurance_activa_number = aw.insurance_activa_number,
                                                     insurance_activa_name = aw.insurance_activa_name,
                                                     insurance_activa_date = aw.insurance_activa_date,
                                                     insurance_activa_exp_date = aw.insurance_activa_exp_date,
                                                     insurance_id = a.insurance_id,
                                                     insurance_name = a.insurance_name,
                                                     insurance_activa_description = aw.insurance_activa_description
                                                 }).ToList<asset_insurancedetailViewModel>();

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

        // GET: asset_insurance/Create
        public ActionResult Create(int? id)
        {
            asset_insuranceViewModel asset = new asset_insuranceViewModel()
            {
                FormMode = EnumFormModeKey.Form_New,
                asset_parent_id = 0,
                asset_registration_list = db.tr_asset_registration.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                insurance_list = db.ms_insurance.Where(r => r.fl_active == true && r.deleted_date == null).ToList()
            };

            //Data view
            asset.detail_insurance_list = (from aw in db.tr_asset_insurance
                                           where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)

                                           join a in db.ms_insurance on aw.insurance_id equals a.insurance_id
                                           where (a.fl_active == true && a.deleted_date == null)

                                           select new asset_insurancedetailViewModel()
                                           {
                                               insurance_activa_id = aw.insurance_activa_id,
                                               insurance_activa_number = aw.insurance_activa_number,
                                               insurance_activa_name = aw.insurance_activa_name,
                                               insurance_activa_date = aw.insurance_activa_date,
                                               insurance_activa_exp_date = aw.insurance_activa_exp_date,
                                               insurance_id = a.insurance_id,
                                               insurance_name = a.insurance_name,
                                               insurance_activa_description = aw.insurance_activa_description
                                           }).ToList<asset_insurancedetailViewModel>();

            return View(asset);
        }

        // POST: asset_insurance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "asset_parent_id, insurance_activa_number, insurance_activa_name, insurance_activa_date, insurance_activa_exp_date, insurance_id, insurance_activa_description")] asset_insuranceViewModel asset_ins)
        {
            if (asset_ins.insurance_activa_date > DateTime.Today)
                ModelState.AddModelError("insurance_activa_date", "Please enter a valid date.");

            if (asset_ins.insurance_activa_date > asset_ins.insurance_activa_exp_date)
                ModelState.AddModelError("insurance_activa_exp_date", "Insurance activa expired date must be after insurance activa date.");

            tr_asset_insurance existinsurance = (from exw in db.tr_asset_insurance.Where(exw => exw.insurance_activa_number == asset_ins.insurance_activa_number && exw.asset_id == asset_ins.asset_parent_id) select exw).FirstOrDefault<tr_asset_insurance>();

            if (existinsurance == null)
            {
                if (ModelState.IsValid)
                {
                    tr_asset_insurance ass_insurance = new tr_asset_insurance();
                    ass_insurance.asset_id = asset_ins.asset_parent_id;
                    ass_insurance.insurance_activa_number = asset_ins.insurance_activa_number;
                    ass_insurance.insurance_activa_name = asset_ins.insurance_activa_name;
                    ass_insurance.insurance_activa_date = asset_ins.insurance_activa_date;
                    ass_insurance.insurance_activa_exp_date = asset_ins.insurance_activa_exp_date;
                    ass_insurance.insurance_id = asset_ins.insurance_id;
                    ass_insurance.insurance_activa_description = asset_ins.insurance_activa_description;

                    ass_insurance.fl_active = true;
                    ass_insurance.created_date = DateTime.Now;
                    ass_insurance.created_by = UserProfile.UserId;
                    ass_insurance.updated_date = DateTime.Now;
                    ass_insurance.updated_by = UserProfile.UserId;
                    ass_insurance.deleted_date = null;
                    ass_insurance.deleted_int = null;
                    ass_insurance.org_id = UserProfile.OrgId;

                    ass_insurance = db.tr_asset_insurance.Add(ass_insurance);
                    db.SaveChanges();

                    //clear
                    ModelState.Clear();
                    asset_ins.insurance_activa_id = 0;
                    asset_ins.insurance_activa_name = string.Empty;
                    asset_ins.insurance_activa_number = string.Empty;
                    asset_ins.insurance_activa_date = null;
                    asset_ins.insurance_activa_exp_date = null;
                    asset_ins.insurance_id = 0;
                    asset_ins.insurance_activa_description = string.Empty;
                }
            }
            else
            {
                //Data view
                List<asset_insurancedetailViewModel> assetlcslist = (from aw in db.tr_asset_insurance
                                                           where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == asset_ins.asset_parent_id)

                                                           join a in db.ms_insurance on aw.insurance_id equals a.insurance_id
                                                           where (a.fl_active == true && a.deleted_date == null)

                                                           select new asset_insurancedetailViewModel()
                                                           {
                                                               insurance_activa_id = aw.insurance_activa_id,
                                                               insurance_activa_number = aw.insurance_activa_number,
                                                               insurance_activa_name = aw.insurance_activa_name,
                                                               insurance_activa_date = aw.insurance_activa_date,
                                                               insurance_activa_exp_date = aw.insurance_activa_exp_date,
                                                               insurance_id = a.insurance_id,
                                                               insurance_name = a.insurance_name,
                                                               insurance_activa_description = aw.insurance_activa_description
                                                           }).ToList<asset_insurancedetailViewModel>();
                ModelState.Clear();
                asset_ins = new asset_insuranceViewModel();
                asset_ins.detail_insurance_list = assetlcslist;
                asset_ins.insurance_list = db.ms_insurance.Where(r => r.fl_active == true && r.deleted_date == null).ToList();
                return View(asset_ins);
            }
            //Data view
            asset_ins.insurance_list = db.ms_insurance.Where(r => r.fl_active == true && r.deleted_date == null).ToList();
            asset_ins.detail_insurance_list = (from aw in db.tr_asset_insurance
                                               where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == asset_ins.asset_parent_id)

                                               join a in db.ms_insurance on aw.insurance_id equals a.insurance_id
                                               where (a.fl_active == true && a.deleted_date == null)

                                               select new asset_insurancedetailViewModel()
                                               {
                                                   insurance_activa_id = aw.insurance_activa_id,
                                                   insurance_activa_number = aw.insurance_activa_number,
                                                   insurance_activa_name = aw.insurance_activa_name,
                                                   insurance_activa_date = aw.insurance_activa_date,
                                                   insurance_activa_exp_date = aw.insurance_activa_exp_date,
                                                   insurance_id = a.insurance_id,
                                                   insurance_name = a.insurance_name,
                                                   insurance_activa_description = aw.insurance_activa_description
                                               }).ToList<asset_insurancedetailViewModel>();

            return View(asset_ins);
        }

        // GET: asset_insurance/Edit/5
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

            asset_insuranceViewModel asset_model = new asset_insuranceViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,

                asset_parent_id = (int)ass_reg.asset_id,
                asset_parent = ass_reg,
                insurance_list = db.ms_insurance.Where(r => r.fl_active == true && r.deleted_date == null).ToList()
            };

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_model.vendor_name = msvendor.vendor_name;

            tr_asset_insurance ass_insurance = db.tr_asset_insurance.Find(idw);
            if (ass_insurance != null)
            {
                asset_model.insurance_activa_id = ass_insurance.insurance_activa_id;
                asset_model.insurance_activa_number = ass_insurance.insurance_activa_number;
                asset_model.insurance_activa_name = ass_insurance.insurance_activa_name;
                asset_model.insurance_activa_date = ass_insurance.insurance_activa_date;
                asset_model.insurance_activa_exp_date = ass_insurance.insurance_activa_exp_date;
                asset_model.insurance_id = ass_insurance.insurance_id;
                asset_model.insurance_activa_description = ass_insurance.insurance_activa_description;
            }

            //Data view edit
            asset_model.detail_insurance_list = (from aw in db.tr_asset_insurance
                                                 where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)

                                                 join a in db.ms_insurance on aw.insurance_id equals a.insurance_id
                                                 where (a.fl_active == true && a.deleted_date == null)

                                                 select new asset_insurancedetailViewModel()
                                                 {
                                                     insurance_activa_id = aw.insurance_activa_id,
                                                     insurance_activa_number = aw.insurance_activa_number,
                                                     insurance_activa_name = aw.insurance_activa_name,
                                                     insurance_activa_date = aw.insurance_activa_date,
                                                     insurance_activa_exp_date = aw.insurance_activa_exp_date,
                                                     insurance_id = a.insurance_id,
                                                     insurance_name = a.insurance_name,
                                                     insurance_activa_description = aw.insurance_activa_description
                                                 }).ToList<asset_insurancedetailViewModel>();

            return View(asset_model);
        }

        // POST: asset_insurance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "insurance_activa_id, asset_parent_id, insurance_activa_number, insurance_activa_name, insurance_activa_date, insurance_activa_exp_date, insurance_id, insurance_activa_description")] asset_insuranceViewModel asset_ins)
        {
            if (asset_ins.insurance_activa_date > DateTime.Today)
                ModelState.AddModelError("insurance_activa_date", "Please enter a valid date.");

            if (asset_ins.insurance_activa_date > asset_ins.insurance_activa_exp_date)
                ModelState.AddModelError("insurance_activa_exp_date", "Insurance activa expired date must be after insurance activa date.");

            if (ModelState.IsValid && asset_ins.insurance_activa_id > 0)
            {
                tr_asset_insurance ass_insurance = db.tr_asset_insurance.Find(asset_ins.insurance_activa_id);
                ass_insurance.asset_id = asset_ins.asset_parent_id;
                ass_insurance.insurance_activa_number = asset_ins.insurance_activa_number;
                ass_insurance.insurance_activa_name = asset_ins.insurance_activa_name;
                ass_insurance.insurance_activa_date = asset_ins.insurance_activa_date;
                ass_insurance.insurance_activa_exp_date = asset_ins.insurance_activa_exp_date;
                ass_insurance.insurance_id = asset_ins.insurance_id;
                ass_insurance.insurance_activa_description = asset_ins.insurance_activa_description;

                ass_insurance.fl_active = true;
                ass_insurance.updated_date = DateTime.Now;
                ass_insurance.updated_by = UserProfile.UserId;
                ass_insurance.deleted_date = null;
                ass_insurance.deleted_int = null;
                ass_insurance.org_id = UserProfile.OrgId;

                db.Entry(ass_insurance).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            tr_asset_registration ass_reg = db.tr_asset_registration.Find(asset_ins.asset_parent_id);
            asset_ins.FormMode = EnumFormModeKey.Form_Edit;

            asset_ins.asset_parent_id = (int)ass_reg.asset_id;
            asset_ins.asset_parent = ass_reg;

            asset_ins.insurance_list = db.ms_insurance.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            //Data view edit
            asset_ins.detail_insurance_list = (from aw in db.tr_asset_insurance
                                               where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == asset_ins.asset_parent_id)

                                               join a in db.ms_insurance on aw.insurance_id equals a.insurance_id
                                               where (a.fl_active == true && a.deleted_date == null)

                                               select new asset_insurancedetailViewModel()
                                               {
                                                   insurance_activa_id = aw.insurance_activa_id,
                                                   insurance_activa_number = aw.insurance_activa_number,
                                                   insurance_activa_name = aw.insurance_activa_name,
                                                   insurance_activa_date = aw.insurance_activa_date,
                                                   insurance_activa_exp_date = aw.insurance_activa_exp_date,
                                                   insurance_id = a.insurance_id,
                                                   insurance_name = a.insurance_name,
                                                   insurance_activa_description = aw.insurance_activa_description
                                               }).ToList<asset_insurancedetailViewModel>();

            return View(asset_ins);
        }

        // GET: asset_insurance/Delete/5
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

            asset_insuranceViewModel asset_model = new asset_insuranceViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,

                asset_parent_id = (int)ass_reg.asset_id,
                asset_parent = ass_reg
            };

            ms_vendor msvendor = db.ms_vendor.Find(ass_reg.vendor_id);
            asset_model.vendor_name = msvendor.vendor_name;

            //Data view
            asset_model.detail_insurance_list = (from aw in db.tr_asset_insurance
                                                 where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)

                                                 join a in db.ms_insurance on aw.insurance_id equals a.insurance_id
                                                 where (a.fl_active == true && a.deleted_date == null)

                                                 select new asset_insurancedetailViewModel()
                                                 {
                                                     insurance_activa_id = aw.insurance_activa_id,
                                                     insurance_activa_number = aw.insurance_activa_number,
                                                     insurance_activa_name = aw.insurance_activa_name,
                                                     insurance_activa_date = aw.insurance_activa_date,
                                                     insurance_activa_exp_date = aw.insurance_activa_exp_date,
                                                     insurance_id = a.insurance_id,
                                                     insurance_name = a.insurance_name,
                                                     insurance_activa_description = aw.insurance_activa_description
                                                 }).ToList<asset_insurancedetailViewModel>();

            return View(asset_model);
        }

        // POST: asset_warranty/Delete/5
        [HttpGet, ActionName("DeleteItem")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteItem(int id)
        {
            tr_asset_insurance tr_asset_insurance = db.tr_asset_insurance.Find(id);
            if (tr_asset_insurance != null)
            {
                tr_asset_insurance.fl_active = false;
                tr_asset_insurance.deleted_int = UserProfile.UserId;
                tr_asset_insurance.deleted_date = DateTime.Now;

                db.Entry(tr_asset_insurance).State = EntityState.Modified;
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
            var _qry = (from aw in db.tr_asset_insurance
                        where (aw.fl_active == true && aw.deleted_date == null && aw.asset_id == id)
                        select new asset_insuranceViewModel()
                        {
                            insurance_activa_id = aw.insurance_activa_id
                        }).ToList<asset_insuranceViewModel>();

            if (_qry != null)
            {
                foreach (asset_insuranceViewModel ass_ins in _qry)
                {
                    tr_asset_insurance tr_asset_insurance = db.tr_asset_insurance.Find(ass_ins.insurance_activa_id);
                    if (tr_asset_insurance != null)
                    {
                        tr_asset_insurance.fl_active = false;
                        tr_asset_insurance.deleted_int = UserProfile.UserId;
                        tr_asset_insurance.deleted_date = DateTime.Now;

                        db.Entry(tr_asset_insurance).State = EntityState.Modified;
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