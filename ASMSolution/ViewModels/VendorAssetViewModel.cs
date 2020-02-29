using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASM_UI.Models
{
    public class VendorAssetViewModel
    {
        //FN_GET_ASSET_VALUE_BY_DEPTYPE
        public int asset_id { get; set; }
        public string asset_number { get; set; }
        public string asset_name { get; set; }
        public int asset_original_currency_id { get; set; }
        public decimal asset_original_value { get; set; }
        public decimal Currency_kurs { get; set; }
        public int depreciation_type_id { get; set; }

        public int fl_change_reason { get; set; }

        //ini smuanya adl asset_book_value
        public decimal asset_book_value { get; set; }
        public decimal fis_book_value { get; set; }
        public decimal mkt_book_value { get; set; }

        public decimal asset_price_fiskal { get; set; }
        public decimal asset_price_market { get; set; }

        public decimal asset_price_market_before { get; set; }
        public decimal asset_price_market_after { get; set; }
        public decimal variant_market { get; set; }
        
        public decimal asset_price_fiskal_before { get; set; }
        public decimal asset_price_fiskal_after { get; set; }
        public decimal variant_fiskal { get; set; }

        public string usage_life_time_fiskal { get; set; }
        public string usage_life_time_market { get; set; }

        public decimal fis_depreciation_per_month { get; set; }
        public decimal mkt_depreciation_per_month { get; set; }
        public decimal fis_total_depreciation { get; set; }
        public decimal mkt_total_depreciation { get; set; }


        public decimal fis_asset_residu_value { get; set; }
        public decimal fis_ddb_percentage { get; set; }

        public decimal mkt_asset_residu_value { get; set; }
        public decimal mkt_ddb_percentage { get; set; }
        
        public decimal fis_asset_usefull_life { get; set; }
        public decimal mkt_asset_usefull_life { get; set; }

        public int cost_depre { get; set; }

        public int fis_change_period { get; set; }
        public int mkt_change_period { get; set; }
        public string periode { get; set; }
        public decimal fis_depreciation_value { get; set; }
        public decimal mkt_depreciation_value { get; set; }

        //tambahan kolom dipakai di process.cshtml depreciation
        public int depreciation_id { get; set; }
        public string department_code { get; set; }
        public string employee_name { get; set; }
        public DateTime asset_receipt_date { get; set; }
    }
}