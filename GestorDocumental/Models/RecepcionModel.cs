using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorDocumental.Models;

namespace GestorDocumental.Models
{
    /// <summary>
    /// 
    /// Modificacion:
    /// Auto : Elena Parra
    /// Fecha: 09 - 12 - 2013
    /// Descripcion : Cree el metodo Update 
    /// </summary>
    public class RecepcionModel
    {
        GestorDocumentalEnt basdat = new GestorDocumentalEnt();
        public int idRecepcion = 0;

        public long insert(Recepcion data)
        {
            basdat.AddToRecepcion(data);
            basdat.SaveChanges();
            this.idRecepcion = (int)basdat.Recepcion.Max(c => c.id);
            //Session["USUARIO_LOGUEADO"] = "";
            return data.numeroLote;
        }

        public void insert_detalle(Recepcion_Detalle data1)
        {
            basdat.AddToRecepcion_Detalle(data1);
            basdat.SaveChanges();
        }


        public void update(Recepcion data)
        {
            Recepcion update = basdat.Recepcion.Where(x => x.id == data.id).First();
            update.estado = 1;
            update.fechaCargue = DateTime.Now;
            basdat.SaveChanges();
        }

        public void update(int numLote)
        {
            Recepcion update = basdat.Recepcion.Where(x => x.numeroLote == numLote).First();
            update.estado = 0;
            basdat.SaveChanges();
        }

        /// <summary>
        /// Obtiene todos las radicaciones fisicas de cada cliente sin cargar o hace el loteo de los negocios
        /// </summary>
        /// <param name="cli"></param>
        /// <returns></returns>
        public IEnumerable<Recepcion> getAllSinCargar(Clientes cli)
        {
            //return basdat.Recepcion.Where(x => (x.nitCliente == cli.CliNit) && (x.estado == 99) || (x.estado == 0)).ToList<Recepcion>();
            return basdat.Recepcion.Where(x => (x.nitCliente == cli.CliNit) && (x.estado == 99)).ToList<Recepcion>();
        }
    }
}