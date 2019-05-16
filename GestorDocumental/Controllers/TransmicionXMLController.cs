using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class TransmicionXMLController : Controller
    {
        GestorDocumentalEnt tablasWS = new GestorDocumentalEnt();

        public string ExecuteWebService(string rutaXML)
        {
            HttpWebRequest request = CreateWebRequest();
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.Load(rutaXML);

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                    return soapResult;
                }
            }
        }

        private HttpWebRequest CreateWebRequest()
        {
            try
            {
                string urlWebService = (from url in tablasWS.Parametros
                                        where url.codigo == "URL_WS"
                                        select url.valor).FirstOrDefault();

                //la URL del webService se debe traer ya sea por Web Config o por la tabla parametros
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@urlWebService);
                webRequest.Headers.Add(@"SOAP:Action");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";
                return webRequest;
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en TransmicionXMLController metodo CreateWebRequest " + e.Message + " stack trace " + e.StackTrace);
                throw e;
            }
        }

        public string guardarResultadoSAP(string resultadoSAP, string usuario, string negID)
        {
            try
            {
                string ArchivoXml = resultadoSAP;
                string resultadoRCODE = "";

                //*****************************************************************
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(ArchivoXml);

                XmlNamespaceManager ns = new XmlNamespaceManager(xml.NameTable);
                ns.AddNamespace("soap-env", "http://schemas.xmlsoap.org/soap/envelope/");
                ns.AddNamespace("n0", "urn:sap-com:document:sap:rfc:functions");

                List<WS_RESULTADO> listaResultado = new List<WS_RESULTADO>();
                XmlNodeList xnLista = xml.SelectNodes("/soap-env:Envelope/soap-env:Body/n0:YCONSOLA_CARGA_WSResponse/IT_RESULTADO/item", ns);
                foreach (XmlNode item in xnLista)
                {
                    WS_RESULTADO datosResultado = new WS_RESULTADO
                    {
                        NEG_ID_CABECERA = Convert.ToDecimal(negID),
                        OK_CODE = item["OK_CODE"].InnerText,
                        DESCRIPTION = item["DESCRIPCION"].InnerText,
                        FECHA_RESPUESTA = DateTime.Now
                    };

                    XmlNodeList xnLista2 = xml.SelectNodes("/soap-env:Envelope/soap-env:Body/n0:YCONSOLA_CARGA_WSResponse", ns);
                    foreach (XmlNode item2 in xnLista2)
                    {
                        datosResultado.RCODE = item2["RCODE"].InnerText;
                        resultadoRCODE = item2["RCODE"].InnerText;
                    }
                    listaResultado.Add(datosResultado);
                }
                //*****************************************************************

                //Instancia para guardar el resultado
                GestorDocumentalEnt data = new GestorDocumentalEnt();
                //Realizar el insert a la tabla de resultados.
                for (int i = 0; i < listaResultado.Count; i++)
                {
                    WS_RESULTADO dataResultado = new WS_RESULTADO
                    {
                        NEG_ID_CABECERA = listaResultado[i].NEG_ID_CABECERA,
                        RCODE = listaResultado[i].RCODE,
                        OK_CODE = listaResultado[i].OK_CODE,
                        DESCRIPTION = listaResultado[i].DESCRIPTION,
                        FECHA_RESPUESTA = listaResultado[i].FECHA_RESPUESTA
                    };

                    data.AddToWS_RESULTADO(dataResultado);
                    data.SaveChanges();
                }

                WebServiceModel wsOp = new WebServiceModel();
                bool resultadoValidacion = wsOp.validaAsignacionTareas(Convert.ToInt32(negID), 60);

                string resultadoFinal = "";
                switch (resultadoRCODE)
                {
                    //Cuando es Cero(0), Significa que la factura ser guardo correctamente en SAP
                    case "0":
                        if (resultadoValidacion)
                        {
                            wsOp.updateAsignacionTareas(1, Convert.ToInt32(negID), Convert.ToInt32(usuario), 60);
                            resultadoFinal = "La factura ha sido transmitida correctamente a SAP";
                        }
                        else
                        {
                            AsignacionTareas dataAsignacionOK = new AsignacionTareas
                            {
                                NegId = Convert.ToDecimal(negID),
                                IdEtapa = 60,
                                Usuario = Convert.ToDecimal(usuario),
                                HoraInicio = DateTime.Now,
                                HoraTerminacion = DateTime.Now,
                                IdEstado = 30
                            };

                            data.AddToAsignacionTareas(dataAsignacionOK);
                            data.SaveChanges();

                            resultadoFinal = "La factura ha sido transmitida correctamente a SAP";
                        }
                        break;

                    //Cuando el Noventa y Nueve(99), Significa que la factura no se guardo correctamente y arrojo errores.
                    case "99":
                        if (resultadoValidacion)
                        {
                            wsOp.updateAsignacionTareas(2, Convert.ToInt32(negID), Convert.ToInt32(usuario), 60);
                            resultadoFinal = "La factura no ha sido transmitida correctamente a SAP";
                        }
                        else
                        {
                            AsignacionTareas dataAsignacionError = new AsignacionTareas
                            {
                                NegId = Convert.ToDecimal(negID),
                                IdEtapa = 60,
                                Usuario = Convert.ToDecimal(usuario),
                                HoraInicio = DateTime.Now,
                                HoraTerminacion = DateTime.Now,
                                IdEstado = 141
                            };

                            data.AddToAsignacionTareas(dataAsignacionError);
                            data.SaveChanges();
                            resultadoFinal = "La factura no ha sido transmitida correctamente a SAP";
                        }
                        break;
                }

                return resultadoFinal;
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en TransmicionXMLController metodo guardarResultadoSAP " + e.Message + " stack trace " + e.StackTrace);
                throw e;
            }

        }
    }
}