using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Microsoft.Reporting.WebForms;
using System.Web.Mvc;
using ASM_UI.Models;
using ASM_UI.Rdlc;
using ASM_UI.Rdlc.DataSet;

namespace ASM_UI.Controllers
{
    /// <summary>
    /// controller semua report
    /// </summary>
    [Authorize]
    public class reportController : BaseController
    {
        private int DB_TIMEOUT = 1800;
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: report
        public ActionResult Index()
        {

            //var _rptList = from t in db.tr_asset_registration
            //               where t.fl_active == true && t.asset_type_id == 1 && t.deleted_date != null
            //               select new
            //               {
            //                   t.asset_number,
            //                   t.asset_name,
            //                   year = t.asset_receipt_date.HasValue ? t.asset_receipt_date.Value.Year : DateTime.Now.Year,
            //                   location_name = t.ms_asset_location.location_name
            //               };

            return View();
        }

        #region Report AssetByLocation
        public ActionResult AssetByLocation()
        {
            Report_AssetByLocationViewModel model = new Report_AssetByLocationViewModel()
            {
                ms_company_list = new List<ms_asmin_company>(),
                asset_location_list = new List<ms_asset_location>(),
                id = 11,
                report_title = "Laporan Fix Asset",
                report_rdlc = "rpt01_AssetByLocation.rdlc",
                company = UserProfile.company_id,
                asset_register = UserProfile.asset_reg_location_id,
                enabled_distric = true,
                enabled_company = true,
                display_format = EnumDisplayFormatKey.WEB
            };
            try
            {
                //report AssetByLocation ini saya kasi no 11

                model.company = UserProfile.company_id;
                model.ms_company = db.ms_asmin_company.Find(UserProfile.company_id);

                model.asset_register = UserProfile.asset_reg_location_id;
                model.ms_asset_register_location = UserProfile.register_location;

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
                //Set Financial Statement
                //value false for Commercial
                //value true for Fiscal
                string[] arr_finstm = new string[2] { "Commercial", "Fiscal" };
                List<SelectListItem> finstm_list = new List<SelectListItem>();
                for (int loop_int = 0; loop_int <= 1; loop_int++)
                {
                    finstm_list.Add(new SelectListItem
                    {
                        Text = arr_finstm[loop_int],
                        Value = (loop_int + 1).ToString()
                    });
                }
                model.fin_statement_list = finstm_list;
            }
            catch (Exception _ex) { ASM_UI.App_Helpers.app_logwriter.ToLog("" + _ex.Message); }
            return View(model);

        }

