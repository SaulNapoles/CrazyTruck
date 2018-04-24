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

        CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();


        [HttpGet]
        public JsonResult listaEscalas(string idFlete)
        {
            try
            {
                int idF = Int32.Parse(idFlete.ToString());

                var listarEscalas = ct.Escala.OrderBy(es => es.fecha)
                        .Where(es => es.idFlete == idF)
                        .Select(e => new {
                            id = e.id,
                            latitud = e.latitud,
                            longitud = e.longitud,
                            nombre = e.nombre,
                            descripcion = e.descripcion,
                            fecha = e.fecha
                        })
                        .ToList();

                return Json(listarEscalas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception) { throw; }

        }

        public JsonResult agregarEscala(Escala escala)
        {
            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();

            try
            {
                int idF = Int32.Parse(escala.idFlete.ToString());

                escala.idFlete = idF;
                escala.tipo = "ESTIMADA";

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

        public JsonResult obtenerEscala(int id)
        {

            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    var escalaObj = ct.Escala
                        //.Include(f => f.Flete)
                        .Where(es => es.id == id)
                        .Select(e => new {
                            id = e.id,
                            latitud = e.latitud,
                            longitud = e.longitud,
                            nombre = e.nombre,
                            descripcion = e.descripcion,
                            fecha = e.fecha
                        })
                        .ToList();

                    return Json(escalaObj, JsonRequestBehavior.AllowGet);
                }
                catch (Exception) { throw; }

            }
            
        }

     
    }
}