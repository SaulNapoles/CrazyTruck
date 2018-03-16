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
            using (CrazyTruckEntitiesCn ct = new CrazyTruckEntitiesCn())
            {
                try
                {
                    //obtener datos
                    opp = ct.Operador.ToList();
           
                }
                catch (Exception)
                {

                    throw;
                }
               
            }
                return View(opp);
        }

        [HttpPost]
        public ActionResult agregar(string operadorNombre, string operadorApellido,
                                    string operadorDireccion, string operadorCurp, string operadorNumLicencia,
                                    string operadorTelefono, string operadorNumOperador, string operadorNss)
        {

            using(CrazyTruckEntitiesCn ct = new CrazyTruckEntitiesCn())
            {
                try
                {
                    Operador op = new Operador();
                    op.nombre = operadorNombre;
                    op.apellido = operadorApellido;
                    op.direccion = operadorDireccion;
                    op.curp = operadorCurp;
                    op.numLicencia = operadorNumLicencia;
                    op.telefono = operadorTelefono;
                    op.numOperador = operadorNumOperador;
                    op.nss = operadorNss;

                    ct.Operador.Add(op);
                    ct.SaveChanges();


                }
                catch (Exception)
                {

                    throw;
                }


            }


            return RedirectToAction("Operadores");
        }
    }
}