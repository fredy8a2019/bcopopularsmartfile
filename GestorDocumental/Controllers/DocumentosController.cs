using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class DocumentosController : Controller
    {
        private GestorDocumentalEnt gd;

        public bool buscarPaginaDigitada(ArchivosAnexos C)
        {
            bool flag2;
            try
            {
                bool flag = false;
                if (this.gd.ArchivosAnexos.Any<ArchivosAnexos>(O => (O.NumPagina == C.NumPagina) & (O.NegId == C.NegId)))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                flag2 = flag;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo buscarPaginaDigitada " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return flag2;
        }

        public bool IndexacionTerminada(ArchivosAnexos C)
        {
            bool flag2;
            try
            {
                bool flag = false;
                this.gd = new GestorDocumentalEnt();
                Captura n = new Captura
                {
                    NegId = C.NegId
                };
                if (int.Parse(this.gd.spContarPaginasIndexadas(new decimal?(C.NegId)).First().ToString()) == this.ObtenerNumPaginasNegocio(n))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                flag2 = flag;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo IndexacionTerminada " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return flag2;
        }

        public void insertarDocsIndexados(ArchivosAnexos aIndex)
        {
            try
            {
                using (this.gd = new GestorDocumentalEnt())
                {
                    if (this.gd.ArchivosAnexos.Any<ArchivosAnexos>(O => ((O.DocId == aIndex.DocId) & (O.NumPagina == aIndex.NumPagina)) & (O.NegId == aIndex.NegId)))
                    {
                        ArchivosAnexos anexos = this.gd.ArchivosAnexos.First<ArchivosAnexos>(i => ((i.DocId == aIndex.DocId) & (i.NumPagina == aIndex.NumPagina)) & (i.NegId == aIndex.NegId));
                        anexos.NegId = aIndex.NegId;
                        anexos.NumPagina = aIndex.NumPagina;
                        anexos.FechaRegistro = aIndex.FechaRegistro;
                        anexos.DocId = aIndex.DocId;
                        anexos.AADescripcion = aIndex.AADescripcion;
                        anexos.Usuario = aIndex.Usuario;
                        this.gd.SaveChanges();
                    }
                    else
                    {
                        this.gd.AddToArchivosAnexos(aIndex);
                        this.gd.SaveChanges();
                    }
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo insertarDocsIndexados " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
        }

        public List<Documentos> obtenerDocumentosCliente(Clientes C)
        {
            List<Documentos> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();

                //List<Documentos> list=obtenerDocumentosIndexacion(negId);
                //ANTERIOR
                List<Documentos> list = (from cd in this.gd.ClienteDocumentos
                                         join d in this.gd.Documentos on cd.DocId equals d.DocId
                                         join cli in this.gd.Clientes on cd.CliNit equals cli.CliNit
                                         where cd.CliNit == C.CliNit
                                         orderby cd.Orden
                                         select d).ToList();

                //List<Documentos> list3 = (from cd in this.gd.ClienteDocumentos
                //                          join d in this.gd.Documentos on cd.DocId equals d.DocId into d
                //                          where cd.CliNit == C.CliNit
                //                          orderby cd.Orden
                //                          select d).ToList();

                Documentos item = new Documentos
                {
                    DocId = 0,
                    DocDescripcion = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo obtenerDocumentosCliente " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<Documentos> obtenerDocumentosClienteConciliacion(Clientes C, int idDocumento)
        {
            List<Documentos> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<Documentos> list = (from cd in this.gd.ClienteDocumentos
                                         join d in this.gd.Documentos on cd.DocId equals d.DocId
                                         where cd.CliNit == C.CliNit && d.DocId == idDocumento

                                         orderby cd.Orden
                                         select d).ToList();

                //List<Documentos> list3 = (from cd in this.gd.ClienteDocumentos
                //                          join d in this.gd.Documentos on cd.DocId equals d.DocId into d
                //                          where cd.CliNit == C.CliNit
                //                          orderby cd.Orden
                //                          select d).ToList();

                Documentos item = new Documentos
                {
                    DocId = 0,
                    DocDescripcion = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo obtenerDocumentosClienteConciliacion " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<Documentos> obtenerDocumentosIndexacion(int negId)
        {
            List<Documentos> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();

                //SACO EL GRUPO AL QUE PERTENECE ESE NEGOCIO
                List<Recepcion> r = (from cd in this.gd.Recepcion
                                     join d in this.gd.CargueLotes on cd.id equals d.idRecepcion
                                     where d.NegId == negId
                                     select cd).ToList();
                int grupo = 0;
                if (r.Count > 0)
                {
                    grupo = (int)r.First().subgrupo.Value;
                }

                //OBTENGO LOS DOCUMENTOS DE ESE NEGOCIO
                list2 = (from cd in this.gd.ProductosDocum
                         join d in this.gd.Documentos on cd.idDocumento equals d.DocId
                         where (cd.idGrupo == grupo) && (d.Activo == true)
                         select d).ToList<Documentos>();

                //list2 = (from cd in this.gd.ClienteDocumentos
                //         join d in this.gd.Documentos on cd.DocId equals d.DocId  where cd.CliNit == C.CliNit

                //         select d).ToList<Documentos>();


            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo obtenerDocumentosIndexacion " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<GruposDocumentos> obtenerGruposDocumentos(Documentos D)
        {
            List<GruposDocumentos> list2;
            try
            {
                //this.gd.GruposDocumentos.First().DocId
                this.gd = new GestorDocumentalEnt();
                List<GruposDocumentos> list = (from d in this.gd.Documentos
                                               join g in this.gd.GruposDocumentos on d.DocId equals g.DocId
                                               where g.DocId == D.DocId
                                               orderby g.Orden
                                               select g).ToList();

                //== Modifica:Ivan Rodriguez; 06/05/2016 =====
                //GruposDocumentos item = new GruposDocumentos
                // {
                //     DocId = 0,
                //     GDocId = 0,
                //     GDocDescripcion = "Seleccione..."
                // };
                //list.Insert(0, item);
                //============================================
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo obtenerGruposDocumentos " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<GruposDocumentos> obtenerGruposDocumentosConciliacion(Documentos D, int idGrupo)
        {
            List<GruposDocumentos> list2;
            try
            {
                //this.gd.GruposDocumentos.First().DocId
                this.gd = new GestorDocumentalEnt();
                List<GruposDocumentos> list = (from d in this.gd.Documentos
                                               join g in this.gd.GruposDocumentos on d.DocId equals g.DocId
                                               where g.DocId == D.DocId && g.GDocId == 6
                                               orderby g.Orden
                                               select g).ToList();

                GruposDocumentos item = new GruposDocumentos
                {
                    DocId = 0,
                    GDocId = 0,
                    GDocDescripcion = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo obtenerGruposDocumentosConciliacion " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public int ObtenerNumPaginasNegocio(Captura N)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                num2 = int.Parse(this.gd.spObtenerNumeroPaginas(N.NegId).First().ToString());

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo ObtenerNumPaginasNegocio " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        public List<spObtenerDocumentosPaginas_Result> ObtenerPaginasIndexadas(Captura N)
        {
            List<spObtenerDocumentosPaginas_Result> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                list2 = this.gd.spObtenerDocumentosPaginas(new decimal?(N.NegId)).ToList<spObtenerDocumentosPaginas_Result>();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo ObtenerPaginasIndexadas " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<spObtenerDocumentosPaginas_Result> ObtenerPaginasIndexadas(int N)
        {
            List<spObtenerDocumentosPaginas_Result> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                list2 = this.gd.spObtenerDocumentosPaginas(new decimal?(N)).ToList<spObtenerDocumentosPaginas_Result>();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo ObtenerPaginasIndexadas " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public int obtenerUltimaPagina(Captura N)
        {
            int num3;
            try
            {
                this.gd = new GestorDocumentalEnt();
                int? nullable = (from s in this.gd.ArchivosAnexos
                                 where s.NegId == N.NegId
                                 select s).Max<ArchivosAnexos, int?>(x => x.NumPagina);
                num3 = int.Parse((nullable.HasValue ? nullable.GetValueOrDefault() : 0).ToString());
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo obtenerUltimaPagina " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return num3;
        }

        public int ObtenrPaginaDocumento(Documentos D, Captura N)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                num2 = int.Parse(this.gd.spObtenerPaginaDocumento(D.DocId, N.NegId).First().ToString());

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo ObtenrPaginaDocumento " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        public void BorrarDocumento(ArchivosAnexos D, Captura N)
        {
            this.gd = new GestorDocumentalEnt();
            try
            {
                using (this.gd)
                {
                    //JFP;Abril-2016;BcoPopular; cambio para que se elimine solo la pagina seleccionada
                    //var entity = this.gd.ArchivosAnexos.Where(x => x.NegId == N.NegId).Where(x => x.NumPagina >= D.NumPagina);
                    var entity = this.gd.ArchivosAnexos.Where(x => x.NegId == N.NegId).Where(x => x.NumPagina == D.NumPagina);
                    foreach (var a in entity)
                    {
                        this.gd.ArchivosAnexos.DeleteObject(a);
                    }
                    this.gd.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo BorrarDocumento " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }


        }

        /// <summary>
        /// JFP; ABRIL/2016; SE CREA PARA REINDEXACION
        /// </summary>
        /// <param name="D"></param>
        /// <param name="N"></param>
        public void BorrarDocumento(ArchivosAnexos D, int _negId)
        {
            this.gd = new GestorDocumentalEnt();
            try
            {
                using (this.gd)
                {
                    //JFP;Abril-2016;BcoPopular; cambio para que se elimine solo la pagina seleccionada
                    //var entity = this.gd.ArchivosAnexos.Where(x => x.NegId == N.NegId).Where(x => x.NumPagina >= D.NumPagina);
                    var entity = this.gd.ArchivosAnexos.Where(x => x.NegId == _negId).Where(x => x.NumPagina == D.NumPagina);
                    foreach (var a in entity)
                    {
                        this.gd.ArchivosAnexos.DeleteObject(a);
                    }
                    this.gd.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en DocumentosController metodo BorrarDocumento " + exception.Message + " stack " + exception.StackTrace);
                throw exception;
            }


        }

    }
}
