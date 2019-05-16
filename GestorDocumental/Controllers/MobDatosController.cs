using System;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class MobDatosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult _obtenerListaClientes()
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().P_Sociedades.ToList(), "Cod_Sociedad", "Descripcion"),
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RecepcionController metodo _GetDropDownList_Oficinas " + exception.Message);
                throw;
            }
        }

        [HttpPost]
        public void GuardarDatos(FormCollection valores)
        {
            GestorDocumentalEnt tablas = new GestorDocumentalEnt();
            Usuarios datosUsuario = (Usuarios)Session["USUARIO"];

            string _documento = valores["txtDocumento"].ToString();
            string _proveedor = valores["txtNoProveedor"].ToString();
            string _sociedad = valores["cmbSociedad"].ToString();
            string _observaciones = valores["txtObservaciones"].ToString();
            int _idUsuario = Convert.ToInt32(datosUsuario.IdUsuario);
            int _idConsecutivo = Convert.ToInt32(Session["ID_MOVIL"].ToString());

            MobDatos nuevaFila = new MobDatos
            {
                idUsuario = _idUsuario,
                fecha = DateTime.Now,
                idMobDoc = _documento,
                idSociedad = _sociedad,
                numProveedor = _proveedor,
                numRad = _idConsecutivo,
                observaciones = _observaciones
            };

            tablas.AddToMobDatos(nuevaFila);
            tablas.SaveChanges();

            Response.Redirect("../Mobile/Index");
        }
    }
}
