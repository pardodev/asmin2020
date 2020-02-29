namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_depreciation_history
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int depreciation_history_id { get; set; }

        public int? depreciation_detail_id { get; set; }

        public int? depreciation_method_id { get; set; }

        public int? period_year { get; set; }

        public int? period_month { get; set; }

        public decimal? fis_depreciation_value { get; set; }

        public decimal? fis_current_asset_value { get; set; }

        public int? fis_usefull_asset_month { get; set; }

        public decimal? mkt_depreciation_value { get; set; }

        public decimal? mkt_current_asset_value { get; set; }

        public int? mkt_usefull_asset_month { get; set; }

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
