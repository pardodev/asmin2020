using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;
using ASM_UI.App_Helpers;
using System.Net.Mail;

namespace ASM_UI.Controllers
{
    public class emailsendController : Controller
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        public ActionResult Index()
        {
            var _list = db.sy_email_log.ToList();
            return View(_list);
        }

        // GET: sy_email_log/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            sy_email_log sy_email_log = db.sy_email_log.Find(id);
            if (sy_email_log == null)
            {
                return HttpNotFound();
            }
            return View(sy_email_log);
        }

        // GET: sy_email_log/Create
        public ActionResult Create()
        {
            List<string> _listTemplate = new List<string>();
            var _list = app_setting.APPLICATION_SETTING.Where(c => c.app_key.Contains("EMAIL_TEMPLATE"));
            foreach (sy_app_setting _setting in _list)
                _listTemplate.Add(_setting.app_key);

            sy_email_log sy_email_log = new sy_email_log()
            {
                //elog_template_list = _listTemplate,
                elog_template = "EMAIL_TEMPLATE_01",
                elog_from = app_setting.MAIL_FROM,
                fl_active = true
            };

            return View(sy_email_log);
        }

        // POST: sy_email_log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "elog_from,elog_to,elog_cc,elog_bcc,elog_subject,elog_body")] sy_email_log email_form)
        {
            if (ModelState.IsValid)
            {
                /*
                cara mengirim email :
                1. panggil EmailHelper
                2. lalu kirim email
                 */
               var EmailHelper = new EmailHelper()
                {
                    ToAddress = email_form.elog_to,
                    CcAddress = email_form.elog_cc,
                    BccAddress = email_form.elog_bcc,
                    MailSubject = email_form.elog_subject,
                    MailBody = email_form.elog_body
                };
                bool boolSent = EmailHelper.Send();
                if (boolSent)
                    return RedirectToAction("Index");
            }

            return View(email_form);
        }

        public ActionResult Resend(int id)
        {
            sy_email_log sy_email_log = db.sy_email_log.Find(id);
            if (sy_email_log == null)
            {
                return HttpNotFound();
            }
            var EmailHelper = new EmailHelper()
            {
                ToAddress = sy_email_log.elog_to,
                CcAddress = sy_email_log.elog_cc,
                BccAddress = sy_email_log.elog_bcc,
                MailSubject = sy_email_log.elog_subject,
                MailBody = sy_email_log.elog_body
            };
            bool boolSent = EmailHelper.Send();

            return RedirectToAction("Index");
        }


        // GET: sy_email_log/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sy_email_log sy_email_log = db.sy_email_log.Find(id);
            if (sy_email_log == null)
            {
                return HttpNotFound();
            }
            return View(sy_email_log);
        }

        // POST: sy_email_log/Delete/5
        [HttpGet, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sy_email_log sy_email_log = db.sy_email_log.Find(id);
            db.sy_email_log.Remove(sy_email_log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}


/*
 
baca ref:
https://stackoverflow.com/questions/26784366/how-to-send-email-from-mvc-5-application
     
*/
