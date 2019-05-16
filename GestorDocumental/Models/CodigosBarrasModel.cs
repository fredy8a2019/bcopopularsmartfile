using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorDocumental.Models
{
    public class CodigosBarrasModel
    {
        private GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>
        /// Crea un nuevo registro en la tabla CodigosCampos
        /// </summary>
        /// <param name="nueva"></param>
        public int AddCodigosBarras(CodigosBarras nueva)         
        {
            db.AddToCodigosBarras(nueva);
            return db.SaveChanges();
        }

        public void update(long idCodbarras) 
        {
            CodigosBarras update = db.CodigosBarras.Where(x => x.IdCodBarras == idCodbarras).First();
            update.estado = 0;
            update.FechaImpreso = null;
            update.CodigoZPL = update.CodigoZPL.Replace("PQ2", "PQ1");  
            db.SaveChanges();
        }
    }
}