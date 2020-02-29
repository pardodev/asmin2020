using System;
using System.Collections;
using System.Configuration;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM_UI.Models;

namespace ASM_UI.App_Helpers
{
    /*
     * Jika menggunakan akun gmail lihat settingan akun gmaiil nya.
     * Baca : https://stackoverflow.com/questions/20906077/gmail-error-the-smtp-server-requires-a-secure-connection-or-the-client-was-not
     * 
     */

    public class EmailHelper
    {
        private string m_mail_server;
        private int m_mail_port;
        private string m_mail_userid;
        private string m_mail_pwd;
        private DateTime m_mail_init_time = DateTime.Now;
        private DateTime m_mail_sent_time = DateTime.Now;
        private List<string> m_error_message = new List<string>();

        #region Construction destruction
        public EmailHelper()
        {
            this.SetupServer();
            this.FromMailAddress = new MailAddress(m_mail_userid);
            this.ToMailAddress = new List<MailAddress>();
            this.ReplyToMailAddress = new List<MailAddress>();
            this.CcMailAddress = new List<MailAddress>();
            this.BccMailAddress = new List<MailAddress>();
            this.AttachmentFiles = new List<string>();

        }

        public EmailHelper(string EmailSubject, string EmailRecipient, string EmailRecipientCc, string EmailBody)
        {
            this.SetupServer();
            this.FromMailAddress = new MailAddress(m_mail_userid);
            this.MailSubject = EmailSubject;
            this.MailBody = EmailBody;
            this.ToMailAddress = new List<MailAddress>();
            this.CcMailAddress = new List<MailAddress>();
            this.BccMailAddress = new List<MailAddress>();
            this.ReplyToMailAddress = new List<MailAddress>();
            this.AttachmentFiles = new List<string>();
        }

        ~EmailHelper()
        {
        }

        #endregion

        #region Properties


        public string MailSubject { get; set; }

        public string MailBody { get; set; }

        public DateTime MailSentDateTime
        {
            get { return m_mail_sent_time; }
        }

        public DateTime MailInitDateTime
        {
            get { return m_mail_init_time; }
        }

        public string FromAddress { get; set; }

        public string ReplyToAddress { get; set; }

        public string ToAddress { get; set; }

        public string CcAddress { get; set; }

        public string BccAddress { get; set; }

        public System.Net.Mail.MailAddress FromMailAddress { get; set; }

        public List<System.Net.Mail.MailAddress> ReplyToMailAddress { get; set; }

        public List<System.Net.Mail.MailAddress> ToMailAddress { get; set; }

        public List<System.Net.Mail.MailAddress> CcMailAddress { get; set; }

        public List<System.Net.Mail.MailAddress> BccMailAddress { get; set; }

        public int MessageId { get; set; }

        public string Email_Template { get; set; }

        /// <summary>
        /// If True, this email containt attachment file in [AttachmentFiles] property
        /// </summary>
        public Boolean HasAttachment
        {
            get
            {
                if (this.AttachmentFiles.Count > 0)
                    return System.IO.File.Exists(this.AttachmentFiles[0]);
                else
                    return false;
            }
        }


        public Boolean MAIL_SendMailAsync { get; set; }

        public Boolean MAIL_EnabledSSL { get; set; }

        public Boolean MAIL_UseDefaultCredentials { get; set; }

        public List<String> AttachmentFiles { get; set; }

        #endregion

        #region Methods

        private void SetupServer()
        {
            m_mail_init_time = DateTime.Now;
            try
            {
                m_mail_server = app_setting.MAIL_SMTPSERVER;
                bool b_port = int.TryParse(app_setting.MAIL_SMTPPORT, out m_mail_port);
                if (m_mail_port <= 0)
                    m_mail_port = 0;

                this.MAIL_SendMailAsync = false;
                this.MAIL_UseDefaultCredentials = app_setting.MAIL_USE_DEFAULT_CREDENTIAL;
                this.MAIL_EnabledSSL = app_setting.MAIL_ENABLE_SSL;

                m_mail_userid = app_setting.MAIL_FROM;
                m_mail_pwd = app_setting.MAIL_USERPASSWORD;
                this.FromAddress = app_setting.MAIL_FROM;
            }
            catch (Exception _ex)
            {
                m_error_message.Add(_ex.Message);
            }
        }

        private System.Net.NetworkCredential SetCredential()
        {
            System.Net.NetworkCredential _networkCred = new System.Net.NetworkCredential();
            System.Security.SecureString _psswd = new System.Security.SecureString();
            try
            {
                foreach (char _chr_item in m_mail_pwd.ToCharArray())
                {
                    _psswd.AppendChar(_chr_item);
                }
                _networkCred.UserName = m_mail_userid;
                _networkCred.SecurePassword = _psswd;
                //_networkCred.Password = this.MAIL_Password;
                //_networkCred.Domain = this.MAIL_SERVER;
            }
            catch (Exception _ex)
            {
                m_error_message.Add(_ex.Message);
            }
            return _networkCred;
        }

