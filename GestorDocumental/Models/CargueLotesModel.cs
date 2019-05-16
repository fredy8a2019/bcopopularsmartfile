using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorDocumental.Models
{
    public class CargueLotesModel
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();
        
        public void  ActualizarNegocioCargado(decimal negId)
        {
            var cargueLotes = db.CargueLotes.Where(c => c.NegId == negId).First();
            cargueLotes.ArchivoCargado = true;
            db.SaveChanges();
        }
    }
}