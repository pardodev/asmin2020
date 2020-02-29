namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sy_message_notification
    {
        [Key]
        public int notify_id { get; set; }

        [StringLength(150)]
        public string notif_group { get; set; }

        [StringLength(150)]
        public string notify_user { get; set; }

        [StringLength(50)]
        public string notify_ip { get; set; }

        [StringLength(255)]
        public string notify_message { get; set; }

        public DateTime? notify_time { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public int? fl_shown { get; set; }

        public DateTime? shown_date { get; set; }

        [StringLength(255)]
        public string shown_ip { get; set; }

        [StringLength(100)]
        public string shown_host { get; set; }

        [StringLength(255)]
        public string err_message { get; set; }
    }
}
