using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Text;

namespace ASM.DataAccess
{
    public class Writer : IDisposable
    {
        #region Variables
        string _templateFileName;
        string _templateFileExt;
        string _curdate;
        string _curweekday;
        string _curmonth;
        string _curyear;
        StreamWriter _writer;
        LogType _type;
        bool _append;
        #endregion

        public Writer(string templateFilename, LogType type, bool isAppend)
        {
            try
            {
                int extpos = templateFilename.LastIndexOf(".");
                _templateFileName = templateFilename.Substring(0, extpos);
                _templateFileExt = templateFilename.Substring(extpos, templateFilename.Length - extpos);  //keep the dot
                _curmonth = "";									// reset curmonth on constructing to force checkLogname() to construct writer
                _type = type;
                _append = isAppend;
            }
            catch
            {
                throw new FormatException("Template Filename not as expected.");
            }
            checkLogname();
        }

        private void checkLogname()
        {
            try
            {
                switch (_type)
                {
                    case LogType.Custome:
                        try
                        {
                            _writer.Close();                                                        //switch writer to the new log name
                        }
                        catch { }
                        _writer = new StreamWriter(_templateFileName + _templateFileExt, _append);
                        _writer.AutoFlush = true;
                        break;
                    case LogType.Daily:
                        //if (DateTime.Now.ToString("dd") != _curdate)									//different date? time to roll over... 
                        //{																				//curdate is instance var that will retain its value until reassign with new value (i.e. when DateTime.Now is in different date).. 
                        try
                        {
                            _writer.Close();                                                        //switch writer to the new log name
                        }
                        catch { }
                        _curdate = DateTime.Now.ToString("dd");
                        _writer = new StreamWriter(_templateFileName + _curdate + _templateFileExt, _append);
                        _writer.AutoFlush = true;

                        string nextdate = DateTime.Now.AddDays(1).ToString("DD");      //remove the oldest logfile if exists .... 
                        if (File.Exists(_templateFileName + nextdate + _templateFileExt))
                            File.Delete(_templateFileName + nextdate + _templateFileExt);
                        //}
                        break;
                    case LogType.Weekly:
                        //if (DateTime.Now.ToString("ddd") != _curmonth)									//different weekday? time to roll over... 
                        //{																				//curweekday is instance var that will retain its value until reassign with new value (i.e. when DateTime.Now is in different weekday).. 
                        try
                        {
                            _writer.Close();                                                        //switch writer to the new log name
                        }
                        catch { }
                        _curweekday = DateTime.Now.ToString("ddd");
                        _writer = new StreamWriter(_templateFileName + _curweekday + _templateFileExt, _append);
                        _writer.AutoFlush = true;

                        string nextweekday = DateTime.Now.AddDays(1).ToString("DDDDD");      //remove the oldest logfile if exists .... 
                        if (File.Exists(_templateFileName + nextweekday + _templateFileExt))
                            File.Delete(_templateFileName + nextweekday + _templateFileExt);
                        //}
                        break;
                    case LogType.Monthly:
                        //if (DateTime.Now.ToString("MMM") != _curmonth)									//different month? time to roll over... 
                        //{																				//curmonth is instance var that will retain its value until reassign with new value (i.e. when DateTime.Now is in different month).. 
                        try
                        {
                            _writer.Close();                                                        //switch writer to the new log name
                        }
                        catch { }
                        _curmonth = DateTime.Now.ToString("MMM");
                        _writer = new StreamWriter(_templateFileName + _curmonth + _templateFileExt, _append);
                        _writer.AutoFlush = true;

                        string nextmonth = DateTime.Now.AddMonths(1).ToString("MMM");      //remove the oldest logfile if exists .... 
                        if (File.Exists(_templateFileName + nextmonth + _templateFileExt))      //      <oldest means: logfile with month component equal to next month, thus had been about 10 months old>
                            File.Delete(_templateFileName + nextmonth + _templateFileExt);
                        //}
                        break;
                    case LogType.Annually:
                        //if (DateTime.Now.ToString("yyyy") != _curyear)									//different year? time to roll over... 
                        //{																				//curyear is instance var that will retain its value until reassign with new value (i.e. when DateTime.Now is in different year).. 
                        try
                        {
                            _writer.Close();                                                        //switch writer to the new log name
                        }
                        catch { }
                        _curyear = DateTime.Now.ToString("yyyy");
                        _writer = new StreamWriter(_templateFileName + _curyear + _templateFileExt, _append);
                        _writer.AutoFlush = true;

                        string nextyear = DateTime.Now.AddYears(1).ToString("yyyy");      //remove the oldest logfile if exists .... 
                        if (File.Exists(_templateFileName + nextyear + _templateFileExt))
                            File.Delete(_templateFileName + nextyear + _templateFileExt);
                        //}
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Write(string str)
        {
            try
            {
                checkLogname();
                _writer.Write(str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _writer.Close();
            }
        }

        public void WriteLine(string str)
        {
            try
            {
                checkLogname();
                _writer.WriteLine(str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _writer.Close();
            }
        }

        public void Close()
        {
            try
            {
                _writer.Flush();
                _writer.Close();
            }
            catch { }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion

        #region Enum
        public enum LogType
        {
            Custome = 0,
            Daily = 1,
            Weekly = 2,
            Monthly = 3,
            Annually = 4
        }
        #endregion
    }

}
