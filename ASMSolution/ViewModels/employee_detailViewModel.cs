using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.Mvc;

namespace ASM_UI.Models
{
    public class employee_setupViewModel
    {
        public employee_setupViewModel()
        {
            this.employee_details = new List<employee_detailViewModel>();
            this.checkbox_approver = new List<SelectedApprover_CheckBoxes>();
            this.FormMode = EnumFormModeKey.Form_New;
        }

        public int employee_id { get; set; }

        [StringLength(20)]
        [Display(Name = "NIK")]
        public string employee_nik { get { return (ms_employee != null) ? ms_employee.employee_nik : ""; } }

        [StringLength(50)]
        [Display(Name = "Name")]
        public string employee_name { get { return (ms_employee != null) ? ms_employee.employee_name : ""; } }

        [StringLength(50)]
        [Display(Name = "Email")]
        public string employee_email { get { return (ms_employee != null) ? ms_employee.employee_email : ""; } }
        
        [Display(Name = "IP Address")]
        public string ip_address { get { return (ms_employee != null) ? ms_employee.ip_address : ""; } }


        public virtual ms_employee ms_employee { get; set; }

        public List<employee_detailViewModel> employee_details { get; set; }

        public int[] selected_employee_id { get; set; }
        public int[] selected_company_id { get; set; }
        public int[] selected_department_id { get; set; }
        public int[] selected_job_level_id { get; set; }
        public int[] selected_user_type_id { get; set; }
        public int[] selected_register_id { get; set; }
        public int[] selected_location_id { get; set; }

        public int[] selected_fl_approver { get; set; }
        public int[] selected_range_id { get; set; }

        public IEnumerable<SelectListItem> sli_company_list { get; set; }
        public IEnumerable<SelectListItem> sli_department_list { get; set; }
        public IEnumerable<SelectListItem> sli_job_level_list { get; set; }
        public IEnumerable<SelectListItem> sli_user_type_list { get; set; }

        public IEnumerable<SelectListItem> sli_register_list { get; set; }
        public IEnumerable<SelectListItem> sli_location_list { get; set; }

        public List<SelectedApprover_CheckBoxes> checkbox_approver { get; set; }
        public IEnumerable<SelectListItem> sli_range_list { get; set; }

        public EnumFormModeKey FormMode { get; set; }

        public List<ms_asmin_company> company_list { get; set; }
    }

    public class employee_detailViewModel
    {
        [Display(Name = "Employee")]
        public int employee_id { get; set; }
        //public virtual ms_employee ms_employee { get; set; }

        [Display(Name = "Company")]
        public int selected_company_id { get; set; }
        public virtual ms_asmin_company ms_asmin_company { get; set; }
        public IEnumerable<SelectListItem> sli_company_list { get; set; }

        [Display(Name = "Department")]
        public int selected_department_id { get; set; }
        //public virtual ms_department ms_department { get; set; }
        public IEnumerable<SelectListItem> sli_department_list { get; set; }

        [Display(Name = "Level Job")]
        public int selected_job_level_id { get; set; }
        //public virtual ms_job_level ms_job_level { get; set; }
        public IEnumerable<SelectListItem> sli_job_level_list { get; set; }

        [Display(Name = "User Type")]
        public int selected_user_type_id { get; set; }
        //public virtual ms_user_type ms_user_type { get; set; }
        public IEnumerable<SelectListItem> sli_user_type_list { get; set; }

        [Display(Name = "Register")]
        public int selected_register_id { get; set; }
        public IEnumerable<SelectListItem> sli_register_list { get; set; }

        [Display(Name = "Location")]
        public int selected_location_id { get; set; }
        public IEnumerable<SelectListItem> sli_location_list { get; set; }

        [Display(Name = "Enable Approval?")]
        public int selected_fl_approver { get; set; }

        [Display(Name = "Approval Range")]
        public int selected_range_id { get; set; }
        //public virtual ms_approval_range ms_approval_range { get; set; }
        public IEnumerable<SelectListItem> sli_range_list { get; set; }

    }

    public class SelectedApprover_CheckBoxes
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }
    }

}