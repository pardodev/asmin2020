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
    public class modalsController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        // GET: modals/ModalInfoAsset/5
        public ActionResult ModalInfoAsset(int? id)
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
                        where (ar.asset_id == id)

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
                        }).FirstOrDefault<asset_registrationViewModel>();

            var _qrysub = new List<subasset_registrationViewModel>();

            if (_qry.asset_type_id == (int)Enum_asset_type_Key.AssetParent)
            {
                //Sub Asset
                _qrysub = (from ar in db.tr_asset_registration
                           where (ar.fl_active == true && ar.deleted_date == null && ar.asset_type_id == (int)Enum_asset_type_Key.AssetChild)

                           join p in db.tr_asset_registration on ar.asset_parent_id equals p.asset_id
                           where (p.fl_active == true && p.deleted_date == null && ar.asset_parent_id == id)

                           join a in db.ms_vendor on ar.vendor_id equals a.vendor_id
                           where (a.fl_active == true && a.deleted_date == null)

                           join b in db.ms_asset_type on ar.asset_type_id equals b.activa_type_id

                           select new subasset_registrationViewModel()
                           {
                               asset_parent = p,
                               asset_parent_id = (int)ar.asset_parent_id,
                               asset_id = ar.asset_id,
                               asset_type_id = ar.asset_type_id,
                               asset_number = ar.asset_number,
                               asset_name = ar.asset_name,
                               asset_serial_number = ar.asset_serial_number,
                               vendor = a,
                               asset_receipt_date = ar.asset_receipt_date,
                               asset_description = ar.asset_description,
                               asset_type = b
                           }).ToList<subasset_registrationViewModel>();
            }
            else if (_qry.asset_type_id == (int)Enum_asset_type_Key.AssetChild)
            {
                //Parent Asset
                _qrysub = (from ar in db.tr_asset_registration
                           where (ar.fl_active == true && ar.deleted_date == null && ar.asset_id == id)

                           join p in db.tr_asset_registration on ar.asset_parent_id equals p.asset_id
                           where (p.fl_active == true && p.deleted_date == null)

                           join a in db.ms_vendor on ar.vendor_id equals a.vendor_id
                           where (a.fl_active == true && a.deleted_date == null)

                           join b in db.ms_asset_type on p.asset_type_id equals b.activa_type_id

                           select new subasset_registrationViewModel()
                           {
                               asset_id = p.asset_id,
                               asset_type_id = p.asset_type_id,
                               asset_number = p.asset_number,
                               asset_name = p.asset_name,
                               asset_serial_number = p.asset_serial_number,
                               vendor = a,
                               asset_receipt_date = p.asset_receipt_date,
                               asset_description = p.asset_description,
                               asset_type = b
                           }).ToList<subasset_registrationViewModel>();
            }

            string imreBase64Data = Convert.ToBase64String(_qry.tr_asset_images.asset_qrcode);
            string imgDataURL = string.Format("data:image/jpg;base64,{0}", imreBase64Data);
            //Passing image data in viewbag to view  
            ViewBag.ImageData = imgDataURL;

            var tuple = new Tuple<asset_registrationViewModel, List<subasset_registrationViewModel>, string>(_qry, _qrysub, asset_registrationViewModel.path_file_asset);

            return PartialView(tuple);
        }
    }
}