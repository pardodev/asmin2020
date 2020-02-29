using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASM_UI.Models
{
    public class MenuViewModel
    {
        public int menu_id { get; set; }
        public int module_id { get; set; }
        public string module_name { get; set; }
        public string menu_code { get; set; }
        public string menu_name { get; set; }
        public string menu_url { get; set; }
        public int rec_order { get; set; }
    }

    public enum EnumBooleanKey
    {
        Yes = 1,
        No = 0
    }


}