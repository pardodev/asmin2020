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
    public class subasset_registrationViewModel
    {
        public subasset_registrationViewModel()
        {

            asset_type_id = (int)Enum_asset_type_Key.AssetChild;  // menandakan sub asset
            asset_parent_list = new List<tr_asset_registration>();
            company_list = new List<ms_asmin_company>();
            asset_reg_location_list = new List<ms_asset_register_location>();
            asset_reg_pic_list = new List<ms_asset_register_pic>();
            asset_category_list = new List<ms_asset_category>();
            vendor_list = new List<ms_vendor>();
            asset_location_list = new List<ms_asset_location>();
            department_list = new List<ms_department>();
            employee_list = new List<ms_employee>();

            tr_asset_images = new tr_asset_image()
            {
                asset_id = 0,
                asset_img_address = base_image_path
            };
        }

        [Key]
        [Display(Name = "Asset ID")]
        public int asset_id { get; set; }

        /// <summary>
        /// 1 : parent
        /// 2 : sub asset (child)
        /// </summary>
        [Display(Name = "Asset Type")]
        public int? asset_type_id { get; set; }
        public virtual ms_asset_type asset_type { get; set; }


        [Display(Name = "Asset Parent Number")]
        public int asset_parent_id { get; set; }
        [Display(Name = "Asset Parent Name")]
        public string asset_parent_name { get; set; }
        public virtual tr_asset_registration asset_parent { get; set; }
        public List<tr_asset_registration> asset_parent_list { get; set; }

        [Display(Name = "Asset Parent Number")]
        public string asset_parent_number { get; set; }

        [StringLength(30)]
        [Display(Name = "Sub-asset Number")]
        public string asset_number { get; set; }


        [Display(Name = "Company")]
        public int? company_id { get; set; }

        public virtual ms_asmin_company company { get; set; }

        public List<ms_asmin_company> company_list { get; set; }


        [Display(Name = "Register Location")]
        public int? asset_reg_location_id { get; set; }

        public virtual ms_asset_register_location asset_reg_location { get; set; }

        public List<ms_asset_register_location> asset_reg_location_list { get; set; }


        [Display(Name = "Register PIC")]
        public int? asset_reg_pic_id { get; set; }

        public virtual ms_asset_register_pic asset_reg_pic { get; set; }

        public List<ms_asset_register_pic> asset_reg_pic_list { get; set; }


        [Display(Name = "Category")]
        public int? category_id { get; set; }

        public virtual ms_asset_category asset_category { get; set; }

        public List<ms_asset_category> asset_category_list { get; set; }


        [StringLength(50)]
        [Display(Name = "Purchase Order No")]
        public string asset_po_number { get; set; }

        [StringLength(50)]
        [Display(Name = "Delivery Order No")]
        public string asset_do_number { get; set; }

        [StringLength(100)]
        [Display(Name = "Sub-asset Name")]
        public string asset_name { get; set; }

        [StringLength(100)]
        [Display(Name = "Sub-asset Brand")]
        public string asset_merk { get; set; }

        [StringLength(50)]
        [Display(Name = "Sub-asset Serial")]
        public string asset_serial_number { get; set; }


        [Display(Name = "Vendor")]
        [Required(ErrorMessage = "Vendor is mandatory")]
        public int? vendor_id { get; set; }

        public virtual ms_vendor vendor { get; set; }

        public List<ms_vendor> vendor_list { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Receipt Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? asset_receipt_date { get; set; }

        [Display(Name = "Sub-asset Location")]
        public int? location_id { get; set; }

        public int? current_location_id { get; set; }

        public virtual ms_asset_location asset_location { get; set; }

        public List<ms_asset_location> asset_location_list { get; set; }


        [Display(Name = "Department")]
        public int? department_id { get; set; }

        public int? current_department_id { get; set; }

        public virtual ms_department department { get; set; }

        public List<ms_department> department_list { get; set; }


        [Display(Name = "Employee")]
        public int? employee_id { get; set; }

        public int? current_employee_id { get; set; }

        public virtual ms_employee employee { get; set; }

        public List<ms_employee> employee_list { get; set; }


        [StringLength(200)]
        [Display(Name = "Sub-asset Description")]
        public string asset_description { get; set; }


        public string base_image_path { get; set; }

        [Display(Name = "Upload File")]
        [StringLength(200)]
        public string asset_img_file { get; set; }

        ////[Required]
        //[Display(Name = "Upload File")]
        //public HttpPostedFileBase asset_file_attach { get; set; }

        public virtual tr_asset_image tr_asset_images { get; set; }

        public bool hasImage { get; set; }

        public EnumFormModeKey FormMode { get; set; }

        //QRCode
        [Display(Name = "QRCode Text")]
        public string QRCodeText { get; set; }
        [Display(Name = "QRCode Image")]
        public string QRCodeImagePath { get; set; }

        public static string path_file_qrcode
        {
            get
            {
                return "~/Images/QRCode/";
            }
        }
        public static string path_file_asset
        {
            get
            {
                return "~/Content/AssetImage/";
            }
        }
    }

}