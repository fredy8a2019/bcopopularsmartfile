using GestorDocumental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class ReporteDocumentosCargadosController : Controller
    {
        //
        // GET: /ReporteDocumentosCargados/

        GestorDocumentalEnt db = new GestorDocumentalEnt();
        private DocumentosController bdoc = new DocumentosController();

        public ActionResult Index(int? confirmacion)
        {
            //<< JFPancho;6-abril-2017; 
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = db.spValidaAccesoModulo(idRol, "/ReporteDocumentosCargados/Index").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            if (Session["CLIENTE"] != null)
            {
                Session["negId"] = "";

                ViewData["Consultor"] = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                ViewData["Rol"] = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;

                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        [HttpPost]
        public JsonResult CrearReporte(string negocio)
        {
            try
            {

                Session["negId"] = negocio;

                _consultaRadicaciones();

                return Json(1, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //CapturaErrores(ex.Message);
                LogRepository.registro("Error en Consulta Busca " + ex.Message+ " stack trace " + ex.StackTrace );
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

        [GridAction]
        public ActionResult _consultaRadicaciones()
        {
            var grilla = cargarConsultaRadicaciones();
            return View(new GridModel<sp_RepDocCar_ObtenerNegocios_Result>(grilla));
        }

        private IEnumerable<sp_RepDocCar_ObtenerNegocios_Result> cargarConsultaRadicaciones()
        {
            try
            {
                string negocio = Session["negId"].ToString();
                if (negocio == "")
                {
                    negocio = "0";
                }

                var grilla = db.sp_RepDocCar_ObtenerNegocios(Convert.ToDecimal(negocio)).ToList();

                return grilla;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Consulta metodo CargaConsulta " + exception.Message);
                ViewData["MensajeError_"] = exception.Message;
                return null;
            }
        }

        [HttpPost]
        public JsonResult ActualizaNegocio(int idNeg)
        {
            try
            {
                Session["idNeg"] = idNeg;

                return Json(1, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogRepository.registro("Error en Consulta Busca " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

        [GridAction]
        public ActionResult _consultarPaginasIndexadas()
        {
            var grilla = cargarPaginasIndexadas();
            return View(new GridModel<listaDocumentosIndexados>(grilla));
        }

        private IEnumerable<listaDocumentosIndexados> cargarPaginasIndexadas()
        {
            try
            {
                int n = (int)Session["idNeg"];
                List<spObtenerDocumentosPaginas_Result> listaDocumentos = this.bdoc.ObtenerPaginasIndexadas(n);

                var grilla = (from a in listaDocumentos
                              select new listaDocumentosIndexados()
                              {
                                  id = a.ID,
                                  idMasc = a.IDMasc,
                                  documento = a.Documento,
                                  pagina = a.Pagina
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

        /*[HttpPost]
        public JsonResult _GetValoresList()
        {
            try
            {
                return Json(new SelectList(getValoresDepConsultor(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario, ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId), "Consultor_Dep", "Descripcion"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                LogRepository.registro("Error en RadicacionController metodo _GetValoresList " + exception.Message);
                throw;
            }
        }*/

        /*public dynamic getValoresDepConsultor(decimal IdUsuario, int? RolId)
        {
            try
            {
                GestorDocumentalEnt db = new GestorDocumentalEnt();
                return db.tb_Dep_Coordinador_Consultor.Where(x => x.Coordinador == IdUsuario && x.Rol == RolId)
                            .Select(x => new
                            {
                                x.Consultor_Dep,
                                x.Descripcion

                            }).ToList();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo getValoresListCamp " + exception.Message);
                throw exception;
            }
        }*/
    }
}