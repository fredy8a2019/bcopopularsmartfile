using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActualizarListas.Models;

namespace ActualizarListas.Clases
{
    public class ConsultasSQL
    {
        //GestorDocumentalModel tablasOp = new GestorDocumentalModel();
        GestorDocEntDataContext tablasOp = new GestorDocEntDataContext();
        
        public List<int> consultarCodigosCampo()
        {
            var query = (from pro in tablasOp.CodigosCampo
                         join cs in tablasOp.Campos on pro.CampId equals cs.CampId
                         where cs.idPadre != null
                         select pro.CampId).Distinct();

            return query.OrderBy(i => i).ToList();
        }

        public List<string> consultarValoresCampos(int id_campo)
        {
            var query = from pro in tablasOp.CodigosCampo
                        where pro.CampId == id_campo
                        select pro.CodDescripcion;

            return query.ToList();
        }

        public List<string> consultarIndicesCampos(int id_campo)
        {
            var query = from pro in tablasOp.CodigosCampo
                        where pro.CampId == id_campo
                        select pro.CodId;

            return query.ToList();
        }

        //******************** Consultas de la lista Departamentos - Ciudades****************//
        public List<int> consultarIdDepartamentos()
        {
            var query = (from pro in tablasOp.P_Ciudad
                        select pro.DeptId).Distinct();

            return query.ToList();
        }

        public List<int> consultarIdCiudades(int idDepartamento)
        {
            var query = from pro in tablasOp.P_Ciudad
                        where pro.DeptId == idDepartamento
                        select pro.CiuId;

            return query.ToList();
        }

        public List<string> consultarNombresCiudades(int idDepartamento)
        {
            var query = from pro in tablasOp.P_Ciudad
                        where pro.DeptId == idDepartamento
                        select pro.CiuNombre;

            return query.ToList();
        }

        //******************** Consultas de la lista de Padre - Hijo****************//
        public List<string> consultarIDCodPadres()
        {
            var query = (from pro in tablasOp.CodigosCampo
                         where pro.CodPadre != null
                         select pro.CodPadre).Distinct();

            return query.ToList();
        }

        public List<string> consultarIDValores(string CodPadre)
        {
            var query = from pro in tablasOp.CodigosCampo
                        where pro.CodPadre == CodPadre
                        select pro.CodId;

            return query.ToList();
        }

        public List<string> consultarDescripcionValores(string CodPadre)
        {
            var query = from pro in tablasOp.CodigosCampo
                        where pro.CodPadre == CodPadre
                        select pro.CodDescripcion;

            return query.ToList();
        }
    }
}
