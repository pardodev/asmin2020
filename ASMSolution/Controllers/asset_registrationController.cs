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
using ZXing;
using System.Drawing;
using System.Drawing.Imaging;
using ASM_UI.App_Helpers;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class asset_registrationController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        private string app_root_path;
        private string base_image_path;


        // GET: asset_registration
        public ActionResult Index()
        {
            //var _list = db.tr_asset_registration.Where(a => a.fl_active == true && a.deleted_date == null).ToList();
            var _qry = new object();
            if (UserProfile.asset_reg_location_id == 2)
            {
                _qry = (from ar in db.tr_asset_registration
                        where (ar.fl_active == true && ar.deleted_date == null
                        && ar.company_id == UserProfile.company_id
                        //&& ar.asset_reg_location_id == UserProfile.asset_reg_location_id 
                        && ar.current_location_id == UserProfile.location_id
                        && ar.asset_type_id == (int)Enum_asset_type_Key.AssetParent)

                        join a in db.ms_vendor on ar.vendor_id equals a.vendor_id
                        where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_asset_register_pic on ar.asset_reg_pic_id equals c.asset_reg_pic_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_asset_type on ar.asset_type_id equals d.activa_type_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_category on ar.category_id equals e.category_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_asmin_company on ar.company_id equals f.company_id
                        where (f.fl_active == true && f.deleted_date == null)

                        join g in db.ms_department on ar.department_id equals g.department_id
                        where (g.fl_active == true && g.deleted_date == null)

                        join h in db.ms_employee on ar.employee_id equals h.employee_id
                        where (h.fl_active == true && h.deleted_date == null)

                        join i in db.ms_asset_location on ar.location_id equals i.location_id
                        where (i.fl_active == true && i.deleted_date == null)

                        select new asset_registrationViewModel()
                        {
                            asset_id = ar.asset_id,
                            asset_type_id = ar.asset_type_id,
                            asset_type = d,
                            asset_number = ar.asset_number,
                            company_id = ar.company_id,
                            company = f,
                            asset_reg_location_id = ar.asset_reg_location_id,
                            asset_reg_location = b,
                            asset_reg_pic_id = ar.asset_reg_pic_id,
                            asset_reg_pic = c,
                            category_id = ar.category_id,
                            asset_category = e,
                            asset_po_number = ar.asset_po_number,
                            asset_do_number = ar.asset_do_number,
                            asset_name = ar.asset_name,
                            asset_merk = ar.asset_merk,
                            asset_serial_number = ar.asset_serial_number,
                            vendor_id = ar.vendor_id,
                            vendor = a,
                            asset_receipt_date = ar.asset_receipt_date,
                            location_id = ar.location_id,
                            asset_location = i,
                            department_id = ar.department_id,
                            department = g,
                            employee_id = ar.employee_id,
                            employee = h,
                            asset_description = ar.asset_description
                        }).ToList<asset_registrationViewModel>();
            }
            else if (UserProfile.asset_reg_location_id == 1)
            {
                _qry = (from ar in db.tr_asset_registration
                        where (ar.fl_active == true && ar.deleted_date == null
                        && ar.company_id == UserProfile.company_id
                        && ar.asset_type_id == (int)Enum_asset_type_Key.AssetParent)

                        join a in db.ms_vendor on ar.vendor_id equals a.vendor_id
                        where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_asset_register_pic on ar.asset_reg_pic_id equals c.asset_reg_pic_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_asset_type on ar.asset_type_id equals d.activa_type_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_category on ar.category_id equals e.category_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_asmin_company on ar.company_id equals f.company_id
                        where (f.fl_active == true && f.deleted_date == null)

                        join g in db.ms_department on ar.department_id equals g.department_id
                        where (g.fl_active == true && g.deleted_date == null)

                        join h in db.ms_employee on ar.employee_id equals h.employee_id
                        where (h.fl_active == true && h.deleted_date == null)

                        join i in db.ms_asset_location on ar.location_id equals i.location_id
                        where (i.fl_active == true && i.deleted_date == null)

                        select new asset_registrationViewModel()
                        {
                            asset_id = ar.asset_id,
                            asset_type_id = ar.asset_type_id,
                            asset_type = d,
                            asset_number = ar.asset_number,
                            company_id = ar.company_id,
                            company = f,
                            asset_reg_location_id = ar.asset_reg_location_id,
                            asset_reg_location = b,
                            asset_reg_pic_id = ar.asset_reg_pic_id,
                            asset_reg_pic = c,
                            category_id = ar.category_id,
                            asset_category = e,
                            asset_po_number = ar.asset_po_number,
                            asset_do_number = ar.asset_do_number,
                            asset_name = ar.asset_name,
                            asset_merk = ar.asset_merk,
                            asset_serial_number = ar.asset_serial_number,
                            vendor_id = ar.vendor_id,
                            vendor = a,
                            asset_receipt_date = ar.asset_receipt_date,
                            location_id = ar.location_id,
                            asset_location = i,
                            department_id = ar.department_id,
                            department = g,
                            employee_id = ar.employee_id,
                            employee = h,
                            asset_description = ar.asset_description
                        }).ToList<asset_registrationViewModel>();
            }
            return View(_qry);
        }

        [HttpGet]
        public JsonResult List()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var query_result = new object();
            int pic_asset = 0;

            if (UserProfile.department_id == 1)
                pic_asset = 1;
            else
                pic_asset = 2;

            if (UserProfile.asset_reg_location_id == 2)
            {
                query_result = (from ar in db.tr_asset_registration
                                where (ar.fl_active == true && ar.deleted_date == null
                                && ar.company_id == UserProfile.company_id                                
                                && ar.current_location_id == UserProfile.location_id        
                                && ar.asset_reg_pic_id == pic_asset                       
                                && ar.asset_type_id == (int)Enum_asset_type_Key.AssetParent)

                                join a in db.ms_vendor on ar.vendor_id equals a.vendor_id
                                where (a.fl_active == true && a.deleted_date == null)

                                join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                                where (b.fl_active == true && b.deleted_date == null)

                                join c in db.ms_asset_register_pic on ar.asset_reg_pic_id equals c.asset_reg_pic_id
                                where (c.fl_active == true && c.deleted_date == null)

                                join d in db.ms_asset_type on ar.asset_type_id equals d.activa_type_id
                                where (d.fl_active == true && d.deleted_date == null)

                                join e in db.ms_asset_category on ar.category_id equals e.category_id
                                where (e.fl_active == true && e.deleted_date == null)

                                join f in db.ms_asmin_company on ar.company_id equals f.company_id
                                where (f.fl_active == true && f.deleted_date == null)

                                join g in db.ms_department on ar.department_id equals g.department_id
                                where (g.fl_active == true && g.deleted_date == null)

                                join h in db.ms_employee on ar.employee_id equals h.employee_id
                                where (h.fl_active == true && h.deleted_date == null)

                                join i in db.ms_asset_location on ar.location_id equals i.location_id
                                where (i.fl_active == true && i.deleted_date == null)

                                select new asset_registrationViewModel()
                                {
                                    asset_id = ar.asset_id,
                                    asset_type_id = ar.asset_type_id,
                                    asset_type = d,
                                    asset_number = ar.asset_number,
                                    company_id = ar.company_id,
                                    company = f,
                                    asset_reg_location_id = ar.asset_reg_location_id,
                                    asset_reg_location = b,
                                    asset_reg_pic_id = ar.asset_reg_pic_id,
                                    asset_reg_pic = c,
                                    category_id = ar.category_id,
                                    asset_category = e,
                                    asset_po_number = ar.asset_po_number,
                                    asset_do_number = ar.asset_do_number,
                                    asset_name = ar.asset_name,
                                    asset_merk = ar.asset_merk,
                                    asset_serial_number = ar.asset_serial_number,
                                    vendor_id = ar.vendor_id,
                                    vendor = a,
                                    asset_receipt_date = ar.asset_receipt_date,
                                    location_id = ar.location_id,
                                    asset_location = i,
                                    department_id = ar.department_id,
                                    department = g,
                                    employee_id = ar.employee_id,
                                    employee = h,
                                    asset_description = ar.asset_description
                                }).ToList<asset_registrationViewModel>();
            }
            else if (UserProfile.asset_reg_location_id == 1)
            {
                query_result = (from ar in db.tr_asset_registration
                                where (ar.fl_active == true && ar.deleted_date == null
                                && ar.company_id == UserProfile.company_id
                                && ar.asset_reg_pic_id == pic_asset
                                && ar.asset_type_id == (int)Enum_asset_type_Key.AssetParent)

                                join a in db.ms_vendor on ar.vendor_id equals a.vendor_id
                                where (a.fl_active == true && a.deleted_date == null)

                                join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                                where (b.fl_active == true && b.deleted_date == null)

                                join c in db.ms_asset_register_pic on ar.asset_reg_pic_id equals c.asset_reg_pic_id
                                where (c.fl_active == true && c.deleted_date == null)

                                join d in db.ms_asset_type on ar.asset_type_id equals d.activa_type_id
                                where (d.fl_active == true && d.deleted_date == null)

                                join e in db.ms_asset_category on ar.category_id equals e.category_id
                                where (e.fl_active == true && e.deleted_date == null)

                                join f in db.ms_asmin_company on ar.company_id equals f.company_id
                                where (f.fl_active == true && f.deleted_date == null)

                                join g in db.ms_department on ar.department_id equals g.department_id
                                where (g.fl_active == true && g.deleted_date == null)

                                join h in db.ms_employee on ar.employee_id equals h.employee_id
                                where (h.fl_active == true && h.deleted_date == null)

                                join i in db.ms_asset_location on ar.location_id equals i.location_id
                                where (i.fl_active == true && i.deleted_date == null)

                                select new asset_registrationViewModel()
                                {
                                    asset_id = ar.asset_id,
                                    asset_type_id = ar.asset_type_id,
                                    asset_type = d,
                                    asset_number = ar.asset_number,
                                    company_id = ar.company_id,
                                    company = f,
                                    asset_reg_location_id = ar.asset_reg_location_id,
                                    asset_reg_location = b,
                                    asset_reg_pic_id = ar.asset_reg_pic_id,
                                    asset_reg_pic = c,
                                    category_id = ar.category_id,
                                    asset_category = e,
                                    asset_po_number = ar.asset_po_number,
                                    asset_do_number = ar.asset_do_number,
                                    asset_name = ar.asset_name,
                                    asset_merk = ar.asset_merk,
                                    asset_serial_number = ar.asset_serial_number,
                                    vendor_id = ar.vendor_id,
                                    vendor = a,
                                    asset_receipt_date = ar.asset_receipt_date,
                                    location_id = ar.location_id,
                                    asset_location = i,
                                    department_id = ar.department_id,
                                    department = g,
                                    employee_id = ar.employee_id,
                                    employee = h,
                                    asset_description = ar.asset_description
                                }).ToList<asset_registrationViewModel>();
            }
            return Json(new { data = query_result }, JsonRequestBehavior.AllowGet);
        }

        // GET: asset_registration/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_asset_registration tr_asset_registration = db.tr_asset_registration.Find(id);
            if (tr_asset_registration == null)
            {
                return HttpNotFound();
            }

            var _qry = (from ar in db.tr_asset_registration
                        where (ar.fl_active == true && ar.asset_id == id && ar.deleted_date == null
                        && ar.company_id == UserProfile.company_id
                        && ar.asset_type_id == (int)Enum_asset_type_Key.AssetParent)

                        join a in db.ms_vendor on ar.vendor_id equals a.vendor_id
                        where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_asset_register_pic on ar.asset_reg_pic_id equals c.asset_reg_pic_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_asset_type on ar.asset_type_id equals d.activa_type_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_category on ar.category_id equals e.category_id
                        where (e.fl_active == true && e.deleted_date == null)

                        join f in db.ms_asmin_company on ar.company_id equals f.company_id
                        where (f.fl_active == true && f.deleted_date == null)

                        join g in db.ms_department on ar.department_id equals g.department_id
                        where (g.fl_active == true && g.deleted_date == null)

                        join h in db.ms_employee on ar.employee_id equals h.employee_id
                        where (h.fl_active == true && h.deleted_date == null)

                        join i in db.ms_asset_location on ar.location_id equals i.location_id
                        where (i.fl_active == true && i.deleted_date == null)

                        join j in db.tr_asset_image on ar.asset_id equals j.asset_id

                        select new asset_registrationViewModel()
                        {
                            asset_id = ar.asset_id,
                            asset_type_id = ar.asset_type_id,
                            asset_type = d,
                            asset_number = ar.asset_number,
                            company_id = ar.company_id,
                            company = f,
                            asset_reg_location_id = ar.asset_reg_location_id,
                            asset_reg_location = b,
                            asset_reg_pic_id = ar.asset_reg_pic_id,
                            asset_reg_pic = c,
                            category_id = ar.category_id,
                            asset_category = e,
                            asset_po_number = ar.asset_po_number,
                            asset_do_number = ar.asset_do_number,
                            asset_name = ar.asset_name,
                            asset_merk = ar.asset_merk,
                            asset_serial_number = ar.asset_serial_number,
                            tr_asset_images = j,
                            vendor_id = ar.vendor_id,
                            vendor = a,
                            asset_receipt_date = ar.asset_receipt_date,
                            location_id = ar.location_id,
                            asset_location = i,
                            department_id = ar.department_id,
                            department = g,
                            employee_id = ar.employee_id,
                            employee = h,
                            asset_description = ar.asset_description
                        }).First<asset_registrationViewModel>();

            //subasset_registration
            var _qrysub = (from ar in db.tr_asset_registration
                           where (ar.fl_active == true && ar.deleted_date == null
                           && ar.company_id == UserProfile.company_id
                           && ar.asset_type_id == (int)Enum_asset_type_Key.AssetChild)

                           join p in db.tr_asset_registration on ar.asset_parent_id equals p.asset_id
                           where (p.fl_active == true && p.deleted_date == null
                           && ar.asset_parent_id == id && p.company_id == UserProfile.company_id
                           )

                           join a in db.ms_vendor on ar.vendor_id equals a.vendor_id
                           where (a.fl_active == true && a.deleted_date == null)

                           join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                           where (b.fl_active == true && b.deleted_date == null)

                           join c in db.ms_asset_register_pic on ar.asset_reg_pic_id equals c.asset_reg_pic_id
                           where (c.fl_active == true && c.deleted_date == null)

                           join d in db.ms_asset_type on ar.asset_type_id equals d.activa_type_id
                           where (d.fl_active == true && d.deleted_date == null)

                           join e in db.ms_asset_category on ar.category_id equals e.category_id
                           where (e.fl_active == true && e.deleted_date == null)

                           join f in db.ms_asmin_company on ar.company_id equals f.company_id
                           where (f.fl_active == true && f.deleted_date == null)

                           join g in db.ms_department on ar.department_id equals g.department_id
                           where (g.fl_active == true && g.deleted_date == null)

                           join h in db.ms_employee on ar.employee_id equals h.employee_id
                           where (h.fl_active == true && h.deleted_date == null)

                           join i in db.ms_asset_location on ar.location_id equals i.location_id
                           where (i.fl_active == true && i.deleted_date == null)

                           select new subasset_registrationViewModel()
                           {
                               asset_parent = p,
                               asset_parent_id = (int)ar.asset_parent_id,
                               asset_id = ar.asset_id,
                               asset_type_id = ar.asset_type_id,
                               asset_type = d,
                               asset_number = ar.asset_number,
                               company_id = ar.company_id,
                               company = f,
                               asset_reg_location_id = ar.asset_reg_location_id,
                               asset_reg_location = b,
                               asset_reg_pic_id = ar.asset_reg_pic_id,
                               asset_reg_pic = c,
                               category_id = ar.category_id,
                               asset_category = e,
                               asset_po_number = ar.asset_po_number,
                               asset_do_number = ar.asset_do_number,
                               asset_name = ar.asset_name,
                               asset_merk = ar.asset_merk,
                               asset_serial_number = ar.asset_serial_number,
                               vendor_id = ar.vendor_id,
                               vendor = a,
                               asset_receipt_date = ar.asset_receipt_date,
                               location_id = ar.location_id,
                               asset_location = i,
                               department_id = ar.department_id,
                               department = g,
                               employee_id = ar.employee_id,
                               employee = h,
                               asset_description = ar.asset_description
                           }).ToList<subasset_registrationViewModel>();

            string imreBase64Data = Convert.ToBase64String(_qry.tr_asset_images.asset_qrcode);
            string imgDataURL = string.Format("data:image/jpg;base64,{0}", imreBase64Data);
            //Passing image data in viewbag to view  
            ViewBag.ImageData = imgDataURL;

            var tuple = new Tuple<asset_registrationViewModel, List<subasset_registrationViewModel>, string>(_qry, _qrysub, asset_registrationViewModel.path_file_asset);

            return View(tuple);
        }

        //QR Code
        private byte[] GenerateQRCode(string qrcodeText)
        {

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            //string barcodePath = Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            byte[] bytes;
            using (MemoryStream memory = new MemoryStream())
            {
                barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                bytes = memory.ToArray();
            }
            return bytes;
        }


        //public List<SelectListItem> GetEmployeeByDepartment(int compid, int deptid)
        public JsonResult GetEmployeeByDepartment(int deptid, int locid)
        {
            var _list = (from emp in db.ms_employee
                         where emp.deleted_date == null && emp.fl_active == true

                         join empdetail in db.ms_employee_detail on emp.employee_id equals empdetail.employee_id
                         where empdetail.deleted_date == null && empdetail.fl_active == true
                                && empdetail.location_id == locid
                                && empdetail.department_id == deptid
                                && empdetail.company_id == UserProfile.company_id

                         //var _list = from t in db.ms_employee_detail
                         //            where t.company_id == compid && t.department_id == deptid
                         //            join e in db.ms_employee on t.employee_id equals e.employee_id
                         //            where e.fl_active == true && e.deleted_date == null
                         select new SelectListItem
                         {
                             Value = emp.employee_id.ToString(),
                             Text = "[" + emp.employee_nik + "] - " + emp.employee_name
                         }).ToList();

            //return _list.ToList<SelectListItem>();
            //return Json(new { data = _list.ToList() }, JsonRequestBehavior.AllowGet);
            return Json(new SelectList(_list.ToList(), "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        // GET: asset_registration/Create
        public ActionResult Create()
        {
            asset_registrationViewModel asset = new asset_registrationViewModel()
            {
                FormMode = EnumFormModeKey.Form_New,

                hasImage = false,

                asset_id = 0,

                asset_type_id = 0,

                asset_quantity = 1,
                company_id = UserProfile.company_id,
                asset_reg_location_id = UserProfile.asset_reg_location_id,
                location_id = UserProfile.location_id,
                //department_id = UserProfile.department_id,

                company_list = db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null && r.company_id == UserProfile.company_id).ToList(),

                asset_reg_location_list = db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null && r.asset_reg_location_id == UserProfile.asset_reg_location_id).ToList(),

                asset_reg_pic_list = db.ms_asset_register_pic.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                asset_category_list = db.ms_asset_category.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                vendor_list = db.ms_vendor.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                asset_location_list = db.ms_asset_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                department_list = db.ms_department.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                //emp akan diambil berdasarkan deptnya
                //employee_list = db.ms_employee.Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList(),
                employee_list = new List<ms_employee>(),

                asset_receipt_date = DateTime.Now

            };

            //if (asset.department_id != 0)
            //{
            //    var _list = (from emp in db.ms_employee
            //                 where emp.deleted_date == null && emp.fl_active == true

            //                 join empdetail in db.ms_employee_detail on emp.employee_id equals empdetail.employee_id
            //                 where empdetail.deleted_date == null && empdetail.fl_active == true
            //                        && empdetail.location_id == UserProfile.location_id
            //                        && empdetail.department_id == asset.department_id
            //                        && empdetail.company_id == UserProfile.company_id

            //                 select new ms_employee
            //                 {
            //                     employee_id = emp.employee_id,
            //                     employee_name = "[" + emp.employee_nik + "] - " + emp.employee_name,
            //                     employee_email = emp.employee_email

            //                 }).ToList();

            //    asset.employee_list = _list;

            //}

            return View(asset);
        }

        // POST: asset_registration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "asset_number,company_id,asset_reg_location_id,asset_reg_pic_id,category_id,asset_po_number,asset_do_number,asset_name,asset_merk,asset_serial_number,vendor_id,asset_receipt_date,location_id,department_id,employee_id,asset_description,asset_file_attach,asset_quantity")] asset_registrationViewModel asset_reg)
        {
            //asset_reg.company_id = UserProfile.company_id;
            //asset_reg.asset_reg_location_id = UserProfile.asset_reg_location_id;
            //asset_reg.location_id = UserProfile.location_id;
            //asset_reg.department_id = UserProfile.department_id;

            if (ModelState.IsValid)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        /*
                         @Pardo... ada koreksi di penomoran asset, sesuai dengan UR dari user format penomoran asset seperti ini 

Misal : ABHOITVHC201900001-00

Rincian :
1. AB - > Nama Company
2. HO      -> Lokasi Register
3. IT        -> PIC Asset Register (ini selalu IT / HGS)
4. VHC    -> Kategory
5. 2019   -> Tahun Perolehan
6. 00001 -> Nomor urut (Nomer akan kembali dari awal, apabila ada salah satu perubahan di nomer 1 sd 5
                         */
                        int _last_no = db.tr_asset_registration.Where(a => a.company_id == asset_reg.company_id
                        && a.asset_reg_location_id == asset_reg.asset_reg_location_id
                        && a.asset_reg_pic_id == asset_reg.asset_reg_pic_id
                        && a.category_id == asset_reg.category_id
                        && a.asset_receipt_date.Value.Year == asset_reg.asset_receipt_date.Value.Year).Count();

                        int asset_quantity = 1;
                        int.TryParse(asset_reg.asset_quantity.ToString(), out asset_quantity);
                        if (asset_quantity <= 0) asset_quantity = 1;

                        for (var rec_loop = 0; rec_loop < asset_quantity; rec_loop++)
                        {
                            #region ASSET 
                            /*kode perusahana*/
                            asset_reg.company = db.ms_asmin_company.Find(asset_reg.company_id);

                            /*kode lokasi registr*/
                            asset_reg.asset_reg_location = db.ms_asset_register_location.Find(asset_reg.asset_reg_location_id);

                            /* PIC asset */
                            asset_reg.asset_reg_pic = db.ms_asset_register_pic.Find(asset_reg.asset_reg_pic_id);

                            /*kode department*/
                            asset_reg.department = db.ms_department.Find(asset_reg.department_id);

                            /*kategori asset*/
                            asset_reg.asset_category = db.ms_asset_category.Find(asset_reg.category_id);

                            /*tahun*/
                            int ass_year = (asset_reg.asset_receipt_date.HasValue) ? asset_reg.asset_receipt_date.Value.Year : DateTime.Now.Year;

                            /*no aktifa seq*/
                            _last_no++;
                            string no_activa = _last_no.ToString().PadLeft(5, '0');

                            string complex_no = asset_reg.company.company_code.ToUpper()
                                + asset_reg.asset_reg_location.asset_reg_location_code.ToUpper()
                                + asset_reg.asset_reg_pic.asset_reg_pic_code.ToUpper()
                                //+ asset_reg.department.department_code
                                + asset_reg.asset_category.category_code
                                + ass_year.ToString();

                            asset_reg.asset_number = complex_no + no_activa + "-00";
                            //asset_reg.asset_number = complex_no + no_activa;

                            tr_asset_registration ass_reg = new tr_asset_registration();
                            ass_reg.asset_type_id = 1; //parent
                            ass_reg.asset_number = asset_reg.asset_number;
                            ass_reg.company_id = asset_reg.company_id;
                            ass_reg.asset_reg_location_id = asset_reg.asset_reg_location_id;
                            ass_reg.asset_reg_pic_id = asset_reg.asset_reg_pic_id;
                            ass_reg.category_id = asset_reg.category_id;
                            ass_reg.asset_po_number = asset_reg.asset_po_number;
                            ass_reg.asset_do_number = asset_reg.asset_do_number;
                            ass_reg.asset_name = asset_reg.asset_name;
                            ass_reg.asset_merk = asset_reg.asset_merk;
                            ass_reg.asset_serial_number = asset_reg.asset_serial_number;
                            ass_reg.vendor_id = asset_reg.vendor_id;
                            ass_reg.asset_receipt_date = asset_reg.asset_receipt_date;
                            ass_reg.location_id = asset_reg.location_id;
                            ass_reg.current_location_id = asset_reg.location_id;
                            ass_reg.department_id = asset_reg.department_id;
                            ass_reg.current_department_id = asset_reg.department_id;
                            ass_reg.asset_description = asset_reg.asset_description;
                            ass_reg.employee_id = asset_reg.employee_id;
                            ass_reg.current_employee_id = asset_reg.employee_id;

                            ass_reg.fl_active = true;
                            ass_reg.created_date = DateTime.Now;
                            ass_reg.created_by = UserProfile.UserId;
                            ass_reg.updated_date = DateTime.Now;
                            ass_reg.updated_by = UserProfile.UserId;
                            ass_reg.deleted_date = null;
                            ass_reg.deleted_by = null;
                            ass_reg.org_id = UserProfile.OrgId;

                            ass_reg = db.tr_asset_registration.Add(ass_reg);
                            db.SaveChanges();
                            #endregion

                            #region FILE ASSET
                            //ass_reg.asset_file_attach = asset_reg.asset_file_attach;
                            if (Request.Files.Count > 0)
                            {
                                //var file = Request.Files[0];
                                app_root_path = Server.MapPath("~/");
                                if (string.IsNullOrWhiteSpace(base_image_path))
                                    base_image_path = asset_registrationViewModel.path_file_asset;

                                string img_path = Server.MapPath(base_image_path);
                                if (!Directory.Exists(img_path))
                                    Directory.CreateDirectory(img_path);

                                var file = Request.Files["asset_img_file"];
                                if (file != null && file.ContentLength > 0)
                                {
                                    var fileName = "asset" + ass_reg.asset_id.ToString() + "_" + Path.GetFileName(file.FileName);
                                    var path = Path.Combine(img_path, fileName);
                                    file.SaveAs(path);
                                    tr_asset_image _ass_img = new tr_asset_image()
                                    {
                                        asset_id = ass_reg.asset_id,
                                        asset_img_address = fileName,
                                        asset_qrcode = GenerateQRCode(asset_reg.asset_number)
                                    };
                                    db.tr_asset_image.Add(_ass_img);
                                    db.SaveChanges();
                                }
                            }
                            #endregion
                        }
                        dbTran.Commit();
                    }
                    catch (Exception _exc)
                    {
                        dbTran.Rollback();
                        string msgError = _exc.Message;
                        if (_exc.InnerException != null)
                            msgError += ". Inner Exception:" + _exc.InnerException.Message;
                        throw new Exception("Fail to create new asset. " + msgError);
                    }

                }

                #region "Send Email Notif to accounting department"
                try
                {
                    ms_disposal_type Dept3 = db.ms_disposal_type.Find(3);
                    int acct_id = Convert.ToInt32(Dept3.disposal_by_dept_id);
                    var _qry = (from dept in db.ms_department.Where(dept => dept.department_id == acct_id) select dept).ToList().FirstOrDefault();
                    if (_qry == null)
                        throw new Exception("Department not found");

                    sy_email_log sy_email_log = new sy_email_log();
                    sy_email_log.elog_to = _qry.department_email;
                    sy_email_log.elog_subject = string.Format("Asset Registration");
                    sy_email_log.elog_template = "EMAIL_TEMPLATE_01";

                    var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_01")).FirstOrDefault();
                    if (_bodymail == null)
                        throw new Exception("Email Template 01 not found");

                    string strBodyMail = _bodymail.app_value;
                    strBodyMail = strBodyMail.Replace("[to]", _qry.department_name + " Department");
                    strBodyMail = strBodyMail.Replace("[action]", "Dispose");
                    strBodyMail = strBodyMail.Replace("[assetnumber]", asset_reg.asset_number);
                    strBodyMail = strBodyMail.Replace("[assetname]", asset_reg.asset_name);

                    ms_asset_location loc = db.ms_asset_location.Find(asset_reg.location_id);
                    if (loc == null)
                        throw new Exception("Asset Location not found");
                    strBodyMail = strBodyMail.Replace("[assetlocation]", loc.location_name);

                    ms_department deptment = db.ms_department.Find(asset_reg.department_id);
                    if (deptment == null)
                        throw new Exception("Department not found");
                    strBodyMail = strBodyMail.Replace("[department]", deptment.department_name);

                    sy_email_log.elog_body = strBodyMail;
                    var EmailHelper = new EmailHelper()
                    {
                        ToAddress = sy_email_log.elog_to,
                        Email_Template = sy_email_log.elog_template,
                        MailSubject = sy_email_log.elog_subject,
                        MailBody = sy_email_log.elog_body
                    };
                    EmailHelper.Send();
                }
                catch { }
                #endregion

                return RedirectToAction("Index");

            }

            #region complete the VM

            //if (asset_reg.company_list == null)
            asset_reg.company_list = db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null && r.company_id == UserProfile.company_id).ToList();

            //if (asset_reg.asset_reg_location_list == null)
            asset_reg.asset_reg_location_list = db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null && r.asset_reg_location_id == UserProfile.asset_reg_location_id).ToList();

            if (asset_reg.asset_reg_pic_list == null || asset_reg.asset_reg_pic_list.Count == 0)
                asset_reg.asset_reg_pic_list = db.ms_asset_register_pic.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_category_list == null || asset_reg.asset_category_list.Count == 0)
                asset_reg.asset_category_list = db.ms_asset_category.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.vendor_list == null || asset_reg.vendor_list.Count == 0)
                asset_reg.vendor_list = db.ms_vendor.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_location_list == null || asset_reg.asset_location_list.Count == 0)
                asset_reg.asset_location_list = db.ms_asset_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.department_list == null || asset_reg.department_list.Count == 0)
                asset_reg.department_list = db.ms_department.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            //asset_reg.employee_list = db.ms_employee.Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList();
            if (asset_reg.department_id != 0 || asset_reg.department_id != null)
            {
                var emp_list = from t1 in db.ms_employee
                               join t2 in db.ms_employee_detail on t1.employee_id equals t2.employee_id
                               where t2.department_id == asset_reg.department_id && t1.deleted_date == null && t1.fl_active == true
                               select new ms_employee()
                               {
                                   employee_id = t1.employee_id,
                                   employee_name = "[" + t1.employee_nik + "] - " + t1.employee_name,
                                   employee_email = t1.employee_email
                               };
                asset_reg.employee_list = emp_list.ToList();
            }
            else
                asset_reg.employee_list = db.ms_employee.Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList();

            return View(asset_reg);
            #endregion

        }

        // GET: asset_registration/Edit/5
        public ActionResult Edit(int? id)
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

            asset_registrationEditViewModel asset_model = new asset_registrationEditViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,
                asset_type_id = 1,
                company_list = db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null && r.company_id == UserProfile.company_id).ToList(),
                asset_reg_location_list = db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null && r.asset_reg_location_id == UserProfile.asset_reg_location_id).ToList(),
                asset_reg_pic_list = db.ms_asset_register_pic.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                asset_category_list = db.ms_asset_category.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                vendor_list = db.ms_vendor.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                asset_location_list = db.ms_asset_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                department_list = db.ms_department.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                employee_list = db.ms_employee.AsEnumerable().Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList(),

                asset_id = ass_reg.asset_id,
                //asset_type_id = ass_reg.asset_type_id,
                asset_number = ass_reg.asset_number,
                company_id = ass_reg.company_id,
                asset_reg_location_id = ass_reg.asset_reg_location_id,
                asset_reg_pic_id = ass_reg.asset_reg_pic_id,
                category_id = ass_reg.category_id,
                asset_serial_number = ass_reg.asset_serial_number,
                asset_po_number = ass_reg.asset_po_number,
                asset_do_number = ass_reg.asset_do_number,
                asset_name = ass_reg.asset_name,
                asset_merk = ass_reg.asset_merk,
                vendor_id = ass_reg.vendor_id,
                asset_quantity = 1,
                asset_receipt_date = ass_reg.asset_receipt_date,
                location_id = ass_reg.location_id,
                department_id = ass_reg.department_id,
                employee_id = ass_reg.employee_id,
                asset_description = ass_reg.asset_description,

                tr_asset_images = db.tr_asset_image.SingleOrDefault(r => r.asset_id == ass_reg.asset_id)
            };

            //employee gak bisa di edit
            //if (ass_reg.department_id != 0 || ass_reg.department_id != null)
            //{
            //    var emp_list = (from t1 in db.ms_employee
            //                   join t2 in db.ms_employee_detail on t1.employee_id equals t2.employee_id
            //                   where t2.department_id == ass_reg.department_id && t1.deleted_date == null && t1.fl_active == true
            //                   select new ms_employee
            //                   {
            //                       employee_id = t1.employee_id,
            //                       employee_name = "[" + t1.employee_nik + "] - " + t1.employee_name,
            //                       employee_email = t1.employee_email
            //                   }).Distinct();

            //    asset_model.employee_list = emp_list.ToList();
            //}
            //else
            //    asset_model.employee_list = db.ms_employee.Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList();


            bool _hasImage = false;
            string base_image_path = asset_registrationViewModel.path_file_asset;
            string img_path = Server.MapPath(base_image_path);

            if (!Directory.Exists(img_path))
                Directory.CreateDirectory(img_path);

            if (asset_model.tr_asset_images != null)
            {
                var fileName = asset_model.tr_asset_images.asset_img_address;
                string path = Path.Combine(img_path, fileName);
                _hasImage = System.IO.File.Exists(path);
            }
            asset_model.hasImage = _hasImage;

            return View(asset_model);
        }


        // POST: asset_registration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "asset_po_number,asset_do_number,asset_name,asset_merk,asset_serial_number,vendor_id,asset_receipt_date,asset_description,asset_file_attach")] asset_registrationEditViewModel asset_reg)
        {
            int asset_id = Convert.ToInt32(Request.Form["asset_id"]);
            if (asset_id <= 0)
                throw new Exception("Invalid Asset ID");

            //tr_asset_registration ass_ori = db.tr_asset_registration.Find(asset_id);
            if (ModelState.IsValid)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region EDIT ASSET
                        //tdk boleh edit lokasi asset do
                        //compy_id, lokasi_register, lokasi, dept, employee tdk boleh di edit

                        tr_asset_registration ass_reg = db.tr_asset_registration.Find(asset_id);
                        ass_reg.asset_type_id = 1; //parent
                        //hal yang tidak di edit
                        asset_reg.asset_number = ass_reg.asset_number;
                        asset_reg.company_id = ass_reg.company_id;
                        asset_reg.asset_reg_location_id = ass_reg.asset_reg_location_id;
                        asset_reg.asset_reg_pic_id = ass_reg.asset_reg_pic_id;
                        asset_reg.category_id = ass_reg.category_id;
                        asset_reg.location_id = ass_reg.location_id;
                        asset_reg.department_id = ass_reg.department_id;
                        asset_reg.employee_id = ass_reg.employee_id;
                        //hal yang tidak di edit

                        asset_reg.company = db.ms_asmin_company.Find(ass_reg.company_id);

                        asset_reg.asset_reg_location = db.ms_asset_register_location.Find(ass_reg.asset_reg_location_id);

                        asset_reg.asset_reg_pic = db.ms_asset_register_pic.Find(ass_reg.asset_reg_pic_id);

                        asset_reg.asset_category = db.ms_asset_category.Find(ass_reg.category_id);

                        //if (string.IsNullOrWhiteSpace(ass_reg.asset_number))
                        //{
                        //    /*no aktifa seq*/
                        //    int ass_year = (asset_reg.asset_receipt_date.HasValue) ? asset_reg.asset_receipt_date.Value.Year : DateTime.Now.Year;
                        //    //int _last_no = db.tr_asset_registration.Count();
                        //    int _last_no = db.tr_asset_registration.Where(a => a.company_id == asset_reg.company_id
                        //    && a.asset_reg_location_id == asset_reg.asset_reg_location_id
                        //    && a.asset_reg_pic_id == asset_reg.asset_reg_pic_id
                        //    && a.category_id == asset_reg.category_id
                        //    && a.asset_receipt_date.Value.Year == asset_reg.asset_receipt_date.Value.Year).Count();

                        //    _last_no++;
                        //    string no_activa = _last_no.ToString().PadLeft(5, '0');

                        //    string complex_no = asset_reg.company.company_code.ToUpper()
                        //        + asset_reg.asset_reg_location.asset_reg_location_code.ToUpper()
                        //        + asset_reg.asset_reg_pic.asset_reg_pic_code.ToUpper()
                        //        //+ asset_reg.department.department_code
                        //        + asset_reg.asset_category.category_code
                        //        + ass_year.ToString();

                        //    asset_reg.asset_number = complex_no + no_activa + "-00";
                        //}
                        //else
                        //{
                        //    ass_reg.asset_number = asset_reg.asset_number;
                        //}

                        ass_reg.asset_po_number = asset_reg.asset_po_number;
                        ass_reg.asset_do_number = asset_reg.asset_do_number;
                        ass_reg.asset_name = asset_reg.asset_name;
                        ass_reg.asset_merk = asset_reg.asset_merk;
                        ass_reg.asset_serial_number = asset_reg.asset_serial_number;
                        ass_reg.vendor_id = asset_reg.vendor_id;
                        ass_reg.asset_description = asset_reg.asset_description;
                        ass_reg.asset_receipt_date = asset_reg.asset_receipt_date;
                        
                        //ass_reg.location_id = asset_reg.location_id;
                        //ass_reg.department_id = asset_reg.department_id;
                        //ass_reg.employee_id = asset_reg.employee_id;

                        ass_reg.fl_active = true;
                        ass_reg.updated_date = DateTime.Now;
                        ass_reg.updated_by = UserProfile.UserId;
                        ass_reg.deleted_date = null;
                        ass_reg.deleted_by = null;
                        ass_reg.org_id = UserProfile.OrgId;

                        db.Entry(ass_reg).State = EntityState.Modified;
                        db.SaveChanges();

                        #endregion

                        #region FILE ASSEST
                        if (Request.Files.Count > 0)
                        {
                            //var file = Request.Files[0];
                            app_root_path = Server.MapPath("~/");
                            if (string.IsNullOrWhiteSpace(base_image_path))
                                base_image_path = asset_registrationEditViewModel.path_file_asset;

                            string img_path = Server.MapPath(base_image_path);
                            if (!Directory.Exists(img_path))
                                Directory.CreateDirectory(img_path);

                            var file = Request.Files["asset_img_file"];
                            if (file != null && file.ContentLength > 0)
                            {
                                string fileName = ""; string path = "";

                                //check existing and delete dari db dan folder
                                tr_asset_image img_db = db.tr_asset_image.SingleOrDefault(c => c.asset_id == asset_id);
                                if (img_db != null)
                                {
                                    fileName = img_db.asset_img_address;
                                    base_image_path = asset_registrationEditViewModel.path_file_asset;
                                    img_path = Server.MapPath(base_image_path);
                                    path = Path.Combine(img_path, fileName);
                                    if (System.IO.File.Exists(path))
                                        System.IO.File.Delete(path);

                                    db.tr_asset_image.Remove(img_db);
                                    db.SaveChanges();
                                }

                                //insert new
                                fileName = "asset" + ass_reg.asset_id.ToString() + "_" + Path.GetFileName(file.FileName);
                                path = Path.Combine(img_path, fileName);
                                file.SaveAs(path);

                                tr_asset_image _ass_img = new tr_asset_image()
                                {
                                    asset_id = ass_reg.asset_id,
                                    asset_img_address = fileName,
                                    asset_qrcode = GenerateQRCode(ass_reg.asset_number)
                                };
                                db.tr_asset_image.Add(_ass_img);
                                db.SaveChanges();
                            }
                        }
                        #endregion
                        dbTran.Commit();
                    }
                    catch (Exception _exc)
                    {
                        dbTran.Rollback();
                        throw new Exception("Fail to save update asset." + _exc.Message);
                    }

                    return RedirectToAction("Index");
                }
            }

            //if (asset_reg.company_list == null)
            asset_reg.company_list = db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null && r.company_id == UserProfile.company_id).ToList();

            //if (asset_reg.asset_reg_location_list == null)
            asset_reg.asset_reg_location_list = db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null && r.asset_reg_location_id == UserProfile.asset_reg_location_id).ToList();

            if (asset_reg.asset_reg_pic_list == null || asset_reg.asset_reg_pic_list.Count == 0)
                asset_reg.asset_reg_pic_list = db.ms_asset_register_pic.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_category_list == null || asset_reg.asset_category_list.Count == 0)
                asset_reg.asset_category_list = db.ms_asset_category.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.vendor_list == null || asset_reg.vendor_list.Count == 0)
                asset_reg.vendor_list = db.ms_vendor.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_location_list == null || asset_reg.asset_location_list.Count == 0)
                asset_reg.asset_location_list = db.ms_asset_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.department_list == null || asset_reg.department_list.Count == 0)
                asset_reg.department_list = db.ms_department.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            //if (asset_reg.department_id != 0 || asset_reg.department_id != null)
            //{
            //    asset_reg.employee_list = db.ms_employee.Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList();
            //}
            //if (asset_reg.department_id != 0 || asset_reg.department_id != null)
            //{
            //    var emp_list = from t1 in db.ms_employee
            //                   join t2 in db.ms_employee_detail on t1.employee_id equals t2.employee_id
            //                   where t2.department_id == asset_reg.department_id && t1.deleted_date == null && t1.fl_active == true
            //                   select new ms_employee()
            //                   {
            //                       employee_id = t1.employee_id,
            //                       employee_name = "[" + t1.employee_nik + "] - " + t1.employee_name,
            //                       employee_email = t1.employee_email
            //                   };

            //    asset_reg.employee_list = emp_list.ToList();
            //}
            //else
            //    asset_reg.employee_list = db.ms_employee.Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList();

            return View(asset_reg);
        }


        //// GET: asset_registration/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tr_asset_registration tr_asset_registration = db.tr_asset_registration.Find(id);
        //    if (tr_asset_registration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tr_asset_registration);
        //}



        // POST: asset_registration/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            //tr_asset_registration tr_asset_registration = db.tr_asset_registration.Find(id);
            //var qry = from t1 in db.tr_asset_registration
            //          join t2 in db.tr_mutation_request on t1.asset_id equals t2.asset_id
            //          join t3 in db.tr_mutation_process in t1.asset_id equals t3.asset_id
            //    select
            //if (tr_asset_registration != null)
            //{
            //    tr_asset_registration.fl_active = false;
            //    tr_asset_registration.deleted_by = UserProfile.OrgId;
            //    tr_asset_registration.deleted_date = DateTime.Now;

            //    db.Entry(tr_asset_registration).State = EntityState.Modified;
            //    db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(0, JsonRequestBehavior.AllowGet);
            //}
            //return RedirectToAction("Index");

            //db.tr_asset_registration.Remove(tr_asset_registration);
            //db.SaveChanges();
            //return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
                            where t.fl_active == true && t.deleted_date == null && t.asset_type_id == 1
                            select t;

                return Json(new { data = _list.ToList() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var _list = from t in db.tr_asset_registration
                            where t.fl_active == true && t.deleted_date == null && t.asset_type_id == 1 && t.asset_id == asset_id
                            select t;

                return Json(_list.ToList(), JsonRequestBehavior.AllowGet);
            }

            //return Json(AssetList, JsonRequestBehavior.AllowGet);

        }

    }
}
