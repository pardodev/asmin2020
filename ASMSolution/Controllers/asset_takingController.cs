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
using System.Globalization;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class asset_takingController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        private string app_root_path;
        private string base_image_path;

        // GET: asset_taking/Index
        public ActionResult Index(asset_takingViewModel model)
        {
            //combobox reg location
            if (UserProfile.asset_reg_location_id == 1)
            {
                model.location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null))
                                       select m).ToList();
            }
            else
            {
                model.location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
                                       select m).ToList();
            }
            if (model.location_id == null || model.location_id == 0)
            {
                ModelState.Clear();
                model.location_id = model.location_list.FirstOrDefault().location_id;
            }

            //combobox year
            List<SelectListItem> year_list = new List<SelectListItem>();
            for (int loop_int = DateTime.Now.Year; loop_int >= DateTime.Now.Year - 2; loop_int--)
            {
                year_list.Add(new SelectListItem
                {
                    Text = loop_int.ToString(),
                    Value = loop_int.ToString()
                });
            }
            model.period_year_list = year_list;
            if (model.period_year == null)
                model.period_year = DateTime.Today.Year;

            //combobox month
            string[] arr_month = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<SelectListItem> month_list = new List<SelectListItem>();
            for (int loop_int = 1; loop_int <= 12; loop_int++)
            {
                month_list.Add(new SelectListItem
                {
                    Text = arr_month[loop_int - 1],
                    Value = loop_int.ToString()
                });
            }
            model.period_month_list = month_list;
            if (model.period_month == null)
                model.period_month = DateTime.Today.Month;

            return View(model);
        }

        // GET: asset_taking/Uploda_data
        public ActionResult UploadData()
        {
            asset_takingViewModel model = new asset_takingViewModel();

            model.asset_taking_id = 0;
            model.company_name = UserProfile.CompanyName;
            model.location_reg_list = (from r in db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null && r.asset_reg_location_id == UserProfile.asset_reg_location_id)
                                       select r).ToList();
            model.location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
                                   select m).ToList();
            model.location_id = UserProfile.location_id;
            //combobox year
            List<SelectListItem> year_list = new List<SelectListItem>();
            for (int loop_int = DateTime.Now.Year; loop_int >= DateTime.Now.Year - 2; loop_int--)
            {
                year_list.Add(new SelectListItem
                {
                    Text = loop_int.ToString(),
                    Value = loop_int.ToString()
                });
            }
            model.period_year_list = year_list;
            if (model.period_year == null)
                model.period_year = DateTime.Today.Year;

            //combobox month
            string[] arr_month = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<SelectListItem> month_list = new List<SelectListItem>();
            for (int loop_int = 1; loop_int <= 12; loop_int++)
            {
                month_list.Add(new SelectListItem
                {
                    Text = arr_month[loop_int - 1],
                    Value = loop_int.ToString()
                });
            }
            model.period_month_list = month_list;
            if (model.period_month == null)
                model.period_month = DateTime.Today.Month;

            return View(model);
        }

        // GET: asset_taking/Details
        public ActionResult Details(int id)
        {
            var _qry = (from tak in db.tr_asset_taking
                        where tak.fl_active == true && tak.deleted_date == null
                                && tak.asset_taking_id == id

                        join a in db.ms_asmin_company on tak.company_id equals a.company_id
                        where a.fl_active == true && a.deleted_date == null

                        join b in db.ms_asset_location on tak.location_id equals b.location_id
                        where b.fl_active == true && b.deleted_date == null

                        select new asset_takingViewModel
                        {
                            asset_taking_id = tak.asset_taking_id,
                            asset_taking_date = tak.asset_taking_date,
                            company_name = a.company_name,
                            location_name = b.location_name
                        }).FirstOrDefault();

            return View(_qry);
        }

        /// <summary>
        /// list utk taking index
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult GetAssetTakingList(string location_id, string month, string year)
        {
            db.Configuration.ProxyCreationEnabled = false;

            int? locid = 0;
            int intmonth = 0;
            int intyear = 0;

            if (String.IsNullOrEmpty(location_id))
                locid = 0;
            else
                locid = Convert.ToInt32(location_id);

            if (String.IsNullOrEmpty(month))
                intmonth = DateTime.Today.Month;
            else
                intmonth = Convert.ToInt32(month);

            if (String.IsNullOrEmpty(year))
                intyear = DateTime.Today.Year;
            else
                intyear = Convert.ToInt32(year);

            var _qry = (from at in db.tr_asset_taking
                        where (at.fl_active == true && at.deleted_date == null && at.fl_submit_data == true
                        && at.location_id == locid && at.period_month == intmonth && at.period_year == intyear)

                        join a in db.ms_asmin_company on at.company_id equals a.company_id
                        where (a.fl_active == true && a.deleted_date == null)

                        join l in db.ms_asset_location on at.location_id equals l.location_id
                        where l.fl_active == true && l.deleted_date == null

                        join b in db.ms_user on at.created_by equals b.user_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join e in db.ms_employee on b.employee_id equals e.employee_id
                        where (e.fl_active == true && e.deleted_date == null)

                        select new asset_takingViewModel
                        {
                            asset_taking_id = at.asset_taking_id,
                            asset_taking_date = at.asset_taking_date,
                            company_id = a.company_id,
                            company_name = a.company_name,
                            location_name = l.location_name,
                            file_name = at.file_name,
                            created_name = e.employee_name
                        }).ToList<asset_takingViewModel>();

            return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// list utk taking process
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult GetAssetTakingSubmit(int id = 0, int process_id = 0)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var _qry = (from at in db.tr_asset_taking_detail
                        where (at.fl_active == (process_id == 2) && at.deleted_date == null
                                && at.asset_taking_id == id)

                        join a in db.tr_asset_registration.Where(a => (a.fl_active == true)) on at.asset_id equals a.asset_id
                        into a
                        from asub in a.DefaultIfEmpty()

                        join b in db.tr_asset_taking on at.asset_taking_id equals b.asset_taking_id
                        where b.fl_active == true && b.deleted_date == null

                        join c in db.ms_asset_status on at.asset_status_id equals c.asset_status_id

                        select new asset_takingViewModel
                        {
                            asset_taking_id = at.asset_taking_id,
                            asset_id = at.asset_id,
                            asset_status_id = at.asset_status_id,
                            asset_status_name = c.asset_status_name,
                            asset_number = at.asset_number,
                            asset_name = asub.asset_name,
                            location_id = asub.location_id,
                            location_id2 = b.location_id,
                            fl_available_asset = at.fl_available_asset,
                            fl_submit_data = at.fl_active
                        }).ToList<asset_takingViewModel>();

            return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// list utk Asset Status pada saat checking data
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult GetAssetStatusList()
        {
            var _employeelist = db.ms_asset_status.ToList();

            return Json(_employeelist, JsonRequestBehavior.AllowGet);
        }

        // POST: asset_taking/UpdateAssetStatus/asset_number=string&asset_status_id=int
        [HttpGet, ActionName("UpdateAssetStatus")]
        //[ValidateAntiForgeryToken]
        public JsonResult UpdateAssetStatus(string asset_number = "", int asset_status_id = 0)
        {
            try
            {
                (from atd in db.tr_asset_taking_detail where atd.asset_number == asset_number select atd).ToList()
                    .ForEach(atd =>
                    {
                        atd.asset_status_id = asset_status_id;
                        atd.updated_by = UserProfile.UserId;
                        atd.updated_date = DateTime.Now;
                    });
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        //POST: asset_taking/uploaddata
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadData(asset_takingViewModel modaltaking)
        {
            if (Request.Files.Count > 0 && modaltaking.asset_taking_id == 0)
            {
                var fileexist = Request.Files["file_name"];
                if (fileexist == null || fileexist.ContentLength == 0)
                {
                    ModelState.AddModelError("file_name", "Asset Taking File is mandatory.");
                }
                else if (!fileexist.FileName.Contains(".txt"))
                {
                    ModelState.AddModelError("file_name", "File is invalid.");
                }
            }
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (Request.Files.Count > 0)
                        {
                            //var file = Request.Files[0];
                            app_root_path = Server.MapPath("~/");
                            if (string.IsNullOrWhiteSpace(base_image_path))
                                base_image_path = asset_registrationViewModel.path_file_assettaking;

                            string img_path = Server.MapPath(base_image_path);
                            if (!Directory.Exists(img_path))
                                Directory.CreateDirectory(img_path);

                            var file = Request.Files["file_name"];
                            if (file != null && file.ContentLength > 0)
                            {
                                modaltaking.file_name = "Taking_" + string.Format("{0}{1}{2}{3:dMyyHHmmss}", UserProfile.company_id, UserProfile.asset_reg_location_id, UserProfile.department_id, DateTime.Now) + "_" + Path.GetFileName(file.FileName);
                                var path = Path.Combine(img_path, modaltaking.file_name);
                                file.SaveAs(path);
                                DateTime processdate = DateTime.Now;

                                //Header
                                var takingHeaderexist = (from a in db.tr_asset_taking
                                                         where a.period_month == modaltaking.period_month && a.period_year == modaltaking.period_year
                                                              && a.fl_submit_data == true && a.company_id == UserProfile.company_id
                                                              && a.location_id == modaltaking.location_id
                                                              && a.fl_active == true && a.deleted_date == null
                                                         select a).FirstOrDefault();
                                tr_asset_taking takingheader = new tr_asset_taking();
                                if (takingHeaderexist == null)
                                {
                                    int dayofmonth = DateTime.DaysInMonth(Convert.ToInt32(modaltaking.period_year), Convert.ToInt32(modaltaking.period_month));
                                    DateTime perioddate = DateTime.ParseExact(string.Format("{0}/{1}/{2}", modaltaking.period_year.ToString(), modaltaking.period_month.ToString(), dayofmonth.ToString()), "yyyy/M/dd", CultureInfo.InvariantCulture);
                                    takingheader.asset_taking_date = perioddate;
                                    takingheader.period_year = modaltaking.period_year;
                                    takingheader.period_month = modaltaking.period_month;
                                    takingheader.company_id = UserProfile.company_id;
                                    takingheader.location_id = modaltaking.location_id;
                                    takingheader.department_id = UserProfile.department_id;
                                    takingheader.file_name = modaltaking.file_name;
                                    takingheader.fl_active = true;
                                    takingheader.created_by = UserProfile.UserId;
                                    takingheader.created_date = processdate;
                                    takingheader = db.tr_asset_taking.Add(takingheader);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    takingheader.asset_taking_id = takingHeaderexist.asset_taking_id;
                                }
                                modaltaking.asset_taking_id = takingheader.asset_taking_id;

                                //detail
                                string[] datafiles = System.IO.File.ReadAllLines(path);

                                foreach (string datafile in datafiles)
                                {
                                    if (datafile.Trim() != string.Empty)
                                    {
                                        var takingexist = (from a in db.tr_asset_taking_detail
                                                           where a.asset_number == datafile && a.asset_taking_id == modaltaking.asset_taking_id
                                                                && a.fl_active == true
                                                           select a).FirstOrDefault();
                                        //jika sudah ada data maka data tidak dimasukkan dalam detail asset taking
                                        if (takingexist == null)
                                        {
                                            var asset = (from a in db.tr_asset_registration
                                                         where a.asset_number == datafile.Trim()
                                                         && a.fl_active == true
                                                         select a).FirstOrDefault();
                                            if (asset != null)
                                            {
                                                //Fisik sesuai data
                                                tr_asset_taking_detail takingdetail = new tr_asset_taking_detail();
                                                takingdetail.asset_taking_id = takingheader.asset_taking_id;
                                                takingdetail.asset_id = asset.asset_id;
                                                takingdetail.asset_number = datafile;
                                                if (modaltaking.location_id == asset.location_id)
                                                    takingdetail.asset_status_id = 1; //available
                                                else
                                                    takingdetail.asset_status_id = 3; //misplaced
                                                takingdetail.fl_available_asset = true;
                                                takingdetail.fl_active = false;
                                                takingdetail.created_date = processdate;
                                                takingdetail.created_by = UserProfile.UserId;
                                                takingdetail = db.tr_asset_taking_detail.Add(takingdetail);
                                                db.SaveChanges();
                                            }
                                            else
                                            {
                                                //Fisik ada, tp data tidak ditemukan; fl_available_asset = false
                                                tr_asset_taking_detail takingdetail = new tr_asset_taking_detail();
                                                takingdetail.asset_taking_id = takingheader.asset_taking_id;
                                                takingdetail.asset_number = datafile;
                                                //if (modaltaking.location_id == asset.location_id)
                                                takingdetail.asset_status_id = 2; //Not available
                                                //else
                                                //    takingdetail.asset_status_id = 3; //misplaced
                                                takingdetail.fl_available_asset = false;
                                                takingdetail.fl_active = false;
                                                takingdetail.created_date = processdate;
                                                takingdetail.created_by = UserProfile.UserId;
                                                takingdetail = db.tr_asset_taking_detail.Add(takingdetail);
                                                db.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            //jika sudah ada di cek dulu, untuk update fl_available
                                            if (takingexist.fl_available_asset == false)
                                            {
                                                //update fl_active asset taking detail
                                                (from atd in db.tr_asset_taking_detail
                                                 where atd.asset_number == takingexist.asset_number
                                                    && atd.asset_taking_id == modaltaking.asset_taking_id
                                                    && atd.fl_active == true
                                                 select atd).ToList()
                                                .ForEach(atd =>
                                                {
                                                    atd.fl_active = false;
                                                    atd.fl_available_asset = true;
                                                    atd.asset_status_id = 1;
                                                });
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }

                                //asset ada di data,fisik tidak ada
                                var astk = (from ar in db.tr_asset_registration
                                            where !(from atd in db.tr_asset_taking_detail
                                                    where atd.asset_taking_id == modaltaking.asset_taking_id
                                                    select atd.asset_id)
                                                   .Contains(ar.asset_id)
                                                   && ar.company_id == UserProfile.company_id
                                                   && ar.location_id == modaltaking.location_id
                                                   && ar.fl_active == true
                                            select ar).ToList();

                                if (astk.Count > 0)
                                {
                                    foreach (tr_asset_registration asset in astk)
                                    {
                                        tr_asset_taking_detail takingdetail = new tr_asset_taking_detail();
                                        takingdetail.asset_taking_id = takingheader.asset_taking_id;
                                        takingdetail.asset_id = asset.asset_id;
                                        takingdetail.asset_number = asset.asset_number;
                                        if (modaltaking.location_id == asset.location_id)
                                            takingdetail.asset_status_id = 2; //Not available
                                        else
                                            takingdetail.asset_status_id = 3; //misplaced
                                        takingdetail.fl_available_asset = false;
                                        takingdetail.fl_active = false;
                                        takingdetail.created_date = processdate;
                                        takingdetail.created_by = UserProfile.UserId;
                                        takingdetail = db.tr_asset_taking_detail.Add(takingdetail);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            modaltaking.process_id = 1; //checking
                        }
                        else if (modaltaking.asset_taking_id > 0)
                        {
                            //submit
                            //update asset taking fl_submit
                            tr_asset_taking assettakingsubmit = db.tr_asset_taking.Find(modaltaking.asset_taking_id);
                            assettakingsubmit.fl_submit_data = true;
                            assettakingsubmit.fl_active = true;
                            db.Entry(assettakingsubmit).State = EntityState.Modified;
                            db.SaveChanges();

                            //update fl_active asset taking detail
                            (from atd in db.tr_asset_taking_detail where atd.asset_taking_id == modaltaking.asset_taking_id && atd.fl_active == false select atd).ToList()
                            .ForEach(atd => atd.fl_active = true);
                            db.SaveChanges();

                            modaltaking.fl_submit_data = true;
                            modaltaking.file_name = assettakingsubmit.file_name;
                            modaltaking.process_id = 2; //submit
                        }
                        transaction.Commit();
                        ModelState.Clear();
                    }
                    catch (Exception ex)
                    {
                        // roll back all database operations, if any thing goes wrong
                        transaction.Rollback();
                        ModelState.AddModelError("", string.Format("Error occured, records rolledback. {0}", ex.Message));
                    }
                }


            }

            modaltaking.company_name = UserProfile.CompanyName;
            modaltaking.location_reg_list = (from r in db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null && r.asset_reg_location_id == UserProfile.asset_reg_location_id)
                                             select r).ToList();
            modaltaking.location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
                                         select m).ToList();
            //combobox year
            List<SelectListItem> year_list = new List<SelectListItem>();
            for (int loop_int = DateTime.Now.Year; loop_int >= DateTime.Now.Year - 2; loop_int--)
            {
                year_list.Add(new SelectListItem
                {
                    Text = loop_int.ToString(),
                    Value = loop_int.ToString()
                });
            }
            modaltaking.period_year_list = year_list;
            if (modaltaking.period_year == null)
                modaltaking.period_year = DateTime.Today.Year;

            //combobox month
            string[] arr_month = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<SelectListItem> month_list = new List<SelectListItem>();
            for (int loop_int = 1; loop_int <= 12; loop_int++)
            {
                month_list.Add(new SelectListItem
                {
                    Text = arr_month[loop_int - 1],
                    Value = loop_int.ToString()
                });
            }
            modaltaking.period_month_list = month_list;
            if (modaltaking.period_month == null)
                modaltaking.period_month = DateTime.Today.Month;

            return View(modaltaking);
        }

        //// POST: asset_taking/cancelitem
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult CancelItem(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //delete asset header
                    tr_asset_taking tr_asset_taking = db.tr_asset_taking.Find(id);
                    if (tr_asset_taking != null)
                    {
                        if (tr_asset_taking.fl_submit_data != true)
                        {
                            tr_asset_taking.fl_active = false;
                            tr_asset_taking.deleted_by = UserProfile.UserId;
                            tr_asset_taking.deleted_date = DateTime.Now;

                            db.Entry(tr_asset_taking).State = EntityState.Modified;
                            db.SaveChanges();

                            //delete asset detail
                            foreach (var ta in db.tr_asset_taking_detail.Where(x => x.asset_taking_id == id).ToList())
                            {
                                ta.fl_active = false;
                                ta.deleted_by = UserProfile.UserId;
                                ta.deleted_date = DateTime.Now;
                            }

                            db.SaveChanges();
                            transaction.Commit();
                        }
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // roll back all database operations, if any thing goes wrong
                    transaction.Rollback();
                    ViewBag.ResultMessage = string.Format("Error occured, records rolledback. {0}", ex.Message);
                }
            }

            return View();
        }
    }
}