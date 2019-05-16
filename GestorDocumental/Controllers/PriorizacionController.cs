using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class PriorizacionController : Controller
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        public ActionResult Index()
        {
            //<<JFPancho;6-abril-2017; valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = db.spValidaAccesoModulo(idRol, "/Priorizacion/Index").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Priorizacion";
                Session["CURRENT_FILE"] = "";
                ViewData["menu"] = "";
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        public ActionResult Priorizar(FormCollection collection)
        {
            string mensaje = "";
            AuditoriaModel am = new AuditoriaModel();
            Auditoria nuevaAuditoria = new Auditoria();

            nuevaAuditoria.aud_idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
            nuevaAuditoria.aud_fechaHora = DateTime.Now;
            nuevaAuditoria.aud_evento = "Priorizaciones";


            //BUSCO EN LOS STRING ESTOS CARACTERES ESPECIALES Y LOS REEMPLAZO, PRINCIPALMENTE EN LA SOCIEDAD
            char cr = (char)13;
            char lf = (char)10;
            char tab = (char)9;

            if (!string.IsNullOrEmpty(collection["txtNegocios"]) || !string.IsNullOrWhiteSpace(collection["txtNegocios"]))
            {
                mensaje = "Negocios";
                this.Priorizar(collection["txtNegocios"].ToString().Trim().Replace("\r", "").Replace("\n", "").Replace(cr, ' ').Replace(lf, ' ').Replace(tab, ' ').Trim(), null, null);
                nuevaAuditoria.aud_observaciones = "Se priorizo los siguientes " + mensaje + " : " + collection["txtNegocios"].ToString();

            }
            else if (!string.IsNullOrEmpty(collection["txtCodBarras"]) || !string.IsNullOrWhiteSpace(collection["txtCodBarras"]))
            {
                mensaje = "Codigos de barras";
                this.Priorizar(null, collection["txtCodBarras"].ToString().Trim().Replace("\r", "").Replace("\n", "").Replace(cr, ' ').Replace(lf, ' ').Replace(tab, ' ').Trim(), null);
                nuevaAuditoria.aud_observaciones = "Se priorizo los siguientes " + mensaje + " : " + collection["txtCodBarras"].ToString();

            }
            else if (!string.IsNullOrEmpty(collection["txtLote"]) || !string.IsNullOrWhiteSpace(collection["txtLote"]))
            {
                mensaje = "Lotes";
                this.Priorizar(null, null, collection["txtLote"].ToString().Trim().Replace("\r", "").Replace("\n", "").Replace(cr, ' ').Replace(lf, ' ').Replace(tab, ' ').Trim());
                nuevaAuditoria.aud_observaciones = "Se priorizo los siguientes " + mensaje + " : " + collection["txtLote"].ToString();

            }

            //Inserta Auditoria
            am.AddAuditoria(nuevaAuditoria);
            return Content("<script language='javascript' type='text/javascript'> alert('" + mensaje + "  priorizados'); window.location.href = '/Priorizacion/Index'; </script>");
        }

        /// <summary>
        /// Ejecuta el sp de priorizacion
        /// </summary>
        private void Priorizar(string negocios, string codBarras, string lotes)
        {
            try
            {
                db.spPriorizacion(negocios, codBarras, lotes);
            }
            catch (Exception es)
            {

                throw;
            }

        }
    }
}
