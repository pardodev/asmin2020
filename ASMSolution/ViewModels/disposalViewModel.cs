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
    public class LastApprovalDTO
    {
        public int request_id { get; set; }
        public int approval_id { get; set; }
        public int? approval_suggestion_id { get; set; }
        public int? approval_status_id { get; set; }
        public int? approval_level_id { get; set; }
    }

    public class disposalViewModel
    {
        public disposalViewModel()
        {
            disposal_type_list = new List<ms_disposal_type>();
            disposal_Approval_list = new List<disposalappViewModel>();
        }

        #region "Disposal Request"
        [Key]
        public int request_id { get; set; }

        public virtual tr_disposal_request tr_disposal_request { get; set; }

        [Display(Name = "Disposal Number")]
        public string disposal_number { get; set; }

        public int? job_level_id { get; set; }

        public int? request_emp_id { get; set; }

        public int? request_dept_id { get; set; }

        public int? request_location_id { get; set; }

        [Display(Name = "Request Description")]
        public string request_description { get; set; }

        [Display(Name = "Approve")]
        public bool? fl_approval { get; set; }

        [Display(Name = "Approval Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? approval_date { get; set; }

        public int? request_status_id { get; set; }

        public string request_status_name { get; set; }

        [Display(Name = "Last Asset Image")]
        public string asset_img_address { get; set; }

        public string path_address { get; set; }

        public virtual tr_disposal_image tr_disposal_image { get; set; }
        #endregion

        #region Asset Register
        [Display(Name = "Asset ID")]
        public int? asset_id { get; set; }

        public virtual tr_asset_registration asset_parent { get; set; }
        public virtual tr_depreciation tr_depreciation { get; set; }

        [Display(Name = "Asset Number")]
        [Required(ErrorMessage = "Asset is mandatory")]
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
        public decimal? asset_book_value { get; set; }

        [Display(Name = "Kurs")]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public decimal? Currency_kurs { get; set; }

        [Display(Name = "Asset Value")]
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        public decimal? asset_original_value { get; set; }

        [Display(Name = "Company")]
        public int? company_id { get; set; }

        public virtual ms_asmin_company company { get; set; }

        [Display(Name = "Asset Location")]
        public int? location_id { get; set; }
        public string location_name { get; set; }

        public virtual ms_asset_location asset_location { get; set; }

        [Display(Name = "Department")]
        public int? department_id { get; set; }
        public string department_name { get; set; }

        public virtual ms_department department { get; set; }

        [Display(Name = "Employee")]
        public int? employee_id { get; set; }
        public string employee_name { get; set; }
        public string employee_email { get; set; }
        public string ip_address { get; set; }
        public virtual ms_employee ms_employee { get; set; }
        public virtual ms_user ms_user { get; set; }

        public virtual ms_employee employee { get; set; }

        [Display(Name = "Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? request_date { get; set; }
        #endregion

        #region "Disposal Approval"
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

        [Display(Name = "Suggestion")]
        public int? approval_suggestion_id { get; set; }

        public string approval_suggestion_name { get; set; }

        public virtual tr_disposal_approval tr_disposal_approval { get; set; }
        public List<ms_disposal_type> disposal_type_list { get; set; }
        public List<disposalappViewModel> disposal_Approval_list { get; set; }
        #endregion

        #region "Disposal Proses"
        public virtual tr_disposal_announcement tr_disposal_announcement { get; set; }

        public int announcement_id { get; set; }

        public int? approval_disposal_type_id { get; set; }

        [Display(Name = "Announcement Status")]
        public bool? fl_announcement_status { get; set; }

        [Display(Name = "Announcement Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? announcement_date { get; set; }

        [StringLength(300)]
        [Display(Name = "Description")]
        public string announcement_description { get; set; }

        [StringLength(300)]
        [Display(Name = "Documents")]
        public string announcement_upload_address { get; set; }

        [Display(Name = "Finance Status")]
        public bool? fl_fin_announcement { get; set; }

        public int? fin_announcement_dept_id { get; set; }

        [Display(Name = "Finance Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fin_announcement_date { get; set; }

        [StringLength(300)]
        public string fin_announcement_upload_address { get; set; }

        [Display(Name = "Suggestion Changes ?")]
        public bool? fl_SuggestionChanges { get; set; }

        [Display(Name = "Dispose Status")]
        public bool? fl_remove_asset { get; set; }

        [Display(Name = "Dispose Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? remove_asset_date { get; set; }

        [StringLength(500)]
        [Display(Name = "Dispose Description")]
        public string remove_asset_description { get; set; }

        public int? remove_asset_emp_id { get; set; }

        public int? remove_asset_dept_id { get; set; }
        #endregion

        public EnumFormModeKey FormMode { get; set; }
    }

    public class disposalappViewModel
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