namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_asset_activa
    {
        [Key]
        public int activa_id { get; set; }

        public int? activa_type_id { get; set; }

        [StringLength(30)]
        public string activa_number { get; set; }

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

        public int? currency_id { get; set; }

        public decimal? asset_po_price { get; set; }

        public decimal? asset_value { get; set; }

        public int? vendor_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? asset_receipt_date { get; set; }

        public int? location_id { get; set; }

        public int? department_id { get; set; }

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
    }
}
