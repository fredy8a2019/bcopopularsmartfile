using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class ArchivoController : Controller
    {

        GestorDocumentalEnt db = new GestorDocumentalEnt();

        //Codigo que se ejecuta cuando la pagina carga por primera vez
        public ActionResult Archivo()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Archivar Unidad documental";
                ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        //Confirmacion que el proceso de archivo fue exitoso
        public ActionResult Confirmar()
        {
            return View();
        }

        //Vallida que la unidad documental se encuentra disponible para realizar el archivo
        [HttpPost]
        public JsonResult validaUD(int _codUD)
        {
            int resultado = Convert.ToInt32(db.spAlm_ValidaUD(_codUD).SingleOrDefault().ToString());
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #region Grilla de las Unidades documentales consultadas
        [HttpPost]
        [GridAction]
        public virtual ActionResult getConsulCUDArchivo(int _cud)
        {
            var grillaArchivo = _getConsulCUDArchivo(_cud);
            return View(new GridModel<grilla_ConsultaCUD>(grillaArchivo));
        }

        public IEnumerable<grilla_ConsultaCUD> _getConsulCUDArchivo(int cud)
        {

            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            try
            {
                var grillaCArch = db.spAlm_ConsCUDArchivo(cud, _idUsuarioProc);
                List<grilla_ConsultaCUD> _grillaCUDArch = new List<grilla_ConsultaCUD>();
                foreach (spAlm_ConsCUDArchivo_Result item in grillaCArch)
                {
                    grilla_ConsultaCUD data = new grilla_ConsultaCUD();
                    data._cud = item.CUD;
                    data._estado = item.Valor;
                    data._cliente = item.CliNombre;
                    data._oficina = item.OFI_Nombre;
                    data._fecTermina = item.HoraTerminacion.ToString();
                    data._nomUsuario = item.NomUsuario;

                    _grillaCUDArch.Add(data);
                }
                return _grillaCUDArch;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        //Inserta el estado del archivo y devuelve el resultado en un entero
        [HttpGet]
        public JsonResult InsertEstadoArchivo(int _cud)
        {
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            var resultado = db.spAlm_InsertProcArchivo(_cud, _idUsuarioProc);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //Lista los archivos disponibles en donde se puede realizar la ubicacion de la unidad documental
        [HttpPost]
        public JsonResult listaArchivos()
        {
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            var resultado = db.spAlm_ConsultaArchivos(_idUsuarioProc);
            try
            {
                return Json(new SelectList(resultado.ToList(), "id_archivo", "cod_archivo"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Lista los modulos disponibles en donde se puede realizar la ubicacion de la unidad documental
        [HttpPost]
        public JsonResult listaModulos(long _codArchivo)
        {
            var resultado = db.spAlm_ConsArchivoModulo(_codArchivo);
            try
            {
                return Json(new SelectList(resultado.ToList(), "id_modulo", "cod_modulo"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Lista los niveles disponibles en donde se puede realizar la ubicacion de la unidad documental
        [HttpPost]
        public JsonResult listaNiveles(long _codModulo)
        {
            var resultado = db.spAlm_ModuloNiveles(_codModulo);
            try
            {
                return Json(new SelectList(resultado.ToList(), "cod_nivel"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Lista las posiciones disponibles en donde se puede realizar la ubicacion de la unidad documental
        [HttpPost]
        public JsonResult listaPosic(long _codModulo, int _nivel)
        {
            var resultado = db.spAlm_ModuloNivelSubN(_codModulo, _nivel);
            try
            {
                return Json(new SelectList(resultado.ToList(), "cod_subnivel"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Elimina el proceso del archivo de la unidad documental
        [HttpPost]
        public void borraProcArchivo(int _codUD)
        {
            db.spAlm_BorraProcArchivo(_codUD);
        }

        //Finaliza el proceso del archivo de las Unidadaes documentales seleccionadas
        [HttpPost]
        public void finProcArchivo(int _codUD, long _codModulo, int _codNivel, int _codSNivel)
        {
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            db.spAlm_FinProcArchivo(_codUD, _codModulo, _codNivel, _codSNivel, _idUsuarioProc);
        }
    }
    public class grilla_ConsultaCUD
    {
        public long? _cud { get; set; }
        public string _estado { get; set; }
        public string _cliente { get; set; }
        public string _oficina { get; set; }
        public string _fecTermina { get; set; }
        public string _nomUsuario { get; set; }
    }

    public class ventanaModal
    {
        public int _cud { get; set; }
        public int _cantDocs { get; set; }
        public string _cliente { get; set; }
        public string _oficina { get; set; }
        public string _fecCreacionUD { get; set; }
    }
}

