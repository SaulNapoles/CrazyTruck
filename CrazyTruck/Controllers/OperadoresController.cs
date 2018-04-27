using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nucleo.CrazyTruck.Models;
using Nucleo.CrazyTruck;
using System.Data.Entity;

namespace CrazyTruck.Controllers
{
    public class OperadoresController : Controller
    {
        // GET: Operadores
        public ActionResult Lista()
        {
            IList<Operador> o = new List<Operador>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    o = ct.Operador.ToList();

                }
                catch (Exception) { throw; }
            }
            return View(o);
        }

                //Metodos CRUD JSON
        //listar operadores despues de una accion
        public ActionResult listarOperadores()
        {
            IList<Operador> o = new List<Operador>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    o = ct.Operador.ToList();

                }
                catch (Exception) { throw; }
            }
            return Json(o,JsonRequestBehavior.AllowGet);
        }

        //Agregar operadores
        [HttpPost]
        public JsonResult agregar(Operador operador)
        {
            using(CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    Random rnd = new Random();
                    string numerOp = operador.curp.Substring(0, 4) + rnd.Next(1000,9999);

                    operador.numOperador = numerOp ; 

                       
                    ct.Operador.Add(operador);
                    ct.SaveChanges();
                    //ct.Dispose();
                }
                catch (Exception){throw;}
            }
            
            return Json(new { succes= true});
        }

        //Editar operadores
        [HttpPost]
        public JsonResult editar(Operador operador)
        {
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    Operador s = ct.Operador.Where(op => op.id == operador.id).FirstOrDefault();
                    s.nombre = operador.nombre;
                    s.apellido = operador.apellido;
                    s.direccion = operador.direccion;
                    s.curp = operador.curp;
                    s.numLicencia = operador.numLicencia;
                    s.telefono = operador.telefono;
                    s.nss = operador.nss;
                    
                    ct.SaveChanges();

                }
                catch (Exception) { throw; }
            }

            return Json(new { succes = true });
        }


        [HttpPost]
        public JsonResult eliminar(int idOperador)
        {
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {              
                    Operador o = ct.Operador.Where(op => op.id == idOperador).FirstOrDefault();
                    ct.Operador.Remove(o);
                    ct.SaveChanges();
                }
                catch (Exception) { throw; }
            }

            return Json(new { succes = true });
        }

        //desplegar info de operador por id
        public ActionResult obtenerOperador(int idOperador)
        {
            
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    var operadorObj = ct.Operador
                        .Where(op => op.id == idOperador)
                        .Select(o => new
                        {
                            id = o.id,
                            numOperador = o.numOperador,
                            nombre = o.nombre,
                            apellido = o.apellido,
                            nss = o.nss,
                            curp = o.curp,
                            numLicencia = o.numLicencia,
                            direccion = o.direccion,
                            telefono = o.telefono
                        })
                        .ToList();

                    return Json(operadorObj, JsonRequestBehavior.AllowGet);

                }
                catch (Exception) { throw; }
            }
        }

    }
}
                