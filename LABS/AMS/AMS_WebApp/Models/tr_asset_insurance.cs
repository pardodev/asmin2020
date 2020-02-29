namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_asset_insurance
    {
        [Key]
        public int insurance_activa_id { get; set; }

        public int? asset_id { get; set; }

        [StringLength(50)]
        public string insurance_activa_number { get; set; }

        [StringLength(100)]
        public string insurance_activa_name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? insurance_activa_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? insurance_activa_exp_date { get; set; }

        public int? insurance_id { get; set; }

        [StringLength(300)]
        public string insurance_activa_description { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_int { get; set; }

        public int? org_id { get; set; }
    }
}
