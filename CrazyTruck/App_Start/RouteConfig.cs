using AutoMapper;
using CrazyTruck.Models;
using Nucleo.CrazyTruck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CrazyTruck
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional }
            );

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Flete, FleteDTO>();
                cfg.CreateMap<Carga, CargaDTO>();
            });
        }
    }
}
