using GestorDocumental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;

namespace GestorDocumental.Controllers
{
    public class EdicionIncidentesCapturaController : Controller
    {
        //
        // GET: /EdicionIncidentesCaptura/
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();

        public ActionResult Index()
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

            //<<JFPancho;6-abril-2017;  
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = dbo.spValidaAccesoModulo(idRol, "/EdicionIncidentesCaptura/Index").FirstOrDefault();

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
            Session["_NroBizagi"] = "";
            Session["_Negocio"] = "";
            return View();
        }

        public ActionResult EditarCaptura(GridEditMode? mode, GridButtonType? type, GridInsertRowPosition?
           insertRowPosition)
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
            int negId = Convert.ToInt32(Session["_Negocio"].ToString());

            if (negId != null && negId != 0)
            {
                var nombreIndex = dbo.sp_Indexacion_NegNumbizagiAccion((int?)negId).ToList().FirstOrDefault();

                ViewData["TITULO"] = "" + negId + " |" + nombreIndex;
            }

            ViewData["mode"] = mode ?? GridEditMode.InLine;
            ViewData["type"] = type ?? GridButtonType.Text;
            ViewData["insertRowPosition"] = insertRowPosition ?? GridInsertRowPosition.Top;

            return View();
        }

        [HttpPost]
        public JsonResult obtenerNegid(int negId)
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
            Session["_Negocio"] = negId;

            ViewData["_CodigoError"] = 1;
            ViewData["_descripcion"] = "Correcto";

            string[] ArrView = { ViewData["_CodigoError"].ToString(), ViewData["_descripcion"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BuscarNroBizagi(string nroBizagi)
        {
            try
            {
                Session["_NroBizagi"] = nroBizagi;

                var resultado = dbo.sp_InCap_BuscarNroBizagi(nroBizagi).ToList().SingleOrDefault();

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
                LogRepository.registro("Error en suspender metodo Suspender " + exception.Message);
                //return RedirectToAction("Index", new { mensaje = 4 });
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            string[] ArrView = { ViewData["_CodigoError"].ToString(), ViewData["_descripcion"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        [GridAction]
        public ActionResult _ConsultarNegocioNroBizagi()
        {
            var grilla = cargarNegocios();

            return View(new GridModel<sp_InCap_ObtenerInfoNroBizagi_Result>(grilla));
        }

        private IEnumerable<sp_InCap_ObtenerInfoNroBizagi_Result> cargarNegocios()
        {
            try
            {
                string nroBizagi = Session["_NroBizagi"].ToString();
                var grilla = dbo.sp_InCap_ObtenerInfoNroBizagi(nroBizagi).ToList();
                if (grilla != null && grilla.Count > 0)
                {
                    Session["ErrorCaptura"] = grilla[0].error.ToString();
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

        [GridAction]
        public ActionResult _ConsultarCamposNegocio()
        {
            var grilla = cargarCampos();
            return View(new GridModel<sp_InCap_ObtenerCampos_Result>(grilla));
        }

        private IEnumerable<sp_InCap_ObtenerCampos_Result> cargarCampos()
        {
            try
            {
                List<sp_InCap_ObtenerCampos_Result> listacampos = new List<sp_InCap_ObtenerCampos_Result>();
                string negId = Session["_Negocio"].ToString();
                listacampos = dbo.sp_InCap_ObtenerCampos(Convert.ToInt32(negId)).ToList();

                return listacampos;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en suspender metodo cargarNegocios " + exception.Message);
                ViewData["MensajeError_"] = exception.Message;
                return null;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _GuardarEdicionCaptura([Bind(Prefix = "inserted")]IEnumerable<sp_InCap_ObtenerCampos_Result> insertedContacts,
            [Bind(Prefix = "updated")]IEnumerable<sp_InCap_ObtenerCampos_Result> updatedContacts,
            [Bind(Prefix = "deleted")]IEnumerable<sp_InCap_ObtenerCampos_Result> deletedContacts, int id, string valor)
        {
            string negId = Session["_Negocio"].ToString();
            string idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario.ToString();
            List<sp_InCap_ObtenerCampos_Result> listacampos = new List<sp_InCap_ObtenerCampos_Result>();

            listacampos = dbo.sp_InCap_ObtenerCampos(Convert.ToInt32(negId)).ToList();

            sp_InCap_ObtenerCampos_Result fila = listacampos.Where(c => c.numeroFila == id).First();

            dbo.sp_InCap_ActualizarCampos(Convert.ToDecimal(negId), fila.campid, fila.numCaptura, fila.indice, fila.docid, Convert.ToDecimal(idUsuario), valor);

            listacampos[id - 1].valor = valor;

            return View(new GridModel<sp_InCap_ObtenerCampos_Result>(listacampos));
        }

        [HttpPost]
        public JsonResult GuardarEnviar()
        {
            try
            {
                string negId = Session["_Negocio"].ToString();

                dbo.sp_InCap_ReiniciarEnvioCaptura(Convert.ToDecimal(negId));

                //si el Resultado es 0 es por que el negocio no cumple alguna de las condiciones
                //entre estas (No existe el negocio,No existen Causales Automáticas para el negocio,No existe la etapa de validación documental)

                ViewData["_CodigoError"] = 1;
                ViewData["_descripcion"] = "exitoso";

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
        public ActionResult obtieneCodPadre(string _codId, int _campId)
        {
            try
            {
                GestorDocumentalEnt dbo = new GestorDocumentalEnt();

                var codPadre = (from cc in dbo.CodigosCampo
                                where cc.CodId == _codId && cc.CampId == _campId
                                select cc.CodCampId).FirstOrDefault();

                return Json(Convert.ToString(codPadre), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion metodo cargaListaDepend, " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }
    }
}