using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASM_UI.Models;

namespace ASM_UI.App_Helpers
{
    public static class app_setting
    {
        #region APP_SETTING

        public static string Client_Mode
        {
            get
            {

                string _string = "";
                try
                {
                    string obj_cfg = System.Configuration.ConfigurationManager.AppSettings["Mode"].ToString();
                    if (!string.IsNullOrWhiteSpace(obj_cfg))
                        _string = obj_cfg;
                }
                catch { }
                return _string;

            }
        }

        public static string COOKIE_NAME
        {
            get
            {

                string _string = "";
                try
                {
                    string obj_cfg = System.Configuration.ConfigurationManager.AppSettings["COOKIE_NAME"].ToString();
                    if (!string.IsNullOrWhiteSpace(obj_cfg))
                        _string = obj_cfg;
                }
                catch
                {
                    _string = "ASM";
                }
                return "." + _string.ToUpper();

            }
        }

        #endregion

        #region CONN_STRING

        public static string ASMConnString
        {
            get
            {
                string _string = "";
                try
                {
                    string obj_cfg = System.Configuration.ConfigurationManager.ConnectionStrings["ModelAsmRemote"].ToString();
                    if (!string.IsNullOrWhiteSpace(obj_cfg))
                        _string = obj_cfg;
                }
                catch { }
                return _string;
            }
        }
        #endregion

        #region APP_DB
        public static IEnumerable<sy_app_setting> APPLICATION_SETTING
        {
            get
            {
                ModelAsmRemote db = new ModelAsmRemote();
                IEnumerable<sy_app_setting> _qry = db.sy_app_setting.Where(a => a.fl_active == true);
                return _qry;
            }
        }

        public static int NOTIF_DISPLAYED_TIMES
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "NOTIF_DISPLAYED_TIMES").Single<sy_app_setting>();
                    return Convert.ToInt32(_obj.app_value);
                }
                catch { }
                return 1;
            }
        }

        public static int NOTIF_DISPLAYED_MAX
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "NOTIF_DISPLAYED_MAX").Single<sy_app_setting>();
                    return Convert.ToInt32(_obj.app_value);
                }
                catch { }
                return 1;
            }
        }

        public static Boolean MAIL_ENABLE_SSL
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_ENABLE_SSL").Single<sy_app_setting>();
                    return _obj.app_value.ToUpper().Equals("TRUE");
                }
                catch { }
                return false;
            }
        }

        public static Boolean MAIL_USE_DEFAULT_CREDENTIAL
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_USE_DEFAULT_CREDENTIAL").Single<sy_app_setting>();
                    return _obj.app_value.ToUpper().Equals("TRUE");
                }
                catch { }
                return false;
            }
        }


        public static string MAIL_FROM
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_FROM").Single<sy_app_setting>();
                    return Convert.ToString(_obj.app_value);
                }
                catch { }
                return "";
            }
        }

        public static string MAIL_SMTPPORT
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_SMTPPORT").Single<sy_app_setting>();
                    return Convert.ToString(_obj.app_value);
                }
                catch { }
                return "";
            }
        }

        public static string MAIL_SMTPSERVER
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_SMTPSERVER").Single<sy_app_setting>();
                    return Convert.ToString(_obj.app_value);
                }
                catch { }
                return "";
            }
        }

        public static string MAIL_TEMPLATE_BODY
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_TEMPLATE_BODY").Single<sy_app_setting>();
                    return Convert.ToString(_obj.app_value);
                }
                catch { }
                return "";
            }
        }

        public static string MAIL_TEMPLATE_SUBJECT
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_TEMPLATE_SUBJECT").Single<sy_app_setting>();
                    return Convert.ToString(_obj.app_value);
                }
                catch { }
                return "";
            }
        }

        public static string MAIL_USERDOMAIN
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_USERDOMAIN").Single<sy_app_setting>();
                    return Convert.ToString(_obj.app_value);
                }
                catch { }
                return "";
            }
        }

        public static string MAIL_USERNAME
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_USERNAME").Single<sy_app_setting>();
                    return Convert.ToString(_obj.app_value);
                }
                catch { }
                return "";
            }
        }

        public static string MAIL_USERPASSWORD
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "MAIL_USERPASSWORD").Single<sy_app_setting>();
                    return Convert.ToString(_obj.app_value);
                }
                catch { }
                return "";
            }
        }

        public static int NOTIF_ANIMATION_INTERVAL
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "NOTIF_ANIMATION_INTERVAL").Single<sy_app_setting>();
                    return Convert.ToInt32(_obj.app_value);
                }
                catch { }
                return 1;
            }
        }

        public static int NOTIF_TASK_INTERVAL
        {
            get
            {
                try
                {
                    sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == "NOTIF_TASK_INTERVAL").Single<sy_app_setting>();
                    return Convert.ToInt32(_obj.app_value);
                }
                catch { }
                return 1;
            }
        }


        public static string GetSetting(string pKey)
        {
            try
            {
                sy_app_setting _obj = APPLICATION_SETTING.Where(a => a.app_key.ToUpper() == pKey.ToUpper()).Single<sy_app_setting>();
                return _obj.app_value;
            }
            catch { }
            return "";
        }

        #endregion

    }
}