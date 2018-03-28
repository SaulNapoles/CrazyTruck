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
            IList<Operador> opp = new List<Operador>();
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try {
                    //obtener datos
                    opp = ct.Operador.ToList();

                }
                catch (Exception) { throw; }
            }
            return View(opp);
        }

        //Agregar operadores
        [HttpPost]
        public JsonResult agregar(Operador operador)
        {
           
            CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn();
            try
            {
                Random rnd = new Random();
                string numerOp = operador.curp.Substring(0, 3) + rnd.Next(1000, 9999);

                operador.numOperador = numerOp;


                ct.Operador.Add(operador);

                ct.SaveChanges();
       
            }
            catch (Exception) { throw; }

            return Json(true, JsonRequestBehavior.AllowGet);
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
        public JsonResult eliminar(string id)
        {
            using (CrazyTruckDBEntitiesCn ct = new CrazyTruckDBEntitiesCn())
            {
                try
                {
                    int sId = Convert.ToInt32(id);
                    Operador o = ct.Operador.Where(op => op.id == sId).FirstOrDefault();
                    ct.Operador.Remove(o);
                    ct.SaveChanges();

                }
                catch (Exception) { throw; }
            }

            return Json(new { succes = true });
        }
        [HttpGet]
        public JsonResult listarOperadores()
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
            return Json(new{List = o });
         }
        
    }
}
                