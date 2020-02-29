namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_disposal_approval
    {
        [Key]
        public int approval_id { get; set; }

        public int? request_id { get; set; }

        public DateTime? approval_date { get; set; }

        public int? approval_location_id { get; set; }

        public int? approval_dept_id { get; set; }

        public int? approval_employee_id { get; set; }

        public int? approval_level_id { get; set; }

        public int? approval_status_id { get; set; }

        [StringLength(500)]
        public string approval_noted { get; set; }

        public int? approval_suggestion_id { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deteled_by { get; set; }

        public int? org_id { get; set; }
    }
}
