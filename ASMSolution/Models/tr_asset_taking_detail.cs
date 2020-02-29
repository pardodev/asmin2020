namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_asset_taking_detail
    {
        [Key]
        public long asset_taking_detail_id { get; set; }

        public int asset_taking_id { get; set; }

        public int asset_id { get; set; }

        [StringLength(30)]
        public string asset_number { get; set; }

        public bool? fl_available_asset { get; set; }

        public int? asset_status_id { get; set; }

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
