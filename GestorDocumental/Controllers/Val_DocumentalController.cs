 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.OleDb;
using GestorDocumental.Models;
using System.Web.UI.WebControls;
using System.Collections;
using Telerik.Web.Mvc;
using System.IO;
using AjaxControlToolkit;
using GestorDocumental.Controllers;
using GestorDocumental.WebService;


namespace GestorDocumental.Controllers
{
    public class Val_DocumentalController : Controller
    {
        //
        // GET: /Val_Documental/
        GestorDocumentalEnt db = new GestorDocumentalEnt();
        private GestorDocumental.Controllers.AsignacionesController bAsig = new Controllers.AsignacionesController();
        private GestorDocumental.Controllers.CapturaController bCap;
        private CamposController bCampos = new Controllers.CamposController();
        private CrearFormulariosCaptura formulario = new CrearFormulariosCaptura();
        int codProceso = 0;
        int codEtapa = 220; //Validación Documental
        
        public ActionResult Val_Documental()
        {
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
            Session["idETAPA"] = 220;
            //<<JFPancho;6-abril-2017;  
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = db.spValidaAccesoModulo(idRol, "/Val_Documental/Val_Documental").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            
            Session["TITULO"] = "0";
            ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            decimal cliNit = Convert.ToDecimal(((Clientes)Session["CLIENTE"]).CliNit);

            //ViewBag.pageLoad = "<script type=\"text/javascript\">pageLoad();</script>";

            Session["lstGrupos"] = null;
            if (Session["lstDocumentos"] == null)
                Session["lstDocumentos"] = formulario.CargarDocumentos((Clientes)this.Session["CLIENTE"]);

            //etapa 220= validacion documental
            //decimal negId = db.spObtenerSiguienteEtapa(_idUsuarioProc, 220).ToList().SingleOrDefault().Value;
            decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, codEtapa).ToList().SingleOrDefault().Value;

            ViewData["_negId"] = negId;
            ViewData["_mensajeInformacion"] = "";

            //Ivan Rodriguez: se obtiene el numero bizagi y la accion para mostrar en el titulo de la indexacion
            //-----------Inicio CambiosIvan Rodriguez
            if (negId != null && negId != 0)
            {
                var nombreIndex = db.sp_Indexacion_NegNumbizagiAccion((int?)negId).ToList().FirstOrDefault();

                ViewData["TITULO"] = "" + negId + " |" + nombreIndex;
            }
            //---------Fin cambio Ivan Rodriguez

            //sub producto asiciado al negocio
            //ViewData["_negIdProduc"] = db.spValDoc_Produc_Negocio(Convert.ToInt32(negId)).ToList().SingleOrDefault();
            List<spValDoc_Produc_Negocio_Result> _lstP = db.spValDoc_Produc_Negocio(Convert.ToInt32(negId)).ToList();
            string producSubpro = null;
            foreach (spValDoc_Produc_Negocio_Result item in _lstP)
            {
                //ViewData["_negIdProduc"] = item.producto;
                //ViewData["_negIdSubProduc"] = item.subproducto;
                producSubpro = item.producto;
                producSubpro = producSubpro + " - " + item.subproducto;
            }
            ViewData["_negIdProduc"] = producSubpro;

