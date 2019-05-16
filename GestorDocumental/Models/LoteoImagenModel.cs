using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorDocumental.Models;

namespace GestorDocumental.Models
{
    public class LoteoImagenModel
    {
        GestorDocumentalEnt basdat = new GestorDocumentalEnt();
        
        public int insertImagen(LoteoImagen data)
        {
            try
            {
                basdat.AddToLoteoImagen(data);
                basdat.SaveChanges();
                return Int32.Parse(basdat.LoteoImagen.Max(c => c.id).ToString());
            }catch(Exception e){
                return -1;
            }
        }

        public void updateImagen(int id)
        {
            try
            {
                LoteoImagen img= basdat.LoteoImagen.First(c => c.id == id);
                img.procesado = false;                
                basdat.SaveChanges();
               
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public void updateImagen(int id, string nuevaImagen)
        {
            try
            {
                LoteoImagen img = basdat.LoteoImagen.First(c => c.id == id);
                
                img.rutaImagen = nuevaImagen;
                basdat.SaveChanges();

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public LoteoImagen selectImagen(int id)
        {
            try
            {
                LoteoImagen img = basdat.LoteoImagen.First(c => c.id == id);
                return img;

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        public LoteoImagen selectImagenNegocio(int idNegocio)
        {
            try
            {
                LoteoImagen img = basdat.LoteoImagen.First(c => c.NegId == idNegocio);
                return img;

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }
    }
}