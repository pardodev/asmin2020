using ASM_UI.Models;
using ASM_UI.CustomAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ASM_UI.CustomAuthentication
{
    public class CustomRole : RoleProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };

            using (ModelAsmRemote _db = new ModelAsmRemote())
            {
                var selectedUser = (from usr in _db.ms_user.Include("ms_employee").Include("ms_employee_detail").Include("job_level_id")
                                    where string.Compare(usr.user_name, username, StringComparison.OrdinalIgnoreCase) == 0
                                    && usr.deleted_date == null && usr.fl_active == true
                                    select usr
                                    //select new User
                                    //{
                                    //    user_id = usr.user_id,
                                    //    user_name = usr.user_name,
                                    //    user_password = usr.user_password,

                                    //    employee_id = usr.ms_employee.employee_id,
                                    //    employee_name = usr.ms_employee.employee_name,
                                    //    employee_email = usr.ms_employee.employee_email,

                                    //    fl_active = (bool)usr.fl_active ,
                                    //    Roles =  usr.ms_employee.ms_employee_detail.user.Select( r => r.user_type_id)                                        
                                    //}
                                    ).FirstOrDefault();

                
                if(selectedUser != null)
                {
                    //userRoles = new[] { selectedUser.ms_employee_detail.Select(r=>r.ms_job_level.job_level_code).ToString() };
                    userRoles = new[] { selectedUser.ms_user_type.user_type_code };
                }

                return userRoles.ToArray();
            }


        }

                                                   

        #region Overrides of Role Provider

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}