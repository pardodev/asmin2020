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
using System.Drawing;
using System.Drawing.Imaging;
using ASM_UI.App_Helpers;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class printController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();
        private string BASE_FOLDER = @"C:\ASM_PRINT\";

        public ActionResult Index(printLabelViewModel model)
        {
            sy_app_setting setting = app_setting.APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "PRINT_BASE_FOLDER").Single<sy_app_setting>();
            if (string.IsNullOrWhiteSpace(setting.app_value))
                BASE_FOLDER = @"C:\ASM_PRINT\";
            else
                BASE_FOLDER = setting.app_value.ToUpper();

            if (!Directory.Exists(BASE_FOLDER))
                Directory.CreateDirectory(BASE_FOLDER);


            //printLabelViewModel model = new printLabelViewModel()
            //{
            //    company_id = UserProfile.company_id,
            //    asset_reg_location_id = UserProfile.asset_reg_location_id
            //};

            model.company_id = UserProfile.company_id;
            model.company_list = db.ms_asmin_company.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asmin_company>();
            model.company = db.ms_asmin_company.Find(UserProfile.company_id);

            model.asset_reg_location_id = UserProfile.asset_reg_location_id;
            model.asset_reg_location = db.ms_asset_register_location.Find(UserProfile.asset_reg_location_id);
            model.asset_reg_location_list = db.ms_asset_register_location.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_register_location>();

            //Set Location List
            if (UserProfile.asset_reg_location_id == 1)
            {
                model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null))
                                             select m).ToList();
            }
            else  //branch
            {
                model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
                                             select m).ToList();
            }

            model.asset_category_list = db.ms_asset_category.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_category>();
            model.department_list = db.ms_department.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_department>();

            if (model.asset_location_id != null && model.asset_location_id > 0)
            {
                var _qry = (from ar in db.tr_asset_registration
                            where (ar.fl_active == true && ar.deleted_date == null && ar.asset_type_id == (int)Enum_asset_type_Key.AssetParent)
                            && ar.company_id == model.company_id
                            && ar.asset_reg_location_id == model.asset_reg_location_id

                            join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                            where (b.fl_active == true && b.deleted_date == null)

                            join e in db.ms_asset_category on ar.category_id equals e.category_id
                            where (e.fl_active == true && e.deleted_date == null && (e.category_id == model.category_id || model.category_id == null || model.category_id == 0))

                            join f in db.ms_asmin_company on ar.company_id equals f.company_id
                            where (f.fl_active == true && f.deleted_date == null)

                            join g in db.ms_department on ar.department_id equals g.department_id
                            where (g.fl_active == true && g.deleted_date == null && (g.department_id == model.department_id || model.department_id == null || model.department_id == 0))

                            join i in db.ms_asset_location on ar.location_id equals i.location_id
                            where (i.fl_active == true && i.deleted_date == null && i.location_id == model.asset_location_id)

                            select new Print_Asset_Items()
                            {
                                asset_id = ar.asset_id,
                                asset_number = ar.asset_number,
                                asset_name = ar.asset_name,
                                category_id = ar.category_id,
                                category_name = e.category_name,
                                department_id = ar.department_id,
                                department_name = g.department_name,

                                Value = ar.asset_id.ToString(),
                                Text = ar.asset_name,
                                Checked = false
                            }).ToList<Print_Asset_Items>();

                model.print_asset_items = _qry;
            }

            return View(model);

        }

        //public ActionResult PrintLabel()
        //{
        //    //db.Configuration.ProxyCreationEnabled = false;
        //    printLabelViewModel model = new printLabelViewModel()
        //    {
        //        company_id = UserProfile.company_id,
        //        asset_reg_location_id = UserProfile.asset_reg_location_id
        //    };

        //    model.company_list = db.ms_asmin_company.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asmin_company>();
        //    model.company = db.ms_asmin_company.Find(UserProfile.company_id);
        //    model.asset_reg_location = db.ms_asset_register_location.Find(UserProfile.asset_reg_location_id);
        //    model.asset_reg_location_list = db.ms_asset_register_location.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_register_location>();

        //    //Set Location List
        //    if (UserProfile.asset_reg_location_id == 1)
        //    {
        //        model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null))
        //                                     select m).ToList();
        //    }
        //    else  //branch
        //    {
        //        model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
        //                                     select m).ToList();
        //    }

        //    model.asset_category_list = db.ms_asset_category.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_category>();
        //    model.department_list = db.ms_department.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_department>();

        //    return View(model);
        //}

        [HttpPost]
        public JsonResult ProcessPrintSelectedData(string id)
        {
            //var print_choose = "Choose Asset ID : "+id;
            string str_message = ""; string[] checked_asset_id = null; string str_file_name = "";
            if (string.IsNullOrWhiteSpace(id))
                checked_asset_id = null; // new string[1] { "0" };
            else
                checked_asset_id = id.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                if (checked_asset_id == null || checked_asset_id.Length <= 0)
                    throw new Exception("None data selected.");

                if (checked_asset_id.Length > 0)
                {
                    if (!Directory.Exists(BASE_FOLDER))
                        Directory.CreateDirectory(BASE_FOLDER);

                    var _qry_list = (from ar in db.tr_asset_registration
                                     where (ar.fl_active == true && ar.deleted_date == null
                                     && checked_asset_id.Contains(ar.asset_id.ToString())
                                     && ar.company_id == UserProfile.company_id
                                     && ar.asset_reg_location_id == UserProfile.asset_reg_location_id)

                                     //join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                                     //where (b.fl_active == true && b.deleted_date == null)

                                     //join e in db.ms_asset_category on ar.category_id equals e.category_id
                                     //where (e.fl_active == true && e.deleted_date == null && (e.category_id == model.category_id || model.category_id == null || model.category_id == 0))

                                     //join f in db.ms_asmin_company on ar.company_id equals f.company_id
                                     //where (f.fl_active == true && f.deleted_date == null)

                                     //join g in db.ms_department on ar.department_id equals g.department_id
                                     //where (g.fl_active == true && g.deleted_date == null && (g.department_id == model.department_id || model.department_id == null || model.department_id == 0))

                                     join i in db.ms_asset_location on ar.location_id equals i.location_id
                                     where (i.fl_active == true && i.deleted_date == null)
                                     orderby i.location_code

                                     select new Print_Asset_Items()
                                     {
                                         asset_id = ar.asset_id,
                                         asset_number = ar.asset_number,
                                         asset_name = ar.asset_name,
                                         category_id = ar.category_id,
                                         //category_name = e.category_name,
                                         department_id = ar.department_id,
                                         //department_name = g.department_name,
                                         location_code = i.location_code,

                                         Value = ar.asset_id.ToString(),
                                         Text = ar.asset_name,
                                         Checked = false
                                     }).ToList<Print_Asset_Items>();

                    string txt_file_name = string.Format("ASSET_BARCODE_{0}.txt", 0);
                    string txt_folder = BASE_FOLDER;
                    string txt_full_name;
                    string tmp_location_code = "";
                    string[] arr_content = null;
                    int line_no = 0;
                    Array.Resize(ref arr_content, line_no + 1);
                    foreach (var _itm in _qry_list)
                    {
                        string curr_location_code = (_itm.location_code != null) ? _itm.location_code : "";
                        if (!tmp_location_code.Equals(curr_location_code.ToUpper()) && line_no > 0)
                        {
                            txt_folder = BASE_FOLDER;
                            txt_folder += tmp_location_code.ToUpper();
                            if (!Directory.Exists(txt_folder))
                                Directory.CreateDirectory(txt_folder);

                            txt_file_name = string.Format("ASSET_BARCODE_{0}.txt", tmp_location_code);
                            txt_full_name = txt_folder + @"\" + txt_file_name;
                            if (!System.IO.File.Exists(txt_full_name))
                                System.IO.File.Delete(txt_full_name);

                            if (arr_content.Length > 0)
                            {
                                System.IO.File.WriteAllLines(txt_full_name, arr_content);
                                str_file_name = txt_full_name;
                            }
                            line_no = 0;
                            arr_content = null;
                        }
                        Array.Resize(ref arr_content, line_no + 1);
                        arr_content[line_no] = string.Format("{0},{1}", _itm.asset_number, _itm.asset_name.ToUpper());
                        line_no += 1;
                        tmp_location_code = curr_location_code.ToUpper();
                    }

                    if (arr_content.Length > 0 && _qry_list.Count > 0)
                    {
                        txt_folder = BASE_FOLDER;
                        txt_folder += @"\" + tmp_location_code.ToUpper();
                        if (!Directory.Exists(txt_folder))
                            Directory.CreateDirectory(txt_folder);

                        txt_file_name = string.Format("ASSET_BARCODE_{0}.txt", tmp_location_code);
                        txt_full_name = txt_folder + @"\" + txt_file_name;
                        if (!System.IO.File.Exists(txt_full_name))
                            System.IO.File.Delete(txt_full_name);

                        System.IO.File.WriteAllLines(txt_full_name, arr_content);
                        str_file_name = txt_full_name;
                    }
                }
                else //kosong
                    str_message = "no one asset sent to printer.";
            }
            catch (Exception _ex)
            {
                str_message = "Failure to create text file. " + _ex.Message;
            }
            finally
            {
                str_message = checked_asset_id.Length.ToString() + " numbers successfully sent to printer" 
                    + Environment.NewLine + str_file_name;
            }
            return Json(str_message);
        }

        [HttpGet]
        /// <summary>
        /// list utk license index
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult GetAssetSearchList(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString, int? asset_location_id = 0, int? category_id = 0, int? department_id = 0)
        {
            db.Configuration.ProxyCreationEnabled = false;

            #region MODEL
            printLabelViewModel model = new printLabelViewModel()
            {
                company_id = UserProfile.company_id,
                asset_reg_location_id = UserProfile.asset_reg_location_id,
                asset_location_id = asset_location_id,
                category_id = category_id,
                department_id = department_id
            };

            model.company_id = UserProfile.company_id;
            model.company_list = db.ms_asmin_company.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asmin_company>();
            model.company = db.ms_asmin_company.Find(UserProfile.company_id);

            model.asset_reg_location_id = UserProfile.asset_reg_location_id;
            model.asset_reg_location = db.ms_asset_register_location.Find(UserProfile.asset_reg_location_id);
            model.asset_reg_location_list = db.ms_asset_register_location.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_register_location>();

            //Set Location List
            if (UserProfile.asset_reg_location_id == 1)
            {
                model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null))
                                             select m).ToList();
            }
            else  //branch
            {
                model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
                                             select m).ToList();
            }

            model.asset_category_list = db.ms_asset_category.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_category>();
            model.department_list = db.ms_department.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_department>();
            #endregion

            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var _qry = (from ar in db.tr_asset_registration
                        where (ar.fl_active == true && ar.deleted_date == null && ar.asset_type_id == (int)Enum_asset_type_Key.AssetParent)
                        && ar.company_id == model.company_id
                        && ar.asset_reg_location_id == model.asset_reg_location_id

                        join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join e in db.ms_asset_category on ar.category_id equals e.category_id
                        where (e.fl_active == true && e.deleted_date == null && (e.category_id == model.category_id || model.category_id == 0 || model.category_id == null))

                        join f in db.ms_asmin_company on ar.company_id equals f.company_id
                        where (f.fl_active == true && f.deleted_date == null)

                        join g in db.ms_department on ar.department_id equals g.department_id
                        where (g.fl_active == true && g.deleted_date == null && (g.department_id == model.department_id || model.department_id == 0 || model.department_id == null))

                        join i in db.ms_asset_location on ar.location_id equals i.location_id
                        where (i.fl_active == true && i.deleted_date == null && (i.location_id == model.asset_location_id))

                        select new Print_Asset_Items()
                        {
                            asset_id = ar.asset_id,
                            asset_number = ar.asset_number,
                            asset_name = ar.asset_name,
                            category_id = ar.category_id,
                            category_name = e.category_name,
                            department_id = ar.department_id,
                            department_name = g.department_name,

                            Value = ar.asset_id.ToString(),
                            Text = ar.asset_name,
                            Checked = false
                        });

            //model.print_asset_items = _qry;

            if (_search)
            {
                switch (searchField)
                {
                    case "asset_number":
                        _qry = _qry.Where(t => t.asset_number.Contains(searchString));
                        break;
                    case "asset_name":
                        _qry = _qry.Where(t => t.asset_name.Contains(searchString));
                        break;
                    case "department_name":
                        _qry = _qry.Where(t => t.department_name.Contains(searchString));
                        break;
                    case "employee_name":
                        _qry = _qry.Where(t => t.category_name.Contains(searchString));
                        break;
                }
            }

            //calc paging
            int totalRecords = _qry.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            if (sord.ToUpper() == "DESC")
            {
                _qry = _qry.OrderByDescending(t => t.asset_name);
                _qry = _qry.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                _qry = _qry.OrderBy(t => t.asset_name);
                _qry = _qry.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _qry
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /*
                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult PrintToTextFile(printLabelViewModel model)
                {
                    model.company_id = UserProfile.company_id;
                    model.company_list = db.ms_asmin_company.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asmin_company>();
                    model.company = db.ms_asmin_company.Find(UserProfile.company_id);

                    model.asset_reg_location_id = UserProfile.asset_reg_location_id;
                    model.asset_reg_location = db.ms_asset_register_location.Find(UserProfile.asset_reg_location_id);
                    model.asset_reg_location_list = db.ms_asset_register_location.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_register_location>();

                    //Set Location List
                    if (UserProfile.asset_reg_location_id == 1)
                    {
                        model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null))
                                                     select m).ToList();
                    }
                    else  //branch
                    {
                        model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
                                                     select m).ToList();
                    }
                    if (model.asset_location_id == null)
                        model.asset_location_id = Convert.ToInt32(Request["asset_location_id"]);

                    if (model.department_id == null)
                        model.department_id = Convert.ToInt32(Request["department_id"]);

                    if (model.category_id == null)
                        model.category_id = Convert.ToInt32(Request["category_id"]);

                    model.asset_category_list = db.ms_asset_category.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_category>();
                    model.department_list = db.ms_department.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_department>();

                    string _ids = Request.Form["checked_asset_id"].ToString().Replace("\\", "").Replace("/", "");
                    if (string.IsNullOrWhiteSpace(_ids))
                        model.checked_asset_id = new string[1] { "0" };
                    else
                        model.checked_asset_id = _ids.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        if (model.checked_asset_id == null || model.checked_asset_id.Length <= 0)
                            throw new Exception("None asset selected.");

                        if (ModelState.IsValid)
                        {
                            if (!Directory.Exists(BASE_FOLDER))
                                Directory.CreateDirectory(BASE_FOLDER);

                            var _qry_list = (from ar in db.tr_asset_registration
                                             where (ar.fl_active == true && ar.deleted_date == null
                                             && model.checked_asset_id.Contains(ar.asset_id.ToString())
                                             && ar.company_id == model.company_id
                                             && ar.asset_reg_location_id == model.asset_reg_location_id)

                                             //join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                                             //where (b.fl_active == true && b.deleted_date == null)

                                             //join e in db.ms_asset_category on ar.category_id equals e.category_id
                                             //where (e.fl_active == true && e.deleted_date == null && (e.category_id == model.category_id || model.category_id == null || model.category_id == 0))

                                             //join f in db.ms_asmin_company on ar.company_id equals f.company_id
                                             //where (f.fl_active == true && f.deleted_date == null)

                                             //join g in db.ms_department on ar.department_id equals g.department_id
                                             //where (g.fl_active == true && g.deleted_date == null && (g.department_id == model.department_id || model.department_id == null || model.department_id == 0))

                                             join i in db.ms_asset_location on ar.location_id equals i.location_id
                                             where (i.fl_active == true && i.deleted_date == null)
                                             orderby i.location_code

                                             select new Print_Asset_Items()
                                             {
                                                 asset_id = ar.asset_id,
                                                 asset_number = ar.asset_number,
                                                 asset_name = ar.asset_name,
                                                 category_id = ar.category_id,
                                                 //category_name = e.category_name,
                                                 department_id = ar.department_id,
                                                 //department_name = g.department_name,
                                                 location_code = i.location_code,

                                                 Value = ar.asset_id.ToString(),
                                                 Text = ar.asset_name,
                                                 Checked = false
                                             }).ToList<Print_Asset_Items>();

                            string txt_file_name = string.Format("ASSET_BARCODE_{0}.txt", 0);
                            string txt_folder = BASE_FOLDER;
                            string txt_full_name;
                            string tmp_location_code = "";
                            string[] arr_content = null;
                            int line_no = 0;
                            Array.Resize(ref arr_content, line_no + 1);
                            foreach (var _itm in _qry_list)
                            {
                                string curr_location_code = (_itm.location_code != null) ? _itm.location_code : "";
                                if (!tmp_location_code.Equals(curr_location_code.ToUpper()) && line_no > 0)
                                {
                                    txt_folder = BASE_FOLDER;
                                    txt_folder += @"\" + tmp_location_code.ToUpper();
                                    if (!Directory.Exists(txt_folder))
                                        Directory.CreateDirectory(txt_folder);

                                    txt_file_name = string.Format("ASSET_BARCODE_{0}.txt", tmp_location_code);
                                    txt_full_name = txt_folder + @"\" + txt_file_name;
                                    if (!System.IO.File.Exists(txt_full_name))
                                        System.IO.File.Delete(txt_full_name);

                                    if (arr_content.Length > 0)
                                        System.IO.File.WriteAllLines(txt_full_name, arr_content);

                                    line_no = 0;
                                    arr_content = null;
                                }
                                Array.Resize(ref arr_content, line_no + 1);
                                arr_content[line_no] = string.Format("{0},{1}", _itm.asset_number, _itm.asset_name);
                                line_no += 1;
                                tmp_location_code = curr_location_code.ToUpper();
                            }

                            if (arr_content.Length > 0 && _qry_list.Count > 0)
                            {
                                txt_folder = BASE_FOLDER;
                                txt_folder += @"\" + tmp_location_code.ToUpper();
                                if (!Directory.Exists(txt_folder))
                                    Directory.CreateDirectory(txt_folder);

                                txt_file_name = string.Format("ASSET_BARCODE_{0}.txt", tmp_location_code);
                                txt_full_name = txt_folder + @"\" + txt_file_name;
                                if (!System.IO.File.Exists(txt_full_name))
                                    System.IO.File.Delete(txt_full_name);
                                System.IO.File.WriteAllLines(txt_full_name, arr_content);

                            }
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception _ex)
                    {
                        model.print_message = "Failure to create text file. " + _ex.Message;

                    }
                    return View(model);
                }
         */

    }
}