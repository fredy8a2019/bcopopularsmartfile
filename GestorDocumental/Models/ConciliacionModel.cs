using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorDocumental.Models;

namespace GestorDocumental.Models
{
    public class ConciliacionModel
    {
        GestorDocumentalEnt basdat = new GestorDocumentalEnt();

        public void insertMultiple(List<Conciliacion> data)
        {
            foreach (var concilia in data)
            {
                basdat.AddToConciliacion(concilia);
            }
            
            basdat.SaveChanges();
        }

        public int getConsecutivo()
        {
            //OBTENGO EL RESGISTRO A ACTUALIZAR
            Consecutivos llave = basdat.Consecutivos.First(c => c.ConCodigo == "KEY_CONC");
          
            //OBTENGO LA LLAVE
            int key=(int)llave.ConValor;

            //INCREMENTO EL CONSECUTIVO
            llave.ConValor = key+1;
            basdat.SaveChanges();

            return key;
        }

        //public IEnumerable<LoteoCambios> selectCambiosImagen(int id)
        //{
        //    IEnumerable<LoteoCambios> cambios = null;
        //    cambios = basdat.LoteoCambios.Where(c => c.idImagen == id).ToList();
        //    return cambios;
        //}

        //public void deleteCambios(int id)
        //{
        //    IEnumerable<LoteoCambios> cambios = this.selectCambiosImagen(id);

        //    foreach (LoteoCambios cambio in cambios)
        //    {
        //        basdat.DeleteObject(cambio);
        //    }

        //    basdat.SaveChanges();
        //}

    }
}