namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sy_ref_approval_level
    {
        [Key]
        public int ref_id { get; set; }

        public int? asset_reg_location_id { get; set; }

        public int? job_level_id { get; set; }

        public int? order_no { get; set; }
    }
}
