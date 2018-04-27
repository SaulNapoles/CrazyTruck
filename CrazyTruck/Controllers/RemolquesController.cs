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
            IList<Gandola> g = new List<Gandola>();
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

                //Metodos CRUD JSON
        //listar Remolques despues de una accion
        public ActionResult listarRemolques()
        {
            IList<Gandola> g = new List<Gandola>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    g = ct.Gandola.ToList();

                }
                catch (Exception) { throw; }
            }
            return Json(g, JsonRequestBehavior.AllowGet);
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

        //listar Remolques por id
        public ActionResult obtenerRemolque(int idRemolque)
        {
            
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    var remolqueObj = ct.Gandola
                        .Where(ga => ga.id == idRemolque)
                        .Select(g => new {
                            id = g.id,
                            matricula = g.matricula,
                            capacidad = g.capacidad
                        })
                        .ToList();
               
                    return Json(remolqueObj, JsonRequestBehavior.AllowGet);
                }
                catch (Exception) { throw; }
            }

        }
    }


    }
