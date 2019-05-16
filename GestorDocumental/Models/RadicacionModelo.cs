using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorDocumental.Models
{
    /// <summary>
    /// Autor : Elena Parra 
    /// Fecha : 25 - 11 - 2013
    /// Descripcion  : Se crea todo la logica necesaria para administracion de la tabla Radicacion
    /// </summary>
    public class RadicacionModelo
    {
        protected GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>
        /// Crea un nuevo registro en la tabla radicacion
        /// </summary>
        /// <param name="nueva">Datos</param>
        /// <returns>Id del regitro almacenado</returns>
        public long Add(Radicacion nueva) {

            long id = long.Parse(db.spInsertarRadicacion(nueva.CliNit, 
                                            nueva.Oficina, 
                                            nueva.IdProducto, 
                                            nueva.SubProducto, 
                                            nueva.FechaRadicacion, 
                                            nueva.Estado,
                                            nueva.CodBarras, 
                                            nueva.IdUsusario, 
                                            nueva.TipoRadicacion,nueva.FechaLocal).ToArray()[0].ToString());
            return id;
        }

        

    }
}