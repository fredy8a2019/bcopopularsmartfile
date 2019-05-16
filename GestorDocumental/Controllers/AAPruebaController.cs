using GestorDocumental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class AAPruebaController : Controller
    {
        //
        // GET: /AAPrueba/
        GestorDocumentalEnt db = new GestorDocumentalEnt();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [GridAction]
        public virtual ActionResult ListAjax(long dato)
        {
            var grilla = _grillaResultado(dato);
            return View(new GridModel<grillaResultadoAlm>(grilla));
        }

        private IEnumerable<grillaResultadoAlm> _grillaResultado(long numero)
        {
            UnidadDocumentalModel modeloUD = new UnidadDocumentalModel(db);
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();

            var grilla = dbo.spAlm_ConsultaDataUD(numero);
            List<grillaResultadoAlm> _grillaAlm = new List<grillaResultadoAlm>();

            foreach (spAlm_ConsultaDataUD_Result item in grilla)
            {
                grillaResultadoAlm data = new grillaResultadoAlm();
                data._CUD = item.CUD;
                data._CliNombre = item.CliNombre;
                data._Tipo = item.Tipo;
                data._FechaCreacion = item.FechaCreacion;
                data._UsuarioCreacion = item.UsuarioCreacion;
                data._Destino = item.Destino;
                data._Subproductos = item.SubProductos;

                _grillaAlm.Add(data);
            }
            return _grillaAlm;
        }
    }

    public class grillaResultadoAlm
    {
        public long _CUD { get; set; }
        public string _CliNombre { get; set; }
        public string _Tipo { get; set; }
        public DateTime? _FechaCreacion { get; set; }
        public decimal? _UsuarioCreacion { get; set; }
        public string _Destino { get; set; }
        public string _Subproductos { get; set; }
    }
}
