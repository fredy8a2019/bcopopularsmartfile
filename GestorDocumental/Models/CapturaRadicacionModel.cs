using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

namespace GestorDocumental.Models
{
    /// <summary>
    /// Autor :Elena Parra Cedeño
    /// Fecha : 28 - 11 --2013
    /// Descripcion : Logica necesaria para administrar los registro de la tabla camputra radicacion 
    /// </summary>
    public class CapturaRadicacionModel
    {
        protected GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>
        /// Hace una un registro masivo de lo que se captura en radicacion
        /// </summary>
        /// <param name="lstCaptura"></param>
        public void InsertarCapturaRadicacion(List<CapturaRadicacion> lstCaptura)
        {
            try
            {
                //using (TransactionScope scope = new TransactionScope())
                //{
                    foreach (CapturaRadicacion captura in lstCaptura)
                    {
                        this.db.AddToCapturaRadicacion(captura);
                        this.db.SaveChanges();
                    }
                //    scope.Complete();
                //}
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaRadicionModel metodo InsertarCapturaRadicaicon  " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }
    }
}