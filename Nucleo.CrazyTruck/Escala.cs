//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nucleo.CrazyTruck
{
    using System;
    using System.Collections.Generic;
    
    public partial class Escala
    {
        public int id { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public System.DateTime fecha { get; set; }
        public string tipo { get; set; }
        public int idFlete { get; set; }
    
        public virtual Flete Flete { get; set; }
    }
}
