//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UPC.TP2.WEB.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_ESPECIALIDAD_SERVICIO
    {
        public T_ESPECIALIDAD_SERVICIO()
        {
            this.T_PLAN_ESPECIALIDADxSERVICIO = new HashSet<T_PLAN_ESPECIALIDADxSERVICIO>();
            this.T_PROGRAMACION_MEDICA = new HashSet<T_PROGRAMACION_MEDICA>();
        }
    
        public int id_especialidad_servicio { get; set; }
        public Nullable<System.DateTime> fecha_ingreso { get; set; }
        public string estado { get; set; }
        public Nullable<int> id_servicio { get; set; }
        public Nullable<int> id_especialidad { get; set; }
    
        public virtual T_ESPECIALIDAD_MEDICA T_ESPECIALIDAD_MEDICA { get; set; }
        public virtual ICollection<T_PLAN_ESPECIALIDADxSERVICIO> T_PLAN_ESPECIALIDADxSERVICIO { get; set; }
        public virtual ICollection<T_PROGRAMACION_MEDICA> T_PROGRAMACION_MEDICA { get; set; }
        public virtual T_SERVICIO_SALUD T_SERVICIO_SALUD { get; set; }
    }
}
