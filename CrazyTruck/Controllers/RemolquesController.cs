using Nucleo.CrazyTruck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CrazyTruck.Controllers
{
    public class RemolquesController : Controller
    {
        // GET: Remolques
        public ActionResult Lista()
        {
            IList<Gandola> g = new List<Nucleo.CrazyTruck.Gandola>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
                try
                {
                    g = ct.Gandola.ToList();
                }
                catch (Exception)
                {
                    throw;
                }

            return View(g);
        }

        //Agregar gandola
        [HttpPost]
        public JsonResult agregar(Gandola gandola)
        {
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    ct.Gandola.Add(gandola);
                    ct.SaveChanges();
                    //ct.Dispose();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return Json(new { succes = true });
        }

        //Editar gandola
        [HttpPost]
        public JsonResult editar(Gandola gandola)
        {
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    Gandola g = ct.Gandola.Where(ga => ga.id == gandola.id).FirstOrDefault();
                    g.matricula = gandola.matricula;
                    g.capacidad = gandola.capacidad;
                    ct.SaveChanges();
                }

                catch (Exception) { throw; }
            }

            return Json(new { succes = true });
        }

        [HttpPost]
        public JsonResult eliminar(int idRemolque)
        {
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
              
                    Gandola g = ct.Gandola.Where(ga => ga.id == idRemolque).FirstOrDefault();
                    ct.Gandola.Remove(g);
                    ct.SaveChanges();
                }
                catch (Exception) { throw; }
            }

            return Json(new { succes = true });
        }
    }


    }
