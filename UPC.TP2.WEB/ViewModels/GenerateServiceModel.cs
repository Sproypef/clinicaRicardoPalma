using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPC.TP2.WEB.Models;

namespace UPC.TP2.WEB.ViewModels
{
    public class GenerateServiceModel
    {
        public IEnumerable<T_PROGRAMACION_MEDICA> LIST_RETIRO_SERVICIO { get; set; }
        public IEnumerable<T_PROGRAMACION_MEDICA> LIST_ASIGNA_PLAN { get; set; }
        public IEnumerable<T_PLAN_DE_SALUD> LIST_PLAN_DE_SALUD { get; set; }
        public T_CONFIGURACION T_CONFIGURACION_RETIRAR { get; set; }
        public T_CONFIGURACION T_CONFIGURACION_ASIGNAR { get; set; }
    }
}