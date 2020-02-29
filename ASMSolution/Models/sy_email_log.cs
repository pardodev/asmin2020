namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sy_email_log
    {
        [Key]
        public int elog_id { get; set; }

        [StringLength(150)]
        public string elog_template { get; set; }

        [StringLength(150)]
        public string elog_from { get; set; }

        [StringLength(150)]
        public string elog_to { get; set; }

        [StringLength(150)]
        public string elog_cc { get; set; }

        [StringLength(150)]
        public string elog_bcc { get; set; }

        [StringLength(150)]
        public string elog_subject { get; set; }

        public string elog_body { get; set; }

        public bool? elog_has_attachment { get; set; }

        [MaxLength(1)]
        public byte[] elog_file_attachment { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public bool? fl_sent { get; set; }

        public DateTime? sent_date { get; set; }

        [StringLength(255)]
        public string err_message { get; set; }
    }
}