            int num5;
            try
            {
                if (negId != 0)
                {
                    codProceso = 1;
                    Session["_neg_"] = System.Configuration.ConfigurationManager.AppSettings["ClientFiles"] + "/" + negId + "/" + negId + ".TIF";

                    //valida que el negId seleccionado no tenga asociada la etapa de validacion documental
                    int sn_Existe = Convert.ToInt32(db.spValDoc_SN_ExistNeg(negId, _idUsuarioProc).ToList().SingleOrDefault().SnExist);

                    //si no existe crea la etapa de validacion documental
                    if (sn_Existe == 0)
                    {
                        db.spIU_EtapaValidDoc(codProceso, negId, _idUsuarioProc, codEtapa);
                    }

                    //-----------------------------------------------------------------------------
                    //-----------------------------------------------------------------------------
                    this.bCap = new GestorDocumental.Controllers.CapturaController();
                    Captura c = new Captura();

                    P_Etapas etapas = new P_Etapas();
                    etapas.IdEtapa = codEtapa;
                    Session["ETAPA"] = etapas;

                    string noCaptura = "1";
                    Session["_NoCaptura"] = noCaptura;

                    Session["lstGrupos"] = null;

                    c.NegId = negId;
                    c.NumCaptura = 1;
                    this.Session["NEGOCIO"] = c;

                    //Obtenemos el grupo asociado al negocio en proceso
                    int idGrupo = obtenerIdGrupo(Convert.ToInt32(negId));
                    int num = this.bCap.ObtenerDocumentoActual(c, (Clientes)this.Session["CLIENTE"], Convert.ToInt32(idGrupo));
                    Documentos d = new Documentos { DocId = num };


                    if (Session["lstDocumentos"] == null)
                        Session["lstDocumentos"] = formulario.CargarDocumentos((Clientes)this.Session["CLIENTE"]);

                    //if (Session["lstGrupos"] == null)
                    //    Session["lstGrupos"] = formulario.CargarGruposDocs(d);

                    num5 = ObtenerSiguienteGrupo(d);

                    //IEnumerable<dynamic> lstGrupos = Session["lstGrupos"] as dynamic;
                    //var grupo = lstGrupos.Where(x => Extends.ObtenerValorReflexion(x, "GDocId").ToString() == num5.ToString()).FirstOrDefault();

                    //Session["_GDocId"] = grupo.GDocId;
                    Session["_GDocId"] = num5;


                    Session["_idDocId"] = num;
                    //-----------------------------------------------------------------------------
                    //-----------------------------------------------------------------------------

                    //********************************************************************************
                    //se obtienen las paginas asociadas al negocio
                    List<spObtenerDocumentosPaginas_Result> _paginasNeg = db.spObtenerDocumentosPaginas(negId).ToList();
                    string cadenaMenu = null;
                    int docId;
                    foreach (spObtenerDocumentosPaginas_Result data in _paginasNeg)
                    {
                        docId = data.ID;
                        if (string.IsNullOrEmpty(cadenaMenu))
                        {
                            cadenaMenu = "<li onclick='alerta(" + data.Pagina + "," + data.ID + ")'><a>" + data.Documento + "</a></li>";
                        }
                        else
                        {
                            cadenaMenu = cadenaMenu + "<li onclick='alerta(" + data.Pagina + "," + data.ID + ")'><a>" + data.Documento + "</a></li>";
                        }
                    }
                    ViewData["_cadenaMenu"] = cadenaMenu;
                    //********************************************************************************
                    //se obtienen politicas generales
                    //parametros: @cod_TCausal: define el tipo de causal que se va a cargar (especifica o general)
                    //            @codProceso: define que proceso se esta realizando(1=VDoc, 2=VDact)

                    List<spValDoc_ListaCausales_Result> _politicasG = db.spValDoc_ListaCausales(2, 1).ToList();
                    string cadenaPoliticas = null;
                    int btns = 0;

                    foreach (spValDoc_ListaCausales_Result data in _politicasG)
                    {
                        if (string.IsNullOrEmpty(cadenaPoliticas))
                        {
                            cadenaPoliticas = cadenaPoliticas + "<tr><td><label>" + data.nom_causal + "</label><input type='hidden' name='hidden" + btns + "' id='" + data.cod_causal + "'/></td><td><input type='radio' id='btn" + btns + "'  name='name" + btns + "' value='0'/>SI</td><td><input type='radio' id='btn" + btns + "' name='name" + btns + "' value='1'/>NO</td></tr>";
                        }
                        else
                        {
                            cadenaPoliticas = cadenaPoliticas + "<tr><td><label>" + data.nom_causal + "</label><input type='hidden' name='hidden" + btns + "' id='" + data.cod_causal + "'/></td><td><input type='radio' id='btn" + btns + "' name='name" + btns + "' value='0'/>SI</td><td><input type='radio' id='btn" + btns + "' name='name" + btns + "' value='1'/>NO</td></tr>";
                        }

                        btns++;
                    }
                    ViewData["_cadenaPoliticas"] = cadenaPoliticas;
                    db.spValDoc_DocExistentes(Convert.ToInt32(negId));
                }
                else
                {
                    ViewData["_cadenaMenu"] = "";
                    ViewData["_cadenaPoliticas"] = "";
                    ViewData["_negId"] = 0;
                    ViewData["_negIdProduc"] = "";
                    //ViewData["_negIdSubProduc"] = "";
                    ViewData["_mensajeInformacion"] = "No existen negocios disponibles para esta etapa";
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Val_Documental.aspx" + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }

            return View();
        }

        public int obtenerIdGrupo(int _negId)
        {
            int idGrupo = Convert.ToInt32(db.spObtenerSubgrupo(_negId).SingleOrDefault().ToString());
            return idGrupo;
        }

        /// <summary>
        /// Método para Cargar los Campos asosiados al documento que se va a validar
        /// </summary>
        #region Cargar Campos tipo Captura
        [HttpPost]
        public JsonResult CargarCampos()
        {
            try
            {
                Table tblControls = new Table();

                GruposDocumentos g = new GruposDocumentos();
                g.GDocId = Convert.ToInt32(Session["_GDocId"].ToString());

                P_Etapas e = new P_Etapas();
                e.IdEtapa = 1;

                Captura n = (Captura)this.Session["NEGOCIO"];
                List<Campos> lstCampos = this.bCampos.ObtenerCamposCliente(g, e, n);
                Session["_listaCampos"] = lstCampos;
                if (lstCampos.Count == 0)
                {
                    Campos camp = new Campos();
                    camp.Activo = false;
                    lstCampos.Add(camp);
                }

                List<spObtenerRespuestasAnteriores_Result> lstRespAnt = null;

                int gDocID = Convert.ToInt32(Session["_GDocId"].ToString());
                int idETAPA = Convert.ToInt32(((P_Etapas)this.Session["ETAPA"]).IdEtapa);
                //string noCaptura = base.Request.QueryString["CAPTURA"].ToString();
                int _idDocumento = Convert.ToInt32(Session["_idDocId"].ToString());
                int _NegId = Convert.ToInt32((int)((Captura)this.Session["NEGOCIO"]).NegId);
                string camposGenerados = formulario.GenerarCampos(tblControls, lstCampos, lstRespAnt, gDocID, idETAPA, "0", _idDocumento, _NegId);
                ViewData["CamposGenerados"] = camposGenerados;
                Campos campos = lstCampos.Find(c => c.CampId == 5);

                Hashtable hashtable = this.bCampos.CamposPresentados(this.bCampos.ObtenerCamposCliente(g, e, n));
                if (String.IsNullOrEmpty(camposGenerados))
                {
                    FormCollection collection = new FormCollection();
                    //Guardar(collection);
                }

                return Json(ViewData["CamposGenerados"], JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Validacion Documental metodo CargarCampos " + exception.Message);
                throw;
            }
        }
        #endregion

        [HttpPost]
        public JsonResult generaCampos(int _DocId)
        {
            try
            {
                //consultar el nombre del documento
                ViewData["nomDoc"] = (from d in db.Documentos
                                      where d.DocId == _DocId
                                      select d.DocDescripcion).SingleOrDefault();

                int _existCausales = 1;
                CrearFormulariosCaptura formularioValDoc = new CrearFormulariosCaptura();
                //********************************************************************************
                //sp spValDoc_DocCampos; obiene la consulta de los campos correspondientes al doc seleccionado
                //parametros: @DocId: codigo del documento seleccionado
                //            @codProceso: define que proceso se esta realizando(1=VDoc, 2=VDact)
                List<spValDoc_DocCampos_Result> lstSpCausales = db.spValDoc_DocCampos(_DocId, 1).ToList();
                if (lstSpCausales.Count == 0)
                {
                    _existCausales = 0;
                }
                List<tvalDoc_Causales> lstCausales = new List<tvalDoc_Causales>();
                decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
                decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, codEtapa).ToList().SingleOrDefault().Value;
                db.sp_ReglaAuto_reiniciar();
                foreach (spValDoc_DocCampos_Result item in lstSpCausales)
                {
                    db.sp_ReglasAutomaticas(negId, _DocId, item.cod_causal, _idUsuarioProc);
                }
                db.sp_ReglasAutomaticas_MenosUno(negId, _DocId, _idUsuarioProc);
                lstSpCausales = db.spValDoc_DocCampos(_DocId, 1).ToList();
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
                    cmp.LongMax = item.LongMax;
                    cmp.PosX = item.PosX;
                    cmp.PosY = item.PosY;
                    cmp.CodFormulario = item.CodFormulario;

                    lstCausales.Add(cmp);
                }
                Table tbl = new Table();
                string campos = formulario.GenerarCamposValDoc(tbl, lstCausales, 100, 0, _DocId, Convert.ToInt32(negId), 1);
                campos = campos.Replace('"', '\'');
                if (_existCausales == 0)
                {
                    ViewData["_camposValDoc"] = 1;
                }
                else
                {
                    ViewData["_camposValDoc"] = campos;
                }
                string[] ArrView = { ViewData["_camposValDoc"].ToString(), ViewData["nomDoc"].ToString() };
                //ViewData["_camposValDoc"] = campos;
                Session["_lstValDoc"] = lstCausales;
                return Json(ArrView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Validacion Documental metodo generaCampos " + exception.Message);
                throw;
            }

        }

        [HttpGet]
        public JsonResult snCausalesCompletas(decimal _negId, int _snIndx, string _observaciones)
        {
            try
            {
                int resultado = 2;

                Session["_txtObservaciones"] = _observaciones;
                Session["_vlrChck"] = _snIndx;

                if (_snIndx == 1)
                {
                    finValidacion();
                    resultado = 1;
                    Session["_vlrChck"] = 2;
                }
                else
                {
                    resultado = Convert.ToInt32(db.spValDoc_ValidaCausalesCompletas(_negId).SingleOrDefault());

                    //modifica Camilo Padilla; 22-junio-2016
                    if (resultado == 1)
                    {
                        db.spValDoc_DocExistentes(Convert.ToInt32(_negId));
                    }
                }

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en  metodo snExisteCausal " + es.Message);
                throw;
            }
        }

        [HttpGet]
        public JsonResult snExisteCausal(decimal _negId, int _snIndx, string _observaciones)
        {
            try
            {
                Session["_txtObservaciones"] = _observaciones;
                Session["_vlrChck"] = _snIndx;
                var resultado = db.spValDoc_SNExisteCausales(_negId).SingleOrDefault();
                return Json(Convert.ToInt32(resultado), JsonRequestBehavior.AllowGet);
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en  metodo snExisteCausal " + es.Message);
                throw;
            }

        }


        [HttpPost]
        [GridAction]
        public virtual ActionResult getConsulCausales(int _negId)
        {
            var grillaCausales = _getConsulCausales(_negId);
            //agregar validacion cuando no traiga resultados la consulta para genrerar alerta

            return View(new GridModel<grilla_CausalesNeg>(grillaCausales));
        }

        public IEnumerable<grilla_CausalesNeg> _getConsulCausales(int _negId)
        {
            try
            {

                var grillaCausales = db.spValDoc_ConsultaCausales(_negId, 1);

                List<grilla_CausalesNeg> _grillaConsulCausales = new List<grilla_CausalesNeg>();
                foreach (spValDoc_ConsultaCausales_Result item in grillaCausales)
                {
                    grilla_CausalesNeg data = new grilla_CausalesNeg();
                    data._Documento = item.DocDescripcion;
                    data._NegId = item.NegId;
                    data._nomCausal = item.nom_causal;
                    data._descCausal = item.desc_causal;
                    data._fecValidacion = item.fec_validacion.ToString();
                    data._NegId = item.NegId;

                    _grillaConsulCausales.Add(data);
                }
                return _grillaConsulCausales;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult CausalesEspecificas(string _campId, string _snCausal, decimal _negId, int cantCausal)
        {
            int vlrAtert = 0;
            try
            {
                for (int i = 0; i < cantCausal; i++)
                {
                if (Convert.ToInt32(_snCausal.Split('_')[i]) == -1)
                {
                    int existe = Convert.ToInt32(db.spValDoc_VlrCausales(Convert.ToInt32(_campId.Split('_')[i]), Convert.ToInt32(_snCausal.Split('_')[i]), _negId).ToList().SingleOrDefault());

                    if (existe == 0)
                    {
                        vlrAtert = 1;
                    }
                }
                else
                {
                    List<tvalDoc_Causales> lstCausales = new List<tvalDoc_Causales>();
                    lstCausales = Session["_lstValDoc"] as List<tvalDoc_Causales>;
                    
                    var a = (from b in lstCausales
                             where b.cod_causal == Convert.ToInt32(_campId.Split('_')[i])
                             select b.cod_causal).SingleOrDefault();

                    int _codCausal = Convert.ToInt32(a);
                    int sn = Convert.ToInt32(_snCausal.Split('_')[i]);
                    bool snCausal = Convert.ToBoolean(sn);
                    decimal _idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
                    db.spValDoc_InsertCausales(_codCausal, snCausal, _negId, _idUsuario);
                }
            }

                return Json(vlrAtert, JsonRequestBehavior.AllowGet);
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en Validación Documental, metodo: CausalesEspecificas " + es.Message);
                throw;
            }
        }

        [HttpPost]
        public void InsertaCausales(int _codCausal, int _snCausal, decimal _negId)
        {
            decimal _idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            db.spValDoc_InsertCausales(_codCausal, Convert.ToBoolean(_snCausal), _negId, _idUsuario);
        }

        [HttpPost]
        public void finValidacion()
        {
            codProceso = 2;
            try
            {
                decimal negId = Convert.ToDecimal(Session["_negId_"]);
                decimal _idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
                object vlrChck = Session["_vlrChck"];
                object txtObservaciones = Session["_txtObservaciones"];

                if (Convert.ToInt32(vlrChck) == 1) //SI ES 1 LA INDEXACION ESTA ERRADA
                {

                    if (Convert.ToString(txtObservaciones) == " ")
                    {
                        Convert.ToString(txtObservaciones);
                        txtObservaciones = "USUARIO NO INGRESÓ OBSERVACIONES - SE BORRAN ETAPAS (INDEXACION/CAPTURA/VALIDACION DOCUMENTAL) ";
                    }
                    else
                    {
                        txtObservaciones = txtObservaciones + " - SE BORRAN ETAPAS (INDEXACION/CAPTURA/VALIDACION DOCUMENTAL)";
                    }

                    //se ejecuta el sp que elimina las etapas del negocio para que este caiga de nuevo en indexacion
                    db.sp_DevolucionIndexacion(_idUsuario, negId, txtObservaciones.ToString());
                }
                else if (Convert.ToInt32(vlrChck) == 0)
                {
                    if (Convert.ToString(txtObservaciones) == " ")
                    {
                        Convert.ToString(txtObservaciones);
                        txtObservaciones = "USUARIO NO INGRESÓ OBSERVACIONES ";
                    }
                    db.spValDoc_Obs_Index(negId, Convert.ToString(txtObservaciones), Convert.ToBoolean(vlrChck), _idUsuario);
                    db.spIU_EtapaValidDoc(codProceso, negId, _idUsuario, codEtapa);
                    
                    //cambio para que genere las causales de documento faltante al terminar de validar el ultimo documento 
                    int resultado = 0;
                    resultado = Convert.ToInt32(db.spValDoc_ValidaCausalesCompletas(negId).SingleOrDefault());// este sp valida que todos los documentos que requieren validar causales las tengan evaluadas

                    //modifica Camilo Padilla; 22-junio-2016
                    if (resultado == 1)
                    {
                        db.spValDoc_DocExistentes(Convert.ToInt32(negId));
                    }
                    //fin cambio
                }
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en Validación Documental, metodo: finValidacion " + es.Message);
                throw;
            }
        }

        public ActionResult confirmaValidacion(decimal _negId)
        {
            ViewData["_negId"] = _negId;
            Session["_negId_"] = _negId;
            return View();
        }

        //JFP: se agrega funcion; junio/2016
        //Obtiene el siguiente grupo que se va a ejecutar en el proceso de captura
        public int ObtenerSiguienteGrupo(Documentos D)
        {
            int num2;
            try
            {
                GestorDocumentalEnt gd = new GestorDocumentalEnt();
                num2 = Convert.ToInt32(gd.spValDoc_ObtenerSigGrupo(new int?(D.DocId)).First<int?>().ToString());
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ObtenerSiguienteGrupo " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

    }

    public class causalesCap
    {
        public int negid { get; set; }
        public int codCausal { get; set; }
        public int sncausal { get; set; }
        public int idusuario { get; set; }
    }

    public class grilla_CausalesNeg
    {
        public string _Documento { get; set; }
        public decimal _NegId { get; set; }
        public int _codCausal { get; set; }
        public string _nomCausal { get; set; }
        public string _descCausal { get; set; }
        public string _fecValidacion { get; set; }
    }
}
