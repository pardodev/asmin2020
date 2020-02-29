using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASM_UI.CustomAuthentication
{
    /// <summary>
    /// role user
    /// </summary>
    public class Role
    {
        /// <summary>
        /// user_type_id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// user_type_code
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// user_type_name
        /// </summary>
        public string RoleName { get; set; }


        /// <summary>
        /// employee user
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

    }
}