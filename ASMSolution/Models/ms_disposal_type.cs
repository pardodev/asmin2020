namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ms_disposal_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ms_disposal_type()
        {
            tr_disposal_announcement = new HashSet<tr_disposal_announcement>();
        }

        [Key]
        public int disposal_type_id { get; set; }

        [StringLength(20)]
        public string disposal_type_code { get; set; }

        [StringLength(50)]
        public string disposal_type_name { get; set; }

        public int? disposal_by_dept_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tr_disposal_announcement> tr_disposal_announcement { get; set; }
    }
}
