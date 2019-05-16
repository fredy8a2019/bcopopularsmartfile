using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using GestorDocumental.WebService;
using GestorDocumental.Models;

namespace GestorDocumental.Models
{
    public class WebServiceModel
    {
        GestorDocumentalEnt tablasWS = new GestorDocumentalEnt();

        public List<DatosEntrada> obtenerDatosEntrada(int NegID)
        {
            var query = from pro in tablasWS.WS_DATOS_ENTRADA
                        where pro.ESTADO == 0 && pro.NEG_ID_CABECERA == NegID
                        select new DatosEntrada()
                        {
                            neg_id = pro.NEG_ID,
                            accion = pro.ACCION,
                            origen = pro.ORIGEN,
                            land1 = pro.LAND1,
                            id_neg_cabecera = pro.NEG_ID_CABECERA,
                            facturaPDFBase64 = pro.FACTURA_PDF_BASE64,
                            estado = pro.ESTADO
                        };
            return query.ToList();
        }

        public List<Factura_cabecera> obtenerDatosFacturaCabecera(decimal id_cabecera)
        {
            var query = from pro in tablasWS.WS_YCONSOLA_CABECERA
                        where pro.NEG_ID == id_cabecera
                        select new Factura_cabecera()
                        {
                            neg_id = pro.NEG_ID,
                            fac_id = pro.FACT_ID,
                            doc_type = pro.DOC_TYPE,
                            doc_number = pro.DOC_NUMBER,
                            doc_date = pro.DOC_DATE,
                            vendor_rfc = pro.VENDOR_RFC,
                            company_rfc = pro.COMPANY_RFC,
                            total_amount = pro.TOTAL_AMOUNT,
                            currency_code = pro.CURRENCY_CODE,
                            employee_number = pro.EMPLOYEE_NUMBER,
                            purchase_order = pro.PURCHASE_ORDER,
                            discount_amount = pro.DISCOUNT_AMOUNT,
                            withholding_tax = pro.WITHOLDING_TAX,
                            advance_flag = pro.ADVANCE_FLAG,
                            fiscal_doc_number = pro.FISCAL_DOC_NUMBER
                        };
            return query.ToList();

        }

        public List<Factura_impuestos> obtenerDatosFacturaImpuestos(decimal id_cabecera)
        {
            var query = from pro in tablasWS.WS_YCONSOLA_IMPUESTOS
                        where pro.NEG_ID_CABECERA == id_cabecera
                        select new Factura_impuestos()
                        {
                            vat_base_amount = pro.VAT_BASE_AMOUNT,
                            vat_percentage = pro.VAT_PERCENTAGE,
                            vat_tax_amount = pro.VAT_TAX_AMOUNT,
                            vat_tax_id = pro.VAT_TAX_ID
                        };
            return query.ToList();
        }

        public List<Factura_posiciones> obtenerDatosFacturaPosiciones(decimal id_cabecera)
        {
            var query = from pro in tablasWS.WS_YCONSOLA_POSICIONES
                        where pro.NEG_ID_CABECERA == id_cabecera
                        select new Factura_posiciones()
                        {
                            delivery_note = pro.DELIVERY_NOTE,
                            deliveryDate = pro.DELIVERY_DATE,
                            item_text = pro.ITEM_TEXT,
                            item_quantity = pro.ITEM_QUANTITY,
                            item_price = pro.ITEM_PRICE,
                            item_amount = pro.ITEM_AMOUNT,
                            item_percentage = pro.ITEM_PERCENTAGE
                        };
            return query.ToList();
        }

        public void updateFacturaEstado(decimal NegId)
        {
            var query = from a in tablasWS.WS_DATOS_ENTRADA
                        where a.NEG_ID_CABECERA == NegId
                        select a;

            foreach (WS_DATOS_ENTRADA datos in query)
            {
                datos.ESTADO = 1;
            }

            tablasWS.SaveChanges();
        }

        public void updateFacturaEstadoReenvio(decimal NegId)
        {
            var query = from a in tablasWS.WS_DATOS_ENTRADA
                        where a.NEG_ID_CABECERA == NegId
                        select a;

            foreach (WS_DATOS_ENTRADA datos in query)
            {
                datos.ESTADO = 0;
            }

            tablasWS.SaveChanges();
        }

