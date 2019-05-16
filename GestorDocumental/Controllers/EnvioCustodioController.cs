using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class EnvioCustodioController : Controller
    {
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();

        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                obtenerFiltros();
                ObtenerIdEnvioDisponible();

                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        #region Grilla documentos listos para envio
        [HttpPost]
        [GridAction]
        public virtual ActionResult getInformacionEnvio()
        {
            //Obtener la informacion de la solicitud del documento para pintarla en la grilla
            var grilla = _getInformacionEnvio();
            return View(new GridModel<grilla_EnvioFinal>(grilla));
        }

        public IEnumerable<grilla_EnvioFinal> _getInformacionEnvio()
        {
            //Metodo que ejecuta y trae la informacion de los documentos que son solicitados
            decimal _cliNit = Convert.ToDecimal(((Usuarios)this.Session["USUARIO"]).CliNit);
            var grilla = dbo.spAlm_GrillaEnvioFinal(_cliNit);
            List<grilla_EnvioFinal> _grillaAlm = new List<grilla_EnvioFinal>();

            foreach (spAlm_GrillaEnvioFinal_Result item in grilla)
            {
                grilla_EnvioFinal data = new grilla_EnvioFinal();
                data._negId = item.NegId.ToString();
                data._producto = item.Producto.ToString();
                data._subProducto = item.SubProducto.ToString();
                data._codBarras = item.CodBarras.ToString();
                data._campoUno = item.Campo1;
                data._campoDos = item.Campo2;
                data._campoTres = item.Campo3;

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }
        #endregion

        #region Grilla documentos guardados listos para el envio
        [HttpPost]
        [GridAction]
        public virtual ActionResult getInformacionEnvioContenido()
        {
            //Obtener la informacion de la solicitud del documento para pintarla en la grilla
            var grilla = _getInformacionEnvioContenido();
            return View(new GridModel<grilla_EnvioFinal>(grilla));
        }

        public IEnumerable<grilla_EnvioFinal> _getInformacionEnvioContenido()
        {
            //Metodo que ejecuta y trae la informacion de los documentos que son solicitados
            decimal _cliNit = Convert.ToDecimal(((Usuarios)this.Session["USUARIO"]).CliNit);
            decimal idEnvio = Convert.ToDecimal(dbo.spAlm_ObtenerIdEnvioDisponible().SingleOrDefault());

            var grilla = dbo.spAlm_GrillaEnvioFinalContenido(idEnvio, _cliNit);
            List<grilla_EnvioFinal> _grillaAlm = new List<grilla_EnvioFinal>();

            foreach (spAlm_GrillaEnvioFinalContenido_Result item in grilla)
            {
                grilla_EnvioFinal data = new grilla_EnvioFinal();
                data._negId = item.NegId.ToString();
                data._producto = item.Producto.ToString();
                data._subProducto = item.SubProducto.ToString();
                data._codBarras = item.CodBarras.ToString();
                data._campoUno = item.Campo1;
                data._campoDos = item.Campo2;
                data._campoTres = item.Campo3;

                _grillaAlm.Add(data);
            }

            return _grillaAlm;
        }
        #endregion

        [HttpPost]
        public void agregarNegocioEnvio(string idEnvio, string negId)
        {
            decimal _idEnvio = Convert.ToDecimal(idEnvio);
            decimal _negId = Convert.ToDecimal(negId);
            dbo.spAlm_AgregarNegocioEnvio(_idEnvio, _negId);
        }

        [HttpPost]
        public void eliminarNegocioEnvio(string idEnvio, string negId)
        {
            decimal _idEnvio = Convert.ToDecimal(idEnvio);
            decimal _negId = Convert.ToDecimal(negId);
            dbo.spAlm_EliminarNegocioEnvio(_idEnvio, _negId);
        }

        [HttpPost]
        public void ConfirmarEnvioFinal(string guia, string currier, string precinto, string idEnvioFinal)
        {
            decimal _cliNit = Convert.ToDecimal(((Usuarios)this.Session["USUARIO"]).CliNit);
            decimal _idUsuario = Convert.ToDecimal(((Usuarios)this.Session["USUARIO"]).IdUsuario);

            dbo.spAlm_ConfirmarEnvioFinal(_cliNit, _idUsuario, guia, currier, precinto, Convert.ToDecimal(idEnvioFinal));
        }

        //Obtiene el nombre de los filtros parametrizados para mostrarlos en la grilla y en el campo filtro
        public void obtenerFiltros()
        {
            List<spAlm_ObtenerNombreFiltro_Result> lst = dbo.spAlm_ObtenerNombreFiltro().ToList();
            ViewData["CAMPO_1"] = lst[0].Valor.ToString();
            ViewData["CAMPO_2"] = lst[1].Valor.ToString();
            ViewData["CAMPO_3"] = lst[2].Valor.ToString();
        }

        public void ObtenerIdEnvioDisponible()
        {
            decimal idEnvio = Convert.ToDecimal(dbo.spAlm_ObtenerIdEnvioDisponible().SingleOrDefault());
            ViewData["_IdEnvioDisponible"] = idEnvio;
        }
    }

    public class grilla_EnvioFinal
    {
        public string _negId { get; set; }
        public string _producto { get; set; }
        public string _subProducto { get; set; }
        public string _codBarras { get; set; }
        public string _campoUno { get; set; }
        public string _campoDos { get; set; }
        public string _campoTres { get; set; }
    }
}
