namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_asset_registration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tr_asset_registration()
        {
            tr_asset_insurance = new HashSet<tr_asset_insurance>();
            tr_asset_license = new HashSet<tr_asset_license>();
            tr_asset_registration1 = new HashSet<tr_asset_registration>();
            tr_asset_warranty = new HashSet<tr_asset_warranty>();
        }

        [Key]
        public int asset_id { get; set; }

        public int? asset_type_id { get; set; }

        public int? asset_parent_id { get; set; }

        [StringLength(30)]
        public string asset_number { get; set; }

        public int? company_id { get; set; }

        public int? asset_reg_location_id { get; set; }

        public int? asset_reg_pic_id { get; set; }

        public int? category_id { get; set; }

        [StringLength(50)]
        public string asset_po_number { get; set; }

        [StringLength(50)]
        public string asset_do_number { get; set; }

        [StringLength(100)]
        public string asset_name { get; set; }

        [StringLength(100)]
        public string asset_merk { get; set; }

        [StringLength(50)]
        public string asset_serial_number { get; set; }

        public int? vendor_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? asset_receipt_date { get; set; }

        public int? location_id { get; set; }

        public int? current_location_id { get; set; }

        public int? department_id { get; set; }

        public int? current_department_id { get; set; }

        public int? employee_id { get; set; }

        public int? current_employee_id { get; set; }

        [StringLength(200)]
        public string asset_description { get; set; }

        public bool? fl_active { get; set; }

        public DateTime? created_date { get; set; }

        public int? created_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

        public int? org_id { get; set; }

        public virtual ms_asmin_company ms_asmin_company { get; set; }

        public virtual ms_asset_category ms_asset_category { get; set; }

        public virtual ms_asset_location ms_asset_location { get; set; }

        public virtual ms_asset_register_location ms_asset_register_location { get; set; }

        public virtual ms_asset_register_pic ms_asset_register_pic { get; set; }

        public virtual ms_asset_type ms_asset_type { get; set; }

        public virtual ms_department ms_department { get; set; }

        public virtual ms_employee ms_employee { get; set; }

        public virtual ms_vendor ms_vendor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tr_asset_insurance> tr_asset_insurance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tr_asset_license> tr_asset_license { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tr_asset_registration> tr_asset_registration1 { get; set; }

        public virtual tr_asset_registration tr_asset_registration2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tr_asset_warranty> tr_asset_warranty { get; set; }
    }
}
