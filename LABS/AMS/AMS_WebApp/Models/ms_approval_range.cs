namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_approval_range
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ms_approval_range()
        {
            ms_employee_detail = new HashSet<ms_employee_detail>();
        }

        [Key]
        public int range_id { get; set; }

        [StringLength(20)]
        public string range_type { get; set; }

        [StringLength(5)]
        public string range_code { get; set; }

        [Column(TypeName = "money")]
        public decimal? range_min { get; set; }

        [Column(TypeName = "money")]
        public decimal? range_max { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ms_employee_detail> ms_employee_detail { get; set; }
    }
}
