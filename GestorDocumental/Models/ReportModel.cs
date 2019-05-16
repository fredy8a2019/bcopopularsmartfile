using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace GestorDocumental.Models
{
    public class ReportModel
    {
        const string GestorDocumentalEntDataContextKey = "DataContext";
        public static GestorDocumentalEnt DB
        {
            get
            {
                if (HttpContext.Current.Items[GestorDocumentalEntDataContextKey] == null)
                    HttpContext.Current.Items[GestorDocumentalEntDataContextKey] = new GestorDocumentalEnt();
                return (GestorDocumentalEnt)HttpContext.Current.Items[GestorDocumentalEntDataContextKey];
            }
        }

        public IEnumerable GetAllAsignacionTareas(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return DB.spReporteEstadoVSNegocios(fechaInicio, fechaFin).Select(x => new
                {

                    Cantidad = int.Parse(x.Cantidad),
                    x.Estado,
                    x.Etapa,
                    HoraInicio = x.HoraInicio.ToString(),
                    HoraTerminacion = x.HoraTerminacion.ToString(),
                    Mes = x.HoraInicio.Month,
                    Year = x.HoraInicio.Year,
                    Dia = x.HoraInicio.Day,
                    x.NegId,
                    Tiempo = int.Parse(x.Tiempo),
                    x.Ususario,
                    x.LoteScaner,
                    x.Terminado,
                    cliente = x.CliNombre,
                    nitcliente = x.CliNit,
                    oficina = x.OFI_Nombre,
                    producto = x.GruDescripcion
                    //Productividad = decimal.Round((decimal.Parse(x.Tiempo == "0" ? x.Cantidad = "0" : x.Cantidad) / decimal.Parse(x.Tiempo == "0" ? x.Tiempo = "1" : x.Tiempo)), 5)// ((Decimal)1/18).ToString("0.###") // 
                });

            }
            catch (Exception e)
            {
                LogRepository.registro("Error en ReportModel metodo GetAllAsignacionTareas " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
            //decimal pro = 0;

            // return basdat.AsignacionTareas.Select(x => new { x.NegId, x.IdEstado, x.IdEtapa });
        }

        /// <summary>
        /// Obtiene la cantidad de documentos que se han processado en cada una de las estapes de negocio 
        /// </summary>
        /// <returns></returns>
        public IList<spFacturacion_Result> getFacturacionData(String fechaInicio, String fechaFin, String cliente, String oficina, String producto, String sociedad, int contador)
        {
            try
            {
                IList<spFacturacion_Result> resultado =
               (IList<spFacturacion_Result>)HttpContext.Current.Session["Datos"];

                if (resultado == null || resultado.Count == 0  || contador == 1)
                {
                    HttpContext.Current.Session["Datos"] =
                      DB.spFacturacion(fechaInicio, fechaFin, cliente, oficina, producto, sociedad).ToList<spFacturacion_Result>();
                }
                return resultado;
                 
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en ReportModel metodo getFacturacionData " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
        }
    }
}