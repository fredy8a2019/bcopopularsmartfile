using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorDocumental.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DatosReferenciacionModelo
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>
        /// Inserta en la tabla 
        /// </summary>
        /// <param name="nuevo"></param>
        public void insertData(DatosReferenciacion nuevo) 
        {
            db.AddToDatosReferenciacion(nuevo);
            db.SaveChanges();
        }

        /// <summary>
        /// Obtiene los datos referenciacion por el id a la formularios y el negocio 
        /// </summary>
        /// <param name="idNeg">Id negocio</param>
        /// <param name="formulario">numero formulario</param>
        /// <returns></returns>
        public DatosReferenciacion GetAsignacionByIdNeg(decimal idNeg, int formulario)
        {
            try
            {
                return db.DatosReferenciacion.First<DatosReferenciacion>(x => x.DR_NegId == idNeg && x.DR_Fomularios == formulario);
            }
            catch (Exception)
            {
                DatosReferenciacion vacio = new DatosReferenciacion();
                return vacio;
            }
           
        } 
    }
}