        [HttpPost, ActionName("AssetByLocResult")]
        [ValidateAntiForgeryToken]
        public ActionResult AssetByLocationDisplayed(Report_AssetByLocationViewModel model)
        {
            model.id = 11;
            model.report_title = "Laporan Fix Asset";

            if (model.flag_consolidation)
            {
                model.report_subtitle = "";
                model.report_rdlc = "rpt01_AssetByLocationConsolidation.rdlc";
            }
            else
            {
                model.report_subtitle = "Disctric";
                model.report_rdlc = "rpt01_AssetByLocation.rdlc";
            }
            model.company = UserProfile.company_id;
            model.asset_register = UserProfile.asset_reg_location_id;

            if (model.ms_company == null)
                model.ms_company = db.ms_asmin_company.Find(UserProfile.company_id);

            if (model.ms_asset_register_location == null)
                model.ms_asset_register_location = UserProfile.register_location;

            if (model.asset_location == null)
                model.asset_location = db.ms_asset_location.Find(model.location_id);


            if (ModelState.IsValid && model.id == 11)
            {
                ds01_AssetByLocaation _ds = new ds01_AssetByLocaation();
                string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
                try
                {
                    var o_conn = new SqlConnection(constring);
                    o_conn.Open();

                    var o_cmd = new SqlCommand();
                    o_cmd.Connection = o_conn;
                    o_cmd.CommandText = "SP_REPORT_AssetByLocation @company_id, @location_register_id, @location_id, @ReportType, @DateStart, @DateEnd";
                    o_cmd.Parameters.AddWithValue("@company_id", model.company);
                    o_cmd.Parameters.AddWithValue("@location_register_id", model.asset_register);

                    if (model.flag_consolidation)
                        o_cmd.Parameters.AddWithValue("@location_id", 0);
                    else
                        o_cmd.Parameters.AddWithValue("@location_id", model.location_id);

                    o_cmd.Parameters.AddWithValue("@ReportType", model.fin_statement_id);
                    o_cmd.Parameters.AddWithValue("@DateStart", model.start_date);
                    o_cmd.Parameters.AddWithValue("@DateEnd", model.end_date);

                    o_cmd.CommandTimeout = DB_TIMEOUT;
                    SqlDataAdapter o_da = new SqlDataAdapter(o_cmd);
                    o_da.Fill(_ds, "TAssetByLocation");
                    o_cmd.Dispose();
                    o_conn.Close();

                    if (_ds.TAssetByLocation.Count > 0)
                    {
                        ds01_AssetByLocaation.TReportInfoRow _row = _ds.TReportInfo.NewTReportInfoRow();
                        _row.report_name = model.report_title;
                        _row.report_title = model.report_title;
                        if (model.flag_consolidation)
                        {
                            _row.report_subtitle = "";
                            _row.report_title_line1 = "";
                        }
                        else
                        {
                            _row.report_subtitle = string.Format("Distric : {0}", model.ms_asset_register_location.asset_reg_location_name.ToUpper());
                            _row.report_title_line1 = string.Format("Lokasi : {0}", model.asset_location.location_name.ToUpper());
                        }

                        _row.report_title_line2 = "Periode - Closing Date";
                        _row.report_title_line3 = string.Format("{0:dd-MMM-yyyy} - {1:dd-MMM-yyyy}", model.start_date, model.end_date);

                        string book_type = "";
                        book_type = (model.fin_statement_id == 1) ? "Commercial" : "Fiscal";
                        _row.report_title_line4 = "Financial Statement : " + book_type;
                        _ds.TReportInfo.AddTReportInfoRow(_row);

                        ds01_AssetByLocaation.TReportParameterRow _rowparam = _ds.TReportParameter.NewTReportParameterRow();
                        //_rowparam.p_category = model.ms_asset_category.category_name;
                        _rowparam.p_company = model.ms_company.company_name;
                        _rowparam.p_location = model.ms_asset_register_location.asset_reg_location_name;
                        //_rowparam.p_year = model.pyear.ToString();
                        //_rowparam.p_month = arr_month[model.pmonth].ToString();

                        _ds.TReportParameter.AddTReportParameterRow(_rowparam);

                        ReportDataSource dsMain = new ReportDataSource("dsMain", (DataTable)_ds.TAssetByLocation);
                        ReportDataSource dsReportInfo = new ReportDataSource("dsReportInfo", (DataTable)_ds.TReportInfo);
                        ReportDataSource dsParameter = new ReportDataSource("dsParameter", (DataTable)_ds.TReportParameter);

                        var reportViewer = new ReportViewer();
                        reportViewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/" + model.report_rdlc);
                        reportViewer.LocalReport.DataSources.Clear();

                        reportViewer.LocalReport.DataSources.Add(dsMain);
                        reportViewer.LocalReport.DataSources.Add(dsReportInfo);
                        reportViewer.LocalReport.DataSources.Add(dsParameter);
                        reportViewer.LocalReport.Refresh();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.AsyncRendering = false;
                        reportViewer.SizeToReportContent = true;
                        reportViewer.ZoomMode = ZoomMode.FullPage;

                        //ViewBag.ReportViewer = reportViewer;
                        TempData["reportviewer"] = reportViewer;
                        return RedirectToAction("ReportDisplay");
                    }

                }
                catch (Exception _ex)
                {
                    ASM_UI.App_Helpers.app_logwriter.ToLog("" + _ex.Message);
                    TempData["error"] = _ex.Message;
                }
            }
            return View(model);
        }

        //[HttpPost]
        //[Route("report/assetbylocation/pdf/{id}")]
        //[ValidateAntiForgeryToken]
        //public ActionResult AssetByLocationPDF(Report_AssetByLocationViewModel model)
        //{

        //    ds01_AssetByLocaation _ds = new ds01_AssetByLocaation();
        //    string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
        //    var o_conn = new SqlConnection(constring);
        //    o_conn.Open();

