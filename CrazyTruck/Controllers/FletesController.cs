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
        // GET
        public ActionResult Lista()
        {
            IList<Flete> listarFletes = new List<Flete>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                { //obtener datos folio triler operador fecha 

                    listarFletes = ct.Flete.Include(t => t.Trailer).Include(o => o.Operador).ToList();


                }
                catch (Exception) { throw; }

                return View(listarFletes);

            }   
        }

                //Metodos CRUD JSON
        //listar flete Trailer Operador despues de una accion
        public ActionResult listarInfoFletes()
        {
            IList<Flete> listarFletes = new List<Flete>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                { //obtener datos folio triler operador fecha 

                    listarFletes = ct.Flete.Include(t => t.Trailer).Include(o => o.Operador).ToList();


                }
                catch (Exception) { throw; }

                return Json(listarFletes, JsonRequestBehavior.AllowGet);
            }
        }

        //agregar flete y carga
        public JsonResult agregarFlete(Flete flete)
        {

            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();

            Random rnd = new Random();
            string fol = DateTime.Now.ToString("yyyy_MM_dd") + "_" + Convert.ToString(rnd.Next(1000, 9999));

            Flete fl = new Flete
            {
                folio = fol,
                idOperador = flete.idOperador,
                idTrailer = flete.idTrailer,
                idUsuario = 20,
                fecha = DateTime.Now
            };

            ct.Flete.Add(fl);
            ct.SaveChanges();

            //optener el flete generado
            var idFlete = ct.Flete.Where(op => op.folio == fol).FirstOrDefault();

            foreach (Carga carga in flete.Carga)
            {
                carga.idFlete = idFlete.id;
                ct.Carga.Add(carga);
            }

            ct.SaveChanges();

            return Json(idFlete.id, JsonRequestBehavior.AllowGet);
        }

        //desplegar info de flete por id
        public ActionResult fDeployInfoById(int id)
        {
            IList<Flete> opList = new List<Flete>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    opList = ct.Flete
                        .Include(t => t.Trailer)
                        .Include(o => o.Operador)
                        .Include(c=> c.Carga)
                        .Where(f=> f.id == id)
                        .ToList();

                }
                catch (Exception) { throw; }
            }
            return Json(opList, JsonRequestBehavior.AllowGet);
        }

        //eliminar flete
        public ActionResult eliminarFlete(int id)
        {
            bool bandera = false;
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    Flete f = ct.Flete.Where(op => op.id == id).FirstOrDefault();
                    ct.Flete.Remove(f);
                    ct.SaveChanges();
                    bandera = true;
                }
                catch (Exception) { bandera = false; }
            }
            return Json(bandera, JsonRequestBehavior.AllowGet);
        }

        //editar flete
        public JsonResult editarFlete(Flete flete)
        {

            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();
            try
            {
                Flete s = ct.Flete.Where(op => op.id == flete.id).FirstOrDefault();
                s.folio = flete.folio;
                s.idOperador = flete.idOperador;
                s.idTrailer = flete.idTrailer;
                s.idUsuario = 20;
                s.fecha = flete.fecha;

                ct.SaveChanges();

                foreach (Carga carga in flete.Carga)
                {
                    carga.idFlete = flete.id;
                    ct.Carga.Add(carga);
                }

                ct.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
           

            return Json(idFlete.id, JsonRequestBehavior.AllowGet);
        }

        //Metodos auxiliares
        [HttpGet]
        public JsonResult listarTrailers()
        {
            List<Trailer> listarTrailers = new List<Trailer>();
            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();
            try
            {
                listarTrailers = ct.Trailer.ToList();
            }
            catch (Exception) { throw; }


            return Json(listarTrailers, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult listarGandolas()
        {
            List<Gandola> listarGandolas = new List<Gandola>();
            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();
            try
            {
                listarGandolas = ct.Gandola.ToList();
            }
            catch (Exception) { throw; }


            return Json(listarGandolas, JsonRequestBehavior.AllowGet);
        }
    }
}
