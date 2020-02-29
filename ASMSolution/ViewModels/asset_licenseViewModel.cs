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
    public class asset_licenseViewModel
    {
        public asset_licenseViewModel()
        {
            asset_registration_list = new List<tr_asset_registration>();
            asset_license_list = new List<tr_asset_license>();
        }

        [Key]
        [Display(Name = "License ID")]
        public int license_id { get; set; }

        [Display(Name = "Asset Number")]
        public int asset_parent_id { get; set; }

        [Display(Name = "Asset Name")]
        public virtual tr_asset_registration asset_parent { get; set; }

        public List<tr_asset_registration> asset_registration_list { get; set; }

        public List<tr_asset_license> asset_license_list { get; set; }

        [StringLength(50)]
        [Display(Name = "License Number")]
        [Required(ErrorMessage = "License Number is mandatory")]
        public string license_number { get; set; }

        [StringLength(50)]
        [Display(Name = "License Name")]
        [Required(ErrorMessage = "License Name is mandatory")]
        public string license_name { get; set; }

        [StringLength(100)]
        [Display(Name = "License Issued By")]
        [Required(ErrorMessage = "License Issued By is mandatory")]
        public string license_issued_by { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "License Start Date")]
        [Required(ErrorMessage = "License Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? license_date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "License Expired Date")]
        [Required(ErrorMessage = "License Expired Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? license_exp_date { get; set; }

        [StringLength(300)]
        [Display(Name = "License Description")]
        public string license_description { get; set; }

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