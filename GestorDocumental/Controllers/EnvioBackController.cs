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
    public class EnvioBackController : Controller
    {
        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                int _cliNit = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).CliNit);
                Session["nit_cliente"] = _cliNit;

                obtenerFiltros();
                generarCampos();
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        #region Grilla Envio Padre
        [HttpPost]
        [GridAction]
        public virtual JsonResult getInformacionEnvio(int nitCliente)
        {
            var grilla = _getInformacionEnvio(nitCliente);
            return Json(new GridModel<grilla_EnvioPadre>(grilla));
        }

        public IEnumerable<grilla_EnvioPadre> _getInformacionEnvio(int nitCliente)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_GrillaEnvio(nitCliente);
            List<grilla_EnvioPadre> _grillaAlm = new List<grilla_EnvioPadre>();

            foreach (spAlm_GrillaEnvio_Result item in grilla)
            {
                grilla_EnvioPadre data = new grilla_EnvioPadre();
                data.cud = item.Cud;
                data.tipoContenedor = item.TipoContenedor;
                data.nomOficina = item.NomOficina;
                data.subProductos = item.SubProductos;
                data.fechaCreacion = item.FechaCreacion.ToString();

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }
        #endregion

        #region Grilla Envio Hijo

        [HttpPost]
        [GridAction]
        public ActionResult getDetalle(int CUD) {
            var grilla = _getDetalle(CUD);
            return View(new GridModel<grilla_EnvioHijo>(grilla));
        }

        public IEnumerable<grilla_EnvioHijo> _getDetalle(int cud)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_GrillaEnvioDetalle(cud);
            List<grilla_EnvioHijo> _grillaAlm = new List<grilla_EnvioHijo>();

            foreach (spAlm_GrillaEnvioDetalle_Result item in grilla)
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


        [HttpPost]
        public void guardarFormato(FormCollection collection)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            int _idUsuario = Convert.ToInt32(((Usuarios)this.Session["USUARIO"]).IdUsuario);

            //Se guarda la captura del CUD seleccionado
            for (int i = 1; i < collection.Count; i++)
            {
                alm_CapturaUD data = new alm_CapturaUD();
                data.CUD = long.Parse(collection[0].ToString());
                data.CampId = Convert.ToInt32(collection.Keys[i].ToString());
                data.Valor = collection[i].ToString();
                data.Usuario = _idUsuario;
                data.Fecha = DateTime.Now;

                dbo.alm_CapturaUD.AddObject(data);
                dbo.SaveChanges();
            }

            dbo.spAlm_CrearAsignacionTareaEnvio(Convert.ToInt32(collection[0].ToString()), _idUsuario);
            Response.Redirect("Index");
        }

        public void generarCampos()
        {
            CrearFormulariosCaptura formularioEnvio = new CrearFormulariosCaptura();
            List<Campos> lstCampos = obtenerCamposEnvio();
            Table ta = new Table();

            string campos = formularioEnvio.GenerarCampos(ta, lstCampos, null, 93, 0, "0", 0, 0);
            campos = campos.Replace('"', '\'');
            ViewData["_camposEnvio"] = campos;
        }

        public List<Campos> obtenerCamposEnvio()
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            var query = (from a in dbo.Campos
                         where a.CodFormulario == 93
                         select a);
            return query.ToList();
        }

        //Obtiene el nombre de los filtros parametrizados para mostrarlos en la grilla y en el campo filtro
        public void obtenerFiltros()
        {
            GestorDocumentalEnt db = new GestorDocumentalEnt();
            List<spAlm_ObtenerNombreFiltro_Result> lst = db.spAlm_ObtenerNombreFiltro().ToList();
            ViewData["CAMPO_1"] = lst[0].Valor.ToString();
            ViewData["CAMPO_2"] = lst[1].Valor.ToString();
            ViewData["CAMPO_3"] = lst[2].Valor.ToString();
        }
    }

    //Datos de la grilla de informacion de la Unidad Documental (Padre)
    public class grilla_EnvioPadre
    {
        public long cud { get; set; }
        public string tipoContenedor { get; set; }
        public string nomOficina { get; set; }
        public string subProductos { get; set; }
        public string fechaCreacion { get; set; }
    }

    public class grilla_EnvioHijo
    {
        public int posicion { get; set; }
        public decimal negId { get; set; }
        public string codBarras { get; set; }
        public int paginas { get; set; }
        public DateTime fecha { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }
        public string campo3 { get; set; }
    }
}
