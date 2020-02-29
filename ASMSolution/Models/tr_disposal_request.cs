namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_disposal_request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tr_disposal_request()
        {
            tr_disposal_announcement = new HashSet<tr_disposal_announcement>();
            tr_disposal_approval = new HashSet<tr_disposal_approval>();
            tr_disposal_image = new HashSet<tr_disposal_image>();
        }

        [Key]
        public int request_id { get; set; }

        public int? asset_id { get; set; }

        [StringLength(30)]
        public string disposal_number { get; set; }

        public DateTime? request_date { get; set; }

        public int? request_emp_id { get; set; }

        public int? request_dept_id { get; set; }

        public int? request_location_id { get; set; }

        [StringLength(300)]
        public string request_description { get; set; }

        public bool? fl_approval { get; set; }

        public DateTime? approval_date { get; set; }

        public int? request_status_id { get; set; }

        public DateTime? request_completed_date { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? update_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tr_disposal_announcement> tr_disposal_announcement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tr_disposal_approval> tr_disposal_approval { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tr_disposal_image> tr_disposal_image { get; set; }
    }
}
