using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleo.CrazyTruck.Models
{
   public class Flete
    {

        public int id { get; set; }
        public string folio { get; set; }
        public int idTrailer { get; set; }
        public int idOperador { get; set; }
        public int idUsuario { get; set; }
        public System.DateTime fecha { get; set; }

      
        public ICollection<Carga> Carga { get; set; }
        public ICollection<Escala> Escala { get; set; }
        public Operador Operador { get; set; }
        public Trailer Trailer { get; set; }
        public Usuario Usuario { get; set; }



        //Metodo para agregar
        //Metodo para eliminar
        //Metodo para buscar
        public List<Flete> getListaFletes(Flete buscar)
        {
           
            try
            {               
                CrazyTruckEntitiesCn context = new CrazyTruckEntitiesCn();
                          
                List<Flete> list = new List<Flete>();
                var datos = context.Flete.Include(d => d.Carga).Where(sd => sd.id == buscar.id).ToList();

                ///codigo para poner xD
                return list;
           
        }catch (Exception)
            {

                throw;
            }
   }


        //Metodo para editar



    }
}
