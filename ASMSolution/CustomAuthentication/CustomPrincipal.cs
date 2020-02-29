using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ASM_UI.CustomAuthentication
{
    public class CustomPrincipal : IPrincipal
    {
        #region Identity Properties

        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_password { get; set; }

        public int employee_id { get; set; }
        public string employee_nik { get; set; }
        public string employee_name { get; set; }
        public string employee_email { get; set; }
        public bool fl_active { get; set; }

        /// <summary>
        /// in this case : user_type_code
        /// </summary>
        public string[] Roles { get; set; }


        #endregion

        public IIdentity Identity
        {
            get; private set;
        }

        public bool IsInRole(string role)
        {
            if (Roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}