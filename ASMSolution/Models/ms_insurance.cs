namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_insurance
    {
        [Key]
        public int insurance_id { get; set; }

        [StringLength(4)]
        public string insurance_code { get; set; }

        [StringLength(50)]
        public string insurance_name { get; set; }

        [StringLength(200)]
        public string insurance_address { get; set; }

        public int? country_id { get; set; }

        [StringLength(50)]
        public string insurance_phone { get; set; }

        [StringLength(50)]
        public string insurance_mail { get; set; }

        [StringLength(50)]
        public string insurance_cp_name { get; set; }

        [StringLength(50)]
        public string insurance_cp_phone { get; set; }

        [StringLength(50)]
        public string insurance_cp_mail { get; set; }

        [StringLength(500)]
        public string insurance_description { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }

        public virtual ms_country ms_country { get; set; }
    }
}
