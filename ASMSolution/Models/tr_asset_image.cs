namespace ASM_UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tr_asset_image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int asset_id { get; set; }

        [Required]
        [StringLength(200)]
        public string asset_img_address { get; set; }

        [Column(TypeName = "image")]
        public byte[] asset_qrcode { get; set; }
    }
}
