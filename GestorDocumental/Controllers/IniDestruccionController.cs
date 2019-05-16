using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;
using System.Web.UI;
using System.IO;
using System.Data;
using System.Data.OleDb;

namespace GestorDocumental.Controllers
{
    public class IniDestruccionController : Controller
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        public ActionResult IniDestruccion()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Crear Archivo";
                ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
                ViewData["_codActa"] = _ObtNroActa();
                Session["_cantArchivos"] = "";

                obtenerFiltros();
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        #region Grilla Lista de documentos para destruccion
        [HttpPost]
        [GridAction]
        public virtual ActionResult grillaListDestruc()
        {
            var grillaList = _grillaListDestruc();
            return View(new GridModel<grilla_ListDestruc>(grillaList));
        }

        public IEnumerable<grilla_ListDestruc> _grillaListDestruc()
        {
            decimal _cliNit = ((Usuarios)this.Session["USUARIO"]).CliNit;

            var grillaList = db.spAlm_ListDestruc(_cliNit);

            List<grilla_ListDestruc> _grillaListDestruc = new List<grilla_ListDestruc>();

            foreach (spAlm_ListDestruc_Result item in grillaList)
            {
                grilla_ListDestruc data = new grilla_ListDestruc();
                data._negId = Convert.ToInt32(item.NegId);
                data._producto = item.Producto;
                data._subProducto = item.SubProducto;
                data._codBarras = item.CodBarras;
                data._campo1 = item.Campo1;
                data._campo2 = item.Campo2;
                data._campo3 = item.Campo3;

                _grillaListDestruc.Add(data);
            }
            return _grillaListDestruc;
        }
        #endregion

        #region Grilla contenido del acta de destruccion
        [HttpPost]
        [GridAction]
        public virtual ActionResult grillaContenidoActa(int _codActa)
        {
            var grillaList = _grillaContenidoActa(_codActa);
            return View(new GridModel<grilla_ContenidoActa>(grillaList));
        }

        public IEnumerable<grilla_ContenidoActa> _grillaContenidoActa(int _codActa)
        {
            var grillaContActa = db.spAlm_ConsulContenidoActa(_codActa);
            List<grilla_ContenidoActa> _grillaCont = new List<grilla_ContenidoActa>();

            foreach (spAlm_ConsulContenidoActa_Result item in grillaContActa)
            {
                grilla_ContenidoActa data = new grilla_ContenidoActa();
                data._negID = item.cod_NegId;
                data._fecCreacion = Convert.ToString(item.fec_acta_creacion);

                _grillaCont.Add(data);
            }
            return _grillaCont;
        }
        #endregion

        [HttpPost]
        public void AgregarAlActaDestruc(int nroActa, string negID)
        {
            int[] _negID = negID.Split(',').Select(Int32.Parse).ToArray();

            decimal _idusuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
            decimal _cliNit = ((Usuarios)this.Session["USUARIO"]).CliNit;

            for (int i = 0; i < _negID.Count(); i++)
            {
                GuardaACTA(nroActa, negID, _idusuario, _cliNit);
            }
        }

