using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class ContabilizacionMasivaController : Controller
    {
        //
        // GET: /ContabilizacionMasiva/
        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
                Session["TITULO"] = "Contabilización masiva";

                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        /// <summary>
        /// Guarda la informacion adicional de la contabilizacion masiva
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GuardarContabilizacion(FormCollection collection)
        {
            HistoricoContabilizacionMasiva data = new HistoricoContabilizacionMasiva();
            data.TipoContabilizacion = collection["tipoContabilizacion"].ToString();
            data.NoDocumentosProcesados = collection["txtNoDocumentos"].ToString();
            data.Observaciones = collection["txtObservaciones"].ToString();
            data.Fecha = Convert.ToDateTime(collection["txtFecha"].ToString());
            data.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;

            GestorDocumentalEnt dao = new GestorDocumentalEnt();
            dao.spGuardarContabilizacion(data.TipoContabilizacion, data.NoDocumentosProcesados, data.Observaciones, data.Fecha, data.Usuario);
            
            return View("Index");
        }

        /// <summary>
        /// Obtiene el total de registros en el archivo subido y lo guarda en una variable de Sesion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult obtenerTotalArchivos()
        {
            string totalRegistros = Session["contadorRegistros"].ToString();
            return Json(totalRegistros, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// El proceso en general que procesa el archivo de excel subido y guardado en un dataset para guardarlo
        /// </summary>
        /// <param name="files"></param>
        /// <param name="tContabilizacion"></param>
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public void CrearContabilizacionMasiva(IEnumerable<HttpPostedFileBase> files, string tContabilizacion)
        {
            try
            {
                if (files != null)
                {
                    string fileName;
                    string filePath;
                    string fileExtension;

                    foreach (var f in files)
                    {
                        //Set the file details
                        SetFileDetails(f, out fileName, out filePath, out fileExtension);

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            string savedExcelFiles = @"~/App_Data/Contabilizacion/" + fileName;
                            f.SaveAs(Server.MapPath(savedExcelFiles));

                            //Obtener la estructura del archivo de Excel.
                            DataSet data = ReadDataFromExcelFiles(savedExcelFiles);

                            switch (tContabilizacion)
                            {
                                case "contabilizados":
                                    //Extraer la informacion necesaria para el archivo Contabilizado
                                    List<UploadExcelContabilizados> lstContabilizados = extraerValoresContabilizados(data);
                                    Session["contadorRegistros"] = lstContabilizados.Count.ToString();

                                    //Guarda los datos de la contabilizacion en las Capturas
                                    _guardarContabilizacion(lstContabilizados);
                                    break;

                                case "rechazados":
                                    //Extraer la informacion necesaria para el archivo Contabilizado
                                    List<UploadExcelRechazados> lstRechazados = extraerValoresRechazados(data);
                                    Session["contadorRegistros"] = lstRechazados.Count.ToString();

                                    //Guarda los datos de la contabilizacion en las Capturas
                                    _guardarContabilizacionRechazados(lstRechazados);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Contabilizacion Masiva metodo guardarContabilizacion " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        /// <summary>
        /// Guarda los datos de una contabilizacion exitosa en la tabla de Captura
        /// </summary>
        /// <param name="lstContabilizados"></param>
        public void _guardarContabilizacion(List<UploadExcelContabilizados> lstContabilizados)
        {
            CapturaController capCon = new CapturaController();
            AsignacionTareasModel asigTarMod = new AsignacionTareasModel();

            var lstCampos = obtenerCampos(0);
            int countCampos = obtenerCamposLongitud(0);

            foreach (UploadExcelContabilizados item in lstContabilizados)
            {
                List<Captura> listCaptura = new List<Captura>();
                //Se valida si ya existe una Asignacion de tarea para el negocio
                //de ser asi no se realiza el proceso de Contabilizacion por segunda ves
                int _negId = obtenerNegId(item._textoCabDocumento);
                bool resultadoValidacion = _validarAsignacionTareas(_negId, item._claseDeDocumento, item._textoCabDocumento);

                if (resultadoValidacion == false)
                {
                    AsignacionTareas nuevaAT = new AsignacionTareas();
                    nuevaAT.NegId = obtenerNegId(item._textoCabDocumento);
                    nuevaAT.IdEtapa = 120;
                    nuevaAT.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                    nuevaAT.HoraInicio = DateTime.Now;
                    nuevaAT.HoraTerminacion = DateTime.Now.AddMinutes(5);
                    nuevaAT.IdEstado = 140;
                    //Inserto la asignacion de tareas
                    asigTarMod.Add(nuevaAT);

                    for (int i = 0; i < countCampos; i++)
                    {
                        Captura nueva = new Captura();
                        nueva.NegId = obtenerNegId(item._textoCabDocumento);
                        nueva.NumCaptura = 4;

                        string _campDescripcion = lstCampos[i].CampId.ToString();
                        switch (_campDescripcion)
                        {
                            case "1161":
                                nueva.CampId = Convert.ToInt32(lstCampos[i].CampId.ToString());
                                nueva.NegValor = item._noDocumento;
                                break;

                            case "1162":
                                nueva.CampId = Convert.ToInt32(lstCampos[i].CampId.ToString());
                                nueva.NegValor = "Contabilizado";
                                break;

                            case "1163":
                                nueva.CampId = Convert.ToInt32(lstCampos[i].CampId.ToString());
                                nueva.NegValor = "";
                                break;

                            case "2192":
                                nueva.CampId = Convert.ToInt32(lstCampos[i].CampId.ToString());
                                nueva.NegValor = "4048";
                                break;
                        }

                        nueva.Indice = 0;
                        nueva.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        nueva.FechaRegistro = DateTime.Now;
                        nueva.DocId = 0;
                        nueva.NegIdBizagi = null;

                        listCaptura.Add(nueva);
                    }

                    //Inserto la lista de captura
                    capCon.InsertarCaptura(listCaptura);
                }
            }
        }

        /// <summary>
        /// Guarda los datos de una contabilizacion rechazada en la tabla de captura
        /// </summary>
        /// <param name="lstRechazados"></param>
        public void _guardarContabilizacionRechazados(List<UploadExcelRechazados> lstRechazados)
        {
            CapturaController capCon = new CapturaController();
            AsignacionTareasModel asigTarMod = new AsignacionTareasModel();
            var lstCampos = obtenerCampos(0);
            int countCampos = obtenerCamposLongitud(0);

            List<Captura> listCaptura = new List<Captura>();
            foreach (UploadExcelRechazados item in lstRechazados)
            {
                //Se valida si ya existe una Asignacion de tarea para el negocio
                //de ser asi no se realiza el proceso de Contabilizacion por segunda ves
                int _negId = obtenerNegId(item._identificadorPrefactura);
                bool resultadoValidacion = _validarAsignacionTareas(_negId, item._claseDeDocumento, item._identificadorPrefactura);

                if (resultadoValidacion == false)
                {
                    AsignacionTareas nuevaAT = new AsignacionTareas();
                    nuevaAT.NegId = obtenerNegId(item._identificadorPrefactura);
                    nuevaAT.IdEtapa = 120;
                    nuevaAT.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                    nuevaAT.HoraInicio = DateTime.Now;
                    nuevaAT.HoraTerminacion = DateTime.Now.AddMinutes(5);
                    nuevaAT.IdEstado = 142;
                    //Inserto la asignacion de tareas
                    asigTarMod.Add(nuevaAT);

                    for (int i = 0; i < countCampos; i++)
                    {
                        Captura nueva = new Captura();
                        nueva.NegId = obtenerNegId(item._identificadorPrefactura);
                        nueva.NumCaptura = 4;

                        string _campDescripcion = lstCampos[i].CampId.ToString();
                        switch (_campDescripcion)
                        {
                            case "1161":
                                nueva.CampId = Convert.ToInt32(lstCampos[i].CampId.ToString());
                                nueva.NegValor = "";
                                break;

                            case "1162":
                                nueva.CampId = Convert.ToInt32(lstCampos[i].CampId.ToString());
                                nueva.NegValor = "Rechazado";
                                break;

                            case "1163":
                                nueva.CampId = Convert.ToInt32(lstCampos[i].CampId.ToString());
                                nueva.NegValor = item._textoMensajeObservaciones;
                                break;

                            case "2192":
                                nueva.CampId = Convert.ToInt32(lstCampos[i].CampId.ToString());
                                nueva.NegValor = "";
                                break;
                        }

                        nueva.Indice = 0;
                        nueva.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        nueva.FechaRegistro = DateTime.Now;
                        nueva.DocId = 0;
                        nueva.NegIdBizagi = null;

                        listCaptura.Add(nueva);
                    }

                    //Inserto la lista de captura
                    capCon.InsertarCaptura(listCaptura);
                    listCaptura.Clear();
                }
            }
        }

        /// <summary>
        /// Validar que el negocio ingresado no tenga una asignacion de tareas de Contabilizacion creada
        /// </summary>
        public bool _validarAsignacionTareas(int negId, string claseDocumento, string codBarras)
        {
            //Se valida que ya exista la etapa de Contabilizacion de ser asi no procesar el archivo
            GestorDocumentalEnt dao = new GestorDocumentalEnt();
            var query = (from a in dao.AsignacionTareas
                         where a.IdEtapa == 120 && a.NegId == negId
                         select a).ToList();

            //Se valida si el documento esta anulado de ser asi no deberia de hacer la contabilizacion
            var queryAnulacion = (from a in dao.AsignacionTareas
                                  where a.IdEtapa == 130 && a.NegId == negId
                                  select a).ToList();

            //Se valida con el codigo de barras que no tenga anulado el codigo de barras
            var queryRadicacion = (from a in dao.Radicacion
                                   where a.CodBarras == codBarras && a.Estado == 150
                                   select a).ToList();

            //Se valida que el negocio ya haya pasado por la etapa de control de calidad
            var queryCCalidad = (from a in dao.AsignacionTareas
                                 where a.IdEtapa == 50 && a.IdEstado == 30 && a.NegId == negId
                                 select a).ToList();

            //Se valida si los documentos procesados son facturas A1 y A3 se valida que ya tenga la etapa de transmision terminada
            if (claseDocumento.Equals("A1") || claseDocumento.Equals("A3"))
            {
                var queryFacturas = (from a in dao.AsignacionTareas
                                     where a.IdEtapa == 60 && a.IdEstado == 30 && a.NegId == negId
                                     select a).ToList();

                if (query.Count > 0 || queryAnulacion.Count > 0 || queryRadicacion.Count > 0)
                    return true;
                else if(queryFacturas.Count > 0)
                    return false;
            }

            if (query.Count > 0 || queryAnulacion.Count > 0 || queryRadicacion.Count > 0)
                return true;
            else if (queryCCalidad.Count > 0)
                return false;
            else {
                return true;
            }
        }

        /// <summary>
        /// Extrae la informacion del libro de Excel subido y la guarda en un dataSet para ser procesado despues
        /// </summary>
        /// <param name="savedExcelFiles"></param>
        /// <returns></returns>
        private DataSet ReadDataFromExcelFiles(string savedExcelFiles)
        {
            //Crear la conexion
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", Server.MapPath(savedExcelFiles));

            //Llenar el DataSet con las hojas del libro de excel
            var adapter = new OleDbDataAdapter("SELECT * FROM [Contabilizados$]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Extrae los datos necesarios del archivo de Excel de Contabilizados.
        /// </summary>
        /// <param name="Exceldata"></param>
        /// <returns></returns>
        private List<UploadExcelContabilizados> extraerValoresContabilizados(DataSet Exceldata)
        {
            List<UploadExcelContabilizados> lstUContabilizados = new List<UploadExcelContabilizados>();
            for (int i = 0; i < Exceldata.Tables[0].Rows.Count; i++)
            {
                UploadExcelContabilizados dao = new UploadExcelContabilizados();
                dao._noDocumento = Exceldata.Tables[0].Rows[i][1].ToString();
                dao._claseDeDocumento = Exceldata.Tables[0].Rows[i][5].ToString();
                dao._textoCabDocumento = Exceldata.Tables[0].Rows[i][9].ToString();
                lstUContabilizados.Add(dao);
            }

            return lstUContabilizados;
        }

        /// <summary>
        /// Extrae los datos necesarios del archivo de Excel de los Rechazados
        /// </summary>
        /// <param name="Exceldata"></param>
        /// <returns></returns>
        private List<UploadExcelRechazados> extraerValoresRechazados(DataSet Exceldata)
        {
            List<UploadExcelRechazados> lstURechazados = new List<UploadExcelRechazados>();
            for (int i = 0; i < Exceldata.Tables[0].Rows.Count; i++)
            {
                UploadExcelRechazados dao = new UploadExcelRechazados();
                dao._identificadorPrefactura = Exceldata.Tables[0].Rows[i][0].ToString();
                dao._textoMensaje = Exceldata.Tables[0].Rows[i][4].ToString();
                dao._claseDeDocumento = Exceldata.Tables[0].Rows[i][6].ToString();
                dao._textoMensajeObservaciones = Exceldata.Tables[0].Rows[i][39].ToString();
                lstURechazados.Add(dao);
            }

            return lstURechazados;
        }

        /// <summary>
        /// Extraer el valor del NegId dependiendo del codigo de barras que se envie
        /// </summary>
        /// <param name="codBarras"></param>
        /// <returns></returns>
        public int obtenerNegId(string codBarras)
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();
            var negId = data.CargueLotes.Where(x => x.CodBarras == codBarras).Max(p => p.NegId);

            return Convert.ToInt32(negId);
        }

        /// <summary>
        /// Obtiene los campos que se van a guardar junto a la contabilizacion
        /// </summary>
        /// <param name="CodOrigen"></param>
        /// <returns></returns>
        public dynamic obtenerCampos(int CodOrigen)
        {
            GestorDocumentalEnt gd = new GestorDocumentalEnt();
            int idFormulario = Convert.ToInt32(gd.Formularios.Where(f => f.CodOrigen == CodOrigen).Select(f => f.IdFormularios).FirstOrDefault());

            return gd.Campos.Join(gd.TiposCampo,
                   camp => camp.TcId,
                   tipCamp => tipCamp.TcId,
                   (camp, tipCamp) => new { Camp = camp, TipCamp = tipCamp })
                   .Where(x => x.Camp.CodFormulario == idFormulario)
                   .OrderBy(x => x.Camp.CampOrden)
                   .Select(x => new
                   {
                       x.Camp.CampId,
                       x.Camp.CampDescripcion,
                       x.Camp.CampAlto,
                       x.Camp.CampAncho,
                       x.Camp.CampObligatorio,
                       x.Camp.TcId,
                       x.Camp.LongMax,
                       x.Camp.CampOrden,
                       x.Camp.idPadre,
                       x.Camp.CodFormulario
                   }).ToList();
        }

        /// <summary>
        /// Obtiene la longitud de los campos consultados para la contabilizacion
        /// </summary>
        /// <param name="CodOrigen"></param>
        /// <returns></returns>
        public int obtenerCamposLongitud(int CodOrigen)
        {
            GestorDocumentalEnt gd = new GestorDocumentalEnt();
            int idFormulario = Convert.ToInt32(gd.Formularios.Where(f => f.CodOrigen == CodOrigen).Select(f => f.IdFormularios).FirstOrDefault());

            return gd.Campos.Join(gd.TiposCampo,
                   camp => camp.TcId,
                   tipCamp => tipCamp.TcId,
                   (camp, tipCamp) => new { Camp = camp, TipCamp = tipCamp })
                   .Where(x => x.Camp.CodFormulario == idFormulario)
                   .OrderBy(x => x.Camp.CampOrden)
                   .Select(x => new
                   {
                       x.Camp.CampId,
                       x.Camp.CampDescripcion,
                       x.Camp.CampAlto,
                       x.Camp.CampAncho,
                       x.Camp.CampObligatorio,
                       x.Camp.TcId,
                       x.Camp.LongMax,
                       x.Camp.CampOrden,
                       x.Camp.idPadre,
                       x.Camp.CodFormulario
                   }).Count();
        }

        /// <summary>
        /// Setea los valores por defecto del archivo
        /// </summary>
        /// <param name="f"></param>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileExtension"></param>
        public static void SetFileDetails(HttpPostedFileBase f, out string fileName, out string filePath, out string fileExtension)
        {
            fileName = Path.GetFileName(f.FileName);
            fileExtension = Path.GetExtension(f.FileName);
            filePath = Path.GetFullPath(f.FileName);
        }
    }

    #region Clases parametricas para los archivos de Excel
    public class UploadExcelContabilizados
    {
        public string _noDocumento { get; set; }
        public string _textoCabDocumento { get; set; }
        public string _claseDeDocumento { get; set; }
    }

    public class UploadExcelRechazados
    {
        public string _identificadorPrefactura { get; set; }
        public string _textoMensaje { get; set; }
        public string _textoMensajeObservaciones { get; set; }
        public string _claseDeDocumento { get; set; }
    }
    #endregion
}
