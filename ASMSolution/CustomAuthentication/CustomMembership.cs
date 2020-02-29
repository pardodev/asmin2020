using ASM_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ASM_UI.CustomAuthentication
{
    public class CustomMembership : MembershipProvider
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">ini sebenarnya username OR NIK</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            using (ModelAsmRemote _db = new ModelAsmRemote())
            {
                //var user = (from _usr in _db.ms_user
                //            where string.Compare(username, _usr.user_name, StringComparison.OrdinalIgnoreCase) == 0
                //            && string.Compare(password, _usr.user_password, StringComparison.OrdinalIgnoreCase) == 0
                //            && _usr.fl_active == true
                //            && _usr.deleted_date == null
                //            select _usr).FirstOrDefault();

                //sesuai request user : bisa userid atau nik
                //date: 2019-08-24
                var user = (from _usr in _db.ms_user
                            join _emp in _db.ms_employee on _usr.employee_id equals _emp.employee_id
                            where (string.Compare(username, _usr.user_name, StringComparison.OrdinalIgnoreCase) == 0
                            || string.Compare(username, _emp.employee_nik, StringComparison.OrdinalIgnoreCase) == 0)
                            && string.Compare(password, _usr.user_password, StringComparison.OrdinalIgnoreCase) == 0
                            && _usr.fl_active == true
                            && _usr.deleted_date == null
                            select _usr).FirstOrDefault();


                return (user != null) ? true : false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object loginview, bool userIsOnline)
        {
            using (ModelAsmRemote _db = new ModelAsmRemote())
            {
                AccountLoginViewModel accountloginview = loginview as AccountLoginViewModel;
                User _user = new User();
                //yang diambil dari login form
                string m_username = accountloginview.UserName;
                int m_company_id = accountloginview.company_id;
                int m_location_reg = accountloginview.asset_reg_location_id;
                int m_user_type_id = accountloginview.user_type_id;

                try
                {
                    //var user = (from _usr in _db.ms_user
                    //            where string.Compare(m_username, _usr.user_name, StringComparison.OrdinalIgnoreCase) == 0
                    //            && _usr.fl_active == true
                    //            && _usr.deleted_date == null

                    //            join _empdetail in _db.ms_employee_detail on _usr.employee_id equals _empdetail.employee_id
                    //            where _empdetail.company_id == m_company_id && _empdetail.asset_reg_location_id == m_location_reg && _empdetail.user_type_id == m_user_type_id

                    //            select _usr).FirstOrDefault();

                    var m_found_user = (
                                        //from _usr in _db.ms_user
                                        //join _emp in _db.ms_employee on _usr.employee_id equals _emp.employee_id
                                        //where 
                                        //(string.Compare(m_username, _usr.user_name, StringComparison.OrdinalIgnoreCase) == 0
                                        //|| string.Compare(m_username, _emp.employee_nik, StringComparison.OrdinalIgnoreCase) == 0)
                                        //&& _user.user_type_id == m_user_type_id
                                        //&& _usr.fl_active == true
                                        //&& _usr.deleted_date == null

                                        //join _empdetail in _db.ms_employee_detail on _emp.employee_id equals _empdetail.employee_id
                                        //where _empdetail.company_id == m_company_id
                                        //&& _empdetail.asset_reg_location_id == m_location_reg
                                        from _usr in _db.ms_user
                                        join _emp in _db.ms_employee on _usr.employee_id equals _emp.employee_id
                                        where _usr.user_type_id == m_user_type_id
                                        && _usr.fl_active == true
                                        && _usr.deleted_date == null
                                        && (string.Compare(m_username, _usr.user_name, StringComparison.OrdinalIgnoreCase) == 0
                                        || string.Compare(m_username, _emp.employee_nik, StringComparison.OrdinalIgnoreCase) == 0)

                                        join _empdetail in _db.ms_employee_detail on _emp.employee_id equals _empdetail.employee_id
                                        where _empdetail.company_id == m_company_id
                                        && _empdetail.location_id == m_location_reg
                                        //&& _empdetail.user_type_id == m_user_type_id       //ambil dari form jangan dari tbl ini
                                        select _usr).FirstOrDefault();

                    if (m_found_user == null)
                    {
                        return null;
                    }

                    //var selectedUser = new CustomMembershipUser(user);
                    _user = new User
                    {
                        user_id = m_found_user.user_id,
                        user_name = m_found_user.user_name,
                        user_password = m_found_user.user_password,
                        //user_type_id = user.user_type_id, /* kalo ambil dari table user */
                        user_type_id = m_user_type_id,      /* ambil dari form login */

                        employee_id = m_found_user.employee_id,
                        employee_nik = m_found_user.ms_employee.employee_nik,
                        employee_name = m_found_user.ms_employee.employee_name,
                        employee_email = m_found_user.ms_employee.employee_email,

                        fl_active = (bool)m_found_user.fl_active
                        //Roles = user.ms_employee.ms_employee_detail.Select(r => new Role
                        //{
                        //    RoleId = r.ms_user_type.user_type_id,
                        //    RoleCode = r.ms_user_type.user_type_code,
                        //    RoleName = r.ms_user_type.user_type_name
                        //}).ToList<Role>()
                    };

                    //var _objRoles = _db.ms_employee_detail.Include("job_level_id").Include("user_type_id").Where(e => e.employee_id == user.employee_id).Select(r => new Role
                    //{
                    //    RoleId = (r.user_type_id.HasValue) ? r.ms_user_type.user_type_id : 0,
                    //    RoleCode = (r.user_type_id.HasValue) ? r.ms_user_type.user_type_code : "GST",
                    //    RoleName = (r.user_type_id.HasValue) ? r.ms_user_type.user_type_name : "Guest"
                    //}).ToList<Role>();
                    var _objRoles = _db.ms_employee_detail.Include("job_level_id")
                        .Where(e => e.employee_id == m_found_user.employee_id
                    && e.company_id == m_company_id
                    && e.asset_reg_location_id == m_location_reg
                    //&& e.user_type_id == m_user_type_id
                    ).Select(r => new Role
                    {
                        RoleId = (r.job_level_id.HasValue) ? r.ms_job_level.job_level_id : 0,
                        RoleCode = (r.job_level_id.HasValue) ? r.ms_job_level.job_level_code : "GST",
                        RoleName = (r.job_level_id.HasValue) ? r.ms_job_level.job_level_name : "Guest"
                    }).ToList<Role>();

                    if (_objRoles != null)
                    {
                        _user.Roles = new List<Role>();
                        foreach (Role _rlItem in _objRoles)
                        {
                            _user.Roles.Add(_rlItem);
                        }
                    }
                    else
                        _user.Roles = new List<Role>();
                }
                catch (Exception _exc)
                {
                    App_Helpers.app_logwriter.ToLog("CustomeMembership.GetUser():" + _exc.Message);
                    //throw new Exception("GetUser():" + _exc.Message);
                }
                var selectedUser = new CustomMembershipUser(_user);
                return selectedUser;
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            using (ModelAsmRemote dbContext = new ModelAsmRemote())
            {
                string username = (from u in dbContext.ms_user
                                   where string.Compare(email, u.ms_employee.employee_email) == 0
                                   select u.user_name).FirstOrDefault();

                return !string.IsNullOrEmpty(username) ? username : string.Empty;
            }
        }


        #region Overrides of Membership Provider

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

        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}