        [HttpPost]
        public void GuardaACTA(int nroActa, string negID, decimal idUsuario, decimal codOficina)
        {
            int _nroActa = nroActa;
            string[] _negID = negID.Split(',');
            string _idUsuario = Convert.ToString(idUsuario);
            int _codOficina = Convert.ToInt32(codOficina);

            for (int i = 0; i < _negID.Length; i++)
            {
                db.spAlm_InsertActaDestruc(_nroActa, Convert.ToInt32(_negID[i].ToString()), _idUsuario, _codOficina);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public void EliminaDocActa(int _negID)
        {
            db.spAlm_EliminaDocActa(_negID);
        }

        [HttpGet]
        public JsonResult cantActas()
        {
            string cant = Session["_cantArchivos"].ToString();
            return Json(cant, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtNroActa()
        {
            var codActa = db.spAlm_ConsecutivoActa().First();
            return Json(codActa, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GrillaPorDoc(int _codProceso, string _codDocumento, int _nroActa)
        {
            decimal _idUsuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
            decimal _cliNit = ((Usuarios)this.Session["USUARIO"]).CliNit;
            var _grillaActa = db.spAlm_DocsDestruc(_codProceso, _codDocumento, _nroActa, Convert.ToString(_idUsuario), Convert.ToInt32(_cliNit));

            return Json(_grillaActa, JsonRequestBehavior.AllowGet);
        }

        /// El proceso en general que procesa el archivo de excel subido y guardado en un dataset para guardarlo
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public void CrearDestruccionMasiva(IEnumerable<HttpPostedFileBase> files, int _nroActa)
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
                            string savedExcelFiles = @"~/App_Data/Destruccion/" + fileName;
                            f.SaveAs(Server.MapPath(savedExcelFiles));

                            //Obtener la estructura del archivo de Excel.
                            DataSet data = ReadDataFromExcelFiles(savedExcelFiles);

                            //Extraer la informacion necesaria para el archivo Contabilizado
                            List<uploadExcelDestruccion> lstDestruidos = extraerValoresDestruidos(data);
                            Session["contadorRegistros"] = lstDestruidos.Count.ToString();

                            //Guarda los datos de la destruccion
                            _guardarDestruccion(lstDestruidos, _nroActa);

                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Destruccion Masiva metodo 'CrearDestruccionMasiva' " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        public void SetFileDetails(HttpPostedFileBase f, out string fileName, out string filePath, out string fileExtension)
        {
            fileName = Path.GetFileName(f.FileName);
            fileExtension = Path.GetExtension(f.FileName);
            filePath = Path.GetFullPath(f.FileName);
        }

        private DataSet ReadDataFromExcelFiles(string savedExcelFiles)
        {
            //Crear la conexion
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", Server.MapPath(savedExcelFiles));

            //Llenar el DataSet con las hojas del libro de excel
            var adapter = new OleDbDataAdapter("SELECT * FROM [jfp$]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds);
            return ds;
        }

        private List<uploadExcelDestruccion> extraerValoresDestruidos(DataSet Exceldata)
        {
            List<uploadExcelDestruccion> lstDestruidos = new List<uploadExcelDestruccion>();
            for (int i = 0; i < Exceldata.Tables[0].Rows.Count; i++)
            {
                uploadExcelDestruccion dao = new uploadExcelDestruccion();
                dao._negID = Exceldata.Tables[0].Rows[i][0].ToString();
                lstDestruidos.Add(dao);
            }

            return lstDestruidos;
        }

        public void _guardarDestruccion(List<uploadExcelDestruccion> lstDestruidos, int nroActa)
        {
            decimal _idusuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario; //@idUsuario
            decimal _cliNit = ((Usuarios)this.Session["USUARIO"]).CliNit; //@codOficina
            int cantArchivos = 0;

            foreach (var item in lstDestruidos)
            {
                var negID = db.spAlm_ListDestrucMasivo(Convert.ToDecimal(item._negID), _cliNit).SingleOrDefault();

                if (negID != null)
                {
                    db.spAlm_InsertActaDestruc(nroActa, Convert.ToInt32(item._negID), Convert.ToString(_idusuario), Convert.ToInt32(_cliNit));
                    cantArchivos++;
                }
            }
            Session["_cantArchivos"] = cantArchivos;
        }

        //Obtiene el nombre de los filtros parametrizados para mostrarlos en la grilla y en el campo filtro
        public void obtenerFiltros()
        {
            List<spAlm_ObtenerNombreFiltro_Result> lst = db.spAlm_ObtenerNombreFiltro().ToList();
            ViewData["CAMPO_1"] = lst[0].Valor.ToString();
            ViewData["CAMPO_2"] = lst[1].Valor.ToString();
            ViewData["CAMPO_3"] = lst[2].Valor.ToString();
        }

        public void CreaActaDestruc(int _nroActa)
        {
            db.spAlm_CreaActaDestruc(_nroActa);
        }

        public int _ObtNroActa()
        {
            var codActa = db.spAlm_ConsecutivoActa().First();
            return Convert.ToInt32(codActa);
        }
    }

    public class grilla_AgregaPorDoc
    {
        public int _result { get; set; }
    }

    public class grilla_ContenidoActa
    {
        public int _negID { get; set; }
        public string _fecCreacion { get; set; }
    }

    public class grilla_ListDestruc
    {
        public int _negId { get; set; }
        public string _codBarras { get; set; }
        public string _producto { get; set; }
        public string _subProducto { get; set; }
        public string _campo1 { get; set; }
        public string _campo2 { get; set; }
        public string _campo3 { get; set; }
    }

    public class uploadExcelDestruccion
    {
        public string _negID { get; set; }
    }
}
