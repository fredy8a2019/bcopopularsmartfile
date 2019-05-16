using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using GestorDocumental.Models;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;

namespace GestorDocumental.Controllers
{
    public class SolicitudController : Controller
    {
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();

        public ActionResult Index()
        {
            generarCampos();
            obtenerFiltros();
            Session["_NoTiquet"] = "";
            return View();
        }

        //Interfaz que muetra la confirmación de la solicitud y el número de Tiquet
        public ActionResult Confirmar()
        {
            return View();
        }

        #region Grilla Solicitud de Documentos
        [HttpPost]
        [GridAction]
        public virtual ActionResult getInformacionSolicitud(string filtro)
        {
            //Obtener la informacion de la solicitud del documento para pintarla en la grilla
            var grilla = _getInformacionSolicitud(filtro);
            return View(new GridModel<grilla_SolicitudDocum>(grilla));
        }

        public IEnumerable<grilla_SolicitudDocum> _getInformacionSolicitud(string filtro)
        {
            //Metodo que ejecuta y trae la informacion de los documentos que son solicitados
            var grilla = dbo.spAlm_GrillaSolicitudDocumentos(filtro);
            List<grilla_SolicitudDocum> _grillaAlm = new List<grilla_SolicitudDocum>();

            foreach (spAlm_GrillaSolicitudDocumentos_Result item in grilla)
            {
                grilla_SolicitudDocum data = new grilla_SolicitudDocum();
                data._negId = item.NegId.ToString();
                data._producto = item.Producto;
                data._subProducto = item.SubProducto;
                data._codBarras = item.CodBarras;
                data._campoUno = item.Campo1;
                data._campoDos = item.Campo2;
                data._campoTres = item.Campo3;

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }
        #endregion

        [HttpPost]
        public void generarSolicitud(FormCollection collection)
        {
            string _docSolicitado = collection[1].ToString();
            int _idUsuario = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).IdUsuario);
            decimal _negId = Convert.ToDecimal(collection[0].ToString());

            decimal noTiquet = Convert.ToDecimal(dbo.spAlm_CrearSolicitudPrestamo(_docSolicitado, _idUsuario, _negId).SingleOrDefault().ToString());
            long _CUD = long.Parse(dbo.spAlm_ObtenerCUD(_negId).SingleOrDefault().ToString());


            //Obtener el valor maximo de la captura
            int numCaptura = (from a in dbo.alm_CapturaUD
                              where a.CUD == _CUD
                              select a.NumCaptura).Max();

            //Se guarda la captura del CUD seleccionado
            for (int i = 2; i < collection.Count; i++)
            {
                alm_CapturaUD data = new alm_CapturaUD();
                data.CUD = _CUD;
                data.NumCaptura = numCaptura + 1;
                data.CampId = Convert.ToInt32(collection.Keys[i].ToString());
                data.Valor = collection[i].ToString();
                data.Usuario = _idUsuario;
                data.Fecha = DateTime.Now;

                dbo.alm_CapturaUD.AddObject(data);
                dbo.SaveChanges();
            }
            Response.Redirect("/Solicitud/Confirmar?noTiquet=" + noTiquet);
        }

        //Obtiene el nombre de los filtros parametrizados para mostrarlos en la grilla y en el campo filtro
        public void obtenerFiltros()
        {
            List<spAlm_ObtenerNombreFiltro_Result> lst = dbo.spAlm_ObtenerNombreFiltro().ToList();
            ViewData["CAMPO_1"] = lst[0].Valor.ToString();
            ViewData["CAMPO_2"] = lst[1].Valor.ToString();
            ViewData["CAMPO_3"] = lst[2].Valor.ToString();
        }

        //Genera los campos para el formulario de solicitud de documentos
        public void generarCampos()
        {
            CrearFormulariosCaptura formularioEnvio = new CrearFormulariosCaptura();
            List<Campos> lstCampos = obtenerCamposEnvio();
            Table ta = new Table();

            string campos = formularioEnvio.GenerarCampos(ta, lstCampos, null, 94, 0, "0", 0, 0);
            campos = campos.Replace('"', '\'');
            ViewData["_camposEnvio"] = campos;
        }

