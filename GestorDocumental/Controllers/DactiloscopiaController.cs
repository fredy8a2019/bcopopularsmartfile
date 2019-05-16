using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.OleDb;
using GestorDocumental.Models;
using System.Web.UI.WebControls;
using System.Collections;
using Telerik.Web.Mvc;


using System.IO;
using AjaxControlToolkit;
using GestorDocumental.Controllers;
using GestorDocumental.WebService;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace GestorDocumental.Controllers
{
    public class DactiloscopiaController : Controller
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();
        private GestorDocumental.Controllers.AsignacionesController bAsig = new Controllers.AsignacionesController();
        private GestorDocumental.Controllers.CapturaController bCap;
        private CamposController bCampos = new Controllers.CamposController();
        private CrearFormulariosCaptura formulario = new CrearFormulariosCaptura();
        
        // GET: /Dactiloscopia/

        public ActionResult Index()
        {
            //Carlos : metodos para limpiar la cache
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

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
            var result = db.spValidaAccesoModulo(idRol, "/Dactiloscopia/Index").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>

            // william valida que la sesion no este caducada si lo esta lo envia al login
            if (Session["CLIENTE"] == null)
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }

            ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            decimal cliNit = Convert.ToDecimal(((Clientes)Session["CLIENTE"]).CliNit);

            ViewData["TITULO"] = "0";
            decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, 260).ToList().SingleOrDefault().Value;
            Session["_negId_"] = negId;
            ViewData["_negId"] = negId;
            ViewData["_mensajeInformacion"] = "";

            //Ivan Rodriguez: se obtiene el numero bizagi y la accion para mostrar en el titulo de la indexacion
            //-----------Inicio CambiosIvan Rodriguez
            if (negId != null && negId != 0)
            {
                var nombreIndex = db.sp_Indexacion_NegNumbizagiAccion((int?)negId).ToList().FirstOrDefault();

                ViewData["TITULO"] = "" + negId + " |" + nombreIndex;
            }
            //---------Fin cambio Ivan Rodriguez           
            
            List<spValDoc_Produc_Negocio_Result> _lstP = db.spValDoc_Produc_Negocio(Convert.ToInt32(negId)).ToList();
            string producSubpro = null;
            foreach (spValDoc_Produc_Negocio_Result item in _lstP)
            {
                
                producSubpro = item.producto;
                producSubpro = producSubpro + " - " + item.subproducto;
            }
            ViewData["_negIdProduc"] = producSubpro;
            
            // se llama un sp que obiene el valor de un campo capturado en la radicacion el cual nos indica si es nuevo o 
            // una devolucion 1 y 2 respectivamente
            var nuevo = db.sp_ValDac_EsNuevo(negId).ToList();

            // valida si existe el campo sino lo crea en 1 y sigue normal de lo contrario si existe guarda la variable 
            // que trae el sp en una bariable con la cual en el index determina que pdf pegarle al visor
            if (nuevo.Count() == 0)
            {
                nuevo.Add("1");
                ViewData["_Nuevo"] = "1";
            }
            else
            {
                ViewData["_Nuevo"] = nuevo[0];
            }

            //valida si es devolucion si es devolucion entra al metodo y crea un nuevo pdf en al cual le agregara los
            //Documentos faltantes para dactiloscopia los cuales optiene del primer negocio creado ligado a ese negocio 
            // bizagi

            if (nuevo[0].Equals("1")) { 
            }
            else
            {
                try
                {

                    List<sp_ValDac_PrimerNedID_Result> NegPrincipal = db.sp_ValDac_PrimerNedID(negId).ToList();

                    ViewData["_NegPrincipal"] = NegPrincipal[0].NegId;

                    CrearArchivo(negId);
                    
                }
                catch (Exception exception)
                {
                    LogRepository.registro("Error en Dactiloscopia metodo crearArchivo " + exception.Message);
                    Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");
                }
            }
                
                ViewData["Hoja 1"] = ""; ViewData["Hoja 2"] = ""; ViewData["Hoja 3"] = ""; ViewData["Hoja 4"] = "";
                ViewData["Doc 1"] = ""; ViewData["Doc 2"] = ""; ViewData["Doc 3"] = ""; ViewData["Doc 4"] = ""; ViewData["Doc 5"] = "";
                ViewData["Mascara1"] = ""; ViewData["Mascara2"] = ""; ViewData["Mascara3"] = ""; ViewData["Mascara4"] = "";
                Session["Mostrar"] = "display: none"; 
                Session["Ocultar"] = "";
                Session["Tajeta"] = "0";

                // se realiza llamada a un sp el cual lista las hojas y los documentos a los cuales se les aplicara dactiloscopia
                // solo se ingresa el idNeg y automaticamente trae esta cansulta
                
                List<sp_ValDactiloscopia_Campos_Result> _NumHojas = db.sp_ValDactiloscopia_Campos(Convert.ToInt32(negId)).ToList();
                
                // este for me obtiene las hojas y los documentos que lueog se usaran en los visores para cargar las paginas adecuadas
                
                Parametros param1 = db.Parametros.First(c => c.codigo == "Doc_Dactiloscopia");
                var tarjeta = db.sp_ValDac_EsTarjeta(negId).ToList();     

                string[] mascaras;
                mascaras = param1.valor.Split(',');

                int fuv = 0; int cc = 0; int pagare = 0; int carta = 0; int cartaypagare = 0; 

                foreach (var item in _NumHojas)
                {
                    if (item.Mascara == mascaras[0].ToString())
                    {
                        ViewData["Hoja 1"] = item.NumPagina;
                        ViewData["Doc 1"] = item.DocId;
                        ViewData["Mascara1"] = item.Mascara;
                        fuv = 1;
                    }
                    if (item.Mascara == mascaras[1].ToString())
                    {
                        ViewData["Hoja 2"] = item.NumPagina;
                        ViewData["Doc 2"] = item.DocId;
                        ViewData["Mascara2"] = item.Mascara;

                        cc = 1;
                    }

                    if (tarjeta[0].Value == 2)
                    {
                        Session["Tajeta"] = "2";
                        if (item.Mascara == mascaras[3].ToString())
                        {
                            ViewData["Hoja 3"] = item.NumPagina;
                            ViewData["Doc 3"] = item.DocId;
                            ViewData["Mascara3"] = item.Mascara;
                            ViewData["Hoja 4"] = item.NumPagina;
                            ViewData["Doc 4"] = item.DocId;
                            ViewData["Mascara4"] = item.Mascara;
                            

                            pagare = 1;
                        }
                        if (item.Mascara == mascaras[4].ToString())
                        {
                            //ViewData["Hoja 4"] = item.NumPagina;
                            //ViewData["Doc 4"] = item.DocId;
                            //ViewData["Mascara4"] = item.Mascara;

                            carta = 1;
                        }
                    }
                    if (tarjeta[0].Value == 1)
                    {
                        Session["Mostrar"] = "";
                        Session["Ocultar"] = "display: none";
                        Session["Tajeta"] = "1";
                        if (item.Mascara == mascaras[2].ToString())
                        {
                            ViewData["Hoja 3"] = item.NumPagina;
                            ViewData["Doc 3"] = item.DocId;
                            ViewData["Mascara3"] = item.Mascara;

                            ViewData["Hoja 4"] = item.NumPagina;
                            ViewData["Doc 4"] = item.DocId;
                            ViewData["Mascara4"] = item.Mascara;

                                                        
                            cartaypagare = 1;
                        }
                    }

                    if (tarjeta[0].Value == 3)
                    {
                        Session["Tajeta"] = "3";
                        Session["Mostrar"] = "";
                        if (item.Mascara == mascaras[3].ToString())
                        {
                            ViewData["Hoja 3"] = item.NumPagina;
                            ViewData["Doc 3"] = item.DocId;
                            ViewData["Mascara3"] = item.Mascara;
                            //ViewData["Hoja 4"] = item.NumPagina;
                            //ViewData["Doc 4"] = item.DocId;
                            //ViewData["Mascara4"] = item.Mascara;

                            pagare = 1;
                        }
                        if (item.Mascara == mascaras[4].ToString())
                        {
                            //ViewData["Hoja 4"] = item.NumPagina;
                            //ViewData["Doc 4"] = item.DocId;
                            //ViewData["Mascara4"] = item.Mascara;

                            carta = 1;
                        }
                        if (item.Mascara == mascaras[2].ToString())
                        {
                            ViewData["Hoja 4"] = item.NumPagina;
                            ViewData["Doc 4"] = item.DocId;
                            ViewData["Mascara4"] = item.Mascara;

                            
                            //Session["Ocultar"] = "display: none";
                            
                            cartaypagare = 1;
                        }

                    }
  
                }

            
            try
            {
                if (negId != 0)
                {
                    ArchivosAnexos(fuv, cc, pagare, carta, negId, cartaypagare);

                    Session["_neg_"] = System.Configuration.ConfigurationManager.AppSettings["ClientFiles"] + "/" + negId + "/" + negId + ".TIF";

                    this.bCap = new GestorDocumental.Controllers.CapturaController();
                    Captura c = new Captura();

                    P_Etapas etapas = new P_Etapas();
                    etapas.IdEtapa = 260;
                    Session["ETAPA"] = etapas;
                    Session["idETAPA"] = 260;
                    string noCaptura = "1";
                    Session["_NoCaptura"] = noCaptura;

                    Session["lstGrupos"] = null;

                    c.NegId = negId;
                    c.NumCaptura = 1;
                    this.Session["NEGOCIO"] = c;

                    //Obtenemos el grupo asociado al negocio en proceso
                    int idGrupo = obtenerIdGrupo(Convert.ToInt32(negId));
                    int num = this.bCap.ObtenerDocumentoActual(c, (Clientes)this.Session["CLIENTE"], 2);
                    Documentos d = new Documentos { DocId = num };


                    if (Session["lstDocumentos"] == null)
                        Session["lstDocumentos"] = formulario.CargarDocumentos((Clientes)this.Session["CLIENTE"]);

                    if (Session["lstGrupos"] == null)
                        Session["lstGrupos"] = formulario.CargarGruposDocs(d);

                    Session["_idDocId"] = num;

                    //cordenadas
                    List<string> _coodenadas = db.spObtenerCoordenadas().ToList();

                    ViewData["CORDENADAS_Y"] = _coodenadas[0];
                    ViewData["CORDENADAS_X"] = _coodenadas[1];
                    ViewData["Block"] = "";
                }
                else
                {
                    ViewData["_cadenaMenu"] = "";
                    ViewData["_cadenaPoliticas"] = "";
                    ViewData["_negId"] = 0;
                    ViewData["_negIdProduc"] = "";
                    //ViewData["_negIdSubProduc"] = "";
                    ViewData["_mensajeInformacion"] = "No existen negocios disponibles para esta etapa";
                    ViewData["Block"] = "disabled = 'true'";
                }

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Dactiloscopia metodo index " + exception.Message); 
                Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");                
            }
           
            return View();
        }


        
        /// <summary>
        /// obtiene el grupo al que pertenese este negocio
        /// </summary>
        /// <param name="_negId"></param>
        /// <returns></returns>

        public int obtenerIdGrupo(int _negId)
        {
            int idGrupo = Convert.ToInt32(db.spObtenerSubgrupo(_negId).SingleOrDefault().ToString());
            return idGrupo;
        }



        // buscar y genera las causales automaticas ademas de cargar la view de Noaprobo obteniendo tambien los
        // documetos a los cuales les aplican las causales
        public ActionResult Noaprobo()
        {

            List<spValDoc_ListaCausales_Result> _politicasG = db.spValDoc_ListaCausales(2,2).ToList();
            string cadenaPoliticas = "";
            int btns = 0;
            
            foreach (spValDoc_ListaCausales_Result data in _politicasG)
            {
                
                if (string.IsNullOrEmpty(cadenaPoliticas))
                {
                    cadenaPoliticas = cadenaPoliticas + "<tr><td><label>" + data.nom_causal + "</label><input type='hidden' name='hidden" + btns + "' id='" + data.cod_causal + "'/></td><td><input type='radio' id='btn" + btns + "'  name='name" + btns + "' value='0'/>SI</td><td><input type='radio' id='btn" + btns + "' name='name" + btns + "' value='1'/>NO</td></tr>";
                }
                else
                {
                    cadenaPoliticas = cadenaPoliticas + "<tr><td><label>" + data.nom_causal + "</label><input type='hidden' name='hidden" + btns + "' id='" + data.cod_causal + "'/></td><td><input type='radio' id='btn" + btns + "' name='name" + btns + "' value='0'/>SI</td><td><input type='radio' id='btn" + btns + "' name='name" + btns + "' value='1'/>NO</td></tr>";
                }

                btns++;
            }
            Session["_NunDocGne"] = btns;
            ViewData["_cadenaPoliticas"] = cadenaPoliticas;

            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, 260).ToList().SingleOrDefault().Value;
            ViewData["_negId"] = negId;

            var tarjeta = db.sp_ValDac_EsTarjeta(negId).ToList();
            List<sp_ValDactiloscopia_Campos_Result> _NumHojas = db.sp_ValDactiloscopia_Campos(Convert.ToInt32(negId)).ToList();

                Session["_ListaFuv"] = ""; Session["_ListaCC"] = ""; Session["_ListaPagare"] = ""; Session["_ListaCarta"] = ""; Session["_ListaPagareycarta"] = "";
            Session["_NunDocFuv"] = 0; Session["_NunDocCC"] = 0; Session["_NunDocPagare"] = 0; Session["_NunDocCarta"] = 0; Session["_NunDocPagareycarta"] = 0;

            List<sp_Dacti_HistoricoComentarios_Result> listaComentarios = db.sp_Dacti_HistoricoComentarios(negId).ToList();

            string comentarios = "";
            foreach (sp_Dacti_HistoricoComentarios_Result fila in listaComentarios)
            {
                comentarios += "Negocio: " + fila.NegId + ", " + fila.Valor + " | ";
            }

            ViewData["HistoricoComentarios"] = comentarios;
            
            for (int i = 0; i < _NumHojas.Count; i++)
            {
                if (i == 0)
                {
                    ViewData["Doc 1"] = _NumHojas[i].DocId;
                }
                if (i == 1)
                {
                    ViewData["Doc 2"] = _NumHojas[i].DocId;
                }
                if (tarjeta[0].Value == 1 && tarjeta[0].Value == 2)
                {
                    if (i == 2)
                    {
                        ViewData["Doc 3"] = _NumHojas[i].DocId;
                        ViewData["Doc 4"] = _NumHojas[i].DocId;
                    }
                    
                }
                else
                {
                    if (i == 2)
                    {
                        ViewData["Doc 3"] = _NumHojas[i].DocId;

                    }
                    if (i == 3)
                    {
                        
                    }
                    if (i == 4)
                    {
                        ViewData["Doc 4"] = _NumHojas[i].DocId;
                    }
                }
            }

            return View();

        }

        public ActionResult Visores()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

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
            var result = db.spValidaAccesoModulo(idRol, "/Dactiloscopia/Index").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>

            // william valida que la sesion no este caducada si lo esta lo envia al login
            if (Session["CLIENTE"] == null)
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }

            ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            decimal cliNit = Convert.ToDecimal(((Clientes)Session["CLIENTE"]).CliNit);

            ViewData["TITULO"] = "0";
            decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, 260).ToList().SingleOrDefault().Value;
            Session["_negId_"] = negId;
            ViewData["_negId"] = negId;
            ViewData["_mensajeInformacion"] = "";

            //Ivan Rodriguez: se obtiene el numero bizagi y la accion para mostrar en el titulo de la indexacion
            //-----------Inicio CambiosIvan Rodriguez
            if (negId != null && negId != 0)
            {
                var nombreIndex = db.sp_Indexacion_NegNumbizagiAccion((int?)negId).ToList().FirstOrDefault();

                ViewData["TITULO"] = "" + negId + " |" + nombreIndex;
            }
            //---------Fin cambio Ivan Rodriguez           

            List<spValDoc_Produc_Negocio_Result> _lstP = db.spValDoc_Produc_Negocio(Convert.ToInt32(negId)).ToList();
            string producSubpro = null;
            foreach (spValDoc_Produc_Negocio_Result item in _lstP)
            {

                producSubpro = item.producto;
                producSubpro = producSubpro + " - " + item.subproducto;
            }
            ViewData["_negIdProduc"] = producSubpro;

            // se llama un sp que obiene el valor de un campo capturado en la radicacion el cual nos indica si es nuevo o 
            // una devolucion 1 y 2 respectivamente
            var nuevo = db.sp_ValDac_EsNuevo(negId).ToList();

            // valida si existe el campo sino lo crea en 1 y sigue normal de lo contrario si existe guarda la variable 
            // que trae el sp en una bariable con la cual en el index determina que pdf pegarle al visor
            if (nuevo.Count() == 0)
            {
                nuevo.Add("1");
                ViewData["_Nuevo"] = "1";
            }
            else
            {
                ViewData["_Nuevo"] = nuevo[0];
            }

            //valida si es devolucion si es devolucion entra al metodo y crea un nuevo pdf en al cual le agregara los
            //Documentos faltantes para dactiloscopia los cuales optiene del primer negocio creado ligado a ese negocio 
            // bizagi

            if (nuevo[0].Equals("1"))
            {
            }
            else
            {
                try
                {

                    List<sp_ValDac_PrimerNedID_Result> NegPrincipal = db.sp_ValDac_PrimerNedID(negId).ToList();

                    ViewData["_NegPrincipal"] = NegPrincipal[0].NegId;

                    CrearArchivo(negId);

                }
                catch (Exception exception)
                {
                    LogRepository.registro("Error en Dactiloscopia metodo crearArchivo " + exception.Message);
                    Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");
                }
            }

            ViewData["Hoja 1"] = ""; ViewData["Hoja 2"] = ""; ViewData["Hoja 3"] = ""; ViewData["Hoja 4"] = "";
            ViewData["Doc 1"] = ""; ViewData["Doc 2"] = ""; ViewData["Doc 3"] = ""; ViewData["Doc 4"] = ""; ViewData["Doc 5"] = "";
            ViewData["Mascara1"] = ""; ViewData["Mascara2"] = ""; ViewData["Mascara3"] = ""; ViewData["Mascara4"] = "";
            Session["Mostrar"] = "display: none";
            Session["Ocultar"] = "";
            Session["Tajeta"] = "0";

            // se realiza llamada a un sp el cual lista las hojas y los documentos a los cuales se les aplicara dactiloscopia
            // solo se ingresa el idNeg y automaticamente trae esta cansulta

            List<sp_ValDactiloscopia_Campos_Result> _NumHojas = db.sp_ValDactiloscopia_Campos(Convert.ToInt32(negId)).ToList();

            // este for me obtiene las hojas y los documentos que lueog se usaran en los visores para cargar las paginas adecuadas

            Parametros param1 = db.Parametros.First(c => c.codigo == "Doc_Dactiloscopia");
            var tarjeta = db.sp_ValDac_EsTarjeta(negId).ToList();

            string[] mascaras;
            mascaras = param1.valor.Split(',');

            int fuv = 0; int cc = 0; int pagare = 0; int carta = 0; int cartaypagare = 0;

            foreach (var item in _NumHojas)
            {
                if (item.Mascara == mascaras[0].ToString())
                {
                    ViewData["Hoja 1"] = item.NumPagina;
                    ViewData["Doc 1"] = item.DocId;
                    ViewData["Mascara1"] = item.Mascara;
                    fuv = 1;
                }
                if (item.Mascara == mascaras[1].ToString())
                {
                    ViewData["Hoja 2"] = item.NumPagina;
                    ViewData["Doc 2"] = item.DocId;
                    ViewData["Mascara2"] = item.Mascara;

                    cc = 1;
                }

                if (tarjeta[0].Value == 2)
                {
                    Session["Tajeta"] = "2";
                    if (item.Mascara == mascaras[3].ToString())
                    {
                        ViewData["Hoja 3"] = item.NumPagina;
                        ViewData["Doc 3"] = item.DocId;
                        ViewData["Mascara3"] = item.Mascara;
                        ViewData["Hoja 4"] = item.NumPagina;
                        ViewData["Doc 4"] = item.DocId;
                        ViewData["Mascara4"] = item.Mascara;


                        pagare = 1;
                    }
                    if (item.Mascara == mascaras[4].ToString())
                    {
                        //ViewData["Hoja 4"] = item.NumPagina;
                        //ViewData["Doc 4"] = item.DocId;
                        //ViewData["Mascara4"] = item.Mascara;

                        carta = 1;
                    }
                }
                if (tarjeta[0].Value == 1)
                {
                    Session["Mostrar"] = "";
                    Session["Ocultar"] = "display: none";
                    Session["Tajeta"] = "1";
                    if (item.Mascara == mascaras[2].ToString())
                    {
                        ViewData["Hoja 3"] = item.NumPagina;
                        ViewData["Doc 3"] = item.DocId;
                        ViewData["Mascara3"] = item.Mascara;

                        ViewData["Hoja 4"] = item.NumPagina;
                        ViewData["Doc 4"] = item.DocId;
                        ViewData["Mascara4"] = item.Mascara;


                        cartaypagare = 1;
                    }
                }

                if (tarjeta[0].Value == 3)
                {
                    Session["Tajeta"] = "3";
                    Session["Mostrar"] = "";
                    if (item.Mascara == mascaras[3].ToString())
                    {
                        ViewData["Hoja 3"] = item.NumPagina;
                        ViewData["Doc 3"] = item.DocId;
                        ViewData["Mascara3"] = item.Mascara;
                        //ViewData["Hoja 4"] = item.NumPagina;
                        //ViewData["Doc 4"] = item.DocId;
                        //ViewData["Mascara4"] = item.Mascara;

                        pagare = 1;
                    }
                    if (item.Mascara == mascaras[4].ToString())
                    {
                        //ViewData["Hoja 4"] = item.NumPagina;
                        //ViewData["Doc 4"] = item.DocId;
                        //ViewData["Mascara4"] = item.Mascara;

                        carta = 1;
                    }
                    if (item.Mascara == mascaras[2].ToString())
                    {
                        ViewData["Hoja 4"] = item.NumPagina;
                        ViewData["Doc 4"] = item.DocId;
                        ViewData["Mascara4"] = item.Mascara;


                        //Session["Ocultar"] = "display: none";

                        cartaypagare = 1;
                    }

                }

            }


            try
            {
                if (negId != 0)
                {
                    ArchivosAnexos(fuv, cc, pagare, carta, negId, cartaypagare);

                    Session["_neg_"] = System.Configuration.ConfigurationManager.AppSettings["ClientFiles"] + "/" + negId + "/" + negId + ".TIF";

                    this.bCap = new GestorDocumental.Controllers.CapturaController();
                    Captura c = new Captura();

                    P_Etapas etapas = new P_Etapas();
                    etapas.IdEtapa = 260;
                    Session["ETAPA"] = etapas;
                    Session["idETAPA"] = 260;
                    string noCaptura = "1";
                    Session["_NoCaptura"] = noCaptura;

                    Session["lstGrupos"] = null;

                    c.NegId = negId;
                    c.NumCaptura = 1;
                    this.Session["NEGOCIO"] = c;

                    //Obtenemos el grupo asociado al negocio en proceso
                    int idGrupo = obtenerIdGrupo(Convert.ToInt32(negId));
                    int num = this.bCap.ObtenerDocumentoActual(c, (Clientes)this.Session["CLIENTE"], 2);
                    Documentos d = new Documentos { DocId = num };


                    if (Session["lstDocumentos"] == null)
                        Session["lstDocumentos"] = formulario.CargarDocumentos((Clientes)this.Session["CLIENTE"]);

                    if (Session["lstGrupos"] == null)
                        Session["lstGrupos"] = formulario.CargarGruposDocs(d);

                    Session["_idDocId"] = num;

                    //cordenadas
                    List<string> _coodenadas = db.spObtenerCoordenadas().ToList();

                    ViewData["CORDENADAS_Y"] = _coodenadas[0];
                    ViewData["CORDENADAS_X"] = _coodenadas[1];
                    ViewData["Block"] = "";
                }
                else
                {
                    ViewData["_cadenaMenu"] = "";
                    ViewData["_cadenaPoliticas"] = "";
                    ViewData["_negId"] = 0;
                    ViewData["_negIdProduc"] = "";
                    //ViewData["_negIdSubProduc"] = "";
                    ViewData["_mensajeInformacion"] = "No existen negocios disponibles para esta etapa";
                    ViewData["Block"] = "disabled = 'true'";
                }

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Dactiloscopia metodo index " + exception.Message);
                Response.Redirect("../ViewsAspx/PageErrors/ErrorPage.html");
            }
            return View();

        }

        /// <summary>
        /// con los campos ya guennerados se llama a este metodo el cual lo pinta en la pagina
        /// </summary>
        /// <returns></returns>
         
        [HttpPost]
        public JsonResult CargarCampos()
        {
            try
            {
                Table tblControls = new Table();

                if (Session["_NomGrupo"] != null)
                {
                    GruposDocumentos g = new GruposDocumentos();
                    g.GDocId = Convert.ToInt32(Session["_GDocId"].ToString());

                    P_Etapas e = new P_Etapas();
                    e.IdEtapa = 1;

                    Captura n = (Captura)this.Session["NEGOCIO"];
                    List<Campos> lstCampos = this.bCampos.ObtenerCamposCliente(g, e, n);
                    Session["_listaCampos"] = lstCampos;
                    if (lstCampos.Count == 0)
                    {
                        Campos camp = new Campos();
                        camp.Activo = false;
                        lstCampos.Add(camp);
                    }

                    List<spObtenerRespuestasAnteriores_Result> lstRespAnt = null;

                    int gDocID = Convert.ToInt32(Session["_GDocId"].ToString());
                    int idETAPA = Convert.ToInt32(((P_Etapas)this.Session["ETAPA"]).IdEtapa);
                    //string noCaptura = base.Request.QueryString["CAPTURA"].ToString();
                    int _idDocumento = Convert.ToInt32(Session["_idDocId"].ToString());
                    int _NegId = Convert.ToInt32((int)((Captura)this.Session["NEGOCIO"]).NegId);
                    string camposGenerados = formulario.GenerarCampos(tblControls, lstCampos, lstRespAnt, gDocID, idETAPA, "0", _idDocumento, _NegId);
                    ViewData["CamposGenerados"] = camposGenerados;
                    Campos campos = lstCampos.Find(c => c.CampId == 5);

                    Hashtable hashtable = this.bCampos.CamposPresentados(this.bCampos.ObtenerCamposCliente(g, e, n));
                    if (String.IsNullOrEmpty(camposGenerados))
                    {
                        FormCollection collection = new FormCollection();
                        //Guardar(collection);
                    }
                }
                return Json(ViewData["CamposGenerados"], JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en Dactiloscopia metodo CargarCampos " + exception.Message); 
                throw;
            }
        }
      
        /// <summary>
        /// este metodo se encarga de generar los campos de las causales que se validaran por iddocumento por este motivo
        /// esque el idDocumento se obtiene al cargar no aprobo y almasena en variables de secion la lista de causales de
        /// cada uno de los documentos
        /// </summary>
        /// <param name="_DocId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult generaCampos(int _DocId)
        {
            CrearFormulariosCaptura formularioValDoc = new CrearFormulariosCaptura();
            List<spValDoc_DocCampos_Result> lstSpCausales = db.spValDoc_DocCampos(_DocId,2).ToList();
            List<tvalDoc_Causales> lstCausales = new List<tvalDoc_Causales>();
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, 260).ToList().SingleOrDefault().Value;
            ViewData["_negId"] = negId;
            var tarjeta = db.sp_ValDac_EsTarjeta(negId).ToList();
            List<sp_ValDactiloscopia_Campos_Result> _NumHojas = db.sp_ValDactiloscopia_Campos(Convert.ToInt32(negId)).ToList();
            foreach (spValDoc_DocCampos_Result item in lstSpCausales)
            {
                tvalDoc_Causales cmp = new tvalDoc_Causales();
                
                cmp.cod_causal = item.cod_causal;
                cmp.nom_causal = item.nom_causal;
                cmp.desc_causal = item.desc_causal;
                cmp.cod_tipo_causal = item.cod_tipo_causal;
                cmp.cod_documento = item.cod_documento;
                cmp.CamAlto = item.CamAlto;
                cmp.CampAncho = item.CampAncho;
                cmp.CampOrden = item.CampOrden;
                cmp.CampObligatorio = item.CampObligatorio;
                cmp.TcId = item.TcId;
                cmp.Bloqueado = item.Bloqueado;
                cmp.Activo = item.Activo;
                cmp.Bloqueado = item.Bloqueado;
                cmp.LongMax = item.LongMax;
                cmp.PosX = item.PosX;
                cmp.PosY = item.PosY;
                cmp.CodFormulario = item.CodFormulario;

                lstCausales.Add(cmp);
            }

            Table tbl = new Table();
            string campos = formulario.GenerarCamposValDoc(tbl, lstCausales, 100, 0, _DocId, Convert.ToInt32(negId),2);
            campos = campos.Replace('"', '\'');
            ViewData["_camposValDoc"] = campos;
            Session["_lstValDoc"] = lstCausales;

                if (_DocId == _NumHojas[0].DocId)
                {
                    Session["_ListaFuv"] = lstCausales;
                    Session["_NunDocFuv"] = lstCausales.Count;

                } if (_DocId == _NumHojas[1].DocId)
                {
                    Session["_ListaCC"] = lstCausales;
                    Session["_NunDocCC"] = lstCausales.Count;

                }
                if (tarjeta[0].Value == 2)
                {
                    if (_DocId == _NumHojas[2].DocId)
                    {
                        Session["_ListaPagare"] = lstCausales;
                        Session["_NunDocPagare"] = lstCausales.Count;

                    } if (_DocId == _NumHojas[3].DocId)
                    {

                        Session["_ListaCarta"] = lstCausales;
                        Session["_NunDocCarta"] = lstCausales.Count;
                    }
                }
                if (tarjeta[0].Value == 1)
                {
                    if (_DocId == _NumHojas[2].DocId)
                    {
                        Session["_ListaPagareycarta"] = lstCausales;
                        Session["_NunDocPagareycarta"] = lstCausales.Count;
                        
                    }
                }

                if (tarjeta[0].Value == 3)
                {

                    if (_DocId == _NumHojas[2].DocId)
                    {
                        Session["_ListaPagare"] = lstCausales;
                        Session["_NunDocPagare"] = lstCausales.Count;

                    } if (_DocId == _NumHojas[3].DocId)
                    {

                        Session["_ListaCarta"] = lstCausales;
                        Session["_NunDocCarta"] = lstCausales.Count;
                    }
                    
                    if (_DocId == _NumHojas[4].DocId)
                    {
                        Session["_ListaPagareycarta"] = lstCausales;
                        Session["_NunDocPagareycarta"] = lstCausales.Count;

                    }
                }
          
            return Json(ViewData["_camposValDoc"], JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// metodo que se encarga de validar que todas las causales de todos los documentos se validaran y retorna un 
        /// 1 o un 0 con los cuales se realizara diferentes aciones donde retorna
        /// </summary>
        /// <param name="_negId"></param>
        /// <param name="_snIndx"></param>
        /// <param name="_observaciones"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult snExisteCausal(decimal _negId, int _snIndx, string _observaciones)
        {
            try
            {
                Session["_txtObservaciones"] = _observaciones;
                Session["_vlrChck"] = _snIndx;

                var a = Int32.Parse(Session["_NunDocGne"].ToString()) + Int32.Parse(Session["_NunDocCarta"].ToString()) + Int32.Parse(Session["_NunDocPagare"].ToString()) + Int32.Parse(Session["_NunDocCC"].ToString()) + Int32.Parse(Session["_NunDocFuv"].ToString()) + Int32.Parse(Session["_NunDocPagareycarta"].ToString());
                //var a = Int32.Parse(Session["_NunDocGne"].ToString())  + Int32.Parse(Session["_NunDocCC"].ToString()) + Int32.Parse(Session["_NunDocFuv"].ToString());
              
                //var resultado = db.spValDoc_SNExisteCausales(_negId).SingleOrDefault();
                var resultado = db.sp_Numero_causales_capturadas(_negId, a).SingleOrDefault();

                if (resultado == 1)
                {
                    return Json(Convert.ToInt32(1), JsonRequestBehavior.AllowGet);
                }else
                {
                    return Json(Convert.ToInt32(0), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en Dactiloscopia  metodo snExisteCausal " + es.Message);
                throw;
            }
        }

        /// <summary>
        /// este metodo es el encargado de validar que se ingresaran todas las causales al dar en el boton de aprobo 
        /// o finalizo
        /// </summary>
        /// <param name="_negId"></param>
        /// <param name="_snIndx"></param>
        /// <param name="_observaciones"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AutoCausal(decimal _negId, int _snIndx, string _observaciones)
        {
            try
            {
                //Session["_txtObservaciones"] = _observaciones;
                Session["_vlrChck"] = _snIndx;
                var a = Int32.Parse(Session["_NunDocGne"].ToString()) + Int32.Parse(Session["_NunDocCarta"].ToString()) + Int32.Parse(Session["_NunDocPagare"].ToString()) + Int32.Parse(Session["_NunDocCC"].ToString()) + Int32.Parse(Session["_NunDocFuv"].ToString()) + Int32.Parse(Session["_NunDocPagareycarta"].ToString());
                //var a = Int32.Parse(Session["_NunDocGne"].ToString())  + Int32.Parse(Session["_NunDocCC"].ToString()) + Int32.Parse(Session["_NunDocFuv"].ToString());
                decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
                
                //var resultado = db.spValDoc_SNExisteCausales(_negId).SingleOrDefault();
                var resultado = db.sp_Numero_causales_capturadas(_negId, a).SingleOrDefault();

                if (resultado == 1)
                {
                    return Json(Convert.ToInt32(1), JsonRequestBehavior.AllowGet);
                }
                else if (resultado == 2)
                {
                    return Json(Convert.ToInt32(2), JsonRequestBehavior.AllowGet);

                }
                else if (resultado == 3) 
                {
                    //db.sp_CausalesAutomaticas(_negId, _idUsuarioProc);
                    return Json(Convert.ToInt32(3), JsonRequestBehavior.AllowGet);
                }
                return Json(Convert.ToInt32(4), JsonRequestBehavior.AllowGet);
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en Dactiloscopia metodo AutoCausal " + es.Message);
                
                throw;
            }

        }


        /// <summary>
        /// obtiene la lista de causales para este negocio
        /// </summary>
        /// <param name="_negId"></param>
        /// <returns></returns>

        [HttpPost]
        [GridAction]
        public virtual ActionResult getConsulCausales(int _negId)
        {
            var grillaCausales = _getConsulCausales(_negId);
            //agregar validacion cuando no traiga resultados la consulta para genrerar alerta

            return View(new GridModel<grilla_CausalesNeg>(grillaCausales));
        }

        /// <summary>
        /// consulta causale por negocio y modulo el modulo es para dactiloscopia es 2 para incluir en la grilla
        /// </summary>
        /// <param name="_negId"></param>
        /// <returns></returns>

        public IEnumerable<grilla_CausalesNeg> _getConsulCausales(int _negId)
        {
            try
            {
                var grillaCausales = db.spValDoc_ConsultaCausales(_negId, 2);
                List<grilla_CausalesNeg> _grillaConsulCausales = new List<grilla_CausalesNeg>();
                foreach (spValDoc_ConsultaCausales_Result item in grillaCausales)
                {
                    grilla_CausalesNeg data = new grilla_CausalesNeg();
                    //data._codCausal = item.cod_causal;
                    data._nomCausal = item.nom_causal;
                    data._descCausal = item.desc_causal;
                    data._fecValidacion = item.fec_validacion.ToString();
                    data._NegId = item.NegId;

                    _grillaConsulCausales.Add(data);
                }
                return _grillaConsulCausales;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// inserta cada una de las causales en la abse de datos
        /// </summary>
        /// <param name="_campId"></param>
        /// <param name="_snCausal"></param>
        /// <param name="_negId"></param>
        /// <param name="doc"></param>

        [HttpPost]
        public void CausalesEspecificas(int _campId, int _snCausal, decimal _negId,int doc)
        {
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            decimal negId = db.sp_AsignaNegocio(_idUsuarioProc, 260).ToList().SingleOrDefault().Value;
            ViewData["_negId"] = negId;
            var tarjeta = db.sp_ValDac_EsTarjeta(negId).ToList();
            List<sp_ValDactiloscopia_Campos_Result> _NumHojas = db.sp_ValDactiloscopia_Campos(Convert.ToInt32(negId)).ToList();
            List<tvalDoc_Causales> lstCausales = new List<tvalDoc_Causales>();
            lstCausales = Session["_lstValDoc"] as List<tvalDoc_Causales>;


            if(_NumHojas[0].DocId == doc ){

                lstCausales = Session["_ListaFuv"] as List<tvalDoc_Causales>;

            } if (_NumHojas[1].DocId == doc)
            {

                lstCausales = Session["_ListaCC"] as List<tvalDoc_Causales>;
            
            }
            if (tarjeta[0].Value == 2)
            {
                if (_NumHojas[2].DocId == doc)
                {

                    lstCausales = Session["_ListaPagare"] as List<tvalDoc_Causales>;

                } if (_NumHojas[3].DocId == doc)
                {

                    lstCausales = Session["_ListaCarta"] as List<tvalDoc_Causales>;

                }
            }
            if (tarjeta[0].Value == 1)
            {
                if (_NumHojas[2].DocId == doc)
                {

                    lstCausales = Session["_ListaPagareycarta"] as List<tvalDoc_Causales>;
                    
                } 
            }
            if (tarjeta[0].Value == 3)
            {
                if (_NumHojas[2].DocId == doc)
                {

                    lstCausales = Session["_ListaPagare"] as List<tvalDoc_Causales>;

                } if (_NumHojas[3].DocId == doc)
                {

                    lstCausales = Session["_ListaCarta"] as List<tvalDoc_Causales>;

                }
                if (_NumHojas[4].DocId == doc)
                {

                    lstCausales = Session["_ListaPagareycarta"] as List<tvalDoc_Causales>;

                }
            }

            var a = (from b in lstCausales
                     where b.cod_causal == _campId
                     select b.cod_causal).SingleOrDefault();


            int _codCausal = Convert.ToInt32(a);
            decimal _idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            db.spValDact_InsertCausales(_codCausal, _snCausal, _negId, _idUsuario,doc);

        }

        //[HttpPost]
        //public void InsertaCausales(int _codCausal, int _snCausal, decimal _negId)
        //{
        //    decimal _idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
        //    db.spValDoc_InsertCausales(_codCausal, Convert.ToBoolean(_snCausal), _negId, _idUsuario);
        //}
       
        /// <summary>
        /// metodo el cual crea el codigo html de la tabla que se mostrara en dactiloscopia como entrada solo resive el negid con el cual
        /// consulta las causales que no aprobo y seran las que se pinten
        /// </summary>
        /// <param name="_negId"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getDatatable(decimal _negId)
        {
            //var grillaCausales = db.spValDoc_ConsultaCausales(_negId,2);

            List<spValDoc_ConsultaCausales_Result> listaCausales = db.spValDoc_ConsultaCausales(_negId,2).ToList();

            string html = "";
            for (int i = 0; i < listaCausales.Count; i++)
            {
                html += "<tr><th style='width: border-width: 0 0 1px 1px;'>" + listaCausales[i].DocDescripcion + "</th>"
                     + "<th style='width: border-width: 0 0 1px 1px;' >" + listaCausales[i].nom_causal + "</th>"
                     + "<th style='width: border-width: 0 0 1px 1px;'>" + listaCausales[i].desc_causal + "</th></tr>";
            }

            return Json(html,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// funcion encargada de llamar al sp que termina la etapa y guarda el comentario del usuario en captura con el campo -996
        /// </summary>
        public void finValidacion()
        {
            decimal negId = Convert.ToDecimal(Session["_negId_"]);
            decimal _idUsuario = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);
            object vlrChck = Session["_vlrChck"];
            var txtObservaciones = Session["_txtObservaciones"];

            if (txtObservaciones == null && txtObservaciones == "")
            {
                Convert.ToString(txtObservaciones);
                txtObservaciones = "USUARIO NO INGRESÓ OBSERVACIONES ";
            }
            

            db.sp_Finalizar_Dactiloscopia_iniciar_WS(negId, txtObservaciones.ToString(), _idUsuario);

        }
        

        /// <summary>
        /// funcion encargada de crear registros en archivos anexos simulando los documentos que faltan las entradas son binarias 1 o 0 
        /// con 1 indicando que el documento esta indexado, si es 0 lo agrega con pagina 0 
        /// </summary>
        /// <param name="fuv"></param>
        /// <param name="cc"></param>
        /// <param name="pagare"></param>
        /// <param name="carta"></param>
        /// <param name="neg"></param>
        public void ArchivosAnexos(int fuv, int cc, int pagare, int carta, decimal neg, int cartaypagare)
        {
            var tarjeta = db.sp_ValDac_EsTarjeta(neg).ToList();
            List<sp_ValDactiloscopia_Documentos_Result> documentos =  db.sp_ValDactiloscopia_Documentos(neg).ToList();
            if(fuv == 0){
                db.sp_ValDactiloscopia_ArchivosAnexos(neg,documentos[0].DocId);
            }
            if(cc == 0){
                db.sp_ValDactiloscopia_ArchivosAnexos(neg, documentos[1].DocId);
            }
            if(tarjeta[0].Value == 2){
                if (pagare == 0)
                {
                    db.sp_ValDactiloscopia_ArchivosAnexos(neg, documentos[2].DocId);
                }
                if (carta == 0)
                {
                    db.sp_ValDactiloscopia_ArchivosAnexos(neg, documentos[3].DocId);
                }
            }
            if (tarjeta[0].Value == 1)
            {
                if (cartaypagare == 0)
                {
                    db.sp_ValDactiloscopia_ArchivosAnexos(neg, documentos[2].DocId);
                }
            }
            if (tarjeta[0].Value == 3)
            {
                if (pagare == 0)
                {
                    db.sp_ValDactiloscopia_ArchivosAnexos(neg, documentos[3].DocId);
                }
                if (carta == 0)
                {
                    db.sp_ValDactiloscopia_ArchivosAnexos(neg, documentos[4].DocId);
                }
                if (cartaypagare == 0)
                {
                    db.sp_ValDactiloscopia_ArchivosAnexos(neg, documentos[2].DocId);
                }
            }
        }

        /// <summary>
        /// funcion la cual se encarga de crear el nuevo pdf que se mostrara en dactiloscopia el cual se creara con el 
        /// primer negocio y el actual de el negocio bizagi se obtendran los documentos que el negocio actual tiene se
        /// valida cuales faltan que sean requeridos en dactiloscopia y los optiene del otro negocio y los agrega a 
        /// archivos añexos para que luego dactiloscopia los tome normalmente
        /// </summary>
        /// <param name="negid"></param>
        public void CrearArchivo(decimal negid)
        {
            
            
            decimal _idUsuarioProc = Convert.ToDecimal(((Usuarios)Session["USUARIO"]).IdUsuario);

            try
            {
                //se obtiene la ruta donde se encuentran los archivos de los negocios
                string rutaNegocios = consultaRutaNegocios();

                //Se obtiene la ruta donde se crearan las carpetas de los archivos bizagi
                string rutaCptaBizagi = rutaNegocios + negid + "\\" +negid +".pdf";

                //se renombra  el archivo existente
                Directory.CreateDirectory(rutaNegocios + "\\" + negid + "\\Devuelto");
                //System.IO.File.Copy(rutaCptaBizagi, rutaNegocios + "//" + negid + "//Devuelto//" +negid +".pdf" );

                string archivoNuevo = negid.ToString();

                //ruta del archivo final
                string filename = rutaNegocios + negid + "\\Devuelto" + "\\" + archivoNuevo + ".pdf";

                List<sp_ValDac_PrimerNedID_Result> lstNeg = db.sp_ValDac_PrimerNedID(negid).ToList();

                string ruta = rutaNegocios  + lstNeg[0].NegId + "\\" + lstNeg[0].NegId + ".pdf";

                PdfReader reader = new PdfReader(ruta);

                Session["_RutaPDF"] = filename;

                List<sp_ValDactiloscopia_Campos_Prueba_Result> lstHojas = db.sp_ValDactiloscopia_Campos_Prueba(Convert.ToInt32(lstNeg[0].NegId), Convert.ToInt32(negid)).ToList();
                
                using (FileStream stream1 = new FileStream(filename, FileMode.Create))
                {
                    PdfImportedPage importedPage = null;
                    Document pdfDoc = new Document(PageSize.A4);
                    PdfCopy pdfCreado = new PdfCopy(pdfDoc, stream1);
                    pdfDoc.Open();

                        string a = rutaNegocios + negid;
                        var files = Directory.GetFiles(a);

                        string file = files[0];

                        pdfCreado.AddDocument(new PdfReader(file));
                        foreach (var paginas in lstHojas)
                        {
                            var numeropagians = pdfCreado.PageNumber;
                            importedPage = pdfCreado.GetImportedPage(reader, Convert.ToInt32(paginas.NumPagina));
                            db.sp_ValDactiloscopia_Archivo(negid, Convert.ToInt32(paginas.DocId), _idUsuarioProc);
                            pdfCreado.AddPage(importedPage);
                        }

                    pdfCreado.Close();
                    pdfDoc.Close();
                    stream1.Close();
                }

                //crearPDFPorTipoDocumental(_nroBizagi, rutaCptaBizagi, filename);
                               
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public JsonResult validarUsuario()
        {
            var s = Session["CLIENTE"];
            if (Session["CLIENTE"] == null)
            {
                //Response.Redirect("../Seguridad/Login");                
                return Json(2,JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// obtiene la ruta donde estan los archivos desde la tabla parametros
        /// </summary>
        /// <returns></returns>
        public string consultaRutaNegocios()
        {
            var valor = (from a in db.Parametros
                         where a.codigo == "PATH_DESTINO"
                         select a.valor).SingleOrDefault();
            return valor;
        }

        /// <summary>
        /// 
        /// o finalizo
        /// </summary>
        /// <param name="_negId"></param>
        /// <param name="_snIndx"></param>
        /// <param name="_observaciones"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult reiniciarCausales(decimal _negId)
        {
            try
            {
                int idetapa = 260;
                var resultado = db.sp_Borrar_Causales(_negId, idetapa).FirstOrDefault();

                if (resultado.ErrorNumber == 1)
                {
                    return Json(Convert.ToInt32(1), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogRepository.registro("Error en Dactiloscopia negid:" + _negId + " metodo reiniciarCausales sp_Borrar_Causales: "+resultado.ErrorMessage);
                    return Json(Convert.ToInt32(0), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en Dactiloscopia negid:" + _negId + " metodo reiniciarCausales " + es.Message + " _Staktrace" + es.StackTrace);

                throw;
            }
            return Json(Convert.ToInt32(4), JsonRequestBehavior.AllowGet);
        }

    }
}