namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_depreciation_change_log
    {
        [Key]
        public int change_log_id { get; set; }

        public int? asset_id { get; set; }

        public int? depreciation_id { get; set; }

        public int? depreciation_detail_id { get; set; }

        public int? fis_depre_correction_month { get; set; }

        public int? mkt_depre_correction_month { get; set; }

        public decimal? variant_fis_cost { get; set; }

        public decimal? variant_mkt_cost { get; set; }
    }
}
