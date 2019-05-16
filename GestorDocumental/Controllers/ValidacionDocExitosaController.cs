using GestorDocumental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class ValidacionDocExitosaController : Controller
    {
        //
        // GET: /ValidacionDocExitosa/
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();

        public ActionResult ValidacionDocExitosa()
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
            var result = dbo.spValidaAccesoModulo(idRol, "/ValidacionDocExitosa/ValidacionDocExitosa").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            ViewData["_negId"] = "0";
            Session["_Negocio"] = "0";
            Session["_NroBizagi"] = "";
            Session["_Negocio"] = "";
            return View();
        }

        [HttpPost]
        public JsonResult BuscarNegocio(string negocio)
        {
            try
            {
                Session["_Negocio"] = negocio;
                ViewData["_negId"] = negocio;
                var resultado = dbo.sp_valDocEx_BuscarNegocio(negocio).ToList().SingleOrDefault();

                //si el Resultado es 0 es por que el negocio no cumple alguna de las condiciones
                //entre estas (No existe el negocio,No existen Causales Automáticas para el negocio,No existe la etapa de validación documental)
                if (resultado.Resultado == 0)
                {
                    ViewData["_CodigoError"] = 0;
                    ViewData["_descripcion"] = resultado.Descripcion;
                }
                else if (resultado.Resultado == 1)
                {
                    ViewData["_CodigoError"] = 1;
                    ViewData["_descripcion"] = resultado.Descripcion;
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en ValidacionDactiloscopiaExitosa metodo BuscarNegocio " + exception.Message);
                //return RedirectToAction("Index", new { mensaje = 4 });
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            string[] ArrView = { ViewData["_CodigoError"].ToString(), ViewData["_descripcion"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        [GridAction]
        public ActionResult _ConsultarNegocio()
        {
            
            var grilla = cargarNegocios();

            return View(new GridModel<sp_valDocEx_ObtenerInfoCausalesNegadas_Result>(grilla));
        }

        private IEnumerable<sp_valDocEx_ObtenerInfoCausalesNegadas_Result> cargarNegocios()
        {
            try
            {
                string _negId = Session["_Negocio"].ToString();

                List<sp_valDocEx_ObtenerInfoCausalesNegadas_Result> grilla = new List<sp_valDocEx_ObtenerInfoCausalesNegadas_Result>();

                if (!_negId.Equals("0") && !_negId.Equals(""))
                {
                    dbo.spValDoc_DocExistentes(Convert.ToInt32(_negId));
                    grilla = dbo.sp_valDocEx_ObtenerInfoCausalesNegadas(Convert.ToInt32(_negId)).ToList();
                    
                }
                return grilla;
                
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en suspender metodo cargarNegocios " + exception.Message);
                ViewData["MensajeError_"] = exception.Message;
                return null;
            }
        }


        [HttpPost]
        public void PasarNegocioExitoso(string negId)
        {
            
            try
            {
                decimal idusuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;

                dbo.sp_valDocEx_CerrarEtapaValDoc(Convert.ToDecimal(negId), idusuario);
                
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en Validación Documental exitoso , metodo: finValidacion " + es.Message);
                throw;
            }
        }
    }
}
