using GestorDocumental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class DevolucionEtapaController : Controller
    {
        //
        // GET: /DevolucionEtapa/
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();

        public ActionResult DevolucionEtapa()
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now);
                Response.Cache.SetNoServerCaching();
                Response.Cache.SetNoStore();
                if (Session["CLIENTE"] == null)
                {
                    Response.Redirect("../Seguridad/Login");
                    return null;
                }
                //<<JFPancho;6-abril-2017;  
                //---valida que el usuario no este activo en mas de una máquina
                LogUsuarios x = new LogUsuarios();
                x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                //---valida si el usuario logueado tiene accceso al modulo
                int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
                var result = dbo.spValidaAccesoModulo(idRol, "/DevolucionEtapa/DevolucionEtapa").FirstOrDefault();

                if (result == 0)
                {
                    Response.Redirect("../Home/Index");
                }
                //JFPancho >>

                Session["_Negocio"] = "";
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en suspender metodo Suspender " + exception.Message);
                //return RedirectToAction("Index", new { mensaje = 4 });
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return View();
        }


        public JsonResult DevolverCaso(string negocio, string etapa)
        {
            try
            {
                Session["_Negocio"] = negocio;

                var resultado = dbo.sp_DevEta_DevolucionEtapa(Convert.ToDecimal(negocio), Convert.ToInt32(etapa)).ToList().SingleOrDefault();
                
                if (resultado.codigo == 0)
                {
                    ViewData["_CodigoError"] = 0;
                    ViewData["_descripcion"] = resultado.mensaje;
                    ViewData["_usuario"] = resultado.usuario;
                }
                else if (resultado.codigo == 1)
                {
                    ViewData["_CodigoError"] = 1;
                    ViewData["_descripcion"] = resultado.mensaje;
                    ViewData["_usuario"] = resultado.usuario;
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en suspender metodo Suspender " + exception.Message);
                //return RedirectToAction("Index", new { mensaje = 4 });
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            string[] ArrView = { ViewData["_CodigoError"].ToString(), ViewData["_descripcion"].ToString(), ViewData["_usuario"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

    }
}
