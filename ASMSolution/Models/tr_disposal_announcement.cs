namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_disposal_announcement
    {
        [Key]
        public int announcement_id { get; set; }

        public int request_id { get; set; }

        public int? approval_disposal_type_id { get; set; }

        public bool? fl_announcement_status { get; set; }

        public bool? fl_suggestion_changes { get; set; }

        public DateTime? announcement_date { get; set; }

        [StringLength(300)]
        public string announcement_description { get; set; }

        [StringLength(300)]
        public string announcement_upload_address { get; set; }

        public int? announcement_emp_id { get; set; }

        public bool? fl_fin_announcement { get; set; }

        public int? fin_announcement_dept_id { get; set; }

        public DateTime? fin_announcement_date { get; set; }

        [StringLength(300)]
        public string fin_announcement_upload_address { get; set; }

        public int? fin_announcement_emp_id { get; set; }

        public bool? fl_remove_asset { get; set; }

        public DateTime? remove_asset_date { get; set; }

        [StringLength(500)]
        public string remove_asset_description { get; set; }

        public int? remove_asset_emp_id { get; set; }

        public int? remove_asset_dept_id { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }

        public virtual ms_disposal_type ms_disposal_type { get; set; }

        public virtual tr_disposal_request tr_disposal_request { get; set; }
    }
}