        private void PopulateEmailAddress()
        {

            if (!string.IsNullOrWhiteSpace(this.ToAddress))
            {
                string[] arr_to = this.ToAddress.Split(new char[4] { ';', ',', '#', '/' }, StringSplitOptions.RemoveEmptyEntries);
                //iterate to validadate
                foreach (string _email in arr_to)
                {
                    try
                    {
                        this.ToMailAddress.Add(new MailAddress(_email, _email));
                    }
                    catch (Exception _ex)
                    {
                        this.m_error_message.Add("Invalid Email To Address:" + _ex.Message);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(this.ReplyToAddress))
            {
                string[] arr_replyto = this.ReplyToAddress.Split(new char[4] { ';', ',', '#', '/' }, StringSplitOptions.RemoveEmptyEntries);
                //iterate to validadate
                foreach (string _email in arr_replyto)
                {
                    try
                    {
                        this.ReplyToMailAddress.Add(new MailAddress(_email, _email));
                    }
                    catch (Exception _ex)
                    {
                        this.m_error_message.Add("Invalid Email ReplyTo Address:" + _ex.Message);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(this.CcAddress))
            {
                string[] arr_cc = this.CcAddress.Split(new char[4] { ';', ',', '#', '/' }, StringSplitOptions.RemoveEmptyEntries);
                //iterate to validadate
                foreach (string _email in arr_cc)
                {
                    try
                    {
                        this.CcMailAddress.Add(new MailAddress(_email, _email));
                    }
                    catch (Exception _ex)
                    {
                        this.m_error_message.Add("Invalid Email Cc Address:" + _ex.Message);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(this.BccAddress))
            {
                string[] arr_bcc = this.BccAddress.Split(new char[4] { ';', ',', '#', '/' }, StringSplitOptions.RemoveEmptyEntries);
                //iterate to validadate
                foreach (string _email in arr_bcc)
                {
                    try
                    {
                        this.BccMailAddress.Add(new MailAddress(_email, _email));
                    }
                    catch (Exception _ex)
                    {
                        this.m_error_message.Add("Invalid Email Bcc Address:" + _ex.Message);
                    }
                }
            }
        }

        public Boolean Send()
        {
            Boolean returnBool = true;

            this.PopulateEmailAddress();
            EmailSenderUserToken user_token = null;
            if (this.MAIL_SendMailAsync)
                user_token = new EmailSenderUserToken();

            SmtpClient smtp_client = null;
            MailMessage mail_message = new MailMessage();
            //string exMessage = "";
            try
            {
                if (m_mail_port != 0)
                    smtp_client = new SmtpClient(m_mail_server, m_mail_port);
                else
                    smtp_client = new SmtpClient(m_mail_server);
                smtp_client.EnableSsl = this.MAIL_EnabledSSL;
                smtp_client.UseDefaultCredentials = this.MAIL_UseDefaultCredentials;
                smtp_client.Credentials = this.SetCredential();

                mail_message.From = this.FromMailAddress;

                if (this.ToMailAddress.Count > 0)
                    foreach (MailAddress _emailAddress in this.ToMailAddress)
                        mail_message.To.Add(_emailAddress);

                if (this.ReplyToMailAddress.Count > 0)
                    foreach (MailAddress _emailAddress in this.ReplyToMailAddress)
                        mail_message.ReplyToList.Add(_emailAddress);

                if (this.CcMailAddress.Count > 0)
                    foreach (MailAddress _emailAddress in this.ToMailAddress)
                        mail_message.CC.Add(_emailAddress);

                if (this.BccMailAddress.Count > 0)
                    foreach (MailAddress _emailAddress in this.ToMailAddress)
                        mail_message.Bcc.Add(_emailAddress);

                if (this.HasAttachment)
                {
                    foreach (string str_file in this.AttachmentFiles)
                    {
                        mail_message.Attachments.Add(new System.Net.Mail.Attachment(str_file));
                    }
                }

                mail_message.Subject = this.MailSubject;
                mail_message.Body = this.MailBody;
                mail_message.IsBodyHtml = true;
                smtp_client.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (this.MAIL_SendMailAsync)
                {
                    smtp_client.SendAsync(mail_message, user_token);

                    user_token.MessageID = this.MessageId;
                    user_token.SendResult = returnBool;
                    user_token.SendResultMessage = "Sucess";
                }
                else
                    smtp_client.Send(mail_message);
            }
            catch (Exception mail_ex)
            {
                string exMessage = "Fail on EmailHelper.Send():" + mail_ex.Message.ToString();
                m_error_message.Add(exMessage);
                returnBool = false;
                if (this.MAIL_SendMailAsync)
                {
                    user_token.SendResult = returnBool;
                    user_token.SendResultMessage = exMessage;
                }
                //throw mail_ex;
            }
            finally
            {
                m_mail_sent_time = DateTime.Now;
                try
                {
                    using (ModelAsmRemote db = new ModelAsmRemote())
                    {
                        string exMessage = "";
                        foreach (string str in this.m_error_message)
                            exMessage += str + Environment.NewLine;

                        sy_email_log sy_email_log = new sy_email_log()
                        {
                            elog_template = this.Email_Template,
                            elog_from = this.FromAddress,
                            elog_to = this.ToAddress,
                            elog_cc = this.CcAddress,
                            elog_bcc = this.BccAddress,
                            elog_subject = this.MailSubject,
                            elog_body = this.MailBody,
                            elog_has_attachment = false,
                            //elog_file_attachment = null,
                            fl_active = true,
                            created_date = m_mail_init_time,
                            created_by = ((USER_PROFILE)System.Web.HttpContext.Current.Session["USER_PROFILE"]).UserId,
                            fl_sent = returnBool,
                            sent_date = m_mail_sent_time,
                            err_message = exMessage
                        };
                        

                        sy_email_log = db.sy_email_log.Add(sy_email_log);
                        db.SaveChanges();
                    };
                }
                catch { }

            }
            return returnBool;
        }

        #endregion
    }

    public class EmailSenderUserToken
    {
        public int MessageID { get; set; }
        public string SendResultMessage { get; set; }
        public Boolean SendResult { get; set; }

    }

}