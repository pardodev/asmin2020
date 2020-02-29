using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ASM_UI;

namespace ASM_UI.Models
{
    public class asset_warrantyViewModel
    {
        public asset_warrantyViewModel()
        {
            asset_registration_list = new List<tr_asset_registration>();
            asset_warranty_list = new List<tr_asset_warranty>();
        }
         
        [Key]
        [Display(Name = "Warranty ID")]
        public int warranty_id { get; set; }

        [Display(Name = "Asset Number")]
        [Required(ErrorMessage = "Asset is mandatory")]
        public int asset_parent_id { get; set; }

        [Display(Name = "Asset Name")]
        public virtual tr_asset_registration asset_parent { get; set; }

        public List<tr_asset_registration> asset_registration_list { get; set; }
        public List<tr_asset_warranty> asset_warranty_list { get; set; }

        [StringLength(50)]
        [Display(Name = "Warranty Number")]
        [Required(ErrorMessage = "Warranty Number is mandatory")]
        public string warranty_number { get; set; }

        [StringLength(100)]
        [Display(Name = "Warranty Item Name")]
        [Required(ErrorMessage = "Warranty Name is mandatory")]
        public string warranty_item_name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Warranty Start Date")]
        [Required(ErrorMessage = "Warranty Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? warranty_date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Warranty Expired Date")]
        [Required(ErrorMessage = "Warranty Expired Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? warranty_exp_date { get; set; }

        [StringLength(300)]
        [Display(Name = "Warranty Description")]
        public string warranty_description { get; set; }

        public virtual ms_vendor ms_vendor { get; set; }

        [StringLength(30)]
        [Display(Name = "Asset Number")]
        public string asset_number { get; set; }

        [StringLength(100)]
        [Display(Name = "Asset Name")]
        public string asset_name { get; set; }

        [Display(Name = "Vendor")]
        public int? vendor_id { get; set; }

        [Display(Name = "Vendor Name")]
        public string vendor_name { get; set; }

        public EnumFormModeKey FormMode { get; set; }
    }
}