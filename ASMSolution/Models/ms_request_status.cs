namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_request_status
    {
        [Key]
        public int request_status_id { get; set; }

        [StringLength(10)]
        public string request_status_code { get; set; }

        [StringLength(50)]
        public string request_status_name { get; set; }
    }
}
