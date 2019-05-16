using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;
using System.Web.UI;

namespace GestorDocumental.Controllers
{
    public class CrearArchivoController : Controller
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        public ActionResult CrearArchivo()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Crear Archivo";
                ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
                ViewData["nomArchivo"] = "";

                if (Session["nomArchivo"] != null)
                {
                    ViewData["nomArchivo"] = Session["nomArchivo"].ToString();
                }

                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        //Genera el codigo del archivo que se esta creando
        [HttpGet]
        public JsonResult GenArchivo()
        {
            decimal idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            var codArchivo = db.spAlm_GeneraCodArchivo(idUsuario).First().codArchivo;
            return Json(codArchivo, JsonRequestBehavior.AllowGet);
        }

        //Genera el codigo del modulo del archivo que se esta creando
        [HttpGet]
        public JsonResult GenCodModulo(long _codArchivo)
        {
            var _codModulo = db.spAlm_GeneraCodModulo(_codArchivo);
            return Json(_codModulo, JsonRequestBehavior.AllowGet);
        }

        //Crea el registro en la base de datos del archivo que se ha creado
        [HttpPost]
        public void InstDatArchivo(long _codArchivo, string _nomArchivo)
        {
            decimal idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            db.spAlm_InsertArchivo(_codArchivo, _nomArchivo, idUsuario);
            Session["nomArchivo"] = null;
        }

        //Crea los modulos y estructura del archivo creado
        [HttpPost]
        public void InstModulos(long codArchivo, long codModulo, string nomModulo, int cantNiveles, int cantSubN, string nomArchivo)
        {
            Session["nomArchivo"] = nomArchivo;
            db.spAlm_InsertModulo(codArchivo, codModulo, nomModulo, cantNiveles, cantSubN);
        }

        #region Grilla de los archivos creados
        [HttpPost]
        [GridAction]
        public virtual ActionResult getDetalleM(long _codArchivo, long _codModulo)
        {
            var grillaM = _getDetalleM(_codArchivo, _codModulo);
            return View(new GridModel<LlenarGrilla>(grillaM));
        }

        public IEnumerable<LlenarGrilla> _getDetalleM(long _codArchivo, long _codModulo)
        {
            var grillaM = db.spAlm_ConsModulos(_codArchivo, _codModulo);
            List<LlenarGrilla> _grillaMod = new List<LlenarGrilla>();

            foreach (spAlm_ConsModulos_Result item in grillaM)
            {
                LlenarGrilla data = new LlenarGrilla();
                data._codModulo = long.Parse(item.cod_modulo.ToString());
                data._nomModulo = item.desc_modulo;
                data._cantN = item.cant_niveles;
                data._cantSN = item.cant_subniveles;

                _grillaMod.Add(data);
            }
            return _grillaMod;
        }
        #endregion
    }
    public class LlenarGrilla
    {
        public long _codModulo { get; set; }
        public string _nomModulo { get; set; }
        public int _cantN { get; set; }
        public int _cantSN { get; set; }
    }
}
