using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CrazyTruck
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    if (Session != null)
        //    {
        //        //Redirect to Welcome Page if Session is not null  
        //        HttpContext.Current.Response.Redirect("~/Home/Inicio", false);

        //    }
        //    else
        //    {
        //        //Redirect to Login Page if Session is null & Expires                   
        //        new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Login" } });
        //    }
        //}

    }
}
