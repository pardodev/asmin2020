using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASM_UI;

namespace ASM_UI.Models
{

    /* 
     * karena viewmodel report kecil2, maka gak perlu dibuat sendiri sendiri
     * semua view model mengenai report taruh disini
     * 
     */

    /// <summary>
    /// Report AssetByLocation viewmodel
    /// </summary>
    public class Report_AssetByLocationViewModel
    {

        public int? id { get; set; }
        public string report_rdlc { get; set; }
        public string report_title { get; set; }
        public string report_subtitle { get; set; }
        public EnumDisplayFormatKey display_format { get; set; }


        [Display(Name = "Company")]
        //[Required(ErrorMessage = "Company is mandatory")]
        public int? company { get; set; }

        public virtual ms_asmin_company ms_company { get; set; }

        public List<ms_asmin_company> ms_company_list { get; set; }

        public bool enabled_company { get; set; }



        [Display(Name = "District")]
        //[Required(ErrorMessage = "District Location is mandatory")]
        public int? asset_register { get; set; }

        public virtual ms_asset_register_location ms_asset_register_location { get; set; }

        public List<ms_asset_register_location> ms_asset_register_location_list { get; set; }

        public bool enabled_distric { get; set; }



        [Display(Name = "Location")]
        //[Required(ErrorMessage = "Please select location")]
        public int? location_id { get; set; }

        public virtual ms_asset_location asset_location { get; set; }

        public virtual List<ms_asset_location> asset_location_list { get; set; }

        [Display(Name = "Consolidation")]
        public bool flag_consolidation { get; set; }

        

