using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using GestorDocumental.Models;

namespace GestorDocumental.WebService
{
    public class CrearXML
    {
        public static string formateaEtiqueta(string dato)
        {
            if (dato == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(dato.Trim()) || string.IsNullOrWhiteSpace(dato.Trim()) || dato == null)
            {
                return null;
            }
            else
            {
                return dato.Trim();
            }
        }

        public static string CrearXMLFile(int negID)
        {
            try
            {
                string rutaFinalArchivo = "";
                WebServiceModel ws = new WebServiceModel();

                List<DatosEntrada> Y_ConsoltaFactura = new List<DatosEntrada>();
                List<Factura_cabecera> facturaCabecera = new List<Factura_cabecera>();
                List<Factura_posiciones> facturaPosiciones = new List<Factura_posiciones>();
                List<Factura_impuestos> facturaImpuestos = new List<Factura_impuestos>();

                int cont = 0;
                string nombreFactura = "";

                Y_ConsoltaFactura = ws.obtenerDatosEntrada(negID);
                foreach (DatosEntrada datos in Y_ConsoltaFactura)
                {
                    facturaCabecera = ws.obtenerDatosFacturaCabecera(datos.id_neg_cabecera);
                    facturaPosiciones = ws.obtenerDatosFacturaPosiciones(datos.id_neg_cabecera);
                    facturaImpuestos = ws.obtenerDatosFacturaImpuestos(datos.id_neg_cabecera);

                    XElement FacturaXML = new XElement("YCONSOLA_CARGA_WS",
                    new XAttribute(XNamespace.Xmlns + "n0", "urn:sap-com:document:sap:rfc:functions"));

                    for (int i = 0; i < 1; i++)
                    {
                        XElement accion = new XElement("Accion".ToUpper(), Y_ConsoltaFactura[cont].accion.Trim());
                        FacturaXML.Add(accion);

                        for (int a = 0; a < facturaCabecera.Count; a++)
                        {
                            string fecha = string.Format("{0:yyyy-MM-dd}", facturaCabecera[a].doc_date);

                            XElement FactCabecera;

                            FactCabecera = new XElement("Factura_cabecera".ToUpper(),
                                     new XElement("Fact_id".ToUpper(), formateaEtiqueta(facturaCabecera[a].fac_id.Trim())),
                                     new XElement("doc_type".ToUpper(), formateaEtiqueta(facturaCabecera[a].doc_type.Trim())),
                                     new XElement("doc_number".ToUpper(), formateaEtiqueta(facturaCabecera[a].doc_number.Trim())),
                                     new XElement("doc_date".ToUpper(), formateaEtiqueta(fecha)),
                                     new XElement("vendor_rfc".ToUpper(), formateaEtiqueta(facturaCabecera[a].vendor_rfc.Trim())),
                                     new XElement("company_rfc".ToUpper(), formateaEtiqueta(facturaCabecera[a].company_rfc.Trim())),
                                     new XElement("total_amount".ToUpper(), facturaCabecera[a].total_amount),
                                     new XElement("currency_code".ToUpper(), formateaEtiqueta(facturaCabecera[a].currency_code.Trim())),
                                     formateaEtiqueta(facturaCabecera[a].employee_number.ToString()) == null ? new XElement("employee_number".ToUpper()) : new XElement("employee_number".ToUpper(), facturaCabecera[a].employee_number),
                                     formateaEtiqueta(facturaCabecera[a].purchase_order.Trim()) == null ? new XElement("purchase_order".ToUpper()) : new XElement("purchase_order".ToUpper(), facturaCabecera[a].purchase_order.Trim()),
                                     formateaEtiqueta(facturaCabecera[a].discount_amount.ToString()) == null ? new XElement("discount_amount".ToUpper()) : new XElement("discount_amount".ToUpper(), facturaCabecera[a].discount_amount),
                                     formateaEtiqueta(facturaCabecera[a].withholding_tax.ToString()) == null ? new XElement("witholding_tax".ToUpper()) : new XElement("witholding_tax".ToUpper(), facturaCabecera[a].withholding_tax),
                                     formateaEtiqueta(facturaCabecera[a].advance_flag) == null ? new XElement("advance_flag".ToUpper()) : new XElement("advance_flag".ToUpper(), formateaEtiqueta(facturaCabecera[a].advance_flag.Trim())),
                                        //NUEVA ETIQUETA PARA EL CAMBIO DE COSTARICA Y PANAMA.
                                     formateaEtiqueta(facturaCabecera[a].fiscal_doc_number) == null ? new XElement("fiscal_doc_number".ToUpper()) : new XElement("fiscal_doc_number".ToUpper(), formateaEtiqueta(facturaCabecera[a].fiscal_doc_number.Trim())));
                            FacturaXML.Add(FactCabecera);

                            nombreFactura = facturaCabecera[a].fac_id.Trim();
                        }

                        XElement FactImpuestos = new XElement("Factura_impuestos".ToUpper());
                        for (int s = 0; s < facturaImpuestos.Count; s++)
                        {
                            XElement itemsFactImpuestos = new XElement("item",
                                new XElement("vat_base_amount".ToUpper(), facturaImpuestos[s].vat_base_amount),
                                new XElement("vat_percentage".ToUpper(), facturaImpuestos[s].vat_percentage),
                                new XElement("vat_tax_amount".ToUpper(), facturaImpuestos[s].vat_tax_amount),
                                formateaEtiqueta(facturaImpuestos[s].vat_tax_id) == null ? new XElement("vat_tax_id".ToUpper().Trim()) : new XElement("vat_tax_id".ToUpper().Trim(), formateaEtiqueta(facturaImpuestos[s].vat_tax_id))
                                );

                            FactImpuestos.Add(itemsFactImpuestos);
                        }
                        FacturaXML.Add(FactImpuestos);

                        #region Convertir el archivo PDF a un string Base64
                        byte[] archivoPDF = convertirArchivo(Y_ConsoltaFactura[cont].facturaPDFBase64);

                        string hex = "";
                        if (archivoPDF != null)
                        {
                            hex = Convert.ToBase64String(archivoPDF);
                        }

                        string nombreArchivo = "@" + Y_ConsoltaFactura[cont].facturaPDFBase64;
                        string extensionArchivo = Path.GetExtension(nombreArchivo);

                        XElement FactPDF = new XElement("factura_pdf_base64".ToUpper(), new XCData(hex));
                        FacturaXML.Add(FactPDF);

                        if (hex != "")
                        {
                            GuardarArchivo(archivoPDF, nombreFactura.ToString(), extensionArchivo);
                        }
                        #endregion Fin de la conversion del archivo PDF

                        XElement FactPosiciones = new XElement("Factura_posiciones".ToUpper());
                        for (int e = 0; e < facturaPosiciones.Count; e++)
                        {
                            string fecha = string.Format("{0:yyyy-MM-dd}", facturaPosiciones[e].deliveryDate);

                            XElement itemsPosiciones = new XElement("item",
                                 formateaEtiqueta(facturaPosiciones[e].delivery_note.Trim()) == null ? new XElement("delivery_note".ToUpper()) : new XElement("delivery_note".ToUpper(), formateaEtiqueta(facturaPosiciones[e].delivery_note.Trim())),
                                 formateaEtiqueta(fecha) == null ? new XElement("delivery_date".ToUpper()) : new XElement("delivery_date".ToUpper(), formateaEtiqueta(fecha)),
                                 formateaEtiqueta(facturaPosiciones[e].item_text.Trim()) == null ? new XElement("item_text".ToUpper()) : new XElement("item_text".ToUpper(), formateaEtiqueta(facturaPosiciones[e].item_text.Trim())),
                                 new XElement("item_quantity".ToUpper(), facturaPosiciones[e].item_quantity),
                                 new XElement("item_price".ToUpper(), facturaPosiciones[e].item_price),
                                 new XElement("item_amount".ToUpper(), facturaPosiciones[e].item_amount),
                                 new XElement("item_percentage".ToUpper(), facturaPosiciones[e].item_percentage));
                            FactPosiciones.Add(itemsPosiciones);
                        }
                        FacturaXML.Add(FactPosiciones);

                        XElement land = new XElement("Land1".ToUpper(), Y_ConsoltaFactura[cont].land1.Trim());
                        FacturaXML.Add(land);

                        XElement origen = new XElement("origen".ToUpper(), Y_ConsoltaFactura[cont].origen.Trim());
                        FacturaXML.Add(origen);
                    }
                    string rutaFacturas = System.Configuration.ConfigurationManager.AppSettings["RutaFacturasXML"].ToString();
                    string nomArchivo = nombreFactura;

                    string archivo = rutaFacturas + "FACT_" + nomArchivo + ".xml";
                    FacturaXML.Save(archivo);

                    //Update el estado de la factura a 1
                    decimal negId = Y_ConsoltaFactura[cont].id_neg_cabecera;
                    ws.updateFacturaEstado(negId);

                    cont++;

                    reemplazarArchivo(archivo);
                    rutaFinalArchivo = archivo;
                }
                return rutaFinalArchivo;
            }
            catch (Exception ex)
            {
                return "Error al crear el fichero XML: " + ex.Message;
            }
        }

