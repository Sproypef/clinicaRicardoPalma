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
    
    public partial class T_PERSONA_PLANSALUD
    {
        public int id_persona_plansalud { get; set; }
        public System.DateTime fecha_inicio { get; set; }
        public System.DateTime fecha_fin { get; set; }
        public int codPersona { get; set; }
        public int id_plan_salud { get; set; }
    
        public virtual T_PERSONA T_PERSONA { get; set; }
        public virtual T_PLAN_DE_SALUD T_PLAN_DE_SALUD { get; set; }
    }
}
