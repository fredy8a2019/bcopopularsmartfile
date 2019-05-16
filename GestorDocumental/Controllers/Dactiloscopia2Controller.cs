using GestorDocumental.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class Dactiloscopia2Controller : Controller
    {
        //
        // GET: /Dactiloscopia2/
        GestorDocumentalEnt db = new GestorDocumentalEnt();
        private GestorDocumental.Controllers.CapturaController bCap;
        private CrearFormulariosCaptura formulario = new CrearFormulariosCaptura();

        /// <summary>
        /// Metodo Que se encarga de cargar la vista principal del modulo Dactiloscopia 2
        /// </summary>
        /// <returns>la vista principal</returns>
        public ActionResult Dactiloscopia2()
        {
            //Carlos : metodos para limpiar la cache
            try
            {
                borraCache();

                if (validarSesion() == false)
                {
                    return null;
                }
                Session["Pagina20"] = "0"; Session["Pagina260"] = "0"; Session["Pagina280"] = "0";
                decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
                decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, 260).ToList().SingleOrDefault().Value;
                Session["_txtObservaciones"] = "";
                ViewData["ProductoDacti"] = "";

                Session["_negId_"] = "";
                if (negId != 0)
                {
                    crearPdfDocsDacti((int)negId);
                    int Perfil = 0;
                    Perfil = obtenerIdGrupo(Convert.ToInt32(negId));
                    ViewData["ProductoDacti"] = Perfil;
                    ViewData["URLDacti"] = "";
                    if (Perfil > 1 && Perfil < 12) //dependiendo del producto se va a cargar una vista de visores diferente
                    {
                        ViewData["URLDacti"] = "onclick=\"popitup('Visores_TDC')\"";
                    }
                    else if (Perfil > 12 && Perfil < 23)
                    {
                        ViewData["URLDacti"] = "onclick=\"popitup('Visores_LD')\"";
                    }
                    else
                    {
                        ViewData["URLDacti"] = "onclick=\"popitup('VisoresMUL')\"";
                    }

                    Session["_negId_"] = negId;

                    ViewData["_mensajeInformacion"] = "";
                    ViewData["Block"] = "";
                    //ViewData["Causales"] = generaCampos(20);

                    CrearTabla(Convert.ToInt32(negId)); //crea la tabla de informacion del cliente
                    CrearDivCausales(Convert.ToInt32(negId));// Crear causales para el caso
                }
                else
                {

                    ViewData["_mensajeInformacion"] = "No existen negocios disponibles para esta etapa";
                    ViewData["Block"] = "disabled = 'true'";
                }

                //_consultaHistorico();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Dactiloscopia2 metodo Dactiloscopia2 " + exception.Message);
                Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");
            }
            return View();

        }

        /// <summary>
        /// Metodo que borra la cache
        /// </summary>
        public void borraCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
        }

        /// <summary>
        /// Este metodo vlida que el usuario este logueado y este tenga acceso al modulo.
        /// </summary>
        /// <returns>la vista principal</returns>
        public Boolean validarSesion()
        {
            if (Session["CLIENTE"] == null)
            {
                Response.Redirect("../Seguridad/Login");
                return false;
            }

            //<<JFPancho;6-abril-2017;  
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = db.spValidaAccesoModulo(idRol, "/Dactiloscopia2/Dactiloscopia2").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
                return false;
            }
            //JFPancho >>
            return true;
        }


        /// <summary>
        /// Crea la tabla Que muestra la informacion del cliente y de el caso
        /// </summary>
        /// <returns>la vista principal</returns>
        public void CrearTabla(int negid)
        {
            try
            {
                string TablaInfoCliente = "";

                List<sp_ValDac2_DatosTablaInfo_Result> datosTabla = db.sp_ValDac2_DatosTablaInfo(negid).ToList();

                foreach (sp_ValDac2_DatosTablaInfo_Result fila in datosTabla)
                {
                    string txt1 = "<tr>" +
                                            "<td>" + fila.campo + "</td>" +
                                            "<td>" + fila.valor + "</td></tr>";
                    TablaInfoCliente += txt1;
                }

                ViewData["CamposTablaGenerados"] = "<table class=\"TablaFormulario table table-bordered\">" + TablaInfoCliente + "</table>";
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Dactiloscopia2 metodo CrearTabla " + exception.Message + "Stactraca: " + exception.StackTrace);
                Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");
            }
        }

        public void CrearDivCausales(int negid)
        {
            try
            {
                string TablaInfoCliente = "";

                List<sp_ValDac2_DocumentosNegocio_Result> datosTabla = db.sp_ValDac2_DocumentosNegocio(negid).ToList();

                foreach (sp_ValDac2_DocumentosNegocio_Result fila in datosTabla)
                {

                    string Causales = generaCampos(fila.DocId);
                    string txt1 = "<fieldset class=\"scheduler-border\">" +
                                        "<h4 style=\"margin-top: 10px;font-weight: bolder;\">" + fila.DocDescripcion + "</h4>" +
                                                "<div id=\"Causales" + fila.DocId + "\">" + Causales +
                                                "</div>" +
                                                "<input  type=\"button\"  class=\"btn btn-Comando\" style=\"margin: 0px 5px 20px 5px;\" value=\".:Actualizar Causales:.\" onclick=\"CausalesImagen('" + fila.DocId + "')\" />" +
                                   "</fieldset>";

                    TablaInfoCliente += txt1;

                }

                ViewData["CausalesGeneradas"] = TablaInfoCliente;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Dactiloscopia2 metodo CrearTabla " + exception.Message + "Stactraca: " + exception.StackTrace);
                Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");
            }
        }

        public ActionResult VisoresMUL()
        {
            try
            {
                borraCache();

                if (validarSesion() == false)
                {
                    return null;
                }
                string negId = "";
                negId = Session["_negId_"].ToString();

                int producto = 0;
                producto = obtenerIdGrupo(Convert.ToInt32(negId));
                ViewData["ProductoDacti"] = producto;
                ViewData["negVisorMUL"] = "";

                ViewData["negVisorMUL"] = negId;


            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Dactiloscopia metodo index " + exception.Message);
                Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");
            }
            return View();

        }

        public ActionResult Visores_LD()
        {
            try
            {
                borraCache();

                if (validarSesion() == false)
                {
                    return null;
                }
                string negId = "";
                negId = Session["_negId_"].ToString();

                int producto = 0;
                producto = obtenerIdGrupo(Convert.ToInt32(negId));
                ViewData["ProductoDacti"] = producto;
                ViewData["negVisorLD"] = "";

                ViewData["negVisorLD"] = negId;


            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Dactiloscopia metodo index " + exception.Message);
                Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");
            }
            return View();

        }

        public ActionResult Visores_TDC()
        {
            try
            {
                borraCache();

                if (validarSesion() == false)
                {
                    return null;
                }
                string negId = "";
                negId = Session["_negId_"].ToString();

                int producto = 0;
                producto = obtenerIdGrupo(Convert.ToInt32(negId));
                ViewData["ProductoDacti"] = producto;
                ViewData["negVisorTDC"] = "";

                ViewData["negVisorTDC"] = negId;

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Dactiloscopia metodo index " + exception.Message);
                Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");
            }
            return View();

        }

        public string generaCampos(int _DocId)
        {

            List<spValDoc_DocCampos_Result> lstSpCausales = db.spValDoc_DocCampos(_DocId, 2).ToList();
            List<tvalDoc_Causales> lstCausales = new List<tvalDoc_Causales>();
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, 260).ToList().SingleOrDefault().Value;
            ViewData["_negId"] = negId;
            var tarjeta = db.sp_ValDac_EsTarjeta(negId).ToList();
            List<sp_ValDactiloscopia_Campos_Result> _NumHojas = db.sp_ValDactiloscopia_Campos(Convert.ToInt32(negId)).ToList();
            foreach (spValDoc_DocCampos_Result item in lstSpCausales)
            {
                tvalDoc_Causales cmp = new tvalDoc_Causales();

                cmp.cod_causal = item.cod_causal;
                cmp.nom_causal = item.nom_causal;
                cmp.desc_causal = item.desc_causal;
                cmp.cod_tipo_causal = item.cod_tipo_causal;
                cmp.cod_documento = item.cod_documento;
                cmp.CamAlto = item.CamAlto;
                cmp.CampAncho = item.CampAncho;
                cmp.CampOrden = item.CampOrden;
                cmp.CampObligatorio = item.CampObligatorio;
                cmp.TcId = item.TcId;
                cmp.Bloqueado = item.Bloqueado;
                cmp.Activo = item.Activo;
                cmp.Bloqueado = item.Bloqueado;
                cmp.LongMax = item.LongMax;
                cmp.PosX = item.PosX;
                cmp.PosY = item.PosY;
                cmp.CodFormulario = item.CodFormulario;

                lstCausales.Add(cmp);
            }

            Table tbl = new Table();
            string campos = formulario.GenerarCamposValDoc(tbl, lstCausales, 100, 0, _DocId, Convert.ToInt32(negId), 2);
            campos = campos.Replace('"', '\'');

            return campos;
        }


        public string consultaRutaNegocios()
        {
            var valor = (from a in db.Parametros
                         where a.codigo == "PATH_DESTINO"
                         select a.valor).SingleOrDefault();
            return valor;
        }

        public int obtenerIdGrupo(int _negId)
        {
            int idGrupo = Convert.ToInt32(db.spObtenerSubgrupo(_negId).SingleOrDefault().ToString());
            return idGrupo;
        }


        [HttpPost]
        public JsonResult validarUsuario()
        {
            var s = Session["CLIENTE"];
            if (Session["CLIENTE"] == null)
            {
                //Response.Redirect("../Seguridad/Login");                
                return Json(2, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CrearTablaHistorico()
        {
            try
            {
                _consultaHistorico();

                return Json(1, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //CapturaErrores(ex.Message);
                LogRepository.registro("Error en Consulta Busca " + ex.Message + " stack trace " + ex.StackTrace);
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

        [GridAction]
        public ActionResult _consultaHistorico()
        {
            var grilla = cargarConsultaHistorico();
            return View(new GridModel<sp_ValDac2_HistoricoCedula_Result>(grilla));
        }

        private IEnumerable<sp_ValDac2_HistoricoCedula_Result> cargarConsultaHistorico()
        {
            try
            {
                string negocio = Session["_negId_"].ToString();
                if (negocio == "")
                {
                    negocio = "0";
                }

                var grilla = db.sp_ValDac2_HistoricoCedula(Convert.ToDecimal(negocio)).ToList();

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


        [HttpPost]
        [GridAction]
        public virtual ActionResult ActualizarCausales()
        {
            int _negId = 0;
            if (!Session["_negId_"].Equals(""))
            {
                _negId = Convert.ToInt32(Session["_negId_"]);
            }
            var grillaCausales = _getConsulCausales(_negId);
            //agregar validacion cuando no traiga resultados la consulta para genrerar alerta

            return View(new GridModel<spValDoc_ConsultaCausales_Result>(grillaCausales));
        }

        public IEnumerable<spValDoc_ConsultaCausales_Result> _getConsulCausales(int _negId)
        {
            try
            {

                var grillaCausales = db.spValDoc_ConsultaCausales(_negId, 2).ToList();

                return grillaCausales;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public void CausalesEspecificas(int _campId, int _snCausal, decimal _negId, int doc)
        {
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, 260).ToList().SingleOrDefault().Value;

            decimal _idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            db.spValDact_InsertCausales(_campId, _snCausal, _negId, _idUsuario, doc);

        }

        /// <summary>
        /// este metodo es el encargado de validar que se ingresaran todas las causales al dar en el boton de aprobo 
        /// o finalizo
        /// </summary>
        /// <param name="_negId"></param>
        /// <param name="_snIndx"></param>
        /// <param name="_observaciones"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidarTotalCausales(string _observaciones)
        {
            try
            {
                Session["_txtObservaciones"] = _observaciones;
                int _negId = 0;
                //Session["_txtObservaciones"] = _observaciones;
                _negId = Convert.ToInt32(Session["_negId_"]);

                //var resultado = db.spValDoc_SNExisteCausales(_negId).SingleOrDefault();
                var resultado = db.sp_ValDac2_validarTotalCausales(_negId).SingleOrDefault();

                if (resultado == 1)
                {
                    return Json(Convert.ToInt32(1), JsonRequestBehavior.AllowGet);
                }
                else if (resultado == 2)
                {
                    return Json(Convert.ToInt32(2), JsonRequestBehavior.AllowGet);

                }
                else if (resultado == 3)
                {
                    //db.sp_CausalesAutomaticas(_negId, _idUsuarioProc);
                    return Json(Convert.ToInt32(3), JsonRequestBehavior.AllowGet);
                }
                return Json(Convert.ToInt32(4), JsonRequestBehavior.AllowGet);
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en Dactiloscopia2 metodo AutoCausal " + es.Message);

                throw;
            }

        }

        /// <summary>
        /// funcion encargada de llamar al sp que termina la etapa y guarda el comentario del usuario en captura con el campo -996
        /// </summary>
        public void finValidacion()
        {
            decimal negId = Convert.ToDecimal(Session["_negId_"]);
            decimal _idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            string txtObservaciones = Session["_txtObservaciones"].ToString();

            if (txtObservaciones == null || txtObservaciones == "" || txtObservaciones == "null")
            {
                Convert.ToString(txtObservaciones);
                txtObservaciones = "";
            }

            db.sp_Finalizar_Dactiloscopia_iniciar_WS(negId, txtObservaciones, _idUsuario);

        }

        //crea un PDF por separado para cada  uno de los documentos que huza el modulo de dactiloscopia
        public void crearPdfDocsDacti(int negid)
        {
            try
            {

                string docPDFOrigen = "";
                string docPDFDestino = "";
                PdfImportedPage importedPage = null;
                string path = consultaRutaNegocios();

                docPDFOrigen = path + negid + "\\" + negid + ".pdf";

                docPDFDestino = path + negid + "\\Pagina-";

                List<sp_ValDac2_DocumentosPaginas_Result> docuPaginas = db.sp_ValDac2_DocumentosPaginas(negid).ToList();

                foreach (sp_ValDac2_DocumentosPaginas_Result docPag in docuPaginas)
                {
                    if (docPag.Mascara == 20)
                    {
                        Session["Pagina20"] = docPag.Mascara;
                    }
                    else if (docPag.Mascara == 260)
                    {
                        Session["Pagina260"] = docPag.Mascara;
                    }
                    else
                    {
                        Session["Pagina280"] = docPag.Mascara;
                    }
                    if (!System.IO.File.Exists(docPDFDestino + docPag.Mascara + ".pdf"))
                    {
                        using (FileStream stream1 = new FileStream(docPDFDestino + docPag.Mascara + ".pdf", FileMode.Create))
                        {
                            Document pdfDoc = new Document(PageSize.A4);
                            PdfCopy pdfCreado = new PdfCopy(pdfDoc, stream1);
                            pdfDoc.Open();
                            importedPage = pdfCreado.GetImportedPage(new PdfReader((string)docPDFOrigen), (int)docPag.NumPagina);

                            pdfCreado.AddPage(importedPage);

                            pdfCreado.Close();
                            pdfDoc.Close();
                            stream1.Close();

                        }
                    }
                }

            }
            catch (Exception e)
            {
                LogRepository.registro("Error en CrearArchivos metodo crearPdfPorTypoDoc:  " + e.Message + "----stack trace----" + e.StackTrace);
                throw;
            }
        }

        [HttpPost]
        public JsonResult reiniciarCausales(decimal _negId)
        {
            try
            {
                int idetapa = 260;
                var resultado = db.sp_Borrar_Causales(_negId, idetapa).FirstOrDefault();

                if (resultado.ErrorNumber == 1)
                {
                    return Json(Convert.ToInt32(1), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogRepository.registro("Error en Dactiloscopia negid:" + _negId + " metodo reiniciarCausales sp_Borrar_Causales: " + resultado.ErrorMessage);
                    return Json(Convert.ToInt32(0), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en Dactiloscopia negid:" + _negId + " metodo reiniciarCausales " + es.Message + " _Staktrace" + es.StackTrace);

                throw;
            }
        }
    }
}
