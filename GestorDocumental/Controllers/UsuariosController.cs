using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using GestorDocumental.Controllers;
using GestorDocumental.wsLogin;

namespace GestorDocumental.Controllers
{
    public class UsuariosController : Controller
    {
        private GestorDocumentalEnt gd = new GestorDocumentalEnt();
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();

        public ActionResult index()
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
            var result = dbo.spValidaAccesoModulo(idRol, "/Usuarios/Index").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            return View();
        }

        /// <summary>
        /// Cambia la contraña de ususario
        /// </summary>
        public ActionResult CambioContrasena(string passActual, string confPass)
        {
            try
            {
                ControlUsuarios val = new ControlUsuarios();
                Usuarios user = new Usuarios();

                //<<JFPancho;6-abril-2017; valida si el usuario logueado tiene accceso al modulo
                int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
                var result = gd.spValidaAccesoModulo(idRol, "/Usuarios/CambioContrasena").FirstOrDefault();

                if (result == 0)
                {
                    Response.Redirect("../Home/Index");
                }
                //JFPancho >>

                user.IdUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;

                //Validar que la contraseña nueva ingresada no exista en los historicos.
                string passEncript = this.encryptar(confPass);
                bool resultadoValidacion = val.ConsultarHistoricos(user.IdUsuario.ToString(), passEncript);

                if (resultadoValidacion)
                {
                    return View("ErrorContrasena");
                    //throw new Exception("La nueva contraseña ingresada fue usada anteriormente.");
                }

                string password = gd.Usuarios.Where(x => x.IdUsuario == user.IdUsuario).Select(z => z.PassCodeUsuario).First().ToString();
                if (this.encryptar(passActual).Equals(password))
                {
                    bool totalHistoricos = val.ConsultarTotalHistoricos(user.IdUsuario.ToString());
                    if (totalHistoricos)
                    {
                        //Cuando pase de 4 borra el registro mas antiguo y crea uno nuevo
                        val.actualizarHistoricos(user.IdUsuario.ToString(), passEncript);
                    }
                    else
                    {
                        int mesesCaducidad = val.ConsultarMesesCaducidad();

                        //Cuando el total de registros no supera 4 se debe insertar uno nuevo
                        UsuariosHistorico dataHistorico = new UsuariosHistorico()
                        {
                            idUsuario = Convert.ToDecimal(user.IdUsuario.ToString()),
                            PassCodeUsuario = this.encryptar(confPass),
                            FechaCaducidad = DateTime.Now.AddMonths(mesesCaducidad),
                            Activo = true
                        };

                        dbo.AddToUsuariosHistorico(dataHistorico);
                        dbo.SaveChanges();
                    }
                    gd.sp_CambioPassword(int.Parse(user.IdUsuario.ToString()), confPass);
                }
                else
                {
                    return View("contrasenaerror");
                }
                return View();
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en UsuariosController metodo CambioContrasena " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
        }

        public string ValidarCaducidadContrasena(string idUsuario, string passActual)
        {
            ControlUsuarios valUsuarios = new ControlUsuarios();
            string passEncript = encryptar(passActual);

            string respuesta = valUsuarios.ConsultarCaducidad(idUsuario, passEncript);
            return respuesta;
        }

        public ActionResult EdicionUsuarios()
        {
            ViewData["P_Roles"] = this.obtenerRolesUsuario();
            ViewData["Clientes"] = this.obtenerClientes();
            return View();
        }

        public string encryptar(string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, password);
                return hash;
            }

        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public void GuardarUsuario1(Usuarios U)
        {
            try
            {
                using (this.gd = new GestorDocumentalEnt())
                {
                    if (this.gd.Usuarios.Any<Usuarios>(O => O.IdUsuario == U.IdUsuario))
                    {
                        Usuarios usuarios = this.gd.Usuarios.First<Usuarios>(i => i.IdUsuario == U.IdUsuario);
                        usuarios.IdUsuario = U.IdUsuario;
                        usuarios.NomUsuario = U.NomUsuario;
                        usuarios.RolId = U.RolId;
                        usuarios.CliNit = U.CliNit;
                        usuarios.PassCodeUsuario = U.PassCodeUsuario;
                        this.gd.SaveChanges();
                    }
                    else
                    {
                        this.gd.AddToUsuarios(U);
                        this.gd.SaveChanges();
                    }
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en UsuariosController metodo GuardarUsuario " + exception.Message);
                throw exception;
            }
        }

        public List<P_Roles> obtenerRolesUsuario()
        {
            List<P_Roles> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<P_Roles> list = (from r in this.gd.P_Roles select r).ToList<P_Roles>();
                P_Roles item = new P_Roles
                {
                    RolId = 0,
                    DescRol = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en UsuariosController metodo obtenerRolesUsuario " + exception.Message);
                throw exception;
            }
            return list2;
        }

        public List<Clientes> obtenerClientes()
        {
            List<Clientes> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<Clientes> list = (from r in this.gd.Clientes select r).ToList<Clientes>();
                Clientes item = new Clientes
                {
                    CliNit = 0,
                    CliNombre = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en UsuariosController metodo Obtener clientes " + exception.Message);
                throw exception;
            }
            return list2;
        }

        public Usuarios obtenerUsuario(Usuarios U)
        {
            Usuarios usuarios2;
            try
            {
                Usuarios usuarios = null;
                using (this.gd = new GestorDocumentalEnt())
                {
                    if (this.gd.Usuarios.Any<Usuarios>(O => O.IdUsuario == U.IdUsuario))
                    {
                        usuarios = this.gd.Usuarios.First<Usuarios>(i => i.IdUsuario == U.IdUsuario);
                    }
                }
                usuarios2 = usuarios;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en UsuariosController metodo obtenerUsuario " + exception.Message);
                throw exception;
            }
            return usuarios2;
        }

        public bool ValidarUsuarios(Usuarios Usu)
        {
            ControlUsuarios ctrl = new ControlUsuarios();
            bool seguridadHabilitada = ctrl.ConsultaSeguridadHabilitada();

            if (seguridadHabilitada)
            {
                bool respuestaOn = ValUsuariosSeguridadOn(Usu);
                return respuestaOn;
            }
            else
            {
                bool respuestaOff = ValUsuariosSeguridadOff(Usu);
                return respuestaOff;
            }
        }

        public bool ValUsuariosSeguridadOn(Usuarios Usu)
        {
            try
            {
                ControlUsuarios db = new ControlUsuarios();
                if (System.Web.HttpContext.Current.Session["contador"] == null)
                {
                    System.Web.HttpContext.Current.Session["contador"] = 0;
                }

                bool usuarioBloq = db.consultarUsuarioBloqueado(Usu.IdUsuario.ToString());
                if (!usuarioBloq)
                {
                    bool respuesta = true;
                    if (obtenerTipoLogin() == 1)
                    {
                        respuesta = db.validarUsuario(Usu.IdUsuario.ToString(), Usu.PassCodeUsuario.ToString());
                        //JFPancho; mayo/2016;se valida que el usuario este activo
                        int rptaAct = Convert.ToInt32(gd.spValidarUsuarioAct(Convert.ToInt32(Usu.IdUsuario), Usu.PassCodeUsuario.ToString()).First());
                        if (rptaAct == 2)
                        {
                            System.Web.HttpContext.Current.Session["activo"] = 0;
                            respuesta = false;
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Session["activo"] = 1;
                        }
                    }
                    else
                    {
                        string usuarioWS = obtenerUsuarioWS(Usu.IdUsuario);
                        SingleSignOn access = new SingleSignOn();
                        bool resultado = access.AutenticarUsuario(usuarioWS, Usu.PassCodeUsuario);
                        if (resultado)
                            respuesta = true;
                        else
                            respuesta = false;
                    }

                    if (respuesta)
                    {
                        System.Web.HttpContext.Current.Session["contador"] = null;
                        return true;
                    }
                    else
                    {
                        bool verificarUsuario = db.consultarUsuarioExistente(Usu.IdUsuario.ToString());
                        if (verificarUsuario)
                        {
                            int contador = Convert.ToInt32(System.Web.HttpContext.Current.Session["contador"].ToString());

                            contador = contador + db.contadorErrores(false);
                            if (contador == 3)
                            {
                                db.bloquearUsuario(Usu.IdUsuario.ToString(), contador);
                                System.Web.HttpContext.Current.Session["contador"] = null;
                                throw new Exception("El usuario: " + Usu.IdUsuario.ToString() + " ha sido bloqueado");
                            }
                            System.Web.HttpContext.Current.Session["contador"] = contador;
                        }
                        return false;
                    }
                }
                else
                {
                    System.Web.HttpContext.Current.Session["contador"] = null;
                    throw new Exception("El usuario se encuentra bloqueado");
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en UsuariosController metodo ValidarUsuarios " + exception.Message);
                throw exception;
            }
        }

        public bool ValUsuariosSeguridadOff(Usuarios Usu)
        {
            bool flag = false;
            try
            {
                if (obtenerTipoLogin() == 1)
                {
                    string respuesta = this.gd.spValidarUsuario((int)Usu.IdUsuario, Usu.PassCodeUsuario).First();
                    if (respuesta.Equals("1"))
                    {
                        flag = true;
                    }
                }
                else
                {
                    string usuarioWS = obtenerUsuarioWS(Usu.IdUsuario);
                    SingleSignOn access = new SingleSignOn();
                    bool resultado = access.AutenticarUsuario(usuarioWS, Usu.PassCodeUsuario);
                    if (resultado)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en UsuariosController metodo ValidarUsuarios " + exception.Message);
                throw exception;
            }
            return flag;
        }

        public int obtenerTipoLogin()
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            var query = (from a in dbo.Parametros
                         where a.codigo == "TIPO_LOGIN"
                         select a.valor).SingleOrDefault();
            return Convert.ToInt32(query);
        }

        public string obtenerUsuarioWS(decimal idUsuario)
        {
            GestorDocumentalEnt dbo = new GestorDocumentalEnt();
            var query = (from a in dbo.Usuarios
                         where a.IdUsuario == idUsuario
                         select a.UsuarioDominio).SingleOrDefault();
            return query.ToString();
        }

        public ActionResult NuevoUsuario(GridEditMode? mode, GridButtonType? type, GridInsertRowPosition?
           insertRowPosition)
        {
            //<<JFPancho;6-abril-2017;  
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = gd.spValidaAccesoModulo(idRol, "/Usuarios/NuevoUsuario").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            Session["TITULO"] = "Administracion Usuarios";
            ViewData["mode"] = mode ?? GridEditMode.InLine;
            ViewData["type"] = type ?? GridButtonType.Text;
            ViewData["insertRowPosition"] = insertRowPosition ?? GridInsertRowPosition.Top;
            return View();
        }

        [GridAction]
        public ActionResult _SelectAjaxEditing()
        {
            return View(new GridModel(UsuariosModel.GetAll()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _SaveAjaxEditing(decimal id)
        {
            UsuariosPropiedades usuario = UsuariosModel.One(p => p.idUsusario == id);

            TryUpdateModel(usuario);
            string idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario.ToString();
            UsuariosModel.Update(usuario, idUsuario);
            return View(new GridModel(UsuariosModel.GetAll()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _InsertAjaxEditing()
        {
            //Create a new instance of the EditableProduct class.
            UsuariosPropiedades usuario = new UsuariosPropiedades();
            //Perform model binding (fill the product properties and validate it).
            if (TryUpdateModel(usuario))
            {
                //The model is valid - insert the product.
                UsuariosModel.Insert(usuario);
            }
            //Rebind the grid
            return View(new GridModel(UsuariosModel.GetAll()));
        }

        public ActionResult DesbloquearUsuarios()
        {
            //<<JFPancho;6-abril-2017;  
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = gd.spValidaAccesoModulo(idRol, "/Usuarios/DesbloquearUsuarios").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            return View();

        }

        [GridAction]
        public ActionResult _UsuariosBloqueados()
        {
            var grilla = _grillaUsBloqueados();
            return View(new GridModel<listaDesbloquearUsuarios>(grilla));
        }

        private IEnumerable<listaDesbloquearUsuarios> _grillaUsBloqueados()
        {
            List<Sp_List_DesbUsuarios_Result> list = gd.Sp_List_DesbUsuarios().ToList();

            List<listaDesbloquearUsuarios> ListRetor = new List<listaDesbloquearUsuarios>();
            for (int i = 0; i < list.Count; i++)
            {
                listaDesbloquearUsuarios temp = new listaDesbloquearUsuarios();
                temp.ID_Usuario = list[i].IdUsuario + "";
                temp.UsuDominio = list[i].UsuarioDominio;
                temp.NomUsuario = list[i].NomUsuario;
                temp.Rol = list[i].DescRol;
                temp.fecha = list[i].FechaBloqueo;
                temp.bloqueado = "Bloqueado";
                ListRetor.Add(temp);
            }

            return ListRetor;
        }

        
        //Debido a que el metodo de Eliminacion no es necesario se ha comentariado esta parte
        //
        //[AcceptVerbs(HttpVerbs.Post)]
        //[GridAction]
        //public ActionResult _DeleteAjaxEditing(decimal id)
        //{
        //    //Find a customer with ProductID equal to the id action parameter
        //    UsuariosPropiedades product = UsuariosModel.One(p => p.idUsuario == id);
        //    if (product != null)
        //    {
        //        //Delete the record
        //        UsuariosModel.Delete(product);
        //    }

        //    //Rebind the grid
        //    return View(new GridModel(UsuariosModel.GetAll()));
        //}

        [GridAction]
        public void ActivarUsuario(decimal Id_Usuario)
        {
            gd.sp_EstadoUsuario(Id_Usuario, false);
            return;
        }

        [GridAction]
        public void ResetPass(decimal Id_Usuario)
        {
            gd.sp_ResetPassUsuario(Id_Usuario, false);
            return;
        } 
    }
    public class listaDesbloquearUsuarios
    {
        public string ID_Usuario { get; set; }
        public string UsuDominio { get; set; }
        public string NomUsuario { get; set; }
        public string Rol { get; set; }
        public DateTime? fecha { get; set; }
        public string bloqueado { get; set; }
    }


}
