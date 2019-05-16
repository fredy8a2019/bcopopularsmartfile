using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    /// <summary>
    /// Autor: Elena Parra
    /// Fecha Creacion: 24 - 10 - 2013
    /// Descripcion: Controla la interfaz de mesa de control 
    /// 
    /// MODIFICACIONES:
    /// </summary>
    public class MesaController : Controller
    {
        private AsignacionTareasModel asignacion = new AsignacionTareasModel();
        private AsignacionesController asigControl = new AsignacionesController();
        private CapturaController capControl = new CapturaController();
        private CapturaModel cm = new CapturaModel();
        private DatosReferenciacionModelo datRefe = new DatosReferenciacionModelo();
        private GestorDocumentalEnt db = new GestorDocumentalEnt();

        /// <summary>
        /// Carga la pagina de referenciacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return LoadPage(80);
        }

        /// <summary>
        /// Carla la pagina busqueda contactro
        /// </summary>
        /// <returns></returns>
        public ActionResult Busqueda()
        {
            return LoadPage(90);
        }

        public ActionResult GuardarCambios(String correo, String fechaNacimiento, String celular, String IdNeg, String observaciones, String firma, String aceptacion)
        {
            try
            {
                var arrayValues = new string[,]{ {"10",correo}, 
                                            {"8",fechaNacimiento}, 
                                            {"9",celular},
                                            {"53",firma},
                                            {"51",aceptacion}};

                InsetarMesaCaptura(arrayValues, decimal.Parse(IdNeg));

                DatosReferenciacion nuevo = new DatosReferenciacion();

                nuevo.DR_NegId = decimal.Parse(IdNeg);
                nuevo.DR_Fomularios = 1;
                nuevo.DR_Ususario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                nuevo.DR_Observaciones = observaciones;
                nuevo.DR_FechaRegisto = DateTime.Now;
                datRefe.insertData(nuevo);

                return View();

            }
            catch (Exception es)
            {
                LogRepository.registro("Error en CruceIdentidadController metodo GuardarCambios" + es.Message + " stack " + es.StackTrace);
                throw;
            }

        }

        public ActionResult GuardarCambiosBusqueda(String telFijo, String numCelular, String observaciones, String IdNeg)
        {
            try
            {
                DatosReferenciacion nuevo = new DatosReferenciacion();

                nuevo.DR_NegId = decimal.Parse(IdNeg);
                nuevo.DR_Fomularios = 2;
                nuevo.DR_Ususario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                nuevo.DR_Telefono = telFijo;
                nuevo.DR_Celular = numCelular;
                nuevo.DR_Observaciones = observaciones;
                nuevo.DR_FechaRegisto = DateTime.Now;

                datRefe.insertData(nuevo);
                asignacion.UpdateEstapaYEstado(90, 90, int.Parse(IdNeg), 80);

                return View();
            }
            catch (Exception es)
            {
                 LogRepository.registro("Error en CruceIdentidadController metodo InsetarMesaCaptura" + es.Message + " stack " + es.StackTrace);
                throw;
            }
            
        }

        public ActionResult ReasignarNegocio(String estado, String negID)
        {
            try
            {
                //Referenciacion
                if (estado == "110")
                {
                    asignacion.UpdateEstapaYEstado(90, 80, int.Parse(negID), 110);
                    ViewData["estado"] = 110;
                }
                //Busqueda de contacto
                else if (estado == "100")
                {
                    asignacion.UpdateEstapaYEstado(90, 90, int.Parse(negID), 100);
                    ViewData["estado"] = 100;
                }
                return View();
            }
            catch (Exception es)
            {
                
                LogRepository.registro("Error en CruceIdentidadController metodo ReasignarNegocio" + es.Message + " stack " + es.StackTrace);
                throw;
            }
            return View();
        }

        public ActionResult Error()
        {

            return View();
        }

        /// <summary>
        /// Carga la pagina dependiendo Referenciacion  o Busqueda Contacto
        /// </summary>
        /// <param name="estado">se carga por el estado</param>
        /// <returns></returns>
        public ActionResult LoadPage(int estado)
        {
            try
            {
                if(estado == 80)
                    Session["TITULO"] = "Referenciaciòn";
                else if(estado == 90)
                    Session["TITULO"] = "Busqueda de contacto";

                P_Etapas nuevaEtapa = new P_Etapas();
                AsignacionTareas tareasEtapa = new AsignacionTareas();
                AsignacionTareas tareaEtapaEstado = new AsignacionTareas();
                nuevaEtapa.IdEtapa = 90;

                if (asigControl.ObtenerNegociosXEntrada((Usuarios)Session["USUARIO_LOGUEADO"], nuevaEtapa) != 0M)
                {
                    tareasEtapa = asignacion.getTareas(90, (Usuarios)Session["USUARIO_LOGUEADO"]);
                    tareaEtapaEstado = asignacion.getTareas(90, (Usuarios)Session["USUARIO_LOGUEADO"], estado, int.Parse(tareasEtapa.NegId.ToString()));
                }
                else
                {
                    asigControl.AsignarUsuarioEtapa((Usuarios)Session["USUARIO_LOGUEADO"], estado);
                    tareasEtapa = asignacion.getTareas(90, (Usuarios)Session["USUARIO_LOGUEADO"]);
                    tareaEtapaEstado = asignacion.getTareas(90, (Usuarios)Session["USUARIO_LOGUEADO"], estado, int.Parse(tareasEtapa.NegId.ToString()));
                }


                if (tareaEtapaEstado == null)
                {
                    Session["ERROR"] = AsignacionTareasModel.error;
                    return base.Redirect("/Mesa/Error");
                }

                ViewData["Id_Neg"] = tareaEtapaEstado.NegId;

                GetDataByNegId((decimal)tareaEtapaEstado.NegId, estado);
                return View();
            }
            catch (Exception es)
            {
                Session["ERROR"] = AsignacionTareasModel.error;
                return base.Redirect("/Mesa/Error");
                throw;
            }

        }

        /// <summary>
        /// Obtiene todos los datos del negocio en su ultima captura y los datos de contacto busqueda
        /// </summary>
        /// <param name="negId">Id del negocio</param>
        public void GetDataByNegId(decimal negId, int estado)
        {
            try
            {
                DatosReferenciacion consulta = new DatosReferenciacion();

                int i = 1;
                var datosNeg = cm.GetValueCaptura(negId);

                if (estado == 80)
                {
                    consulta = datRefe.GetAsignacionByIdNeg(negId, 2);
                }

                foreach (var item in datosNeg)
                {
                    ViewData["DatosIdNeg" + i] = item;
                    i++;
                }

                    ViewData["EnabledFecha"] = true;
                if (!(ViewData["DatosIdNeg8"] == null || ViewData["DatosIdNeg8"].ToString().Trim() == ""))
                {
                    ViewData["EnabledFecha"] = false;
                }

                ViewData["DatosIdNeg1"] = ViewData["DatosIdNeg1"] == null || ViewData["DatosIdNeg1"] == string.Empty ? string.Empty : cm.GetDescriptionTD(ViewData["DatosIdNeg1"].ToString());
                ViewData["DatosIdNeg11"] = ViewData["DatosIdNeg11"] == null || ViewData["DatosIdNeg11"] == string.Empty ? string.Empty : cm.GetNameDepartamentoById(int.Parse(ViewData["DatosIdNeg11"].ToString()));
                ViewData["DatosIdNeg12"] = ViewData["DatosIdNeg12"] == null || ViewData["DatosIdNeg12"] == string.Empty ? string.Empty : cm.GetNameCiudadById(int.Parse(ViewData["DatosIdNeg12"].ToString()));

                ViewData["ValueChekFirma"] = ViewData["DatosIdNeg53"] == "true" || ViewData["DatosIdNeg53"] != string.Empty ? "1" : "0";

                ViewData["ValueChekTerminos"] = ViewData["DatosIdNeg51"] == "true" || ViewData["DatosIdNeg51"] != string.Empty ? "1" : "0";

                ViewData["busqTel"] = consulta.DR_Telefono;
                ViewData["busqCel"] = consulta.DR_Celular;
                ViewData["busqObser"] = consulta.DR_Observaciones;
            }
            catch (Exception es)
            {
                
               LogRepository.registro("Error en CruceIdentidadController metodo GetDataNegId" + es.Message + " stack " + es.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Inserta la camptura 4 en la tabla capturas
        /// </summary>
        /// <param name="arrayValues"></param>
        /// <param name="IdNeg"></param>
        public void InsetarMesaCaptura(string[,] arrayValues, decimal IdNeg)
        {
            try
            {
                
            List<Captura> lisCaptura = new List<Captura>();

            for (int i = 0; i < 5; i++)
            {
                if (arrayValues[i, 1] != null)
                {
                    if ( arrayValues[i, 1] != string.Empty)
                    {
                        Captura nuevaCaptura = new Captura();
                        nuevaCaptura.NegId = IdNeg;
                        nuevaCaptura.NumCaptura = 4;
                        nuevaCaptura.CampId = int.Parse(arrayValues[i, 0]);
                        nuevaCaptura.NegValor = arrayValues[i, 1];
                        nuevaCaptura.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        nuevaCaptura.FechaRegistro = DateTime.Now;
                        nuevaCaptura.DocId = int.Parse(cm.GetDogId(nuevaCaptura.CampId));
                        lisCaptura.Add(nuevaCaptura);
                    }
                   
                }
               
            }

            capControl.InsertarCaptura(lisCaptura);
            asignacion.cierraEtapa(int.Parse(IdNeg.ToString()), 90);
            }
            catch (Exception es)
            {

                LogRepository.registro("Error en CruceIdentidadController metodo InsetarMesaCaptura" + es.Message + " stack " + es.StackTrace);
                throw;
            }

        }
    }
}


