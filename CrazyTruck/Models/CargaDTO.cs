using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrazyTruck.Models
{
    [JsonObject(IsReference = true)]
    public class CargaDTO
    {
        public int id { get; set; }
        public int idGandola { get; set; }
        public int idFlete { get; set; }
        public string descripcion { get; set; }
        public decimal peso { get; set; }
    }
}