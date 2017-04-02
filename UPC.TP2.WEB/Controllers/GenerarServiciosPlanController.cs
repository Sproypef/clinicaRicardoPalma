using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPC.TP2.WEB.Models;
using UPC.TP2.WEB.ViewModels;
using UPC.TP2.WEB.Utils;

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

        public ActionResult Generar()
        {
            //Validation dates

            HttpRequestBase request = Request.Form.AllKeys.Length == 0 ? (HttpRequestBase)TempData["request"] : Request;
            ViewBag.Message = TempData["Message"];

            string fi = request["fecha_inicio"] ?? "";
            string ff = request["fecha_fin"] ?? "";

            ViewBag.FechaInicio = fi;
            ViewBag.FechaFin = ff;

            if (fi == "" || ff == "")
            {
                ViewBag.Message = "Debe ingresar ambas fechas";
                return View("Index");
            }
            else
            {
                try
                {

                    DateTime dtfi = DateTime.Parse(fi);
                    DateTime dtff = DateTime.Parse(ff);

                    long dif = dtff.CompareTo(dtfi);

                    if (dif < 0)
                    {
                        ViewBag.Message = "La fecha final no debe ser menor que la inicial";
                        return View("Index");
                    }

                }
                catch (FormatException)
                {
                    ViewBag.Message = "Verifique que las fechas sean correctas";
                    return View("Index");
                }
            }

            //-- BEGIN: Full logic 

            DateTime FechaInicio = DateTime.Parse(fi);
            DateTime FechaFin = DateTime.Parse(ff);

            List<T_PROGRAMACION_MEDICA> ret_serv = db.T_PROGRAMACION_MEDICA
                .Where(x => x.fecha >= FechaInicio && x.fecha <= FechaFin)
                .OrderBy(x => x.id_persona_plansalud)
                .ToList();

            List<T_PROGRAMACION_MEDICA> asi_plan = db.T_PROGRAMACION_MEDICA
                .Where(x => x.fecha >= FechaInicio && x.fecha <= FechaFin)
                .OrderBy(x => x.id_persona_plansalud)
                .ToList();

            List<T_ESPECIALIDAD_MEDICA> esp = db.T_ESPECIALIDAD_MEDICA.ToList();
            List<T_ESPECIALIDAD_SERVICIO> esp_serv = db.T_ESPECIALIDAD_SERVICIO.ToList();
            List<T_PLAN_DE_SALUD> pla_salu = db.T_PLAN_DE_SALUD.ToList();
            List<T_PERSONA_PLANSALUD> per_pla_salu = db.T_PERSONA_PLANSALUD.ToList();

            T_CONFIGURACION config_asignar = db.T_CONFIGURACION.Where(x => x.indicador == "asignar_servicio").FirstOrDefault();
            T_CONFIGURACION config_retirar = db.T_CONFIGURACION.Where(x => x.indicador == "retirar_servicio").FirstOrDefault();

            //To json ret_serv
            object obj_ret_serv = ret_serv
                .Join(db.T_PERSONA, rs => rs.codPersona, pe => pe.codPersona, (rs, pe)=> new { rs, pe})
                .Join(db.T_PERSONA_PLANSALUD, rs_pe => rs_pe.pe.codPersona, pp => pp.codPersona, (rs_pe, pp) => new { rs_pe.rs, rs_pe.pe, pp })
                .Join(db.T_ESPECIALIDAD_SERVICIO, ret_pp => ret_pp.rs.id_especialidad_servicio, esp_ser => esp_ser.id_especialidad_servicio, (ret_pp, esp_ser) => new { ret_pp.rs, ret_pp.pe, ret_pp.pp, esp_ser })
                .Where(x => x.rs.fecha >= x.pp.fecha_inicio && x.rs.fecha <= x.pp.fecha_fin)
                .GroupBy(gb => new { gb.pp.id_plan_salud, gb.esp_ser.id_especialidad_servicio }, (key, group) => new { id_plan_salud = key.id_plan_salud, id_esp_ser = key.id_especialidad_servicio, group = group.ToList() })
                .Select(x => new
                {
                    id_plan_salud = x.id_plan_salud,
                    id_esp_ser = x.id_esp_ser,
                    id_especialidad = x.group.First().esp_ser.id_especialidad,
                    id_servicio = x.group.First().esp_ser.T_ESPECIALIDAD_MEDICA.nomEspecialidad,
                    programacion = x.group,
                    nombre_plan_salud = x.group.First().pp.T_PLAN_DE_SALUD.nombre_plan,
                    nombre_especialidad = x.group.First().esp_ser.T_ESPECIALIDAD_MEDICA.nomEspecialidad,
                    nombre_servicio = x.group.First().esp_ser.T_SERVICIO_SALUD.nombre_servicio,
                    cantidad = x.group.Count(),
                    color = (x.group.Count() <= Int32.Parse(config_retirar.valor_maximo) && x.group.Count() >= Int32.Parse(config_retirar.valor_minimo)) ? "orange" : ""
        })
                .ToList();

            //To json asi_plan //TEMP
            object obj_asi_plan = asi_plan
                .Join(db.T_PERSONA, rs => rs.codPersona, pe => pe.codPersona, (rs, pe) => new { rs, pe })
                .Join(db.T_PERSONA_PLANSALUD, rs_pe => rs_pe.pe.codPersona, pp => pp.codPersona, (rs_pe, pp) => new { rs_pe.rs, rs_pe.pe, pp })
                .DefaultIfEmpty()
                .Join(db.T_ESPECIALIDAD_SERVICIO, ret_pp => ret_pp.rs.id_especialidad_servicio, esp_ser => esp_ser.id_especialidad_servicio, (ret_pp, esp_ser) => new { ret_pp.rs, ret_pp.pe, ret_pp.pp, esp_ser })
                .Where(x => x.pp == null || x.rs.fecha < x.pp.fecha_inicio || x.rs.fecha > x.pp.fecha_fin)
                .GroupBy(gb => new { gb.esp_ser.id_especialidad_servicio }, (key, group) => new { id_esp_ser = key.id_especialidad_servicio, group = group.ToList() })
                .Select(x => new
                {
                    id_esp_ser = x.id_esp_ser,
                    id_especialidad = x.group.First().esp_ser.id_especialidad,
                    id_servicio = x.group.First().esp_ser.T_ESPECIALIDAD_MEDICA.nomEspecialidad,
                    nombre_plan_salud = x.group.First().pp.T_PLAN_DE_SALUD.nombre_plan,
                    nombre_especialidad = x.group.First().esp_ser.T_ESPECIALIDAD_MEDICA.nomEspecialidad,
                    nombre_servicio = x.group.First().esp_ser.T_SERVICIO_SALUD.nombre_servicio,
                    cantidad = x.group.Count(),
                    color = (x.group.Count() <= Int32.Parse(config_asignar.valor_maximo) && x.group.Count() >= Int32.Parse(config_asignar.valor_minimo)) ? "orange" : ""
                })
                .ToList();

            GenerateServiceModel GSM = new GenerateServiceModel()
            {
                LIST_RETIRO_SERVICIO = ret_serv,
                LIST_ASIGNA_PLAN = asi_plan,
                LIST_PLAN_DE_SALUD = pla_salu,
                T_CONFIGURACION_ASIGNAR = config_asignar,
                T_CONFIGURACION_RETIRAR = config_retirar,
                JSON_LIST_ASIGNA_PLAN = JArray.FromObject(obj_asi_plan, Util.jsonNoLoop()),
                JSON_LIST_RETIRO_SERVICIO = JArray.FromObject(obj_ret_serv, Util.jsonNoLoop())
            };

            //-- END: Full logic

            return View("GenerateServices", GSM);
        }

        [HttpPost]
        public ActionResult GenerarAction()
        {
            /*
            TempData["request"] = Request;
            string accion = Request["accion"] ?? "";

            if(accion == "retirar")
            {
                List<string> keys_retirar = Request.Form.AllKeys.Select(x => x).Where(x => x.StartsWith("checkbox_retirar_")).ToList();
                foreach (string item in keys_retirar)
                {
                    string[] splits = item.Split('_');
                    int key_plan = Int32.Parse(splits[2]);
                    int key_serv = Int32.Parse(splits[3]);

                    T_PLAN_SERVICIO ps_m = null;
                    try
                    {
                        ps_m = db.T_PLAN_SERVICIO.Where(x => x.id_plan_salud == key_plan && x.id_servicio == key_serv).First();
                    }
                    catch (Exception e)
                    {

                    }
                    

                    if (ps_m != null)
                    {
                        ps_m.estado = "0";
                        db.Entry(ps_m).State = EntityState.Modified;

                        try
                        {
                            db.SaveChanges();
                            ViewBag.Message = "El retiro de servicio se a realizado corectamente";
                        }
                        catch (Exception e)
                        {
                            ViewBag.Message = e.Message;
                        }
                    } else
                    {
                        ViewBag.Message = "No existe esta asignacion actualmente";
                    }
                    
                    
                }
                
            }
            else if ( accion == "asignar")
            {
                List<string> keys_retirar = Request.Form.AllKeys.Select(x => x).Where(x => x.StartsWith("checkbox_asignar_")).ToList();
                foreach (string item in keys_retirar)
                {
                    string[] splits = item.Split('_');
                    int key_serv = Int32.Parse(splits[2]);
                    int key_plan = Int32.Parse(Request["select_plan_salud_" + key_serv]);

                    T_PLAN_SERVICIO ps = new T_PLAN_SERVICIO()
                    {
                        estado = "1",
                        fecha_registro = DateTime.Today,
                        id_plan_salud = key_plan,
                        id_servicio = key_serv
                    };

                    db.T_PLAN_SERVICIO.Add(ps);
                    db.SaveChanges();

                }
                ViewBag.Message = "La asignación se a realizado correctamente";
            }
            else
            {

            }
            */

            TempData["Message"] = ViewBag.Message;

            return RedirectToAction("Generar", "GenerarServiciosPlan");
        }

    }
}
