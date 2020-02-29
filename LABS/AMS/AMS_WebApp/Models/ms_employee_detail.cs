namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_employee_detail
    {
        [Key]
        public int emp_det_id { get; set; }

        public int? employee_id { get; set; }

        public int? company_id { get; set; }

        public int? department_id { get; set; }

        public int? job_level_id { get; set; }

        public int? user_type_id { get; set; }

        public bool? fl_approver { get; set; }

        public int? range_id { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }

        public virtual ms_approval_range ms_approval_range { get; set; }

        public virtual ms_asmin_company ms_asmin_company { get; set; }

        public virtual ms_department ms_department { get; set; }

        public virtual ms_employee ms_employee { get; set; }

        public virtual ms_job_level ms_job_level { get; set; }

        public virtual ms_user_type ms_user_type { get; set; }
    }
}
