using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using AjaxControlToolkit;
using GestorDocumental.Controllers;
using GestorDocumental.Models;
using GestorDocumental.WebService;
using System.Collections;
using System.Web.UI.WebControls;
using System.Threading;

namespace GestorDocumental.Controllers
{
    public class CapturaInformacionController : Controller
    {

        private CrearFormulariosCaptura formulario = new CrearFormulariosCaptura();

        /// <summary>
        /// Carga la pagina con el formulario y los campos a Capturar
        /// </summary>
        /// <returns>Resultado de la operación y carga la pagina</returns>
        public ActionResult Index()
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

            Session["idETAPA"] = 30;
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            P_Etapas etapas = new P_Etapas();
            string noCaptura = base.Request.QueryString["CAPTURA"].ToString();
            try
            {
                //Valida la cantidad de Capturas realizadas por el Usuario.
                string _idUsuario = Session["_IDUsuario_"].ToString();
                string mensajeCapturas = consultaTotalCapturas(_idUsuario);
                Session["TotalCapturas"] = mensajeCapturas;
                
                AsignacionesController bAsig = new AsignacionesController();
                CapturaController bCap;
                DocumentosController bdoc;

                if (Request.HttpMethod != "POST")
                {
                    Session["lstGrupos"] = null;
                    Session["CamposCCalidad"] = null;
                    if (Session["lstDocumentos"] == null)
                        Session["lstDocumentos"] = formulario.CargarDocumentos((Clientes)this.Session["CLIENTE"]);

                    
                    

                    Session["_NoCaptura"] = noCaptura;
                    switch (noCaptura)
                    {
                        case "1":
                            //<<JFPancho;6-abril-2017;  
                            //---valida que el usuario no este activo en mas de una máquina
                            LogUsuarios x = new LogUsuarios();
                            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                            //---valida si el usuario logueado tiene accceso al modulo
                            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
                            var result = data.spValidaAccesoModulo(idRol, "/CapturaInformacion/Index?Captura=1").FirstOrDefault();

                            if (result == 0)
                            {
                                Response.Redirect("../Home/Index");
                            }
                            //JFPancho >>
                            Session["TITULO"] = "Captura de datos No. 1";
                            Session["ETAPA"] = etapas;
                            etapas.IdEtapa = 30;
                            break;

                        case "2":
                            //<<JFPancho;6-abril-2017;  
                            //---valida que el usuario no este activo en mas de una máquina
                            LogUsuarios x2 = new LogUsuarios();
                            x2.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                            //---valida si el usuario logueado tiene accceso al modulo
                            int? idRol2 = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
                            var result2 = data.spValidaAccesoModulo(idRol2, "/CapturaInformacion/Index?Captura=2").FirstOrDefault();

                            if (result2 == 0)
                            {
                                Response.Redirect("../Home/Index");
                            }
                            //JFPancho >>
                            Session["TITULO"] = "Captura de datos No. 2";
                            etapas.IdEtapa = 40;
                            Session["ETAPA"] = etapas;
                            break;

                        case "3":
                            //<<JFPancho;6-abril-2017;  
                            //---valida que el usuario no este activo en mas de una máquina
                            LogUsuarios x3 = new LogUsuarios();
                            x3.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                            //---valida si el usuario logueado tiene accceso al modulo
                            int? idRol3 = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
                            var result3 = data.spValidaAccesoModulo(idRol3, "/CapturaInformacion/Index?Captura=3").FirstOrDefault();

                            if (result3 == 0)
                            {
                                Response.Redirect("../Home/Index");
                            }
                            //JFPancho >>
                            Session["TITULO"] = "Control de calidad captura";
                            etapas.IdEtapa = 50;
                            Session["ETAPA"] = etapas;
                            break;
                    }

                    bCap = new GestorDocumental.Controllers.CapturaController();
                    Captura c = new Captura();

                    c.NegId = bAsig.ObtenerNegociosXEntrada((Usuarios)this.Session["USUARIO"], etapas);

                    if (c.NegId == 0M)
                    {
                        c.NegId = 0M;
                        this.Session["NEGOCIO"] = c;
                        ViewData["noNegocios"] = "No existen negocios disponibles para esta etapa";
                        Session["NegId"] = 0;
                    }
                    else
                    {

                        int num5, num6;

                        c.NumCaptura = int.Parse(base.Request.QueryString["CAPTURA"].ToString());
                        this.Session["NEGOCIO"] = c;

                        // William; Obtiene el idcase de la etapa anterior y la utiliza en la creacion de la asignacion de tareas
                        // de esta estapa
                        var Case = data.sp_IdCase_Indexacion(c.NegId).ToList();

                        ////BUSCO EL CLIENTE ASOCIADO A ESE NEGOCIO Y LO SOBREESCRIBO EN LA SESSION
                        ////REENDERIZO NUEVAMENTE EL COMBO
                        AsignacionTareas a = new AsignacionTareas
                        {
                            IdEtapa = ((P_Etapas)this.Session["ETAPA"]).IdEtapa,
                            NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                            Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                            IdEstado = 10,
                            HoraInicio = DateTime.Now,
                            idCase = Convert.ToInt32(Case[0]) //William Cicua; agrega campo
                        };

                        if (!bAsig.ExisteEtapa(a))
                            bAsig.insertarAsignacion(a);

                        //Obtenemos el grupo asociado al negocio en proceso
                        int negID = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                        int idGrupo = bCap.obtenerSubGrupo(negID);

                        //Modifica William Cicua; 05/05/2016; valida si hay algun documento al que se le capturen campos.
                        //  si hay documentos no hace nada pero sino hay finaliza la captura y guarda en tb_logEtapas
                        //  recargando la pagina y cargando el sigueinte negocio.

                        decimal neg = 0;
                        Session["NegId"] = ((Captura)this.Session["NEGOCIO"]).NegId;
                        neg = ((Captura)this.Session["NEGOCIO"]).NegId;
                        var documentos = data.sp_DocumentosCaptura(neg).ToList();

                        if (documentos[0].Value == 0)
                        {
                            data.sp_FinlizarCaptura(neg);

                            Response.Redirect("Index?Captura=" + noCaptura);
                        }

                        int num = bCap.ObtenerDocumentoActual(c, (Clientes)this.Session["CLIENTE"], idGrupo);
                        Documentos d = new Documentos { DocId = num };

                        Session["DocId"] = d.DocId;
                        //********************
                        int num2 = bCap.ObtenerGrupoActual(c, d);
                        int num3 = bCap.ObtenerUltimoGrupo(d);

                        //SE DEBE CONSIDERAR QUE MUCHOS DOCUMENTOS  TIENEN SOLO UN GRUPO , POR LO TANTO SE DEBE VALIDAR SI ES EL PRIMERO Y ES UNICO , NO PASAR AL SIGUIENTE DOCUMENTO 
                        //Tambien se debe validar si el grupo ya ha sido caturado
                        if ((num2 == num3 & num3 != 1) | (num2 == num3 & bCap.ExisteCapturaGrupo(c, num3)))
                        {
                            //Obtenemos el grupo asociado al negocio en proceso
                            int negID_docum = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                            int idGrupo_docum = bCap.obtenerSubGrupo(negID_docum);

                            //======= << Modifica: Ivan Rodriguez; 06/05/2016 =============
                            //int num4 = this.bCap.ObtenerSiguienteDocumento(c, (Clientes)this.Session["CLIENTE"], idGrupo_docum);
                            //d.DocId = num4;

                            int num4 = 0;
                            if (bCap.ExisteCapturaGrupo(c, num3))
                            {
                                num4 = bCap.ObtenerSiguienteDocumento(c, (Clientes)this.Session["CLIENTE"], idGrupo_docum);
                                d.DocId = num4;
                            }
                            else
                            {
                                num4 = num;
                            }
                            //=== Ivan Rodriguez >> =======================================


                            //Cargar Lista de documentos
                            IEnumerable<dynamic> lstDocumentos = Session["lstDocumentos"] as dynamic;
                            var documento = lstDocumentos.Where(x => Extends.ObtenerValorReflexion(x, "DocId").ToString() == num4.ToString()).FirstOrDefault();
                            Session["_idDocId"] = documento.DocId;
                            ViewData["_NomDocumento"] = documento.DocDescripcion;

                            if (Session["lstGrupos"] == null)
                                Session["lstGrupos"] = formulario.CargarGruposDocs(d);
                            //Fin de cargar Lista Documentos

                            num5 = bCap.ObtenerSiguienteGrupo(c, d);

                            //Cargar Lista de Grupos de Documentos
                            IEnumerable<dynamic> lstGrupos = Session["lstGrupos"] as dynamic;
                            var grupo = lstGrupos.Where(x => Extends.ObtenerValorReflexion(x, "GDocId").ToString() == num5.ToString()).FirstOrDefault();
                            Session["_GDocId"] = grupo.GDocId;
                            string menuC = cargarNovedades(grupo.GDocId);
                            ViewData["_MenuContextual"] = menuC;
                            ViewData["_NomGrupo"] = grupo.GDocDescripcion;
                            Session["_NomGrupoSession"] = grupo.GDocDescripcion;
                            //Fin de cargar Lista Grupos de Documentos

                            //Obtenemos el grupo asociado al negocio en proceso
                            int negID3 = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                            int idGrupo3 = bCap.obtenerSubGrupo(negID3);
                            num6 = bCap.obtenerUltimoDocumento((Clientes)this.Session["CLIENTE"], idGrupo3, negID3);
                        }
                        else
                        {
                            d.DocId = num;

                            //Cargar Lista de documentos
                            IEnumerable<dynamic> lstDocumentos = Session["lstDocumentos"] as dynamic;
                            var documento = lstDocumentos.Where(x => Extends.ObtenerValorReflexion(x, "DocId").ToString() == num.ToString()).FirstOrDefault();
                            Session["_idDocId"] = documento.DocId;
                            ViewData["_NomDocumento"] = documento.DocDescripcion;

                            if (Session["lstGrupos"] == null)
                                Session["lstGrupos"] = formulario.CargarGruposDocs(d);
                            //Fin de cargar Lista Documentos

                            num5 = bCap.ObtenerSiguienteGrupo(c, d);

                            //Cargar Lista de Grupos de Documentos
                            IEnumerable<dynamic> lstGrupos = Session["lstGrupos"] as dynamic;
                            var grupo = lstGrupos.Where(x => Extends.ObtenerValorReflexion(x, "GDocId").ToString() == num5.ToString()).FirstOrDefault();
                            Session["_GDocId"] = grupo.GDocId;
                            string menuC = cargarNovedades(grupo.GDocId);
                            ViewData["_MenuContextual"] = menuC;

                            ViewData["_NomGrupo"] = grupo.GDocDescripcion;
                            Session["_NomGrupoSession"] = grupo.GDocDescripcion;
                            //Fin de cargar Lista Grupos de Documentos

                            //Obtenemos el grupo asociado al negocio en proceso
                            int _negID = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                            int _idGrupo = bCap.obtenerSubGrupo(_negID);

                            num6 = bCap.obtenerUltimoDocumento((Clientes)this.Session["CLIENTE"], _idGrupo, _negID);
                        }
                        this.CargarCampos();
                        //Ivan Rodriguez: se obtiene el numero bizagi y la accion para mostrar en el titulo de la indexacion
                        //-----------Inicio CambiosIvan Rodriguez
                        ViewData["Negocio"] = "Negocio: ";
                        if (c.NegId != null && c.NegId != 0)
                        {
                            var nombreIndex = data.sp_Indexacion_NegNumbizagiAccion((int?)c.NegId).ToList().FirstOrDefault();

                            ViewData["Negocio"] = ViewData["Negocio"] + c.NegId.ToString() + " |" + nombreIndex;
                        }
                        else
                        {
                            ViewData["Negocio"] = "Negocio: " + c.NegId.ToString();
                        }

                        //---------Fin cambio Ivan Rodriguez


                        ViewData["_Negocio"] = c.NegId.ToString();
                    }
                }
                else
                {
                    this.CargarCampos();
                }

                if (((Captura)this.Session["NEGOCIO"]).NegId != 0M)
                {
                    Documentos documentos2 = new Documentos();
                    documentos2.DocId = Convert.ToInt32(Session["_idDocId"].ToString());

                    bdoc = new GestorDocumental.Controllers.DocumentosController();
                    ViewData["NumPagina"] = bdoc.ObtenrPaginaDocumento(documentos2, (Captura)this.Session["NEGOCIO"]).ToString();
                }
                
                //var validacionUsuarioNeg =

                //=== Modifica: Juliana Pancho; Fecha: 08/FEB/2017 ==============================
                LogRepository.registroCaptura("Ingresa a Captura: " + base.Request.QueryString["CAPTURA"].ToString() + "; Usuario: " + _idUsuario + "; Negocio: " + ((Captura)this.Session["NEGOCIO"]).NegId + "; Documento: " + ViewData["_NomDocumento"] + "; Grupo: " + ViewData["_NomGrupo"]);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo Page_Load " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
            string usuario= Session["_IDUsuario_"].ToString();
            int? resutado = data.sp_Cap_ValidacionNegocioAsignado(((Captura)this.Session["NEGOCIO"]).NegId, Convert.ToDecimal(usuario), etapas.IdEtapa).FirstOrDefault();

            if (((Captura)this.Session["NEGOCIO"]).NegId == 0M)
            {
                return View();
            }
            else if (resutado == 1)
            {
                return View();
            }
            else if (resutado == 0)
            {
                Response.Redirect("Index?Captura=" + noCaptura);
            }else{
                return View();
            }
            
            return View();
        }

