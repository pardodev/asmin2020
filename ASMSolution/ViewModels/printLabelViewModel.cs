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
    public class printLabelViewModel
    {

        [Display(Name = "Company")]
        //[Required(ErrorMessage = "Company is mandatory")]
        public int? company_id { get; set; }
        public virtual ms_asmin_company company { get; set; }
        public List<ms_asmin_company> company_list { get; set; }


        [Display(Name = "Asset Taking Location")]
        //[Required(ErrorMessage = "Register Location is mandatory")]
        public int? asset_reg_location_id { get; set; }
        public virtual ms_asset_register_location asset_reg_location { get; set; }
        public List<ms_asset_register_location> asset_reg_location_list { get; set; }


        [Display(Name = "Location")]
        //[Required(ErrorMessage = "Location is mandatory")]
        public int? asset_location_id { get; set; }
        public virtual ms_asset_location asset_location { get; set; }
        public List<ms_asset_location> asset_location_list { get; set; }


        [Display(Name = "Category")]
        //[Required(ErrorMessage = "Category is mandatory")]
        public int? category_id { get; set; }
        public virtual ms_asset_category asset_category { get; set; }
        public List<ms_asset_category> asset_category_list { get; set; }


        [Display(Name = "Department")]
        //[Required(ErrorMessage = "Department is mandatory")]
        public int? department_id { get; set; }
        public virtual ms_department department { get; set; }
        public List<ms_department> department_list { get; set; }



        public List<Print_Asset_Items> print_asset_items { get; set; }

        public string[] checked_asset_id { get; set; }
        public string print_message { get; set; }

    }

    public class Print_Asset_Items
    {
        public int asset_id { get; set; }
        //public virtual tr_asset_registration tr_asset_registration { get; set; }

        [Display(Name = "Asset No")]
        public string asset_number { get; set; }

        [Display(Name = "Asset Nam")]
        public string asset_name { get; set; }

        public string location_code { get; set; }

        public int? category_id { get; set; }
        //public virtual ms_asset_category asset_category { get; set; }
        [Display(Name = "Category")]
        public string category_name { get; set; }

        public int? department_id { get; set; }
        //public virtual ms_department department { get; set; }
        [Display(Name = "Department")]
        public string department_name { get; set; }

        public string Value { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }

    }
        
}