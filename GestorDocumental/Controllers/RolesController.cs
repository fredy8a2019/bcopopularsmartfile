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
    public class RolesController : Controller
    {
        //
        // GET: /Roles/
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();
        public ActionResult Index()
        {
            if (Session["CLIENTE"] == null)
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
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

        #region Grilla Administracion Roles Activados
        [HttpPost]
        [GridAction]
        public JsonResult getInfoRolesActivados()
        {
            var grilla = _getInfoRolesActivados();
            return Json(new GridModel<grillaPRoles>(grilla));
        }

        public IEnumerable<grillaPRoles> _getInfoRolesActivados()
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            var grilla = (from a in dbo.P_Roles
                          where a.RolId != 1 && a.Activo == true
                          select a).ToList();
            List<P_Roles> _grillaAlm = grilla;

            List<grillaPRoles> lstPRoles = new List<grillaPRoles>();
            foreach (var item in _grillaAlm)
            {
                grillaPRoles data = new grillaPRoles();
                data.idRol = item.RolId.ToString();

                if (item.Activo == true)
                    data.activo = "Verdadero";
                else
                    data.activo = "Falso";

                data.nombreRol = item.DescRol;
                lstPRoles.Add(data);
            }

            return lstPRoles;
        }
        #endregion

        #region Grilla Administracion Roles Desactivados
        [HttpPost]
        [GridAction]
        public JsonResult getInfoRolesDesactivados()
        {
            var grilla = _getInfoRolesDesactivados();
            return Json(new GridModel<grillaPRoles>(grilla));
        }

        public IEnumerable<grillaPRoles> _getInfoRolesDesactivados()
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            var grilla = (from a in dbo.P_Roles
                          where a.RolId != 1 && a.Activo == false
                          select a).ToList();
            List<P_Roles> _grillaAlm = grilla;

            List<grillaPRoles> lstPRoles = new List<grillaPRoles>();
            foreach (var item in _grillaAlm)
            {
                grillaPRoles data = new grillaPRoles();
                data.idRol = item.RolId.ToString();

                if (item.Activo == true)
                    data.activo = "Verdadero";
                else
                    data.activo = "Falso";

                data.nombreRol = item.DescRol;
                lstPRoles.Add(data);
            }

            return lstPRoles;
        }
        #endregion

        #region Grilla Menu - Roles Asociados
        [HttpPost]
        [GridAction]
        public JsonResult getRolesMenu(int idRol)
        {
            var grilla = _getRolesMenu(idRol);
            return Json(new GridModel<grillaRolesMenu>(grilla));
        }

        public IEnumerable<grillaRolesMenu> _getRolesMenu(int idRol)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            var grilla = dbo.sp_ConsultaRolesAsociados(idRol);

            List<grillaRolesMenu> lstGrilla = new List<grillaRolesMenu>();
            foreach (sp_ConsultaRolesAsociados_Result item in grilla)
            {
                grillaRolesMenu data = new grillaRolesMenu();
                data.idMenu = item.IdMenu;
                data.descripcionMenu = item.DescMenu;

                lstGrilla.Add(data);
            }

            return lstGrilla;
        }
        #endregion

        #region Grilla Menu - Roles Faltantes

        [HttpPost]
        [GridAction]
        public JsonResult getRolesMenuFaltantes(int idRol)
        {
            var grilla = _getRolesMenuFaltantes(idRol);
            return Json(new GridModel<grillaRolesMenu>(grilla));
        }

        public IEnumerable<grillaRolesMenu> _getRolesMenuFaltantes(int idRol)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            var grilla = dbo.sp_ConsultaRolesFaltantes(idRol);

            List<grillaRolesMenu> lstGrilla = new List<grillaRolesMenu>();
            foreach (sp_ConsultaRolesFaltantes_Result item in grilla)
            {
                grillaRolesMenu data = new grillaRolesMenu();
                data.idMenu = item.IdMenu;
                data.descripcionMenu = item.DescMenu;

                lstGrilla.Add(data);
            }

            return lstGrilla;
        }
        #endregion

        [HttpPost]
        public string desactivarRol(int idRol, string nombreRol)
        {
            try
            {
                GestorDocumentalEnt db = new GestorDocumentalEnt();
                db.sp_DesactivarRoles(idRol);

                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Se ha desactivado el rol: " + nombreRol + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Correcto";
            }
            catch (Exception ex)
            {
                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Error desactivando el Rol: " + ex.Message + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Incorrecto";
            }
        }

        [HttpPost]
        public string activarRol(int idRol, string nombreRol)
        {
            try
            {
                GestorDocumentalEnt db = new GestorDocumentalEnt();
                db.sp_ActivarRoles(idRol);

                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Se ha activado el rol: " + nombreRol + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Correcto";
            }
            catch (Exception ex)
            {
                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Error activando el Rol: " + ex.Message + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Incorrecto";
            }
        }

        [HttpPost]
        public string crearRol(string nombreRol)
        {
            try
            {
                P_Roles data = new P_Roles();
                data.DescRol = nombreRol;
                data.Activo = true;

                GestorDocumentalEnt db = new GestorDocumentalEnt();
                db.P_Roles.AddObject(data);
                db.SaveChanges();

                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Se ha creado el rol: " + nombreRol + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Correcto";
            }
            catch (Exception ex)
            {
                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Error creando el Rol: " + ex.Message + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Incorrecto";
            }
        }

        [HttpPost]
        public string eliminarMenuRol(int idRol, int idMenu, string nombreRol, string nombreModulo)
        {
            try
            {
                GestorDocumentalEnt db = new GestorDocumentalEnt();
                db.sp_EliminarRolesMenu(idRol, idMenu);

                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Se ha eliminado el modulo: " + nombreModulo + " del rol: " + nombreRol + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Correcto";
            }
            catch (Exception ex)
            {
                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Error eliminando la relacion Rol-Menu: " + ex.Message + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Incorrecto";
            }
        }

        [HttpPost]
        public string asociarMenuRol(int idRol, int idMenu, string nombreRol, string nombreModulo)
        {
            try
            {
                GestorDocumentalEnt db = new GestorDocumentalEnt();
                db.sp_AsociarMenuRol(idRol, idMenu);

                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Se ha agregado el modulo: " + nombreModulo + " al rol: " + nombreRol + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Correcto";
            }
            catch (Exception ex)
            {
                //Generamos el log del proceso que acabamos de realizar.
                string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                mensajeLog = mensajeLog + "    Error creando la relacion Rol-Menu: " + ex.Message + " Usuario: " + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                escribirLog(mensajeLog);

                return "Incorrecto";
            }
        }

        public string obtenerRutaLog()
        {
            GestorDocumentalEnt db = new GestorDocumentalEnt();
            var ruta = (from a in db.Parametros
                        where a.codigo == "RUTA_LOG_ROLES"
                        select a.valor).SingleOrDefault();

            return ruta;
        }

        public void escribirLog(string mensaje)
        {
            string rutaLog = obtenerRutaLog();
            string nombreArchivoLog = "AdministracionRoles_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
            rutaLog = rutaLog + nombreArchivoLog;

            if (System.IO.File.Exists(rutaLog))
            {
                System.IO.StreamWriter archivo = new System.IO.StreamWriter(rutaLog, true);
                mensaje = mensaje + " " + "IP:" + obtenerIP();
                archivo.WriteLine(mensaje);

                archivo.Close();
            }
            else
            {
                System.IO.StreamWriter archivo = new System.IO.StreamWriter(rutaLog);
                mensaje = mensaje + " " + "IP:" + obtenerIP();
                archivo.WriteLine(mensaje);

                archivo.Close();
            }
        }

        public string obtenerIP()
        {
            string localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily.ToString().ToUpper().Equals("INTERNETWORK")).FirstOrDefault().ToString();

            string strHostName = System.Net.Dns.GetHostName();
            string struser = null;
            System.Security.Principal.WindowsIdentity user = System.Security.Principal.WindowsIdentity.GetCurrent();
            struser = user.Name;

            return localIp + " Equipo:" + strHostName + " Usuario:" + struser;
        }
    }

    public class grillaPRoles
    {
        public string idRol { get; set; }
        public string nombreRol { get; set; }
        public string activo { get; set; }
    }

    public class grillaRolesMenu
    {
        public int idMenu { get; set; }
        public string descripcionMenu { get; set; }
    }
}
