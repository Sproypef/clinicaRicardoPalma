using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UPC.TP2.WEB.Utils
{
    public static class Util
    {
        public static Dictionary<int, string> BitacoraEstados(){
            Dictionary<int, string> estados = new Dictionary<int, string>();
            estados.Add(1, "ABIERTO");
            estados.Add(2, "CERRADO");
            estados.Add(3, "RECHAZADO");
            return estados;
        }

        public static Dictionary<int, string> BitacoraTipos()
        {
            Dictionary<int, string> tipos = new Dictionary<int, string>();
            tipos.Add(1, "APORTES");
            tipos.Add(2, "RECLAMOS");
            return tipos;
        }

        public static Dictionary<int, string> EstrategiaComercialDetalleEstado()
        {
            Dictionary<int, string> tipos = new Dictionary<int, string>();
            tipos.Add(1, "ABIERTO");
            tipos.Add(2, "EJECUCION");
            tipos.Add(3, "COMPLETO");
            return tipos;
        }

        public static JsonSerializerSettings jsonNoLoopSettings()
        {
            return new JsonSerializerSettings {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
        public static JsonSerializer jsonNoLoop()
        {
            return new JsonSerializer() {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public static object getToday()
        {
            return DateTime.Now;
        }

    }
}