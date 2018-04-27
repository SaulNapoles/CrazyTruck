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
            if (Session != null)
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
            else
            {
                return RedirectToAction("Index", "Login");
            }
         
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
        public ActionResult oDeployInfoById(int id)
        {
            IList<Operador> o = new List<Operador>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    //obtener datos
                    var getOp = ct.Operador.Find(id);
                    o.Add(getOp);

                }
                catch (Exception) { throw; }
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }

    }
}
                