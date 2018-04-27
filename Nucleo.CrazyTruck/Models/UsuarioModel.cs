using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace Nucleo.CrazyTruck.Models
{
    public class UsuarioModel
    {
        CrazyTruckDBEntitiesCn context = new CrazyTruckDBEntitiesCn();

        public bool nuevoUsuario(Usuario user)
        {
            try
            {
                if (user!=null)
                {
                    var temp = context.Usuario.Where(u=> u.email ==user.email).FirstOrDefault();
                    if (temp == null)
                    {
                        user.activado = false;
                        user.passRequest = false;
                        user.password = SHA256Encrypt(user.password);
                        user.rol = "User";
                        context.Usuario.Add(user);
                        context.SaveChanges();
                        activateEmail(user.email, user.password);
                        context.Dispose();
                        return true;
                    }else
                    {
                        return false;
                    }
                }else
                {
                    return false;
                }
                
            }
            catch (Exception)
            {

                throw;
            }
         

        }

        public void activateEmail(string correo, string password)
        {
            //Inicializan las variables del cliente, las credenciales y el mensaje
            NetworkCredential login;
            SmtpClient client;
            MailMessage msg = new MailMessage();

            //Se obtiene un codigo aleatorio como token
            string token = codigoAleatorio();
            //Se inserta el token que se le va a dar a dicha activacion respecto al correo que lo solicito
            modificarTokenActivacion(correo, token, password);

            //Se agrega la informacion del mensaje
            msg.From = new MailAddress("crazytruckcc@gmail.com");
            msg.Subject = "Activar correo";
            msg.To.Add(new MailAddress(correo));
            msg.Body = "Se ha creado una cuenta con este correo en la pagina crazy truck, para activar la cuenta, favor" +
                "de hacer click en el siguiente enlace o copiar y pegar en su navegador: http://localhost:62560/Login/activarUsuario?=" + token;

            //Se declaran las variables con la siguiente informacion
            
            //Las credenciales almacena la informacion del correo del cual se envia 
            login = new NetworkCredential("crazytruckcc@gmail.com", "Cece2018");

            //Se declara el host y el puerto por el cual se va a enviar el correo
            client = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            client.EnableSsl = true;
            client.Credentials = login;
            client.Send(msg);
        }

        //Metodo el cual modifica el token del correo al cual se va a enviar el email de ativacion respecto al correo
        public void modificarTokenActivacion(string correo, string token, string password)
        {
            //int idUsu= context.Usuario.Single(d => d.email == correo).id;
            Usuario usu = context.Usuario.Where(e => e.email == correo && e.password == password).FirstOrDefault();
            usu.tokenActivacion = token;
            context.SaveChanges();

        }

        //Metodo el cual envia un correo de recuperar contrasenia al correo asignado
        public void RecoverPass(string correo)
        {
            NetworkCredential login;
            SmtpClient client;
            MailMessage msg = new MailMessage();

            string token = codigoAleatorio();
            editaRecuperar(correo, token);

            msg.From = new MailAddress("crazytruckcc@gmail.com");
            msg.Subject = "Activacion";
            msg.To.Add(new MailAddress (correo));
            msg.Body = "Se ha creado solicitado una recuperacion de contrasenia para su cuenta de crazy truck, " +
                "para recuperar dicha contrasenia favor de hacer click en el siguiente enlace o copiar y pegar "+
                "en su navegador: http://localhost:62560/recuperarUsuario?=" + token;
            
            login = new NetworkCredential("crazytruckcc@gmail.com", "Cece2018");
            client = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            client.EnableSsl = true;
            client.Credentials = login;
            client.Send(msg);
        }

        //Metodo el cual genera un codigo aleatorio con la clase Guid la cual te da un total de 16 digitos
        public string codigoAleatorio()
        {
            //Declaramos una variable int la cual se va a utilizar para indicar cuantos caracteres de Guid va a tener el token
            int longitud = 7;
            //Aqui la variable miGuid ya tiene un valor de 16 digitos, por ejemplo "1254-4828-8448-8549"
            Guid miGuid = Guid.NewGuid();
            //Aqui lo pasamos a string para poder retornarlo
            //Luego le decimos que las partes donde esten guiones (los cuales los pone solo la clase Guid al generar un string)
            //sean eliminados y el string se recorra a la misma vez con la campo "string.Empty"
            //Ejemplo "Guid solo" = 1254-4828-8448-8549 "ConReplace"= 1254482884488549
            //Por ultimo, con el campo ".Substring" le indicamos que desde el primer valor hasta el ultimo valor indicado en
            //la variable "longitud" es lo que se va a regresar en la variable token
            string token = miGuid.ToString().Replace("-", string.Empty).Substring(0, longitud);
            return token;
        }

        //Metodo el cual edita los siguientes datos del correo al cual se le esta modificando la contrasenia 
        public void editaRecuperar(string correo, string token)
        {
            int idT = context.Usuario.Single(d => d.email == correo).id;
            Usuario usu = context.Usuario.Where(a => a.id == idT).First();

            usu.passRequest = true;
            usu.tokenPassword = token;

            context.SaveChanges();
        }

        //Metodo el cual comprueba que el logeo que realizado correctamente
        public Usuario login (string correo, string contrasenia)
        {
            try
            {
                Usuario usu = context.Usuario.Where(d => d.email == correo).FirstOrDefault();

              

                if (usu != null)
                {
                    //Usuario temUser = new Usuario();
                    string pass = SHA256Encrypt(contrasenia);
                    if (usu.email == correo && usu.password == pass && usu.activado == true)
                    {
                        //temUser.apellido = usu.apellido.Trim();
                        //temUser.nombre = usu.nombre.Trim();
                        //temUser.rol = usu.rol.Trim();
                        //temUser.email = usu.email.Trim();
                        //temUser.id = usu.id;
                        Console.Write("Se encontró el usuario con los datos correctos");
                        return usu;
                    }
                    else
                    {
                        Console.Write("No se pudo encontrar al usuario");
                        return null;
                    }

                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
            

        }


        private string SHA256Encrypt(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        //Metodo que encripta la contraseya tanto al momento de crear al usuario como al querer cambiar la contrasenia
        public string passHash(string pass, string emailSalt)
        {
            string contra = pass;

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPassHash = Convert.ToBase64String(hashBytes);

            return savedPassHash;
        }

        //Metodo el cual se utiliza al momento de hacer click en el enlace del correo, este metodo jala el id del token
        //Y busca en la base de datos el email registrado con ese token para devolver un usuario y de ahi mdoificar la contraseyia
        public Usuario buscarPorTokenPass(string token)
        {
            Usuario usu = context.Usuario.Single(d => d.tokenPassword == token);

            if (usu.Equals(null))
            {
                Console.Write("No se encontró ningun usuario con ese token o no hay token");
                return null;
            }
            else
            {
                Console.Write("Se encontró el usuario");
                return usu;
            }
        }

        //Metodo para restablecer los datos del usuario respecto a los tokens modificaos al solicitar la contraseya
        //Se declaran con sus valores predeterminados
        public void restablecerTokenPass(int idT)
        {
            Usuario usu = context.Usuario.Single(d=> d.id == idT);
            usu.passRequest = false;
            usu.tokenPassword = "";
            context.SaveChanges();
        }

        //Metodo para buscar un usuario por su token de activacion, este al momento de haber hecho click en el enlace de 
        //Actiacion buscara dicho usuario para poder manipualr sus datos, como modificar sus tokens y marcar como activado
        public Usuario buscarPorTokenActivar(string token)
        {
            Usuario usu = context.Usuario.Single(d => d.tokenActivacion == token);
            if (usu.Equals(null))
            {
                Console.Write("No se encontro usuario o token no existe");
                return null;
            }
            else
            {
                Console.Write("Se contró el usuario");
                return usu;
            }
        }

        //Metodo el cual se usa para marcar como activado el usuario y restablecer sus valores
        public bool activarUsuarioToken(string token)
        {

            Usuario usu = context.Usuario.Where(d=> d.tokenActivacion == token).FirstOrDefault();
            if (usu!=null)
            {
                usu.activado = true;
                usu.tokenActivacion = "";
                context.SaveChanges();
                return true;
            }else
            {
                return false;
            }          
        }
    }
}
