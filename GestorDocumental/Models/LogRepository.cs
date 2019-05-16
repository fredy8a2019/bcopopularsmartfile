using System;
using System.IO;
using System.Web.Mvc;
using System.Web;
using GestorDocumental.Controllers;
using System.Linq;

namespace GestorDocumental.Models
{
    public static class LogRepository
    {
        private static GestorDocumentalEnt bd = new GestorDocumentalEnt();

        public static void registro(string mensaje)
        {
            try
            {   
                //Pass the filepath and filename to the StreamWriter Constructor
                
                Parametros param1 = bd.Parametros.First(c => c.codigo == "PATH_LOG");
                StreamWriter sw = new StreamWriter(param1.valor,true);

                //Write a line of text
                sw.WriteLine(DateTime.Now+" "+mensaje);

                //Close the file
                sw.Close();
                bd.Connection.Close();
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }
        }

        public static void registroCaptura(string mensaje)
        {
            //====== Creado por: Juliana Pancho; 08/FEB/2017 ===================== 
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor  

                Parametros param1 = bd.Parametros.First(c => c.codigo == "PATH_LOG_CAPTURA");
                StreamWriter sw = new StreamWriter(param1.valor, true);

                //Write a line of text 
                sw.WriteLine(DateTime.Now + " " + mensaje);

                //Close the file 
                sw.Close();
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: " + e.Message); 
            }
            finally
            {
                //Console.WriteLine("Executing finally block."); 
            }
        }


    }
}