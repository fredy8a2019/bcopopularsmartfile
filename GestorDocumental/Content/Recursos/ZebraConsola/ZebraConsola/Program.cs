using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.SharpZebra.Printing;

namespace ZebraConsola
{
    /// <summary>
    /// Auto : Elena Parra
    /// Fecha : 03-01-2014
    /// Descripcion:  Programa para hacer un puente entre el aplicativo web y la impresora  
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            //var codigo = "26_0000001218";

            //string com1 = "^XA";
            //string com2 = "^FO140,35";
            //string txt3 = "^A0,20,15";
            //string com3 = "^FDMonterrey " + DateTime.Now.ToString() + "^FS";
            //string com8 = "^FO140,55";
            //string txt4 = "^A0,20,15";
            //string com9 = "^FDComercializadora Metrogas, S.A. de C.V^FS";
            //string com4 = "^FO140,80^BY2";
            //string com5 = "^BCN,90,Y,N,N";
            //string com6 = "^FD" + codigo + "^FS";
            //string com7 = "^PQ1,0,1Y^XZ";

            //string zebraInstructions = string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}\r\n{6}\r\n{7}\r\n{8}\r\n{9}\r\n{10}\r\n",
            //                                          com1, com2, txt3, com3, com8, txt4, com9, com4, com5, com6, com7);

            //string selectedPrinterName = @"\\7.215.100.113\Zebra"; //@"\\10.215.129.13\Zebra"; //Nombre de la impresora Zebra

            //Print(zebraInstructions, selectedPrinterName);

            Print(args[0], args[1]);
        }

        /// <summary>
        /// Utiliza la dll SharpZebra Para imprimir desde la aplicacion web
        /// </summary>
        /// <param name="zebraInstructions"></param>
        /// <param name="selectedPrinterName"></param>
        private static void Print(string zebraInstructions, string selectedPrinterName)
        {

            new ZebraPrinter(selectedPrinterName).Print(zebraInstructions); //Metodo de las DLL
        }
    }
}
