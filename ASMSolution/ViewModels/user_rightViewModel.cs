using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASM_UI.Models
{
    public class user_rightViewModel
    {
        public user_rightViewModel()
        {
            this.checkbox_menu_id = new List<SelectedMenu_CheckBoxes>();
            this.FormMode = EnumFormModeKey.Form_New;
        }

        [Required]
        [Display(Name = "Job Level")]
        public int job_level_id { get; set; }

        public virtual IEnumerable<ms_job_level> job_level_list { get; set; }


        [Required]
        [Display(Name = "User Type")]
        public int user_type_id { get; set; }

        public virtual IEnumerable<ms_user_type> user_type_list { get; set; }

        public List<SelectedMenu_CheckBoxes> checkbox_menu_id { get; set; }

        public string selected_menu_id_str { get; set; }

        [Display(Name = "Menu")]
        public virtual IEnumerable<ms_menu> ms_menus { get; set; }

        public EnumFormModeKey FormMode { get; set; }


    }

    public class SelectedMenu_CheckBoxes
    {
        public int menu_id { get; set; }
        public virtual ms_menu ms_menu  { get; set; }

        public string Value { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
    }

    public enum EnumFormModeKey
    {        
        Form_New = 0,
        Form_Edit = 1
    }


}