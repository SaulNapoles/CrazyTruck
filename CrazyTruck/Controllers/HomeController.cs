﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrazyTruck.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Inicio()
        {
            if (Session!=null)
            {
                return View();
            }else
            {
              return  RedirectToAction("Index", "Login");
            }
            
        }


    }
}