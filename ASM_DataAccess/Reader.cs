using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ASM.DataAccess
{
    public class Reader : IDisposable
    {

        string _templateFileName;
        string _templateFileExt;
        StreamReader _reader;

        public Reader(string templateFilename)
        {
            try
            {
                int extpos = templateFilename.LastIndexOf(".");
                _templateFileName = templateFilename.Substring(0, extpos);
                _templateFileExt = templateFilename.Substring(extpos, templateFilename.Length - extpos);  //keep the dot                
                if (!File.Exists(templateFilename))
                    throw new Exception("File not found " + _templateFileName);
            }
            catch
            {
                throw new FormatException("Template Filename not as expected.");
            }
        }

        private void checkLogname()
        {
            try
            {
                _reader.Close();
            }
            catch { }
            _reader = new StreamReader(_templateFileName + _templateFileExt);
        }

        public string ReadToEnd()
        {
            try
            {
                checkLogname();
                return _reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _reader.Close();
            }
        }

        public string ReadLine()
        {
            try
            {
                checkLogname();
                return _reader.ReadLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _reader.Close();
            }
        }

        private void Close()
        {
            try
            {
                _reader.Close();
                _reader.Dispose();
            }
            catch { }
        }


        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }

}
