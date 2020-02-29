using System;
using System.IO;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ASM_UI.Models;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class asset_searchController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();
      
        // GET: asset_search
        public ActionResult Index(string strAssetNumber, string strAssetName, string strMerk, string TypeSelectLoc, string TypeSelect)
        {
            #region "for dropdown Location"
            var _Location = from s in db.ms_asset_location
                            where (s.fl_active == true && s.deleted_date == null)
                            select s;

            SelectList itemsTypeLoc = new SelectList(_Location, "location_code", "location_name", TypeSelectLoc);

            ViewBag.TypeSelectLoc = itemsTypeLoc;
            ViewBag.TypeSelectValue = TypeSelectLoc;
            #endregion

            #region "for dropdown Category"
            var _Category = from s in db.ms_asset_category
                            where (s.fl_active == true && s.deleted_date == null)
                            select s;

            SelectList itemsType = new SelectList(_Category, "category_code", "category_name", TypeSelect);

            ViewBag.TypeSelect = itemsType;
            ViewBag.TypeSelectValue = TypeSelect;
            #endregion

            return View();
        }

        /// <summary>
        /// list utk license index
        /// </summary>
        /// <returns>JSON</returns>
        public JsonResult GetAssetSearchList(string strAssetNumber, string strAssetName, string strMerk, string TypeSelectLoc, string TypeSelect)
        {
            db.Configuration.ProxyCreationEnabled = false;

            if (String.IsNullOrEmpty(strAssetNumber))
                strAssetNumber = string.Empty;

            if (String.IsNullOrEmpty(strAssetName))
                strAssetName = string.Empty;

            if (String.IsNullOrEmpty(strMerk))
                strMerk = string.Empty;

            if (String.IsNullOrEmpty(TypeSelectLoc) || TypeSelect == "ALL")
                TypeSelectLoc = string.Empty;

            if (String.IsNullOrEmpty(TypeSelect) || TypeSelect == "ALL")
                TypeSelect = string.Empty;

            var _qry = (from ar in db.tr_asset_registration
                        where (ar.fl_active == true && ar.deleted_date == null 
                        //&& ar.asset_type_id == (int)Enum_asset_type_Key.AssetParent
                        && ar.asset_number.Contains(strAssetNumber) 
                        && ar.asset_name.Contains(strAssetName) 
                        && ar.asset_merk.Contains(strMerk))
                        //&& ar.company_id == UserProfile.company_id 
                        //&& ar.department_id == UserProfile.department_id
                        join a in db.ms_vendor on ar.vendor_id equals a.vendor_id
                        where (a.fl_active == true && a.deleted_date == null)

                        join b in db.ms_asset_register_location on ar.asset_reg_location_id equals b.asset_reg_location_id
                        where (b.fl_active == true && b.deleted_date == null)

                        join c in db.ms_asset_register_pic on ar.asset_reg_pic_id equals c.asset_reg_pic_id
                        where (c.fl_active == true && c.deleted_date == null)

                        join d in db.ms_asset_type on ar.asset_type_id equals d.activa_type_id
                        where (d.fl_active == true && d.deleted_date == null)

                        join e in db.ms_asset_category on ar.category_id equals e.category_id
                        where (e.fl_active == true && e.deleted_date == null && e.category_code.Contains(TypeSelect))

                        join f in db.ms_asmin_company on ar.company_id equals f.company_id
                        where (f.fl_active == true && f.deleted_date == null)

                        join g in db.ms_department on ar.department_id equals g.department_id
                        where (g.fl_active == true && g.deleted_date == null)

                        join h in db.ms_employee on ar.employee_id equals h.employee_id
                        where (h.fl_active == true && h.deleted_date == null)

                        join i in db.ms_asset_location on ar.location_id equals i.location_id
                        where (i.fl_active == true && i.deleted_date == null && i.location_code.Contains(TypeSelectLoc))

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

            return Json(new { data = _qry }, JsonRequestBehavior.AllowGet);
        }

        // GET: asset_search/Details/11
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);

            var cmd = new SqlCommand("SEARCHASSETBYID", conn);
            cmd.CommandText = "Exec SP_SEARCH_ASSETBYID @asset_id";

            cmd.Parameters.AddWithValue("@asset_id", id);
            conn.Open();
            cmd.ExecuteNonQuery();

            SqlDataAdapter dataAdapt = new SqlDataAdapter();
            dataAdapt.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dataAdapt.Fill(dataTable);

            conn.Close();

            asset_registrationViewModel data = new asset_registrationViewModel();
            data.asset_id = Convert.ToInt32(dataTable.Rows[0]["asset_id"]);
            data.asset_number = dataTable.Rows[0]["asset_number"].ToString();
            data.asset_name = dataTable.Rows[0]["asset_name"].ToString();
            data.company_register = dataTable.Rows[0]["company_register"].ToString();
            data.asset_merk = dataTable.Rows[0]["asset_merk"].ToString();
            data.location_register = dataTable.Rows[0]["location_register"].ToString();
            data.asset_serial_number = dataTable.Rows[0]["asset_serial_number"].ToString();
            data.pic_register = dataTable.Rows[0]["pic_register"].ToString();
            data.asset_description = dataTable.Rows[0]["asset_description"].ToString();
            data.category_register = dataTable.Rows[0]["category_register"].ToString();
            data.asset_receipt_date = Convert.ToDateTime(dataTable.Rows[0]["asset_receipt_date"]);
            data.vendor_name = dataTable.Rows[0]["vendor_name"].ToString();
            data.location_asset = dataTable.Rows[0]["location_asset"].ToString();
            data.original_price = isnullnumber(dataTable.Rows[0]["original_price"]);
            data.department_asset = dataTable.Rows[0]["department_asset"].ToString();
            data.usage_life_time_fiskal = dataTable.Rows[0]["usage_life_time_fiskal"].ToString();
            data.employee_name = dataTable.Rows[0]["employee_name"].ToString();
            data.fis_book_value = isnullnumber(dataTable.Rows[0]["fis_book_value"]);
            return View(data);
        }

        private Decimal isnullnumber(object oData)
        {
            if (oData == DBNull.Value)
                return 0;
            else
                return Convert.ToDecimal(oData);
        }
    }
}