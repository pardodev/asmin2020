#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

#endregion

namespace ASM_UI.Models
{
    //public class AccountLoginModel
    //{
    //    [Required]
    //    [EmailAddress]
    //    public string Email { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }

    //    public string ReturnUrl { get; set; }
    //    public bool RememberMe { get; set; }
    //}

    public class AccountLoginViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "User Name or NIK")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int company_id { get; set; }

        public virtual List<ms_asmin_company> company_list { get; set; }


        [Required]
        [Display(Name = "Register Location")]
        public int asset_reg_location_id { get; set; }

        public virtual List<ms_asset_register_location> asset_register_location_list { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public int user_type_id { get; set; }

        public virtual List<ms_user_type> user_type_list { get; set; }


        public string return_url { get; set; }

        public bool remember_me { get; set; }
    }

    public class CustomSerializeViewModel
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_password { get; set; }
        public int user_type_id { get; set; }

        public int employee_id { get; set; }
        public string employee_nik { get; set; }
        public string employee_name { get; set; }
        public string employee_email { get; set; }
        public bool fl_active { get; set; }

        public List<string> RoleCode { get; set; }

    }


    public class AccountForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class AccountResetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }


    public class AccountRegistrationViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string user_name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string user_password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string user_PasswordConfirm { get; set; }


        [Required]
        [Display(Name = "Employee Name")]
        public string employee_name { get; set; }

        [Required]
        [Display(Name = "NIK")]
        public string employee_nik { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string employee_email { get; set; }

    }

    public class AccountChangePasswordViewModel
    {
        public int user_id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }


        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "New Password")]
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password does not matched.")]
        public string ConfirmPassword { get; set; }
    }


}