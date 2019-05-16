using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ActualizarListas.Models;

namespace ActualizarListas.Clases
{
    public class CrearArchivo
    {
        public List<int> consultarTablas()
        {
            ConsultasSQL coOp = new ConsultasSQL();
            List<int> listaCodigosCampo = coOp.consultarCodigosCampo();

            return listaCodigosCampo;
        }

        public string consultarDescripcionesCampo(int id_campo)
        {
            ConsultasSQL _coP = new ConsultasSQL();
            List<string> listaDescripciones = _coP.consultarValoresCampos(id_campo);

            string datos = "";
            foreach (string dato in listaDescripciones)
            {
                if (datos == "")
                {
                    datos = dato;
                }
                else
                {
                    datos = datos + "," + dato;
                }
            }

            return datos;
        }

        public string consultarIndicesCampos(int id_campo)
        {
            ConsultasSQL _coP = new ConsultasSQL();
            List<string> listaIndices = _coP.consultarIndicesCampos(id_campo);

            string datos = "";
            foreach (string dato in listaIndices)
            {
                if (datos == "")
                {
                    datos = dato;
                }
                else
                {
                    datos = datos + "," + dato;
                }
            }

            return datos;
        }

        //******************************* Datos del script de las Ciudades - Departamentos
        public List<int> consultarIdDeptos()
        {
            ConsultasSQL dpt = new ConsultasSQL();
            List<int> listaCodigosDeptos = dpt.consultarIdDepartamentos();

            return listaCodigosDeptos;
        }

        public string consultarDescripcionesCiudades(int id_Departamento)
        {
            ConsultasSQL _idDept = new ConsultasSQL();
            List<string> listaDescripciones = _idDept.consultarNombresCiudades(id_Departamento);

            string datos = "";
            foreach (string dato in listaDescripciones)
            {
                if (datos == "")
                    datos = dato;
                else
                    datos = datos + "," + dato;
            }
            return datos;
        }

        public string consultarIndicesCiudades(int id_Departamento)
        {
            ConsultasSQL _idDept = new ConsultasSQL();
            List<int> listaIndices = _idDept.consultarIdCiudades(id_Departamento);

            string datos = "";
            foreach (int dato in listaIndices)
            {
                if (datos == "")
                    datos = dato.ToString();
                else
                    datos = datos + "," + dato.ToString();
            }
            return datos;
        }


        //******************************* Datos del script de las Listas Padre - Hijo
        public List<string> consultar_IDCODPadres()
        {
            ConsultasSQL val = new ConsultasSQL();
            List<string> listaCodigosPadres = val.consultarIDCodPadres();

            return listaCodigosPadres;
        }

        public string consultarDescripcionesValores(string CodPadre)
        {
            ConsultasSQL _val = new ConsultasSQL();
            List<string> listaDescripciones = _val.consultarDescripcionValores(CodPadre);

            string datos = "";
            foreach (string dato in listaDescripciones)
            {
                if (datos == "")
                    datos = dato;
                else
                    datos = datos + "," + dato;
            }
            return datos;
        }

        public string consultar_IDValores(string CodPadre)
        {
            ConsultasSQL _val = new ConsultasSQL();
            List<string> listaIDValores = _val.consultarIDValores(CodPadre);

            string datos = "";
            foreach (string dato in listaIDValores)
            {
                if (datos == "")
                    datos = dato;
                else
                    datos = datos + "," + dato;
            }
            return datos;
        }

    }
}
