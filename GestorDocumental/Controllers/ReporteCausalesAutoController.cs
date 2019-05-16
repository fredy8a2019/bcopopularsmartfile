using GestorDocumental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class ReporteCausalesAutoController : Controller
    {
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();
        Captura n = new Captura();
        int estado;
        //
        // GET: /ReporteCausalesAuto/

        
        public ActionResult ReporteCausalesAuto()
        {
            try
            {
                //JFP
                ModelState.Clear();
                //<< JFPancho;6-abril-2017; 
                //---valida que el usuario no este activo en mas de una máquina
                LogUsuarios x = new LogUsuarios();
                x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                //---valida si el usuario logueado tiene accceso al modulo
                int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
                var result = dbo.spValidaAccesoModulo(idRol, "/Radicacion/Index").FirstOrDefault();

                if (result == 0)
                {
                    Response.Redirect("../Home/Index");
                }
                //JFPancho >>
                if (estado == 0)
                {
                    ViewData["ROtxtPagina"] = "";
                    ViewData["ROtxtDocumento"] = "";
                    ViewData["_btnGuardarVisible"] = "hidden";
                    ViewData["_btnFinalizarVisible"] = "hidden";
                    ViewData["_disableCampDoc"] = "disabled='disabled'";
                    ViewData["_ValorPagina_"] = "1";

                    Session["NEGOCIO"] = 0;
                    Session["_Negocio"] = 0;
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en ReportCausalesAuto.cs metodo Index" + exception.Message);
                throw;
            }
            return View();
        }

        [HttpPost]
        public JsonResult BuscarNegocio(decimal _NegId)
        {
            try
            {
                estado = 1;
                n.NegId = _NegId;
                int num;

                //es uno cuando se debe mostrar el boton de Ingresar
                ViewData["_btns"] = 1;

                Session["NEGOCIO"] = n;
                Session["_Negocio"] = (int)_NegId;
                ViewData["_ExisteNegId"] = 0;
                decimal _Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                var resultado = dbo.sp_RCA_BuscarNegocio(Convert.ToInt32(_NegId)).ToList().SingleOrDefault();

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
                LogRepository.registro("Error en ReIndexacionImg.aspx metodo BuscarNegocio " + exception.Message);
                throw;
            }
            string[] ArrView = { ViewData["_CodigoError"].ToString(), ViewData["_descripcion"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        [GridAction]
        public ActionResult _consultarDatosReporte()
        {
            var grilla = traerDatosReporte();
            return View(new GridModel<_grillaReporteCausalesAuto>(grilla));
        }

        private IEnumerable<_grillaReporteCausalesAuto> traerDatosReporte()
        {
            try
            {
                //Captura n = (Captura)this.Session["NEGOCIO"];
                int negocio = Convert.ToInt32(Session["_Negocio"].ToString());
                List<sp_RCA_ObtenerCausalesAuto_Result> listaReporte = dbo.sp_RCA_ObtenerCausalesAuto(negocio).ToList();

                var grilla = (from a in listaReporte
                              select new _grillaReporteCausalesAuto()
                              {
                                  NombreCampo = a.NombreCampo,
                                  ValorCampo = a.ValorCampo,
                                  NombreCausal = a.NombreCausal,
                                  ParametroEvaluacion = a.ParametroEvaluacion,
                                  Resultado = a.Resultado,
                                  Documento = a.Documento
                              });
                return grilla;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo cargarPaginasIndexadas " + exception.Message);
                ViewData["MensajeError_"] = exception.Message;
                return null;
            }
        }

    }
    public class _grillaReporteCausalesAuto
    {
        public string NombreCampo { get; set; }
        public string ValorCampo { get; set; }
        public string NombreCausal { get; set; }
        public string ParametroEvaluacion { get; set; }
        public string Resultado { get; set; }
        public string Documento { get; set; }
    }
}
