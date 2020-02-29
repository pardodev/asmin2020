using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;

namespace ASM_UI.Controllers
{
    public class errorController : BaseController
    {
        // GET: error
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Index(string aspxerrorpath = "")
        //{
        //    return View();
        //}
        
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        //public JsonResult index(errorHandlerViewModel view_err)
        //{
            
        //    return Json(view_err, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult ConnectionLost()
        //{
        //    errorHandlerViewModel err_view = new errorHandlerViewModel
        //    {
        //        error_description = "Connection Lost!",
        //        error_code = "500"
        //    };
        //    return View(err_view);
        //}
        
    }
}