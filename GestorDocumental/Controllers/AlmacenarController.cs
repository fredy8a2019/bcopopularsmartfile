using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class AlmacenarController : Controller
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        #region Metodos de la pagina de crear la Unidad documental
        public ActionResult CrearUnidad()
        {
            if (Session["CLIENTE"] != null)
            {
                ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
                Session["TITULO"] = "Crear Unidad Documental";

                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        [HttpPost]
        public ActionResult Guardar(FormCollection collection)
        {
            alm_UnidadDocumental unidadDObjeto = new alm_UnidadDocumental();
            alm_GruposCUD grupoAlmacenado;
            alm_CapturaUD capturaUD;
            List<alm_GruposCUD> listaGrupos = new List<alm_GruposCUD>();
            List<alm_CapturaUD> listCaptura = new List<alm_CapturaUD>();
            UnidadDocumentalModel unidadDModel = new UnidadDocumentalModel(db);
            ParametrosController parametros = new ParametrosController(db);

            unidadDObjeto.Cliente = ((GestorDocumental.Models.Clientes)Session["CLIENTE"]).CliNit;
            unidadDObjeto.TipoContenedor = Convert.ToInt32(collection["tipoContenedor"]);
            unidadDObjeto.DestinoId = Convert.ToInt32(collection["destinoAlmacenamiento"]);
            unidadDObjeto.FechaCreacion = DateTime.Now;
            unidadDObjeto.UsuarioCreacion = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
            unidadDObjeto.Estado = 1;
            unidadDObjeto.Oficina = Convert.ToInt32(collection["oficina"]);

            //Guarda el los dato de la unidad documental creada
            //Y crea la nueva asignacion de tareas para la Unidad documental creada
            long CUD = unidadDModel.Add(unidadDObjeto);
            string parametroImpresion = parametros.get("TYPO_PRINT_ALM");

            var llaves = collection.AllKeys;

            for (int i = 0; i < llaves.Length; i++)
            {
                //Busca los subproudctos seleccionados
                if (llaves[i].Contains("subProc_"))
                {
                    grupoAlmacenado = new alm_GruposCUD();
                    grupoAlmacenado.CUD = CUD;
                    grupoAlmacenado.GruId = Convert.ToInt32(llaves[i].Split(new String[] { "subProc_" },
                                 StringSplitOptions.RemoveEmptyEntries)[0].ToString());
                    grupoAlmacenado.Activo = true;
                    listaGrupos.Add(grupoAlmacenado);
                }

                //Busca los los campos dinamicos pintados
                IEnumerable<dynamic> campos = Session["listaCamposDinamicos"] as dynamic;

                if (llaves[i].Contains("idCampo_"))
                {
                    capturaUD = new alm_CapturaUD();

                    string idcampo = llaves[i].Split(new String[] { "idCampo_" },
                                  StringSplitOptions.RemoveEmptyEntries)[0].ToString();

                    var campo = campos.Where(x => Extends.ObtenerValorReflexion(x, "CampId").ToString() == idcampo).FirstOrDefault();

                    if (campo.TcId == 5)
                    {
                        capturaUD.CUD = CUD;
                        capturaUD.CampId = int.Parse(idcampo);
                        capturaUD.Valor = CodigoDelItemLista(int.Parse(collection[llaves[i]]));
                        capturaUD.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        capturaUD.Fecha = DateTime.Now;
                    }
                    else
                    {
                        capturaUD.CUD = CUD;
                        capturaUD.CampId = int.Parse(idcampo);
                        capturaUD.Valor = collection[llaves[i]];
                        capturaUD.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        capturaUD.Fecha = DateTime.Now;

                    }

                    listCaptura.Add(capturaUD);
                }
            }

            //Guarda los datos de los grupos que estan almacenado en esa unidad documental
            unidadDModel.Add(listaGrupos);

            //Guarda los datos adicionales de la unidad documental
            unidadDModel.Add(listCaptura);

            ViewData["CUD"] = CUD;
            ViewData["parametroImpresion"] = parametroImpresion;

            if (parametroImpresion == "2")
            {
                //Queda pendiente crear el codigo de la zebra
            }

            return View();
        }
        #endregion

        #region Metodos de la pagina de Almacenar en la Unidad Documental
        public ActionResult Almacenar()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Almacenar";
                ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;

                //Obtener el nombre de la oficina.
                decimal _cliNit = ((Usuarios)this.Session["USUARIO"]).CliNit;

                obtenerFiltros();
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        [HttpGet]
        public JsonResult _getDataCUD(long numeroCud)
        {
            try
            {
                UnidadDocumentalModel modeloUD = new UnidadDocumentalModel(db);
                var datos = modeloUD.getCUD(numeroCud);

                //Obtiene la informacion de la Unidad documental consultada. Y extrae los subProductos asignados
                //a la Unidad documental.
                List<spAlm_ConsultaDataUD_Result> lstDatos = new List<spAlm_ConsultaDataUD_Result>();
                lstDatos = db.spAlm_ConsultaDataUD(numeroCud).ToList();

                Session["CUD"] = numeroCud;
                return Json(datos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en  metodo _getDataCUD " + es.Message);
                throw;
            }
        }

        [HttpPost]
        public JsonResult _CrearEtapaBloqueo(int numeroCud)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            int usuario = Convert.ToInt32(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);

            List<spAlm_CrearAsignacionTarea_Result> lstResultados = new List<spAlm_CrearAsignacionTarea_Result>();
            lstResultados = dbo.spAlm_CrearAsignacionTarea(numeroCud, usuario).ToList();

            return Json(lstResultados, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult almacenarPistoleo(string numeroDocumento)
        {
            numeroDocumento = numeroDocumento.Replace('?', '_');
            int _negId = Convert.ToInt32(db.spAlm_validarReglas(numeroDocumento).SingleOrDefault());
            if (_negId != 0)
            {
                decimal _idusuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;

                //Guardar el documento en la Unidad Documental, con el pistoleo
                int posicion = obtenerUlitmaPosicionCUD(long.Parse(Session["CUD"].ToString()));
                if (posicion == 0)
                    posicion = 1;
                else
                    posicion = posicion + 1;

                GuardarCUD(posicion.ToString(), "0", Session["CUD"].ToString(), _negId);
                return Json(_negId, JsonRequestBehavior.AllowGet);
            }
            return Json(_negId, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Grilla busqueda por Lotes
        [HttpPost]
        [GridAction]
        public virtual ActionResult getAlmacenarLote(long NoLote)
        {
            Session["_NoLote"] = NoLote;
            var grilla = _getAlmacenarLote(NoLote);
            return View(new GridModel<grilla_AlmacenarLote>(grilla));
        }

        public IEnumerable<grilla_AlmacenarLote> _getAlmacenarLote(long NoLote)
        {
            UnidadDocumentalModel modeloUD = new UnidadDocumentalModel(db);
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_AlmacenarLote(NoLote, long.Parse(Session["CUD"].ToString()));
            List<grilla_AlmacenarLote> _grillaAlm = new List<grilla_AlmacenarLote>();

            foreach (spAlm_AlmacenarLote_Result item in grilla)
            {
                grilla_AlmacenarLote data = new grilla_AlmacenarLote();
                data._id = item.id;
                data._subgrupo = item.subgrupo;
                data._principales = item.principales;
                data._fechaRecepcion = item.fechaRecepcion.ToString();
                data._numeroLote = item.numeroLote;
                data._fechaCargue = item.fechaCargue.ToString();
                data._tipo = "Lote";

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }
        #endregion

        #region Grilla detalle de los lotes
        [HttpPost]
        [GridAction]
        public virtual ActionResult getDetalle(long loteONumeriUnico)
        {
            int _cud = Convert.ToInt32(Session["CUD"].ToString());
            var grilla = _getDetalle(loteONumeriUnico, _cud);
            return View(new GridModel<grilla_DetalleLote>(grilla));
        }

        public IEnumerable<grilla_DetalleLote> _getDetalle(long loteONumeroUnico, int _cud)
        {
            UnidadDocumentalModel modeloUD = new UnidadDocumentalModel(db);
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_DetalleLote(loteONumeroUnico, _cud);
            List<grilla_DetalleLote> _grillaAlm = new List<grilla_DetalleLote>();

            foreach (spAlm_DetalleLote_Result item in grilla)
            {
                grilla_DetalleLote data = new grilla_DetalleLote();
                data._negID = item.NegId;
                data._paginas = item.Paginas;
                data._codBarras = item.CodBarras;
                data._gruDescripcion = item.GruDescripcion;

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult AlmacenarDocumento(int negID)
        {
            int _negID = negID;
            decimal _idusuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;

            //Guardar el negocio en la Unidad documental con busqueda de lote
            int posicion = obtenerUlitmaPosicionCUD(long.Parse(Session["CUD"].ToString()));
            if (posicion == 0)
                posicion = 1;
            else
                posicion = posicion + 1;

            GuardarCUD(posicion.ToString(), Session["_NoLote"].ToString(), Session["CUD"].ToString(), _negID);
            var grilla = _getDetalle(long.Parse(Session["_NoLote"].ToString()), Convert.ToInt32(Session["CUD"].ToString()));
            return View(new GridModel<grilla_DetalleLote>(grilla));
        }

        #endregion

        #region Grilla Documentos por Reglas

        [HttpPost]
        [GridAction]
        public virtual ActionResult getGrillaReglas(int ud)
        {
            //Obtener el nombre de la oficina.
            int _cliNit = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).CliNit);

            if (Session["CUD"] == null)
            {
                var grilla = _getGrillaReglas(ud, _cliNit);
                return View(new GridModel<grilla_ReglasDocumentos>(grilla));
            }
            else
            {
                var grilla = _getGrillaReglas(Convert.ToInt32(Session["CUD"].ToString()), _cliNit);
                return View(new GridModel<grilla_ReglasDocumentos>(grilla));
            }
        }

        public IEnumerable<grilla_ReglasDocumentos> _getGrillaReglas(int ud, int clienteNit)
        {
            UnidadDocumentalModel modelUD = new UnidadDocumentalModel(db);
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_GrillaReglas(ud, clienteNit);
            List<grilla_ReglasDocumentos> _grillaReglas = new List<grilla_ReglasDocumentos>();

            foreach (spAlm_GrillaReglas_Result item in grilla)
            {
                grilla_ReglasDocumentos data = new grilla_ReglasDocumentos();
                data._negID = Convert.ToInt32(item.NegId);
                data._paginas = Convert.ToInt32(item.Paginas);
                data._codBarras = item.CodBarras;
                data._producto = item.Producto;
                data._subProducto = item.SubProducto;
                data._campo1 = item.Campo1;
                data._campo2 = item.Campo2;
                data._campo3 = item.Campo3;

                _grillaReglas.Add(data);
            }
            return _grillaReglas;
        }

        [HttpPost]
        public void AlmacenarDocumentoReglas(string negID)
        {
            int[] _negID = negID.Split(',').Select(Int32.Parse).ToArray();

            decimal _idusuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;

            //Guardar el negocio en la Unidad documental con busqueda de lote
            int posicion = obtenerUlitmaPosicionCUD(long.Parse(Session["CUD"].ToString()));
            if (posicion == 0)
                posicion = 1;
            else
                posicion = posicion + 1;

            for (int i = 0; i < _negID.Count(); i++)
            {
                GuardarCUD(posicion.ToString(), "0", Session["CUD"].ToString(), _negID[i]);
            }
        }

        #endregion

        #region Grilla Contenido UD
        [HttpPost]
        [GridAction]
        public virtual ActionResult getContenidoUD(int ud)
        {
            var grilla = _getContenidoUD(ud);
            return View(new GridModel<grilla_contenidoUD>(grilla));
        }

        public IEnumerable<grilla_contenidoUD> _getContenidoUD(int ud)
        {
            UnidadDocumentalModel modeloUD = new UnidadDocumentalModel(db);
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_ConsultaContenidoUD(ud);
            List<grilla_contenidoUD> _grillaAlm = new List<grilla_contenidoUD>();

            foreach (spAlm_ConsultaContenidoUD_Result item in grilla)
            {
                grilla_contenidoUD data = new grilla_contenidoUD();
                data._cud = Convert.ToInt32(item.CUD);
                data._codBarras = item.CodBarras;
                data._negId = Convert.ToInt32(item.NegId);
                data._fecha = item.Fecha.ToString();

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }

        [HttpPost]
        public ActionResult EliminarDocumentoUD(int negID, int ud)
        {
            try
            {
                GestorDocumentalEnt dbo = new GestorDocumentalEnt();
                dbo.spAlm_EliminarDocumentoUD(negID);

                var grilla = _getContenidoUD(ud);
                return View(new GridModel<grilla_contenidoUD>(grilla));
            }
            catch (Exception es)
            {
                LogRepository.registro(es.Message);
                var grilla = _getContenidoUD(ud);
                return View(new GridModel<grilla_contenidoUD>(grilla));
            }
        }

        #endregion

        #region Funciones JSON
        [HttpGet]
        public JsonResult _GetCampos(int CodOrigen)
        {
            try
            {
                CamposController camposCon = new CamposController();
                var campos = camposCon.getCamposFormulario(CodOrigen);
                Session["listaCamposDinamicos"] = campos;
                return Json(campos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en  metodo _GetCampos " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene el codigo que tiene cada item de una lista
        /// </summary>
        private string CodigoDelItemLista(int codCampId)
        {
            try
            {
                return new GestorDocumental.Models.GestorDocumentalEnt().CodigosCampo.Where(c => c.CodCampId == codCampId).FirstOrDefault().CodId;
            }
            catch (Exception es)
            {
                return "";
            }
        }

        [HttpPost]
        public void GuardarCUD(string posicion, string lote, string numeroCUD, int neg_ID)
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();

            int _posicion = Convert.ToInt32(posicion);
            int _lote = Convert.ToInt32(lote);
            int _numeroCUD = Convert.ToInt32(numeroCUD);
            string idUsuario = Session["_IDUsuario_"].ToString();

            data.spAlm_GuardarContenidoCUD(_numeroCUD, _posicion, _lote, idUsuario, neg_ID);
        }

        #endregion

        //Metodo para obtener el nombre de la oficina para almacenar los documentos
        public string obtenerNombreOficina(decimal CliNit)
        {
            GestorDocumentalEnt db = new GestorDocumentalEnt();
            var query = (from a in db.P_Oficinas
                         where a.OFI_CodNit == CliNit
                         select a.OFI_Nombre).SingleOrDefault();
            return query.ToString();
        }

        //Obtiene la ultima posicion del la Unidad Documental
        public int obtenerUlitmaPosicionCUD(long cud)
        {
            GestorDocumentalEnt db = new GestorDocumentalEnt();

            int posicion = 0;
            var query = (from a in db.alm_ContenidoUD
                         where a.CUD == cud
                         select a.Posicion).Max();

            if (query == null)
                return 0;
            else
                return Convert.ToInt32(query.ToString());
        }

        [HttpGet]
        public JsonResult _validarCUD(int numeroCud)
        {
            try
            {
                UnidadDocumentalModel modeloUD = new UnidadDocumentalModel(db);
                int _cliNit = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).CliNit);

                int datos = modeloUD.validarCUD(numeroCud, _cliNit);
                return Json(datos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo para asignarle el estado a la Unidad documental como estado "liberada"
        [HttpPost]
        public void liberarCajaUD(string numeroCUD)
        {
            int usuario = Convert.ToInt32(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            int _numeroCUD = Convert.ToInt32(numeroCUD);

            db.spAlm_LiberarCUD(_numeroCUD, usuario);
        }

        //Metodo para asignarle el estado a la Unidad documental como estado "cerrada"
        [HttpPost]
        public void cerrarCajaUD(string numeroCUD)
        {
            int usuario = Convert.ToInt32(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            int _numeroCUD = Convert.ToInt32(numeroCUD);
            db.spAlm_CerrarCUD(_numeroCUD, usuario);
        }

        //Obtiene el nombre de los filtros parametrizados para mostrarlos en la grilla y en el campo filtro
        public void obtenerFiltros()
        {
            List<spAlm_ObtenerNombreFiltro_Result> lst = db.spAlm_ObtenerNombreFiltro().ToList();
            ViewData["CAMPO_1"] = lst[0].Valor.ToString();
            ViewData["CAMPO_2"] = lst[1].Valor.ToString();
            ViewData["CAMPO_3"] = lst[2].Valor.ToString();
        }
    }

    //Datos de la grilla de Detalle de los Lotes
    public class grilla_DetalleLote
    {
        public decimal _negID { get; set; }
        public int _paginas { get; set; }
        public string _codBarras { get; set; }
        public string _gruDescripcion { get; set; }
    }

    //Estructura de la grilla de Almacenar los documentos buscados por Lote
    public class grilla_AlmacenarLote
    {
        public long _id { get; set; }
        public long? _subgrupo { get; set; }
        public decimal? _principales { get; set; }
        public string _fechaRecepcion { get; set; }
        public long _numeroLote { get; set; }
        public string _fechaCargue { get; set; }
        public string _tipo { get; set; }
    }

    //Estructua de la grilla del contenido de la Unidad documental
    public class grilla_contenidoUD
    {
        public int _cud { get; set; }
        public int _negId { get; set; }
        public string _codBarras { get; set; }
        public string _fecha { get; set; }
    }

    //Estructura de la grilla de las reglas de los documentos listos para almacenar
    public class grilla_ReglasDocumentos
    {
        public int _negID { get; set; }
        public int _paginas { get; set; }
        public string _codBarras { get; set; }
        public string _producto { get; set; }
        public string _subProducto { get; set; }
        public string _campo1 { get; set; }
        public string _campo2 { get; set; }
        public string _campo3 { get; set; }
    }
}
