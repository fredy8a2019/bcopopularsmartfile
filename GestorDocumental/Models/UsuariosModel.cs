using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net;

namespace GestorDocumental.Models
{
    public class UsuariosModel
    {
        private static GestorDocumentalEnt db = new GestorDocumentalEnt();

        public static IList<UsuariosPropiedades> GetAll()
        {
            IList<UsuariosPropiedades> resultado =
                (IList<UsuariosPropiedades>)HttpContext.Current.Session["Usuarios"];

            if (resultado == null)
            {
                var datosUsusarios = db.spUsuariosAplicacion();

                HttpContext.Current.Session["Usuarios"] = resultado =
                        datosUsusarios.Select(x => new UsuariosPropiedades
                        {
                            idUsusario = x.IdUsuario,
                            Nombre = x.NomUsuario,
                            usuarioDominio = x.UsuarioDominio,
                            nitCliente = x.CliNombre,
                            rol = x.DescRol,
                            activo = x.Activo,
                            impresora = x.Periferico_Impresora,
                            FechaRegistro = x.FecRegistro

                        }).ToList();
            }

            return resultado;
        }

        public static UsuariosPropiedades One(Func<UsuariosPropiedades, bool> predicate)
        {
            return GetAll().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Logica necesaria para Crear un nuevo usuario
        /// </summary>
        /// <param name="usuario"></param>
        public static void Insert(UsuariosPropiedades usuario)
        {
            UsuariosPropiedades usuarioActualizado = new UsuariosPropiedades();

            usuarioActualizado.idUsusario = usuario.idUsusario;
            usuarioActualizado.Nombre = usuario.Nombre;
            usuarioActualizado.rol = RolName(int.Parse(usuario.rol));
            usuarioActualizado.nitCliente = NombreCliente(decimal.Parse(usuario.nitCliente));
            usuarioActualizado.FechaRegistro = usuario.FechaRegistro;
            usuarioActualizado.impresora = usuario.impresora;
            usuarioActualizado.usuarioDominio = usuario.usuarioDominio;

            GetAll().Insert(0, usuarioActualizado);
            db.sp_GuardarNuevoUsuario(long.Parse(usuario.idUsusario.ToString()), usuario.Nombre, long.Parse(usuario.nitCliente), int.Parse(usuario.rol), usuario.activo, usuario.impresora, usuario.usuarioDominio);
        }

        /// <summary>
        /// Logica necesaria para Actualizar un usuario
        /// </summary>
        /// <param name="usuario"></param>
        public static void Update(UsuariosPropiedades usuario, string _idUsuario)
        {
            UsuariosPropiedades target = One(p => p.idUsusario == usuario.idUsusario);
            if (target != null)
            {
                try
                {
                    target.Nombre = usuario.Nombre;
                    target.nitCliente = NombreCliente(decimal.Parse(usuario.nitCliente)); ;
                    target.rol = RolName(int.Parse(usuario.rol));
                    target.activo = usuario.activo;
                    target.impresora = usuario.impresora;
                    target.usuarioDominio = usuario.usuarioDominio;

                    Usuarios update = db.Usuarios.Where(x => x.IdUsuario == usuario.idUsusario).First();
                    update.NomUsuario = usuario.Nombre;
                    update.CliNit = NItCliente(usuario.nitCliente);
                    update.RolId = IdRol(usuario.rol);
                    update.Activo = usuario.activo;
                    update.UsuarioDominio = usuario.usuarioDominio;

                    if (usuario.activo == true)
                    {
                        //Generamos el log del proceso que acabamos de realizar.
                        string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        mensajeLog = mensajeLog + "    Se ha activado el Usuario:" + usuario.idUsusario + " Usuario que ejecuta:" + _idUsuario;
                        UsuariosModel process = new UsuariosModel();
                        process.escribirLog(mensajeLog);
                    }
                    else if (usuario.activo == false)
                    {
                        //Generamos el log del proceso que acabamos de realizar.
                        string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        mensajeLog = mensajeLog + "    Se ha desactivado el Usuario:" + usuario.idUsusario + " Usuario que ejecuta:" + _idUsuario;
                        UsuariosModel process = new UsuariosModel();
                        process.escribirLog(mensajeLog);
                    }

                    update.Periferico_Impresora = usuario.impresora;
                    db.SaveChanges();

                    //Generamos el log del proceso que acabamos de realizar.
                    string mensajeLogUp = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    mensajeLogUp = mensajeLogUp + "    Se han actualizado los datos del Usuario:" + usuario.idUsusario + " Usuario que ejecuta:" + _idUsuario;
                    UsuariosModel processUp = new UsuariosModel();
                    processUp.escribirLog(mensajeLogUp);
                }
                catch (Exception ex)
                {
                    //Generamos el log del proceso que acabamos de realizar.
                    string mensajeLog = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    mensajeLog = mensajeLog + "    Error actualizando el Usuario: " + ex.Message + " Usuario que ejecuta:" + _idUsuario;
                    UsuariosModel processUp = new UsuariosModel();
                    processUp.escribirLog(mensajeLog);
                }
            }
        }

        //Debido a que el metodo de Eliminacion no es necesario se ha comentariado esta parte
        //
        ///// <summary>
        ///// Logica necesaria para eliminar un usuario
        ///// </summary>
        ///// <param name="usuario"></param>
        //public static void Delete(UsuariosPropiedades usuario)
        //{
        //    UsuariosPropiedades target = One(p => p.idUsuario == usuario.idUsuario);
        //    if (target != null)
        //    {
        //        GetAll().Remove(target);               
        //    }

        //    Usuarios update = db.Usuarios.Where(x => x.IdUsuario == usuario.idUsuario).First();
        //    update.Activo = false;
        //    db.SaveChanges();

        //}

        /// <summary>
        /// Obtiene la descripcion de cliente  
        /// </summary>
        private static string NombreCliente(decimal nitCliente)
        {
            try
            {
                return db.Clientes.Where(x => x.CliNit == nitCliente).First().CliNombre;
            }
            catch (Exception es)
            {
                return "";
                throw;
            }
        }

        /// <summary>
        ///Obtener el nit cliente 
        /// </summary>
        private static decimal NItCliente(string nomCliente)
        {
            try
            {
                return db.Clientes.Where(x => x.CliNombre == nomCliente).First().CliNit;
            }
            catch (Exception es)
            {
                return 0M;
                throw;
            }
        }

        /// <summary>
        /// Obtiene la descripcion del rol
        /// </summary>
        private static string RolName(int id)
        {
            try
            {
                return db.P_Roles.Where(x => x.RolId == id).First().DescRol;
            }
            catch (Exception es)
            {

                throw;
                return "";
            }
        }

        /// <summary>
        ///Obtener el id del rol 
        /// </summary>
        private static int IdRol(string nomRol)
        {
            try
            {
                return db.P_Roles.Where(x => x.DescRol == nomRol).First().RolId;
            }
            catch (Exception es)
            {
                return 0;
                throw;
            }
        }

        /// <summary>
        /// Obtiene la Ruta Fisica en donde se van a guardar los archivos Log
        /// </summary>
        /// <returns></returns>
        public string obtenerRutaLog()
        {
            GestorDocumentalEnt db = new GestorDocumentalEnt();
            var ruta = (from a in db.Parametros
                        where a.codigo == "RUTA_LOG_USUARIOS"
                        select a.valor).SingleOrDefault();

            return ruta;
        }

        /// <summary>
        /// Se construye el mensaje en el log, y se escribe en el archivo
        /// </summary>
        /// <param name="mensaje"></param>
        public void escribirLog(string mensaje)
        {
            string rutaLog = obtenerRutaLog();
            string nombreArchivoLog = "AdministracionUsuarios_" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
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

        /// <summary>
        /// Se obtiene y captura la IP del equipo que esta realizando el proceso.
        /// </summary>
        /// <returns></returns>
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

    /// <summary>
    /// Autor   : Elena Parra
    /// Fecha   : 30 - 01 - 2014
    /// Decription: Decribe la propiedades que debe tener los usuarios de la aplicacion.
    /// </summary>
    [KnownType(typeof(UsuariosPropiedades))]
    public class UsuariosPropiedades
    {
        [Required]
        [DataType("String")]
        public decimal idUsusario { get; set; }

        [DisplayName("Usuario Dominio")]
        public string usuarioDominio { get; set; }

        [Required]
        [DisplayName("Nombre Completo")]
        public string Nombre { get; set; }

        [Required]
        [DataType("String")]
        public string nitCliente { get; set; }

        [DataType("Integer")]
        public string rol { get; set; }

        public bool? activo { get; set; }

        [DisplayName("Nombre Impresora")]
        public string impresora { get; set; }

        [DisplayName("Fecha Registo")]
        [DataType(DataType.Date)]
        public DateTime? FechaRegistro { get; set; }
    }
}