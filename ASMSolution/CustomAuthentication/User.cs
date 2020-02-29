using ASM_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASM_UI.CustomAuthentication
{
    /// <summary>
    /// user dalam hal ini adalah gabungan antara ms_user dan ms_employee
    /// UserRole adalah ms_employee_detail
    /// </summary>
    public class User
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

        /*
        karena satu user bisa mempunyai :
        - lebih dari satu role, 
        - di company yang berbeda, 
        - didepartement berbeda dan 
        - job level yang berbeda        
        */

        /// <summary>
        /// employee_detail
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// company list
        /// </summary>
        public virtual ICollection<ms_asmin_company> Companies { get; set; }

        /// <summary>
        /// department list
        /// </summary>
        public virtual ICollection<ms_department> Departments { get; set; }

        /// <summary>
        /// joblevel
        /// </summary>
        public virtual ICollection<ms_job_level> JobLevels { get; set; }

        /// <summary>
        /// ms_user_type
        /// </summary>
        public virtual ICollection<ms_user_type> ms_user_type { get; set; }

    }
}