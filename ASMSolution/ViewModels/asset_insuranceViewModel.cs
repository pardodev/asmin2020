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
    public class asset_insuranceViewModel
    {
        public asset_insuranceViewModel()
        {
            asset_registration_list = new List<tr_asset_registration>();
            asset_insurance_list = new List<tr_asset_insurance>();
            insurance_list = new List<ms_insurance>();
            detail_insurance_list = new List<asset_insurancedetailViewModel>();
        }

        [Key]
        [Display(Name = "Insurance Activa ID")]
        public int insurance_activa_id { get; set; }

        [Display(Name = "Asset Number")]
        public int asset_parent_id { get; set; }

        [Display(Name = "Asset Name")]
        public virtual tr_asset_registration asset_parent { get; set; }

        public List<tr_asset_registration> asset_registration_list { get; set; }

        public List<tr_asset_insurance> asset_insurance_list { get; set; }

        public List<asset_insurancedetailViewModel> detail_insurance_list { get; set; }

        [StringLength(50)]
        [Display(Name = "Insurance Number")]
        [Required(ErrorMessage = "Insurance Activa Number is mandatory")]
        public string insurance_activa_number { get; set; }

        [StringLength(100)]
        [Display(Name = "Insurance Name")]
        [Required(ErrorMessage = "Insurance Activa Name is mandatory")]
        public string insurance_activa_name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Insurance Date")]
        [Required(ErrorMessage = "Insurance Activa Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? insurance_activa_date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Insurance Expired Date")]
        [Required(ErrorMessage = "Insurance Activa Expired Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? insurance_activa_exp_date { get; set; }

        [Display(Name = "Insurance Company")]
        [Required(ErrorMessage = "Insurance Company is mandatory")]
        public int? insurance_id { get; set; }

        [StringLength(300)]
        [Display(Name = "Insurance Description")]
        public string insurance_activa_description { get; set; }

        public virtual ms_vendor ms_vendor { get; set; }

        public virtual ms_insurance ms_insurance { get; set; }
        public List<ms_insurance> insurance_list { get; set; }

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

        [StringLength(100)]
        [Display(Name = "Insurance Company Name")]
        public string insurance_name { get; set; }

        public EnumFormModeKey FormMode { get; set; }
    }

    public class asset_insurancedetailViewModel
    {
        public int insurance_activa_id { get; set; }

        public int? asset_id { get; set; }

        [StringLength(50)]
        public string insurance_activa_number { get; set; }

        [StringLength(100)]
        public string insurance_activa_name { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? insurance_activa_date { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? insurance_activa_exp_date { get; set; }

        public int? insurance_id { get; set; }

        public string insurance_name { get; set; }

        [StringLength(300)]
        public string insurance_activa_description { get; set; }
    }
}