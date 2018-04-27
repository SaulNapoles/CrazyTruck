using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CrazyTruck.Models;
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
        public JsonResult listarInfoFletes()
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
                idUsuario = 28,
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
        public JsonResult obtenerInfoFlete(int idFlete)
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

                    
                    //Mapear objeto
                    var fleteDTO = Mapper.Map<FleteDTO>(flete);

                    List<CargaDTO> list = new List<CargaDTO>();

                    ICollection<Carga> cargas = ct.Carga
                        .OrderBy(ga => ga.idGandola)
                        .Where(c => c.idFlete == idFlete).ToList();

                    foreach (Carga c in cargas)
                    {
                        list.Add(Mapper.Map<CargaDTO>(c));
                    }
                    
                    ICollection<CargaDTO> col = list;
                    fleteDTO.Carga = col;

                    /*string data = JsonConvert.SerializeObject(flete, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });

                    var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = Int32.MaxValue;

                    return jsonResult;*/

                    /*var filePath = @"Fletes/somewhere.json";
                    //var path = Server.MapPat('.')
                    //Console.Write("Directorio:", path);

                    using (var fs = new FileStream(filePath, FileMode.CreateNew))
                    using (var sw = new StreamWriter(fs))
                    using (var jw = new JsonTextWriter(sw))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(jw, flete);
                    }*/

                    /*string json = JsonConvert.SerializeObject(flete, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    //write string to file
                    System.IO.File.WriteAllText(Server.MapPath("~/Views/Fletes/LocalJSONFile.json"), json);*/

                    //serealizar a json y guardarlo en un archivo local como cache
                    using (TextWriter writer = new StreamWriter(Server.MapPath("~/LocalJSONFile.json")))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(writer, fleteDTO);
                    }

                    return Json(new { succes = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception) { throw; }
            }
            
        }

        //eliminar flete
        public ActionResult eliminarFlete(int id)
        {
            bool succes = false;
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    Flete f = ct.Flete.Where(op => op.id == id).FirstOrDefault();
                    ct.Flete.Remove(f);
                    ct.SaveChanges();
                    succes = true;
                }
                catch (Exception) { succes = false; }
            }
            return Json(succes, JsonRequestBehavior.AllowGet);
        }

        //editar flete
        public JsonResult editarFlete(Flete flete)
        {

            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();
            try
            {
                Flete f = ct.Flete.Where(fl => fl.id == flete.id).FirstOrDefault();
                f.idOperador = flete.idOperador;
                f.idTrailer = flete.idTrailer;

                //ct.SaveChanges();

                foreach (Carga carga in flete.Carga)
                {
                    if(carga.id.Equals(0))
                    {
                        carga.idFlete = flete.id;
                        ct.Carga.Add(carga);

                        ct.SaveChanges();
                    }
                    else if(carga.idGandola.Equals(0))
                    {
                        ct.Carga.Remove(carga);

                        ct.SaveChanges();
                    }
                    else
                    {
                        Carga c = ct.Carga.Where(ca => ca.id == carga.id).FirstOrDefault();
                        c.idGandola = carga.idGandola;
                        c.descripcion = carga.descripcion;
                        c.peso = carga.peso;

                        ct.SaveChanges();
                    }
                    
                }
                
            }
            catch (Exception) { throw; }

            return Json(new { succes = true });
        }

        //Metodos auxiliares
        CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();
        
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
