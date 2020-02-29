using ASM_UI.CustomAuthentication;
using ASM_UI.Models;
using ASM_UI.App_Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ASM_UI.Controllers
{
    public abstract class BaseController : Controller
    {
        public USER_PROFILE UserProfile = (USER_PROFILE)System.Web.HttpContext.Current.Session["USER_PROFILE"];
        //private string _controllerName;

        public BaseController()
        {
            //_controllerName = RouteData.Values["controller"].ToString().ToLower();
            //app_logwriter.ToLog(UserProfile.UserName + " navigate to /" + _controllerName + " : " + Request.Url);
        }

        //public string controllerName
        //{
        //    get {
        //        return _controllerName;
        //    }
        //}

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            App_Helpers.app_logwriter.ToLog(filterContext.Exception.Message);
            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Index", "Error");
            //// OR 
            //filterContext.Result = new ViewResult
            //{
            //    ViewName = "~/Views/error/ErrorPage.cshtml"
            //};
        }

    }
}