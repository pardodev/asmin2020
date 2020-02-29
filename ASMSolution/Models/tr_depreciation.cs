namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_depreciation
    {
        [Key]
        public int depreciation_id { get; set; }

        public int? asset_id { get; set; }

        public int? depreciation_type_id { get; set; }

        public decimal? asset_original_value { get; set; }

        public int? asset_original_currency_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? elips_date { get; set; }

        public decimal? usd_kurs { get; set; }

        public decimal? idr_kurs { get; set; }

        public decimal? asset_book_value { get; set; }
        public decimal? asset_book_value_idr { get; set; }

        public decimal? fis_asset_residu_value { get; set; }

        public int? fis_asset_usefull_life { get; set; }

        public decimal? fis_ddb_percentage { get; set; }

        public decimal? mkt_asset_residu_value { get; set; }

        public int? mkt_asset_usefull_life { get; set; }

        public decimal? mkt_ddb_percentage { get; set; }

        public bool? fl_depreciation { get; set; }

        public DateTime? depreciation_date { get; set; }

        public bool? fl_change_reason { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }
    }
}
