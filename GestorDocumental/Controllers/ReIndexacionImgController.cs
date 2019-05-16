using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Collections;

using GestorDocumental.Models;
using GestorDocumental.WebService;
using Telerik.Web.Mvc;
using AjaxControlToolkit;

//referencias librerias manipulacion PDF
using RasterEdge.Imaging.Basic;
using RasterEdge.XDoc.PDF;
using System.IO;

namespace GestorDocumental.Controllers
{
    public class ReIndexacionImgController : Controller
    {
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();
        private AsignacionesController CAsig = new AsignacionesController();
        private List<Documentos> tDocs = new List<Documentos>();
        private DocumentosController CDoc = new DocumentosController();
        int estado;
        Captura n = new Captura();

        public ActionResult ReIndexacionImg()
        {
            //ViewBag.pageLoad = "<script type=\"text/javascript\">pageLoad();</script>";

            try
            {
                //JFP
                ModelState.Clear();
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

                //<<JFPancho;6-abril-2017;  
                //---valida que el usuario no este activo en mas de una máquina
                LogUsuarios x = new LogUsuarios();
                x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                //---valida si el usuario logueado tiene accceso al modulo
                int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
                var result = dbo.spValidaAccesoModulo(idRol, "/ReIndexacionImg/ReIndexacionImg").FirstOrDefault();

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
                LogRepository.registro("Error en IndexacionImg.aspx metodo Page_Load " + exception.Message);
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
                Session["_Negocio"] = (int)n.NegId;
                ViewData["_ExisteNegId"] = 0;
                decimal _Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                var resultado = dbo.sp_ConsultaNegocio(_NegId, _Usuario).ToList().SingleOrDefault();

                int snNeg = Convert.ToInt32(resultado.result);
                if (snNeg == 1)
                {
                    var result = dbo.sp_ValIndexacion(Convert.ToDecimal(_NegId), _Usuario).SingleOrDefault();

                    //si existe captura y no se ha cerrado crea etapa reindexacion y modifica etapa captura 
                    int sn_exist = Convert.ToInt32(dbo.sp_ProcReindexacion(1, _Usuario, _NegId).ToList().SingleOrDefault());

                    if (sn_exist == 0)
                    {
                        if (result == 0)
                        {
                            var numN = dbo.sp_ObtSigPag(Convert.ToDecimal(_NegId)).SingleOrDefault();
                            num = Convert.ToInt32(numN);
                        }
                        else
                        {
                            num = this.CDoc.obtenerUltimaPagina(n) + 1;
                        }
                        ViewData["_ValorPagina_"] = num.ToString();
                        int num2 = this.CDoc.ObtenerNumPaginasNegocio(n);
                        if (num > num2)
                        {
                            //es dos cuando se debe mostrar el boton de Finalizar
                            ViewData["_btns"] = 2;
                        }

                        ViewData["_ExisteNegId"] = 1;
                        ViewData["_archTiff"] = System.Configuration.ConfigurationManager.AppSettings["ClientFiles"] + "/" + ((Captura)this.Session["NEGOCIO"]).NegId.ToString() + "/" + ((Captura)this.Session["NEGOCIO"]).NegId.ToString() + ".pdf";
                        ViewData["_CapturaCerrada"] = 0;

                    }
                    else if (sn_exist == 1)
                    {
                        ViewData["_ExisteNegId"] = 1;
                        ViewData["_btns"] = "";
                        ViewData["_archTiff"] = "";
                        ViewData["_ValorPagina_"] = "1";
                        ViewData["_CapturaCerrada"] = 1;
                    }
                    else if (sn_exist == 2)
                    {
                        ViewData["_ExisteNegId"] = 1;
                        ViewData["_btns"] = "";
                        ViewData["_archTiff"] = "";
                        ViewData["_ValorPagina_"] = "1";
                        ViewData["_CapturaCerrada"] = 2;
                    }

                }
                else if (snNeg == 0)
                {
                    ViewData["_ExisteNegId"] = 0;
                    ViewData["_btns"] = "";
                    ViewData["_archTiff"] = "";
                    ViewData["_ValorPagina_"] = "1";
                    ViewData["_CapturaCerrada"] = 0;
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en ReIndexacionImg.aspx metodo BuscarNegocio " + exception.Message);
                throw;
            }
            string[] ArrView = { ViewData["_ExisteNegId"].ToString(), ViewData["_archTiff"].ToString(), ViewData["_ValorPagina_"].ToString(), ViewData["_btns"].ToString(), ViewData["_CapturaCerrada"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [GridAction]
        public ActionResult _consultarDocumentos(int _negId)
        {

            var grilla = _grillaWsResultado(_negId);
            return View(new GridModel<_grillalstResultDocs>(grilla));

        }

        private IEnumerable<_grillalstResultDocs> _grillaWsResultado(int _negId)
        {
            int negocio = Convert.ToInt32(Session["_Negocio"].ToString());

            List<Documentos> listaDocumentos = CDoc.obtenerDocumentosIndexacion(negocio);

            var grilla = (from a in listaDocumentos
                          select new _grillalstResultDocs()
                          {
                              docId = a.DocId,
                              docIdMasc = a.Mascara,
                              docDescripcion = a.DocDescripcion
                          });
            return grilla;
        }

        [GridAction]
        public ActionResult _consultarPaginasIndexadas()
        {
            var grilla = cargarPaginasIndexadas();
            return View(new GridModel<_grillalstDocsIndexados>(grilla));
        }

        private IEnumerable<_grillalstDocsIndexados> cargarPaginasIndexadas()
        {
            try
            {
                //Captura n = (Captura)this.Session["NEGOCIO"];
                int negocio = Convert.ToInt32(Session["_Negocio"].ToString());
                List<spObtenerDocumentosPaginas_Result> listaDocumentos = this.CDoc.ObtenerPaginasIndexadas(negocio);

                var grilla = (from a in listaDocumentos
                              select new _grillalstDocsIndexados()
                              {
                                  id = a.ID,
                                  idMasc = a.IDMasc,
                                  documento = a.Documento,
                                  pagina = a.Pagina,
                                  negId = a.NegId
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


        public string consultDcoId(string _txtDocM, int _negId)
        {
            int txtDocMasc = Convert.ToInt32(_txtDocM);

            //SACO EL GRUPO AL QUE PERTENECE ESE NEGOCIO
            List<Recepcion> r = (from cd in this.dbo.Recepcion
                                 join d in this.dbo.CargueLotes on cd.id equals d.idRecepcion
                                 where d.NegId == _negId
                                 select cd).ToList();

            int grupo = 0;
            if (r.Count > 0)
            {
                grupo = (int)r.First().subgrupo.Value;
            }

            //SACO CODIGO DEL DOCUEMNTO RELACIONADO A LA MARCARA
            List<int> DocId = (from t1 in this.dbo.ProductosDocum
                               join t2 in this.dbo.Documentos on t1.idDocumento equals t2.DocId
                               where (t1.idGrupo == grupo) && (t2.Mascara == txtDocMasc)
                               select t2.DocId).ToList();

            int _DocId = 0;
            if (DocId.Count > 0)
            {
                _DocId = DocId[0];
            }
            return _DocId + "";
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public void BorrarDocumento(int idDocumento, int numPagina, int negId)
        {
            ModelState.Clear();
            ArchivosAnexos D = new ArchivosAnexos();
            D.DocId = idDocumento;
            D.NumPagina = numPagina;
            CDoc.BorrarDocumento(D, negId);
            n.NegId = negId;
            BuscarNegocio(Convert.ToDecimal(negId));
        }

        [HttpPost]
        public JsonResult AgregaIndexacion(string _nroDocumento, string _nroPagina, decimal _negId)
        {
            int negocio = Convert.ToInt32(Session["_Negocio"].ToString());
            string txtDocumento = consultDcoId(_nroDocumento, negocio);

            //string txtDocumento = collection["txtDocumento"].ToString();
            string txtPagina = _nroPagina;
            ViewData["MensajeError_"] = "";
            Session["_Error"] = 0;
            try
            {
                //Verifica que el numero de documento que digita este en la lista asignada
                int NedId = int.Parse(((Captura)this.Session["NEGOCIO"]).NegId.ToString());
                this.tDocs = this.CDoc.obtenerDocumentosIndexacion(NedId);
                var DocumentosIdex = tDocs.Find(x => x.DocId == int.Parse(txtDocumento));

                //JFP; abril-2016; verificar que no se indexe mas de un documento con la misma tipologia a no ser que se permita
                int IndexaMultiple = dbo.sp_ValidaIndexaMultiple(Convert.ToInt32(txtDocumento), Convert.ToInt32(_nroDocumento), Convert.ToDecimal(NedId)).ToList().SingleOrDefault().Value;

                //int sn_indexa = Convert.ToInt32(IndexaMultiple.ToString());
                if (IndexaMultiple == 1)
                {
                    if ((txtDocumento.Trim() != string.Empty) & (txtPagina.Trim() != string.Empty))
                    {
                        if (DocumentosIdex != null)
                        {
                            if (Convert.ToInt32(txtPagina) <= this.CDoc.ObtenerNumPaginasNegocio((Captura)this.Session["NEGOCIO"]))
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

                                if (!this.CDoc.buscarPaginaDigitada(c))
                                {
                                    this.CDoc.insertarDocsIndexados(c);
                                    this.cargarPaginasIndexadas();
                                    ViewData["_ValorPagina_"] = (int.Parse(txtPagina) + 1).ToString();
                                    if (this.CDoc.IndexacionTerminada(c))
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
                                    Session["_NumPaginas_"] = this.CDoc.ObtenerNumPaginasNegocio((Captura)this.Session["NEGOCIO"]).ToString();
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
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo Button1_Click " + exception.Message);
                ViewData["MensajeError_"] = "Error en el metodo Button1_Click en indexacion " + exception.InnerException.ToString();
                base.Response.Redirect("/ReIndexacionImg/Index");
            }

            string[] ArrView = { ViewData["MensajeError_"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult FinIndexacion()
        {
            try
            {
                ViewData["MensajeError_"] = "";

                decimal _usu = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                var _neg = Session["_Negocio"];
                var resultado = dbo.sp_ValIndexacion(Convert.ToDecimal(_neg), _usu).SingleOrDefault();
                if (resultado == 1)
                {

                    dbo.sp_ProcReindexacion(2, _usu, Convert.ToDecimal(_neg));

                    //HCamilo: llamado al metodo que reordena pdf
                    ReordenarDocumento(int.Parse(((Captura)this.Session["NEGOCIO"]).NegId.ToString()));

                }
                else if (resultado == 0)
                {
                    ViewData["MensajeError_"] = "No se han indexado todos los documentos aún.";
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en ReIndexacionImg.aspx metodo FinIndexacion " + exception.Message);
                ViewData["MensajeError_"] = exception.Message;
            }
            string[] ArrView = { ViewData["MensajeError_"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }

        /// <summary> 
        /// Función que reordena las paginas del PDF para convertirlo nuevamente a TIFF 
        /// </summary> 
        public void ReordenarDocumento(int NegId)
        {
            //Rutas Actuales y nuevas de los archivos PDF 
            string PathActualPDF = Path.Combine(Server.MapPath("/Content/ArchivosCliente/" + NegId + "/"), NegId + ".pdf");
            string PathNuevoPDF = Path.Combine(Server.MapPath("/Content/ArchivosCliente_BK/"), NegId + ".pdf");

            //Rutas Actuales y nuevas de los archivos TIFF 
            string PathActualTIF = Path.Combine(Server.MapPath("/Content/ArchivosCliente/" + NegId + "/"), NegId + ".TIF");
            string PathNuevoTIF = Path.Combine(Server.MapPath("/Content/ArchivosCliente/" + NegId + "/"), NegId + ".TIF");

            //Verifica si existe el archivo 
            if (System.IO.File.Exists(PathNuevoPDF))
            {
                //Eliminar el archivo 
                System.IO.File.Delete(PathNuevoPDF);
            }
            //Copia el archivo original en una carpeta espejo 
            System.IO.File.Copy(PathActualPDF, PathNuevoPDF);
            //Obtener orden de los documentos por negocio 
            List<spObtenerOrdenDocumentosPorMascaraNegId_Result> ObtenerDoc = dbo.spObtenerOrdenDocumentosPorMascaraNegId(NegId).ToList();

            //Leer documento pdf 
            PDFDocument doc = new PDFDocument(PathNuevoPDF);

            if (ObtenerDoc.Count > 0)
            {
                //Cuenta la cantidad total de paginas del pdf 
                int pageCount = doc.GetPageCount();
                //Declaración Array que almacena el nuevo orden de las paginas 

                //int[] pageOrders = new int[pageCount];
                List<int> pageOrders = new List<int>();

                List<nuevoOrden> Antigorder = new List<nuevoOrden>();
                int contar = 0;
                foreach (spObtenerOrdenDocumentosPorMascaraNegId_Result dato in ObtenerDoc)
                {
                    int pag_actual = dato.NumPagina - 1;
                    nuevoOrden order = new nuevoOrden()
                    {
                        Pagina = pag_actual,
                        Orden = contar
                    };
                    Antigorder.Add(order);
                    contar++;
                }
                //Ordena los items de forma ascendente por la variable orden de la clase (nuevoOrden) 
                IEnumerable<nuevoOrden> neworder = Antigorder.OrderBy(x => x.ObtenerOrden());
                //int contador = 0;
                //Asigna los ordenes al array pageOrders 
                foreach (nuevoOrden ord in neworder)
                {
                    pageOrders.Add(Convert.ToInt32(ord.Pagina));
                    //contador++;
                }
                //Reordena el pdf con las paginas asignadas en el array pageOrders 
                doc.SortPage(pageOrders.ToArray());
                //Reemplaza el documento actual por el reordenado 

                //Elimina archivos originales 
                if (System.IO.File.Exists(PathActualPDF))
                {
                    // The files are not actually removed in this demo 
                    System.IO.File.Delete(PathActualPDF);
                    System.IO.File.Delete(PathActualTIF);
                }

                doc.Save(PathActualPDF);

                dbo.spOrdenarDocumentosPorMascaraNegId(NegId);

                //Convertir el pdf a tiff 
                //int paginasConvertidas = Extends.ConvertirimagenPDFaTIFF(PathActualPDF, PathActualTIF);
                //if (System.IO.File.Exists(PathActualPDF))
                //{
                //    System.IO.File.Move(PathNuevoPDF, PathActualPDF);
                //}
            }

        }
    }

    public class _grillalstResultDocs
    {
        public int docId { get; set; }
        public int? docIdMasc { get; set; }
        public string docDescripcion { get; set; }
    }

    public class _grillalstDocsIndexados
    {
        public int id { get; set; }
        public int? idMasc { get; set; }
        public string documento { get; set; }
        public int pagina { get; set; }
        public decimal negId { get; set; }
    }

    public class newOrden
    {
        public int Pagina { get; set; }
        public int Orden { get; set; }
        public int ObtenerOrden()
        {
            return Orden;
        }
    }
}
