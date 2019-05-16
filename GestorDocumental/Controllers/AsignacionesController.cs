using System;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class AsignacionesController : Controller
    {
        private GestorDocumentalEnt gd;
        private System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog("LogGestorDocumental", "GestorDocumental", "GestorDocumentalLog");

        public ActionResult finIndexacion()
        {
            return base.Redirect("/ViewsAspx/IndexacionImg.aspx");
        }

        public bool ExisteEtapa(AsignacionTareas A)
        {
            bool flag2;
            try
            {
                bool flag = false;
                if (this.gd.AsignacionTareas.Any<AsignacionTareas>(O => (O.NegId == A.NegId) & (O.IdEtapa == A.IdEtapa)))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                flag2 = flag;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en AsignacionesController metodo ExisteEtapa " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return flag2;
        }

        public void insertarAsignacion(AsignacionTareas A)
        {
            try
            {
                this.gd = new GestorDocumentalEnt();
                if (this.gd.AsignacionTareas.Any<AsignacionTareas>(O => (O.NegId == A.NegId) & (O.IdEtapa == A.IdEtapa)))
                {
                    AsignacionTareas tareas = this.gd.AsignacionTareas.First<AsignacionTareas>(i => (i.NegId == A.NegId) & (i.IdEtapa == A.IdEtapa));
                    tareas.HoraTerminacion = new DateTime?(DateTime.Now);
                    tareas.IdEstado = 30;
                    this.gd.SaveChanges();
                    //Session["NEGOCIO"] = null;
                    
                }
                else
                {
                    this.gd.AddToAsignacionTareas(A);
                    this.gd.SaveChanges();
                }
                
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en AsignacionesController metodo insertarAsignacion " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        public decimal ObtenerNegociosXEntrada(Usuarios U, P_Etapas E)
        {
            decimal num;
            try
            {
                this.gd = new GestorDocumentalEnt();
                //string xx = this.gd.spObtenerSiguienteEtapa(new decimal?(U.IdUsuario), new int?(E.IdEtapa)).FirstOrDefault().Column1.Value.ToString();
                num = decimal.Parse(this.gd.spObtenerSiguienteEtapa(new decimal?(U.IdUsuario), new int?(E.IdEtapa)).First<decimal?>().ToString());
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en AsignacionesController metodo ObtenerNegociosXEntrada " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return num;
        }

        /// <summary>
        /// Asigna un nuevo ususario a la etapa mesa de control
        /// </summary>
        /// <param name="usuario">El ususario de la session que ha iniciado</param>
        public void AsignarUsuarioEtapa(Usuarios usuario, int estado)
        {
            try
            {
                gd = new GestorDocumentalEnt();

                var coleccionNegID = this.gd.CargueLotes.OrderBy(x => x.prioridad)
                                                        .OrderBy(x => x.LoteFecha)
                                                        .Where(x => x.ArchivoCargado == true && x.Terminado == false)
                                                        .Select(x => x.NegId).AsParallel();

                var coleccionAsig = this.gd.AsignacionTareas.Where(x => x.IdEtapa == 90 && x.IdEstado == estado && x.Usuario == 0).Select(x => new { x.NegId, x.Usuario }).AsParallel();
                int Control = 0;
                decimal num = 0;
                AsignacionTareas nuevo = new AsignacionTareas();

                foreach (var item in coleccionNegID)
                {
                    if (Control == 0)
                    {
                        foreach (var itemBusqueda in coleccionAsig)
                        {
                            if (item == itemBusqueda.NegId)
                            {
                                num = item;
                                Control = 1;
                                break;
                            }
                        }
                    }
                }

                nuevo = this.gd.AsignacionTareas.First<AsignacionTareas>(i => (i.NegId == num) && (i.IdEtapa == 90) && (i.IdEstado == estado));
                nuevo.Usuario = usuario.IdUsuario;
                this.gd.SaveChanges();
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en AsignacionesController metodo AsignarUsuarioEtapa " + es.Message + " stack trace " + es.StackTrace);                
                throw;
            }
        }
    }
}
