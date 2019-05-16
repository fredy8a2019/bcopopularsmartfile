using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GestorDocumental.Models;
using GestorDocumental.WebService;
using Telerik.Web.Mvc;

//referencias librerias manipulacion PDF
//using RasterEdge.Imaging.Basic;
//using RasterEdge.XDoc.PDF;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Telerik.Web.Mvc.UI;

namespace GestorDocumental.Controllers
{
    public class IndexacionImgController : Controller
    {
        
        int estado;

        public ActionResult Index(GridEditMode? mode, GridButtonType? type, GridInsertRowPosition?
           insertRowPosition)
        {
            ViewBag.pageLoad = "<script type=\"text/javascript\">pageLoad();</script>";
            //Carlos : metodos para limpiar la cache
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
            if (Session["CLIENTE"] == null)
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
            Session["idETAPA"] = 20;
            try
            {
                //JFP
                ModelState.Clear();
                GestorDocumentalEnt data = new GestorDocumentalEnt();
                AsignacionesController bAsig = new AsignacionesController();
                DocumentosController bdoc = new DocumentosController();
                ViewData["ROtxtPagina"] = "";
                ViewData["ROtxtDocumento"] = "";
                ViewData["_btnFinalizarVisible"] = "hidden";
                ViewData["_disableCampDoc"] = "";
                ViewData["_ValorPagina_"] = "";
                ViewData["tamaño"] = "style='height: 850px'";
                ViewData["_disableDocFaltantes"] = "hidden";

                //<<JFPancho;6-abril-2017;  
                //---valida que el usuario no este activo en mas de una máquina
                LogUsuarios x = new LogUsuarios();
                x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                //---valida si el usuario logueado tiene accceso al modulo
                int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
                var result = data.spValidaAccesoModulo(idRol, "/IndexacionImg/Index").FirstOrDefault();

                if (result == 0)
                {
                    Response.Redirect("../Home/Index");
                }
                //JFPancho >>

                if (Request.HttpMethod != "POST")
                {
                    ViewData["_ValorPagina_"] = "1";
                    P_Etapas etapas = new P_Etapas
                    {
                        IdEtapa = 20
                    };

                    Captura n = new Captura();
                    decimal dec = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                    var negId = bAsig.ObtenerNegociosXEntrada((Usuarios)this.Session["USUARIO"], etapas);
                    if (negId == 0M)
                    {
                        n.NegId = 0M;
                        this.Session["NEGOCIO"] = n;
                        ViewData["ROtxtPagina"] = "readonly='true'";
                        ViewData["ROtxtDocumento"] = "readonly='true'";
                        ViewData["MensajeError_"] = "No existen negocios disponibles para esta etapa";
                        Session["TITULO"] = "Indexación";
                    }
                    else
                    {
                        n.NegId = negId;

                        // William; Obtiene el idcase de la etapa anterior y la utiliza en la creacion de la asignacion de tareas
                        // de esta estapa
                        var Case = data.sp_IdCase_Indexacion(n.NegId).ToList();

                        //Ivan Rodriguez: se obtiene el numero bizagi y la accion para mostrar en el titulo de la indexacion
                        //-----------Inicio CambiosIvan Rodriguez
                        if (negId != null && negId != 0)
                        {
                            var nombreIndex = data.sp_Indexacion_NegNumbizagiAccion((int?)negId).ToList().FirstOrDefault();

                            this.Session["NEGOCIO"] = n;
                            Session["TITULO"] = "Indexación del Negocio:" + n.NegId.ToString() + " |" + nombreIndex;
                        }
                        else
                        {
                            Session["TITULO"] = "Indexación del Negocio: 0";
                        }

                        //---------Fin cambio Ivan Rodriguez
                        
                        AsignacionTareas a = new AsignacionTareas
                        {
                            NegId = n.NegId,
                            IdEtapa = etapas.IdEtapa,
                            Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                            HoraInicio = DateTime.Now,
                            IdEstado = 10,
                            idCase = Convert.ToInt32(Case[0]) //William Cicua; se agrega campo
                        };

                        cargarPaginasIndexadas();
                        int num;
                        decimal _usu = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                        var resultado = data.sp_ValIndexacion(Convert.ToDecimal(negId), _usu).SingleOrDefault();
                        if (resultado == 0)
                        {
                            var numN = data.sp_ObtSigPag(Convert.ToDecimal(negId)).SingleOrDefault();
                            num = Convert.ToInt32(numN);
                        }
                        else
                        {
                            num = bdoc.obtenerUltimaPagina(n) + 1;
                        }
                        ViewData["_ValorPagina_"] = num.ToString();
                        int num2 = bdoc.ObtenerNumPaginasNegocio(n);
                        if (num > num2)
                        {
                            ViewData["_btnFinalizarVisible"] = "visible";
                            ViewData["_disableCampDoc"] = "disabled='disabled'";
                            ViewData["_btnGuardarVisible"] = "hidden";
                        }
                        if (!bAsig.ExisteEtapa(a))
                        {
                            bAsig.insertarAsignacion(a);
                        }
                        string usuario = Session["_IDUsuario_"].ToString();
                        int? resutado = data.sp_Cap_ValidacionNegocioAsignado(((Captura)this.Session["NEGOCIO"]).NegId, Convert.ToDecimal(usuario), etapas.IdEtapa).FirstOrDefault();

                        if (resutado == 0)
                        {
                            Response.Redirect("../IndexacionImg/Index");
                        }
                    }

                    Session["_Negocio"] = (int)n.NegId;
                    if (n.NegId == 0)
                    {
                        ViewData["MensajeError_"] = "No existen documentos asociados a el subgrupo.";
                    }
                }
                else
                {
                    ViewData["MensajeError_"] = "";
                }

                if (((Captura)this.Session["NEGOCIO"]).NegId != 0M)
                {
                    Session["_NumPaginas_"] = bdoc.ObtenerNumPaginasNegocio((Captura)this.Session["NEGOCIO"]).ToString();
                }
                var accion = data.sp_Obtener_Accion((int?)((Captura)this.Session["NEGOCIO"]).NegId).ToList().FirstOrDefault();
                int accion2 = Convert.ToInt32(accion);
                if (accion2 == 1 || accion2 == 3)
                {
                    ViewData["_disableDocFaltantes"] = "hidden";
                }
                else
                {
                    ViewData["_disableDocFaltantes"] = "visible";
                    ViewData["tamaño"] = "style='height: 1160px'";
                }
                ViewData["mode"] = mode ?? GridEditMode.InLine;
                ViewData["type"] = type ?? GridButtonType.Text;
                ViewData["insertRowPosition"] = insertRowPosition ?? GridInsertRowPosition.Top;
                return View();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo Page_Load " + exception.Message);
                throw;
            }
        }

