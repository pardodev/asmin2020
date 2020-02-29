using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace ASM_UI.Controllers
{
    [Authorize]
    public class depreciationController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        // GET: depreciation
        #region ActionResultView

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Set()
        {
            return View();
        }

        public ActionResult Change()
        {
            return View();
        }

        public ActionResult ModalAsset()
        {
            return PartialView();
        }

        public ActionResult ModalApproval()
        {
            return PartialView();
        }

        public ActionResult ModalDepreciation()
        {
            return PartialView();
        }

        #endregion

        #region Set Dropdown Value
        public JsonResult GetDepreciationMethod()
        {

            var CountryList = db.ms_depreciation_type.Where(t => t.deleted_date == null).Select(
                t => new
                {
                    t.depreciation_type_id,
                    t.depreciation_type_name,
                }).ToList();
            return Json(CountryList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssetList(string id)
        {
            int AssetID = int.Parse(id);
            if (AssetID == 0)
            {
                List<VendorAssetViewModel> VendorAssetList = new List<VendorAssetViewModel>();

                string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
                var conn = new SqlConnection(constring);

                var cmd2 = new SqlCommand("GetData", conn);
                cmd2.CommandText = "EXEC SP_GET_ALL_ASSET_TO_PROCESS @company_id";
                cmd2.Parameters.AddWithValue("@company_id", UserProfile.company_id);
                conn.Open();
                using (SqlDataReader dr = cmd2.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(dr);

                    foreach (DataRow row in tb.Rows)
                    {
                        VendorAssetViewModel vn = new VendorAssetViewModel();

                        vn.asset_id = int.Parse(row["asset_id"].ToString());
                        vn.asset_number = row["asset_number"].ToString();
                        vn.asset_name = row["asset_name"].ToString();
                        vn.asset_receipt_date = DateTime.Parse(row["asset_receipt_date"].ToString());
                        VendorAssetList.Add(vn);
                    }
                }
                conn.Close();

                return Json(new { data = VendorAssetList.ToList() }, JsonRequestBehavior.AllowGet);


                //var AssetList = db.tr_asset_registration.Where(t => t.deleted_date == null && t.fl_active == true).Select(
                //t => new
                //{
                //    t.asset_id,
                //    t.asset_number,
                //    t.asset_name,
                //    t.asset_receipt_date
                //}).ToList();
                //return Json(new { data = AssetList.ToList() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var AssetList = db.tr_asset_registration.Where(t => t.deleted_date == null
                && t.asset_id == AssetID
                && t.fl_active == true
                //&& t.current_location_id == UserProfile.location_id
                ).Select(
                t => new
                {
                    t.asset_id,
                    t.asset_number,
                    t.asset_name,
                    t.asset_receipt_date,
                    t.fl_active
                }).ToList();
                //return Json(new { data = AssetList.ToList() }, JsonRequestBehavior.AllowGet);
                return Json(AssetList.ToList(), JsonRequestBehavior.AllowGet);
            }
            //return Json(AssetList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssetListToChanged()
        {
            var AssetList = from ast in db.tr_asset_registration
                            where (ast.deleted_date == null && ast.fl_active == true
                            && ast.company_id == UserProfile.company_id
                            //&& ast.asset_reg_location_id == UserProfile.asset_reg_location_id /* diremark by hendy */
                            //&& ast.current_location_id == UserProfile.location_id
                            )
                            join dep in db.tr_depreciation
                            on ast.asset_id equals dep.asset_id
                            where (dep.fl_depreciation == true && dep.fl_active == true)
                            select new
                            {
                                ast.asset_id,
                                ast.asset_number,
                                ast.asset_name,
                                ast.asset_receipt_date
                            };
            //var AssetList = db.tr_asset_registration.Where(t => t.deleted_date == null && t.fl_active == true).Select(
            //    t => new
            //    {
            //        t.asset_id,
            //        t.asset_number,
            //        t.asset_name,
            //        t.asset_receipt_date
            //    }).ToList();
            return Json(new { data = AssetList.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CostDepreByPeriod(string id)
        {
            int AssetID;
            //int.TryParse(Request.Form["asset_id"], out AssetID);
            int.TryParse(id, out AssetID);
            List<VendorAssetViewModel> VendorAssetList = new List<VendorAssetViewModel>();

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);

            var cmd2 = new SqlCommand("GetData", conn);
            cmd2.CommandText = "EXEC SP_COST_DEPRECIATION @asset_id ";
            cmd2.Parameters.AddWithValue("@asset_id", AssetID);

            conn.Open();
            using (SqlDataReader dr = cmd2.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(dr);

                foreach (DataRow row in tb.Rows)
                {
                    VendorAssetViewModel vn = new VendorAssetViewModel();

                    vn.fis_change_period = int.Parse(row["fis_depre_correction_month"].ToString());
                    vn.mkt_change_period = int.Parse(row["mkt_depre_correction_month"].ToString());
                    vn.variant_fiskal = decimal.Parse(row["variant_fis_cost"].ToString());
                    vn.variant_market = decimal.Parse(row["variant_mkt_cost"].ToString());

                    VendorAssetList.Add(vn);
                }
            }
            conn.Close();
            return Json(VendorAssetList.ToList(), JsonRequestBehavior.AllowGet);
            //return Json(new { data = VendorAssetList.ToList() }, JsonRequestBehavior.AllowGet);

            //return Json(AssetList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult ProcessGenerate()
        {
            int CurrUser = UserProfile.UserId;
            int OrgID = UserProfile.OrgId;

            List<VendorAssetViewModel> VendorAssetList = new List<VendorAssetViewModel>();

            #region remark

            int AssetID, DepreciationTypeID, OriCurrencyID;
            DateTime elipsDate;
            Decimal OriValue, usdKurs, idrKurs, BookValue;
            int FisUsefulLife, MktUsefulLife;
            Decimal FisResiduValue, FisDDBPercentage, MktResiduValue, MktDDBPercentage;

            string mode;

            int.TryParse(Request.Form["asset_id"], out AssetID);
            int.TryParse(Request.Form["depre_method"], out DepreciationTypeID);
            int.TryParse(Request.Form["currency"], out OriCurrencyID);

            mode = Request.Form["mode"].ToString();

            Decimal.TryParse(Request.Form["price"], out OriValue);
            DateTime.TryParse(Request.Form["elips_date"], out elipsDate);
            Decimal.TryParse(Request.Form["usd_kurs"], out usdKurs);
            Decimal.TryParse(Request.Form["idr_kurs"], out idrKurs);
            Decimal.TryParse(Request.Form["asset_value_usd"], out BookValue);

            int.TryParse(Request.Form["fiskal_life_value"], out FisUsefulLife);
            int.TryParse(Request.Form["komersil_life_value"], out MktUsefulLife);

            Decimal.TryParse(Request.Form["fiskal_residu_value"], out FisResiduValue);
            Decimal.TryParse(Request.Form["komersil_residu_value"], out MktResiduValue);
            Decimal.TryParse(Request.Form["fiskal_depreciation"], out FisDDBPercentage);
            Decimal.TryParse(Request.Form["komersial_depreciation"], out MktDDBPercentage);

            #endregion

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);


            var cmd = new SqlCommand("InsertDepreciation", conn);
            cmd.CommandText = "Exec SP_INSERT_TR_DEPRECIATION @asset_id,@depreciation_type_id, @asset_original_value,@asset_original_currency_id ,@elips_date ,@usd_kurs ,@idr_kurs, @asset_book_value ,@fis_asset_residu_value ,@fis_asset_usefull_life ,@fis_ddb_percentage ,@mkt_asset_residu_value ,@mkt_asset_usefull_life ,@mkt_ddb_percentage ,@created_by ,@orgID ";

            cmd.Parameters.AddWithValue("@asset_id", AssetID);
            cmd.Parameters.AddWithValue("@depreciation_type_id", DepreciationTypeID);
            cmd.Parameters.AddWithValue("@asset_original_value", OriValue);
            cmd.Parameters.AddWithValue("@asset_original_currency_id", OriCurrencyID);

            cmd.Parameters.AddWithValue("@elips_date", elipsDate);
            cmd.Parameters.AddWithValue("@usd_kurs", usdKurs);
            cmd.Parameters.AddWithValue("@idr_kurs", idrKurs);
            cmd.Parameters.AddWithValue("@asset_book_value", BookValue);
            cmd.Parameters.AddWithValue("@created_by", CurrUser);
            cmd.Parameters.AddWithValue("@orgID", OrgID);

            cmd.Parameters.AddWithValue("@fis_asset_residu_value", FisResiduValue);
            cmd.Parameters.AddWithValue("@fis_asset_usefull_life", FisUsefulLife);
            cmd.Parameters.AddWithValue("@fis_ddb_percentage", FisDDBPercentage);

            cmd.Parameters.AddWithValue("@mkt_asset_residu_value", MktResiduValue);
            cmd.Parameters.AddWithValue("@mkt_asset_usefull_life", MktUsefulLife);
            cmd.Parameters.AddWithValue("@mkt_ddb_percentage", MktDDBPercentage);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            var cmd2 = new SqlCommand("InsertDepreciation", conn);
            cmd2.CommandText = "select * from dbo.FN_GET_ASSET_HISTORY_BY_DEPRETYPE(@asset_id,@depreciation_type_id)";
            cmd2.Parameters.AddWithValue("@asset_id", AssetID);
            cmd2.Parameters.AddWithValue("@depreciation_type_id", DepreciationTypeID);
            conn.Open();
            using (SqlDataReader dr = cmd2.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(dr);

                foreach (DataRow row in tb.Rows)
                {
                    VendorAssetViewModel vn = new VendorAssetViewModel();

                    //vn.asset_id = int.Parse(row["asset_id"].ToString());
                    //vn.depreciation_type_id = int.Parse(row["depreciation_type_id"].ToString());
                    //vn.fis_asset_usefull_life = decimal.Parse(row["fis_asset_usefull_life"].ToString());
                    //vn.mkt_asset_usefull_life = decimal.Parse(row["mkt_asset_usefull_life"].ToString());

                    //vn.periode = int.Parse(row["period"].ToString());
                    vn.periode = row["period"].ToString();
                    //vn.asset_price_fiskal = decimal.Parse(row["asset_price_fiskal"].ToString());
                    //vn.asset_price_market = decimal.Parse(row["asset_price_market"].ToString()); 
                    //vn.fis_depreciation_value = decimal.Parse(row["fis_depreciation_value"].ToString());
                    //vn.mkt_depreciation_value = decimal.Parse(row["mkt_depreciation_value"].ToString());
                    vn.asset_price_fiskal_before = decimal.Parse(row["asset_price_fiskal_before"].ToString());
                    vn.asset_price_fiskal_after = decimal.Parse(row["asset_price_fiskal_after"].ToString());
                    vn.variant_fiskal = vn.asset_price_fiskal_after - vn.asset_price_fiskal_before;

                    vn.asset_price_market_before = decimal.Parse(row["asset_price_market_before"].ToString());
                    vn.asset_price_market_after = decimal.Parse(row["asset_price_market_after"].ToString());
                    vn.variant_market = vn.asset_price_market_after - vn.asset_price_market_before;

                    //vn.asset_number = row["asset_number"].ToString();
                    //vn.asset_name = row["asset_name"].ToString();
                    //vn.asset_price_fiskal = decimal.Parse(row["asset_price_fiskal"].ToString());
                    //vn.asset_price_market = decimal.Parse(row["asset_price_market"].ToString());
                    //vn.usage_life_time_fiskal = row["usage_life_time_fiskal"].ToString();
                    //vn.usage_life_time_market = row["usage_life_time_market"].ToString();

                    //vn.fis_book_value = decimal.Parse(row["fis_book_value"].ToString());
                    //vn.mkt_book_value = decimal.Parse(row["mkt_book_value"].ToString());
                    VendorAssetList.Add(vn);
                }
            }
            conn.Close();


            return Json(VendorAssetList.ToList(), JsonRequestBehavior.AllowGet);
            //return Json(VendorAssetList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save()
        {
            #region Variable Declaration
            int CurrUser, OrgID;
            string mode;

            int AssetID, DepreciationTypeID, OriCurrencyID;
            DateTime elipsDate;
            Decimal OriValue, usdKurs, idrKurs, BookValue, BookValueIDR;
            int FisUsefulLife, MktUsefulLife;
            Decimal FisResiduValue, MktResiduValue;
            #endregion

            #region Variable Value Assign

            CurrUser = UserProfile.UserId;
            OrgID = UserProfile.OrgId;
            mode = Request.Form["mode"].ToString();

            int.TryParse(Request.Form["asset_id"], out AssetID);
            int.TryParse(Request.Form["depre_method"], out DepreciationTypeID);
            int.TryParse(Request.Form["currency"], out OriCurrencyID);

            Decimal.TryParse(Request.Form["price"].Replace(",",""), out OriValue);
            var a_date = Request.Form["elips_date"];
            string[] arr_part = a_date.Split('-');
            string d_date = null;
            if (arr_part.Length >= 2)
                d_date = arr_part[2] + "/" + arr_part[1] + "/" + arr_part[0];
            else
                d_date = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
            DateTime.TryParse(d_date, out elipsDate);
            Decimal.TryParse(Request.Form["usd_kurs"].Replace(",", ""), out usdKurs);
            Decimal.TryParse(Request.Form["idr_kurs"], out idrKurs);
            Decimal.TryParse(Request.Form["asset_value_usd"].Replace(",", ""), out BookValue);
            Decimal.TryParse(Request.Form["asset_value_idr"].Replace(",", ""), out BookValueIDR);

            int.TryParse(Request.Form["fiskal_life_value"], out FisUsefulLife);
            int.TryParse(Request.Form["komersil_life_value"], out MktUsefulLife);

            Decimal.TryParse(Request.Form["fiskal_residu_value"], out FisResiduValue);
            Decimal.TryParse(Request.Form["komersil_residu_value"], out MktResiduValue);

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            #endregion

            var conn = new SqlConnection(constring);

            if (mode.ToLower() == "edit")
            {
                var cmd = new SqlCommand("UpdateDepreciation", conn);
                cmd.CommandText = "Exec SP_UPDATE_TR_DEPRECIATION @asset_id,@depreciation_type_id, @asset_original_value,@asset_original_currency_id ,@elips_date ,@usd_kurs ,@idr_kurs ,@asset_book_value,@asset_book_value_idr ,@fis_asset_residu_value ,@fis_asset_usefull_life ,@mkt_asset_residu_value ,@mkt_asset_usefull_life ,@updated_by ,@orgID ";

                cmd.Parameters.AddWithValue("@asset_id", AssetID);
                cmd.Parameters.AddWithValue("@depreciation_type_id", DepreciationTypeID);
                cmd.Parameters.AddWithValue("@asset_original_value", OriValue);
                cmd.Parameters.AddWithValue("@asset_original_currency_id", OriCurrencyID);

                cmd.Parameters.AddWithValue("@elips_date", elipsDate);
                cmd.Parameters.AddWithValue("@usd_kurs", usdKurs);
                cmd.Parameters.AddWithValue("@idr_kurs", idrKurs);
                cmd.Parameters.AddWithValue("@asset_book_value", BookValue);
                cmd.Parameters.AddWithValue("@asset_book_value_idr", BookValueIDR);
                cmd.Parameters.AddWithValue("@updated_by", CurrUser);
                cmd.Parameters.AddWithValue("@orgID", OrgID);

                cmd.Parameters.AddWithValue("@fis_asset_residu_value", FisResiduValue);
                cmd.Parameters.AddWithValue("@fis_asset_usefull_life", FisUsefulLife);

                cmd.Parameters.AddWithValue("@mkt_asset_residu_value", MktResiduValue);
                cmd.Parameters.AddWithValue("@mkt_asset_usefull_life", MktUsefulLife);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                return Json("Update Depreciation Success", JsonRequestBehavior.AllowGet);

            }
            else
            {
                var cmd = new SqlCommand("InsertDepreciation", conn);
                cmd.CommandText = "Exec SP_INSERT_TR_DEPRECIATION @asset_id,@depreciation_type_id, @asset_original_value,@asset_original_currency_id ,@elips_date ,@usd_kurs ,@idr_kurs ,@asset_book_value,@asset_book_value_idr ,@fis_asset_residu_value ,@fis_asset_usefull_life ,@mkt_asset_residu_value ,@mkt_asset_usefull_life ,@created_by ,@orgID ";

                cmd.Parameters.AddWithValue("@asset_id", AssetID);
                cmd.Parameters.AddWithValue("@depreciation_type_id", DepreciationTypeID);
                cmd.Parameters.AddWithValue("@asset_original_value", OriValue);
                cmd.Parameters.AddWithValue("@asset_original_currency_id", OriCurrencyID);

                cmd.Parameters.AddWithValue("@elips_date", elipsDate);
                cmd.Parameters.AddWithValue("@usd_kurs", usdKurs);
                cmd.Parameters.AddWithValue("@idr_kurs", idrKurs);
                cmd.Parameters.AddWithValue("@asset_book_value", BookValue);
                cmd.Parameters.AddWithValue("@asset_book_value_idr", BookValueIDR);
                cmd.Parameters.AddWithValue("@created_by", CurrUser);
                cmd.Parameters.AddWithValue("@orgID", OrgID);

                cmd.Parameters.AddWithValue("@fis_asset_residu_value", FisResiduValue);
                cmd.Parameters.AddWithValue("@fis_asset_usefull_life", FisUsefulLife);

                cmd.Parameters.AddWithValue("@mkt_asset_residu_value", MktResiduValue);
                cmd.Parameters.AddWithValue("@mkt_asset_usefull_life", MktUsefulLife);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                return Json("Insert Depreciation Success", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveChange()
        {
            #region Variable Declaration
            int CurrUser, OrgID, ChangeReason, depreciationID;
            string mode;

            int AssetID, DepreciationTypeID, OriCurrencyID;
            DateTime elipsDate;
            Decimal OriValue, usdKurs, idrKurs, BookValue, BookValueIDR;
            int FisUsefulLife, MktUsefulLife;
            Decimal FisResiduValue, MktResiduValue;
            #endregion

            #region Variable Value Assign

            CurrUser = UserProfile.UserId;
            OrgID = UserProfile.OrgId;
            mode = Request.Form["mode"].ToString();

            int.TryParse(Request.Form["asset_id"], out AssetID);
            int.TryParse(Request.Form["depre_method"], out DepreciationTypeID);
            int.TryParse(Request.Form["currency"], out OriCurrencyID);
            int.TryParse(Request.Form["change_reason"], out ChangeReason);
            int.TryParse(Request.Form["depreciation_id"], out depreciationID);

            Decimal.TryParse(Request.Form["price"], out OriValue);
            var a_date = Request.Form["elips_date"];
            string[] arr_part = a_date.Split('-');
            string d_date = null;
            if (arr_part.Length >= 2)
                d_date = arr_part[2] + "/" + arr_part[1] + "/" + arr_part[0];
            else
                d_date = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
            DateTime.TryParse(d_date, out elipsDate);
            //DateTime.TryParse(Request.Form["elips_date"], out elipsDate);
            Decimal.TryParse(Request.Form["usd_kurs"], out usdKurs);
            Decimal.TryParse(Request.Form["idr_kurs"], out idrKurs);
            Decimal.TryParse(Request.Form["asset_value_usd"], out BookValue);
            Decimal.TryParse(Request.Form["asset_value_idr"], out BookValueIDR);

            int.TryParse(Request.Form["fiskal_life_value"], out FisUsefulLife);
            int.TryParse(Request.Form["komersil_life_value"], out MktUsefulLife);

            Decimal.TryParse(Request.Form["fiskal_residu_value"], out FisResiduValue);
            Decimal.TryParse(Request.Form["komersil_residu_value"], out MktResiduValue);

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            #endregion

            var conn = new SqlConnection(constring);

            var cmd = new SqlCommand("InsertDepreciation", conn);
            cmd.CommandText = "Exec SP_INSERT_CHANGE_TR_DEPRECIATION @asset_id,@depreciation_type_id, @asset_original_value,@asset_original_currency_id ,@elips_date, @usd_kurs, @idr_kurs ,@asset_book_value,@asset_book_value_idr ,@fis_asset_residu_value ,@fis_asset_usefull_life ,@mkt_asset_residu_value ,@mkt_asset_usefull_life ,@created_by ,@orgID,@fl_change_reason,@depreciation_id ";

            cmd.Parameters.AddWithValue("@asset_id", AssetID);
            cmd.Parameters.AddWithValue("@depreciation_type_id", DepreciationTypeID);
            cmd.Parameters.AddWithValue("@asset_original_value", OriValue);
            cmd.Parameters.AddWithValue("@asset_original_currency_id", OriCurrencyID);

            cmd.Parameters.AddWithValue("@elips_date", elipsDate);
            cmd.Parameters.AddWithValue("@usd_kurs", usdKurs);
            cmd.Parameters.AddWithValue("@idr_kurs", idrKurs);
            cmd.Parameters.AddWithValue("@asset_book_value", BookValue);
            cmd.Parameters.AddWithValue("@asset_book_value_idr", BookValueIDR);
            cmd.Parameters.AddWithValue("@created_by", CurrUser);
            cmd.Parameters.AddWithValue("@orgID", OrgID);

            cmd.Parameters.AddWithValue("@fis_asset_residu_value", FisResiduValue);
            cmd.Parameters.AddWithValue("@fis_asset_usefull_life", FisUsefulLife);

            cmd.Parameters.AddWithValue("@mkt_asset_residu_value", MktResiduValue);
            cmd.Parameters.AddWithValue("@mkt_asset_usefull_life", MktUsefulLife);

            cmd.Parameters.AddWithValue("@fl_change_reason", ChangeReason);
            cmd.Parameters.AddWithValue("@depreciation_id", depreciationID);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            var cmd2 = new SqlCommand("ChangeDepreciation", conn);
            cmd2.CommandText = "Exec SP_INSERT_CHANGE_TR_DEPRECIATION_PROCESS @asset_id,@depreciation_type_id,@created_by ,@orgID ";

            cmd2.Parameters.AddWithValue("@asset_id", AssetID);
            cmd2.Parameters.AddWithValue("@depreciation_type_id", DepreciationTypeID);
            cmd2.Parameters.AddWithValue("@created_by", CurrUser);
            cmd2.Parameters.AddWithValue("@orgID", OrgID);

            conn.Open();
            cmd2.ExecuteNonQuery();
            conn.Close();

            List<VendorAssetViewModel> VendorAssetList = new List<VendorAssetViewModel>();
            var cmd3 = new SqlCommand("ShowSimulationData", conn);
            cmd3.CommandText = "EXEC SP_GET_DEPRECIATION_SIMULATION @asset_id,@depreciation_type_id";
            cmd3.Parameters.AddWithValue("@asset_id", AssetID);
            cmd3.Parameters.AddWithValue("@depreciation_type_id", DepreciationTypeID);

            conn.Open();
            using (SqlDataReader dr = cmd3.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(dr);

                foreach (DataRow row in tb.Rows)
                {
                    VendorAssetViewModel vn = new VendorAssetViewModel();
                    vn.periode = row["period"].ToString();

                    vn.asset_price_fiskal_before = decimal.Parse(row["asset_price_fiskal_before"].ToString());
                    vn.asset_price_fiskal_after = decimal.Parse(row["asset_price_fiskal_after"].ToString());
                    vn.variant_fiskal = vn.asset_price_fiskal_after - vn.asset_price_fiskal_before;

                    vn.asset_price_market_before = decimal.Parse(row["asset_price_market_before"].ToString());
                    vn.asset_price_market_after = decimal.Parse(row["asset_price_market_after"].ToString());
                    vn.variant_market = vn.asset_price_market_after - vn.asset_price_market_before;

                    VendorAssetList.Add(vn);
                }
            }
            conn.Close();


            return Json(VendorAssetList.ToList(), JsonRequestBehavior.AllowGet);

            //return Json("Change Depreciation Success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDepreciation(string id)
        {
            int CurrUser = UserProfile.UserId;
            int OrgID = UserProfile.OrgId;
            int depreciationID;
            string mode = id;
            int.TryParse(Request.Form["depreciation_id"], out depreciationID);

            List<VendorAssetViewModel> VendorAssetList = new List<VendorAssetViewModel>();

            int AssetID, DepreciationTypeID;

            int.TryParse(Request.Form["asset_id"], out AssetID);
            int.TryParse(Request.Form["depre_method"], out DepreciationTypeID);
            CurrUser = UserProfile.UserId;
            OrgID = UserProfile.OrgId;

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);

            var cmd = new SqlCommand("DeleteDepreciation", conn);
            cmd.CommandText = "Exec SP_DELETE_ASSET_DEPRECIATION @asset_id,@depreciation_type_id,@userid,@orgid,@mode,@depreciation_id";

            cmd.Parameters.AddWithValue("@asset_id", AssetID);
            cmd.Parameters.AddWithValue("@depreciation_type_id", DepreciationTypeID);
            cmd.Parameters.AddWithValue("@userid", CurrUser);
            cmd.Parameters.AddWithValue("@orgid", OrgID);
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@depreciation_id", depreciationID);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            if (mode.ToLower() == "set")
            {
                return Json("Delete Depreciation Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Cancel Change Success", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SubmitDepreciation(string id)
        {
            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);

            string mode = Request.Form["mode"].ToString();
            int AssetID = int.Parse(Request.Form["asset_id"].ToString());

            var cmd = new SqlCommand("SubmitDepreciation", conn);
            cmd.CommandText = "Exec SP_SUBMIT_DEPRECIATION_CHANGES @asset_id,@depreciation_type_id, @mode, @org_id";

            cmd.Parameters.AddWithValue("@asset_id", AssetID);
            cmd.Parameters.AddWithValue("@depreciation_type_id", 1);
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@org_id", UserProfile.OrgId);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Json("Change Depreciation Success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAssetDepreciationDetail(string id)
        {
            //int AssetID = int.Parse(id);
            int AssetID, DepreTypeID;
            if (id != null)
            {
                AssetID = int.Parse(id);
                DepreTypeID = 1;
            }
            else
            {
                AssetID = int.Parse(Request.Form["asset_id"].ToString());
                DepreTypeID = int.Parse(Request.Form["depre_method"].ToString());
            }
            List<depreciationViewModel> VendorAssetList = new List<depreciationViewModel>();

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);

            var cmd2 = new SqlCommand("GetDepreciation", conn);
            //cmd2.CommandText = "select * from dbo.FN_GET_ASSET_VALUE_BY_DEPTYPE(@vendor_id,@asset_id,@depreciation_type_id)";
            cmd2.CommandText = "EXEC SP_GET_DEPRECIATION_BY_ASSET @asset_id,@depreciation_type_id ";
            cmd2.Parameters.AddWithValue("@asset_id", AssetID);
            cmd2.Parameters.AddWithValue("@depreciation_type_id", DepreTypeID);
            conn.Open();
            using (SqlDataReader dr = cmd2.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(dr);

                foreach (DataRow row in tb.Rows)
                {
                    depreciationViewModel vn = new depreciationViewModel();

                    vn.asset_id = int.Parse(row["asset_id"].ToString());
                    vn.depreciation_type_id = int.Parse(row["depreciation_type_id"].ToString());
                    vn.asset_original_value = decimal.Parse(row["asset_original_value"].ToString());
                    vn.asset_original_currency_id = int.Parse(row["asset_original_currency_id"].ToString());
                    vn.asset_book_value = decimal.Parse(row["asset_book_value"].ToString());
                    vn.asset_book_value_idr = decimal.Parse(row["asset_book_value_idr"].ToString());
                    vn.elips_date = Convert.ToDateTime(row["elips_date"]);
                    vn.usd_kurs = decimal.Parse(row["usd_kurs"].ToString());
                    vn.idr_kurs = decimal.Parse(row["idr_kurs"].ToString());
                    vn.depreciation_id = int.Parse(row["depreciation_id"].ToString());
                    vn.fl_change_reason = int.Parse(row["fl_change_reason"].ToString());
                    //bool changeReason = bool.Parse(row["fl_change_reason"].ToString());
                    //if (changeReason)
                    //{
                    //    vn.fl_change_reason = 1;
                    //}
                    //else
                    //{
                    //    vn.fl_change_reason = 0;
                    //}
                    vn.fis_asset_usefull_life = decimal.Parse(row["fis_asset_usefull_life"].ToString());
                    vn.fis_asset_residu_value = decimal.Parse(row["fis_asset_residu_value"].ToString());
                    //20190603, Alvin, remark bagian detailnya karena hanya btuh header, BEGIN
                    //vn.fis_ddb_percentage = decimal.Parse(row["fis_ddb_percentage"].ToString());
                    //20190603, Alvin, remark bagian detailnya karena hanya btuh header, END

                    vn.mkt_asset_usefull_life = decimal.Parse(row["mkt_asset_usefull_life"].ToString());
                    vn.mkt_asset_residu_value = decimal.Parse(row["mkt_asset_residu_value"].ToString());
                    //20190603, Alvin, remark bagian detailnya karena hanya btuh header, BEGIN
                    //vn.mkt_ddb_percentage = decimal.Parse(row["mkt_ddb_percentage"].ToString());
                    //20190603, Alvin, remark bagian detailnya karena hanya btuh header, END

                    //20190603, Alvin, remark bagian detailnya karena hanya btuh header, BEGIN
                    //vn.usage_life_time_fiskal = row["usage_life_time_fiskal"].ToString();
                    //vn.usage_life_time_market = row["usage_life_time_market"].ToString();
                    //vn.fis_depreciation_value = decimal.Parse(row["fis_depreciation_value"].ToString());
                    //vn.mkt_depreciation_value = decimal.Parse(row["mkt_depreciation_value"].ToString());

                    //vn.period = int.Parse(row["period"].ToString());
                    //vn.fis_book_value = decimal.Parse(row["fis_book_value"].ToString());

                    //vn.fis_total_depreciation = vn.fis_depreciation_value * vn.period;

                    //vn.mkt_book_value = decimal.Parse(row["mkt_book_value"].ToString());
                    //vn.mkt_total_depreciation = vn.mkt_depreciation_value * vn.period;
                    //20190603, Alvin, remark bagian detailnya karena hanya btuh header, END
                    VendorAssetList.Add(vn);
                }
            }
            conn.Close();

            return Json(VendorAssetList.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCurrentDepreciationValue(string id)
        {
            int CreatedBy = UserProfile.UserId;
            int OrgID = UserProfile.OrgId;

            List<depreciationViewModel> VendorAssetList = new List<depreciationViewModel>();


            int AssetID, DepreciationTypeID;

            int.TryParse(Request.Form["asset_id"], out AssetID);
            int.TryParse(Request.Form["depre_method"], out DepreciationTypeID);
            CreatedBy = UserProfile.UserId;
            OrgID = UserProfile.OrgId;

            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);

            var cmd = new SqlCommand("InsertDepreciation", conn);
            cmd.CommandText = "Exec SP_CANCEL_ASSET_DEPRECIATION @asset_id,@depreciation_type_id";

            cmd.Parameters.AddWithValue("@asset_id", AssetID);
            cmd.Parameters.AddWithValue("@depreciation_type_id", DepreciationTypeID);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Json("Cancel Depreciation Success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetKurs(DateTime date, int currency_id, int mode)
        {
            decimal _result = 0;
            //try
            //{
            string constring = WebConfigurationManager.ConnectionStrings["ModelAsmRemote"].ConnectionString;
            var conn = new SqlConnection(constring);
            conn.Open();
            var cmd = new SqlCommand("SP_GET_KURS", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@curr_id", currency_id);
            cmd.Parameters.AddWithValue("@mode", mode);

            _result = Convert.ToDecimal(cmd.ExecuteScalar());
            conn.Close();
            var jsonData = new
            {
                data = _result.ToString()
            };
            //}
            //catch { }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        // GET: depreciation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_depreciation tr_depreciation = db.tr_depreciation.Find(id);
            if (tr_depreciation == null)
            {
                return HttpNotFound();
            }
            return View(tr_depreciation);
        }

        // GET: depreciation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: depreciation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "depreciation_id,asset_id,depreciation_type_id,asset_original_value,asset_original_currency_id,elips_date,usd_kurs,idr_kurs,asset_book_value,fis_asset_residu_value,fis_asset_usefull_life,fis_ddb_percentage,mkt_asset_residu_value,mkt_asset_usefull_life,mkt_ddb_percentage,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_depreciation tr_depreciation)
        {
            if (ModelState.IsValid)
            {
                db.tr_depreciation.Add(tr_depreciation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tr_depreciation);
        }

        // GET: depreciation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_depreciation tr_depreciation = db.tr_depreciation.Find(id);
            if (tr_depreciation == null)
            {
                return HttpNotFound();
            }
            return View(tr_depreciation);
        }

        // POST: depreciation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "depreciation_id,asset_id,depreciation_type_id,asset_original_value,asset_original_currency_id,elips_date,usd_kurs,idr_kurs,asset_book_value,fis_asset_residu_value,fis_asset_usefull_life,fis_ddb_percentage,mkt_asset_residu_value,mkt_asset_usefull_life,mkt_ddb_percentage,fl_active,created_date,created_by,updated_date,updated_by,deleted_date,deleted_by,org_id")] tr_depreciation tr_depreciation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tr_depreciation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tr_depreciation);
        }

        // GET: depreciation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tr_depreciation tr_depreciation = db.tr_depreciation.Find(id);
            if (tr_depreciation == null)
            {
                return HttpNotFound();
            }
            return View(tr_depreciation);
        }

        // POST: depreciation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tr_depreciation tr_depreciation = db.tr_depreciation.Find(id);
            db.tr_depreciation.Remove(tr_depreciation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
