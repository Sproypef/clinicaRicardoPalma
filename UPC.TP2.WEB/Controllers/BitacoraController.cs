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
    public class BitacoraController : Controller
    {
        private BDCLINICAEntities db = new BDCLINICAEntities();

        //
        // GET: /Bitacora/

        public ActionResult Index()
        {
            var t_bitacora_incidencia = db.T_BITACORA_INCIDENCIA.Include(t => t.T_EMPLEADO).Include(t => t.T_PLAN_DE_SALUD);
            BitacoraViewModel bvm = new BitacoraViewModel {
                BITACORAS = t_bitacora_incidencia.ToList(),
                PLANES_DE_SALUD = db.T_PLAN_DE_SALUD.ToList()
            };
            return View(bvm);
        }

        public ActionResult ChangePlanSalud()
        {
            int id_plan_salud = 0;
            try {

                id_plan_salud = Int32.Parse(Request["bitacora_action_select_plan"]);
                ViewData["id_plan_salud"] = id_plan_salud.ToString();

            } catch (Exception e) {

                ViewBag.Message = "Su parámetros son incorrectos";
                return View("Index");
            }


            var t_bitacora_incidencia = db.T_BITACORA_INCIDENCIA.Include(t => t.T_EMPLEADO).Include(t => t.T_PLAN_DE_SALUD);               
            BitacoraViewModel bvm = new BitacoraViewModel
            {
                BITACORAS = (id_plan_salud == -1) ? t_bitacora_incidencia.ToList() : t_bitacora_incidencia.Where(x => x.id_plan_salud == id_plan_salud).ToList(),
                PLANES_DE_SALUD = db.T_PLAN_DE_SALUD.ToList(),
               
            };
            return View("Index", bvm);
        }

        //
        // GET: /Bitacora/Details/5

        public ActionResult Details(int id = 0)
        {
            T_BITACORA_INCIDENCIA t_bitacora_incidencia = db.T_BITACORA_INCIDENCIA.Find(id);
            if (t_bitacora_incidencia == null)
            {
                return HttpNotFound();
            }
            return View(t_bitacora_incidencia);
        }

        //
        // GET: /Bitacora/Create

        public ActionResult Create()
        {
            ViewBag.idempleado = new SelectList(db.T_EMPLEADO, "idEmpleado", "nomEmpleado");
            ViewBag.id_plan_salud = new SelectList(db.T_PLAN_DE_SALUD, "id_plan_salud", "nombre_plan");
            return View();
        }

        //
        // POST: /Bitacora/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_BITACORA_INCIDENCIA t_bitacora_incidencia)
        {
            if (ModelState.IsValid)
            {
                db.T_BITACORA_INCIDENCIA.Add(t_bitacora_incidencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idempleado = new SelectList(db.T_EMPLEADO, "idEmpleado", "nomEmpleado", t_bitacora_incidencia.idempleado);
            ViewBag.id_plan_salud = new SelectList(db.T_PLAN_DE_SALUD, "id_plan_salud", "nombre_plan", t_bitacora_incidencia.id_plan_salud);
            return View(t_bitacora_incidencia);
        }

        //
        // GET: /Bitacora/Edit/5

        public ActionResult Edit(int id = 0)
        {
            T_BITACORA_INCIDENCIA t_bitacora_incidencia = db.T_BITACORA_INCIDENCIA.Find(id);
            if (t_bitacora_incidencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.idempleado = new SelectList(db.T_EMPLEADO, "idEmpleado", "nomEmpleado", t_bitacora_incidencia.idempleado);
            ViewBag.id_plan_salud = new SelectList(db.T_PLAN_DE_SALUD, "id_plan_salud", "nombre_plan", t_bitacora_incidencia.id_plan_salud);
            return View(t_bitacora_incidencia);
        }

        //
        // POST: /Bitacora/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_BITACORA_INCIDENCIA t_bitacora_incidencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_bitacora_incidencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idempleado = new SelectList(db.T_EMPLEADO, "idEmpleado", "nomEmpleado", t_bitacora_incidencia.idempleado);
            ViewBag.id_plan_salud = new SelectList(db.T_PLAN_DE_SALUD, "id_plan_salud", "nombre_plan", t_bitacora_incidencia.id_plan_salud);
            return View(t_bitacora_incidencia);
        }

        //
        // GET: /Bitacora/Delete/5

        public ActionResult Delete(int id = 0)
        {
            T_BITACORA_INCIDENCIA t_bitacora_incidencia = db.T_BITACORA_INCIDENCIA.Find(id);
            if (t_bitacora_incidencia == null)
            {
                return HttpNotFound();
            }
            return View(t_bitacora_incidencia);
        }

        //
        // POST: /Bitacora/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            T_BITACORA_INCIDENCIA t_bitacora_incidencia = db.T_BITACORA_INCIDENCIA.Find(id);
            db.T_BITACORA_INCIDENCIA.Remove(t_bitacora_incidencia);
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