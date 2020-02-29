using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASM_UI.App_Helpers
{
    public class app_logwriter
    {
        public static void ToLog(string pMessage)
        {
            ToLog(pMessage, true);
        }

        public static void ToLog(string pMessage, bool bWithNewLine)
        {
            string _log_message = pMessage;
            if (!string.IsNullOrWhiteSpace(_log_message))
            {
                try
                {
                    string _app_log_file = "asmlog.log";
                    string _log_path = "";
                    try
                    {
                        _log_path = System.Configuration.ConfigurationManager.AppSettings["LOG_PATH"].ToString();
                    }
                    catch { _log_path = ""; }

                    if (string.IsNullOrWhiteSpace(_log_path))
                        _log_path = HttpContext.Current.Server.MapPath("~/");

                    if (!System.IO.Directory.Exists(_log_path))
                        System.IO.Directory.CreateDirectory(_log_path);

                    string CurrentDirectory = _log_path;
                    if (CurrentDirectory.Substring(CurrentDirectory.Length - 1, 1) == @"\")
                        CurrentDirectory = CurrentDirectory.Substring(0, CurrentDirectory.Length - 1);

                    string _log_path_file = CurrentDirectory + @"\" + _app_log_file;
                    TextWriter _log_writer = new TextWriter(_log_path_file, TextWriter.LogType.Daily, true);
                    if (_log_writer == null)
                        _log_writer = new TextWriter(_log_path_file, TextWriter.LogType.Daily, true);

                    string _long_message = string.Format("{0} : {1}", DateTime.Now, _log_message);
                    if (bWithNewLine)
                        _log_writer.WriteLine(_long_message);
                    else
                        _log_writer.Write(_long_message);
                }
                catch { }
            }
        }

    }
}