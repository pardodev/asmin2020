namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_vendor
    {
        [Key]
        public int vendor_id { get; set; }

        [StringLength(4)]
        public string vendor_code { get; set; }

        [StringLength(50)]
        public string vendor_name { get; set; }

        [StringLength(200)]
        public string vendor_address { get; set; }

        public int? country_id { get; set; }

        [StringLength(50)]
        public string vendor_phone { get; set; }

        [StringLength(50)]
        public string vendor_mail { get; set; }

        [StringLength(50)]
        public string vendor_cp_name { get; set; }

        [StringLength(50)]
        public string vendor_cp_phone { get; set; }

        [StringLength(50)]
        public string vendor_cp_mail { get; set; }

        [StringLength(500)]
        public string vendor_description { get; set; }

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
