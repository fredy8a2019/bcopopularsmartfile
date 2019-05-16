using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;
using DevExpress.Web.Mvc;


namespace GestorDocumental.Controllers
{
    public class ReportController : Controller
    {
        private GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>
        /// Region para mostrar el reporte de facturacion 
        /// </summary>
        /// <returns></returns>
        #region ReporteFacturacion

        public ActionResult FacturacionIndex()
        {
            //var xx = ((GestorDocumental.Models.Clientes)Session["CLIENTE"]).CliNombre;
            DateTime fechaHoy = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
            DateTime fechaIniMes = Convert.ToDateTime(fechaInicioMes(DateTime.Now).ToShortDateString());

            if (Session["FechaInial"] == null && Session["FechaFin"] == null)
            {
                Session["FechaInial"] = fechaIniMes.ToShortDateString();
                Session["FechaFin"] = fechaHoy.ToShortDateString();
                Session["fCliente"] = string.Empty;
                Session["Oficina"] = string.Empty;
                Session["Producto"] = string.Empty;
                Session["Sociedad"] = string.Empty;
                Session["Etapa"] = string.Empty;
            }
            Session["contador"] = 0;

            return View();
        }

        public ActionResult PivotGridPartialFacturacion()
        {
            string FechaInicio = (Session["FechaInial"].ToString());
            string FechaFin = (Session["FechaFin"].ToString());
            string Cliente = Session["fCliente"].ToString();
            string Ofician = Session["Oficina"].ToString();
            string Producto = Session["Producto"].ToString();
            string Sociedad = Session["Sociedad"].ToString();
            Session["contador"] = int.Parse(Session["contador"].ToString()) + 1;

            int contador = int.Parse(Session["contador"].ToString());

            return PartialView("PivotGridPartialFacturacion", getDataFactura(FechaInicio, FechaFin, Cliente, Ofician, Producto, Sociedad, contador));
        }

        public static IEnumerable getDataFactura(string fechaInicio, string fechaFin, string Cliente, string Oficina, string Producto, string Sociedad, int contador)
        {
            ReportModel rm = new ReportModel();
            return rm.getFacturacionData(fechaInicio, fechaFin, Cliente, Oficina, Producto, Sociedad, contador);
        }

        public ActionResult Filtros(FormCollection collection)
        {
            Session["FechaInial"] = collection["fechaInicial"]; ;
            Session["FechaFin"] = collection["fechaFin"];
            Session["fCliente"] = collection["Cliente"];
            Session["Oficina"] = collection["Oficinas"];
            Session["Producto"] = collection["Productos"];
            Session["Sociedad"] = collection["Sociedad"];

            return Content("<script language='javascript' type='text/javascript'>   location.href = '/Report/FacturacionIndex'; </script>");
        }

        public ActionResult Archivo()
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + "PivotGrid.xlsx");
            Response.WriteFile(Server.MapPath("~/Content/Tmp/" + "PivotGrid.xlsx"));
            Response.ContentType = "";
            Response.End();
            return View();
   
        } 


        #endregion

        #region ReporteEstados

        public System.DateTime fechaInicioMes(DateTime date)
        {
            if (date.Day == 1)
            {
                date.AddDays(date.AddDays(-1).Day - date.Day);
                date = Convert.ToDateTime(date.AddMonths(-1).ToShortDateString());
                return date;
            }
            else
            {

                return date.AddDays(-date.AddDays(-1).Day);
            }
        }

        public ActionResult Index()
        {
            DateTime fechaHoy = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
            DateTime fechaIniMes = Convert.ToDateTime(fechaInicioMes(DateTime.Now).ToShortDateString());
            //
            //Session["FechaInial"] = "2014-01-01";
            //Session["FechaFin"] = "2014-01-01";

            if (Session["FechaInial"] == null && Session["FechaFin"] == null)
            {
                Session["FechaInial"] = fechaIniMes.ToShortDateString();
                Session["FechaFin"] = fechaHoy.ToShortDateString();
            }

            //DateTime FechaInicio = Convert.ToDateTime(Session["FechaInial"].ToString());
            //DateTime FechaFin = Convert.ToDateTime(Session["FechaFin"].ToString());

            //SessionRepository.dataRepEstado(FechaInicio, FechaFin);


            return View();
        }


        public ActionResult VerNegocio(String negId)
        {
            Parametros param = db.Parametros.First(c => c.codigo == "PATH_VISOR");

            Session["IMG_VISOR"] = param.valor.ToString() + negId + "/" + negId + ".tif";
            ViewData["NEG"] = negId;

            return View();
        }


        public ActionResult PivotGridPartial()
        {

            DateTime FechaInicio = Convert.ToDateTime(Session["FechaInial"].ToString());
            DateTime FechaFin = Convert.ToDateTime(Session["FechaFin"].ToString());

            //IEnumerable result = SessionRepository.dataRepEstado(FechaInicio, FechaFin);

            return PartialView("PivotGridPartial", GetSalesPerson(FechaInicio, FechaFin));

        }

        public ActionResult VariablesFiltrar(DateTime fechaInicial, DateTime fechaFin)
        {
            Session["FechaInial"] = fechaInicial.ToShortDateString();
            Session["FechaFin"] = fechaFin.ToShortDateString();
            return View();
        }

        public static IEnumerable GetSalesPerson(DateTime fechaInicio, DateTime fechaFin)
        {
            ReportModel rm = new ReportModel();
            return rm.GetAllAsignacionTareas(fechaInicio, fechaFin).AsParallel();
            //return (basdat.AsignacionTareas.Select(x => new { x.NegId, x.IdEstado, x.IdEtapa, x.Usuario }));
        }
        #endregion
    }
    
}
