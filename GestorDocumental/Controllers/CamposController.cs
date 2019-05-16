using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    /// <summary>
    /// Autor : _______
    /// Fecha : _______
    /// Descripcion : Logica necesaria para la consulta de los registros de la tabla campos
    /// 
    /// Modificado.
    /// Autor : Elena Parra
    /// Fecha : 26 -11 - 3013
    /// Descripcion: Agrege metodos.
    /// </summary>
    public class CamposController : Controller
    {
        private GestorDocumentalEnt gd;

        public Hashtable CamposPresentados(List<Campos> lstCamp)
        {
            Hashtable hashtable2;
            try
            {
                Hashtable hashtable = new Hashtable();
                foreach (Campos campos in lstCamp)
                {
                    hashtable.Add(campos.CampId, campos);
                }
                hashtable2 = hashtable;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo CamposPresentados " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return hashtable2;
        }

        public List<Campos> ObtenerCamposCliente(GruposDocumentos g, P_Etapas e, Captura n)
        {
            List<Campos> list4;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<Campos> list = null;
                if (e.IdEtapa == 1)
                {
                    list = (from c in this.gd.Campos
                            where c.GDocId == g.GDocId & (c.Activo == true) & (c.CampNumCaptura == 1)
                            orderby c.CampOrden
                            select c).ToList<Campos>();
                }
                else if (e.IdEtapa == 2)
                {
                    list = (from c in this.gd.Campos
                            where (c.GDocId == g.GDocId) & (c.CampNumCaptura2 == true) & (c.Activo == true)
                            orderby c.CampOrden
                            select c).ToList<Campos>();
                }
                else if (e.IdEtapa == 3)
                {
                    List<Captura> list2 = (from c in this.gd.Campos
                                           join cap in this.gd.Captura on c.CampId equals cap.CampId
                                           where (c.GDocId == g.GDocId) & (c.ControlCalidad == true) & (c.Activo == true) & (cap.NegId == n.NegId) & (c.CampNumCaptura == 1) & (cap.NumCaptura == 1)
                                           select cap).Union((from c in this.gd.Campos
                                                              join cap in this.gd.Captura on c.CampId equals cap.CampId
                                                              where (c.GDocId == g.GDocId) & (c.ControlCalidad == true) & (c.Activo == true) & (cap.NegId == n.NegId) & (c.CodFormulario != null) & (cap.NumCaptura == 1)
                                                              select cap))
                                           .ToList<Captura>();

                    List<Captura> list3 = (from c in this.gd.Campos
                                           join cap in this.gd.Captura on c.CampId equals cap.CampId
                                           where (c.GDocId == g.GDocId) & (c.ControlCalidad == true) & (c.Activo == true) & (c.CampNumCaptura2 == true) & (cap.NegId == n.NegId) & (cap.NumCaptura == 2)
                                           select cap).ToList<Captura>();

                    List<Captura> ListResult = (from c1 in list2
                                                join c2 in list3 on c1.CampId equals c2.CampId
                                                join c in this.gd.Campos on c2.CampId equals c.CampId
                                                where (c1.NegValor.ToString().ToUpper().Trim() != c2.NegValor.ToString().ToUpper().Trim())
                                                select c1).ToList();

                    list = (from c in gd.Campos.ToList() //where ListResult.Select(x=> x.CampId ).Contains(c.CampId) select c).ToList();
                            join r in ListResult on c.CampId equals r.CampId
                            orderby c.CampOrden
                            select c).ToList();

                    // se agregan los campos que quedaron en captura 1 y 2 en -999 o -1
                    List<Campos> listRest = null;
                    listRest = obtenerCamposFaltantes(g, e, n, list);
                    if (listRest.Count > 0)
                    {
                        list.AddRange(listRest);
                    }

                    if (list.Count > 0)
                    {
                        List<Campos> ListPadresipad = new List<Campos>();
                        ListPadresipad = obtenerPadres(list2, list, ListPadresipad);
                        if (ListPadresipad.Count > 0)
                            list.AddRange(ListPadresipad);
                    }

                    //verificamos si alguno de los campos depende de otro , en tal caso debemos mostrar el campo del cuel depende 
                    List<Campos> lstDependientes = new List<Campos>();
                    Campos CampDep;

                    foreach (Campos item in list)
                    {
                        if ((gd.Campos.Where(c => c.CampDependiente == item.CampId).Count() > 0))
                        {
                            CampDep = gd.Campos.Where(c => c.CampDependiente == item.CampId).First();
                            if (!(list.Any(k => k.CampId == CampDep.CampId)))
                            {
                                lstDependientes.Add(gd.Campos.Where(c => c.CampDependiente == item.CampId).First());

                            }
                        }

                    }

                    list.AddRange(lstDependientes);

                    //valida si existe captura para una grilla 
                    if (list.Count == 0)
                    {
                        List<Captura> ListGrillas = (from c in this.gd.Campos
                                                     join cap in this.gd.Captura on c.CampId equals cap.CampId
                                                     where (c.GDocId == g.GDocId) & (c.ControlCalidad == true) & (c.CampNumCaptura2 == true) & (cap.NegId == n.NegId) & (cap.NumCaptura == 2)
                                                     select cap).ToList<Captura>();
                        if (ListGrillas.Count > 0)
                        {
                            Campos camp = new Campos();
                            camp.Activo = false;
                            camp.CampDescripcion = "";
                            list.Add(camp);
                        }
                    }

                    list = list.OrderBy(o => o.CampOrden).ToList();

                }
                else if (e.IdEtapa == 4)
                {
                    list = (from c in this.gd.Campos
                            where (c.GDocId == g.GDocId) & (c.CampNumCaptura == 4) & (c.Activo == true)
                            select c).ToList<Campos>();
                }
                list4 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo ObtenerCamposCliente " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            //Session["TOTAL_CAMPOS"] = list4.Count;
            //SessionRepository.setAtributo("TOTAL_CAMP",list4.Count);
            return list4;
        }

        public List<Campos> obtenerCamposFaltantes(GruposDocumentos g, P_Etapas e, Captura n, List<Campos> list)
        {
            //agregar campos capturados en -999
            List<Campos> listRest =  new List<Campos>();
            try
            {
                List<Campos> list1 = (from c in this.gd.Campos
                                      join cap in this.gd.Captura on c.CampId equals cap.CampId
                                      where (c.GDocId == g.GDocId) & (c.Activo == true) & (c.CampNumCaptura == 1) & (cap.NegId == n.NegId) & (cap.NumCaptura == 1) & ((cap.NegValor == "-999") | (cap.NegValor == "-111") )
                                      select c).ToList<Campos>();

                foreach (Campos l3 in list1)
                {
                    if (listRest.Where(c1 => c1.CampId == l3.CampId).Count() == 0 && list.Where(c1 => c1.CampId == l3.CampId).Count() == 0)
                    {
                        listRest.Add(l3);
                    }
                }

                List<Campos> list2 = (from c in this.gd.Campos
                                      join cap in this.gd.Captura on c.CampId equals cap.CampId
                                      where (c.GDocId == g.GDocId) & (c.Activo == true) & (c.CampNumCaptura2 == true) & (cap.NegId == n.NegId) & (cap.NumCaptura == 2) & ((cap.NegValor == "-999") | (cap.NegValor == "-111") )
                                      select c).ToList<Campos>();

                foreach (Campos l3 in list2)
                {
                    if (listRest.Where(c1 => c1.CampId == l3.CampId).Count() == 0 && list.Where(c1 => c1.CampId == l3.CampId).Count() == 0)
                    {
                        listRest.Add(l3);
                    }
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo ObtenerCamposCliente " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }

            return listRest;
        }

        public List<Campos> obtenerPadres(List<Captura> list2, List<Campos> list, List<Campos> ListPadresipad)
        {
            List<Campos> ListDep = (from c1 in list2
                                    join c2 in list on c1.CampId equals c2.CampId
                                    join c in this.gd.Campos on c2.CampId equals c.CampId
                                    where (c.CampDependiente != null || c.idPadre != null)
                                    select c).ToList<Campos>();
            //muestra los hijos que tienen neg valor diferente 
            List<Campos> ListPadrescampdep = new List<Campos>();

            //obtiene los campos padres de los anteriores hijos 
            List<Campos> ListDep2 = (from c1 in ListDep
                                     join c in this.gd.Campos on c1.CampDependiente equals c.CampId
                                     select c).ToList();
            //Logica para agregar los padres de hijos que tambien sean padres
            foreach (Campos l1 in ListDep2)
            {
                int campid = (int)l1.CampId;
                int esprimero = 0;

                if (ListPadrescampdep.Where(c1 => c1.CampId == campid).Count() == 0 && list.Where(c1 => c1.CampId == campid).Count() == 0)
                {
                    if (l1.CampDependiente == null)
                    {
                        ListPadrescampdep.Add(l1);
                    }
                    else
                    {
                        List<Campos> ListPadrePadre = new List<Campos>();
                        ListPadrePadre.Add(l1);
                        int idpadre = (int)l1.CampId;
                        while (idpadre != -3)
                        {
                            ListPadrePadre = (from c in this.gd.Campos
                                              where (c.CampId == idpadre)
                                              select c).ToList();
                            if (ListPadrePadre.Count > 0)
                            {
                                foreach (Campos ll1 in ListPadrePadre)
                                {
                                    if (ListPadrescampdep.Where(c1 => c1.CampId == ll1.CampId).Count() == 0 && list.Where(c1 => c1.CampId == ll1.CampId).Count() == 0)
                                    {
                                        ListPadrescampdep.Add(ll1);
                                        if (ll1.CampDependiente != null)
                                            idpadre = (int)ll1.CampDependiente;
                                        else idpadre = -3;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            ListPadresipad.AddRange(ListPadrescampdep);

            //obtiene los campos padres de los anteriores hijos 
            List<Campos> ListDep3 = (from c1 in ListDep
                                     join c in this.gd.Campos on c1.idPadre equals c.CampId
                                     select c).ToList();
            //Logica para agregar los padres de hijos que tambien sean padres
            foreach (Campos l1 in ListDep3)
            {
                int campid = (int)l1.CampId;
                int esprimero = 0;

                if (ListPadresipad.Where(c1 => c1.CampId == campid).Count() == 0 && list.Where(c1 => c1.CampId == campid).Count() == 0)
                {
                    if (l1.idPadre == null)
                    {
                        ListPadresipad.Add(l1);
                    }
                    else
                    {
                        List<Campos> ListPadrePadre = new List<Campos>();
                        ListPadrePadre.Add(l1);
                        int idpadre = (int)l1.CampId;
                        while (idpadre != -3)
                        {
                            ListPadrePadre = (from c in this.gd.Campos
                                              where (c.CampId == idpadre)
                                              select c).ToList();
                            if (ListPadrePadre.Count > 0)
                            {
                                foreach (Campos ll1 in ListPadrePadre)
                                {
                                    if (ListPadresipad.Where(c1 => c1.CampId == ll1.CampId).Count() == 0 && list.Where(c1 => c1.CampId == ll1.CampId).Count() == 0)
                                    {
                                        ListPadresipad.Add(ll1);
                                        if (ll1.idPadre != null)
                                            idpadre = (int)ll1.idPadre;
                                        else idpadre = -3;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ListPadresipad;
        }

        public List<P_Ciudad> obtenerCiudades(P_Departamentos D)
        {
            List<P_Ciudad> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<P_Ciudad> list = (from ciu in this.gd.P_Ciudad
                                       orderby ciu.CiuNombre
                                       where ciu.DeptId == D.DeptId
                                       select ciu).ToList<P_Ciudad>();
                P_Ciudad item = new P_Ciudad
                {
                    CiuId = -1,
                    CiuNombre = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo obtenerCiudades " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<CodigosCampo> obtenerCodigosCampo(Campos camp)
        {

            List<CodigosCampo> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<CodigosCampo> list = (from c in this.gd.CodigosCampo
                                           where c.CampId == camp.CampId
                                           orderby c.CodOrden
                                           select c).ToList<CodigosCampo>();
                //SI NO SELECCIONA NADA AGREGO ESTE CAMPO COMO NULO
                Parametros param = this.gd.Parametros.First(c => c.codigo == "VAL_NUL");

                CodigosCampo item = new CodigosCampo
                {
                    CodId = param.valor,
                    CodDescripcion = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo obtenerCodigosCampo " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<CodigosCampo> obtenerCodigosCampoVD(tvalDoc_Causales camp)
        {
            string cod_causal = Convert.ToString(camp.cod_causal);
            List<CodigosCampo> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<CodigosCampo> list = (from c in this.gd.CodigosCampo
                                           where c.CodPadre == cod_causal
                                           orderby c.CodOrden
                                           select c).ToList<CodigosCampo>();
                //SI NO SELECCIONA NADA AGREGO ESTE CAMPO COMO NULO
                Parametros param = this.gd.Parametros.First(c => c.codigo == "VAL_NUL");

                CodigosCampo item = new CodigosCampo
                {
                    CodId = param.valor,
                    CodDescripcion = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo obtenerCodigosCampo " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<CodigosCampo> obtenerCodigosCampoChk(Campos camp)
        {
            List<CodigosCampo> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<CodigosCampo> list = (from c in this.gd.CodigosCampo
                                           where c.CampId == camp.CampId
                                           select c).ToList<CodigosCampo>();
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo obtenerCodigosCampo " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<P_Departamentos> obtenerDepartamentos()
        {
            List<P_Departamentos> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<P_Departamentos> list = (from d in this.gd.P_Departamentos
                                              orderby d.DeptNombre
                                              select d).ToList<P_Departamentos>();

                P_Departamentos item = new P_Departamentos
                {
                    DeptId = -1,
                    DeptNombre = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo obtenerDepartamentos " + exception.Message);
                throw exception;
            }
            return list2;
        }

        public List<spObtenerRespuestasAnteriores_Result> ObtenerRespuestasAnteriores(Captura N)
        {
            List<spObtenerRespuestasAnteriores_Result> lst;
            List<spObtenerRespuestasAnteriores_Result> lst_retorna = new List<spObtenerRespuestasAnteriores_Result>();

            try
            {

                this.gd = new GestorDocumentalEnt();
                lst = gd.spObtenerRespuestasAnteriores(N.NegId).ToList();

                foreach (spObtenerRespuestasAnteriores_Result item in lst)
                {

                    int tipo = this.gd.Campos.Where(c => c.CampId == item.CAMPO).First().TcId;

                    ////ESTOS SON CAMPOS TIPO LISTA POR TANTO LO QUE SACO ES LA DESCRIPCION NO EL VALOR
                    if (tipo == 5 )
                    {
                        if (item.RESP1 != "-1" & item.RESP1 != "")
                        {
                            item.RESP1 = this.gd.CodigosCampo.Where(c => c.CampId == item.CAMPO && c.CodId == item.RESP1).First().CodDescripcion;
                        }
                        else
                        {
                            item.RESP1 = "Ninguna";
                        }

                        if (item.RESP2 != "-1" & item.RESP2 != "")
                        {
                            item.RESP2 = this.gd.CodigosCampo.Where(c => c.CampId == item.CAMPO && c.CodId == item.RESP2).First().CodDescripcion;
                        }
                        else
                        {
                            item.RESP2 = "Ninguna";
                        }
                    }

                    //si el campo es un check 
                    if (tipo == 11)
                    {
                        if (item.RESP1 != "0" | item.RESP1 != "")
                        {
                            item.RESP1 = "SI";
                        }
                        else
                        {
                            item.RESP1 = "NO";
                        }
                    }

                    if (tipo == 6)
                    {
                        int r1 = int.Parse(item.RESP1);
                        int r2 = int.Parse(item.RESP2);

                        if (r1 != 0 & r1 != -1)
                        {
                            item.RESP1 = this.gd.P_Departamentos.Where(c => c.DeptId == r1).First().DeptNombre;
                        }

                        if (r2 != 0 & r2 != -1)
                        {
                            item.RESP2 = this.gd.P_Departamentos.Where(c => c.DeptId == r2).First().DeptNombre;
                        }
                    }

                    if (tipo == 7)
                    {
                        int r1 = ((item.RESP1 == "") ? -1 : int.Parse(item.RESP1));
                        int r2 = ((item.RESP2 == "") ? -1 : int.Parse(item.RESP2));
                        if (item.RESP1.Equals("") | item.RESP1.Equals("0") | item.RESP1.Equals("-1"))
                        {
                            item.RESP1 = "Ninguna";
                        }
                        else
                        {
                            item.RESP1 = this.gd.P_Ciudad.Where(c => c.CiuId == r1).First().CiuNombre;
                        }

                        if (item.RESP2.Equals("") | item.RESP2.Equals("0") | item.RESP2.Equals("-1"))
                        {
                            item.RESP2 = "Ninguna";
                        }
                        else
                        {
                            item.RESP2 = this.gd.P_Ciudad.Where(c => c.CiuId == r2).First().CiuNombre;
                        }

                    }

                    lst_retorna.Add(item);
                }

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo ObtenerRespuestasAnteriores " + exception.Message);
                throw exception;
            }

            return lst_retorna;

        }

        public Campos ObtenerCampoComodin(GruposDocumentos GD, Clientes Cli)
        {
            try
            {
                this.gd = new GestorDocumentalEnt();
                return (from c in gd.Campos where c.TcId == 10 & c.GDocId == GD.GDocId select c).First();

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo ObtenerCampoComodin " + exception.Message);
                throw exception;
            }

        }

        /// <summary>
        /// Obtiene los campos para de el formulario que desea pintar
        /// Nota: El codOrigen es un numero compuesto por el subproducto id  y el estado 
        /// </summary>
        /// <param name="CodOrigen">Codigo del formulario</param>
        /// <returns></returns>
        public dynamic getCamposFormulario(int CodOrigen)
        {
            try
            {
                this.gd = new GestorDocumentalEnt();
                int idFormulario = Convert.ToInt32(gd.Formularios.Where(f => f.CodOrigen == CodOrigen).Select(f => f.IdFormularios).FirstOrDefault());

                return gd.Campos.Join(gd.TiposCampo,
                       camp => camp.TcId,
                       tipCamp => tipCamp.TcId,
                       (camp, tipCamp) => new { Camp = camp, TipCamp = tipCamp })
                       .Where(x => x.Camp.CodFormulario == idFormulario)
                       .OrderBy(x => x.Camp.CampOrden)
                       .Select(x => new
                       {
                           x.Camp.CampId,
                           x.Camp.CampDescripcion,
                           x.Camp.CampAlto,
                           x.Camp.CampAncho,
                           x.Camp.CampObligatorio,
                           x.Camp.TcId,
                           x.Camp.LongMax,
                           x.Camp.CampOrden,
                           x.Camp.idPadre,
                           x.Camp.CodFormulario
                       }).ToArray();

            }
            catch (Exception exception)
            {

                LogRepository.registro("Error en CamposController metodo getCamposFormulario " + exception.Message);
                throw exception;
            }
        }

        /// <summary>
        /// Otiene los valores del campo que es de tipo lista
        /// </summary>
        /// <param name="idCamp">Id Del campo</param>
        /// <param name="PaisCliente">Pasi cliente</param>
        /// <returns></returns>
        public dynamic getValoresListCamp(int idCamp, string CliNit)
        {

            try
            {
                this.gd = new GestorDocumentalEnt();
                return gd.CodigosCampo.Where(x => x.CampId == idCamp).OrderBy(x => x.CodOrden)
                            .Select(x => new
                            {
                                x.CodId,
                                x.CodDescripcion,
                                x.CodCampId

                            }).ToList();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CamposController metodo getValoresListCamp " + exception.Message);
                throw exception;
            }
        }

    }
}
