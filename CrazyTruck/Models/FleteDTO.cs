using Newtonsoft.Json;
using Nucleo.CrazyTruck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrazyTruck.Models
{
    [JsonObject(IsReference = true)]
    public class FleteDTO
    {
        public int id { get; set; }
        public string folio { get; set; }
        public int idTrailer { get; set; }
        public int idOperador { get; set; }
        public int idUsuario { get; set; }
        public System.DateTime fecha { get; set; }
        public ICollection<CargaDTO> Carga { get; set; }
    }
}