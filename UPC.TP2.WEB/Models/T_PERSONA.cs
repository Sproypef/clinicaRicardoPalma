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
    
    public partial class T_PERSONA
    {
        public T_PERSONA()
        {
            this.T_EMPLEADO = new HashSet<T_EMPLEADO>();
            this.T_EMPRESA = new HashSet<T_EMPRESA>();
            this.T_PACIENTE = new HashSet<T_PACIENTE>();
            this.T_PERSONA_PLANSALUD = new HashSet<T_PERSONA_PLANSALUD>();
            this.T_PROGRAMACION_MEDICA = new HashSet<T_PROGRAMACION_MEDICA>();
        }
    
        public int codPersona { get; set; }
        public string nompersona { get; set; }
    
        public virtual ICollection<T_EMPLEADO> T_EMPLEADO { get; set; }
        public virtual ICollection<T_EMPRESA> T_EMPRESA { get; set; }
        public virtual ICollection<T_PACIENTE> T_PACIENTE { get; set; }
        public virtual ICollection<T_PERSONA_PLANSALUD> T_PERSONA_PLANSALUD { get; set; }
        public virtual ICollection<T_PROGRAMACION_MEDICA> T_PROGRAMACION_MEDICA { get; set; }
    }
}
