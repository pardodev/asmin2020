namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_user_rights
    {
        [Key]
        public int user_rights_id { get; set; }

        public int? menu_id { get; set; }

        public int user_type_id { get; set; }

        public int job_level_id { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }

        public virtual ms_job_level ms_job_level { get; set; }

        public virtual ms_user_type ms_user_type { get; set; }
    }
}
