namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sy_app_setting
    {
        [Key]
        public int app_id { get; set; }

        [StringLength(150)]
        public string app_key { get; set; }

        public string app_value { get; set; }

        [StringLength(150)]
        public string app_desc { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }
    }
}
