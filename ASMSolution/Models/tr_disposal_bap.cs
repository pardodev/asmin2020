namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_disposal_bap
    {
        [Key]
        public int disposal_bap_id { get; set; }

        public int? disposal_id { get; set; }

        public int? disposal_dept_id { get; set; }

        public bool? fl_disposal_process { get; set; }

        public DateTime? disposal_bap_date { get; set; }

        [StringLength(300)]
        public string disposal_bap_description { get; set; }

        [StringLength(300)]
        public string disposal_upload_address { get; set; }

        public int? disposal_change_dept_id { get; set; }

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
