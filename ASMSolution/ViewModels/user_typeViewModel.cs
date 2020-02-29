using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ASM_UI.Models
{
    public class user_typeViewModel
    {
        [Key]
        public int user_type_id { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = "User type code is mandatory")]
        [Display(Name = "User Type Code")]
        public string user_type_code { get; set; }


        [StringLength(50)]
        [Required(ErrorMessage = "User type name is mandatory")]
        [Display(Name = "User Type Name")]
        public string user_type_name { get; set; }



        [Display(Name = "Is Active")]
        public string fl_active { get; set; }


        [Display(Name = "Last Modified Date")]
        public DateTime? rec_modified_date { get; set; }


        [Display(Name = "Last Modified By")]
        public string rec_modified_by { get; set; }

    }
}