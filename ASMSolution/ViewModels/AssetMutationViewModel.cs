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
    public class AssetMutationViewModel
    {
        public AssetMutationViewModel()
        {
            mutation_Approval_list = new List<mutationappViewModel>();
        }

        #region Mutation Request
        public virtual tr_mutation_request tr_mutation_request { get; set; }
        
        [Key]
        public int request_id { get; set; }

        [Display(Name = "Mutation Number")]
        public string request_code { get; set; }

        public DateTime? request_date { get; set; }

        public int? request_status { get; set; }
        
        public string request_status_name { get; set; }

        public int? request_emp_id { get; set; }

        public int? request_dept_id { get; set; }

        public int? request_location_id { get; set; }

        public int? request_level_id { get; set; }

        [Display(Name = "Employee")]
        [Required(ErrorMessage = "Transfer to Employee is mandatory")]
        public int? transfer_to_emp_id { get; set; }
        public string transfer_to_emp_name { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "Transfer to Department is mandatory")]
        public int? transfer_to_dept_id { get; set; }
        public string transfer_to_dept_name { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "Transfer to Location is mandatory")]
        public int? transfer_to_location_id { get; set; }
        public string transfer_to_location_name { get; set; }

        [Display(Name = "Approve")]
        public bool? fl_approval { get; set; }

        [Display(Name = "Approval Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? approval_date { get; set; }



        #endregion

        #region Asset Register
        public virtual tr_asset_registration asset_parent { get; set; }
        public virtual tr_depreciation tr_depreciation { get; set; }

        [Display(Name = "Asset ID")]
        [Required(ErrorMessage = "Asset Number is mandatory")]
        public int? asset_id { get; set; }
        
        [Display(Name = "Asset Number")]
        public string asset_number { get; set; }

        [Display(Name = "Asset Name")]
        public string asset_name { get; set; }

        [Display(Name = "Asset Receipt Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? asset_receipt_date { get; set; }

        [Display(Name = "Currency")]
        public string currency_code { get; set; }

        public virtual ms_currency ms_currency { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public decimal? asset_original_value { get; set; }

        [Display(Name = "Asset Value (in USD)")]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public decimal? asset_book_value { get; set; }

        [Display(Name = "Kurs")]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public decimal? currency_kurs { get; set; }

        [Display(Name = "Company")]
        public int? company_id { get; set; }

        public virtual ms_asmin_company company { get; set; }

        [Display(Name = "Asset Location")]
        //public int? location_id { get; set; }
        public int? current_location_id { get; set; }
        public string location_name { get; set; }

        public virtual ms_asset_location asset_location { get; set; }

        [Display(Name = "Department")]
        //public int? department_id { get; set; }
        public int? current_department_id { get; set; }
        public string department_name { get; set; }

        public virtual ms_department department { get; set; }

        [Display(Name = "Employee")]
        public int? employee_id { get; set; }
        public int? current_employee_id { get; set; }
        public string employee_name { get; set; }
        public string employee_email { get; set; }
        public string ip_address { get; set; }

        public virtual ms_employee employee { get; set; }
        #endregion

        #region "Mutation Approval"
        public virtual tr_mutation_approval tr_mutation_approval { get; set; }

        public int approval_id { get; set; }

        public int? approval_location_id { get; set; }

        public int? approval_dept_id { get; set; }

        public int? approval_employee_id { get; set; }

        [Display(Name = "Approval Name")]
        public string approval_employee_name { get; set; }

        public int? approval_level_id { get; set; }

        [Display(Name = "Approval Title")]
        public string approval_level_name { get; set; }

        public int? approval_status_id { get; set; }

        [Display(Name = "Approval Status")]
        public string approval_status_Name { get; set; }
        
        [StringLength(500)]
        [Display(Name = "Reject Reason")]
        public string approval_noted { get; set; }

        public List<mutationappViewModel> mutation_Approval_list { get; set; }
        #endregion

        #region Mutation Process
        public virtual tr_mutation_process process { get; set; }
        public virtual ms_courier courier { get; set; }

        public int? mutation_id { get; set; }

        [Display(Name = "Send By")]
        public int? courier_id { get; set; }
        [Display(Name = "PIC Confirmation")]
        public bool? fl_pic_asset_comfirm { get; set; }

        #endregion

        #region Mutation Receive
        [Display(Name = "Received")]
        public bool? fl_pic_asset_receive { get; set; }

        [Display(Name = "Feedback")]
        public string courier_description { get; set; }
        #endregion

        public EnumFormModeKey FormMode { get; set; }

        //public int asset_id { get; set; }
        //public string asset_number { get; set; }
        //public string asset_name { get; set; }
        //public DateTime asset_receipt_date { get; set; }

        //public int asset_original_currency_id { get; set; }
        //public string currency_code { get; set; }
        //public decimal asset_original_value { get; set; }
        //public decimal currency_kurs { get; set; }
        //public decimal asset_book_value { get; set; }

        //public string location_name { get; set; }
        //public string department_name { get; set; }
        //public string employee_name { get; set; }
    }

    public class mutationappViewModel
    {
        public int approval_id { get; set; }

        public int? approval_location_id { get; set; }

        public int? approval_dept_id { get; set; }

        public int? approval_employee_id { get; set; }

        [Display(Name = "Approval Name")]
        public string approval_employee_name { get; set; }

        public int? approval_level_id { get; set; }

        [Display(Name = "Approval Title")]
        public string approval_level_name { get; set; }

        public int? approval_status_id { get; set; }

        [Display(Name = "Approval Status")]
        public string approval_status_Name { get; set; }

        [Display(Name = "Approval Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? approval_date { get; set; }

        [StringLength(500)]
        [Display(Name = "Reject Reason")]
        public string approval_noted { get; set; }

        [Display(Name = "Suggestion")]
        public int? approval_suggestion_id { get; set; }

        public string approval_suggestion_name { get; set; }
    }
}