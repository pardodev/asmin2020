using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ASM_UI.Models
{
    public class moduleViewModel
    {
        [Key]
        public int module_id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Code is mandatory")]
        [Display(Name = "Code")]
        public string module_code { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Name is mandatory")]
        [Display(Name = "Name")]
        public string module_name { get; set; }

        [Display(Name = "Is Active")]
        public string fl_active { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime? rec_modified_date { get; set; }

        [Display(Name = "Last Modified By")]
        public string rec_modified_by { get; set; }

    }
}