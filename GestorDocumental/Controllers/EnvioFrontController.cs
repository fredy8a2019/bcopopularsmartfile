using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;
using System.Web.UI.WebControls;

namespace GestorDocumental.Controllers
{
    public class EnvioFrontController : Controller
    {
        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                //Obtener el nit del cliente asociado al usuario logueado en el sistema
                int _cliNit = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).CliNit);
                Session["nit_cliente"] = _cliNit;
                obtenerFiltros();
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        #region Grilla Recepcion Padre
        [HttpPost]
        [GridAction]
        public virtual ActionResult getInformacionRecepcion(int cud, int nitCliente)
        {
            //Obtener la informacion de la grilla de las unidades documentales que estan listas para el modulo de Recepcion
            var grilla = _getInformacionRecepcion(cud, nitCliente);
            return View(new GridModel<grilla_RecepcionPadre>(grilla));
        }

        public IEnumerable<grilla_RecepcionPadre> _getInformacionRecepcion(int cud, int nitCliente)
        {
            //Metodo que ejecuta el sp que trae la informacion de la unidad documental consultada
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_GrillaRecepcion(cud, nitCliente);
            List<grilla_RecepcionPadre> _grillaAlm = new List<grilla_RecepcionPadre>();

            foreach (spAlm_GrillaRecepcion_Result item in grilla)
            {
                grilla_RecepcionPadre data = new grilla_RecepcionPadre();
                data.cud = item.Cud;
                data.tipoContenedor = item.TipoContenedor;
                data.nomOficina = item.NomOficina;
                data.subProductos = item.SubProductos;
                data.fechaEnvio = item.FechaEnvio.ToString();

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }
        #endregion

        #region Grilla Recepcion Hijo
        [HttpPost]
        [GridAction]
        public ActionResult getDetalle(int CUD)
        {
            //Metodo para consultar la informacion en detalle de los documentos que estan en la unidad documental
            var grilla = _getDetalle(CUD);
            return View(new GridModel<grilla_EnvioHijo>(grilla));
        }

        public IEnumerable<grilla_EnvioHijo> _getDetalle(int cud)
        {
            //Ejecutar el sp que consulta la informacion detallada de la grilla de recepcion de la Unidad documental
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_GrillaRecepcionDetalle(cud);
            List<grilla_EnvioHijo> _grillaAlm = new List<grilla_EnvioHijo>();

            foreach (spAlm_GrillaRecepcionDetalle_Result item in grilla)
            {
                grilla_EnvioHijo data = new grilla_EnvioHijo();
                data.posicion = Convert.ToInt32(item.Posicion);
                data.negId = item.NegId;
                data.codBarras = item.CodBarras;
                data.paginas = item.Paginas;
                data.fecha = Convert.ToDateTime(item.Fecha);
                data.campo1 = item.Campo_1;
                data.campo2 = item.Campo_2;
                data.campo3 = item.Campo_3;

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }
        #endregion

        #region Grilla Documentos Procesados
        [HttpPost]
        [GridAction]
        public ActionResult getGrillaDocumentosProcesados(int CUD)
        {
            //Mostrar la grilla de los documentos que ya han sido procesados
            var grilla = _getGrillaDocumentosProcesados(CUD);
            return View(new GridModel<grilla_RecepcionHijo>(grilla));
        }

        public IEnumerable<grilla_RecepcionHijo> _getGrillaDocumentosProcesados(int cud)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_GrillaDocProcesadosRecepcion(cud);
            List<grilla_RecepcionHijo> _grillaAlm = new List<grilla_RecepcionHijo>();

            foreach (spAlm_GrillaDocProcesadosRecepcion_Result item in grilla)
            {
                grilla_RecepcionHijo data = new grilla_RecepcionHijo();
                data.posicion = Convert.ToInt32(item.Posicion);
                data.negId = item.NegId;
                data.codBarras = item.CodBarras;
                data.paginas = item.DescEstado;
                data.fecha = Convert.ToDateTime(item.Fecha);
                data.campo1 = item.Campo_1;
                data.campo2 = item.Campo_2;
                data.campo3 = item.Campo_3;

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }

        #endregion

        [HttpPost]
        public void procesarArchivos(string negID)
        {
            //Metodo que se encarga de procesar los documentos seleccionados y les crea la asignacion de tareas
            //dependiendo del estado de como ha llegado
            string[] _negID = negID.Split(',');
            List<string> recibidos = new List<string>();
            List<string> rechazados = new List<string>();

            //Separar los negocios Recibidos y No recibidos
            for (int i = 0; i < _negID.Length; i++)
            {
                if (_negID[i].EndsWith("0"))
                {
                    string[] _rechazado = _negID[i].Split('_');
                    string rechazado = _rechazado[0];
                    rechazados.Add(rechazado);
                }
                else if (_negID[i].EndsWith("1"))
                {
                    string[] _recibido = _negID[i].Split('_');
                    string recibido = _recibido[0];
                    recibidos.Add(recibido);
                }
            }

            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            //Se crea la asignacion de tareas para los archivos que no fueron recibidos
            foreach (string item in rechazados)
            {
                alm_AsignacionTareasArchivos data = new alm_AsignacionTareasArchivos
                {
                    NegId = Convert.ToDecimal(item),
                    IdEtapa = 180,
                    Usuario = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).IdUsuario),
                    HoraInicio = DateTime.Now,
                    HoraTerminacion = DateTime.Now,
                    IdEstado = 174
                };

                dbo.alm_AsignacionTareasArchivos.AddObject(data);
                dbo.SaveChanges();
            }

            //Se crea la asignacion de tareas para los archivos que fueron recibidos
            foreach (string item in recibidos)
            {
                alm_AsignacionTareasArchivos data = new alm_AsignacionTareasArchivos
                {
                    NegId = Convert.ToDecimal(item),
                    IdEtapa = 180,
                    Usuario = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).IdUsuario),
                    HoraInicio = DateTime.Now,
                    HoraTerminacion = DateTime.Now,
                    IdEstado = 173
                };

                dbo.alm_AsignacionTareasArchivos.AddObject(data);
                dbo.SaveChanges();
            }
        }

        [HttpGet]
        public JsonResult confirmarRecibido(int cud)
        {
            //Valida que en la unidad documental ya se encuentren procesados todos los documentos.
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            int resultado = Convert.ToInt32(dbo.spAlm_confirmarRecibido(cud).SingleOrDefault());
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void crearAsignacionTareaUD(int cud, string txtObservaciones)
        {
            //Crea la asignacion de tareas de la Unidad documental y termina la etapa de esta
            int _idUsuario = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).IdUsuario);
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            dbo.spAlm_CrearAsignacionTareaRecepcion(cud, _idUsuario, txtObservaciones);
        }

        [HttpPost]
        public void eliminarDocumentoRecepcionado(int negID)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            dbo.spAlm_EliminarDocumentoRecepcionado(negID);
        }

        //Obtiene el nombre de los filtros parametrizados para mostrarlos en la grilla y en el campo filtro
        public void obtenerFiltros()
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            List<spAlm_ObtenerNombreFiltro_Result> lst = dbo.spAlm_ObtenerNombreFiltro().ToList();
            ViewData["CAMPO_1"] = lst[0].Valor.ToString();
            ViewData["CAMPO_2"] = lst[1].Valor.ToString();
            ViewData["CAMPO_3"] = lst[2].Valor.ToString();
        }
    }

    //Datos de la grilla de la informacion de la Unidad documental para Recepcionar
    public class grilla_RecepcionPadre
    {
        public long cud { get; set; }
        public string tipoContenedor { get; set; }
        public string nomOficina { get; set; }
        public string subProductos { get; set; }
        public string fechaEnvio { get; set; }
    }

    public class grilla_RecepcionHijo
    {
        public int posicion { get; set; }
        public decimal negId { get; set; }
        public string codBarras { get; set; }
        public string paginas { get; set; }
        public DateTime fecha { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }
        public string campo3 { get; set; }
    }
}
