using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPC.TP2.WEB.Models;
using System.Web.Helpers;
using System.Collections;


namespace UPC.TP2.WEB.PlanSalud.Controllers
{
    public class GenerarServiciosPlanController : Controller
    {
        private BDCLINICAEntities db = new BDCLINICAEntities();
        // GET: /GenerarServiciosPlan/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChartServicios()
        {
            var _context = new BDCLINICAEntities();
            ArrayList xvalue = new ArrayList();
            ArrayList yvalue = new ArrayList();

            var result = (from c in _context.T_PLAN_SERVICIO select c);
            result.ToList().ForEach(rs => xvalue.Add(rs.id_plan_salud));
            result.ToList().ForEach(rs => yvalue.Add(rs.id_plan_servicio));

            new Chart(width: 500, height: 300, theme: ChartTheme.Vanilla3D)
            .AddTitle("Prueba de Grafico")
            .AddSeries("Default", chartType:"column", xValue:xvalue, yValues:yvalue)
            .Write("bpm");
            return View();
        }

    }
}
