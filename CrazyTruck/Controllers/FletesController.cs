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

                List<Carga> cargas = new List<Carga>();
                
                Random rnd = new Random();
                string fol = DateTime.Now +Convert.ToString(rnd.Next(1000, 9999));

                Flete fl = new Flete();
                fl.folio = fol;
                fl.idOperador = flete.idOperador;
                fl.idTrailer = flete.idTrailer;
                fl.idUsuario = flete.idUsuario;
                fl.fecha = DateTime.Now;
                fl.Carga = new List<Carga>();
                foreach (Carga cg in fl.Carga)
                {                                 
                fl.Carga.Add(cg);
                }
                ct.Flete.Add(fl);
                ct.SaveChanges();

            //optener el id del flete
            var idFlete = ct.Flete.Where(op => op.folio == fol).FirstOrDefault(); 

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
