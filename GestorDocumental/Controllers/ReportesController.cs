using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class ReportesController : Controller
    {
        private GestorDocumentalEnt gd = new GestorDocumentalEnt();

        public List<ConsultaPorCedula_Result> ConsultaPorCedula(Clientes c, string cedula)
        {
            List<ConsultaPorCedula_Result> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<ConsultaPorCedula_Result> list = this.gd.ConsultaPorCedula(cedula, new decimal?(c.CliNit)).ToList<ConsultaPorCedula_Result>();
                ConsultaPorCedula_Result item = new ConsultaPorCedula_Result
                {
                    NegId = 0,
                    Nombre = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception)
            {
                throw;
            }
            return list2;
        }

        public List<DocumentosNegocio_Result> DocumentosNegocio(Captura cap)
        {
            List<DocumentosNegocio_Result> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<DocumentosNegocio_Result> list = this.gd.DocumentosNegocio(new decimal?(cap.NegId)).ToList<DocumentosNegocio_Result>();
                DocumentosNegocio_Result item = new DocumentosNegocio_Result
                {
                    DocId = 0,
                    DocDescripcion = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception)
            {
                throw;
            }
            return list2;
        }

        public List<ReporteEstadosNegocios_Result> ReporteEstadosNegocios(DateTime d1, DateTime d2)
        {
            List<ReporteEstadosNegocios_Result> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                list2 = this.gd.ReporteEstadosNegocios(new DateTime?(d1), new DateTime?(d2)).ToList<ReporteEstadosNegocios_Result>();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list2;
        }

        public List<ReporteResumenEstadoNegocios_Result> ReporteResumenEstadoNegocios(DateTime d1, DateTime d2)
        {
            List<ReporteResumenEstadoNegocios_Result> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                list2 = this.gd.ReporteResumenEstadoNegocios(new DateTime?(d1), new DateTime?(d2)).ToList<ReporteResumenEstadoNegocios_Result>();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list2;
        }

        public List<spReporteRecepcion_Result> getSpReporteRecepcion(String fechaInicial, String fechaFinal, String cliente, String oficina, String producto, String subProducto, String estado, String lote)
        {
            try
            {
                this.gd = new GestorDocumentalEnt();
                return gd.spReporteRecepcion(fechaInicial, fechaFinal, cliente, oficina,producto, subProducto,estado,lote).ToList<spReporteRecepcion_Result>();                
              }
            catch (Exception es)
            {
                throw es;
            }
        }

        public List<spReporteContabilizacion_Result> ReporteContabilizacion(string fechaInicio, string fechaFin, string cliente,
                                                                            string oficina, string producto, string subProducto,
                                                                            string proveedor, string nombreSociedad,string codigoBarras,string noDocumento)
        {
            try
            {
                return gd.spReporteContabilizacion(fechaInicio,fechaFin,
                                                    cliente,oficina,producto,
                                                    subProducto,proveedor,
                                                    nombreSociedad,codigoBarras,noDocumento,null).ToList<spReporteContabilizacion_Result>();
            }
            catch (Exception es)
            {

                throw;
            }
        }

        public List<spReporteRadicacion_Result> ReporteRadicacion (String FechaInicio,	String FechaFin ,String Cliente ,
	                                                               String Oficina, String Producto, String SubProducto ,
	                                                               String Estado ,String Sociedad ,String Proveedor ,	
	                                                               String NoDocumento ,String Causal ,String CodigoBarras ,String NegId )
        {
            try
            {
                return gd.spReporteRadicacion(FechaInicio, FechaFin, Cliente, Oficina, Producto, 
                                            SubProducto, Estado, Sociedad, Proveedor, NoDocumento, 
                                            Causal, CodigoBarras, NegId).ToList<spReporteRadicacion_Result>();
            }
            catch (Exception es )
            {                
                throw;
            }
        }
    }
}
