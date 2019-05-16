using GestorDocumental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class SuspenderController : Controller
    {
        //
        // GET: /Suspender/

        GestorDocumentalEnt db = new GestorDocumentalEnt();

        public ActionResult Index()
        {
            //<<JFPancho;6-abril-2017;  
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = db.spValidaAccesoModulo(idRol, "/Suspender/Index").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>

            if (Session["CLIENTE"] == null)
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
            return View();
        }
        [HttpPost]
        public JsonResult Suspender(string Neg, string Etapa)
        {
            try
            {
                sp_SuspenderNegocio_Result resultado = db.sp_SuspenderNegocio(Convert.ToInt32(Neg), Convert.ToInt32(Etapa)).ToList().FirstOrDefault(); ;

                //return RedirectToAction("Index", new { mensaje = 3});
                if (resultado.codigo == 0)
                {
                    ViewData["_CodigoError"] = 0;
                    ViewData["_descripcion"] = resultado.mensaje;
                    
                }
                else if (resultado.codigo == 1)
                {
                    ViewData["_CodigoError"] = 1;
                    ViewData["_descripcion"] = resultado.mensaje;

                }

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en suspender metodo Suspender " + exception.Message);
                //return RedirectToAction("Index", new { mensaje = 4 });
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            string[] ArrView = { ViewData["_CodigoError"].ToString(), ViewData["_descripcion"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Activar(string Neg, string Etapa)
        {
            try
            {
                var resultado = db.sp_Suspender_Activar(Convert.ToInt32(Neg), Convert.ToInt32(Etapa));
                //return RedirectToAction("Index", new { mensaje = 1 });
                return Json(1, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en suspender metodo Activar " + exception.Message);
                //return RedirectToAction("Index", new { mensaje = 2 });
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [GridAction]
        public ActionResult _consultarNegociosSuspendidos()
        {
            var grilla = cargarNegocios();
            return View(new GridModel<sp_Suspender_Suspendidos_Result>(grilla));
        }

        private IEnumerable<sp_Suspender_Suspendidos_Result> cargarNegocios()
        {
            try
            {

                var grilla = db.sp_Suspender_Suspendidos().ToList();
                return grilla;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en suspender metodo cargarNegocios " + exception.Message);
                ViewData["MensajeError_"] = exception.Message;
                return null;
            }
        }
    }
}