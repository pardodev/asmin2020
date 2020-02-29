namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_approval_status
    {
        [Key]
        public int approval_status_id { get; set; }

        [StringLength(10)]
        public string approval_status_code { get; set; }

        [StringLength(50)]
        public string approval_status_name { get; set; }
    }
}
