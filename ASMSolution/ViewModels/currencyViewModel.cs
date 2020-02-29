using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ASM_UI.Models
{
    public class currencyViewModel
    {

        [Key]
        public int currency_id { get; set; }

        [StringLength(3)]
        [Required(ErrorMessage = "Currency code is mandatory")]
        [Display(Name = "Code")]
        public string currency_code { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Currency name is mandatory")]
        [Display(Name = "Name")]
        public string currency_name { get; set; }

        [Display(Name = "Is Active")]
        public string fl_active { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime? rec_modified_date { get; set; }

        [Display(Name = "Last Modified By")]
        public string rec_modified_by { get; set; }


    }
}