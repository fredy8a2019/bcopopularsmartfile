using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;


namespace GestorDocumental.Controllers
{
    public class LoteoController : Controller
    {
        private LoteoImagenModel loteoModel = new LoteoImagenModel();
        private LoteoCambiosModel loteoCambios = new LoteoCambiosModel();
        private GestorDocumentalEnt bd = new GestorDocumentalEnt();
        private AsignacionTareasModel asignacion = new AsignacionTareasModel();
        private int etapaLoteo = 60;

        public ActionResult Save(IEnumerable<HttpPostedFileBase> attachments)
        {

            try
            {
                // The Name of the Upload component is "attachments" 
                foreach (var file in attachments)
                {
                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);

                    this.InsertarCambio((int)Session["CURRENT_PAGE"], 0, 0, physicalPath, 1);
                }
                // Return an empty string to signify success
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en LoteoController metodo Save " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
            
            return Content("");
        }

        public ActionResult Index()
        {
            try
            {
                Session["TITULO"] = "Loteo de imágenes";
                AsignacionTareas tareas = asignacion.getTareasLoteo(etapaLoteo, (Usuarios)Session["USUARIO_LOGUEADO"]);

                if (tareas == null)
                {
                    Session["ERROR"] = AsignacionTareasModel.error;
                    return base.Redirect("/Loteo/error");
                }

                Parametros param = bd.Parametros.First(c => c.codigo == "PATH_VISOR");
                Parametros param1 = bd.Parametros.First(c => c.codigo == "PATH_TOTAL");

                if (Session["IMG_VISOR"] == null)
                {
                    LoteoImagen imagen = bd.LoteoImagen.Where(c => c.NegId == tareas.NegId).First();

                    //VERIFICO QUE EL ARCHIVO QUE VOY A MOSTRAR REALMENTE EXISTA
                    if (!System.IO.File.Exists(param1.valor + tareas.NegId + @"\" + imagen.imagenLoteada))
                    {
                        if (System.IO.File.Exists(param1.valor + tareas.NegId + @"\" + tareas.NegId + ".tif"))
                        {
                            imagen.imagenLoteada = tareas.NegId + ".tif";
                            imagen.rutaImagen = param1.valor + tareas.NegId + @"\" + tareas.NegId + ".tif";
                            bd.SaveChanges();
                        }

                    }

                    //ESTABLEZCO LA IMAGEN DEL VISOR
                    Session["IMG_VISOR"] = param.valor + tareas.NegId + "/" + imagen.imagenLoteada;

                    //SI EL CAMPO DE IMAGEN LOTEADA ES NULO O NO TIENE VALOR ENTONCES POR DEFAULT ES EL MISMO NOMBRE DEL NEGOCIO
                    if (imagen.imagenLoteada == null || String.IsNullOrEmpty(imagen.imagenLoteada))
                    {
                        Session["IMG_VISOR"] = param.valor + tareas.NegId + "/" + tareas.NegId + ".tif";
                    }
                    Session["IMG_RUTA"] = param.valor + tareas.NegId + "/";
                    Session["IMG_RUTA_TOTAL"] = param1.valor + tareas.NegId + @"\";
                }

                Session["ID_NEGOCIO"] = tareas.NegId;
                Session["ID_IMAGEN_LOTEO"] = (int)loteoModel.selectImagenNegocio((int)tareas.NegId).id;
                ViewBag.cambios = loteoCambios.selectCambiosImagen((int)Session["ID_IMAGEN_LOTEO"]);
                return View();
            }
            catch (Exception e)
            {
                //Session["ERROR"] = AsignacionTareasModel.error + "<br>" + e.Message;
                LogRepository.registro("Error en LoteoController metodo Index " + e.Message + " stack trace " + e.StackTrace);
                return base.Redirect("/Loteo/error");                 
            }
            
            
        }

        //ESTABLEZCO POR SESSION LA PAGINA ACTUAL
        public ActionResult setCurrentPage(int pagina)
        {
            try
            {
                Session["CURRENT_PAGE"] = pagina;
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en LoteoController metodo setCurrentPage " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
            
            return View();
        }

        //ESTABLEZCO POR SESSION LA PAGINA ACTUAL
        public ActionResult error()
        {
            return View();
        }

        //FINALIZO LA IMAGEN Y LA ENVIO PARA QUE SEA LOTEADA
        public ActionResult finalizarImagen()
        {
            try
            {
                loteoModel.updateImagen((int)Session["ID_IMAGEN_LOTEO"]);
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en LoteoController metodo finalizarImagen " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
            
            return View();
        }

        //DESPUES DE ESPERAR A QUE LA IMEGN SEA LOTEADA MUSTRO LA NUEVA IMAGEN CREADA, CON DISTINTO NOMBRE
        public ActionResult actualizaImagen()
        {
            try
            {
                //MUESTRO LA NUEVA IMAGEN LOTEADA            
                string imagenLoteada = loteoModel.selectImagen((int)Session["ID_IMAGEN_LOTEO"]).imagenLoteada;
                Session["IMG_VISOR"] = Session["IMG_RUTA"] + imagenLoteada;
                string nuevaImagen = Session["IMG_RUTA_TOTAL"] + imagenLoteada;
                loteoModel.updateImagen((int)Session["ID_IMAGEN_LOTEO"], nuevaImagen);
                //BORRO LOS CAMBIOS QUE ENVIE PARA ESA IMAGEN
                loteoCambios.deleteCambios((int)Session["ID_IMAGEN_LOTEO"]);
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en LoteoController metodo actualizaImagen " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
            
            return View("Index");
        }

        public ActionResult finLoteo()
        {
            try
            {
                string imagenLoteada = loteoModel.selectImagen((int)Session["ID_IMAGEN_LOTEO"]).rutaImagen;
                asignacion.cierraEtapa(Int32.Parse(Session["ID_NEGOCIO"].ToString()), etapaLoteo);
                //VUELVO A DEJAR LA IMAGEN TAL CUAL ESTABA EN UN PRINCIPIO CON EL NUEMRO DEL NEGOCIO
                if (System.IO.File.Exists(imagenLoteada))
                {
                    System.IO.File.Move(imagenLoteada, bd.Parametros.First(c => c.codigo == "PATH_TOTAL").valor + @"\" + Session["ID_NEGOCIO"].ToString() + @"\" + Session["ID_NEGOCIO"].ToString() + ".tif");
                }
                Session["IMG_VISOR"] = null;
                return base.Redirect("/Loteo/Index");
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en LoteoController metodo actualizaImagen " + e.Message + " stack trace " + e.StackTrace);
                Session["ERROR"] = Session["ERROR"] + e.Message;
                return base.Redirect("/Loteo/error"); 
                throw;
            }            
        }

        [GridAction]
        public ActionResult SelectCambios()
        {

            return View(new GridModel(new LoteoCambiosModel().selectCambiosImagen((int)Session["ID_IMAGEN_LOTEO"])));
        }
                
        public ActionResult InsertarCambio(int pagina,int eliminar, int rotar,String rutaAdicionar,int adicionar)
        {
            try
            {
                LoteoCambios cambio = new LoteoCambios();
                cambio.pagina = pagina;
                cambio.idImagen = (int)Session["ID_IMAGEN_LOTEO"];
                cambio.rotar = rotar;
                cambio.eliminar = eliminar;
                cambio.adicionar = adicionar;
                cambio.rutaAdicionar = rutaAdicionar;
                new LoteoCambiosModel().insert(cambio);
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en LoteoController metodo InsertarCambio " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
            
            return View();
        }
    }
}
