namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sy_lookup
    {
        [Key]
        public int lkp_id { get; set; }

        [StringLength(150)]
        public string lkp_code { get; set; }

        [StringLength(150)]
        public string lkp_value { get; set; }

        public string lkp_text { get; set; }

        public int? rec_order { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }
    }
}
