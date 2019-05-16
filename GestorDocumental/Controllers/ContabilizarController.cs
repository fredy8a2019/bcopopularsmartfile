using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class ContabilizarController : Controller
    {
        //
        // GET: /Contabilizar/

        private GestorDocumentalEnt bd = new GestorDocumentalEnt();

        public ActionResult Index(String negId, String label, String producto, String SubProducto, String NoDocumentos, String Proveedor, String sociedad)
        {
            Parametros param = bd.Parametros.First(c => c.codigo == "TOOL_VIS_REP");
            Parametros paramVisor = bd.Parametros.First(c => c.codigo == "PATH_VISOR");

            var negId2 = int.Parse(negId);

            var numPaginas = bd.CargueLotes.First(c => c.NegId == negId2).Paginas;

            Session["TOOL_BAR_REP"] = param.valor.ToString();
            Session["IMG_VISOR_REP"] = paramVisor.valor + negId + "/" + negId + ".tif";
            Session["IMG_DOWN_PDF"] = paramVisor.valor + negId + "/" + negId + ".pdf";

            ViewData["paginas"] = numPaginas;
            
            ViewData["NEG"] = negId;

            Session["negID"] = negId;

            ViewData["Cliente"] = ((Usuarios)Session["USUARIO"]).NomUsuario;

            ViewData["Lable"] = label;
            ViewData["Producto"] = producto;
            ViewData["SubProducto"] = SubProducto;
            ViewData["NoDocumento"] = NoDocumentos;
            ViewData["Proveedor"] = Proveedor;
            ViewData["Sociedad"] = sociedad;

            return View();
        }

        /// <summary>
        /// Gurada los cambia del negocio en el modulo de contabilizacion 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Guardar(FormCollection collection)
        {
            try
            {
                CamposController camposCon = new CamposController();
                CapturaController capCon = new CapturaController();
                AsignacionTareasModel asigTarMod = new AsignacionTareasModel();

                List<Captura> listCaptura = new List<Captura>();
                var campos = camposCon.getCamposFormulario(0);
                //var values = (collection["_value"]).Split(',').ToArray();
                int i = 0;
                foreach (var item in campos)
                {
                    Captura nueva = new Captura();
                    if (Extends.ObtenerValorReflexion(item, "CampDescripcion").ToString() == "Estado")
                    {
                        AsignacionTareas nuevaAT = new AsignacionTareas();
                        nuevaAT.NegId = decimal.Parse(Session["negID"].ToString());
                        nuevaAT.IdEtapa = 120;
                        nuevaAT.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        nuevaAT.HoraInicio = DateTime.Now;
                        nuevaAT.HoraTerminacion = DateTime.Now.AddMinutes(5);
                        nuevaAT.IdEstado = int.Parse(collection[i].ToString());
                        //Inserto la asignacion de tareas
                        asigTarMod.Add(nuevaAT);

                        nueva.NegId = decimal.Parse(Session["negID"].ToString());
                        nueva.NumCaptura = 4;
                        nueva.CampId = int.Parse(Extends.ObtenerValorReflexion(item, "CampId").ToString());
                        nueva.Indice = 0;
                        nueva.NegValor = _getDescripcion(int.Parse(collection[i].ToString()));
                        nueva.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        nueva.FechaRegistro = DateTime.Now;
                        nueva.DocId = 0;
                        nueva.NegIdBizagi = null;

                    }
                    else
                    {
                        nueva.NegId = decimal.Parse(Session["negID"].ToString());
                        nueva.NumCaptura = 4;
                        nueva.CampId = int.Parse(Extends.ObtenerValorReflexion(item, "CampId").ToString());
                        nueva.Indice = 0;
                        nueva.NegValor = collection[i];
                        nueva.Usuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        nueva.FechaRegistro = DateTime.Now;
                        nueva.DocId = 0;
                        nueva.NegIdBizagi = null;

                    }
                    listCaptura.Add(nueva);
                    i++;
                }
                //Inserto la lista de captura
                capCon.InsertarCaptura(listCaptura);

                return Content("<script language='javascript' type='text/javascript'>  window.close(); </script>");
            }
            catch (Exception exception)
            {

                LogRepository.registro("Error en RadicacionController metodo Guardar " + exception.Message);
                return Content("<script language='javascript' type='text/javascript'> alert('Error!! no se pudo \n guardar los cambios'); window.close(); </script>");
                throw;
            }

        }

        /// <summary>
        /// Obtiene los campos que del formulario.
        /// </summary>
        /// <param name="CodOrigen"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult _GetCampos(int CodOrigen)
        {
            try
            {
                CamposController camposCon = new CamposController();
                var campos = camposCon.getCamposFormulario(CodOrigen);

                return Json(campos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetCampos " + exception.Message);
                throw;
            }

        }

        /// <summary>
        /// Otiene la lista de los estados en los que un negocio puedes estar cuando se va a contabilizar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult _GetDropDownListEstados()
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().P_Estados.Where(c => c.Tipo == 2).ToList(), "IdEstado", "DescEstado"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownListEstados " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los valore de negocio cuando ha sido contabilizado
        /// </summary>
        /// <param name="negId">Negocio</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult _GetValorCampos(string negId)
        {
            try
            {
                CapturaController capControll = new CapturaController();
                var datos = capControll.getCapturaContabilidad(decimal.Parse(negId));
                return Json(datos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetValorCampos " + exception.Message);
                throw;
            }

        }

        /// <summary>
        /// Obtiene la descripcion del estado
        /// </summary>
        /// <param name="idEstado"></param>
        /// <returns></returns>
        public string _getDescripcion(int idEstado)
        {
            return bd.P_Estados.Where(x => x.IdEstado == idEstado).First<P_Estados>().DescEstado;
        }
    }
}
