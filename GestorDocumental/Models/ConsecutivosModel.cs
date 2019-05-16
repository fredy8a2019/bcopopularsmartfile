using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorDocumental.Models;

namespace GestorDocumental.Models
{
    public class ConsecutivosModel
    {
        static GestorDocumentalEnt basdat = new GestorDocumentalEnt();

        public static int getConsecutivo(string cod)
        {
            //OBTENGO EL RESGISTRO A ACTUALIZAR
            Consecutivos llave = basdat.Consecutivos.First(c => c.ConCodigo == cod);

            //OBTENGO LA LLAVE
            int key = (int)llave.ConValor;

            //INCREMENTO EL CONSECUTIVO
            llave.ConValor = key + 1;
            try
            {
                basdat.SaveChanges();
            }
            catch (Exception es)
            {
                return key;
            }

            return key;

        }

        /// <summary>
        /// Genera el codigo de barras para cada pais dependiendo el cliente que este loggeado
        /// </summary>
        /// <param name="codPais"></param>
        /// <param name="CodParametro"></param>
        /// <returns></returns>
        public static string ConsecutivoRadicado(string codPais, string CodParametro)
        {
            string consecutivoRecepcion = string.Format("{0:0000000000}", getConsecutivo(CodParametro));
            return codPais + "_" + consecutivoRecepcion;
        }
    }
}