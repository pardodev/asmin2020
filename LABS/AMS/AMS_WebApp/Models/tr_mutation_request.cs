namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_mutation_request
    {
        [Key]
        public int request_id { get; set; }

        public int? asset_id { get; set; }

        public DateTime? request_date { get; set; }

        public int? request_emp_id { get; set; }

        public int? request_dept_id { get; set; }

        public int? request_location_id { get; set; }

        public int? request_level_id { get; set; }

        public int? transfer_to_location_id { get; set; }

        public int? transfer_to_dept_id { get; set; }

        public int? transfer_to_emp_id { get; set; }

        public bool? fl_approval { get; set; }

        public DateTime? approval_date { get; set; }

        public int? request_status { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }
    }
}
