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
    
    public partial class T_INVESTIGACION_COMERCIAL
    {
        public T_INVESTIGACION_COMERCIAL()
        {
            this.T_CONFIGURACION = new HashSet<T_CONFIGURACION>();
            this.T_PLAN_DE_SALUD = new HashSet<T_PLAN_DE_SALUD>();
            this.T_PROYECTO_PLANSALUD = new HashSet<T_PROYECTO_PLANSALUD>();
        }
    
        public int id_investigacion_comercial { get; set; }
        public string nombre { get; set; }
        public string resultado_sexo { get; set; }
        public string resultado_edad { get; set; }
        public string resultado_servicio { get; set; }
        public string resultado_especialidad { get; set; }
        public string fecha_creacion { get; set; }
    
        public virtual ICollection<T_CONFIGURACION> T_CONFIGURACION { get; set; }
        public virtual ICollection<T_PLAN_DE_SALUD> T_PLAN_DE_SALUD { get; set; }
        public virtual ICollection<T_PROYECTO_PLANSALUD> T_PROYECTO_PLANSALUD { get; set; }
    }
}
