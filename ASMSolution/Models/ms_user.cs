namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_user
    {
        [Key]
        public int user_id { get; set; }

        [StringLength(50)]
        public string user_name { get; set; }

        [StringLength(255)]
        public string user_password { get; set; }

        public int user_type_id { get; set; }

        public int employee_id { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }

        public virtual ms_employee ms_employee { get; set; }

        public virtual ms_user_type ms_user_type { get; set; }

    }
}
