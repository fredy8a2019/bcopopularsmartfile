using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class SolicitudBackController : Controller
    {
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Confirmar()
        {
            return View();
        }

        #region Grilla Documentos con solicitud pendiente
        [HttpPost]
        [GridAction]
        public virtual ActionResult getInformacionSPendientes()
        {
            //Obtener la informacion de los documentos pendientes por una solicitud de prestamo
            var grilla = _getInformacionSPendientes();
            return View(new GridModel<grilla_SolicitudesPendientes>(grilla));
        }

        public IEnumerable<grilla_SolicitudesPendientes> _getInformacionSPendientes()
        {
            //Metodo que ejecuta y trae la informacion de los documentos que tienen una solicitud de prestamo pendiente.
            var grilla = dbo.spAlm_GrillaSolicitudesPendientes();
            List<grilla_SolicitudesPendientes> _grillaAlm = new List<grilla_SolicitudesPendientes>();

            foreach (spAlm_GrillaSolicitudesPendientes_Result item in grilla)
            {
                grilla_SolicitudesPendientes data = new grilla_SolicitudesPendientes();
                data._noTiquet = item.NoTiquet.ToString();
                data._negId = item.NegId.ToString();
                data._documentoSolicitado = item.DocumentoSolicitado.ToString();
                data._fechaSolicitud = item.FechaSolicitud.ToString();
                data._usuario = item.Usuario.ToString();

                _grillaAlm.Add(data);
            }
            return _grillaAlm;
        }
        #endregion

        #region Grilla Detalle Documentos con solicitud pendiente

        [HttpPost]
        [GridAction]
        public virtual ActionResult getInformacionDetalle(int noTiquet)
        {
            var grilla = _getInformacionDetalle(noTiquet);
            return View(new GridModel<grilla_SolicitudesPendientesDetalle>(grilla));
        }

        public IEnumerable<grilla_SolicitudesPendientesDetalle> _getInformacionDetalle(int noTiquet)
        {
            var grilla = dbo.spAlm_GrillaSolicitudesPendientesDetalle(noTiquet);
            List<grilla_SolicitudesPendientesDetalle> _grillaAlm = new List<grilla_SolicitudesPendientesDetalle>();

            foreach (spAlm_GrillaSolicitudesPendientesDetalle_Result item in grilla)
            {
                grilla_SolicitudesPendientesDetalle data = new grilla_SolicitudesPendientesDetalle();
                data._negId = item.NegId.ToString();
                data._noDocumento = item.DocumentoSolicitado;

                _grillaAlm.Add(data);
            }
            return _grillaAlm;
        }
        #endregion

        #region Grilla Documentos con Devolucion pendiente
        [HttpPost]
        [GridAction]
        public virtual ActionResult getInformacionSDevolucion()
        {
            //Obtener la informacion de los documentos pendientes por una solicitud de prestamo
            var grilla = _getInformacionSDevolucion();
            return View(new GridModel<grilla_SolicitudesDevolucion>(grilla));
        }

        public IEnumerable<grilla_SolicitudesDevolucion> _getInformacionSDevolucion()
        {
            //Metodo que ejecuta y trae la informacion de los documentos que estan pendientes por devolucion
            var grilla = dbo.spAlm_GrillaSolicitudesDevolucion();
            List<grilla_SolicitudesDevolucion> _grillaAlm = new List<grilla_SolicitudesDevolucion>();

            foreach (spAlm_GrillaSolicitudesDevolucion_Result item in grilla)
            {
                grilla_SolicitudesDevolucion data = new grilla_SolicitudesDevolucion();
                string semaforo = "";

                data._noTiquet = item.NoTiquet.ToString();
                data._negId = item.NegId.ToString();
                data._documentoSolicitado = item.DocumentoSolicitado.ToString();
                data._fechaSolicitud = item.FechaDevolucion.ToString();
                data._usuario = item.Usuario.ToString();

                if (item.Semaforo == 1)
                    semaforo = "../../Images/Semaforo/CirculoRojo.png";
                else
                    semaforo = "../../Images/Semaforo/Circuloverde.png";
                data._semaforo = semaforo;

                _grillaAlm.Add(data);
            }
            return _grillaAlm;
        }
        #endregion

        [HttpPost]
        public void actualizarFechaEntrega(FormCollection collection)
        {
            int diasAnadidos = Convert.ToInt32(collection["txtDias"].ToString());
            int noTiquet = Convert.ToInt32(collection["noTiquet"].ToString());

            dbo.spAlm_ActualizarFechaEntrega(diasAnadidos, noTiquet);
            Response.Redirect("/SolicitudBack/Confirmar");
        }

        [HttpPost]
        public void confirmarDevolucion(string negId)
        {
            string[] _negID = negId.Split(',');
            for (int i = 0; i < _negID.Length; i++)
            {
                dbo.spAlm_ConfirmarDevolucion(Convert.ToDecimal(_negID[i]));
            }
        }

        [HttpGet]
        public JsonResult obtenerCamposSolicitud(int negId)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            if (negId != 0)
            {
                var lstCampos = dbo.spAlm_ObtenerCamposSPrestamos();
                List<spAlm_ObtenerValoresCapPrestamos_Result> lstvalores = dbo.spAlm_ObtenerValoresCapPrestamos(negId).ToList();

                string oTabla = "<table>";
                string sCampos = "";
                string cTabla = "</table>";
                int contador = 0;

                foreach (spAlm_ObtenerCamposSPrestamos_Result item in lstCampos)
                {
                    if (String.IsNullOrEmpty(sCampos))
                    {
                        sCampos = "<tr><td>" + item.CampDescripcion + "</td></tr>" +
                            "<tr><td><textarea rows=\"4\" cols=\"50\" readonly='true' class=\"form-control cmenu1\" value='texto' id='"
                            + item.CampId + "'>" + lstvalores[contador].Valor + "</textarea></td></tr>";
                    }
                    else
                    {
                        sCampos = sCampos + "<tr><td>" + item.CampDescripcion + "</td></tr>" +
                            "<tr><td><textarea rows=\"4\" cols=\"50\" readonly='true' class=\"form-control cmenu1\" value='texto' id='"
                            + item.CampId + "'>" + lstvalores[contador].Valor + "</textarea></td></tr>";
                    }

                    contador++;
                }

                string htmlEs = oTabla + sCampos + cTabla;
                return Json(htmlEs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string htmlEs = "";
                return Json(htmlEs, JsonRequestBehavior.AllowGet);
            }
        }
    }

    //Datos de la grilla de las solicitudes pendientes
    public class grilla_SolicitudesPendientes
    {
        public string _noTiquet { get; set; }
        public string _negId { get; set; }
        public string _documentoSolicitado { get; set; }
        public string _fechaSolicitud { get; set; }
        public string _usuario { get; set; }
    }

    //Datos de la grilla del detalle de las solicitudes pendientes cuando es prestamos masivo
    public class grilla_SolicitudesPendientesDetalle
    {
        public string _negId { get; set; }
        public string _noDocumento { get; set; }
    }

    //Datos de la grilla de las solicitudes con devolucion
    public class grilla_SolicitudesDevolucion
    {
        public string _noTiquet { get; set; }
        public string _negId { get; set; }
        public string _documentoSolicitado { get; set; }
        public string _fechaSolicitud { get; set; }
        public string _usuario { get; set; }
        public string _semaforo { get; set; }
    }
}