        //    var o_cmd = new SqlCommand();
        //    o_cmd.Connection = o_conn;
        //    o_cmd.CommandText = "SP_REPORT_AssetByLocation @location_id, @ReportType, @AsOfDate";
        //    o_cmd.Parameters.AddWithValue("@location_id", 1);
        //    o_cmd.Parameters.AddWithValue("@ReportType", 1);
        //    o_cmd.Parameters.AddWithValue("@AsOfDate", model.end_date);
        //    o_cmd.CommandTimeout = DB_TIMEOUT;
        //    SqlDataAdapter o_da = new SqlDataAdapter(o_cmd);
        //    o_da.Fill(_ds, "TAssetByLocation");
        //    o_cmd.Dispose();
        //    o_conn.Close();

        //    Warning[] warnings;
        //    string mimeType;
        //    string[] streamids;
        //    string encoding;
        //    string filenameExtension;

        //    var viewer = new ReportViewer();
        //    viewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/" + model.report_rdlc);
        //    viewer.LocalReport.DataSources.Clear();
        //    ReportDataSource o_rds = new ReportDataSource("dsMain", (DataTable)_ds.TAssetByLocation);
        //    viewer.LocalReport.DataSources.Add(o_rds);
        //    viewer.LocalReport.Refresh();

        //    var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

        //    return new FileContentResult(bytes, mimeType);
        //    //return File(bytes, mimeType, locationId.ToString() + "_AssetByLocation.pdf");

        //}


        #endregion

        //public ActionResult GetAssetByLocationPDF(int locationId = 0)
        //{
        //    dsAssetByLocation _ds = new dsAssetByLocation();
        //    string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
        //    var o_conn = new SqlConnection(constring);
        //    o_conn.Open();

        //    var o_cmd = new SqlCommand();
        //    o_cmd.Connection = o_conn;
        //    o_cmd.CommandText = "SP_REPORT_AssetByLocation @location_id";
        //    o_cmd.Parameters.AddWithValue("@location_id", locationId);
        //    o_cmd.CommandTimeout = DB_TIMEOUT;
        //    SqlDataAdapter o_da = new SqlDataAdapter(o_cmd);
        //    o_da.Fill(_ds, "TAssetByLocation");
        //    o_cmd.Dispose();
        //    o_conn.Close();

        //    Warning[] warnings;
        //    string mimeType;
        //    string[] streamids;
        //    string encoding;
        //    string filenameExtension;

        //    var viewer = new ReportViewer();
        //    viewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/AssetByLocation.rdlc");
        //    viewer.LocalReport.DataSources.Clear();
        //    ReportDataSource o_rds = new ReportDataSource("dsMain", (DataTable)_ds.TAssetByLocation);
        //    viewer.LocalReport.DataSources.Add(o_rds);
        //    viewer.LocalReport.Refresh();

        //    var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

        //    return new FileContentResult(bytes, mimeType);
        //    //return File(bytes, mimeType, locationId.ToString() + "_AssetByLocation.pdf");
        //}


        #region reports 
        //public ActionResult AssetListing()
        //{
        //    Report_AssetListingViewModel rpt01 = new Report_AssetListingViewModel();

        //    //rpt01.period_year = (int)year;
        //    for (int loop_int = 2019; loop_int <= DateTime.Now.Year + 2; loop_int++)
        //    {
        //        rpt01.period_year_list.Add(loop_int);
        //    }

        //    //rpt01.period_month = (int)month;

        //    //rpt01.company_id = 0;
        //    rpt01.company = db.ms_asmin_company.Find(rpt01.company_id);
        //    rpt01.company_list = db.ms_asmin_company.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asmin_company>();


        //    //rpt01.location_id = 0;
        //    rpt01.asset_location = db.ms_asset_location.Find(rpt01.location_id);
        //    rpt01.asset_location_list = db.ms_asset_location.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_location>();

        //    //rpt01.asset_reg_location_id = 0;
        //    rpt01.asset_reg_location = db.ms_asset_register_location.Find(rpt01.asset_reg_location_id);
        //    rpt01.asset_reg_location_list = db.ms_asset_register_location.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_register_location>();


        //    //rpt01.category_id = 0;
        //    rpt01.asset_category = db.ms_asset_category.Find(rpt01.company_id);
        //    rpt01.asset_category_list = db.ms_asset_category.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_category>();

