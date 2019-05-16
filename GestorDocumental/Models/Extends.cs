using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.SharpZebra.Printing;
using System.Diagnostics;

namespace GestorDocumental.Models
{
    public class Extends
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>
        /// Permite obtener el valor de la propiedad basado en reflexión
        /// </summary>
        /// <param name="obj">recibe el objeto</param>
        /// <param name="nameProperty">recibe el nombre de la propiedad</param>
        /// <returns>valor de la propiedad</returns>
        public static object ObtenerValorReflexion(dynamic obj, string nameProperty)
        {
            return obj.GetType().GetProperty(nameProperty).GetValue(obj, new object[] { });
        }

        /// <summary>
        /// Convierte una imagen pdf a una multi tiff
        /// </summary>
        /// <param name="pdfPath">Ruta del archivo origen </param>
        /// <param name="tiffPath">Ruta del archivo destino con extencion tiff</param>
        public static int ConvertirimagenPDFaTIFF(string pdfPath, string tiffPath)
        {
            //modifica William Cicua; 05/05/2016; se crea parametro que activa u desactiva la 
            // conversion a .tif
            GestorDocumentalEnt db = new GestorDocumentalEnt();
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.Serial = "10160711675";   //Serial develop 
            //f.Serial = "10160735830"; //Serial Server 
            f.OpenPdf(pdfPath);
            if (f.PageCount > 0)
            {
                f.ImageOptions.Dpi = 200;
                var algo = f.PageCount;
                string tifactivo1 = db.Parametros.Where(c => c.codigo == "CONVERSION_A_TIFF_ACTIVO").First().valor;

                int tifactivo = Convert.ToInt32(tifactivo1);
                if (tifactivo == 1)
                {
                    if (f.ToMultipageTiff(tiffPath, System.Drawing.Imaging.EncoderValue.CompressionCCITT4) == 0)
                    {
                        //System.Diagnostics.Process.Start(tiffPath);
                    }
                }
            }
            return f.PageCount;
        }

        public void ConvertirImagenTifaPDF(string INtifPath, string OUTpdfPath)
        {
            SautinSoft.PdfVision f = new SautinSoft.PdfVision();
            f.Serial = "10360634037";     //Serial develop 
            //f.Serial = "10160735830";   //Serial Server
            f.ConvertImageFileToPDFFile(INtifPath, OUTpdfPath);
        }

        /// <summary>
        /// Crear e imprime el codigo de barras 
        /// </summary>
        public bool CrearEImprimirEtiqueta(string oficina, string fecha, string sociedad, string codigo, string nombreImpresora, decimal? usuario, string idRadicacion)
        {
            LogRepository.registro("ENTRA AL METODO CrearEImprimirEtiqueta ");

            bool respuesta = false;
            var estiloLetra = db.Parametros.Where(x => x.codigo == "ZPL_CSSLETRA").First().valor;
            var texto1Posicion = db.Parametros.Where(x => x.codigo == "COD_POSIT_TXT1").First().valor;
            var texto2Posicion = db.Parametros.Where(x => x.codigo == "COD_POSIT_TXT2").First().valor;
            var codigoPosicion = db.Parametros.Where(x => x.codigo == "COD_POSIT_COD").First().valor;
            var codigoDraw = db.Parametros.Where(x => x.codigo == "COD_STANDAR").First().valor;
            var cantidadPrint = db.Parametros.Where(x => x.codigo == "COD_CANT_PRINT").First().valor;

            var margenIzquierda = db.ImpresorasZebra.Where(x => x.impresora == nombreImpresora.Trim()).FirstOrDefault().izquierda;

            string com1 = "^XA";
            string com2 = texto1Posicion.Replace("&", margenIzquierda.ToString().Trim());           //"^FO" + valorLeft + ",35";//"^FO140,35"
            string txt3 = estiloLetra;
            string com3 = "^FD" + oficina + "  " + fecha + "^FS";
            string com8 = texto2Posicion.Replace("&", margenIzquierda.ToString().Trim());           //"^FO" + valorLeft + ",55";//"^FO140,55"
            string txt4 = estiloLetra;
            string com9 = "^FD" + sociedad + "^FS";
            string com4 = codigoPosicion.Replace("&", margenIzquierda.ToString().Trim());  //"^FO" + valorLeft + ",80^BY2";//"^FO140,80^BY2"
            string com5 = codigoDraw;      //"^BCN,90,Y,N,N";
            string com6 = "^FD" + codigo + "^FS";
            string com7 = "^PQ" + cantidadPrint + ",0,1Y^XZ";


            string zebraInstructions = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                                      com1, com2, txt3, com3, com8, txt4, com9, com4, com5, com6, com7);

            string selectedPrinterName = nombreImpresora; //Nombre de la impresora Zebra

            // new ZebraPrinter(selectedPrinterName).Print(zebraInstructions); //Metodo de las DLL
            LogRepository.registro("el codigo completo de la zebra" + zebraInstructions);

            CodigosBarrasModel codModel = new CodigosBarrasModel();
            CodigosBarras nuevo = new CodigosBarras();
            nuevo.CodigoZPL = zebraInstructions;
            nuevo.estado = 0;
            nuevo.impresora = nombreImpresora;
            nuevo.usuario = usuario;
            nuevo.IdRadicacion = long.Parse(idRadicacion);
            var resultado = codModel.AddCodigosBarras(nuevo);
            LogRepository.registro("Guardar la informacion" + respuesta);
            if (resultado >= 1)
            {
                respuesta = true;
            }
            else
            {
                respuesta = false;
            }
            return respuesta;
        }

        /// <summary>
        /// Valida la extencion de archivos que puede cargar el componente upload
        /// de la aplicaicon
        /// </summary>
        public static bool ValidarExtencion(string nameFile)
        {
            GestorDocumentalEnt db = new GestorDocumentalEnt();
            var extencionArchivo = db.Parametros.Where(x => x.codigo == "EXTEND").First().valor;
            var arry = extencionArchivo.Split(',');
            var extArchiEntrada = nameFile.Split('.');
            bool ArchCorrecto = false;
            for (int i = 0; i < arry.Length; i++)
            {
                if (arry[i] == extArchiEntrada[1] || arry[i] == "")
                {
                    ArchCorrecto = true;
                    return ArchCorrecto;
                }
                else
                {
                    ArchCorrecto = false;
                }
            }
            return ArchCorrecto;
        }
    }
}