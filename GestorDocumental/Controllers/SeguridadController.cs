using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc.UI;

namespace GestorDocumental.Controllers
{
    public class SeguridadController : Controller
    {
        public string prefijo_menu = "";
        GestorDocumentalEnt data = new GestorDocumentalEnt();

        public ActionResult Index()
        {
            data.Connection.Close();
            return View();
        }

        public string consultaTotalCapturas(string idUsuario)
        {
            List<spConsultarTotalCapturas_Result> consulta = data.spConsultarTotalCapturas(idUsuario).ToList();
            string mensaje = " Capturas: " + consulta[0].TotalNegocios.ToString();
            return mensaje;
        }

        public ActionResult Login()
        {
            ViewData["Respuesta"] = "";
            return View();
        }

        public ActionResult Logout()
        {
            //<< JFPancho; 6-abril-2017
            LogUsuarios x = new LogUsuarios();
            if (Session["USUARIO_LOGUEADO"] != null)
            {
                x.CierraSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            }
            // JFPancho >>
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            return base.Redirect("/Seguridad/Login");
        }

        public ActionResult Inicio()
        {
            return View();
        }

        /*
         * **********************************
         EN BASE A UN PADRE CARGO SUS HIJOS DINAMICAMENTE POR AJAX
         * **********************************
         */
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CargarHijosMenu(TreeViewItem node)
        {
            try
            {
                int? parentId = !string.IsNullOrEmpty(node.Value) ? (int?)Convert.ToInt32(node.Value) : null;

                MenuController controlador = new MenuController();
                List<spObtenerMenuHijosPerfil_Result> menus = controlador.ObtenerHijosPerfil((int)parentId, (int)Session["ROL_USUARIO"]);
                IEnumerable nodes = from item in menus

                                    select new TreeViewItemModel
                                    {
                                        Text = item.DescMenu,
                                        NavigateUrl = prefijo_menu + item.Url,
                                        LoadOnDemand = false,
                                        Enabled = true
                                    };

                return new JsonResult { Data = nodes };
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en SeguridadController metodo CargarHijosMenu " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        [HttpGet]
        public ActionResult CargarHijosMenu(int parentId)
        {
            try
            {
                MenuController controlador = new MenuController();
                List<spObtenerMenuHijosPerfil_Result> menus = controlador.ObtenerHijosPerfil((int)parentId, (int)Session["ROL_USUARIO"]);

                return Json(menus, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en SeguridadController metodo CargarHijosMenu " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        public ActionResult ValidarUsuario(string txtUsuario, string txtContrasena)
        {
            string respuesta = "";
            #region Login por Base de datos
            try
            {
                //Validar si la seguridad de las contraseñas esta habilitada o no lo esta
                ControlUsuarios ctrl = new ControlUsuarios();
                bool seguridadHabilitada = ctrl.ConsultaSeguridadHabilitada();

                UsuariosController busu;
                busu = new UsuariosController();
                decimal idUsuario = Convert.ToDecimal(txtUsuario);
                Usuarios usu = new Usuarios
                {
                    IdUsuario = Convert.ToDecimal(txtUsuario),
                    PassCodeUsuario = txtContrasena,
                    NomUsuario = data.Usuarios.Where(c => c.IdUsuario == idUsuario).First().NomUsuario,
                    CliNit = data.Usuarios.Where(c => c.IdUsuario == idUsuario).First().CliNit,
                    //<< JFPancho; 6-Abril-2017; se agrega rol a objeto de Usuarios >>
                    RolId = data.Usuarios.Where(c => c.IdUsuario == idUsuario).First().RolId
                };

                //usu = null;
                Session["USUARIO_LOGUEADO"] = usu;
                if (busu.ValidarUsuarios(usu))
                {
                    Session["USUARIO"] = usu;
                    Session["NombreUsuario"] = usu.NomUsuario;
                    List<ClientesUsuario> cus = data.ClientesUsuario.Where(c => c.idUsuario == usu.IdUsuario).ToList();

                    //Valida la cantidad de Capturas realizadas por el Usuario.
                    string _idUsuario = usu.IdUsuario.ToString();
                    Session["_IDUsuario_"] = usu.IdUsuario.ToString();
                    string mensajeCapturas = consultaTotalCapturas(_idUsuario);
                    Session["TotalCapturas"] = mensajeCapturas;

                    Clientes clientes = new Clientes();
                    clientes.CliNit = (decimal)cus.First().idCliente;

                    List<Clientes> Datos = data.Clientes.Where(c => c.CliNit == clientes.CliNit).ToList();
                    clientes.CliNombre = (string)Datos.First().CliNombre;
                    clientes.Codlabel = (string)Datos.First().Codlabel;
                    clientes.CodParametros = (string)Datos.First().CodParametros;
                    clientes.CliNit = Datos.First().CliNit;
                    Session["CLIENTE"] = clientes;
                    Session["SW_CONCILIACION"] = 0;
                    //LE CARGO EL ROL A ESE USUARIO
                    Session["ROL_USUARIO"] = data.Usuarios.Where(c => c.IdUsuario == usu.IdUsuario).First().RolId;
                    Session["USUARIOS_CLIENTES"] = cus;

                    //SETEO EL PARAMETRO QUE ME DEFINIRA LOS BOTONES QUE SE MUESTRAN EN LA BARRA DEL ALTERNATIFF
                    Parametros param = data.Parametros.First(c => c.codigo == "TOOL_VIS");
                    Session["TOOL_BAR"] = param.valor;

                    //JFP; Campos para el visor del validacion documental
                    Parametros paramVD = data.Parametros.First(c => c.codigo == "TOOL_VIS_VD");
                    Session["TOOL_BAR_VD"] = paramVD.valor;

                    //SETEA EL PARAMETRO QUE DEFINE LOS BOTONES QUE SE MUESTRAN EN EL ALTERNATIFF PARA LOS OTROS EL
                    //MODULO DE DACTILOSCOPIA PARA LA CEDULA, LOS CUALES ESTAN PARAMETRIZADOS EN LA TABLA
                    //PARAMETROS| AGREGADO 17/03/2016 WILLIAM CICUA
                    Parametros dactiloscopiaCedula = data.Parametros.First(c => c.codigo == "TOOL_D");
                    Session["TOOL_DAC"] = dactiloscopiaCedula.valor;

                    //SETEA EL PARAMETRO QUE DEFINE LOS BOTONES QUE SE MUESTRAN EN EL ALTERNATIFF PARA LOS OTROS EL
                    //MODULO DE DACTILOSCOPIA PARA LOS OTROS DOCUMENTOS, LOS CUALES ESTAN PARAMETRIZADOS EN LA TABLA
                    //PARAMETROS| AGREGADO 17/03/2016 WILLIAM CICUA

                    Parametros dactiloscopiaOtros = data.Parametros.First(c => c.codigo == "TOOL_D2");
                    Session["TOOL_DAC2"] = dactiloscopiaOtros.valor;

                    //VALIDAR SI EL APLICATIVO TIENE HABILITADA LA SEGURIDAD DE USUARIOS
                    if (seguridadHabilitada)
                    {
                        //Validar si la contraseña se encuentra proxima a expirar
                        UsuariosController a = new UsuariosController();
                        if (a.obtenerTipoLogin() == 1)
                        {
                            string mensaje = busu.ValidarCaducidadContrasena(usu.IdUsuario.ToString(), txtContrasena);
                            if (mensaje.Equals("Caducada"))
                            {
                                throw new Exception("Su contraseña ha Caducado");
                            }

                            if (mensaje.Contains("cambiarla"))
                            {
                                System.Web.HttpContext.Current.Session["Mensaje"] = mensaje;
                            }
                        }
                    }

                    // SI EL USUARIO ES MOBIL LO REDIRECCIONO A LA PANTALLA EN CUESTION
                    if ((int)Session["ROL_USUARIO"] == 5)
                    {
                        return base.Redirect("/Mobile/Index");
                    }

                    //IMPORTANTE USAR EL BASE REDIRECT TAL CUAL PARA QUE ENLACE BIEN LA SESSION
                    //return base.Redirect("/ViewsAspx/Inicio.aspx");

                    //<< JFPancho;6-abril-2017; se agrega para validar logueo de usuarios
                    //return base.Redirect("/Home/Index");
                    LogUsuarios x = new LogUsuarios();
                    if (x.ValidaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario))
                    {
                        Session["USUARIO_LOGUEADO"] = null;
                        respuesta = "El usuario ya se encuentra logueado en otro equipo.";
                    }
                    else
                    {
                        x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                        return base.Redirect("/Home/Index");
                    }
                    // JFPancho -->>
                }
                else
                {
                    //Modifica: JFPancho; mayo/2016; se agrega msn de error 
                    int usu_activo = Convert.ToInt32(System.Web.HttpContext.Current.Session["activo"].ToString());
                    if (usu_activo == 0)
                    {
                        respuesta = "El usuario no se encuentra activo";
                    }
                    else
                    {
                        respuesta = "El usuario o contraseña digitados no coinciden";
                    }

                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en SeguridadController metodo ValidarUsuario " + exception.Message + " stack trace " + exception.StackTrace);
                respuesta = exception.Message;
            }
            #endregion

            ViewData["Respuesta"] = respuesta;
            return View("Login");
        }
    }
}