using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ASM_UI.CustomAuthentication
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_password { get; set; }
        public int user_type_id { get; set; }

        public int employee_id { get; set; }
        public string employee_nik { get; set; }
        public string employee_name { get; set; }
        public string employee_email { get; set; }
        public bool fl_active { get; set; }

        public ICollection<Role> Roles { get; set; }
        //public virtual ASM_UI.Models.ms_user_type ms_user_type { get; set; }

        #endregion

        public CustomMembershipUser(User user) :
            base("CustomMembership", user.user_name, user.user_id, user.employee_email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            user_id = user.user_id;
            user_name = user.user_name;
            user_password = user.user_password;
            user_type_id = user.user_type_id;

            employee_id = user.employee_id;
            employee_nik = user.employee_nik;
            employee_name = user.employee_name;
            employee_email = user.employee_email;

            Roles = user.Roles;
        }


    }
}