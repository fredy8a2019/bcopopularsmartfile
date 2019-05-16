using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorDocumental.Models;

namespace GestorDocumental.Models
{
    /// <summary>
    /// Autor: Elena Parra 
    /// Fecha: 21 - 10 - 2013 
    /// Descripcion: Verifica los datos de los usuarios del cliente 
    /// </summary>
    public class Verif_DataModelo
    {
        GestorDocumentalEnt basdat = new GestorDocumentalEnt();

        /// <summary>
        /// Almacena los datos del archivo que se carga para verificar los 
        /// datos que nos envia el porveedor.
        /// </summary>
        /// <param name="nuevo">Datos</param>
        /// <returns></returns>
        public long AddVerif_Datos_Maestra(Verif_Datos_Maestra nuevo)
        {

            basdat.AddToVerif_Datos_Maestra(nuevo);
            basdat.SaveChanges();
            int id = basdat.Verif_Datos_Maestra.Max(x => x.VDM_ID);
            return id;
        }

        /// <summary>
        /// Almacena los registros que envia el proveedor en el archivo 
        /// </summary>
        /// <param name="nuevo">datos cliente</param>
        /// <returns></returns>
        public string AddVerif_Datos(Verif_Datos nuevo)
        {

            basdat.AddToVerif_Datos(nuevo);
            basdat.SaveChanges();
            return "Datos almacenados";
        }

        /// <summary>
        /// Altera el campo 
        /// </summary>
        /// <param name="idVDM"></param>
        /// <param name="value"></param>
        public void UpdateVerif_Datos_Maestra(long idVDM, long value)         
        {
           var registro = basdat.Verif_Datos_Maestra.Where(x => x.VDM_ID == idVDM).FirstOrDefault();
           registro.VDM_CantidadRegistros = int.Parse(value.ToString());
           basdat.SaveChanges();
        }

        /// <summary>
        /// Ejecuta el procedimiento almacenado spCruceIdentidad
        /// </summary>
        public void CruceDeIdentidad()
        {
            basdat.spCruceIdentidad();
        }
        
    }
}