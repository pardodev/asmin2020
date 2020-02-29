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
    public class subasset_registrationController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        private string app_root_path;
        private string base_image_path;

        // GET: asubsset_registration
        public ActionResult Index()
        {
            //var _list = db.tr_asset_registration.Where(a => a.fl_active == true && a.deleted_date == null).ToList();
            var _qry = (from ar in db.tr_asset_registration
                        where (ar.fl_active == true && ar.deleted_date == null
                        && ar.company_id == UserProfile.company_id
                        && ar.current_location_id == UserProfile.location_id
                        && ar.asset_type_id == (int)Enum_asset_type_Key.AssetChild)

                        join p in db.tr_asset_registration on ar.asset_parent_id equals p.asset_id
                        where (p.fl_active == true && p.deleted_date == null)
                        //into tmp_t1

                        //from t1 in tmp_t1.
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

            return View(_qry);
        }

        [HttpGet]
        public JsonResult List()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var query_result = (from ar in db.tr_asset_registration
                                where (ar.fl_active == true && ar.deleted_date == null
                                && ar.company_id == UserProfile.company_id
                                && ar.current_location_id == UserProfile.location_id
                                && ar.asset_type_id == (int)Enum_asset_type_Key.AssetChild)

                                join p in db.tr_asset_registration on ar.asset_parent_id equals p.asset_id
                                where (p.fl_active == true && p.deleted_date == null)

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
                                    asset_parent_id = (int)ar.asset_parent_id,
                                    asset_parent_number = p.asset_number,
                                    asset_parent_name = p.asset_name,
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
                                }).ToList();

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
                        where (ar.fl_active == true && ar.deleted_date == null && ar.asset_type_id == (int)Enum_asset_type_Key.AssetChild) && ar.asset_id == id

                        join p in db.tr_asset_registration on ar.asset_parent_id equals p.asset_id
                        where (p.fl_active == true && p.deleted_date == null)

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
                            tr_asset_images = j,
                            department_id = ar.department_id,
                            department = g,
                            employee_id = ar.employee_id,
                            employee = h,
                            asset_description = ar.asset_description,
                        }).First<subasset_registrationViewModel>();

            string imreBase64Data = Convert.ToBase64String(_qry.tr_asset_images.asset_qrcode);
            string imgDataURL = string.Format("data:image/jpg;base64,{0}", imreBase64Data);
            //Passing image data in viewbag to view  
            ViewBag.ImageData = imgDataURL;

            var tuple = new Tuple<subasset_registrationViewModel, string>(_qry, subasset_registrationViewModel.path_file_asset);

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

        // GET: asset_registration/Create/5
        public ActionResult Create(int id = 0)
        {
            int parent_id = (int)id;
            subasset_registrationViewModel subasset = null;
            if (parent_id == 0)
            {
                subasset = new subasset_registrationViewModel()
                {
                    FormMode = EnumFormModeKey.Form_New,

                    hasImage = false,

                    asset_id = 0,

                    asset_type_id = (int)Enum_asset_type_Key.AssetChild,

                    asset_parent_id = parent_id,

                    asset_parent_list = db.tr_asset_registration.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                    company_list = db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                    asset_reg_location_list = db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                    asset_reg_pic_list = db.ms_asset_register_pic.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                    asset_category_list = db.ms_asset_category.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                    vendor_list = db.ms_vendor.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                    asset_location_list = db.ms_asset_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                    department_list = db.ms_department.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                    employee_list = db.ms_employee.AsEnumerable().Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList(),

                    asset_receipt_date = DateTime.Now
                };
            }
            else
            {
                tr_asset_registration asset_parent_db = db.tr_asset_registration.Find(parent_id);
                if (asset_parent_db == null)
                    return RedirectToAction("Create");

                subasset = new subasset_registrationViewModel()
                {
                    FormMode = EnumFormModeKey.Form_New,

                    hasImage = false,

                    asset_id = 0,

                    asset_type_id = (int)Enum_asset_type_Key.AssetChild,

                    asset_parent = asset_parent_db,
                    asset_parent_id = asset_parent_db.asset_id,
                    asset_parent_list = db.tr_asset_registration.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),

                    company_list = db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                    company_id = asset_parent_db.company_id,
                    company = asset_parent_db.ms_asmin_company,

                    asset_reg_location_list = db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                    asset_reg_location_id = asset_parent_db.asset_reg_location_id,
                    asset_reg_location = asset_parent_db.ms_asset_register_location,

                    asset_reg_pic_list = db.ms_asset_register_pic.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                    asset_reg_pic_id = asset_parent_db.asset_reg_pic_id,
                    asset_reg_pic = asset_parent_db.ms_asset_register_pic,

                    asset_category_list = db.ms_asset_category.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                    asset_category = asset_parent_db.ms_asset_category,
                    category_id = asset_parent_db.category_id,

                    vendor_list = db.ms_vendor.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                    vendor = asset_parent_db.ms_vendor,
                    vendor_id = asset_parent_db.vendor_id,

                    asset_location_list = db.ms_asset_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                    asset_location = asset_parent_db.ms_asset_location,
                    location_id = asset_parent_db.location_id,

                    department_list = db.ms_department.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                    department_id = asset_parent_db.department_id,
                    department = asset_parent_db.ms_department,

                    employee_list = db.ms_employee.AsEnumerable().Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList(),
                    employee_id = asset_parent_db.employee_id,
                    employee = asset_parent_db.ms_employee,

                    asset_receipt_date = DateTime.Now
                };
            }
            return View(subasset);
        }

        // POST: asset_registration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "asset_parent_id,asset_number,company_id,asset_reg_location_id,asset_reg_pic_id,category_id,asset_po_number,asset_do_number,asset_name,asset_merk,asset_serial_number,vendor_id,asset_receipt_date,location_id,department_id,employee_id,asset_description,asset_file_attach")] subasset_registrationViewModel asset_reg)
        {
            if (ModelState.IsValid)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region SUBASSET
                        tr_asset_registration asset_parent = db.tr_asset_registration.Find(asset_reg.asset_parent_id);

                        int _last_no = db.tr_asset_registration.Where(a => a.asset_parent_id == asset_reg.asset_parent_id).Count();

                        /*no aktifa seq*/
                        _last_no++;
                        string no_activa = _last_no.ToString().PadLeft(2, '0');

                        string[] complex_no = asset_parent.asset_number.Split(new char[3] { '.', '-', '/' });

                        asset_reg.asset_number = complex_no[0] + "-" + no_activa;

                        tr_asset_registration ass_reg = new tr_asset_registration();
                        ass_reg.asset_type_id = (int)Enum_asset_type_Key.AssetChild; //child
                        ass_reg.asset_parent_id = asset_reg.asset_parent_id;
                        ass_reg.tr_asset_registration2 = asset_parent;

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

                        #region FILEASSET
                        //ass_reg.asset_file_attach = asset_reg.asset_file_attach;
                        if (Request.Files.Count > 0)
                        {
                            //var file = Request.Files[0];
                            app_root_path = Server.MapPath("~/");
                            if (string.IsNullOrWhiteSpace(base_image_path))
                                base_image_path = subasset_registrationViewModel.path_file_asset;

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

                        dbTran.Commit();
                    }
                    catch (Exception _exc)
                    {
                        dbTran.Rollback();
                        throw new Exception("Fail to save create new sub-asset." + _exc.Message);
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
                    sy_email_log.elog_subject = string.Format("Sub Asset Registration");
                    sy_email_log.elog_template = "EMAIL_TEMPLATE_90";

                    var _bodymail = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE_90")).FirstOrDefault();
                    if (_bodymail == null)
                        throw new Exception("Email Template 01 not found");

                    string strBodyMail = _bodymail.app_value;
                    strBodyMail = strBodyMail.Replace("[to]", _qry.department_name + " Department");
                    strBodyMail = strBodyMail.Replace("[action]", "Dispose");
                    strBodyMail = strBodyMail.Replace("[assetnumber]", asset_reg.asset_number);
                    strBodyMail = strBodyMail.Replace("[assetname]", asset_reg.asset_name);

                    ms_asset_location loc = db.ms_asset_location.Find(asset_reg.location_id);
                    if (loc == null)
                        throw new Exception("Sub Asset Location not found");
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
            if (asset_reg.asset_parent_list == null)
                asset_reg.asset_parent_list = db.tr_asset_registration.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.company_list == null)
                asset_reg.company_list = db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_reg_location_list == null)
                asset_reg.asset_reg_location_list = db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_reg_pic_list == null)
                asset_reg.asset_reg_pic_list = db.ms_asset_register_pic.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_category_list == null)
                asset_reg.asset_category_list = db.ms_asset_category.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.vendor_list == null)
                asset_reg.vendor_list = db.ms_vendor.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_location_list == null)
                asset_reg.asset_location_list = db.ms_asset_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.department_list == null)
                asset_reg.department_list = db.ms_department.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.employee_list == null)
                asset_reg.employee_list = db.ms_employee.AsEnumerable().Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList();

            #endregion

            return View(asset_reg);
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

            subasset_registrationViewModel asset_model = new subasset_registrationViewModel()
            {
                FormMode = EnumFormModeKey.Form_Edit,
                asset_type_id = (int)Enum_asset_type_Key.AssetChild,
                asset_parent = ass_reg.tr_asset_registration2,
                asset_parent_id = (int)ass_reg.asset_parent_id,
                company_list = db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                asset_reg_location_list = db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                asset_reg_pic_list = db.ms_asset_register_pic.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                asset_category_list = db.ms_asset_category.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                vendor_list = db.ms_vendor.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                asset_location_list = db.ms_asset_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                department_list = db.ms_department.Where(r => r.fl_active == true && r.deleted_date == null).ToList(),
                employee_list = db.ms_employee.AsEnumerable().Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList(),

                asset_id = ass_reg.asset_id,
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
                asset_receipt_date = ass_reg.asset_receipt_date,
                location_id = ass_reg.location_id,
                department_id = ass_reg.department_id,
                employee_id = ass_reg.employee_id,
                asset_description = ass_reg.asset_description,

                tr_asset_images = db.tr_asset_image.SingleOrDefault(r => r.asset_id == ass_reg.asset_id)
            };

            bool _hasImage = false;
            string base_image_path = subasset_registrationViewModel.path_file_asset;
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
        public ActionResult Edit([Bind(Include = "asset_parent_id,asset_number,company_id,asset_reg_location_id,asset_reg_pic_id,category_id,asset_po_number,asset_do_number,asset_name,asset_merk,asset_serial_number,vendor_id,asset_receipt_date,location_id,department_id,employee_id,asset_description,asset_file_attach")] subasset_registrationViewModel asset_reg)
        {
            int asset_id = Convert.ToInt32(Request.Form["asset_id"]);

            if (ModelState.IsValid && asset_id > 0)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region SUBASSET
                        tr_asset_registration ass_reg = db.tr_asset_registration.Find(asset_id);
                        ass_reg.asset_type_id = (int)Enum_asset_type_Key.AssetChild; //child
                        ass_reg.asset_parent_id = asset_reg.asset_parent_id;

                        ass_reg.company_id = asset_reg.company_id;
                        asset_reg.company = db.ms_asmin_company.Find(asset_reg.company_id);

                        ass_reg.asset_reg_location_id = asset_reg.asset_reg_location_id;
                        asset_reg.asset_reg_location = db.ms_asset_register_location.Find(asset_reg.asset_reg_location_id);

                        ass_reg.asset_reg_pic_id = asset_reg.asset_reg_pic_id;
                        asset_reg.asset_reg_pic = db.ms_asset_register_pic.Find(asset_reg.asset_reg_pic_id);

                        ass_reg.category_id = asset_reg.category_id;
                        asset_reg.asset_category = db.ms_asset_category.Find(asset_reg.category_id);

                        asset_reg.department = db.ms_department.Find(asset_reg.department_id);

                        if (string.IsNullOrWhiteSpace(ass_reg.asset_number))   //incase nomor belum ada --> koreksi nomornya
                        {
                            /*no aktifa seq*/
                            int _last_no = db.tr_asset_registration.Where(r => r.asset_parent_id == asset_reg.asset_parent_id).Count();
                            _last_no++;
                            string no_activa = _last_no.ToString().PadLeft(2, '0');
                            string[] complex_no = ass_reg.tr_asset_registration2.asset_number.Split(new char[3] { '.', '-', '/' });
                            ass_reg.asset_number = complex_no[0] + "-" + no_activa;
                        }
                        else
                        {
                            ass_reg.asset_number = asset_reg.asset_number;
                        }

                        ass_reg.asset_po_number = asset_reg.asset_po_number;
                        ass_reg.asset_do_number = asset_reg.asset_do_number;
                        ass_reg.asset_name = asset_reg.asset_name;
                        ass_reg.asset_merk = asset_reg.asset_merk;
                        ass_reg.asset_serial_number = asset_reg.asset_serial_number;
                        ass_reg.vendor_id = asset_reg.vendor_id;
                        ass_reg.asset_description = asset_reg.asset_description;
                        ass_reg.asset_receipt_date = asset_reg.asset_receipt_date;
                        ass_reg.location_id = asset_reg.location_id;
                        ass_reg.department_id = asset_reg.department_id;
                        ass_reg.employee_id = asset_reg.employee_id;

                        ass_reg.fl_active = true;
                        ass_reg.updated_date = DateTime.Now;
                        ass_reg.updated_by = UserProfile.UserId;
                        ass_reg.deleted_date = null;
                        ass_reg.deleted_by = null;
                        ass_reg.org_id = UserProfile.OrgId;

                        db.Entry(ass_reg).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion

                        #region FILE SUPBASSET
                        if (Request.Files.Count > 0)
                        {
                            //var file = Request.Files[0];
                            app_root_path = Server.MapPath("~/");
                            if (string.IsNullOrWhiteSpace(base_image_path))
                                base_image_path = subasset_registrationViewModel.path_file_asset;

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
                                    base_image_path = subasset_registrationViewModel.path_file_asset;
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
                                    asset_qrcode = GenerateQRCode(asset_reg.asset_number)
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
                        throw new Exception("Fail to save update sub-asset." + _exc.Message);
                    }

                }
                return RedirectToAction("Index");
            }

            #region complete the VM
            if (asset_reg.asset_parent_list == null)
                asset_reg.asset_parent_list = db.tr_asset_registration.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.company_list == null)
                asset_reg.company_list = db.ms_asmin_company.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_reg_location_list == null)
                asset_reg.asset_reg_location_list = db.ms_asset_register_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_reg_pic_list == null)
                asset_reg.asset_reg_pic_list = db.ms_asset_register_pic.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_category_list == null)
                asset_reg.asset_category_list = db.ms_asset_category.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.vendor_list == null)
                asset_reg.vendor_list = db.ms_vendor.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.asset_location_list == null)
                asset_reg.asset_location_list = db.ms_asset_location.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.department_list == null)
                asset_reg.department_list = db.ms_department.Where(r => r.fl_active == true && r.deleted_date == null).ToList();

            if (asset_reg.employee_list == null)
                asset_reg.employee_list = db.ms_employee.AsEnumerable().Where(r => r.fl_active == true && r.deleted_date == null).Select(e => new ms_employee { employee_id = e.employee_id, employee_name = "[" + e.employee_nik + "] - " + e.employee_name, employee_email = e.employee_email }).ToList();
            #endregion

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
            tr_asset_registration tr_asset_registration = db.tr_asset_registration.Find(id);
            if (tr_asset_registration != null)
            {
                tr_asset_registration.fl_active = false;
                tr_asset_registration.deleted_by = UserProfile.OrgId;
                tr_asset_registration.deleted_date = DateTime.Now;

                db.Entry(tr_asset_registration).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
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


        public ActionResult ModalAsset()
        {
            return PartialView();
        }



    }
}
