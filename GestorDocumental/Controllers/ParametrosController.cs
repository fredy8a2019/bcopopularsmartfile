using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{ 
    public class ParametrosController : Controller
    {
        private GestorDocumentalEnt db;

        public ParametrosController(GestorDocumentalEnt _conexion)
        {
            db = _conexion;
        }

        public ViewResult Index()
        {
            return View(db.Parametros.ToList());
        }

        public ViewResult Details(short id)
        {
            Parametros parametros = null;
            try
            {
                parametros=db.Parametros.Single(p => p.id == id);
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en ParametrosController metodo Details " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
            
            return View(parametros);
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(Parametros parametros)
        {
            if (ModelState.IsValid)
            {
                db.Parametros.AddObject(parametros);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(parametros);
        }
 
        public ActionResult Edit(short id)
        {
            Parametros parametros = db.Parametros.Single(p => p.id == id);
            return View(parametros);
        }

        [HttpPost]
        public ActionResult Edit(Parametros parametros)
        {
            if (ModelState.IsValid)
            {
                db.Parametros.Attach(parametros);
                db.ObjectStateManager.ChangeObjectState(parametros, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parametros);
        }

        public ActionResult Delete(short id)
        {
            Parametros parametros = db.Parametros.Single(p => p.id == id);
            return View(parametros);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {            
            Parametros parametros = db.Parametros.Single(p => p.id == id);
            db.Parametros.DeleteObject(parametros);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public string get(String codigo)
        {
            return db.Parametros.Single(p => p.codigo == codigo).valor;
        }
    }
}