        public void updateAsignacionTareas(int opcionUpdate, int negID, int noUsuario, int idEtapa)
        {
            var query = from a in tablasWS.AsignacionTareas
                        where a.NegId == negID && a.Usuario == noUsuario && a.IdEtapa == idEtapa
                        select a;

            switch (opcionUpdate)
            {
                case 1:

                    foreach (AsignacionTareas datos in query)
                    {
                        datos.IdEstado = 30;
                    }
                    tablasWS.SaveChanges();

                    break;

                case 2:

                    foreach (AsignacionTareas datos in query)
                    {
                        datos.IdEstado = 141;
                    }
                    tablasWS.SaveChanges();

                    break;
            }
        }

        public List<Campos> obtenerCamposGrilla(int id_padre, int id_documento_actual)
        {
            var query = (from pro in tablasWS.Campos
                         join gDocumentos in tablasWS.GruposDocumentos on pro.GDocId equals gDocumentos.GDocId
                         where pro.idPadre == id_padre && gDocumentos.DocId == id_documento_actual
                         select pro).OrderBy(d => d.CampOrden);

            return query.ToList();
        }

        public List<CodigosCampo> consultarOpcionesLista(int campID)
        {
            var query = from pro in tablasWS.CodigosCampo
                        where pro.CampId == campID
                        select pro;

            return query.ToList();
        }

        public int obtenerLongitudCampo(int campID)
        {
            int query = Convert.ToInt32((from pro in tablasWS.Campos
                                         where pro.CampId == campID
                                         select pro.LongMax).FirstOrDefault());
            return query;
        }

        public string obtenerValorLista(int campID, string CodID)
        {
            string query = (from valCampo in tablasWS.CodigosCampo
                            where valCampo.CampId == campID && valCampo.CodId == CodID
                            select valCampo.CodDescripcion).FirstOrDefault();
            return query;
        }

        public bool validaLista(int campID)
        {
            var query = from pro in tablasWS.CodigosCampo
                        where pro.CampId == campID
                        select pro;

            List<CodigosCampo> listaCodigos = query.ToList();
            if (listaCodigos.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool validaAsignacionTareas(int idNegocio, int idEtapa)
        {
            var query = from pro in tablasWS.AsignacionTareas
                        where pro.NegId == idNegocio && pro.IdEtapa == idEtapa
                        select pro;

            List<AsignacionTareas> listaAsignacionTareas = query.ToList();
            if (listaAsignacionTareas.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Captura> obtenerCamposCCalidad(int numeroCaptura, int grupoDocumento, int NegId)
        {
            var query = (from captura in tablasWS.Captura
                         join campos in tablasWS.Campos on captura.CampId equals campos.CampId
                         where captura.Indice > 0 && campos.GDocId == grupoDocumento && captura.NumCaptura == numeroCaptura && captura.NegId == NegId && campos.ControlCalidad == true
                         select captura).OrderBy(d => d.Indice);

            return query.ToList();
        }

        public string obtenerNomCampo(int IdCampo)
        {
            string query = (from Nomcampos in tablasWS.Campos
                            where Nomcampos.CampId == IdCampo
                            select Nomcampos.CampDescripcion).FirstOrDefault();
            return query;
        }

        public string obtenerValoresCaptura(int idCampo)
        {
            string query = (from nomCampos in tablasWS.Captura
                            where nomCampos.CampId == idCampo
                            select nomCampos.NegValor).FirstOrDefault();
            return query;
        }

        public decimal obtenerNoUsuario(int idNegocio)
        {
            string query = (from noUsuario in tablasWS.AsignacionTareas
                             where noUsuario.NegId == idNegocio
                             select noUsuario.Usuario).FirstOrDefault().ToString();
            return Convert.ToDecimal(query);
        }

        public int obtenerTipoCampo(int idCampo)
        {
            int query = (from tcampo in tablasWS.Campos
                         where tcampo.CampId == idCampo
                         select tcampo.TcId).FirstOrDefault();
            return query;
        }

        public void volcarDatosCaptura_WebService(int idNegocio)
        {
            decimal? idNeg = idNegocio;
            int _resultado = tablasWS.spCargueTablasWS_SAP(idNeg);
        }
    }
}