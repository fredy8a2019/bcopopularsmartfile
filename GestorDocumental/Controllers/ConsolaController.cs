using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using GestorDocumental.Models;
using GestorDocumental.WebService;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class ConsolaController : Controller
    {
        GestorDocumentalEnt wsTablas = new GestorDocumentalEnt();
        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Consola de resultados XML";
                Session["CURRENT_FILE"] = "";
                ViewData["menu"] = "";

                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        [GridAction]
        public ActionResult _verResultados()
        {
            var grilla = _grillaWsResultado();
            return View(new GridModel<grillaResultado>(grilla));
        }

        [GridAction]
        public ActionResult _verResultadosDetalle(int idNegocio)
        {
            var grilla = _grillaWsResultadoDetalle(idNegocio);
            return View(new GridModel<grillaResultadoDetalle>(grilla));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult ReenviarNegocio(int idNegocio)
        {
            WebServiceModel wsOp = new WebServiceModel();
            wsOp.updateFacturaEstadoReenvio(idNegocio);

            decimal usuario = wsOp.obtenerNoUsuario(idNegocio);
            string rutaXML = CrearXML.CrearXMLFile(idNegocio);

            if (System.IO.File.Exists(rutaXML))
            {
                Thread t = new Thread(() => new WebServiceController().llamadoWebService(rutaXML, usuario.ToString(), idNegocio.ToString()));
                t.Start();
            }

            var grilla = _grillaWsResultado();
            return View(new GridModel<grillaResultado>(grilla));
        }

        private IEnumerable<grillaResultado> _grillaWsResultado()
        {
            var grilla = (from ws in wsTablas.WS_YCONSOLA_CABECERA
                          join wsDat in wsTablas.WS_DATOS_ENTRADA on ws.NEG_ID equals wsDat.NEG_ID_CABECERA
                          select new grillaResultado()
                          {
                              no_negocio = ws.NEG_ID,
                              no_negocio_gestor = wsDat.NEG_ID_CABECERA,
                              no_factura = ws.FACT_ID,
                              tipo_documento = ws.DOC_TYPE,
                              fecha_documento = ws.DOC_DATE
                          }).OrderByDescending(d => d.no_negocio).Distinct();
            return grilla;
        }

        private IEnumerable<grillaResultadoDetalle> _grillaWsResultadoDetalle(int idNegocio)
        {
            var grilla = (from ws in wsTablas.WS_RESULTADO
                          where ws.NEG_ID_CABECERA == idNegocio
                          select new grillaResultadoDetalle()
                          {
                              rcode = ws.RCODE,
                              ok_code = ws.OK_CODE,
                              descripcion = ws.DESCRIPTION,
                              fecha_respuesta = ws.FECHA_RESPUESTA
                          });
            return grilla;
        }
    }

    public class grillaResultado
    {
        public decimal no_negocio { get; set; }
        public decimal no_negocio_gestor { get; set; }
        public string no_factura { get; set; }
        public string tipo_documento { get; set; }
        public DateTime? fecha_documento { get; set; }
    }

    public class grillaResultadoDetalle
    {
        public string rcode { get; set; }
        public string ok_code { get; set; }
        public string descripcion { get; set; }
        public DateTime? fecha_respuesta { get; set; }
    }
}
