using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorDocumental.Models;

namespace GestorDocumental.Models
{
    public class LoteoCambiosModel
    {
        GestorDocumentalEnt basdat = new GestorDocumentalEnt();

        public void insert(LoteoCambios data)
        {
            basdat.AddToLoteoCambios(data);
            basdat.SaveChanges();
        }

        public IEnumerable<LoteoCambios> selectCambiosImagen(int id)
        {   
            IEnumerable<LoteoCambios> cambios = null;
            cambios = basdat.LoteoCambios.Where(c => c.idImagen == id).ToList();
            return cambios;
        }

        public void deleteCambios(int id)
        {
            IEnumerable<LoteoCambios> cambios = this.selectCambiosImagen(id);

            foreach (LoteoCambios cambio in cambios)
            {
                basdat.DeleteObject(cambio);
            }

            basdat.SaveChanges();
        }

    }
}