using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using Nucleo.CrazyTruck;

namespace CrazyTruck.Controllers
{
    public class EscalaController : Controller
    {
        //// GET: Escala
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public JsonResult agregarEscala(Escala escala)
        {
            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();

            try
            {
                ct.Escala.Add(escala);
                ct.SaveChanges();
                //ct.Dispose();
            }
            catch (Exception) { throw; }

            return Json(new { succes = true });
        }

        public JsonResult eliminarEscala(int id)
        {
            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();
            try
            {
                Escala e = ct.Escala.Where(es => es.id == id).FirstOrDefault();
                ct.Escala.Remove(e);
                ct.SaveChanges();
            }
            catch (Exception) { throw; }

            return Json(new { succes = true });
        }

        public JsonResult editarEscala(Escala escala)
        {
            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();
            try
            {
                Escala e = ct.Escala.Where(es => es.id == escala.id).FirstOrDefault();
                e.latitud = escala.latitud;
                e.longitud = escala.longitud;
                e.nombre = escala.nombre;
                e.tipo = escala.tipo;
                e.descripcion = escala.descripcion;
                e.fecha = escala.fecha;
                e.idFlete = e.idFlete;

                ct.SaveChanges();
            }
            catch (Exception) { throw; }

            return Json(new { succes = true });
        }

        public JsonResult eDeployInfoById(int id)
        {
            IList<Escala> escList = new List<Escala>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    escList = ct.Escala
                        .Include(f => f.Flete )
                        .Where(e => e.id == id)
                        .ToList();
                }
                catch (Exception) { throw; }
            }
            return Json(escList, JsonRequestBehavior.AllowGet);
        }

     
    }
}