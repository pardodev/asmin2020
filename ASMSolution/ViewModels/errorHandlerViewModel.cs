using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASM_UI.Models
{
    public class errorHandlerViewModel
    {
        public string error_number { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }

        public DateTime err_created { get; set; }
        public string err_source { get; set; }
    }
}