        //    return View(rpt01);
        //}
        /// <summary>
        /// RPT01 = ASSET LISTING
        /// </summary>
        /// <param name="company"></param>
        /// <param name="location"></param>
        /// <param name="register"></param>
        /// <param name="category"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        // GET : report/AssetListing/1
        public ActionResult AssetListing(Report_AssetListingViewModel rpt01)
        {
            #region parameter values
            rpt01.ms_company = db.ms_asmin_company.Find(rpt01.company);
            rpt01.ms_company_list = db.ms_asmin_company.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asmin_company>();

            rpt01.ms_asset_register_location = db.ms_asset_register_location.Find(rpt01.asset_register);
            rpt01.ms_asset_register_location_list = db.ms_asset_register_location.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_register_location>();

            rpt01.ms_asset_category = db.ms_asset_category.Find(rpt01.category);
            rpt01.ms_category_list = db.ms_asset_category.Where(c => c.fl_active == true && c.deleted_date == null).ToList<ms_asset_category>();

            List<SelectListItem> year_list = new List<SelectListItem>();
            for (int loop_int = 2019; loop_int <= DateTime.Now.Year + 2; loop_int++)
            {
                year_list.Add(new SelectListItem
                {
                    Text = loop_int.ToString(),
                    Value = loop_int.ToString()
                });
            }
            rpt01.period_year_list = year_list;

            string[] arr_month = new string[13] { "[ All ]", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<SelectListItem> month_list = new List<SelectListItem>();
            for (int loop_int = 0; loop_int <= 12; loop_int++)
            {
                month_list.Add(new SelectListItem
                {
                    Text = arr_month[loop_int],
                    Value = loop_int.ToString()
                });
            }
            rpt01.period_month_list = month_list;
            #endregion

            if (rpt01.id == null)
            {
                //default value
                ModelState.Clear();
                rpt01.id = 1;
                rpt01.pmonth = DateTime.Now.Month;
                rpt01.pyear = DateTime.Now.Year;
                rpt01.company = UserProfile.OrgId;
                rpt01.asset_register = UserProfile.asset_reg_location_id;
                rpt01.report_title = "Asset Liting";
                rpt01.report_rdlc = "rpt01_AssetListing.rdlc";
                ViewBag.ReportViewer = null;
                return View(rpt01);
            }

            if (ModelState.IsValid && rpt01.id == 1)
            {
                ds01_AssetListing _ds = new ds01_AssetListing();
                string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
                try
                {
                    var o_conn = new SqlConnection(constring);
                    o_conn.Open();

                    var o_cmd = new SqlCommand();
                    o_cmd.Connection = o_conn;
                    o_cmd.CommandText = "SP_REPORT01_AssetListing @company_id, @location_register_id, @category_id, @period_year, @period_month";
                    o_cmd.Parameters.AddWithValue("@company_id", rpt01.company);
                    o_cmd.Parameters.AddWithValue("@location_register_id", rpt01.asset_register);
                    o_cmd.Parameters.AddWithValue("@category_id", rpt01.category);
                    o_cmd.Parameters.AddWithValue("@period_year", rpt01.pyear);
                    o_cmd.Parameters.AddWithValue("@period_month", rpt01.pmonth);
                    o_cmd.CommandTimeout = DB_TIMEOUT;
                    SqlDataAdapter o_da = new SqlDataAdapter(o_cmd);
                    o_da.Fill(_ds, "TAssetList");
                    o_cmd.Dispose();
                    o_conn.Close();

                    if (_ds.TAssetList.Count > 0)
                    {
                        ds01_AssetListing.TReportInfoRow _row = _ds.TReportInfo.NewTReportInfoRow();
                        _row.report_name = rpt01.report_title;
                        _row.report_title = rpt01.report_title;
                        _row.report_subtitle = "";
                        _ds.TReportInfo.AddTReportInfoRow(_row);

                        ds01_AssetListing.TReportParameterRow _rowparam = _ds.TReportParameter.NewTReportParameterRow();
                        _rowparam.p_category = rpt01.ms_asset_category.category_name;
                        _rowparam.p_company = rpt01.ms_company.company_name;
                        _rowparam.p_location = rpt01.ms_asset_register_location.asset_reg_location_name;
                        _rowparam.p_year = rpt01.pyear.ToString();
                        _rowparam.p_month = arr_month[rpt01.pmonth].ToString();
                        _ds.TReportParameter.AddTReportParameterRow(_rowparam);

                        ReportDataSource dsMain = new ReportDataSource("dsMain", (DataTable)_ds.TAssetList);
                        ReportDataSource dsReportInfo = new ReportDataSource("dsReportInfo", (DataTable)_ds.TReportInfo);
                        ReportDataSource dsParameter = new ReportDataSource("dsParameter", (DataTable)_ds.TReportParameter);

                        var reportViewer = new ReportViewer();
                        reportViewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/" + rpt01.report_rdlc);
                        reportViewer.LocalReport.DataSources.Clear();

                        reportViewer.LocalReport.DataSources.Add(dsMain);
                        reportViewer.LocalReport.DataSources.Add(dsReportInfo);
                        reportViewer.LocalReport.DataSources.Add(dsParameter);
                        reportViewer.LocalReport.Refresh();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.AsyncRendering = false;
                        reportViewer.SizeToReportContent = true;
                        reportViewer.ZoomMode = ZoomMode.FullPage;

                        ViewBag.ReportViewer = reportViewer;
                    }

                }
                catch (Exception _ex)
                {
                    ASM_UI.App_Helpers.app_logwriter.ToLog("Report [" + rpt01.report_title + "] Err:" + _ex.Message);
                    throw new Exception("Report [" + rpt01.report_title + "] Err:" + _ex.Message);
                }
            }
            return View(rpt01);
        }

        // GET : report/AssetTaking/1
        public ActionResult AssetTaking(Report_AssetTakingViewModel model)
        {
            if (UserProfile.UserId == 0)
                ModelState.AddModelError("error_message", "User Profile is invalid, Please re-login.");
            else
            {
                model.company_name = UserProfile.CompanyName;
                model.location_reg_name = UserProfile.register_location.asset_reg_location_name;

                //Set Location List
                if (UserProfile.asset_reg_location_id == 1)
                {
                    model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null))
                                                 select m).ToList();
                }
                else
                {
                    model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
                                                 select m).ToList();
                }
            }

            //Set Financial Statement
            //value 1 for Commercial
            //value 2 for Fiscal
            string[] arr_finstm = new string[2] { "Commercial", "Fiscal" };
            List<SelectListItem> finstm_list = new List<SelectListItem>();
            for (int loop_int = 0; loop_int <= 1; loop_int++)
            {
                finstm_list.Add(new SelectListItem
                {
                    Text = arr_finstm[loop_int],
                    Value = (loop_int == 1).ToString()
                });
            }
            model.fin_statement_list = finstm_list;

            //default value pertama kali dibuka
            if (model.report_id == null)
            {
                ModelState.Clear();
                model.report_id = 2;
                return View(model);
            }

            //Validasi tambahan
            if (model.start_date > DateTime.Today)
                ModelState.AddModelError("start_date", "Please enter a valid date.");

            if (model.start_date > model.end_date)
                ModelState.AddModelError("end_date", "End date must be after start date.");

            if (model.fl_consolidated == false)
            {
                if (model.location_id == null)
                    ModelState.AddModelError("location_id", "Please select location.");
            }
            else
                model.location_id = 1; //hanya untuk isi parameter report

            if (ModelState.IsValid)
            {
                string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
                try
                {
                    var o_conn = new SqlConnection(constring);
                    o_conn.Open();

                    var o_cmd = new SqlCommand();
                    o_cmd.Connection = o_conn;


                    ds02_AssetTaking _ds = new ds02_AssetTaking();
                    _ds.EnforceConstraints = false;
                    o_cmd.CommandText = "SP_REPORT02_AssetTaking @StarDate, @EndDate, @Company_id, @Location_id, @FinStatement, @isConsolidation";
                    o_cmd.Parameters.AddWithValue("@StarDate", model.start_date);
                    o_cmd.Parameters.AddWithValue("@EndDate", model.end_date);
                    o_cmd.Parameters.AddWithValue("@Company_id", UserProfile.company_id);
                    o_cmd.Parameters.AddWithValue("@Location_id", model.location_id);
                    o_cmd.Parameters.AddWithValue("@FinStatement", model.fin_statement_id);
                    o_cmd.Parameters.AddWithValue("@isConsolidation", model.fin_statement_id);
                    o_cmd.CommandTimeout = DB_TIMEOUT;

                    SqlDataAdapter o_da = new SqlDataAdapter(o_cmd);
                    o_da.Fill(_ds, "SP_REPORT02_AssetTaking");
                    o_cmd.Dispose();
                    o_conn.Close();

                    if (_ds.Tables.Count > 0)
                    {
                        var reportViewer = new ReportViewer();

                        if (!model.fl_consolidated)
                            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/rpt02_AssetTaking.rdlc");
                        else if (model.fl_consolidated)
                            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/rpt02_AssetTakingConsolidated.rdlc");

                        List<ReportParameter> rptParams = new List<ReportParameter>();

                        ReportParameter PrmStartdate = new ReportParameter("paramStartDate");
                        PrmStartdate.Values.Add(Convert.ToDateTime(model.start_date).ToString("yyyy/MM/dd"));
                        rptParams.Add(PrmStartdate);

                        ReportParameter PrmEnddate = new ReportParameter("paramEndDate");
                        PrmEnddate.Values.Add(Convert.ToDateTime(model.end_date).ToString("yyyy/MM/dd"));
                        rptParams.Add(PrmEnddate);

                        reportViewer.LocalReport.SetParameters(rptParams);
                        reportViewer.LocalReport.DataSources.Clear();

                        ReportDataSource o_rds = new ReportDataSource("DataSetAssetTaking", (DataTable)_ds.SP_REPORT02_AssetTaking);
                        reportViewer.LocalReport.DataSources.Add(o_rds);
                        reportViewer.LocalReport.Refresh();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.AsyncRendering = false;
                        reportViewer.SizeToReportContent = true;
                        reportViewer.ZoomMode = ZoomMode.FullPage;


                        //ViewBag.ReportViewer = reportViewer;
                        TempData["reportviewer"] = reportViewer;
                        return RedirectToAction("ReportDisplay");
                    }
                }
                catch (Exception _ex)
                {
                    ASM_UI.App_Helpers.app_logwriter.ToLog("Report [Asset Taking] Err:" + _ex.Message);
                    TempData["error"] = _ex.Message;
                }
                finally
                { }
            }

            return RedirectToAction("ReportDisplay");
        }


        // GET : report/AssetValue/1
        public ActionResult AssetValue()
        {
            Report_AssetRPT03ViewModel rpt03 = new Report_AssetRPT03ViewModel();

            return View(rpt03);
        }


        // GET : report/AssetMutation/1
        //public ActionResult AssetMutation()
        public ActionResult AssetMutationList(Report_AssetRPT04AssetMutationViewModel model)
        {
            //Report_AssetRPT04ViewModel rpt04 = new Report_AssetRPT04ViewModel();

            //return View(rpt04);
            if (UserProfile.UserId == 0)
                ModelState.AddModelError("mutation_message", "User Profile is invalid, Please re-login.");
            else
            {
                model.company_name = UserProfile.CompanyName;
                model.location_reg_name = UserProfile.register_location.asset_reg_location_name;

                //Set Location List
                if (UserProfile.asset_reg_location_id == 1)
                {
                    model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null))
                                                 select m).ToList();
                }
                else
                {
                    model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
                                                 select m).ToList();
                }
            }

            //Set Financial Statement
            //value false for Commercial
            //value true for Fiscal
            //string[] arr_finstm = new string[2] { "Commercial", "Fiscal" };
            //List<SelectListItem> finstm_list = new List<SelectListItem>();
            //for (int loop_int = 0; loop_int <= 1; loop_int++)
            //{
            //    finstm_list.Add(new SelectListItem
            //    {
            //        Text = arr_finstm[loop_int],
            //        Value = (loop_int == 1).ToString()
            //    });
            //}
            //model.fin_statement_list = finstm_list;

            //default value pertama kali dibuka
            if (model.report_id == null)
            {
                ModelState.Clear();
                model.report_id = 4;
                return View(model);
            }

            //Validasi tambahan
            if (model.mutation_start_date > DateTime.Today)
                ModelState.AddModelError("mutation_start_date", "Please enter a valid date.");

            if (model.mutation_start_date > model.mutation_end_date)
                ModelState.AddModelError("mutation_end_date", "End date must be after start date.");

            if (model.fl_consolidated == false)
            {
                if (model.location_id == null)
                    ModelState.AddModelError("location_id", "Please select location.");
            }
            else
                model.location_id = 1; //hanya untuk isi parameter report

            if (ModelState.IsValid)
            {
                string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
                try
                {
                    var o_conn = new SqlConnection(constring);
                    o_conn.Open();

                    var o_cmd = new SqlCommand();
                    o_cmd.Connection = o_conn;


                    ds04_AssetMutation _ds = new ds04_AssetMutation();
                    o_cmd.CommandText = "SP_REPORT04_AssetMutation @district_id, @location_id, @fl_consolidated, @startDate, @endDate";
                    o_cmd.Parameters.AddWithValue("@district_id", UserProfile.company_id);
                    o_cmd.Parameters.AddWithValue("@location_id", model.location_id);
                    //o_cmd.Parameters.AddWithValue("@FinStatement", model.fin_statement_id);
                    o_cmd.Parameters.AddWithValue("@fl_consolidated", model.fl_consolidated);
                    o_cmd.Parameters.AddWithValue("@startDate", model.mutation_start_date);
                    o_cmd.Parameters.AddWithValue("@endDate", model.mutation_end_date);
                    o_cmd.CommandTimeout = DB_TIMEOUT;
                    SqlDataAdapter o_da = new SqlDataAdapter(o_cmd);
                    o_da.Fill(_ds, "SP_REPORT04_AssetMutation");
                    o_cmd.Dispose();
                    o_conn.Close();

                    if (_ds.Tables.Count > 0)
                    {
                        var reportViewer = new ReportViewer();

                        if (!model.fl_consolidated)
                            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/rpt04_AssetMutation.rdlc");
                        else if (model.fl_consolidated)
                            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/rpt04_AssetMutationConsolidation.rdlc");

                        List<ReportParameter> rptParams = new List<ReportParameter>();

                        ReportParameter PrmStartdate = new ReportParameter("paramStartDate");
                        PrmStartdate.Values.Add(Convert.ToDateTime(model.mutation_start_date).ToString("yyyy/MM/dd"));
                        rptParams.Add(PrmStartdate);

                        ReportParameter PrmEnddate = new ReportParameter("paramEndDate");
                        PrmEnddate.Values.Add(Convert.ToDateTime(model.mutation_end_date).ToString("yyyy/MM/dd"));
                        rptParams.Add(PrmEnddate);

                        reportViewer.LocalReport.SetParameters(rptParams);
                        reportViewer.LocalReport.DataSources.Clear();

                        ReportDataSource o_rds = new ReportDataSource("ds_Asset_Mutation", (DataTable)_ds.SP_REPORT04_AssetMutation);
                        reportViewer.LocalReport.DataSources.Add(o_rds);
                        reportViewer.LocalReport.Refresh();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.AsyncRendering = false;
                        reportViewer.SizeToReportContent = true;
                        reportViewer.ZoomMode = ZoomMode.FullPage;


                        //ViewBag.ReportViewer = reportViewer;
                        TempData["reportviewer"] = reportViewer;
                        return RedirectToAction("ReportDisplay");
                    }
                }
                catch (Exception _ex)
                {
                    ASM_UI.App_Helpers.app_logwriter.ToLog("Report [Mutation List] Err:" + _ex.Message);
                    TempData["error"] = _ex.Message;
                }
                finally
                { }
            }

            return RedirectToAction("ReportDisplay");
        }


        // GET : report/AssetDisposal/1
        public ActionResult AssetDisposal(Report_AssetDisposalViewModel model)
        {
            if (UserProfile.UserId == 0)
                ModelState.AddModelError("disposal_message", "User Profile is invalid, Please re-login.");
            else
            {
                model.company_name = UserProfile.CompanyName;
                model.location_reg_name = UserProfile.register_location.asset_reg_location_name;

                //Set Location List
                if (UserProfile.asset_reg_location_id == 1)
                {
                    model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null))
                                                 select m).ToList();
                }
                else
                {
                    model.asset_location_list = (from m in db.ms_asset_location.Where(m => (m.fl_active == true && m.deleted_date == null && m.asset_reg_location_id == UserProfile.asset_reg_location_id))
                                                 select m).ToList();
                }
            }

            //Set Financial Statement
            //value false for Commercial
            //value true for Fiscal
            string[] arr_finstm = new string[2] { "Commercial", "Fiscal" };
            List<SelectListItem> finstm_list = new List<SelectListItem>();
            for (int loop_int = 0; loop_int <= 1; loop_int++)
            {
                finstm_list.Add(new SelectListItem
                {
                    Text = arr_finstm[loop_int],
                    Value = (loop_int == 1).ToString()
                });
            }
            model.fin_statement_list = finstm_list;

            //default value pertama kali dibuka
            if (model.report_id == null)
            {
                ModelState.Clear();
                model.report_id = 5;
                return View(model);
            }

            //Validasi tambahan
            if (model.disposal_start_date > DateTime.Today)
                ModelState.AddModelError("disposal_start_date", "Please enter a valid date.");

            if (model.disposal_start_date > model.disposal_end_date)
                ModelState.AddModelError("disposal_end_date", "End date must be after start date.");

            if (model.fl_consolidated == false)
            {
                if (model.location_id == null)
                    ModelState.AddModelError("location_id", "Please select location.");
            }
            else
                model.location_id = 1; //hanya untuk isi parameter report

            if (ModelState.IsValid)
            {
                string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
                try
                {
                    var o_conn = new SqlConnection(constring);
                    o_conn.Open();

                    var o_cmd = new SqlCommand();
                    o_cmd.Connection = o_conn;


                    dsDisposalList _ds = new dsDisposalList();
                    _ds.EnforceConstraints = false;
                    o_cmd.CommandText = "SP_REPORT05_AssetDisposal @StarDate, @EndDate, @Company_id, @Location_id, @FinStatement, @isConsolidation";
                    o_cmd.Parameters.AddWithValue("@StarDate", model.disposal_start_date);
                    o_cmd.Parameters.AddWithValue("@EndDate", model.disposal_end_date);
                    o_cmd.Parameters.AddWithValue("@Company_id", UserProfile.company_id);
                    o_cmd.Parameters.AddWithValue("@Location_id", model.location_id);
                    o_cmd.Parameters.AddWithValue("@FinStatement", model.fin_statement_id);
                    o_cmd.Parameters.AddWithValue("@isConsolidation", model.fin_statement_id);
                    o_cmd.CommandTimeout = DB_TIMEOUT;
                    SqlDataAdapter o_da = new SqlDataAdapter(o_cmd);
                    o_da.Fill(_ds, "SP_REPORT05_AssetDisposal");
                    o_cmd.Dispose();
                    o_conn.Close();

                    if (_ds.Tables.Count > 0)
                    {
                        var reportViewer = new ReportViewer();

                        if (!model.fl_consolidated)
                            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/DisposalList.rdlc");
                        else if (model.fl_consolidated)
                            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Rdlc/DisposalConsolidation.rdlc");

                        List<ReportParameter> rptParams = new List<ReportParameter>();

                        ReportParameter PrmStartdate = new ReportParameter("paramStartDate");
                        PrmStartdate.Values.Add(Convert.ToDateTime(model.disposal_start_date).ToString("yyyy/MM/dd"));
                        rptParams.Add(PrmStartdate);

                        ReportParameter PrmEnddate = new ReportParameter("paramEndDate");
                        PrmEnddate.Values.Add(Convert.ToDateTime(model.disposal_end_date).ToString("yyyy/MM/dd"));
                        rptParams.Add(PrmEnddate);

                        reportViewer.LocalReport.SetParameters(rptParams);
                        reportViewer.LocalReport.DataSources.Clear();

                        ReportDataSource o_rds = new ReportDataSource("DataSetDisposalList", (DataTable)_ds.SP_REPORT05_AssetDisposal);
                        reportViewer.LocalReport.DataSources.Add(o_rds);
                        reportViewer.LocalReport.Refresh();

                        reportViewer.ProcessingMode = ProcessingMode.Local;
                        reportViewer.AsyncRendering = false;
                        reportViewer.SizeToReportContent = true;
                        reportViewer.ZoomMode = ZoomMode.FullPage;


                        //ViewBag.ReportViewer = reportViewer;
                        TempData["reportviewer"] = reportViewer;
                        return RedirectToAction("ReportDisplay");
                    }
                }
                catch (Exception _ex)
                {
                    ASM_UI.App_Helpers.app_logwriter.ToLog("Report [Disposal List] Err:" + _ex.Message);
                    TempData["error"] = _ex.Message;
                }
                finally
                { }
            }

            return RedirectToAction("ReportDisplay");
        }

        // GET : report/AssetDepreciate/1
        public ActionResult AssetDepreciate()
        {
            Report_AssetRPT06ViewModel rpt06 = new Report_AssetRPT06ViewModel();

            return View(rpt06);
        }

        public ActionResult ReportDisplay()
        {
            if (TempData["reportviewer"] != null)
                ViewBag.ReportViewer = TempData["reportviewer"] as ReportViewer;

            if (TempData["error"] != null)
                ViewBag.ErrorMessage = TempData["error"] as string;

            return PartialView();
        }
        #endregion


    }
}