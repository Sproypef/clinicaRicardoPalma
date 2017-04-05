using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPC.TP2.WEB.Models;
using UPC.TP2.WEB.ViewModels;

namespace UPC.TP2.WEB.Controllers
{
    public class EstrategiaComercialController : Controller
    {
        private BDCLINICAEntities db = new BDCLINICAEntities();

        //
        // GET: /EstrategiaComercial/

        public ActionResult Index()
        {
            int id_plan_salud = 0; //Consider that not exist id_plan_salud equal to 0: Optinality change for ej. to -99

            id_plan_salud = Int32.Parse(Request["estrategia_comercial_action_select_plan"] ?? (TempData["est_com.id_plan_salud"] != null ? TempData["est_com.id_plan_salud"].ToString() : "-1")); //-1: All record by default
            ViewData["id_plan_salud"] = id_plan_salud.ToString();

            //Get messages from others actions : When redirect to here
            ViewBag.Message = TempData["est_com.Message"] != null ? TempData["est_com.Message"].ToString() : "";

            //==== BEGIN: Main process
            var t_estrategia_comercial = db.T_ESTRATEGIA_COMERCIAL.Include(t => t.T_PLAN_DE_SALUD);
            var tv_estrategia_comercial = from est_com in t_estrategia_comercial
                                          select new TV_ESTRATEGIA_COMERCIAL
                                          {
                                            id_estrategia_comercial = est_com.id_estrategia_comercial,
                                            nombre = est_com.nombre,
                                            descripcion = est_com.descripcion,
                                            objetivo = est_com.objetivo,
                                            fecha_registro = est_com.fecha_registro,
                                            fecha_fin = est_com.T_ESTRATEGIA_COMERCIAL_DETALLE.Max(x => x.fecha_fin),
                                            presupuesto = est_com.presupuesto,
                                            real = est_com.T_ESTRATEGIA_COMERCIAL_DETALLE.Sum(x => x.monto),
                                            desviacion = (est_com.presupuesto - est_com.T_ESTRATEGIA_COMERCIAL_DETALLE.Sum(x => x.monto)) / est_com.presupuesto,
                                            id_plan_salud = est_com.id_plan_salud
                                          };

            EstrategiaComercialViewModel evm = new EstrategiaComercialViewModel
            {
                ESTRATEGIAS_COMERCIALES = id_plan_salud > 0 ? tv_estrategia_comercial.Where(x => x.id_plan_salud == id_plan_salud).ToList() : tv_estrategia_comercial.ToList(),
                PLANES_DE_SALUD = db.T_PLAN_DE_SALUD.Where(x => x.estado == "1" && x.fecha_inicio <= DateTime.Now && x.fecha_fin >= DateTime.Now).ToList()
            };
            //==== END: Main process

            return View(evm);
        }

        //
        // GET: /EstrategiaComercial/Details/5

        public ActionResult Details(int id = 0)
        {
            T_ESTRATEGIA_COMERCIAL t_estrategia_comercial = db.T_ESTRATEGIA_COMERCIAL.Find(id);
            if (t_estrategia_comercial == null)
            {
                return HttpNotFound();
            }
            return View(t_estrategia_comercial);
        }

        //
        // GET: /EstrategiaComercial/Create

        public ActionResult Create()
        {
            T_ESTRATEGIA_COMERCIAL est_com = null;

            try
            {
                int est_com_plan = Int32.Parse(Request["estrategia_comercial_action_select_plan"]);
                TempData["est_com.id_plan_salud"] = est_com_plan.ToString();

                string est_com_nombre = Request["estrategia_comercial_nombre"];
                string est_com_objetivo = Request["estrategia_comercial_objetivo"];
                decimal est_com_presupuesto = Decimal.Parse(Request["estrategia_comercial_presupuesto"]);

                est_com = new T_ESTRATEGIA_COMERCIAL()
                {
                    nombre = est_com_nombre,
                    objetivo = est_com_objetivo,
                    fecha_registro = DateTime.Now,
                    presupuesto = est_com_presupuesto,
                    id_plan_salud = est_com_plan
                };
            }
            catch (Exception e)
            {
                TempData["est_com.Message"] = "No se pudo grabar la Estrategia Comercial";
                return RedirectToAction("Index");
            }

            try
            {
                db.T_ESTRATEGIA_COMERCIAL.Add(est_com);
                db.SaveChanges();

                TempData["est_com.Message"] = "La Estrategia Comercial a sido grabada correctamente";

                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                TempData["est_com.Message"] = "No se pudo grabar la Estrategia Comercial";
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /EstrategiaComercial/Edit/5

        public ActionResult Edit(int id = 0)
        {

            T_ESTRATEGIA_COMERCIAL t_estrategia_comercial = db.T_ESTRATEGIA_COMERCIAL.Find(id);
            if (t_estrategia_comercial == null)
            {
                return HttpNotFound();
            }

            EstrategiaComercialViewModel evm = new EstrategiaComercialViewModel()
            {
                ESTRATEGIA_COMERCIAL = t_estrategia_comercial
            };

            return View(evm);
        }

        //
        // POST: /EstrategiaComercial/Edit/5

        public ActionResult EditSave()
        {
            int bit_id = Int32.Parse(Request["bitacora_edit_id"] ?? "-1");

            string bit_estado = Request["bitacora_edit_estado"] ?? String.Empty;
            string bit_seguimiento = Request["bitacora_edit_seguimiento"] ?? String.Empty;

            T_BITACORA_INCIDENCIA bit = db.T_BITACORA_INCIDENCIA.Find(bit_id);
            bit.estado = bit_estado;

            try
            {
                db.Entry(bit).State = EntityState.Modified;
                db.SaveChanges();

                if (bit_estado != String.Empty && bit_estado.ToLower() != "cerrado")
                {
                    T_SEGUIMIENTO bit_seg = new T_SEGUIMIENTO()
                    {
                        id_bitacora = bit.id_bitacora,
                        id_plan_salud = bit.id_plan_salud,
                        seguimiento = bit_seguimiento,
                        fecha_registro = DateTime.Now,
                        usuario = "Dennis Urbano"
                    };

                    db.T_SEGUIMIENTO.Add(bit_seg);
                    db.SaveChanges();
                }

                ViewBag.Message = "La incidencia a sido grabada correctamente";
                TempData["Message"] = ViewBag.Message;
                return RedirectToAction("Edit", new { id = bit_id });

            }
            catch (Exception e)
            {
                ViewBag.Message = "No se pudo grabar la incidencia";
                TempData["Message"] = ViewBag.Message;
                return View("Edit");
            }
        }

        //
        // GET: /EstrategiaComercial/Delete/5

        public ActionResult Delete(int id = 0)
        {
            T_ESTRATEGIA_COMERCIAL t_estrategia_comercial = db.T_ESTRATEGIA_COMERCIAL.Find(id);
            if (t_estrategia_comercial == null)
            {
                return HttpNotFound();
            }
            return View(t_estrategia_comercial);
        }

        //
        // POST: /EstrategiaComercial/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            T_ESTRATEGIA_COMERCIAL t_estrategia_comercial = db.T_ESTRATEGIA_COMERCIAL.Find(id);
            db.T_ESTRATEGIA_COMERCIAL.Remove(t_estrategia_comercial);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}