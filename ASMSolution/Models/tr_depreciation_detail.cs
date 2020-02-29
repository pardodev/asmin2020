namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_depreciation_detail
    {
        [Key]
        public int depreciation_detail_id { get; set; }

        public int? depreciation_id { get; set; }

        public int? depreciation_type_id { get; set; }

        public int? period { get; set; }

        public int? period_year { get; set; }

        public int? period_month { get; set; }

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
