using Nucleo.CrazyTruck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrazyTruck.Controllers
{
    public class TrailersController : Controller
    {
        // GET: Trailers
        public ActionResult Lista()
        {
            IList<Trailer> t = new List<Trailer>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    t = ct.Trailer.ToList();

                }
                catch (Exception) { throw; }
            }
            return View(t);
        }

        //Metodos CRUD JSON
        //listar trailers despues de una accion
        public ActionResult listarTrailers()
        {
            IList<Trailer> t = new List<Trailer>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    t = ct.Trailer.ToList();

                }
                catch (Exception) { throw; }
            }
            return Json(t, JsonRequestBehavior.AllowGet);
        }

        //Agregar trailers
        [HttpPost]
        public JsonResult agregar(Trailer trailer)
        {
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    ct.Trailer.Add(trailer);
                    ct.SaveChanges();
                    //ct.Dispose();
                }
                catch (Exception) { throw; }
            }

            return Json(new { succes = true });
        }

        //Editar operadores
        [HttpPost]
        public JsonResult editar(Trailer trailer)
        {
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    Trailer t = ct.Trailer.Where(tr => tr.id == trailer.id).FirstOrDefault();
                    t.modelo = trailer.modelo;
                    t.anio = trailer.anio;
                    t.matricula = trailer.matricula;
                    t.color = trailer.color;
                    
                    ct.SaveChanges();

                }
                catch (Exception) { throw; }
            }

            return Json(new { succes = true });
        }

        [HttpPost]
        public JsonResult eliminar(int idTrailer)
        {
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {

                    Trailer t = ct.Trailer.Where(tr => tr.id == idTrailer).FirstOrDefault();
                    ct.Trailer.Remove(t);
                    ct.SaveChanges();
                }
                catch (Exception) { throw; }
            }

            return Json(new { succes = true });
        }

        //listar trailer por id
        public ActionResult tDeployInfoById(int id)
        {
            IList<Trailer> t = new List<Trailer>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    var trailer= ct.Trailer.Find(id);
                    t.Add(trailer);
                }
                catch (Exception) { throw; }
            }
            return Json(t, JsonRequestBehavior.AllowGet);
        }
    }
}