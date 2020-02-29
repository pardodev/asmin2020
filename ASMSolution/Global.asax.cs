using ASM_UI.CustomAuthentication;
using ASM_UI.Models;
using ASM_UI.App_Helpers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Optimization;

namespace ASM_UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var binder = new App_Start.UTCDateTimeModelBinder();
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);
        }

        protected void Session_Start()
        {
            app_logwriter.ToLog(string.Format("New SessionID Started {0},", Session.SessionID));
            USER_PROFILE UserProfile = new USER_PROFILE()
            {
                SESSION_CREATED = DateTime.Now,
                SESSION_ID = Session.SessionID
            };
            if (UserProfile.UserId == 0)
            {
                //Redirect to Welcome Page if Session is null  
                HttpContext.Current.Response.Redirect("~/Account/Login?ReturnUrl=%2f", false);
            }

            Session["USER_PROFILE"] = UserProfile;
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            //if (Session["USER_PROFILE"] != null)
            //{
            //    USER_PROFILE UserProfile = (USER_PROFILE)Session["USER_PROFILE"];
            //    if (UserProfile.UserId == 0)
            //    {
            //        HttpContext.Current.Response.Redirect("~/Account/Login?ReturnUrl=%2f", false);
            //    }
            //}

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //HttpCookie authCookie = Request.Cookies[cookie_name];
            if (authCookie != null && HttpContext.Current.User == null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serializeModel = JsonConvert.DeserializeObject<CustomSerializeViewModel>(authTicket.UserData);

                CustomPrincipal principal = new CustomPrincipal(authTicket.Name);
                principal.user_id = serializeModel.user_id;
                principal.user_name = serializeModel.user_name;
                principal.user_password = serializeModel.user_password;
                principal.employee_id = serializeModel.employee_id;
                principal.employee_nik = serializeModel.employee_nik;
                principal.employee_name = serializeModel.employee_name;
                principal.employee_email = serializeModel.employee_email;
                principal.Roles = serializeModel.RoleCode.ToArray<string>();

                HttpContext.Current.User = principal;
            }

        }


        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            //log the error!
            app_logwriter.ToLog(ex.Message);
        }

    }
}
