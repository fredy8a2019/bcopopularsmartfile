using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    /// <summary>
    /// Autor :       Elena Parra
    /// Fecha :       28 - 11 - 2013
    /// Descriptcion: tiene toda la logica para controllar el modulo de 
    ///               radicacion en cada uno de sus procesos 
    /// </summary>
    public class RadicacionController : Controller
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();
        

        public ActionResult Index()
        {
            //Carlos : metodos para limpiar la cache
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

            //<< JFPancho;6-abril-2017; 
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = db.spValidaAccesoModulo(idRol, "/Radicacion/Index").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>

            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Radicacion de documentos";
                ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        [HttpPost]
        public ActionResult Guardar(FormCollection collection)
        {
            try
            {
                ConsecutivosModel consecutivo = new ConsecutivosModel();
                CamposController camposCon = new CamposController();
                RadicacionModelo radicacionModel = new RadicacionModelo();
                CapturaRadicacionModel capRadMod = new CapturaRadicacionModel();
                List<CapturaRadicacion> liscaptura = new List<CapturaRadicacion>();
                Radicacion data = new Radicacion();
                Extends extends = new Extends();

                data.CliNit = ((GestorDocumental.Models.Clientes)Session["CLIENTE"]).CliNit;
                data.Oficina = Convert.ToInt32(collection["Oficinas"]);
                data.IdProducto = Convert.ToInt32(collection["Productos"]);
                data.SubProducto = Convert.ToInt32(collection["SubProductos"]);
                data.Estado = Convert.ToInt32(collection["Estados"]);
                data.FechaRadicacion = DateTime.Now;
                data.IdUsusario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                data.TipoRadicacion = collection["Tipo"].ToString();

                //JFP; BcoPopular; abril-2016
                //data.FechaLocal = DateTime.Parse(collection["FechaLugarActual"].ToString());
                LogRepository.registro("RadicacionController metodo Guardar data.FechaLocal: " + DateTime.Now + " ;data.FechaRadicacion: " + data.FechaRadicacion);
                data.FechaLocal = DateTime.Now;


                if (data.Estado == 120)
                {
                    if (collection["NegAnuladoConfirmado"].ToString().Trim() == "")
                    {
                        data.CodBarras = ConsecutivosModel.ConsecutivoRadicado(((Clientes)Session["CLIENTE"]).Codlabel, ((Clientes)Session["CLIENTE"]).CodParametros);
                    }
                    else
                    {
                        /*EN ESTA PARTE SE HACE LA ANULACION DE NEGOCIO*/
                        var datos = db.spAnulacionNegocios(decimal.Parse(collection["NegAnuladoConfirmado"].ToString()), ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario, string.Empty, 1);
                        data.CodBarras = datos.ToArray()[0];

                        AuditoriaModel am = new AuditoriaModel();
                        Auditoria nuevaAuditoria = new Auditoria();

                        nuevaAuditoria.aud_idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        nuevaAuditoria.aud_fechaHora = DateTime.Now;
                        nuevaAuditoria.aud_evento = "Anulaciones";
                        nuevaAuditoria.aud_observaciones = "Se anulo el negocio No. " + collection["NegAnuladoConfirmado"].ToString() + " y se genero con el codigo de barras " + data.CodBarras;
                        //Inserta Auditoria
                        am.AddAuditoria(nuevaAuditoria);
                    }

                    ViewData["Lable"] = data.CodBarras;

                    string valor = collection["SubProductos"].ToString();

                    valor = subProducto(int.Parse(collection["SubProductos"].ToString()));

                    if (collection["Sociedad"] != "-1" && (!(string.IsNullOrEmpty(collection["Sociedad"].ToString().Trim())) || !(string.IsNullOrWhiteSpace(collection["Sociedad"].ToString().Trim()))))
                    {
                        ViewData["descSociedad"] = CodigosCampos(int.Parse(collection["Sociedad"]));
                    }

                    //Recepcion del archivo virtual
                    if (data.TipoRadicacion == "virtual")
                    {
                        RecepcionarArchivo(data);
                    }
                }
                else if (data.Estado == 130)
                {
                    if (collection["Causal"] != "-1" && (!(string.IsNullOrEmpty(collection["Causal"].ToString().Trim())) || !(string.IsNullOrWhiteSpace(collection["Causal"].ToString().Trim()))))
                    {
                        ViewData["motivoDevolucion"] = CodigosCampos(int.Parse(collection["Causal"]));
                    }

                    if (collection["NegAnuladoConfirmado"].ToString().Trim() != "")
                    {
                        /*EN ESTA PARTE SE HACE LA ANULACION DE NEGOCIO*/
                        var datos = db.spAnulacionNegocios(decimal.Parse(collection["NegAnuladoConfirmado"].ToString()), ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario, string.Empty, 1);

                        AuditoriaModel am = new AuditoriaModel();
                        Auditoria nuevaAuditoria = new Auditoria();

                        nuevaAuditoria.aud_idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                        nuevaAuditoria.aud_fechaHora = DateTime.Now;
                        nuevaAuditoria.aud_evento = "Anulaciones";
                        nuevaAuditoria.aud_observaciones = "Se anulo el negocio No. " + collection["NegAnuladoConfirmado"].ToString() + " y se genero como una devolucion ";
                        //Inserta Auditoria
                        am.AddAuditoria(nuevaAuditoria);
                    }

                }

                var idRadicacion = radicacionModel.Add(data);

                //Consulto el formulario
                var formulario = collection["SubProductos"] + collection["Estados"];

                //Obtengo lo campos del formulario
                //    var campos = camposCon.getCamposFormulario(int.Parse(formulario));
                IEnumerable<dynamic> campos = Session["listaCampos"] as dynamic;

                //obtengo los datos desde la interfaz
                //  var values = (collection["_value"]).Split(',').ToArray();
                var llaves = collection.AllKeys;

                for (int i = 0; i < llaves.Length; i++)
                {
                    if (llaves[i].Contains("idCampo_"))
                    {
                        CapturaRadicacion nueva = new CapturaRadicacion();

                        string idcampo = llaves[i].Split(new String[] { "idCampo_" },
                                StringSplitOptions.RemoveEmptyEntries)[0].ToString();

                        var campo = campos.Where(x => Extends.ObtenerValorReflexion(x, "CampId").ToString() == idcampo).FirstOrDefault();

                        if (campo.TcId == 5)
                        {

                            nueva.IdRadicacion = idRadicacion;
                            nueva.IdCampo = campo.CampId;
                            nueva.Valor = CodigoDelItemLista(int.Parse(collection[llaves[i]]));
                            nueva.IdUsuario = data.IdUsusario;
                            nueva.FechaCaptura = DateTime.Now;
                            nueva.IdFormulario = campo.CodFormulario;
                        }
                        else
                        {
                            nueva.IdRadicacion = idRadicacion;
                            nueva.IdCampo = campo.CampId;
                            nueva.Valor = collection[llaves[i]];
                            nueva.IdUsuario = data.IdUsusario;
                            nueva.FechaCaptura = DateTime.Now;
                            nueva.IdFormulario = campo.CodFormulario;

                        }

                        liscaptura.Add(nueva);
                    }
                }


                capRadMod.InsertarCapturaRadicacion(liscaptura);

                ViewData["estado"] = (collection["Estados"]).ToString();
                ViewData["estadoDescrip"] = estado(int.Parse(collection["Estados"].ToString()));
                ViewData["subProducto"] = subProducto(int.Parse(collection["SubProductos"].ToString()));
                ViewData["Oficina"] = oficina(int.Parse(collection["Oficinas"].ToString()));
                ViewData["estadoRadicacion"] = data.TipoRadicacion;
                ViewData["idRadicacion"] = idRadicacion;

                //BUSCO EN LOS STRING ESTOS CARACTERES ESPECIALES Y LOS REEMPLAZO, PRINCIPALMENTE EN LA SOCIEDAD
                char cr = (char)13;
                char lf = (char)10;
                char tab = (char)9;

                ///Se valida estado de la radicacion para guardar el codigo de barrar y almacenarlo en la tabla codigoBarras
                if (data.Estado == 120)
                {
                    if (data.TipoRadicacion.Trim() == "fisico")
                    {
                        char[] MyChar = { ' ' };
                        var nombreImpresora = db.Usuarios.Where(x => x.IdUsuario == data.IdUsusario).First().Periferico_Impresora;
                        var respuesta = extends.CrearEImprimirEtiqueta(ViewData["Oficina"].ToString().TrimStart(MyChar).TrimEnd(MyChar),
                                                                       collection["FechaLugarActual"].ToString().TrimStart(MyChar).TrimEnd(MyChar),
                                                                       ViewData["descSociedad"].ToString().Trim().Replace("\r", "").Replace("\n", "").Replace(cr, ' ').Replace(lf, ' ').Replace(tab, ' ').Trim(),
                                                                       data.CodBarras.Trim(),
                                                                       nombreImpresora.Trim(),
                                                                       data.IdUsusario,
                                                                       (idRadicacion).ToString().Trim());
                    }
                }

                return View();
            }
            catch (Exception exception)
            {
                //LogRepository.registro("Error en RadicacionController metodo Guardar " + exception.Message + " linea " + exception.StackTrace + "Usuario" + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                LogRepository.registro("Error en RadicacionController metodo Guardar " + exception.Message + " linea " + exception.StackTrace + "Usuario" + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                return base.RedirectToAction("Index", "Radicacion");
                //throw;                
            }
        }

        /// <summary>
        /// Guarda el archivo en una ruta especifica para porder leerlo 
        /// </summary>
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> attachments)
        {
            try
            {
                // The Name of the Upload component is "attachments" 
                foreach (var file in attachments)
                {
                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(file.FileName);
                    if (Extends.ValidarExtencion(fileName))
                    {
                        // var reNameFile = RenombrarArchivo(fileName);
                        var physicalPath = Path.Combine(Server.MapPath("/App_Data/ArchivosVirtuales/"), fileName);

                        // The files are not actually saved in this demo
                        file.SaveAs(physicalPath);

                        if (Path.GetExtension(fileName) == ".tif")
                        {
                            Extends s = new Extends();
                            var rutaFisicaTIF = Path.Combine(Server.MapPath("/App_Data/ArchivosVirtuales/"), fileName);

                            string[] archivo = fileName.Split('.');
                            string nomArchivo = archivo[0].ToString() + ".pdf";

                            var rutaFisicaPDF = Path.Combine(Server.MapPath("/App_Data/ArchivosVirtuales/"), nomArchivo);
                            s.ConvertirImagenTifaPDF(rutaFisicaTIF, rutaFisicaPDF);
                            physicalPath = rutaFisicaPDF;
                            fileName = nomArchivo;
                        }

                        Session["CURRENT_FILE"] = physicalPath;
                        string rutaOrigen = db.Parametros.Where(c => c.codigo == "PATH_ARCH_VIRTUAL").First().valor;

                        string pdf = @"" + rutaOrigen + fileName;
                        string tiff = @"" + rutaOrigen + RenombrarArchivoExttiff(fileName);
                        Session["NumPages"] = Extends.ConvertirimagenPDFaTIFF(pdf, tiff);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Ingrese un archivo con extencion correcta");
                    }

                }
                // Return an empty string to signify success
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en RadicacionController metodo SaveFile " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
            return Content("");
        }

        /// <summary>
        /// Elimina el archivo que se subio
        /// </summary>
        public ActionResult Remove(string[] fileNames)
        {

            // The parameter of the Remove action must be called "fileNames"
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath("/App_Data/ArchivosVirtuales/"), fileName);
                var filNameTiff = RenombrarArchivoExttiff(fileName);
                var physicalPathTiff = Path.Combine(Server.MapPath("/App_Data/ArchivosVirtuales/"), filNameTiff);
                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed in this demo
                    System.IO.File.Delete(physicalPath);
                    System.IO.File.Delete(physicalPathTiff);
                }
            }
            // Return an empty string to signify success
            return Content("");
        }

        #region ReimprimirCodigoBarras

        public ActionResult IndexCodigos()
        {
            Session["TITULO"] = "Codigos de barras";
            ViewData["Cliente"] = ((Clientes)Session["CLIENTE"]).CliNombre;
            return View();
        }

        /// <summary>
        /// Carga a la vista los datos de los codigos de barras
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _GrillaCodigosBarras()
        {
            var grilla = this.GrillaCodigo();
            return View(new GridModel<GrillaCodigosBarras>(grilla));
        }

        /// <summary>
        /// Logica necesario para reeimprimir el codigo de barras
        /// </summary>
        /// <param name="IdCodBarras"></param>
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Imprimir(string IdCodBarras)
        {
            CodigosBarrasModel cbm = new CodigosBarrasModel();
            AuditoriaModel am = new AuditoriaModel();
            Auditoria nuevaAuditoria = new Auditoria();

            //Altera cod barras
            cbm.update(long.Parse(IdCodBarras));

            nuevaAuditoria.aud_idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
            nuevaAuditoria.aud_fechaHora = DateTime.Now;
            nuevaAuditoria.aud_evento = "Reimpresion";
            nuevaAuditoria.aud_observaciones = "Se reimprime el registro " + IdCodBarras + " de tabla codigoBarras";
            //Inserta Auditoria
            am.AddAuditoria(nuevaAuditoria);

            var grilla = this.GrillaCodigo();
            return View(new GridModel<GrillaCodigosBarras>(grilla));
        }

        /// <summary>
        /// Obtiene los datos del codigo de barras que se genero
        /// </summary>
        private IEnumerable<GrillaCodigosBarras> GrillaCodigo()
        {
            string nitCliente = (((Clientes)Session["CLIENTE"]).CliNit).ToString();

            var datos = db.spReimpresionCodigoBarras(nitCliente)
                .Select(x => new GrillaCodigosBarras()
                {
                    _CodigoBarras = x.CodBarras,
                    _NomCliente = x.CliNombre,
                    _NomOficina = x.OFI_Nombre,
                    _NomProducto = x.Producto,
                    _NomSubProducto = x.SubProducto,
                    _NomImpresora = x.impresora,
                    _NomUsuario = x.NomUsuario,
                    _NomSociedad = x.Sociedad,
                    FechaRadicacion = x.FechaRadicacion.ToString(),
                    idCodBarras = x.IdCodBarras,
                    idradicaicion = long.Parse(x.IdRadicacion.ToString()),
                    FechaImpreso = x.FechaImpreso.ToString()
                }).ToList();
            return datos;
        }

        #endregion

        /// <summary>
        /// Recepcion del archivo
        /// </summary>
        /// <param name="collection"></param>
        public void RecepcionarArchivo(Radicacion collection)
        {
            try
            {
                Recepcion data = new Recepcion();
                ConsecutivosModel consecutivo = new ConsecutivosModel();
                RecepcionModel modelo = new RecepcionModel();
                CargueLotesModel carguemodel = new CargueLotesModel();

                data.nitCliente = ((GestorDocumental.Models.Clientes)Session["CLIENTE"]).CliNit;
                data.subgrupo = collection.SubProducto;
                data.idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                data.principales = 1;
                data.activo = true;
                data.estado = 0;
                data.numeroLote = ConsecutivosModel.getConsecutivo("LOTES");
                data.fechaRecepcion = DateTime.Now;
                data.observaciones = "Recepcion virtual";

                //Insertar Recepcion
                int id_lote = int.Parse(modelo.insert(data).ToString());

                int numeroPaginas = int.Parse(Session["NumPages"].ToString());

                //Hace la incercion a la tabla carge lotes
                db.spInsertarCargueLotes(data.nitCliente,
                                        id_lote.ToString(),
                                        id_lote,
                                        DateTime.Now,
                                        RenombrarArchivoExttiff(Path.GetFileName(Session["CURRENT_FILE"].ToString())),
                                        numeroPaginas, 1,
                                        int.Parse(data.id.ToString()),
                                        collection.CodBarras,
                                        DateTime.Now,
                                        data.idUsuario,
                                        id_lote.ToString(),
                                        null);

                string lote = id_lote.ToString();
                var negId = db.CargueLotes.Where(x => x.LoteScaner == lote).Select(x => x.NegId).FirstOrDefault();

                //Renombro los archivos
                MueveArchivosYRenombrar(negId.ToString());

                //Actualizar Carguelotes
                carguemodel.ActualizarNegocioCargado(negId);

                //Actualiza la tabla recepcion
                modelo.update(data);

                //Actualiza la tabla carge lotes
                db.spActualizarLotesCargados(id_lote.ToString(), id_lote, collection.IdUsusario);

                Recepcion_Detalle data1 = new Recepcion_Detalle();

                data1.DET_idrecepcion = modelo.idRecepcion;
                data1.DET_Oficina = collection.Oficina;
                data1.DET_Anexo = numeroPaginas;
                modelo.insert_detalle(data1);
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en RadicacionController metodo RecepcionarArchivo " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }

        }

        /// <summary>
        /// Mueve el archivo de la carpeta ArchivosVirtuales a ArchivosCliente y lo renombra
        /// con el negiId que le pertece
        /// </summary>
        public void MueveArchivosYRenombrar(string NegId)
        {
            try
            {
                string Destino = db.Parametros.Where(c => c.codigo == "PATH_TOTAL").First().valor;
                Directory.CreateDirectory(Destino + @"\" + NegId);

                var nameFilePdfNuevo = RenombrarArchivo(Path.GetFileName(Session["CURRENT_FILE"].ToString()), NegId);

                var RutaArchivoPdfViejo = Session["CURRENT_FILE"].ToString();

                System.IO.File.Copy(RutaArchivoPdfViejo, Destino + @"\" + NegId + @"\" + nameFilePdfNuevo);
                System.IO.File.Delete(RutaArchivoPdfViejo);


                //Modifica William Cicua; 05/05/2016; mueve el archivo tif si este se ha creado
                string tifactivo1 = db.Parametros.Where(c => c.codigo == "CONVERSION_A_TIFF_ACTIVO").First().valor;

                int tifactivo = Convert.ToInt32(tifactivo1);
                if (tifactivo == 1)
                {
                    var nameFileTiffNuevo = RenombrarArchivo(RenombrarArchivoExttiff(Path.GetFileName(Session["CURRENT_FILE"].ToString())), NegId);
                    var RutaArchivoTiffViejo = RenombrarArchivoExttiff(Session["CURRENT_FILE"].ToString());
                    System.IO.File.Copy(RutaArchivoTiffViejo, Destino + @"\" + NegId + @"\" + nameFileTiffNuevo);
                    System.IO.File.Delete(RutaArchivoTiffViejo);
                }
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en RadicacionController metodo MueveArchivosYRenombrar " + es.Message);
                throw;
            }
        }

        /// <summary>
        /// Cambia le extencion del archivo a .tiff
        /// </summary>
        /// <param name="nameFile"> nombre del archivo</param>
        /// <returns>archivo renombrado</returns>
        protected string RenombrarArchivoExttiff(string nameFile)
        {
            try
            {
                var arryString = nameFile.Split('.');
                return arryString[0] + ".tif";
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en RecepcionController metodo RenombrarArchivo " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }

        }

        /// <summary>
        /// Re nombre el archivo y mantiene su extencion 
        /// </summary>
        protected string RenombrarArchivo(string nameFile, string NuevoNombre)
        {
            try
            {
                var arryString = nameFile.Split('.');
                return NuevoNombre + "." + arryString[1];
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en RecepcionController metodo RenombrarArchivo " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }

        }

        #region Funciones JSON

        /// <summary>
        /// Guarda el codigo de barras para imprimir
        /// </summary>
        /// <param name="datos">Dalos que va a imprimir</param>
        [HttpGet]
        public string getGenerarCod(string datos)
        {
            try
            {

                LogRepository.registro("ENTRA AL METODO getGenerarCod ");
                char[] MyChar = { ' ' };
                var usuarioLog = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                var dato = datos.Split(';');
                var oficina = (dato[0].TrimStart(MyChar)).TrimEnd(MyChar);
                var sociedad = (dato[1].TrimStart(MyChar)).TrimEnd(MyChar);
                var codigo = (dato[2].TrimStart(MyChar)).TrimEnd(MyChar);
                var fecha = dato[3];
                var idRadicacion = dato[4];
                var nombreImpresora = db.Usuarios.Where(x => x.IdUsuario == usuarioLog).First().Periferico_Impresora;
                var respuesta = new Extends().CrearEImprimirEtiqueta(oficina, fecha, sociedad, codigo, nombreImpresora, usuarioLog, idRadicacion);
                string mensaje = string.Empty;
                if (respuesta == true)
                {
                    mensaje = "1";
                }
                else
                {
                    mensaje = "0";
                }
                return mensaje;

            }
            catch (Exception es)
            {

                LogRepository.registro("Error en RadicacionController metodo getGenerarCod " + es.Message);
                return "Error al imprimir el codigo";
                throw;
            }

        }

        [HttpPost]
        public JsonResult _GetDropDownList_Oficinas(int? DropDownList_Clientes)
        {
            try
            {
                DropDownList_Clientes = (int)((Clientes)Session["CLIENTE"]).CliNit;
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().P_Oficinas.Where(c => c.OFI_CodNit == DropDownList_Clientes).ToList(), "OFI_IdOficina", "OFI_Nombre"),

                JsonRequestBehavior.AllowGet);
            }

            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownList_Oficinas " + exception.Message);
                throw;
            }

        }

        [HttpPost]
        public JsonResult _GetDropDownListSubProductos(int? DropDownList_Grupos)
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().Grupos.Where(c => c.GruIdPadre == DropDownList_Grupos).ToList(), "GruId", "GruDescripcion"),
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownListSubProductos " + exception.Message);
                throw;
            }

        }

        [HttpPost]
        public JsonResult _GetDropDownListProductos(int? DropDownList_Clientes)
        {
            try
            {
                DropDownList_Clientes = (int)((Clientes)Session["CLIENTE"]).CliNit;
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().Grupos.Where(c => c.CliNit == DropDownList_Clientes).ToList(), "GruId", "GruDescripcion"),

                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownListProductos " + exception.Message);
                throw;
            }

        }

        [HttpPost]
        public JsonResult _GetDropDownListEstados()
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().P_Estados.Where(c => c.Tipo == 1).ToList(), "IdEstado", "DescEstado"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetDropDownListEstados " + exception.Message);
                throw;
            }

        }


        [HttpGet]
        public JsonResult _GetCampos(int CodOrigen)
        {
            try
            {

                CamposController camposCon = new CamposController();
                var campos = camposCon.getCamposFormulario(CodOrigen);
                Session["listaCampos"] = campos;
                return Json(campos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RadicacionController metodo _GetCampos " + exception.Message);
                throw;
            }

        }

        [HttpGet]
        public JsonResult _GetValoresList(int CampId)
        {
            try
            {
                string nitCliente = (((Clientes)Session["CLIENTE"]).CliNit).ToString();
                CamposController camposCon = new CamposController();
                return Json(new SelectList(camposCon.getValoresListCamp(CampId, nitCliente), "CodCampId", "CodDescripcion"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                LogRepository.registro("Error en RadicacionController metodo _GetValoresList " + exception.Message);
                throw;
            }
        }


        public int? DropDownList_Oficinas { get; set; }

        #endregion

        #region ConsultItemList


        /// <summary>
        /// Obtiene la descripcion del la sociedad por el codigo
        /// </summary>
        private string oficina(int idOficina)
        {
            try
            {
                return new GestorDocumental.Models.GestorDocumentalEnt().P_Oficinas.Where(c => c.OFI_IdOficina == idOficina).FirstOrDefault().OFI_Nombre;
            }
            catch (Exception es)
            {
                return "";
            }

        }

        /// <summary>
        /// Obtiene la descripcion de los campos deatalle
        /// </summary>
        private string CodigosCampos(int codCampId)
        {
            try
            {
                return new GestorDocumental.Models.GestorDocumentalEnt().CodigosCampo.Where(c => c.CodCampId == codCampId).FirstOrDefault().CodDescripcion;
            }
            catch (Exception es)
            {
                return "";
            }

        }

        /// <summary>
        /// Obtiene la descripcion del la subproducto por el codigo
        /// </summary>
        private string subProducto(int idSubProducto)
        {
            try
            {
                return new GestorDocumental.Models.GestorDocumentalEnt().Grupos.Where(c => c.GruId == idSubProducto).FirstOrDefault().GruDescripcion;
            }
            catch (Exception es)
            {
                return "";
            }

        }

        /// <summary>
        /// Obtiene la descripcion del la estado por el codigo
        /// </summary>
        private string estado(int idEstado)
        {
            try
            {
                return new GestorDocumental.Models.GestorDocumentalEnt().P_Estados.Where(e => e.IdEstado == idEstado).FirstOrDefault().DescEstado;
            }
            catch (Exception es)
            {
                return "";
            }

        }

        /// <summary>
        /// Obtiene el codigo que tiene cada item de una lista
        /// </summary>
        private string CodigoDelItemLista(int codCampId)
        {
            try
            {
                return new GestorDocumental.Models.GestorDocumentalEnt().CodigosCampo.Where(c => c.CodCampId == codCampId).FirstOrDefault().CodId;
            }
            catch (Exception es)
            {
                return "";
            }
        }

        #endregion

    }

    /// <summary>
    /// Clase que tiene las columnas para ver la grilla de los codigos de barras
    /// </summary>
    public class GrillaCodigosBarras
    {
        public string _CodigoBarras { set; get; }
        public string _NomCliente { set; get; }
        public string _NomOficina { set; get; }
        public string _NomProducto { set; get; }
        public string _NomSubProducto { set; get; }
        public string _NomImpresora { set; get; }
        public string _NomUsuario { set; get; }
        public string _NomSociedad { set; get; }
        public string FechaRadicacion { set; get; }
        public long idCodBarras { set; get; }
        public long idradicaicion { set; get; }
        public string FechaImpreso { set; get; }
    }

}
