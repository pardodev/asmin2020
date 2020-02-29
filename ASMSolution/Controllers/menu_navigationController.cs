namespace ASM_UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using ASM_UI.Models;

    public class menu_navigationController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuNavigation()
        {
            List<Models.MenuViewModel> ListMenu = new List<Models.MenuViewModel>();
            try
            {
                if (UserProfile.user_type_id != null && UserProfile.job_level_id != null)
                {

                    var result = (from m in db.ms_menu
                                  where (m.deleted_date == null && m.fl_active == true)
                                  join n in db.ms_module on m.module_id equals n.module_id
                                  join ur in db.ms_user_rights on m.menu_id equals ur.menu_id
                                  where (ur.user_type_id == UserProfile.user_type_id 
                                  && ur.job_level_id == UserProfile.job_level_id)
                                  select new
                                  {
                                      m.menu_id,
                                      n.module_name,
                                      m.module_id,
                                      m.menu_code,
                                      m.menu_name,
                                      m.menu_url,
                                      m.rec_order
                                  }).ToList();

                    foreach (var mn in result)
                    {
                        Models.MenuViewModel Menu = new Models.MenuViewModel();
                        Menu.menu_id = mn.menu_id;
                        Menu.menu_code = mn.menu_code;
                        Menu.menu_name = mn.menu_name;
                        Menu.menu_url = mn.menu_url;
                        Menu.module_id = (int)mn.module_id;
                        Menu.module_name = mn.module_name;
                        Menu.rec_order = (int)mn.rec_order;
                        ListMenu.Add(Menu);
                    }
                    //App_Helpers.app_logwriter.ToLog(string.Format("Menu for UserType=[{0}] and JobLevel=[{1}] ready", UserProfile.ms_user_type.user_type_name, UserProfile.ms_job_level.job_level_name));
                }
            }
            catch (Exception _ex)
            {
                //throw new Exception("Connection Lost!"+ex.Message);
                App_Helpers.app_logwriter.ToLog("Connection Lost!" + _ex.Message);
                return RedirectToAction("ConnectionLost", "error");
            }
            return PartialView("_Aside", ListMenu.OrderBy(n => n.rec_order));
        }
    }
}