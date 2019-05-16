using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActualizarListas.Clases;
using ActualizarListas.Models;
using System.IO;

namespace ActualizarListas
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CrearArchivo opCrear = new CrearArchivo();
                GestorDocumentalModel op = new GestorDocumentalModel();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Seleccione: \n 1. Archivo Listas.js \n 2. Archivo ListasCiudades.js \n 3. Archivo ListasPadres.js");
                int seleccion = Convert.ToInt32(Console.ReadLine());
                switch (seleccion)
                {
                    case 1:
                        List<int> lista = opCrear.consultarTablas();

                        FileStream stream = new FileStream(@args[0] + "Listas.js", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(stream);

                        //****************************************************
                        writer.WriteLine("function obtenerListas(idCampo) {");
                        writer.WriteLine("    switch (idCampo) {");

                        foreach (int datos in lista)
                        {
                            string descripciones = opCrear.consultarDescripcionesCampo(datos);

                            writer.WriteLine();
                            writer.WriteLine("          case \"" + datos + "\": ");
                            writer.WriteLine("              var dato" + datos + " = \"" + descripciones + "\"" + ";");
                            writer.WriteLine("              return dato" + datos + ";");
                            writer.WriteLine("              break;");
                        }
                        writer.WriteLine("       }");
                        writer.WriteLine("}");

                        //****************************************************
                        writer.WriteLine("function obtenerListasIndice(idCampo) {");
                        writer.WriteLine("    switch (idCampo) {");

                        foreach (int datos in lista)
                        {
                            string descripciones = opCrear.consultarIndicesCampos(datos);

                            writer.WriteLine();
                            writer.WriteLine("          case \"" + datos + "\": ");
                            writer.WriteLine("              var dato" + datos + " = \"" + descripciones + "\"" + ";");
                            writer.WriteLine("              return dato" + datos + ";");
                            writer.WriteLine("              break;");
                        }
                        writer.WriteLine("       }");
                        writer.WriteLine("}");

                        writer.Close();
                        break;

                    case 2:
                        List<int> listaCiudades = opCrear.consultarIdDeptos();

                        FileStream streamCiudaes = new FileStream(@args[0] + "ListasCiudades.js", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter writerCiudades = new StreamWriter(streamCiudaes);

                        //*****************************************************
                        writerCiudades.WriteLine("function obtenerListasCiudades(idCampo) {");
                        writerCiudades.WriteLine("    switch (idCampo) {");

                        foreach (int datos in listaCiudades)
                        {
                            string descripciones = opCrear.consultarDescripcionesCiudades(datos);

                            writerCiudades.WriteLine();
                            writerCiudades.WriteLine("          case \"" + datos + "\": ");
                            writerCiudades.WriteLine("              var dato" + datos + " = \"" + descripciones + "\"" + ";");
                            writerCiudades.WriteLine("              return dato" + datos + ";");
                            writerCiudades.WriteLine("              break;");
                        }
                        writerCiudades.WriteLine("       }");
                        writerCiudades.WriteLine("}");

                        //****************************************************
                        writerCiudades.WriteLine("function obtenerListasIndiceCiudades(idCampo) {");
                        writerCiudades.WriteLine("    switch (idCampo) {");

                        foreach (int datos in listaCiudades)
                        {
                            string descripciones = opCrear.consultarIndicesCiudades(datos);

                            writerCiudades.WriteLine();
                            writerCiudades.WriteLine("          case \"" + datos + "\": ");
                            writerCiudades.WriteLine("              var dato" + datos + " = \"" + descripciones + "\"" + ";");
                            writerCiudades.WriteLine("              return dato" + datos + ";");
                            writerCiudades.WriteLine("              break;");
                        }
                        writerCiudades.WriteLine("       }");
                        writerCiudades.WriteLine("}");

                        writerCiudades.Close();
                        break;

                    case 3:
                        List<string> listaCodPadres = opCrear.consultar_IDCODPadres();

                        FileStream streamCodPadres = new FileStream(@args[0] + "ListasPadre.js", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter writerCodPadres = new StreamWriter(streamCodPadres);

                        //*****************************************************
                        writerCodPadres.WriteLine("function obtenerListasPadre(idCampo) {");
                        writerCodPadres.WriteLine("    switch (idCampo) {");

                        foreach (string datos in listaCodPadres)
                        {
                            string descripciones = opCrear.consultarDescripcionesValores(datos);

                            writerCodPadres.WriteLine();
                            writerCodPadres.WriteLine("          case \"" + datos + "\": ");
                            writerCodPadres.WriteLine("              var dato" + datos + " = \"" + descripciones + "\"" + ";");
                            writerCodPadres.WriteLine("              return dato" + datos + ";");
                            writerCodPadres.WriteLine("              break;");
                        }
                        writerCodPadres.WriteLine("       }");
                        writerCodPadres.WriteLine("}");

                        //****************************************************
                        writerCodPadres.WriteLine("function obtenerListasIndicePadre(idCampo) {");
                        writerCodPadres.WriteLine("    switch (idCampo) {");

                        foreach (string datos in listaCodPadres)
                        {
                            string descripciones = opCrear.consultar_IDValores(datos);

                            writerCodPadres.WriteLine();
                            writerCodPadres.WriteLine("          case \"" + datos + "\": ");
                            writerCodPadres.WriteLine("              var dato" + datos + " = \"" + descripciones + "\"" + ";");
                            writerCodPadres.WriteLine("              return dato" + datos + ";");
                            writerCodPadres.WriteLine("              break;");
                        }
                        writerCodPadres.WriteLine("       }");
                        writerCodPadres.WriteLine("}");

                        writerCodPadres.Close();
                        break;
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }
    }
}