        /// <summary>
        /// Genera los menús contextuales para todos los campos del documento.
        /// </summary>
        /// <param name="GdocID">Id del documento a consultar para el menú contextual.</param>
        /// <returns>Cadena de texto con la estructura del menu contextual en Javascript.</returns>
        public string cargarNovedades(int GdocID)
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            List<spConsultarNovedades_Result> lista = data.spConsultarNovedades(GdocID).ToList();

            string menuContextual = "";
            foreach (spConsultarNovedades_Result dat in lista)
            {
                string icono = "";
                if (String.IsNullOrEmpty(dat.NoveIcono))
                    icono = "../../Scripts/MenuContext/ilegible.ico";
                else
                    icono = dat.NoveIcono;

                if (menuContextual == "")
                {
                    menuContextual = "{ '" + dat.NoveDescripcion + "' : {" +
                        "onmousedown: function (menuItem, menu) {" +
                        "if ($(this).attr('readonly') == 'readonly') {" +
                        "$(this).val('');" +
                        "$(this).removeAttr('readonly');" +
                        //"$('.dp').datepicker({maxDate: '+0D', defaulDate: '+1w', changeMounth: true, numberOfMonths: 1, dateFormat: 'dd/mm/yy'});" +
                        //"$('.dpms').datepicker({maxDate: '+0D', defaulDate: '+1w', changeMounth: true, numberOfMonths: 1, dateFormat: 'dd/mm/yy'});" +
                        "}" +
                        "else {" +
                        "$(this).val('" + dat.NoveValor + "');" +
                        "$(this).attr('readonly', true);" +
                        //"$(this).datepicker('destroy');" +
                        "}" +
                        "}, icon: '" + icono + "'" +
                        "}" +
                        "}";
                }
                else
                {
                    menuContextual = menuContextual + "," + "$.contextMenu.separator," +
                        "{ '" + dat.NoveDescripcion + "' : {" +
                        "onmousedown: function (menuItem, menu) {" +
                        "if ($(this).attr('readonly') == 'readonly') {" +
                        "$(this).val('');" +
                        "$(this).removeAttr('readonly');" +
                        //"$('.dp').datepicker({maxDate: '+0D', defaulDate: '+1w', changeMounth: true, numberOfMonths: 1, dateFormat: 'dd/mm/yy'});" +
                        //"$('.dpms').datepicker({maxDate: '+0D', defaulDate: '+1w', changeMounth: true, numberOfMonths: 1, dateFormat: 'dd/mm/yy'});" +
                        "}" +
                        "else {" +
                        "$(this).val('" + dat.NoveValor + "');" +
                        "$(this).attr('readonly', true);" +
                        //"$(this).datepicker('destroy');" +
                        "}" +
                        "}, icon: '" + icono + "'" +
                        "}" +
                        "}";
                }
            }
            return menuContextual;
        }

