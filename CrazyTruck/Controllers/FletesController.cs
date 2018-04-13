using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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
            string fol = DateTime.Now.ToString("yyyy_MM_dd")+"_"+Convert.ToString(rnd.Next(1000, 9999));

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
        public ActionResult obtenerInfoFlete(int idFlete)
        {
            //IList<Flete> opList = new List<Flete>();
            
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    Flete flete = ct.Flete
                        //.Include(t => t.Trailer)
                        //.Include(o => o.Operador)
                        //.Include(c => c.Carga)
                        //.Include(u => u.Usuario)
                        //.Include(e => e.Escala)
                        .Where(f => f.id == idFlete)
                        .SingleOrDefault();

                    string data = JsonConvert.SerializeObject(flete, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        
                    });

                    var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = Int32.MaxValue;

                    return jsonResult;

                }
                catch (Exception) { throw; }
            }
            
        }

        CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();

        //Metodos auxiliares
        [HttpGet]
        public JsonResult listaTrailers()
        {
            try
            {
                var listarTrailers = ct.Trailer.OrderBy(tr => tr.modelo)
                        .Select(t => new {
                            id = t.id,
                            matricula = t.matricula,
                            modelo = t.modelo,
                            anio = t.anio
                        })
                        .ToList();

                return Json(listarTrailers, JsonRequestBehavior.AllowGet);
            }
            catch (Exception) { throw; }
            
        }
        
        [HttpGet]
        public JsonResult listaOperadores()
        {
            try
            {
                var listarOperadores = ct.Operador.OrderBy(op => op.nombre)
                        .Select(o => new {
                            id = o.id,
                            numOperador = o.numOperador,
                            nombre = o.nombre,
                            apellido = o.apellido
                        })
                        .ToList();

                return Json(listarOperadores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception) { throw; }

        }
        
        [HttpGet]
        public JsonResult listaRemolques()
        {
            try
            {
                var listarGandolas = ct.Gandola.OrderBy(ga => ga.matricula)
                        .Select(g => new {
                            id = g.id,
                            matricula = g.matricula
                        })
                        .ToList();

                return Json(listarGandolas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception) { throw; }
            
        }
    }
}