        #region Convertir al archivo PDF a bytes
        public static byte[] convertirArchivo(string rutaArchivo)
        {
            if (File.Exists(rutaArchivo))
            {
                FileStream fs = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
                byte[] arregloBytes = new byte[fs.Length];

                fs.Read(arregloBytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                return arregloBytes;
            }
            else
            {
                try
                {
                    SautinSoft.PdfVision v = new SautinSoft.PdfVision();
                    v.Serial = "10360615139";

                    string rutaTif = rutaArchivo.Replace(".pdf", ".TIF");

                    byte[] bytesTiff = File.ReadAllBytes(rutaTif);
                    byte[] bytesPDF = v.ConvertImageStreamToPdfStream(bytesTiff);

                    BinaryWriter writer = null;
                    writer = new BinaryWriter(File.OpenWrite(rutaArchivo));

                    writer.Write(bytesPDF);
                    writer.Flush();
                    writer.Close();

                    return bytesPDF;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        #endregion

        #region Para guardar el archivo XML en una ruta especifica
        public static bool GuardarArchivo(byte[] data, string nombreArchivo, string extensionArchivo)
        {
            BinaryWriter writer = null;
            string rutaFacturas = System.Configuration.ConfigurationManager.AppSettings["RutaFacturasPDF"].ToString();

            string archivo = rutaFacturas + "FACT_" + nombreArchivo + extensionArchivo;

            try
            {
                writer = new BinaryWriter(File.OpenWrite(archivo));

                writer.Write(data);
                writer.Flush();
                writer.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Para formatear el archivo XML
        public static void reemplazarArchivo(string ruta)
        {
            int noLinea = 1;
            string textoAreemplazarInicio = "<urn:YCONSOLA_CARGA_WS>";
            string reemplazoUltimaLinea = "</urn:YCONSOLA_CARGA_WS>";

            string line = null;
            string rutaFacturas = System.Configuration.ConfigurationManager.AppSettings["RutaFacturasXML"].ToString();

            string archivoTemporal = rutaFacturas + "Temporal.xml";

            string[] totalLineas = File.ReadAllLines(ruta);
            int ultimaLinea = totalLineas.Length;

            using (TextReader tr = File.OpenText(ruta))
            {
                using (TextWriter tw = File.CreateText(archivoTemporal))
                {
                    while ((line = tr.ReadLine()) != null)
                    {
                        if (line != "<?xml version=\"1.0\" encoding=\"utf-8\"?>")
                        {
                            if (noLinea == 1)
                            {
                                tw.WriteLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">");
                                tw.WriteLine("<soapenv:Header/>");
                                tw.WriteLine("<soapenv:Body>");
                            }

                            if (line == "<YCONSOLA_CARGA_WS xmlns:n0=\"urn:sap-com:document:sap:rfc:functions\">")
                            {
                                line = line.Replace("<YCONSOLA_CARGA_WS xmlns:n0=\"urn:sap-com:document:sap:rfc:functions\">", textoAreemplazarInicio);
                            }

                            if (line == "</YCONSOLA_CARGA_WS>")
                            {
                                line = line.Replace("</YCONSOLA_CARGA_WS>", reemplazoUltimaLinea);
                            }

                            tw.WriteLine(line);
                            noLinea++;
                        }
                    }
                    tw.WriteLine("</soapenv:Body>");
                    tw.WriteLine("</soapenv:Envelope>");
                }
            }
            File.Delete(ruta);
            File.Move(archivoTemporal, ruta);
        }
        #endregion
    }
}