        public List<Campos> obtenerCamposEnvio()
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            var query = (from a in dbo.Campos
                         where a.CodFormulario == 94
                         select a);
            return query.ToList();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public void solicitudPrestamoMasivo(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                decimal _noTiquet = 0;
                if (files != null)
                {
                    string fileName;
                    string filePath;
                    string fileExtension;

                    foreach (var f in files)
                    {
                        //Set File Detalis
                        SetFileDetails(f, out fileName, out filePath, out fileExtension);

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            string savedExcelFiles = @"~/App_Data/Prestamos/" + fileName;
                            f.SaveAs(Server.MapPath(savedExcelFiles));

                            //Obtener la estructura del archivo de Excel.
                            DataSet data = ReadDataFromExcelFiles(savedExcelFiles);
                            List<datosPrestamoMasivo> lstPrestamoMasivo = extraerValoresPrestamoMasivo(data);

                            //Obtener Todos los negocios que estan cumpliendo las reglas de prestamos
                            GestorDocumentalEnt db = new GestorDocumentalEnt();

                            List<datosPrestamosProbados> lstPrestamosAprobados = new List<datosPrestamosProbados>();
                            foreach (var item in lstPrestamoMasivo)
                            {
                                spAlm_GrillaSolicitudDocumentos_Result datos = db.spAlm_GrillaSolicitudDocumentos(item._noDocumento).SingleOrDefault();
                                datosPrestamosProbados dao = new datosPrestamosProbados();

                                if (datos != null)
                                {
                                    dao._documentoSolicitado = datos.Campo1.ToString();
                                    dao._negId = Convert.ToInt32(datos.NegId);
                                    lstPrestamosAprobados.Add(dao);
                                }
                            }

                            int _idUsuario = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).IdUsuario);
                            int _cantidadDocumentos = lstPrestamosAprobados.Count;

                            decimal noTiquet = Convert.ToDecimal(db.spAlm_CrearSolicitudPrestamoMasivo("", 0, _idUsuario, _cantidadDocumentos).SingleOrDefault().ToString());
                            foreach (var item in lstPrestamosAprobados)
                            {
                                //Insertar en la tabla detalle de solicitudes.
                                dbo.spAlm_CrearSolicitudPrestamoMasivoDetalle(Convert.ToInt32(noTiquet), 
                                    Convert.ToDecimal(item._negId), 
                                    item._documentoSolicitado,
                                    Convert.ToDecimal(_idUsuario));
                            }
                            _noTiquet = noTiquet;
                        }
                    }
                    Session["_NoTiquet"] = _noTiquet;
                }
            }
            catch (Exception)
            {
                throw;
            }
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
            var adapter = new OleDbDataAdapter("SELECT * FROM [Prestamos$]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Extrae los datos necesarios del archivo de Excel de Contabilizados.
        /// </summary>
        /// <param name="Exceldata"></param>
        /// <returns></returns>
        private List<datosPrestamoMasivo> extraerValoresPrestamoMasivo(DataSet Exceldata)
        {
            List<datosPrestamoMasivo> lstPrestamoMasivo = new List<datosPrestamoMasivo>();
            for (int i = 0; i < Exceldata.Tables[0].Rows.Count; i++)
            {
                datosPrestamoMasivo dao = new datosPrestamoMasivo();
                dao._noDocumento = Exceldata.Tables[0].Rows[i][0].ToString();
                lstPrestamoMasivo.Add(dao);
            }

            return lstPrestamoMasivo;
        }

        /// <summary>
        /// Obtiene el total de registros en el archivo subido y lo guarda en una variable de Sesion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult obtenernoTiquet()
        {
            string totalRegistros = Session["_NoTiquet"].ToString();
            return Json(totalRegistros, JsonRequestBehavior.AllowGet);
        }
    }

    //Datos de la grilla de solicitud de Documentos
    public class grilla_SolicitudDocum
    {
        public string _negId { get; set; }
        public string _producto { get; set; }
        public string _subProducto { get; set; }
        public string _codBarras { get; set; }
        public string _campoUno { get; set; }
        public string _campoDos { get; set; }
        public string _campoTres { get; set; }
    }

    public class datosPrestamoMasivo
    {
        public string _noDocumento { get; set; }
    }

    public class datosPrestamosProbados
    {
        public string _documentoSolicitado { get; set; }
        public int _negId { get; set; }
    }
}
