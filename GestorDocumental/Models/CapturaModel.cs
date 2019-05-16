using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace GestorDocumental.Models
{
    /// <summary>
    /// Autor: Elena Parra 
    /// Fecha: 25 - 10 - 2013 
    /// Descripcion: Almacena la logica necesaria para las consultas que necesite la tabla captura
    /// </summary>
    public class CapturaModel
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>
        /// Obtiene los valores de la ultima captura de cada campo
        /// </summary>
        /// <param name="negId">IdNeg</param>
        /// <returns>Lista de valores</returns>
        public IEnumerable GetValueCaptura(decimal negId)
        {
            IList<dynamic>  values =  new List<dynamic>();
            for (int i = 1; i <= 56; i++)
            {
                var value = db.Captura.OrderByDescending(x => x.NumCaptura).Where(x => x.NegId == negId && x.CampId == i).Select(x => x.NegValor).Take(1).AsParallel().FirstOrDefault();
                values.Add(value);         
            }
            return values; 
        }

        /// <summary>
        /// Obtienea el nombre de la ciudad por el id
        /// </summary>
        /// <param name="CiudadId">IdCiudad</param>
        /// <returns>Nombre de la ciudad</returns>
        public string GetNameCiudadById(int CiudadId)
        {
            return db.P_Ciudad.Where(x => x.CiuId == CiudadId).Select(x => x.CiuNombre).AsParallel().FirstOrDefault();
        }

        /// <summary>
        /// Obtiene el nombre del departamento por el id
        /// </summary>
        /// <param name="DepartamentoID">Id Departamento</param>
        /// <returns>Nombre departamento</returns>
        public string GetNameDepartamentoById(int DepartamentoID)
        {
            return db.P_Departamentos.Where(x => x.DeptId == DepartamentoID).Select(x => x.DeptNombre).AsParallel().FirstOrDefault();
        }

        /// <summary>
        /// Obtiene el id del documento que cada campo 
        /// </summary>
        /// <param name="CampId">IdCampo</param>
        /// <returns>id documento</returns>
        public string GetDogId(int CampId) 
        {
            return db.Campos.Where(x => x.CampId == CampId).Select(x => x.GDocId).AsParallel().FirstOrDefault().ToString();
        }

        /// <summary>
        /// Obtiene la descripcion del tipo de documento   
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionTD(string CodId) 
        {
            return db.CodigosCampo.Where(x => x.CodId == CodId).Select(x => x.CodDescripcion).FirstOrDefault().ToString();
        }


    }
}