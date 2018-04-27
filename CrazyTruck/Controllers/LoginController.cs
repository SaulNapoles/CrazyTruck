using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nucleo.CrazyTruck;
using Nucleo.CrazyTruck.Models;
namespace CrazyTruck.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            //Usuario e = new Usuario();
            //e.nombre = "Mar";
            //e.apellido = "Iba";
            //e.rol = "Admin";
            //e.email = "kaese_57@hotmail.com";
            //e.password = "1234";
            //addUsuario();

            return View();
        }

        [HttpPost]
        public ActionResult startSession(string userEmail, string userPass)
        {
            CrazyTruckDBEntitiesCn cn = new CrazyTruckDBEntitiesCn();

            UsuarioModel lg = new UsuarioModel();
            Usuario u = new Usuario();
            u = lg.login(userEmail, userPass);
           if(u!=null)
            {

                Session["nombre"] = u.nombre;
                Session["apellido"] = u.apellido;
                Session["email"] = u.email;
                Session["rol"] = u.rol;
                Session["id"] = u.id;
                //return Json(Session, JsonRequestBehavior.AllowGet);
               return RedirectToAction("Inicio", "Home");
            }
            else
            {
                //string errorMsg = "Error, no se encontro al usuario";
                //return Json(errorMsg, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index", "Login");
            }

            
        }

        [HttpPost]
        public ActionResult addUsuario(Usuario usuario)
        {
            
            try
            {
                UsuarioModel s = new UsuarioModel();
                if (s.nuevoUsuario(usuario) == true)
                {
                    return Json(new { success = true });
                } else
                {
                    return Json(new { failed = false });
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult activarUsuario(string token)
        {
            UsuarioModel m = new UsuarioModel();
            if (m.activarUsuarioToken(token)== true)
            {
               return RedirectToAction("Index", "Login");
            }else
            {
                return RedirectToAction("Index","Login", false);
            }

        }

        [HttpPost]
        public ActionResult LogOut()
        {
            Session["nombre"] = null;
            Session["apellido"] = null;
            Session["email"] = null;
            Session["rol"] = null;
            Session["id"] = null;
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Response.AddHeader("Cache-control", "no-store, must-revalidate, private, no-cache");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Response.AppendToLog("window.location.reload();");

            return Json(new {success = true });
                //RedirectToAction("Index", "Login");
        }
    }
}