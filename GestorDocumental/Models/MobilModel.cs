using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using System.Collections;

namespace GestorDocumental.Models
{
    /// <summary>
    /// Autor:          Elena Parra
    /// Fecha creacion: 03 - 02 - 2014
    /// Descripcion:    Modela los datos necesarios para la administracion del modulo mobil de radicacion 
    /// </summary>
    public class MobilModel
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>     
        /// Obtiene los documentos a listar
        /// </summary>
        /// <returns></returns>
        public IList<DocumentosMobil> GetDocumentos()
        {
            return db.MobDocumentos.Select(x => new DocumentosMobil()
            {
                Id = x.id,
                descripcion = x.descripcion                
            }).ToList();

        }

        /// <summary>
        /// Obtiene una lista de chequeo segun el documento 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ListaChequeo> GetListaChequeo(int idDocumentos)
        {
            return db.MobListaChequeo.Where(x => x.mobDocId == idDocumentos).Select(x => new ListaChequeo
            {
                id = x.id,
                descripcion = x.descripcion,
                idDocuemto = x.mobDocId,
                seleccion = false
            }).ToList();
        }

        /// <summary>
        /// Logica necesaria para guardar en la tabla mobCaptura
        /// </summary>
        /// <param name="ListaCaptura"></param>
        public void addMobCaptura(List<MobCaptura> ListaCaptura)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (MobCaptura captura in ListaCaptura)
                    {
                        this.db.AddToMobCaptura(captura);
                        this.db.SaveChanges();
                    }
                    scope.Complete();
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaRadicionModel metodo addMobCaptura  " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

    }

    public class DocumentosMobil
    {
        public long Id { get; set; }
        public string descripcion { get; set; }
    }

    public class ListaChequeo
    {
        public long id { get; set; }
        public string descripcion { get; set; }
        public long? idDocuemto { get; set; }
        public bool seleccion { get; set; }
    }
}