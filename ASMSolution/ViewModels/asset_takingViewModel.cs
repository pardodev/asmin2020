using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ASM_UI;
using System.Web.Mvc;

namespace ASM_UI.Models
{
    public class asset_takingViewModel
    {
        public asset_takingViewModel()
        {
            asset_registration_list = new List<tr_asset_registration>();
            asset_taking_detail_list = new List<tr_asset_taking_detail>();
            location_list = new List<ms_asset_location>();
            location_reg_list = new List<ms_asset_register_location>();
        }

        [Key]
        public int asset_taking_id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Taking Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? asset_taking_date { get; set; }

        [Display(Name = "Period Year")]
        [Required(ErrorMessage = "Period Year is mandatory")]
        public int? period_year { get; set; }
        public IEnumerable<SelectListItem> period_year_list { get; set; }

        [Display(Name = "Period Month")]
        [Required(ErrorMessage = "Period Month is mandatory")]
        public int? period_month { get; set; }
        public IEnumerable<SelectListItem> period_month_list { get; set; }

        public int? company_id { get; set; }

        [Display(Name = "Company")]
        public string company_name { get; set; }

        //Asset Taking Detail
        public List<tr_asset_registration> asset_registration_list { get; set; }
        public List<tr_asset_taking_detail> asset_taking_detail_list { get; set; }

        public virtual tr_asset_registration asset_register { get; set; }

        public int? taking_parent_id { get; set; }

        public List<ms_asset_register_location> location_reg_list { get; set; }
        public int? location_reg_id { get; set; }
        public List<ms_asset_location> location_list { get; set; }
        [Display(Name = "Asset Taking Location")]
        [Required(ErrorMessage = "Asset Location is mandatory")]
        public int? location_id { get; set; }
        public int? location_id2 { get; set; }

        [Display(Name = "Location")]
        public string location_name { get; set; }

        public int? department_id { get; set; }

        [Display(Name = "Department")]
        public string dept_name { get; set; }

        public int? employee_id { get; set; }

        [Display(Name = "Employee")]
        public string employee_name { get; set; }

        public int asset_id { get; set; }

        public int? asset_status_id { get; set; }
        public string asset_status_name { get; set; }

        [Display(Name = "Asset Number")]
        public string asset_number { get; set; }

        [Display(Name = "Asset Name")]
        public string asset_name { get; set; }

        [Display(Name = "Asset Taking File")]
        public string file_name { get; set; }

        public bool? fl_available_asset { get; set; }

        [Display(Name = "Processed by")]
        public string created_name { get; set; }

        public bool? fl_submit_data { get; set; }

        public int? process_id { get; set; }
    }
}