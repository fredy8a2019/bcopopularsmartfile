using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;
using System.IO;
using System.Net;

namespace GestorDocumental.Controllers
{
    public class ParametrosGeneralesController : Controller
    {
        //
        // GET: /ParametrosGenerales/
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();
        public ActionResult Index()
        {
            //<<JFPancho;6-abril-2017;  
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = dbo.spValidaAccesoModulo(idRol, "/Roles/Index").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            return View();
        }

        #region Grilla Parametros Generales

        [HttpPost]
        [GridAction]
        public ActionResult getInfoParametros()
        {
            var grilla = _getInfoParametros();
            return Json(new GridModel<grillaParametrosGenerales>(grilla));
        }

        public IEnumerable<grillaParametrosGenerales> _getInfoParametros()
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            var grilla = dbo.spGrillaParametrosGenerales();

            List<spGrillaParametrosGenerales_Result> _grillaParametros = grilla.ToList();

            List<grillaParametrosGenerales> lstParametros = new List<grillaParametrosGenerales>();
            foreach (var item in _grillaParametros)
            {
                grillaParametrosGenerales data = new grillaParametrosGenerales();
                data.id = item.id;
                data.Codigo = item.codigo;
                data.Valor = item.valor;
                data.Descripcion = item.descripcion;

                lstParametros.Add(data);
            }

            return lstParametros;
        }


        #endregion

        [HttpPost]
        public ActionResult editarInformacion(FormCollection collection)
        {
            int _id = Convert.ToInt32(collection["txtID"]);
            string _codigo = collection["txtCodigo"];
            string _valor = collection["txtValor"];
            string _descripcion = collection["txtDescripcion"];

            dbo.spActualizarParametros(_id, _codigo, _valor, _descripcion);

            return View("Index");
        }

        [HttpPost]
        public ActionResult insertarParametro(FormCollection collection)
        {
            string _codigo = collection["txtCodigo"];
            string _valor = collection["txtValor"];
            string _descripcion = collection["txtDescripcion"];

            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            dbo.spInsertarParametro(_codigo, _valor, _descripcion);

            return View("Index");
        }
    }

    public class grillaParametrosGenerales
    {
        public int id { get; set; }
        public string Codigo { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
    }
}
