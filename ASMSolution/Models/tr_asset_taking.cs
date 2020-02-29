namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_asset_taking
    {
        [Key]
        public int asset_taking_id { get; set; }

        public DateTime? asset_taking_date { get; set; }

        public int? period_year { get; set; }

        public int? period_month { get; set; }

        public int? company_id { get; set; }

        public int? location_id { get; set; }

        public int? department_id { get; set; }

        [StringLength(50)]
        public string file_name { get; set; }

        public int? employee_id { get; set; }

        public bool? fl_submit_data { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }
    }
}
