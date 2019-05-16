using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorDocumental.Models
{
    /// <summary>
    /// Autor : Elena Parra 
    /// Fecha :  - 07 - 2014
    /// Descripcion  : Se crea todo la logica necesaria para administracion de los datos de una unidad documental
    /// </summary>
    public class UnidadDocumentalModel
    {
        private GestorDocumentalEnt _db;

        public UnidadDocumentalModel(GestorDocumentalEnt _conexion)
        {
            _db = _conexion;
        }

        /// <summary>
        /// Crea un  nuevo registro en la tabla undad dumental
        /// </summary>
        /// <param name="nuevo">Objeto unidad ducumental</param>
        /// <returns>long</returns>
        public long Add(alm_UnidadDocumental nuevo)
        {
            return long.Parse(_db.spAlm_InsertarUnidadDocumental(nuevo.Cliente,
                nuevo.Oficina, nuevo.TipoContenedor,
                nuevo.FechaCreacion, nuevo.UsuarioCreacion,
                nuevo.DestinoId).ToArray()[0].Identity.ToString());
        }

        /// <summary>
        /// Guarda la captura de la unidad documental que se creo
        /// </summary>
        /// <param name="listaCaptura"></param>
        public void Add(List<alm_CapturaUD> listaCaptura)
        {
            try
            {
                foreach (alm_CapturaUD captura in listaCaptura)
                {
                    this._db.AddToalm_CapturaUD(captura);
                    this._db.SaveChanges();
                }
            }
            catch (Exception es)
            {
                throw;
            }
        }

        /// <summary>
        /// ALmacena los subproductos almacenados en la unidad documental
        /// </summary>
        /// <param name="gruposAlmacenado"></param>
        public void Add(List<alm_GruposCUD> gruposAlmacenado)
        {
            try
            {
                foreach (alm_GruposCUD grupo in gruposAlmacenado)
                {
                    this._db.AddToalm_GruposCUD(grupo);
                    this._db.SaveChanges();
                }
            }
            catch (Exception es)
            {
                throw;
            }
        }

        /// <summary>
        /// Consulta los datos de una unidad documental
        /// </summary>
        /// <param name="cud"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> getCUD(long cud)
        {
            try
            {
                return _db.spAlm_ConsultaDataUD(cud);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Valida si la Unidad documental tiene el estado de Caja Cerrada
        /// </summary>
        /// <param name="cud"></param>
        /// <returns></returns>
        public int validarCUD(int cud, int cliNit)
        {
            try
            {
                List<spAlm_ValidarEstadoUD_Result> resultado = new List<spAlm_ValidarEstadoUD_Result>();
                resultado = _db.spAlm_ValidarEstadoUD(cud, cliNit).ToList();
                return Convert.ToInt32(resultado[0].Resultado);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Consulta si el lote que se va a almacenar cumple con todos lo requicitos para almacenar 
        /// </summary>
        /// <param name="cud"></param>
        /// <param name="NoLote"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> _almacenarPorLote(long cud, long NoLote)
        {
            try
            {
                return _db.spAlm_AlmacenarLote(NoLote, cud);
            }
            catch (Exception es)
            {
                throw;
            }
        }

        ///// <summary>
        ///// Consulta los negocios del lote para verlos en el detalle
        ///// </summary>
        ///// <param name="NoLote"></param>
        ///// <returns></returns>
        //public IEnumerable<dynamic> _getDetalleLote(long NoLote)
        //{
        //    try
        //    {
        //        return _db.spAlm_DetalleLote(NoLote);
        //    }
        //    catch (Exception es)
        //    {
        //        throw;
        //    }
        //}
    }
}