using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nucleo.CrazyTruck;

namespace CrazyTruck.Controllers
{
    public class FletesController : Controller
    {
        // GET: Home
        public ActionResult Lista()
        {
            return View();
        }

        /*// GET: Fletes
        public ActionResult Lista()
        {
            IList<Flete> f = new List<Flete>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    f = ct.Flete.ToList();

                }
                catch (Exception) { throw; }
            }
            return View(f);
        }*/

        

    }
}
