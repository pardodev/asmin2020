using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASM_UI.Models
{
    public class user_rightViewModel
    {
        [Display(Name = "Job Level")]
        public int job_level_id { get; set; }

        public virtual ms_job_level job_level { get; set; }

        [Display(Name = "User Type")]
        public int user_type_id { get; set; }

        public virtual ms_user_type user_type { get; set; }
        

        public List<menu_access> menu_access { get; set; }

        [Display(Name = "Menu")]
        public virtual ICollection<ms_menu> ms_menus { get; set; }

        
    }

    public class menu_access
    {
        public int menu_id { get; set; }
        public bool is_allowed { get; set; }
    }

}