using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorDocumental.Models
{
    public class AuditoriaModel
    {

        private static GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>
        /// Crea un nuevo registro en la tabla auditoria 
        /// </summary>
        /// <param name="nueva"></param>
        public int AddAuditoria(Auditoria nueva)
        {
            db.AddToAuditoria(nueva);
           return  db.SaveChanges();
        }


    }
}