        /// <summary>
        /// Método para Cargar los Campos asosiados al documento que se va a capturar
        /// </summary>
        private void CargarCampos()
        {
            try
            {
                CamposController bCampos = new CamposController();
                Table tblControls = new Table();
                if (ViewData["_NomGrupo"] != null)
                {
                    GruposDocumentos g = new GruposDocumentos();
                    g.GDocId = Convert.ToInt32(Session["_GDocId"].ToString());

                    P_Etapas e = new P_Etapas();
                    e.IdEtapa = int.Parse(Session["_NoCaptura"].ToString());

                    Captura n = (Captura)this.Session["NEGOCIO"];
                    List<Campos> lstCampos = bCampos.ObtenerCamposCliente(g, e, n);
                    Session["_listaCampos"] = lstCampos;
                    if (lstCampos.Count == 0)
                    {
                        Campos camp = new Campos();
                        camp.Activo = false;
                        camp.CampDescripcion = "-8888";
                        lstCampos.Add(camp);
                    }

                    ///SI LA ETAPA ES 3 , CONTROL DE CALIDAD SE DEBE CARGAR LOS CAMPOS  DE RESPUESTAS ANTERIORES 
                    List<spObtenerRespuestasAnteriores_Result> lstRespAnt = null;
                    if (e.IdEtapa == 3 && ViewData["_NomGrupo"].ToString().Contains("Posiciones") && lstCampos[0].Activo == false)
                    {
                        Session["tId_Etapa"] = e.IdEtapa;

                        Campos tiposCampo = new Campos();
                        tiposCampo.TcId = 13;
                        tiposCampo.GDocId = g.GDocId;
                        tiposCampo.ControlCalidad = true;

                        List<Campos> listasCCalidad = new List<Campos>();
                        listasCCalidad.Add(tiposCampo);
                        if (lstCampos != null)
                        {
                            if (lstCampos.Count == 1 && lstCampos[0].CampDescripcion.Equals("-8888"))
                            {
                                listasCCalidad = null;
                            }
                        }

                        int gDocID = Convert.ToInt32(Session["_GDocId"].ToString());
                        int idETAPA = Convert.ToInt32(e.IdEtapa);
                        string noCaptura = Session["_NoCaptura"].ToString();
                        int _idDocumento = Convert.ToInt32(Session["_idDocId"].ToString());
                        int _NegId = Convert.ToInt32((int)((Captura)this.Session["NEGOCIO"]).NegId);
                        string camposGenerados = "";
                        if (listasCCalidad != null)
                        {
                            camposGenerados = formulario.GenerarCampos(tblControls, listasCCalidad, lstRespAnt, gDocID, idETAPA, noCaptura, _idDocumento, _NegId);
                        }
                        ViewData["CamposGenerados"] = camposGenerados;
                        if (String.IsNullOrEmpty(camposGenerados))
                        {
                            FormCollection collection = new FormCollection();
                            Guardar(collection);
                        }
                    }
                    else if (e.IdEtapa == 3 && ViewData["_NomGrupo"].ToString().Contains("Impuestos") && lstCampos[0].Activo == false)
                    {
                        Session["tId_Etapa"] = e.IdEtapa;

                        Campos tiposCampo = new Campos();
                        tiposCampo.TcId = 14;
                        tiposCampo.GDocId = g.GDocId;
                        tiposCampo.ControlCalidad = true;

                        List<Campos> listasCCalidad = new List<Campos>();
                        listasCCalidad.Add(tiposCampo);

                        int gDocID = Convert.ToInt32(Session["_GDocId"].ToString());
                        int idETAPA = Convert.ToInt32(e.IdEtapa);
                        string noCaptura = Session["_NoCaptura"].ToString();
                        int _idDocumento = Convert.ToInt32(Session["_idDocId"].ToString());
                        int _NegId = Convert.ToInt32((int)((Captura)this.Session["NEGOCIO"]).NegId);
                        string camposGenerados = formulario.GenerarCampos(tblControls, listasCCalidad, lstRespAnt, gDocID, idETAPA, noCaptura, _idDocumento, _NegId);
                        ViewData["CamposGenerados"] = camposGenerados;
                        if (String.IsNullOrEmpty(camposGenerados))
                        {
                            FormCollection collection = new FormCollection();
                            Guardar(collection);
                        }
                    }
                    else if (e.IdEtapa == 3 && ViewData["_NomGrupo"].ToString().Contains("Cabecera") && lstCampos[0].Activo == true || e.IdEtapa == 3 && ViewData["_NomGrupo"].ToString().Contains("Posiciones") && lstCampos[0].Activo == true
                        || e.IdEtapa == 3 && ViewData["_NomGrupo"].ToString().Contains("Impuestos") && lstCampos[0].Activo == true)
                    {
                        lstRespAnt = bCampos.ObtenerRespuestasAnteriores(n);

                        int gDocID = Convert.ToInt32(Session["_GDocId"].ToString());
                        int idETAPA = Convert.ToInt32(((P_Etapas)this.Session["ETAPA"]).IdEtapa);
                        string noCaptura = Session["_NoCaptura"].ToString();
                        int _idDocumento = Convert.ToInt32(Session["_idDocId"].ToString());
                        int _NegId = Convert.ToInt32((int)((Captura)this.Session["NEGOCIO"]).NegId);
                        string camposGenerados = formulario.GenerarCampos(tblControls, lstCampos, lstRespAnt, gDocID, idETAPA, noCaptura, _idDocumento, _NegId);
                        ViewData["CamposGenerados"] = camposGenerados;
                        Session["tId_Etapa"] = null;
                        if (String.IsNullOrEmpty(camposGenerados))
                        {
                            FormCollection collection = new FormCollection();
                            Guardar(collection);
                        }
                    }
                    else
                    {
                        Session["tId_Etapa"] = null;
                        if (e.IdEtapa == 3)
                            lstRespAnt = bCampos.ObtenerRespuestasAnteriores(n);
                        else
                            lstRespAnt = null;

                        int gDocID = Convert.ToInt32(Session["_GDocId"].ToString());
                        int idETAPA = Convert.ToInt32(((P_Etapas)this.Session["ETAPA"]).IdEtapa);

                        //======= << Modifica: Ivan Rodriguez; 06/05/2016 =============
                        //string noCaptura = base.Request.QueryString["CAPTURA"].ToString();
                        string noCaptura = Session["_NoCaptura"].ToString();
                        //=== Ivan Rodriguez >> =======================================


                        int _idDocumento = Convert.ToInt32(Session["_idDocId"].ToString());
                        int _NegId = Convert.ToInt32((int)((Captura)this.Session["NEGOCIO"]).NegId);
                        string camposGenerados = formulario.GenerarCampos(tblControls, lstCampos, lstRespAnt, gDocID, idETAPA, noCaptura, _idDocumento, _NegId);
                        ViewData["CamposGenerados"] = camposGenerados;
                        Campos campos = lstCampos.Find(c => c.CampId == 5);

                        Hashtable hashtable = bCampos.CamposPresentados(bCampos.ObtenerCamposCliente(g, e, n));
                        if (String.IsNullOrEmpty(camposGenerados))
                        {
                            FormCollection collection = new FormCollection();
                            Guardar(collection);
                        }
                    }
                    ///***************************************************************************************
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo CargarCampos " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Guarda la captura del documento actual y procede a cargar el siguiente documento
        /// </summary>
        /// <param name="collection">El formulario que se muestra actualmente.</param>
        /// <returns>Resultado dela operación y carga la pagina</returns>
        [HttpPost]
        public ActionResult Guardar(FormCollection collection)
        {
            #region Guarda el proceso de las Capturas.
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            CamposController bCampos = new CamposController();
            CapturaController bCap = new CapturaController();
            AsignacionesController bAsig = new AsignacionesController();

            try
            {

                //***********************************************
                if (((P_Etapas)this.Session["ETAPA"]).IdEtapa == 50)
                {


                    /*debemos insetar la etapa cruce de identidad  */
                    if (bCap.ExisteControlCalidad((Captura)this.Session["NEGOCIO"]) == 0)
                    {
                        AsignacionTareas a1 = new AsignacionTareas
                        {
                            IdEstado = 30,
                            HoraTerminacion = new DateTime?(DateTime.Now),
                            NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                            IdEtapa = ((P_Etapas)this.Session["ETAPA"]).IdEtapa
                        };

                        bAsig.insertarAsignacion(a1);

                        /*Asignamos el estado de terminacion*/
                        CapturaController controlador = new CapturaController();

                        //CREO LA ETAPA RESPECTIVA Y PASO EL USUARIO
                        string tipoCaptura = base.Request.QueryString["Captura"].ToString();

                        if (tipoCaptura.Equals("3"))
                        {
                            string negId = ((Captura)this.Session["NEGOCIO"]).NegId.ToString();
                            string usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario.ToString();

                            //Volcar los datos de la captura a las tablas del WebService
                            WebServiceModel ws = new WebServiceModel();
                            ws.volcarDatosCaptura_WebService(Convert.ToInt32(negId));

                            //Crear el archivo XML y guardarlo en una ruta.
                            string rutaXML = CrearXML.CrearXMLFile(Convert.ToInt32(negId));

                            if (System.IO.File.Exists(rutaXML))
                            {
                                Thread t = new Thread(() => new WebServiceController().llamadoWebService(rutaXML, usuario.ToString(), negId));
                                t.Start();
                                t.Join();
                            }
                        }
                        base.Response.Redirect("/Captura/confirmaPagina?Captura=" + tipoCaptura);
                        Session["lstGrupos"] = null;
                    }
                }

                //Guardamos los valores de la Grilla de Posiciones y Impuestos
                string[] listaIndices = Request.Form.GetValues("indicePos");
                int tCaptura = Convert.ToInt32(Session["_NoCaptura"].ToString());

                if ((Session["_NomGrupoSession"].ToString().Contains("Posiciones") && tCaptura == 1) || (Session["_NomGrupoSession"].ToString().Contains("Posiciones") && tCaptura == 2)
                    || (Session["_NomGrupoSession"].ToString().Contains("Impuestos") && tCaptura == 1) || (Session["_NomGrupoSession"].ToString().Contains("Impuestos") && tCaptura == 2))
                {
                    if (listaIndices != null && Convert.ToDecimal(Session["NegId"]) != 0)
                    {
                        Session["CamposCCalidad"] = null;
                        guardarValoresGrillaPosiciones();
                        if (tCaptura != 3)
                        {
                            for (int i = 0; i < listaIndices.Count(); i++)
                            {
                                decimal NegId = Convert.ToDecimal(Session["NegId"]);
                                decimal Usuario = Convert.ToDecimal(Session["_IDUsuario_"]);
                                int DocId = int.Parse(Session["_idDocId"].ToString());
                                int NCaptura = Convert.ToInt32(Session["_NoCaptura"]);
                                LogRepository.registro("Error sp_CamposNoCapturadosGrilla SMF Captura Negid: " + NegId + " Usuario:" + Usuario + " DocId:" + DocId + " NCaptura: " + NCaptura + " cantidad: " + listaIndices.Count() + " item: " + i + " listaIndices: " + listaIndices[i]);
                                //William Eduardo Cicua Ruiz
                                data.sp_CamposCheckGrilla(NegId, Usuario, DocId, Convert.ToInt32(listaIndices[i]), NCaptura);
                                //data.sp_CamposNoCapturadosGrilla(NegId, Usuario, DocId, Convert.ToInt32(listaIndices[i]), NCaptura);
                            }
                        }
                    }
                }

                //Guardamos los valores de la Grilla de control de calidad
                if (Session["CamposCCalidad"] != null)
                {
                    guardarControlCalidad();
                }

                bCap = new GestorDocumental.Controllers.CapturaController();
                List<Captura> lstCaptura = new List<Captura>();
                this.InsertarCampos(collection, lstCaptura);

                //Si no existen campos para guardar, debemos crear un campo comodin asociado al grupo actual e insertarlo 
                //con el fin de mantener la posicion actual
                //1. Obtenemos el documento actual  antes de hacer la insercion 
                Documentos DocActual = new Documentos();
                DocActual.DocId = int.Parse(Session["_idDocId"].ToString());

                //2.Obtenemos el grupo actual antes de la insercion
                GruposDocumentos grupoActual = new GruposDocumentos();
                grupoActual.GDocId = int.Parse(Session["_GDocId"].ToString());

                //Creamos el campo y lo anexamos a la lista 
                Campos CampComodin;

                if (lstCaptura.Count == 0 && listaIndices == null)
                {
                    CampComodin = bCampos.ObtenerCampoComodin(grupoActual, (Clientes)this.Session["CLIENTE"]);
                    //CampComodin = bCampos.ObtenerComodinGeneral();
                    Captura CapComodin = new Captura
                    {
                        CampId = CampComodin.CampId,
                        NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                        NegValor = "",
                        Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                        DocId = int.Parse(Session["_idDocId"].ToString()),
                        FechaRegistro = DateTime.Now,
                        NumCaptura = Convert.ToInt32(Session["_NoCaptura"].ToString())
                    };
                    lstCaptura.Add(CapComodin);
                }

                bCap.InsertarCaptura(lstCaptura);

                // William Eduardo Cicua Ruiz
                if (listaIndices == null && tCaptura != 3 && Convert.ToDecimal(Session["NegId"]) != 0)
                {

                    decimal NegId = Convert.ToDecimal(Session["NegId"]);
                    decimal Usuario = Convert.ToDecimal(Session["_IDUsuario_"]);
                    int DocId = int.Parse(Session["_idDocId"].ToString());
                    int NCaptura = Convert.ToInt32(Session["_NoCaptura"]);

                    //Actualiza fechas mal ingresadas por 999

                    data.sp_CamposFecha(NegId, Usuario, DocId, NCaptura);

                    // William Eduardo Cicua Ruiz
                    data.sp_CamposCheck(NegId, Usuario, DocId, NCaptura);
                    //data.sp_CamposNoCapturados(NegId, Usuario, DocId, NCaptura);
                    LogRepository.registro("Error sp_CamposNoCapturados SMF Captura Negid: " + NegId + " Usuario:" + Usuario + " DocId:" + DocId + "NCaptura" + NCaptura);
                }


                decimal Negocio = Convert.ToDecimal(Session["NegId"]);
                //if (tCaptura != 3) // esta parte se quita ya que se pasa al servicio de captura 
                //{
                //    data.sp_Captura_Puntuacion(Negocio); // se usa para quitar los puntos de los valores capturados de los campos tipos moneda 

                //}
                //Obtenemos el grupo asociado al negocio en proceso
                int negID = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                int idGrupo = bCap.obtenerSubGrupo(negID);

                int num = bCap.obtenerUltimoDocumento((Clientes)this.Session["CLIENTE"], idGrupo, negID);

                //Parametrizando los Documentos
                Documentos d = new Documentos();
                d.DocId = bCap.ObtenerDocumentoActual((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"], idGrupo);

                Documentos documentos2 = new Documentos();
                documentos2.DocId = bCap.ObtenerSiguienteDocumento((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"], idGrupo);

                int num2 = 0;
                ///DOCUMENTO ACTUAL = ULTIMO DOCUMENTO Y EL GRUPO INICIAL = ULTIMO GRUPO 
                /// O SIGUIENTE DOCUMENTO = DOCUMENTO ACTUAL (REVISAR ASI ESTABA LA VALIDACION )
                int gactual = bCap.ObtenerGrupoActual((Captura)this.Session["NEGOCIO"], d);
                int gsiguiente = bCap.ObtenerUltimoGrupo(d);

                ///Se debe validar que 
                if ((d.DocId == num) & (gactual == gsiguiente))
                {
                    AsignacionTareas a = new AsignacionTareas
                    {
                        IdEstado = 20,
                        HoraTerminacion = new DateTime?(DateTime.Now),
                        NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                        IdEtapa = ((P_Etapas)this.Session["ETAPA"]).IdEtapa
                    };
                    bAsig.insertarAsignacion(a);
                    data.spValidacionIndexacionErrada(Convert.ToInt32(a.NegId));

                    /*************************/
                    string tipoCaptura = Session["_NoCaptura"].ToString();
                    num2 = 1;
                    //=== Modifica: Juliana Pancho; Fecha: 10/FEB/2017 ============================== 
                    LogRepository.registroCaptura("Cierra etapa en AsignacionTareas Captura: " + Session["_NoCaptura"] + "; Usuario: " + Session["_IDUsuario_"] + "; Negocio: " + a.NegId + "; Documento: " + d.DocId + "; Grupo: " + gactual);

                }
                else
                {
                    int num6;
                    int num3 = bCap.ObtenerGrupoActual((Captura)this.Session["NEGOCIO"], d);
                    int num4 = bCap.ObtenerUltimoGrupo(d);
                    if (num3 == num4)
                    {
                        //Obtenemos el grupo asociado al negocio en proceso
                        int negID_docum = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                        int idGrupo_docum = bCap.obtenerSubGrupo(negID_docum);

                        int num5 = bCap.ObtenerSiguienteDocumento((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"], idGrupo_docum);

                        //Cargar Lista de documentos
                        IEnumerable<dynamic> lstDocumentos = Session["lstDocumentos"] as dynamic;
                        var documento = lstDocumentos.Where(x => Extends.ObtenerValorReflexion(x, "DocId").ToString() == num5.ToString()).FirstOrDefault();
                        Session["_idDocId"] = documento.DocId;

                        //=======<< Modifica: Ivan Rodriguez; 06/05/2016 >>=============
                        //ViewBag["_NomDocumento"] = documento.DocDescripcion;
                        ViewData["_NomDocumento"] = documento.DocDescripcion;

                        Documentos documentos3 = new Documentos();
                        documentos3.DocId = num5;

                        //=======<< Modifica: Ivan Rodriguez; 06/05/2016 >>=============
                        Session["lstGrupos"] = formulario.CargarGruposDocs(documentos3);

                        //Cargar Lista de Grupos de Documentos
                        num6 = bCap.ObtenerSiguienteGrupo((Captura)this.Session["NEGOCIO"], documentos3);
                        //num6 = Convert.ToInt32(data.spObtenerGdocid(documentos3.DocId, idGrupo_docum).First<int?>().ToString());

                        IEnumerable<dynamic> lstGrupos = Session["lstGrupos"] as dynamic;
                        var grupo = lstGrupos.Where(x => Extends.ObtenerValorReflexion(x, "GDocId").ToString() == num6.ToString()).FirstOrDefault();
                        Session["_GDocId"] = grupo.GDocId;


                        ViewData["_NomGrupo"] = grupo.GDocDescripcion;
                        //Fin de cargar Lista de Grupos de Documentos
                    }
                    else
                    {
                        //Obtenemos el grupo asociado al negocio en proceso
                        int negID2 = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                        int idGrupo2 = bCap.obtenerSubGrupo(negID2);

                        Documentos documentos4 = new Documentos();
                        documentos4.DocId = bCap.ObtenerDocumentoActual((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"], idGrupo2);

                        num6 = bCap.ObtenerSiguienteGrupo((Captura)this.Session["NEGOCIO"], documentos4);

                        //Cargar Lista de Grupos de Documentos
                        IEnumerable<dynamic> lstGrupos = Session["lstGrupos"] as dynamic;
                        var grupo = lstGrupos.Where(x => Extends.ObtenerValorReflexion(x, "GDocId").ToString() == num6.ToString()).FirstOrDefault();
                        Session["_GDocId"] = grupo.GDocId;
                        ViewData["_NomGrupo"] = grupo.GDocDescripcion;
                        // Fin de cargar Lista de Grupos de Docuentos
                    }
                }
                this.CargarCampos();
                Documentos documentos5 = new Documentos();
                documentos5.DocId = int.Parse(Session["_idDocId"].ToString());
                DocumentosController bdoc = new GestorDocumental.Controllers.DocumentosController();
                ViewData["NumPagina"] = bdoc.ObtenrPaginaDocumento(documentos5, (Captura)this.Session["NEGOCIO"]).ToString();

                if (num2 == 1)
                {
                    string tipoCaptura = Session["_NoCaptura"].ToString();
                    ///Si se inserta la ultima captura 1 se hace la ejecucion del sp de capturaRadicacion a captura
                    if (tipoCaptura.Equals("1"))
                    {
                        CapturaController controlador = new CapturaController();
                        decimal negId = ((Captura)this.Session["NEGOCIO"]).NegId;
                        controlador.spCapturaRadicacionACaptura(negId);
                    }

                    if (tipoCaptura.Equals("3"))
                    {
                        decimal negID2 = (decimal)((Captura)this.Session["NEGOCIO"]).NegId;
                    }

                    Session["CamposCCalidad"] = null;
                    Session["Id_Grilla"] = null;

                    //Crea el archivo XML cuando termina Control de Calidad con el negocio que finaliza
                    //Solo para las facturas
                    /*if (tipoCaptura.Equals("3"))
                    {
                        string negId = ((Captura)this.Session["NEGOCIO"]).NegId.ToString();
                        string usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario.ToString();

                        //Volcar los datos de la captura a las tablas del WebService
                        WebServiceModel ws = new WebServiceModel();
                        ws.volcarDatosCaptura_WebService(Convert.ToInt32(negId));

                        //Crear el archivo XML y guardarlo en una ruta.
                        string rutaXML = CrearXML.CrearXMLFile(Convert.ToInt32(negId));

                        if (System.IO.File.Exists(rutaXML))
                        {
                            Thread t = new Thread(() => new WebServiceController().llamadoWebService(rutaXML, usuario.ToString(), negId));
                            t.Start();
                            t.Join();
                        }
                    }*/

                    string idUsuario = Session["_IDUsuario_"].ToString();
                    string mensajeCapturas = consultaTotalCapturas(idUsuario);
                    System.Web.HttpContext.Current.Session["TotalCapturas"] = mensajeCapturas;

                    base.Response.Redirect("/Captura/confirmaPagina?Captura=" + tipoCaptura);
                    Session["lstGrupos"] = null;

                    int docid = (int)Session["_idDocId"];

                    data.sp_MarcarDevolucion(Negocio.ToString(), 0, docid);

                    //=== Modifica: Juliana Pancho; Fecha: 10/FEB/2017 ============================== 
                    LogRepository.registroCaptura("Finaliza Captura: " + tipoCaptura + "; Usuario: " + idUsuario + "; Negocio: " + ((Captura)this.Session["NEGOCIO"]).NegId + "; Documento: " + ViewData["_NomDocumento"] + "; Grupo: " + ViewData["_NomGrupo"]);

                    return View();
                }
                data.Connection.Close();
                return View();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo Button1_Click1 " + exception.Message);
                return View();
            }
            #endregion
        }

        /// <summary>
        /// Consulta el Total de las capturas realizadas por el usuario actual en el dia.
        /// </summary>
        /// <param name="idUsuario">Id del Usuario actualmente Logueado</param>
        /// <returns>Mensaje resumiendo el número de capturas que lleva el Usuario en el Dia</returns>
        public string consultaTotalCapturas(string idUsuario)
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            List<spConsultarTotalCapturas_Result> consulta = data.spConsultarTotalCapturas(idUsuario).ToList();
            string mensaje = " Capturas: " + consulta[0].TotalNegocios.ToString();
            return mensaje;
        }

        /// <summary>
        /// Inserta los Campos a la base de datos.
        /// </summary>
        /// <param name="coleccionControles">Los campos del formulario capturado</param>
        /// <param name="lstCaptura">La lista de la base de datos de los controles de captura</param>
        private void InsertarCampos(FormCollection coleccionControles, List<Captura> lstCaptura)
        {
            for (int i = 0; i < coleccionControles.Count; i++)
            {
                List<Campos> listaCampos = (List<Campos>)Session["_listaCampos"];
                string[] keyColeccion = coleccionControles.AllKeys;

                if (listaCampos.Exists(x => x.CampId.ToString() == keyColeccion[i].ToString()))
                {
                    Captura item = new Captura
                    {
                        CampId = Convert.ToInt32(keyColeccion[i].ToString()),
                        NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                        NegValor = coleccionControles[i].ToString(),
                        Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                        DocId = int.Parse(Session["_idDocId"].ToString()),
                        FechaRegistro = DateTime.Now,
                        NumCaptura = int.Parse(Session["_NoCaptura"].ToString())
                    };
                    lstCaptura.Add(item);
                }
            }
        }

        /// <summary>
        /// Guarda la captura realizada en el proceso de Control de Calidad
        /// </summary>
        public void guardarControlCalidad()
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();

            //Insertar valores en las Posiciones.
            string[] listaValoresNuevos = Request.Form.GetValues("CapturaTres");
            List<Captura> listaCapturaTres = (List<Captura>)Session["CamposCCalidad"];

            for (int i = 0; i < listaCapturaTres.Count; i++)
            {
                //Inserta la Captura Tres
                Captura capturaTres = new Captura
                {
                    NegId = listaCapturaTres[i].NegId,
                    NumCaptura = listaCapturaTres[i].NumCaptura,
                    CampId = listaCapturaTres[i].CampId,
                    Indice = listaCapturaTres[i].Indice,
                    NegValor = listaValoresNuevos[i],
                    Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                    FechaRegistro = listaCapturaTres[i].FechaRegistro,
                    DocId = listaCapturaTres[i].DocId
                };

                data.AddToCaptura(capturaTres);
                data.SaveChanges();
            }
            Session["CamposCCalidad"] = null;
        }

        /// <summary>
        /// Guarda en Captura las grillas de Posiciones y Impuestos respectivamente
        /// </summary>
        public void guardarValoresGrillaPosiciones()
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            //Insertar valores en las Posiciones
            int valorGrilla = Convert.ToInt32(Session["Id_Grilla"]);
            int idDocumento = Convert.ToInt32(Session["_idDocId"]);

            WebServiceModel s = new WebServiceModel();
            List<Campos> listaCampos = s.obtenerCamposGrilla(valorGrilla, idDocumento);

            int indice = 1;
            string[] listaIndicesMapea = Request.Form.GetValues("indicePos");

            for (int i = 0; i < listaIndicesMapea.Length; i++)
            {
                listaIndicesMapea[i] = indice.ToString();
                indice++;
            }

            string[] campos;
            foreach (Campos datos in listaCampos)
            {
                campos = Request.Form.GetValues(datos.NomBizagi);
                string[] listaIndices = listaIndicesMapea;

                if (campos.Length < listaIndices.Length)
                {
                    int totalCampos = campos.Length;
                    int totalIndices = listaIndices.Length;

                    int dif = totalIndices - totalCampos;
                    List<string> _listaCampos = campos.ToList();

                    for (int i = 0; i < dif; i++)
                    {
                        _listaCampos.Add("0");
                    }

                    campos = _listaCampos.ToArray();
                }

                for (int i = 0; i < listaIndices.Length; i++)
                {
                    if (campos == null)
                    {
                        campos = new string[listaIndices.Length];
                        for (int a = 0; a < campos.Length; a++)
                        {
                            campos[a] = "";
                        }
                    }

                    string campoGuardar = campos[i];

                    //Insert a la tabla capturas
                    Captura nuevaCaptura = new Captura
                    {
                        Indice = Convert.ToInt32(listaIndices[i]),
                        CampId = Convert.ToInt32(datos.CampId),
                        NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                        NegValor = campoGuardar,
                        Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                        DocId = Convert.ToInt32(Session["_idDocId"].ToString()),
                        FechaRegistro = DateTime.Now,
                        NumCaptura = Convert.ToInt32(Session["_NoCaptura"].ToString())
                    };
                    data.AddToCaptura(nuevaCaptura);
                    data.SaveChanges();
                }
            }
            Session["Id_Grilla"] = null;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        public ActionResult consultarValores(string parametro)
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            var query = (from a in data.Parametros
                         where a.codigo == parametro
                         select a.valor).SingleOrDefault();

            return this.Json(new { data = query });
        }

        //Funcion creada para validar que la parametrizacion del campo se encuentre en base
        [HttpPost]
        public ActionResult validaParamCamposDep(int _CampId, int _opcionLst)
        {
            try
            {
                GestorDocumentalEnt data = new GestorDocumentalEnt();
                int result = Convert.ToInt32(data.sp_ValidaParamCampDependientes(_CampId, _opcionLst).ToList().SingleOrDefault());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion metodo validaParamCamposDep, " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        //funcion creada para Retornar la lista de los campos que se deben activar para el campo seleccionado
        [HttpPost]
        public ActionResult lstCamposDep(int _campId)
        {
            try
            {
                GestorDocumentalEnt data = new GestorDocumentalEnt();
                //List<sp_CamposDependientes_Result> lstCamp = new List<sp_CamposDependientes_Result>();
                List<sp_CamposDependientes_Result> lstCamp = new List<sp_CamposDependientes_Result>();
                lstCamp = data.sp_CamposDependientes(_campId).ToList();
                List<int> ls = new List<int>();
                for (int i = 0; i < lstCamp.Count; i++)
                {
                    ls.Add(lstCamp[i].NroCampId);
                }
                return Json(ls, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion metodo lstCamposDep, " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }

        }

        [HttpPost]
        public ActionResult cargaListaDepend(int opcionLst)
        {
            try
            {
                int result = 1;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion metodo cargaListaDepend, " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// funcion creada para obtener el codigo padre para las listas en cascada
        /// </summary>
        /// <param name="_codId"></param>
        /// <param name="_campId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult obtieneCodPadre(string _codId, int _campId)
        {
            try
            {
                GestorDocumentalEnt data = new GestorDocumentalEnt();

                var codPadre = (from cc in data.CodigosCampo
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

        // William Eduardo Cicua Ruiz
        [HttpPost]
        public ActionResult Checks(string CampId)
        {
            try
            {

                GestorDocumentalEnt data = new GestorDocumentalEnt();
                int Campo = Convert.ToInt32(CampId);

                var campos = data.sp_CamposCheckAlmenosUno(Campo).ToList();

                return Json(campos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion metodo cargaListaDepend, " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        // Ivan Rodriguez
        [HttpPost]
        public ActionResult marcarNegocioDevolucion()
        {
            try
            {
                GestorDocumentalEnt data = new GestorDocumentalEnt();
                string negId = ((Captura)this.Session["NEGOCIO"]).NegId.ToString();
                int docid = (int)Session["_idDocId"];

                data.sp_MarcarDevolucion(negId, 1, docid);

                ViewData["_CodigoError"] = 1;
                ViewData["_descripcion"] = "Exitoso";

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacionControler metodo marcarNegocioDevolucion " + exception.Message + "__" + exception.StackTrace);

                ViewData["_CodigoError"] = 0;
                ViewData["_descripcion"] = exception.Message;
            }
            string[] ArrView = { ViewData["_CodigoError"].ToString(), ViewData["_descripcion"].ToString() };
            return Json(ArrView, JsonRequestBehavior.AllowGet);
        }
    }
}
