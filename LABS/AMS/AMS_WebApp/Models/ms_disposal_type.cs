namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_disposal_type
    {
        [Key]
        public int disposal_type_id { get; set; }

        [StringLength(20)]
        public string disposal_type_code { get; set; }

        [StringLength(50)]
        public string disposal_type_name { get; set; }

        public int? disposal_by_dept_id { get; set; }
    }
}
