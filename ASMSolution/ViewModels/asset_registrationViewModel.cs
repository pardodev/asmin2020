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
    public class asset_registrationViewModel
    {

        public asset_registrationViewModel()
        {

            asset_type_id = (int)Enum_asset_type_Key.AssetParent;
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
            asset_quantity = 1;
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

        //[Display(Name = "Asset Parent")]
        //public int? asset_parent_id { get; set; }
        //public virtual tr_asset_registration asset_parent { get; set; }


        [StringLength(30)]
        [Display(Name = "Asset Number")]
        public string asset_number { get; set; }


        [Display(Name = "Company")]
        [Required(ErrorMessage = "Company is mandatory")]
        public int? company_id { get; set; }

        public virtual ms_asmin_company company { get; set; }

        public List<ms_asmin_company> company_list { get; set; }


        [Display(Name = "Register Location")]
        [Required(ErrorMessage = "Register Location is mandatory")]
        public int? asset_reg_location_id { get; set; }

        public virtual ms_asset_register_location asset_reg_location { get; set; }

        public List<ms_asset_register_location> asset_reg_location_list { get; set; }


        [Display(Name = "Register PIC")]
        [Required(ErrorMessage = "Register PIC is mandatory")]
        public int? asset_reg_pic_id { get; set; }

        public virtual ms_asset_register_pic asset_reg_pic { get; set; }

        public List<ms_asset_register_pic> asset_reg_pic_list { get; set; }


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is mandatory")]
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
        [Display(Name = "Asset Name")]
        [Required(ErrorMessage = "Asset Name is mandatory")]
        public string asset_name { get; set; }

        [StringLength(100)]
        [Display(Name = "Asset Brand")]
        public string asset_merk { get; set; }

        [StringLength(50)]
        [Display(Name = "Asset Serial")]
        public string asset_serial_number { get; set; }

        [Display(Name = "Vendor")]
        [Required(ErrorMessage = "Vendor is mandatory")]
        public int? vendor_id { get; set; }

        public virtual ms_vendor vendor { get; set; }

        public List<ms_vendor> vendor_list { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "If empty, system will set to 1")]
        [Range(0, 200)]
        public int? asset_quantity { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Receipt Date")]
        [Required(ErrorMessage = "Receipt Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? asset_receipt_date { get; set; }


        [Display(Name = "Asset Location")]
        [Required(ErrorMessage = "Asset Location is mandatory")]
        public int? location_id { get; set; }

        public virtual ms_asset_location asset_location { get; set; }

        public List<ms_asset_location> asset_location_list { get; set; }


        [Display(Name = "Department")]
        //[Required(ErrorMessage = "Department is mandatory")]
        public int? department_id { get; set; }

        public virtual ms_department department { get; set; }

        public List<ms_department> department_list { get; set; }


        [Display(Name = "Employee")]
        //[Required(ErrorMessage = "Employee is mandatory")]
        public int? employee_id { get; set; }

        public virtual ms_employee employee { get; set; }

        public List<ms_employee> employee_list { get; set; }


        [StringLength(200)]
        [Display(Name = "Asset Description")]
        public string asset_description { get; set; }


        public string base_image_path { get; set; }

        //[Required]
        [Display(Name = "Upload File")]
        [StringLength(200)]
        public string asset_img_file { get; set; }

        public int? org_id { get; set; }

        ////[Required]
        //[Display(Name = "Upload File")]
        //public HttpPostedFileBase asset_file_attach { get; set; }

        public virtual tr_asset_image tr_asset_images { get; set; }


        public EnumFormModeKey FormMode { get; set; }

        public bool hasImage { get; set; }

        //For search Asset details
        public string company_register { get; set; }
        public string location_register { get; set; }
        public string pic_register { get; set; }
        public string category_register { get; set; }
        public string vendor_name { get; set; }
        public string location_asset { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public Decimal original_price { get; set; }

        public string department_asset { get; set; }

        [Display(Name = "Usage Life in Months")]
        public string usage_life_time_fiskal { get; set; }

        public string employee_name { get; set; }

        [Display(Name = "Book Value")]
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public Decimal fis_book_value { get; set; }

        //QRCode & Image Asset
        [Display(Name = "QRCode Text")]
        public string QRCodeText { get; set; }
        [Display(Name = "QRCode Image")]
        public string QRCodeImagePath { get; set; }
        public static string path_file_asset
        {
            get
            {
                return "~/Content/AssetImage/";
            }
        }
        public static string path_file_disposal
        {
            get
            {
                return "~/Content/DisposalImage/";
            }
        }
        public static string path_file_assettaking
        {
            get
            {
                return "~/Content/AssetTakingFile/";
            }
        }
        //end of qrcode
    }


    public enum Enum_asset_type_Key
    {
        AssetParent = 1,
        AssetChild = 2        
    }
    

    public class asset_registrationEditViewModel
    {

        public asset_registrationEditViewModel()
        {

            asset_type_id = (int)Enum_asset_type_Key.AssetParent;
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
            asset_quantity = 1;
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

        //[Display(Name = "Asset Parent")]
        //public int? asset_parent_id { get; set; }
        //public virtual tr_asset_registration asset_parent { get; set; }


        [StringLength(30)]
        [Display(Name = "Asset Number")]
        public string asset_number { get; set; }


        [Display(Name = "Company")]
        //[Required(ErrorMessage = "Company is mandatory")]
        public int? company_id { get; set; }

        public virtual ms_asmin_company company { get; set; }

        public List<ms_asmin_company> company_list { get; set; }


        [Display(Name = "Register Location")]
        //[Required(ErrorMessage = "Register Location is mandatory")]
        public int? asset_reg_location_id { get; set; }

        public virtual ms_asset_register_location asset_reg_location { get; set; }

        public List<ms_asset_register_location> asset_reg_location_list { get; set; }


        [Display(Name = "Register PIC")]
        //[Required(ErrorMessage = "Register PIC is mandatory")]
        public int? asset_reg_pic_id { get; set; }

        public virtual ms_asset_register_pic asset_reg_pic { get; set; }

        public List<ms_asset_register_pic> asset_reg_pic_list { get; set; }


        [Display(Name = "Category")]
        //[Required(ErrorMessage = "Category is mandatory")]
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
        [Display(Name = "Asset Name")]
        [Required(ErrorMessage = "Asset Name is mandatory")]
        public string asset_name { get; set; }

        [StringLength(100)]
        [Display(Name = "Asset Brand")]
        public string asset_merk { get; set; }

        [StringLength(50)]
        [Display(Name = "Asset Serial")]
        public string asset_serial_number { get; set; }

        [Display(Name = "Vendor")]
        [Required(ErrorMessage = "Vendor is mandatory")]
        public int? vendor_id { get; set; }

        public virtual ms_vendor vendor { get; set; }

        public List<ms_vendor> vendor_list { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "If empty, system will set to 1")]
        [Range(0, 200)]
        public int? asset_quantity { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Receipt Date")]
        [Required(ErrorMessage = "Receipt Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? asset_receipt_date { get; set; }


        [Display(Name = "Asset Location")]
        //[Required(ErrorMessage = "Asset Location is mandatory")]
        public int? location_id { get; set; }

        public virtual ms_asset_location asset_location { get; set; }

        public List<ms_asset_location> asset_location_list { get; set; }


        [Display(Name = "Department")]
        //[Required(ErrorMessage = "Department is mandatory")]
        public int? department_id { get; set; }

        public virtual ms_department department { get; set; }

        public List<ms_department> department_list { get; set; }


        [Display(Name = "Employee")]
        //[Required(ErrorMessage = "Employee is mandatory")]
        public int? employee_id { get; set; }

        public virtual ms_employee employee { get; set; }

        public List<ms_employee> employee_list { get; set; }


        [StringLength(200)]
        [Display(Name = "Asset Description")]
        public string asset_description { get; set; }


        public string base_image_path { get; set; }

        //[Required]
        [Display(Name = "Upload File")]
        [StringLength(200)]
        public string asset_img_file { get; set; }

        public int? org_id { get; set; }

        ////[Required]
        //[Display(Name = "Upload File")]
        //public HttpPostedFileBase asset_file_attach { get; set; }

        public virtual tr_asset_image tr_asset_images { get; set; }


        public EnumFormModeKey FormMode { get; set; }

        public bool hasImage { get; set; }

        //For search Asset details
        public string company_register { get; set; }
        public string location_register { get; set; }
        public string pic_register { get; set; }
        public string category_register { get; set; }
        public string vendor_name { get; set; }
        public string location_asset { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public Decimal original_price { get; set; }

        public string department_asset { get; set; }

        [Display(Name = "Usage Life in Months")]
        public string usage_life_time_fiskal { get; set; }

        public string employee_name { get; set; }

        [Display(Name = "Book Value")]
        [DisplayFormat(DataFormatString = "{0:#,###.##}", ApplyFormatInEditMode = true)]
        public Decimal fis_book_value { get; set; }

        //QRCode & Image Asset
        [Display(Name = "QRCode Text")]
        public string QRCodeText { get; set; }
        [Display(Name = "QRCode Image")]
        public string QRCodeImagePath { get; set; }
        public static string path_file_asset
        {
            get
            {
                return "~/Content/AssetImage/";
            }
        }
        public static string path_file_disposal
        {
            get
            {
                return "~/Content/DisposalImage/";
            }
        }
        public static string path_file_assettaking
        {
            get
            {
                return "~/Content/AssetTakingFile/";
            }
        }
        //end of qrcode
    }

}