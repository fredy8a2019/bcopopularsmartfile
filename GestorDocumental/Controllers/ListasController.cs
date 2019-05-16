using System;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class ListasController : Controller
    {
        private GestorDocumentalEnt _conexion;
        public ListasController()
        {
            _conexion = new GestorDocumentalEnt();
        }

        [HttpPost]
        public JsonResult _GetDropDownListClientes()
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().Clientes.ToList(),
                                           "CliNit", "CliNombre"),
                           JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownListClientes " + exception.Message);
                throw;
            }
        }

        [HttpPost]
        public JsonResult _GetDropDownListProductos(int? DropDownList_Clientes)
        {
            try
            {

                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().Grupos.Where(c => c.CliNit == DropDownList_Clientes).ToList(), "GruId", "GruDescripcion"),

                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownListProductos " + exception.Message);
                throw;
            }
        }

        [HttpPost]
        public JsonResult _GetDropDownListSubProductos(int? DropDownList_Productos)
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().Grupos.Where(c => c.GruIdPadre == DropDownList_Productos).ToList(), "GruId", "GruDescripcion"),
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownListSubProductos " + exception.Message);
                throw;
            }

        }

        [HttpPost]
        public JsonResult _GetDropDownList_Oficinas(int? DropDownList_Clientes)
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().P_Oficinas.Where(c => c.OFI_CodNit == DropDownList_Clientes).ToList(), "OFI_IdOficina", "OFI_Nombre"),

                JsonRequestBehavior.AllowGet);
            }

            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownList_Oficinas " + exception.Message);
                throw;
            }

        }

        [HttpPost]
        public JsonResult _GetDropDownList_Sociedades(int? DropDownList_Clientes)
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().P_Sociedades.Where(c => c.CliNit == DropDownList_Clientes).ToList(), "Cod_Sociedad", "Descripcion"),

                JsonRequestBehavior.AllowGet);
            }

            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownList_Oficinas " + exception.Message);
                throw;
            }

        }

        [HttpPost]
        public JsonResult _GetDropDownListEstados()
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().P_Estados.Where(c => c.Tipo == 1).ToList(), "IdEstado", "DescEstado"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownListEstados " + exception.Message);
                throw;
            }
        }

        [HttpPost]
        public JsonResult _GetDropDownRolesApp()
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().P_Roles.Where(x => x.Activo == true).ToList(), "RolId", "DescRol"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownRolesApp " + exception.Message);
                throw;
            }
        }

        [HttpPost]
        public JsonResult _GetProductosDelNegocio()
        {
            try
            {
                GestorDocumentalEnt db = new GestorDocumentalEnt();

                return Json(new SelectList(db.Grupos.Join(db.TiposGrupos,
                                grupos => grupos.TipGruId,
                                tipos => tipos.TipGruId,
                                (grupos, tipos) => new { Grupo = grupos, Tipo = tipos })
                                .Where(x => x.Grupo.GruIdPadre == 0).GroupBy(x => new { x.Tipo.TipGruId,x.Tipo.TipDescripcion})
                                .Select(x => new { TipGruId = x.Key.TipGruId , TipDescripcion = x.Key.TipDescripcion }), "TipGruId", "TipDescripcion"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetProductosDelNegocio " + exception.Message);
                throw;
            }
        }

        ///ALMACENAMIENTO
        /// <summary>
        /// Obtiene todos los tipo de contenedores que pueden existir en un archivo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _GetTipoContenedor()
        {
            try
            {
                return Json(new SelectList(_conexion.alm_TipoUD.ToList(), "Id", "Tipo"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception es)
            {

                LogRepository.registro("Error en RadicacionController metodo _GetTipoContenedor " + es.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos lo posibles destinos que tiene una unidad documental
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _GetDestino()
        {
            try
            {
                return Json(new SelectList(_conexion.alm_DestinoUD.ToList(), "Id", "Destino"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception es)
            {

                LogRepository.registro("Error en RadicacionController metodo _GetDestino " + es.Message);
                throw;
            }

        }
    }
}
