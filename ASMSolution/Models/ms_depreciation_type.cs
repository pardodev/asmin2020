namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_depreciation_type
    {
        [Key]
        public int depreciation_type_id { get; set; }

        [StringLength(50)]
        public string depreciation_type_code { get; set; }

        [StringLength(50)]
        public string depreciation_type_name { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? update_date { get; set; }

        public int? update_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }
    }
}
