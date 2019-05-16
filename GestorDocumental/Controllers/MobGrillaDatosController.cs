using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class MobGrillaDatosController : Controller
    {
        GestorDocumentalEnt tablas = new GestorDocumentalEnt();
        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Consola de resultados XML";
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
        public ActionResult _verMobDatos()
        {
            var grilla = _grillaModDatos();
            return View(new GridModel<_mobDatos>(grilla));
        }

        [GridAction]
        public ActionResult _verDetalleGrilla(int idRad)
        {
            var grilla = _detalleGrilla(idRad);
            return View(new GridModel<_detalleGrilla>(grilla));
        }

        #region Grilla Mob Datos
        private IEnumerable<_mobDatos> _grillaModDatos()
        {
            var grilla = (from ws in tablas.MobDatos
                          join mob in tablas.P_Sociedades on ws.idSociedad equals mob.Cod_Sociedad
                          select new _mobDatos()
                          {
                              _fecha = ws.fecha,
                              _idMobDoc = ws.idMobDoc,
                              _idSociedad = mob.Descripcion,
                              _numProveedor = ws.numProveedor,
                              _numRad = ws.numRad,
                              _observaciones = ws.observaciones
                          }).OrderByDescending(d => d._numRad);

            return grilla;
        }

        private IEnumerable<_detalleGrilla> _detalleGrilla(int _idRad)
        {
            var grilla = (from ws in tablas.MobCaptura
                          join l in tablas.MobListaChequeo on ws.idListaChequeo equals l.id
                          join d in tablas.MobDocumentos on l.mobDocId equals d.id
                          where ws.numRad == _idRad
                          select new _detalleGrilla()
                          {
                              _numRadicacion = ws.numRad,
                              _descripcionLChequeo = l.descripcion,
                              _descripcionDocumento = d.descripcion
                          });

            return grilla;
        }
        #endregion
    }

    public class _mobDatos
    {
        public DateTime? _fecha { get; set; }
        public string _idMobDoc { get; set; }
        public string _idSociedad { get; set; }
        public string _numProveedor { get; set; }
        public long? _numRad { get; set; }
        public string _observaciones { get; set; }
    }

    public class _detalleGrilla
    {
        public int? _numRadicacion { get; set; }
        public string _descripcionLChequeo { get; set; }
        public string _descripcionDocumento { get; set; }
    }
}
