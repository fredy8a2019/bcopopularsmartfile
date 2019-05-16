using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;
using System.Web.UI;

namespace GestorDocumental.Controllers
{
    public class FinDestruccionController : Controller
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        public ActionResult FinDestruccion()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Crear Archivo";
                ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;

                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        #region Grilla Acta Padre
        [HttpPost]
        [GridAction]
        public virtual JsonResult getInformacionActa()
        {
            var grillaInfo = _getInformacionActa();
            return Json(new GridModel<grilla_InfoActa>(grillaInfo));
        }

        public IEnumerable<grilla_InfoActa> _getInformacionActa()
        {
            var grillaInfo = db.spAlm_ConsulActas();
            List<grilla_InfoActa> _grilla = new List<grilla_InfoActa>();

            foreach (spAlm_ConsulActas_Result item in grillaInfo)
            {
                grilla_InfoActa data = new grilla_InfoActa();
                data._nroActa = item.Nro_Acta;
                data._cantNeg = Convert.ToInt32(item.Cant_Negocios);
                data._fechCreacion = Convert.ToString(item.Fecha_Creacíon_Acta);
                data._oficina = item.Oficina;
                data._cliente = item.Cliente;

                _grilla.Add(data);
            }
            return _grilla;
        }
        #endregion

        #region Grilla Acta Hijo
        [HttpPost]
        [GridAction]
        public ActionResult getDetalleActa(int _nroActa)
        {
            var grilla = _getDetalleActa(_nroActa);
            return View(new GridModel<grilla_DetalleActa>(grilla));
        }

        public IEnumerable<grilla_DetalleActa> _getDetalleActa(int _nroActa)
        {
            var grilla = db.spAlm_DetalleActa(_nroActa);
            List<grilla_DetalleActa> _grillaDet = new List<grilla_DetalleActa>();

            foreach (spAlm_DetalleActa_Result item in grilla)
            {
                grilla_DetalleActa data = new grilla_DetalleActa();
                data._codNeg = item.cod_NegId;
                data._codUsuario = item.cod_usuario;
                data._codBarras = item.cod_barras;

                _grillaDet.Add(data);
            }
            return _grillaDet;
        }
        #endregion

        [HttpPost]
        public void destrucFinal(int _nroActa)
        {
            db.spAlm_DestrucFInal(_nroActa);
        }
    }

    public class grilla_InfoActa
    {
        public int _nroActa { get; set; }
        public int _cantNeg { get; set; }
        public string _fechCreacion { get; set; }
        public string _oficina { get; set; }
        public string _cliente { get; set; }

    }

    public class grilla_DetalleActa
    {
        public int _codNeg { get; set; }
        public decimal _codUsuario { get; set; }
        public string _codBarras { get; set; }
    }
}