        [GridAction]
        public ActionResult _consultarDocumentos()
        {
            var grilla = _grillaWsResultado();
            return View(new GridModel<listaResultadoDocumentos>(grilla));
        }

        private IEnumerable<listaResultadoDocumentos> _grillaWsResultado()
        {
            DocumentosController bdoc = new DocumentosController();
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            int negocio = Convert.ToInt32(Session["_Negocio"].ToString());
            List<Documentos> listaDocumentos = bdoc.obtenerDocumentosIndexacion(negocio);
            ViewData["_disableDocFaltantes"] = "visible";
            ViewData["tamaño"] = "style='height: 850px'";
            int accion2 = 0;
            if (negocio != 0)
            {
                var accion = data.sp_Obtener_Accion((int?)negocio).ToList().FirstOrDefault();
                accion2 = Convert.ToInt32(accion);
                if (accion2 == 1 || accion2 == 3)
                {
                    ViewData["_disableDocFaltantes"] = "hidden";
                }
                else
                {
                    ViewData["_disableDocFaltantes"] = "visible";
                    ViewData["tamaño"] = "style='height: 1150px'";
                }

            }

            if (accion2 == 3 || accion2 == 4)
            {
                var grilla = (from a in listaDocumentos
                              where a.Mascara != 10 & a.Mascara != 11 & a.Mascara != 12 & a.Mascara != 13
                              select new listaResultadoDocumentos()
                              {
                                  docId = a.DocId,
                                  docIdMasc = a.Mascara,
                                  docDescripcion = a.DocDescripcion
                              });
                return grilla;
            }
            else
            {
                var grilla = (from a in listaDocumentos
                              where a.Mascara != 14 & a.Mascara != 15 & a.Mascara != 16 & a.Mascara != 17
                              select new listaResultadoDocumentos()
                              {
                                  docId = a.DocId,
                                  docIdMasc = a.Mascara,
                                  docDescripcion = a.DocDescripcion
                              });
                return grilla;
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
                DocumentosController bdoc = new DocumentosController();
                Captura n = (Captura)this.Session["NEGOCIO"];
                List<spObtenerDocumentosPaginas_Result> listaDocumentos = bdoc.ObtenerPaginasIndexadas(n);

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

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public void BorrarDocumento(int idDocumento, int numPagina)
        {
            DocumentosController bdoc = new DocumentosController();
            //JFP; obtener nro negocio
            int _negocio = Convert.ToInt32(Session["_Negocio"].ToString());

            ModelState.Clear();
            ArchivosAnexos D = new ArchivosAnexos();
            D.DocId = idDocumento;
            D.NumPagina = numPagina;
            bdoc.BorrarDocumento(D, ((Captura)Session["NEGOCIO"]));
        }

        [HttpPost]
        public JsonResult FinIndexacion()
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            AsignacionesController bAsig = new AsignacionesController();
            try
            {
                ViewData["MensajeError_"] = "";

                decimal _usu = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                var _neg = Session["_Negocio"];
                var resultado = data.sp_ValIndexacion(Convert.ToDecimal(_neg), _usu).SingleOrDefault();
                if (resultado == 1)
                {
                    AsignacionTareas a = new AsignacionTareas
                    {
                        IdEstado = 30,
                        HoraTerminacion = new DateTime?(DateTime.Now),
                        NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                        IdEtapa = 20
                    };
                    bAsig.insertarAsignacion(a);


                    //Response.Redirect("../IndexacionConf/Index");
                }
                else if (resultado == 0)
                {
                    Response.Redirect("../IndexacionConf/Index");
                    ViewData["MensajeError_"] = "No se han indexado todos los documentos aún.";
                }
                data.Connection.Close();
            }
            catch (Exception exception)
            {
                data.Connection.Close();
                LogRepository.registro("Error en IndexacionImg.aspx metodo FinIndexacion " + exception.Message);
                ViewData["MensajeError_"] = exception.Message;
            }

            string[] ArrView = { ViewData["MensajeError_"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AgregaIndexacion(string _nroDocumento, string _nroPagina)
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            AsignacionesController bAsig = new AsignacionesController();
            DocumentosController bdoc = new DocumentosController();
            List<Documentos> doc = new List<Documentos>();
            int negocio = Convert.ToInt32(Session["_Negocio"].ToString());
            string txtPagina = _nroPagina;
            ViewData["MensajeError_"] = "";
            Session["_Error"] = 0;
            ViewData["ROtxtPagina"] = "";
            ViewData["ROtxtDocumento"] = "";
            ViewData["_btnFinalizarVisible"] = "hidden";
            ViewData["_disableCampDoc"] = "";
            ViewData["_btnGuardarVisible"] = "";
            ViewData["_ValorPagina_"] = "";
            try
            {
                if (_nroDocumento.Equals("") || _nroDocumento.Equals(null))
                {
                    ViewData["MensajeError_"] = "Debe ingresar un documento para indexar.";
                }
                else
                {
                    //string txtDocumento = collection["txtDocumento"].ToString();
                    string txtDocumento = consultDcoId(_nroDocumento, negocio);


                    //Verifica que el numero de documento que digita este en la lista asignada
                    int NedId = int.Parse(((Captura)this.Session["NEGOCIO"]).NegId.ToString());
                    doc = bdoc.obtenerDocumentosIndexacion(NedId);
                    var DocumentosIdex = doc.Find(x => x.DocId == int.Parse(txtDocumento));

                    //JFP; abril-2016; verificar que no se indexe mas de un documento con la misma tipologia a no ser que se permita
                    int IndexaMultiple = data.sp_ValidaIndexaMultiple(Convert.ToInt32(txtDocumento), Convert.ToInt32(_nroDocumento), Convert.ToDecimal(NedId)).ToList().SingleOrDefault().Value;

                    //int sn_indexa = Convert.ToInt32(IndexaMultiple.ToString());
                    if (IndexaMultiple == 1)
                    {
                        if ((txtDocumento.Trim() != string.Empty) & (txtPagina.Trim() != string.Empty))
                        {
                            if (DocumentosIdex != null)
                            {
                                if (Convert.ToInt32(txtPagina) <= bdoc.ObtenerNumPaginasNegocio((Captura)this.Session["NEGOCIO"]))
                                {
                                    ArchivosAnexos c = new ArchivosAnexos
                                    {
                                        NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                                        AADescripcion = ((Captura)this.Session["NEGOCIO"]).NegId.ToString(),
                                        DocId = Convert.ToInt32(txtDocumento),
                                        NumPagina = Convert.ToInt32(txtPagina),
                                        FechaRegistro = DateTime.Now,
                                        Usuario = new decimal?(((Usuarios)this.Session["USUARIO"]).IdUsuario)
                                    };

                                    if (!bdoc.buscarPaginaDigitada(c))
                                    {
                                        bdoc.insertarDocsIndexados(c);
                                        cargarPaginasIndexadas();
                                        var a = (int.Parse(txtPagina) + 1).ToString();
                                        ViewData["_Pagina"] = (int.Parse(txtPagina) + 1).ToString();
                                        if (bdoc.IndexacionTerminada(c))
                                        {
                                            ViewData["_btnFinalizarVisible"] = "visible";
                                            ViewData["_disableCampDoc"] = "disabled='disabled'";
                                            ViewData["_btnGuardarVisible"] = "hidden";
                                            ViewData["ROtxtDocumento"] = "readonly='true'";
                                            ViewData["ValorDocumento"] = "";
                                            ViewData["MensajeError_"] = "";
                                        }
                                        ViewData["_btnFinalizarVisible"] = "hidden";
                                        ViewData["MensajeError_"] = "";
                                        Session["_NumPaginas_"] = bdoc.ObtenerNumPaginasNegocio((Captura)this.Session["NEGOCIO"]).ToString();
                                    }
                                    else
                                    {
                                        ViewData["MensajeError_"] = "La pagina ingresada ya se encuentra asignada a otro documento";
                                    }
                                }
                                else
                                {
                                    ViewData["MensajeError_"] = "El numero de pagina es mayor al total de paginas del archivo";
                                }
                            }
                            else
                            {
                                ViewData["MensajeError_"] = "Digite un documento valido de la lista";
                            }
                        }
                        else
                        {
                            ViewData["MensajeError_"] = "Digite el documento y la pagina correspondiente.";
                        }
                    }
                    else
                    {
                        ViewData["MensajeError_"] = "Ya existe un documento clasificado con el código ingresado, por favor validar si es un anexo del tipo documental.";
                    }
                }

                // Modificacion 04/05/2016 William Eduardo Cicua
                // este if recargar la pagina si no hay ningun error al indexar 
                // se cambia visibilidad de los botones y se desactivan los campos
                if (ViewData["MensajeError_"].ToString() == "")
                {


                    ViewData["_ValorPagina_"] = "1";
                    P_Etapas etapas = new P_Etapas
                    {
                        IdEtapa = 20
                    };

                    Captura n = new Captura();
                    decimal dec = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                    var negId = bAsig.ObtenerNegociosXEntrada((Usuarios)this.Session["USUARIO"], etapas);
                    if (negId == 0M)
                    {
                        n.NegId = 0M;
                        this.Session["NEGOCIO"] = n;
                        ViewData["ROtxtPagina"] = "readonly='true'";
                        ViewData["ROtxtDocumento"] = "readonly='true'";
                        ViewData["MensajeError_"] = "No existen negocios disponibles para esta etapa";
                        Session["TITULO"] = "Indexación";
                    }
                    else
                    {
                        n.NegId = negId;
                        this.Session["NEGOCIO"] = n;
                        Session["TITULO"] = "Indexación del Negocio:" + n.NegId.ToString();
                        AsignacionTareas a = new AsignacionTareas
                        {
                            NegId = n.NegId,
                            IdEtapa = etapas.IdEtapa,
                            Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                            HoraInicio = DateTime.Now,
                            IdEstado = 10
                        };

                        cargarPaginasIndexadas();
                        int num;
                        decimal _usu = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                        var resultado = data.sp_ValIndexacion(Convert.ToDecimal(negId), _usu).SingleOrDefault();
                        if (resultado == 0)
                        {
                            var numN = data.sp_ObtSigPag(Convert.ToDecimal(negId)).SingleOrDefault();
                            num = Convert.ToInt32(numN);
                        }
                        else
                        {
                            num = bdoc.obtenerUltimaPagina(n) + 1;
                        }
                        ViewData["_ValorPagina_"] = num.ToString();
                        int num2 = bdoc.ObtenerNumPaginasNegocio(n);
                        if (num > num2)
                        {
                            ViewData["_btnFinalizarVisible"] = "visible";
                            ViewData["_disableCampDoc"] = "disabled='disabled'";
                            ViewData["_btnGuardarVisible"] = "hidden";
                        }
                        if (!bAsig.ExisteEtapa(a))
                        {
                            bAsig.insertarAsignacion(a);
                        }
                    }

                    Session["_Negocio"] = (int)n.NegId;
                    if (n.NegId == 0)
                    {
                        ViewData["MensajeError_"] = "No existen documentos asociados a el subgrupo.";
                    }


                    if (((Captura)this.Session["NEGOCIO"]).NegId != 0M)
                    {
                        Session["_NumPaginas_"] = bdoc.ObtenerNumPaginasNegocio((Captura)this.Session["NEGOCIO"]).ToString();
                    }
                    string[] ArrView = { ViewData["MensajeError_"].ToString(), ViewData["_ValorPagina_"].ToString(), ViewData["_btnFinalizarVisible"].ToString(), ViewData["_disableCampDoc"].ToString(), ViewData["_btnGuardarVisible"].ToString() };
                    return Json(ArrView, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo Button1_Click " + exception.Message);
                ViewData["MensajeError_"] = "Error en el metodo Button1_Click en indexacion " + exception.InnerException.ToString();
                base.Response.Redirect("/IndexacionImg/Index");
            }


            string[] ArrView2 = { ViewData["MensajeError_"].ToString() };
            return Json(ArrView2, JsonRequestBehavior.AllowGet);
        }

        /// <summary> 
        /// Función que reordena las paginas del PDF para convertirlo nuevamente a TIFF 
        /// </summary> 
        [HttpPost]
        public void ReordenarDocumento(int NegId)
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            try
            {
                //Rutas Actuales y nuevas de los archivos PDF 
                string PathActualPDF = Path.Combine(Server.MapPath("/Content/ArchivosCliente/" + NegId + "/"), NegId + ".pdf");
                string PathNuevoPDF = Path.Combine(Server.MapPath("/Content/ArchivosCliente_BK/"), NegId + ".pdf");

                //Rutas Actuales y nuevas de los archivos TIFF 
                //string PathActualTIF = Path.Combine(Server.MapPath("/Content/ArchivosCliente/" + NegId + "/"), NegId + ".TIF");
                //string PathNuevoTIF = Path.Combine(Server.MapPath("/Content/ArchivosCliente/" + NegId + "/"), NegId + ".TIF");

                //Verifica si existe el archivo 
                if (System.IO.File.Exists(PathNuevoPDF))
                {
                    //Eliminar el archivo 
                    System.IO.File.Delete(PathNuevoPDF);
                }
                //Copia el archivo original en una carpeta espejo 
                System.IO.File.Copy(PathActualPDF, PathNuevoPDF);
                //Obtener las paginas en el orden de la posicion 
                List<spObtenerOrdenDocumentosPorMascaraNegId_Result> ObtenerDoc = data.spObtenerOrdenDocumentosPorMascaraNegId(NegId).ToList();

                //Leer documento pdf 
                //PDFDocument doc = new PDFDocument(PathNuevoPDF);

                PdfReader read = new PdfReader(PathActualPDF);
                Document doc = new Document(read.GetPageSizeWithRotation(1));
                PdfCopy pdfCopyProvider = new PdfCopy(doc, new System.IO.FileStream(PathNuevoPDF, System.IO.FileMode.Create));

                doc.Open();

                if (ObtenerDoc.Count > 0)
                {
                    //Declaración Array que almacena el nuevo orden de las paginas 

                    List<int> pageOrders = new List<int>();

                    List<nuevoOrden> Antigorder = new List<nuevoOrden>();

                    //modifica:Ivan Rodriguez; mayo/2016
                    //llena lista de enteros ocn el orden nnuevo del pdf
                    foreach (spObtenerOrdenDocumentosPorMascaraNegId_Result dato in ObtenerDoc)
                    {
                        pageOrders.Add(Convert.ToInt32(dato.NumPagina));
                    }

                    //Reordena el pdf con las paginas asignadas en el array pageOrders 
                    //doc.SortPage(pageOrders.ToArray());
                    //Reemplaza el documento actual por el reordenado 

                    for (int i = 0; i < pageOrders.Count; i++)
                    {
                        PdfImportedPage importedPage = pdfCopyProvider.GetImportedPage(read, pageOrders[i]);
                        pdfCopyProvider.AddPage(importedPage);
                    }
                    doc.Close();
                    read.Close();

                    //Elimina archivos originales 
                    if (System.IO.File.Exists(PathActualPDF))
                    {
                        // The files are not actually removed in this demo 
                        System.IO.File.Delete(PathActualPDF);
                        //System.IO.File.Delete(PathActualTIF);
                    }

                    //doc.Save(PathActualPDF);
                    System.IO.File.Copy(PathNuevoPDF, PathActualPDF);
                    data.spOrdenarDocumentosPorMascaraNegId(NegId);
                    
                    //Convertir el pdf a tiff 
                    //int paginasConvertidas = Extends.ConvertirimagenPDFaTIFF(PathActualPDF, PathActualTIF);

                }
                data.Connection.Close();

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo ReordenarDocumento negid:" + NegId + "__" + exception.Message + "__Stak trace:" + exception.StackTrace);
                data.spBloquearEtapa(NegId,20);                                         
                throw;
            }
            
        }

        public string consultDcoId(string _txtDocM, int _negId)
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            int txtDocMasc = Convert.ToInt32(_txtDocM);

            //SACO EL GRUPO AL QUE PERTENECE ESE NEGOCIO
            List<Recepcion> r = (from cd in data.Recepcion
                                 join d in data.CargueLotes on cd.id equals d.idRecepcion
                                 where d.NegId == _negId
                                 select cd).ToList();

            int grupo = 0;
            if (r.Count > 0)
            {
                grupo = (int)r.First().subgrupo.Value;
            }

            //SACO CODIGO DEL DOCUEMNTO RELACIONADO A LA MARCARA
            List<int> DocId = (from t1 in data.ProductosDocum
                               join t2 in data.Documentos on t1.idDocumento equals t2.DocId
                               where (t1.idGrupo == grupo) && (t2.Mascara == txtDocMasc)
                               select t2.DocId).ToList();

            int _DocId = 0;
            if (DocId.Count > 0)
            {
                _DocId = DocId[0];
            }
            return _DocId + "";
        }

        [GridAction]
        public ActionResult _ConsultarDocumentosFaltantes()
        {
            var grilla = cargarDocumentosFaltantes();
            return View(new GridModel<sp_Index_obtenerDocfaltantes_Result>(grilla));
        }

        private IEnumerable<sp_Index_obtenerDocfaltantes_Result> cargarDocumentosFaltantes()
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            try
            {
                List<sp_Index_obtenerDocfaltantes_Result> listacampos = new List<sp_Index_obtenerDocfaltantes_Result>();
                string negId = Session["_Negocio"].ToString();
                listacampos = data.sp_Index_obtenerDocfaltantes(Convert.ToInt32(negId)).ToList();
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
        public ActionResult _GuardarDocumentosFaltantes([Bind(Prefix = "inserted")]IEnumerable<sp_Index_obtenerDocfaltantes_Result> insertedContacts,
            [Bind(Prefix = "updated")]IEnumerable<sp_Index_obtenerDocfaltantes_Result> updatedContacts,
            [Bind(Prefix = "deleted")]IEnumerable<sp_Index_obtenerDocfaltantes_Result> deletedContacts, int id, int pagina)
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            ViewData["ultimaPagina"] = 0;
            string negId = Session["_Negocio"].ToString();
            string idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario.ToString();
            List<sp_Index_obtenerDocfaltantes_Result> listacampos = new List<sp_Index_obtenerDocfaltantes_Result>();

            listacampos = data.sp_Index_obtenerDocfaltantes(Convert.ToInt32(negId)).ToList();

            sp_Index_obtenerDocfaltantes_Result fila = listacampos.Where(c => c.docid == id).First();

            var result = data.sp_Index_ActualizarDocIndexados(Convert.ToDecimal(negId), fila.docid, fila.tipocampo, pagina, Convert.ToDecimal(idUsuario)).FirstOrDefault();
            if (result.resultado == 0)
            {
                for (int i = 0; i < listacampos.Count; i++)
                {
                    if (listacampos[i].docid == id)
                    {
                        if (pagina == 0)
                        {
                            listacampos[i].pagina = null;
                        }
                        else { 
                            listacampos[i].pagina = pagina;
                            var resultado = data.sp_ValIndexacion(Convert.ToDecimal(negId), Convert.ToDecimal(idUsuario)).SingleOrDefault();
                            if (resultado==1)
                            {
                                int numPaginas = 0;
                                var numN = data.sp_ObtSigPag(Convert.ToDecimal(negId)).SingleOrDefault();
                                numPaginas = Convert.ToInt32(numN);

                                ViewData["ultimaPagina"] = 1;
                                
                            }
                        }
                    }
                }
            }           

            return View(new GridModel<sp_Index_obtenerDocfaltantes_Result>(listacampos));
        }

        [HttpPost]
        public JsonResult IndexacionMasiva()
        {
            try
            {
                GestorDocumentalEnt data = new GestorDocumentalEnt();
                ViewData["MensajeError_"] = "";
                ViewData["_btnFinalizarVisible"] = "hidden";
                ViewData["_disableCampDoc"] = "";
                ViewData["_btnGuardarVisible"] = "";

                decimal _usu = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                var _neg = Session["_Negocio"];
                var resultado = data.sp_Index_IndexMasiva(Convert.ToDecimal(_neg), _usu).SingleOrDefault();
                if (resultado.resultado == 1)
                {
                    ViewData["MensajeError_"] = resultado.mensaje;
                }
                else
                {
                    int numPaginas = 0;
                    var numN = data.sp_ObtSigPag(Convert.ToDecimal(_neg)).SingleOrDefault();
                    numPaginas = Convert.ToInt32(numN);

                    ViewData["_ValorPagina_"] = numPaginas.ToString();
                    ViewData["_btnFinalizarVisible"] = "visible";
                    ViewData["_disableCampDoc"] = "disabled='disabled'";
                    ViewData["_btnGuardarVisible"] = "hidden";
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo IndexacionMasiva " + exception.Message);
                ViewData["MensajeError_"] = exception.Message;
            }
            string[] ArrView = { ViewData["MensajeError_"].ToString(), ViewData["_btnFinalizarVisible"].ToString(), ViewData["_disableCampDoc"].ToString(), ViewData["_btnGuardarVisible"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BorrarPaginasIndexadas()
        {
            int? resultadoBorrar = 0;
            string mensajeBorrar = "";
            try
            {
                GestorDocumentalEnt data = new GestorDocumentalEnt();

                var _neg = Session["_Negocio"];

                var resultado = data.sp_Index_BorrarPaginasIndexacion(Convert.ToDecimal(_neg)).SingleOrDefault();
                resultadoBorrar = resultado.resultado;
                if (resultado.resultado == 1)
                {

                    mensajeBorrar = resultado.mensaje;
                }
                else
                {
                    int numPaginas = 0;
                    var numN = data.sp_ObtSigPag(Convert.ToDecimal(_neg)).SingleOrDefault();
                    numPaginas = Convert.ToInt32(numN);

                    ViewData["_ValorPagina_"] = numPaginas.ToString();
                    
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo IndexacionMasiva " + exception.Message);
                mensajeBorrar = exception.Message;
            }
            string[] ArrView = { mensajeBorrar.ToString(), resultadoBorrar.ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult validarDocumentosFaltantes()
        {
            string retorno = "0";
            string documentosFaltantes = "";
            try
            {
                GestorDocumentalEnt data = new GestorDocumentalEnt();
                ViewData["MensajeError_"] = "";

                decimal _usu = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                var _neg = Session["_Negocio"];
                List<sp_Index_obtenerDocfaltantes_Result> listacampos = new List<sp_Index_obtenerDocfaltantes_Result>();
                listacampos = data.sp_Index_obtenerDocfaltantes(Convert.ToInt32(_neg)).ToList();

                for (int i = 0; i < listacampos.Count; i++)
                {
                    if (listacampos[i].pagina == null)
                    {
                        retorno = "1";
                        documentosFaltantes = documentosFaltantes + listacampos[i].documento + ", ";
                    }
                }

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo validarDocumentosFaltantes " + exception.Message);
                ViewData["MensajeError_"] = exception.Message;
            }
            string[] ArrView = { retorno, documentosFaltantes };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }
    }


    public class listaResultadoDocumentos
    {
        public int docId { get; set; }
        public int? docIdMasc { get; set; }
        public string docDescripcion { get; set; }
    }

    public class listaDocumentosIndexados
    {
        public int id { get; set; }
        public int? idMasc { get; set; }
        public string documento { get; set; }
        public int pagina { get; set; }
    }

    public class nuevoOrden
    {
        public int Pagina { get; set; }
        public int Orden { get; set; }
        public int ObtenerOrden()
        {
            return Orden;
        }
    }
}
