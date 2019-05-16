using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class ServicioCargueController : Controller
    {
        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Servicio de cargue";
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

        [GridAction]
        public ActionResult _AjaxBinding()
        {
            var grilla = grillaLotes();
            return View(new GridModel<GrillaServicio>(grilla));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Importar(string lote)
        {
            RecepcionModel recep = new RecepcionModel();
            AuditoriaModel am = new AuditoriaModel();
            Auditoria nuevaAuditoria = new Auditoria();
            recep.update(int.Parse(lote));

            nuevaAuditoria.aud_idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
            nuevaAuditoria.aud_fechaHora = DateTime.Now;
            nuevaAuditoria.aud_evento = "Importe Lotes";
            nuevaAuditoria.aud_observaciones = "Se importa lote No. " + lote + " del cliente " + ((Clientes)Session["CLIENTE"]).CliNombre;
            //Inserta Auditoria
            am.AddAuditoria(nuevaAuditoria);

            var grilla = grillaLotes();
            return View(new GridModel<GrillaServicio>(grilla));
        }

        private IEnumerable<GrillaServicio> grillaLotes()
        {
            CargueController nueva = new CargueController();
            RecepcionModel recep = new RecepcionModel();
            string folderToBrowse = @"" + nueva.DirectorioLotes(((GestorDocumental.Models.Clientes)Session["CLIENTE"]));
            DirectoryInfo DirInfo = new DirectoryInfo(folderToBrowse);

            var datosCarpetas = DirInfo.GetFileSystemInfos();

            var datosrecepcion = recep.getAllSinCargar(((GestorDocumental.Models.Clientes)Session["CLIENTE"]));


            var grilla = datosCarpetas.Join(datosrecepcion,
                    Carpetas => (Carpetas.Name),
                    LotesRecepcion => (LotesRecepcion.numeroLote).ToString(),
                    (Carpetas, LotesRecepcion) => new { Carpetas = Carpetas, Lotes = LotesRecepcion })
                    .Select(x => new GrillaServicio()
                    {
                        Lote = x.Carpetas.Name,
                        FechaCreacion = x.Carpetas.CreationTime,
                        Principlales = x.Lotes.principales,
                        Estado = x.Lotes.estado == 99 ? "Pendiente" : "En proceso"
                    });

            return grilla;
        }
    }

    public class GrillaServicio
    {
        public string Lote { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string Estado { get; set; }
        public decimal? Principlales { get; set; }
    }
}