        [DataType(DataType.Date)]
        [Display(Name = "Periode Start")]
        //[Required(ErrorMessage = "Start Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? start_date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Periode End")]
        [Required(ErrorMessage = "End Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? end_date { get; set; }

        [Display(Name = "Financial Statements")]
        [Required(ErrorMessage = "Please select financial statements")]
        public int? fin_statement_id { get; set; }
        public IEnumerable<SelectListItem> fin_statement_list { get; set; }

        public string error_message { get; set; }

    }

    public enum EnumDisplayFormatKey
    {
        WEB,
        PDF
    }


    public class Report_AssetBarcodeViewModel
    {
        public Report_AssetBarcodeViewModel()
        {
            report_id = 12;
        }

        [Display(Name = "Report Id")]
        public int? report_id { get; set; }



    }

    /// <summary>
    /// Asset Listing
    /// </summary>
    public class Report_AssetListingViewModel
    {
        private USER_PROFILE UserProfile = (USER_PROFILE)System.Web.HttpContext.Current.Session["USER_PROFILE"];

        public Report_AssetListingViewModel()
        {
            this.report_title = "Asset Listing Report";
            this.report_rdlc = "rpt01_AssetListing.rdlc";
            this.pmonth = DateTime.Now.Month;
            this.pyear = DateTime.Now.Year;
            this.company = UserProfile.company_id;
            this.asset_register = UserProfile.asset_reg_location_id;
        }

        public int? id { get; set; }
        public string report_rdlc { get; set; }
        public string report_title { get; set; }
        public string report_subtitle { get; set; }

        [Display(Name = "Company")]
        [Required(ErrorMessage = "Company is mandatory")]
        public int? company { get; set; }

        public virtual ms_asmin_company ms_company { get; set; }

        public List<ms_asmin_company> ms_company_list { get; set; }


        //[Display(Name = "Asset Location")]
        //[Required(ErrorMessage = "Asset Location is mandatory")]
        //public int? location { get; set; }

        //public virtual ms_asset_location ms_asset_location { get; set; }

        //public List<ms_asset_location> ms_asset_location_list { get; set; }



        [Display(Name = "Register Location")]
        [Required(ErrorMessage = "Register Location is mandatory")]
        public int? asset_register { get; set; }

        public virtual ms_asset_register_location ms_asset_register_location { get; set; }

        public List<ms_asset_register_location> ms_asset_register_location_list { get; set; }



        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is mandatory")]
        public int? category { get; set; }

        public virtual ms_asset_category ms_asset_category { get; set; }

        public List<ms_asset_category> ms_category_list { get; set; }


        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year is mandatory")]
        public int pyear { get; set; }
        public IEnumerable<SelectListItem> period_year_list { get; set; }

        [Display(Name = "Month")]
        [Required(ErrorMessage = "Month is mandatory")]
        public int pmonth { get; set; }
        public IEnumerable<SelectListItem> period_month_list { get; set; }

    }


    public class Report_AssetTakingViewModel
    {
        public Report_AssetTakingViewModel()
        {
            //report_id = 2;
            asset_location_list = new List<ms_asset_location>();
            fin_statement_list = new List<SelectListItem>();
        }
        [Display(Name = "Report Id")]
        public int? report_id { get; set; }

        [Display(Name = "Company Name")]
        public string company_name { get; set; }

        [Display(Name = "District")]
        public string location_reg_name { get; set; }

        [Display(Name = "Location")]
        //[Required(ErrorMessage = "Please select location")]
        public int? location_id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Periode Start")]
        [Required(ErrorMessage = "Asset Taking Start Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? start_date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Periode End")]
        [Required(ErrorMessage = "AssetTaking End Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? end_date { get; set; }

        [Display(Name = "Financial Statements")]
        [Required(ErrorMessage = "Please select financial statements")]
        public bool? fin_statement_id { get; set; }
        public IEnumerable<SelectListItem> fin_statement_list { get; set; }

        [Display(Name = "Consolidation")]
        public bool fl_consolidated { get; set; }

        public string error_message { get; set; }

        public virtual ms_asset_location asset_location { get; set; }

        public virtual List<ms_asset_location> asset_location_list { get; set; }

    }


    public class Report_AssetRPT03ViewModel
    {

        public Report_AssetRPT03ViewModel()
        {
            report_id = 3;
        }
        [Display(Name = "Report Id")]
        public int? report_id { get; set; }



    }


    public class Report_AssetRPT04AssetMutationViewModel
    {

        public Report_AssetRPT04AssetMutationViewModel()
        {
            //report_id = 5;
            asset_location_list = new List<ms_asset_location>();
            //fin_statement_list = new List<SelectListItem>();
        }
        [Display(Name = "Report Id")]
        public int? report_id { get; set; }

        [Display(Name = "Company Name")]
        public string company_name { get; set; }

        [Display(Name = "District")]
        public string location_reg_name { get; set; }

        [Display(Name = "Location")]
        //[Required(ErrorMessage = "Please select location")]
        public int? location_id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Periode Start")]
        [Required(ErrorMessage = "Mutation Start Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? mutation_start_date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Periode End")]
        [Required(ErrorMessage = "Mutation End Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? mutation_end_date { get; set; }

        //[Display(Name = "Financial Statements")]
        //[Required(ErrorMessage = "Please select financial statements")]
        //public bool? fin_statement_id { get; set; }
        //public IEnumerable<SelectListItem> fin_statement_list { get; set; }

        [Display(Name = "Consolidation")]
        public bool fl_consolidated { get; set; }

        public string mutation_message { get; set; }

        public virtual ms_asset_location asset_location { get; set; }

        public virtual List<ms_asset_location> asset_location_list { get; set; }
    }

    public class Report_AssetDisposalViewModel
    {

        public Report_AssetDisposalViewModel()
        {
            //report_id = 5;
            asset_location_list = new List<ms_asset_location>();
            fin_statement_list = new List<SelectListItem>();
        }
        [Display(Name = "Report Id")]
        public int? report_id { get; set; }

        [Display(Name = "Company Name")]
        public string company_name { get; set; }

        [Display(Name = "District")]
        public string location_reg_name { get; set; }

        [Display(Name = "Location")]
        //[Required(ErrorMessage = "Please select location")]
        public int? location_id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Periode Start")]
        [Required(ErrorMessage = "Disposal Start Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? disposal_start_date { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Periode End")]
        [Required(ErrorMessage = "Disposal End Date is mandatory")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? disposal_end_date { get; set; }

        [Display(Name = "Financial Statements")]
        [Required(ErrorMessage = "Please select financial statements")]
        public bool? fin_statement_id { get; set; }
        public IEnumerable<SelectListItem> fin_statement_list { get; set; }

        [Display(Name = "Consolidation")]
        public bool fl_consolidated { get; set; }

        public string disposal_message { get; set; }

        public virtual ms_asset_location asset_location { get; set; }

        public virtual List<ms_asset_location> asset_location_list { get; set; }
    }

    public class Report_AssetRPT06ViewModel
    {

        public Report_AssetRPT06ViewModel()
        {
            report_id = 6;
        }
        [Display(Name = "Report Id")]
        public int? report_id { get; set; }



    }

    public class Report_AssetRPT07ViewModel
    {

        public Report_AssetRPT07ViewModel()
        {
            report_id = 7;
        }
        [Display(Name = "Report Id")]
        public int? report_id { get; set; }



    }


    public class Report_AssetRPT08ViewModel
    {

        public Report_AssetRPT08ViewModel()
        {
            report_id = 8;
        }
        [Display(Name = "Report Id")]
        public int? report_id { get; set; }



    }

    public class Report_AssetRPT09ViewModel
    {

        public Report_AssetRPT09ViewModel()
        {
            report_id = 9;
        }
        [Display(Name = "Report Id")]
        public int? report_id { get; set; }



    }

    public class Report_AssetRPT10ViewModel
    {

        public Report_AssetRPT10ViewModel()
        {
            report_id = 10;
        }
        [Display(Name = "Report Id")]
        public int? report_id { get; set; }



    }



}