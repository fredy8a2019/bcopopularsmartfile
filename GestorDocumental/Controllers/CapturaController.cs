using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class CapturaController : Controller
    {
        private GestorDocumentalEnt gd;

        //INSERTO LA CAPTURA EN LA TABLA
        public void InsertarCaptura(List<Captura> lstCaptura)
        {
            try
            {
                this.gd = new GestorDocumentalEnt();
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (Captura captura in lstCaptura)
                    {
                        this.gd.AddToCaptura(captura);
                        this.gd.SaveChanges();
                    }
                    scope.Complete();
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo InsertarCaptura " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        //[WebMethod]
        public ActionResult ObtenerDListas(int idCampo)
        {
            WebServiceModel s = new WebServiceModel();
            List<CodigosCampo> listaCod = s.consultarOpcionesLista(idCampo);

            string DescCampos = "";
            for (int i = 0; i < listaCod.Count; i++)
            {
                if (DescCampos.Trim() == "")
                {
                    DescCampos = listaCod[i].CodDescripcion.ToString();
                }
                else
                {
                    DescCampos = DescCampos + "," + listaCod[i].CodDescripcion.ToString();
                }
            }
            ViewData["data"] = DescCampos;
            return View();
        }

        //FUNCION PARA ORDENAR UN STRING
        public string SortStringChars(string s)
        {
            try
            {
                char[] c = s.ToCharArray();
                Array.Sort(c);
                return new String(c);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo SortStringChars " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        public ActionResult confirmaPagina(int Captura)
        {
            //Session["CURRENT_PAGE"] = pagina;
            ViewData["captura"] = Captura;
            return View();
        }

        //FUNCION POBTENER EL CLIENTE DE UN NEGOCIO
        public decimal getClient(decimal negId)
        {
            try
            {
                this.gd = new GestorDocumentalEnt();
                return gd.CargueLotes.Where(c => c.NegId == negId).First().CliId;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo getClient " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        //VALIDO LO CAPTURADO CONTRA LA CONCILIACION QUE SE CAPTURO EN LA RECEPCION CON LA CARGA DEL ARCHIVO
        public bool ValidaConciliacion(List<Captura> lstCaptura, int NegId, int gruDocConciliacion, int sw_concilia)
        {
            bool respuesta = false;
            try
            {
                this.gd = new GestorDocumentalEnt();

                //OBTENGO EN ID QUE RECEPCIONO ESE NEGOCIO
                int idRecepcion = (int)gd.CargueLotes.First(c => c.NegId == NegId).idRecepcion;
                //GENERO LOS CODIGOS EN LA BD

                //genero los codigos de conciliacion para esa recepcion
                gd.spCodigosConcilia(idRecepcion, gruDocConciliacion);

                string codigoCaptura = "";
                List<Conciliacion> dataCapturada = new List<Conciliacion>();
                ConciliacionModel modeloConciliacion = new ConciliacionModel();
                int i = 1;
                foreach (Captura captura in lstCaptura)
                {
                    codigoCaptura += captura.NegValor.Trim().Replace(" ", "");

                    Conciliacion noConcilia = new Conciliacion();
                    noConcilia.campo = captura.NegValor;
                    noConcilia.orden = i;
                    noConcilia.idRecepcion = idRecepcion;
                    noConcilia.conciliado = false;
                    noConcilia.negId = NegId;
                    noConcilia.fechaCargue = DateTime.Now;

                    dataCapturada.Add(noConcilia);

                    i = i + 1;
                }

                //ordeno el string del codigo de la captura
                codigoCaptura = SortStringChars(codigoCaptura);
                //busco ese codigo en la tabla
                int total = gd.Conciliacion.Count(c => c.combinacion == codigoCaptura && c.idRecepcion == idRecepcion && c.conciliado == null);
                Conciliacion concilia = null;

                if (total > 0)
                {
                    concilia = gd.Conciliacion.First(c => c.combinacion == codigoCaptura && c.idRecepcion == idRecepcion && c.conciliado == null);
                }

                if (concilia == null)
                {
                    if (sw_concilia == 0)
                    {
                        modeloConciliacion.insertMultiple(dataCapturada);
                    }
                    respuesta = false;
                }
                else
                {
                    gd.spActualizaConcilia(concilia.consecutivo, NegId, idRecepcion);
                    respuesta = true;
                }

                return respuesta;

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ValidaConciliacion " + exception.Message + " stack trace " + exception.StackTrace);
                return false;
                throw exception;
            }
        }

        //Obtiene los campos asociados a los grupos documentales
        public int ObtenerCaposXGrupo(int grupo)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                //num2 = int.Parse(this.gd.spCamposXGrupo(new int?(grupo)).First<int?>().ToString());
                num2 = int.Parse(this.gd.spCamposXGrupo(new int?(grupo)).ToString());
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ObtenerCaposXGrupo " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Obtiene el tipo de documento que se va a procesar de inmediato dependiendo del grupo documental
        public int ObtenerDocumentoActual(Captura C, Clientes Cli, int idGrupo)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                num2 = int.Parse(this.gd.spObtenerDocumentoActual(C.NegId, Cli.CliNit, C.NumCaptura, idGrupo).First().ToString());

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ObtenerDocumentoActual " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Obtiene el grupo que se esta procesando actualmente en captura
        public int ObtenerGrupoActual(Captura C, Documentos D)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                num2 = int.Parse(this.gd.spObtenerGrupoActual(new int?(D.DocId), new decimal?(C.NegId), new int?(C.NumCaptura)).First<int?>().ToString());

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ObtenerGrupoActual " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Obtiene el siguiente documento que se va a procesar en Captura, con tal de validar si el proceso
        //de captura ya ha terminado de procesar todos los documentos del negocio
        public int ObtenerSiguienteDocumento(Captura C, Clientes Cli, int idGrupo)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                num2 = int.Parse(this.gd.spObtenerSiguienteDocumento(new decimal?(C.NegId), new decimal?(Cli.CliNit), new int?(C.NumCaptura), idGrupo).First<int?>().ToString());

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ObtenerSiguienteDocumento " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Obtiene el siguiente grupo que se va a ejecutar en el proceso de captura
        public int ObtenerSiguienteGrupo(Captura C, Documentos D)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();

                //======= << Modifica: Ivan Rodriguez; 06/05/2016 =============
                num2 = int.Parse(this.gd.spObtenerSiguienteGrupo(new int?(D.DocId), new decimal?(C.NegId), new int?(C.NumCaptura)).First<int?>().ToString());
                //num2 = Convert.ToInt32(gd.spObtenerGdocid(D.DocId).ToList().SingleOrDefault());
                //num2 = Convert.ToInt32(gd.spObtenerGdocid(D.DocId).First<int?>().ToString());
                //=== Ivan Rodriguez >> =======================================
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ObtenerSiguienteGrupo " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Obtiene el grupo de conciliacion dependiendo del Documento seleccionado
        public int ObtenerGrupoConciliacion(int idDocumento)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                //this.gd = new GestorDocumentalEnt();
                num2 = this.gd.GruposDocumentos.Where(c => c.DocId == idDocumento).First().GDocId;

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ObtenerGrupoConciliacion " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Obtiene el ultimo documento, asociado al grupo y al negocio, esto con tal de validar
        //si el documento que se esta procesando actualmente es ya el ultimo del negocio en captura
        public int obtenerUltimoDocumento(Clientes C, int idGrupo, int NegId)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                num2 = int.Parse(this.gd.soObtenerUltimoDocumentoCliente(new decimal?(C.CliNit), idGrupo, NegId).First<int?>().ToString());
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo obtenerUltimoDocumento " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Obtiene el subgrupo asociado al negocio que se esta procesando actualmente
        public int obtenerSubGrupo(int? NegId)
        {
            int num1;
            try
            {
                this.gd = new GestorDocumentalEnt();
                num1 = int.Parse(this.gd.spObtenerSubgrupo(NegId).First<int?>().ToString());
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo obtenerSubGrupo " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num1;
        }

        //Obtenemos el ultimo grupo de documentos dependiendo del documento asociado
        public int ObtenerUltimoGrupo(Documentos D)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                num2 = int.Parse(this.gd.spObtenerUltimoGrupoDocumento(new int?(D.DocId)).First<int?>().ToString());

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ObtenerUltimoGrupo " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Obtenemos el ultimo grupo documental al cual se le ha hecho captura, esto con tal de validar
        //que cuando un usuario no ha terminado el proceso de captura de un negocio y lo deja a medias,
        //el sistema pueda reconocer desde donde ha dejado la captura de ese negocio para continuar con
        //lo faltante
        public int ObtenerUltimoGrupoDocumentoCapturado(Captura N)
        {
            int num2;
            try
            {
                num2 = 0;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ObtenerUltimoGrupoDocumentoCapturado " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Se valida que exista control de calidad dependiendo de las capturas realizadas en el negocio
        //de ser asi pasa al proceso de Control de Calidad (Captura 3)
        public int ExisteControlCalidad(Captura N)
        {
            int num2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                num2 = int.Parse(this.gd.spExisteControlCalidad(N.NegId, 1).First<int?>().ToString());

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ExisteControlCalidad " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Valida si existe la captura del grupo que se esta procesando actualmente
        public bool ExisteCapturaGrupo(Captura N, int GdocI)
        {
            bool num2 = false;

            try
            {
                this.gd = new GestorDocumentalEnt();
                if (this.gd.spExisteCapturaGrupo(N.NegId, GdocI, N.NumCaptura).First() == 1)
                {
                    num2 = true;
                }
                else
                {
                    num2 = false;
                }


            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo ExisteControlCalidad " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num2;
        }

        //Obtiene el valor del campo de la tabla de parametros
        public string valorCampo(string val)
        {
            this.gd = new GestorDocumentalEnt();
            Parametros param = this.gd.Parametros.First(c => c.codigo == "VAL_NUL");
            if (val == "" || val == param.valor)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }

        /// <summary>
        ///Pasa los registros de captura radicacion a captura 
        /// </summary>
        public void spCapturaRadicacionACaptura(decimal negId)
        {
            try
            {
                gd = new GestorDocumentalEnt();
                gd.spCapturaradicacionACaptura(negId);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo spCapturaRadicacionACaptura " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        /// <summary>
        /// Otiene los registro que se obtuvieron en el modulo de contabilidad
        /// </summary>
        /// <param name="negId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> getCapturaContabilidad(decimal negId)
        {
            try
            {
                gd = new GestorDocumentalEnt();
                return gd.spCapturaContabilidad(Int32.Parse(negId.ToString()))
                                 .Select(x => new
                                 {
                                     x.CampId,
                                     x.NegValor,
                                     x.Usuario,
                                     x.NumCaptura,
                                     x.FechaRegistro
                                 });
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo getCapturaContabilidad " + exception.Message + " stack trace " + exception.StackTrace);
                return null;
                throw exception;
            }
        }

        #region Metodo Obsoleto
        public void CrearEtapasBizagi(int negId, int IdUsuario)
        {
            try
            {
                //this.gd = new GestorDocumentalEnt();

                //string fechaNacimiento = valorCampo(this.gd.spUltimoValCamp(negId, 8).FirstOrDefault() ?? "");
                //string celular = valorCampo(this.gd.spUltimoValCamp(negId, 9).FirstOrDefault() ?? "");
                //string correo = valorCampo(this.gd.spUltimoValCamp(negId, 10).FirstOrDefault() ?? "");
                //string firma = valorCampo(this.gd.spUltimoValCamp(negId, 53).FirstOrDefault() ?? "");

                //int resultado = int.Parse(fechaNacimiento + "" + celular + "" + correo);
                //int etapaBizagi = 0;

                ////90	Mesa de Control	9
                ////80	Contacto - Referenciacion
                ////90	Consulta

                //AsignacionTareas tarea = new AsignacionTareas();
                //switch (resultado)
                //{
                //    case 101:

                //        if (firma.Equals('0'))
                //        {
                //            tarea.NegId = negId;
                //            tarea.HoraInicio = DateTime.Now;
                //            tarea.IdEtapa = 90;
                //            tarea.IdEstado = 80;
                //            //Referenciacion
                //        }
                //        else
                //        {
                //            tarea.NegId = negId;
                //            tarea.HoraInicio = DateTime.Now;
                //            tarea.HoraTerminacion = DateTime.Now;
                //            tarea.IdEtapa = 90;
                //            tarea.IdEstado = 30;
                //            tarea.Usuario = IdUsuario;
                //        }

                //        break;
                //    case 100:
                //        tarea.NegId = negId;
                //        tarea.HoraInicio = DateTime.Now;
                //        tarea.IdEtapa = 90;
                //        tarea.IdEstado = 90;
                //        //Consulta
                //        break;
                //    case 10:
                //        tarea.NegId = negId;
                //        tarea.HoraInicio = DateTime.Now;
                //        tarea.IdEtapa = 90;
                //        tarea.IdEstado = 80;
                //        //Referenciacion
                //        break;
                //    case 1:

                //        if (firma.Equals('0'))
                //        {
                //            tarea.NegId = negId;
                //            tarea.HoraInicio = DateTime.Now;
                //            tarea.IdEtapa = 90;
                //            tarea.IdEstado = 80;
                //            //Referenciacion
                //        }
                //        else
                //        {
                //            tarea.NegId = negId;
                //            tarea.HoraInicio = DateTime.Now;
                //            tarea.HoraTerminacion = DateTime.Now;
                //            tarea.IdEtapa = 90;
                //            tarea.IdEstado = 30;
                //            tarea.Usuario = IdUsuario;
                //        }
                //        break;
                //    case 110:
                //        tarea.NegId = negId;
                //        tarea.HoraInicio = DateTime.Now;
                //        tarea.IdEtapa = 90;
                //        tarea.IdEstado = 80;
                //        //Referenciacion
                //        break;
                //    case 0:
                //        tarea.NegId = negId;
                //        tarea.HoraInicio = DateTime.Now;
                //        tarea.HoraTerminacion = DateTime.Now;
                //        tarea.IdEtapa = 90;
                //        tarea.IdEstado = 90;

                //        //Consulta
                //        break;
                //    case 111:

                //        if (firma.Equals('0'))
                //        {
                //            tarea.NegId = negId;
                //            tarea.HoraInicio = DateTime.Now;
                //            tarea.IdEtapa = 90;
                //            tarea.IdEstado = 80;
                //            //Referenciacion
                //        }
                //        else
                //        {
                //            tarea.NegId = negId;
                //            tarea.HoraInicio = DateTime.Now;
                //            tarea.HoraTerminacion = DateTime.Now;
                //            tarea.IdEtapa = 90;
                //            tarea.IdEstado = 30;
                //            tarea.Usuario = IdUsuario;
                //        }
                //        break;
                //    case 11:
                //        tarea.NegId = negId;
                //        tarea.HoraInicio = DateTime.Now;
                //        tarea.IdEtapa = 90;
                //        tarea.IdEstado = 80;

                //        //Referenciacion
                //        break;
                //    default:
                //        tarea.NegId = negId;
                //        tarea.HoraInicio = DateTime.Now;
                //        tarea.HoraTerminacion = DateTime.Now;
                //        tarea.IdEtapa = 90;
                //        tarea.IdEstado = 30;
                //        tarea.Usuario = IdUsuario;
                //        break;
                //}

                //this.gd.AddToAsignacionTareas(tarea);
                //this.gd.SaveChanges();

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaController metodo CrearEtapasBizagi " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }

        }
        #endregion
    }
}
