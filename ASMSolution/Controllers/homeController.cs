﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASM_UI.Models;

namespace ASM_UI.Controllers
{
    public class homeController : BaseController
    {
        private ModelAsmRemote db = new ModelAsmRemote();

        [Authorize]
        public ActionResult Index()
        {
            App_Helpers.app_logwriter.ToLog("Naviage to HomePage");

            return View();

        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}