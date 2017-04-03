using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPC.TP2.WEB.Utils
{
    public static class Util
    {
        enum BitacoraEstados{
            ABIERTO,
            COMPLETO,
            RECHAZADO
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

    }
}