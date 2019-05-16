using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using GestorDocumental.Controllers;
using GestorDocumental.Models;
using GestorDocumental.WebService;

namespace GestorDocumental.ViewsAspx
{
    public partial class CapturaInformacion : System.Web.UI.Page
    {
        private GestorDocumental.Controllers.AsignacionesController bAsig = new Controllers.AsignacionesController();
        private GestorDocumental.Controllers.CamposController bCampos = new Controllers.CamposController();
        private GestorDocumental.Controllers.CapturaController bCap;
        private GestorDocumental.Controllers.DocumentosController bdoc;

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "FuncionLoad", "pageLoad();", true);
            try
            {
                if (!this.Page.IsPostBack)
                {
                    this.CargarDocumentos((Clientes)this.Session["CLIENTE"]);

                    P_Etapas etapas = new P_Etapas();
                    if (base.Request.QueryString["CAPTURA"].ToString() == "1")
                    {
                        Session["TITULO"] = "Captura de datos No. 1";
                        etapas.IdEtapa = 30;
                        this.Session["ETAPA"] = etapas;
                    }
                    else if (base.Request.QueryString["CAPTURA"].ToString() == "2")
                    {
                        Session["TITULO"] = "Captura de datos No. 2";
                        etapas.IdEtapa = 40;
                        this.Session["ETAPA"] = etapas;
                    }
                    else if (base.Request.QueryString["CAPTURA"].ToString() == "3")
                    {
                        Session["TITULO"] = "Control de calidad captura";
                        etapas.IdEtapa = 50;
                        this.Session["ETAPA"] = etapas;
                    }

                    this.bCap = new GestorDocumental.Controllers.CapturaController();
                    Captura c = new Captura();
                    if (this.bAsig.ObtenerNegociosXEntrada((Usuarios)this.Session["USUARIO"], etapas) == 0M)
                    {
                        c.NegId = 0M;
                        this.Session["NEGOCIO"] = c;
                        this.lstDocumentos.Enabled = false;
                        this.lstGrupos.Enabled = false;
                        this.lblError.Text = "No existen negocios disponibles para esta etapa";
                        this.UpdatePanel2.Update();
                    }
                    else
                    {
                        int num5;
                        int num6;
                        c.NegId = this.bAsig.ObtenerNegociosXEntrada((Usuarios)this.Session["USUARIO"], etapas);
                        c.NumCaptura = int.Parse(base.Request.QueryString["CAPTURA"].ToString());
                        this.Session["NEGOCIO"] = c;

                        ////BUSCO EL CLIENTE ASOCIADO A ESE NEGOCIO Y LO SOBREESCRIBO EN LA SESSION
                        ////REENDERIZO NUEVAMENTE EL COMBO
                        AsignacionTareas a = new AsignacionTareas
                        {
                            IdEtapa = ((P_Etapas)this.Session["ETAPA"]).IdEtapa,
                            NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                            Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                            IdEstado = 10,
                            HoraInicio = DateTime.Now
                        };

                        if (!this.bAsig.ExisteEtapa(a))
                        {
                            this.bAsig.insertarAsignacion(a);
                        }

                        //Obtenemos el grupo asociado al negocio en proceso
                        int negID = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                        int idGrupo = bCap.obtenerSubGrupo(negID);
                        int num = this.bCap.ObtenerDocumentoActual(c, (Clientes)this.Session["CLIENTE"], idGrupo);

                        Documentos d = new Documentos
                        {
                            DocId = num
                        };

                        //********************
                        int num2 = this.bCap.ObtenerGrupoActual(c, d);
                        int num3 = this.bCap.ObtenerUltimoGrupo(d);

                        //SE DEBE CONSIDERAR QUE MUCHOS DOCUMENTOS  TIENEN SOLO UN GRUPO , POR LO TANTO SE DEBE VALIDAR SI ES EL PRIMERO Y ES UNICO , NO PASAR AL SIGUIENTE DOCUMENTO 
                        //Tambien se debe validar si el grupo ya ha sido caturado
                        if ((num2 == num3 & num3 != 1) | (num2 == num3 & bCap.ExisteCapturaGrupo(c, num3)))
                        {
                            //Obtenemos el grupo asociado al negocio en proceso
                            int negID_docum = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                            int idGrupo_docum = bCap.obtenerSubGrupo(negID_docum);

                            int num4 = this.bCap.ObtenerSiguienteDocumento(c, (Clientes)this.Session["CLIENTE"], idGrupo_docum);
                            d.DocId = num4;

                            this.lstDocumentos.SelectedIndex = this.lstDocumentos.Items.IndexOf(this.lstDocumentos.Items.FindByValue(num4.ToString()));
                            this.CargarGruposDocs(d);

                            num5 = this.bCap.ObtenerSiguienteGrupo(c, d);
                            this.lstGrupos.SelectedIndex = this.lstGrupos.Items.IndexOf(this.lstGrupos.Items.FindByValue(num5.ToString()));

                            //Obtenemos el grupo asociado al negocio en proceso
                            int negID3 = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                            int idGrupo3 = bCap.obtenerSubGrupo(negID3);

                            num6 = this.bCap.obtenerUltimoDocumento((Clientes)this.Session["CLIENTE"], idGrupo3, negID3);
                        }
                        else
                        {
                            d.DocId = num;
                            this.lstDocumentos.SelectedIndex = this.lstDocumentos.Items.IndexOf(this.lstDocumentos.Items.FindByValue(num.ToString()));
                            this.CargarGruposDocs(d);
                            num5 = this.bCap.ObtenerSiguienteGrupo(c, d);
                            this.lstGrupos.SelectedIndex = this.lstGrupos.Items.IndexOf(this.lstGrupos.Items.FindByValue(num5.ToString()));

                            //Obtenemos el grupo asociado al negocio en proceso
                            int _negID = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                            int _idGrupo = bCap.obtenerSubGrupo(_negID);

                            num6 = this.bCap.obtenerUltimoDocumento((Clientes)this.Session["CLIENTE"], _idGrupo, _negID);
                        }
                        this.CargarCampos();
                        ((Label)base.Master.FindControl("lblNegocio")).Text = "Negocio:" + c.NegId.ToString();
                    }
                }
                else
                {
                    this.CargarCampos();
                }

                this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "FileLoad", "neg = '" + ConfigurationManager.AppSettings["ClientFiles"] + "/" + ((Captura)this.Session["NEGOCIO"]).NegId.ToString() + "/" + ((Captura)this.Session["NEGOCIO"]).NegId.ToString() + ".TIF';", true);
                ScriptManager.RegisterStartupScript(btnInsertar, btnInsertar.GetType(), "EnterDisable", "$().ready(function () { $('input').keypress(function (evt) { var charCode = evt.charCode || evt.keyCode; if (charCode == 13) {return false;}});});", true);
                ScriptManager.RegisterStartupScript(btnInsertar, btnInsertar.GetType(), "checkBoxEvent", "  $().ready(function () { function asignarEvento(objeto) { $('input[type=checkbox]').focusin(function () { CoordenadasCheckbox(this) });} asignarEvento();});", true);

                if (((Captura)this.Session["NEGOCIO"]).NegId != 0M)
                {
                    Documentos documentos2 = new Documentos
                    {
                        DocId = int.Parse(this.lstDocumentos.SelectedValue)
                    };
                    this.bdoc = new GestorDocumental.Controllers.DocumentosController();
                    this.NumPagina.Value = this.bdoc.ObtenrPaginaDocumento(documentos2, (Captura)this.Session["NEGOCIO"]).ToString();
                }
                this.lstDocumentos.Attributes.Add("onChange", "CargarPaginaDigitada();");
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo Page_Load " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            #region Guarda el proceso de las Capturas.
            try
            {
                //***********************************************
                if (((P_Etapas)this.Session["ETAPA"]).IdEtapa == 50)
                {
                    bCap = new CapturaController();

                    /*debemos insetar la etapa cruce de identidad  */
                    if (bCap.ExisteControlCalidad((Captura)this.Session["NEGOCIO"]) == 0)
                    {
                        AsignacionTareas a1 = new AsignacionTareas
                        {
                            IdEstado = 30,
                            HoraTerminacion = new DateTime?(DateTime.Now),
                            NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                            IdEtapa = ((P_Etapas)this.Session["ETAPA"]).IdEtapa
                        };

                        this.bAsig.insertarAsignacion(a1);

                        /*Asignamos el estado de terminacion*/
                        CapturaController controlador = new CapturaController();

                        //CREO LA ETAPA RESPECTIVA Y PASO EL USUARIO
                        string tipoCaptura = base.Request.QueryString["Captura"].ToString();

                        if (tipoCaptura.Equals("3"))
                        {
                            string negId = ((Captura)this.Session["NEGOCIO"]).NegId.ToString();
                            string usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario.ToString();

                            //Volcar los datos de la captura a las tablas del WebService
                            WebServiceModel ws = new WebServiceModel();
                            ws.volcarDatosCaptura_WebService(Convert.ToInt32(negId));

                            //Crear el archivo XML y guardarlo en una ruta.
                            string rutaXML = CrearXML.CrearXMLFile(Convert.ToInt32(negId));

                            if (File.Exists(rutaXML))
                            {
                                //La operación para enviar la factura XML mediante el web Service a SAP
                                //TransmicionXMLController operacionesSAP = new TransmicionXMLController();
                                //string respuestaSAP = operacionesSAP.ExecuteWebService(rutaXML);
                                //string resultadoFinal = operacionesSAP.guardarResultadoSAP(respuestaSAP, usuario, negId);

                                Thread t = new Thread(() => new WebServiceController().llamadoWebService(rutaXML, usuario.ToString(), negId));
                                t.Start();
                                t.Join();
                            }
                        }

                        base.Response.Redirect("/Captura/confirmaPagina?Captura=" + tipoCaptura);
                    }
                }

                //Guardamos los valores de la Grilla de Posiciones y Impuestos
                string[] listaIndices = Request.Form.GetValues("indicePos");
                int tCaptura = Convert.ToInt32(base.Request.QueryString["Captura"].ToString());

                if (this.lstGrupos.SelectedItem.Text.Contains("Posiciones") && tCaptura == 1 || this.lstGrupos.SelectedItem.Text.Contains("Posiciones") && tCaptura == 2
                    || this.lstGrupos.SelectedItem.Text.Contains("Impuestos") && tCaptura == 1 || this.lstGrupos.SelectedItem.Text.Contains("Impuestos") && tCaptura == 2)
                {
                    if (listaIndices != null)
                    {
                        Session["CamposCCalidad"] = null;
                        guardarValoresGrillaPosiciones();
                    }
                }

                //Guardamos los valores de la Grilla de control de calidad
                if (Session["CamposCCalidad"] != null)
                {
                    guardarControlCalidad();
                }

                this.bCap = new GestorDocumental.Controllers.CapturaController();
                List<Captura> lstCaptura = new List<Captura>();
                this.InsertarCampos(this.pControls, lstCaptura);

                //Si no existen campos para guardar, debemos crear un campo comodin asociado al grupo actual e insertarlo 
                //con el fin de mantener la posicion actual

                //1. Obtenemos el documento actual  antes de hacer la insercion 
                Documentos DocActual = new Documentos();
                DocActual.DocId = int.Parse(lstDocumentos.SelectedValue);

                //2.Obtenemos el grupo actual antes de la insercion
                GruposDocumentos grupoActual = new GruposDocumentos();
                grupoActual.GDocId = int.Parse(lstGrupos.SelectedValue);

                //Creamos el campo y lo anexamos a la lista 
                Campos CampComodin;

                if (lstCaptura.Count == 0)
                {
                    CampComodin = bCampos.ObtenerCampoComodin(grupoActual, (Clientes)this.Session["CLIENTE"]);
                    Captura CapComodin = new Captura
                    {
                        CampId = CampComodin.CampId,
                        NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                        NegValor = "",
                        Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                        DocId = int.Parse(this.lstDocumentos.SelectedValue),
                        FechaRegistro = DateTime.Now,
                        NumCaptura = int.Parse(base.Request.QueryString["Captura"])
                    };
                    lstCaptura.Add(CapComodin);
                }

                this.bCap.InsertarCaptura(lstCaptura);

                //Obtenemos el grupo asociado al negocio en proceso
                int negID = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                int idGrupo = bCap.obtenerSubGrupo(negID);

                int num = this.bCap.obtenerUltimoDocumento((Clientes)this.Session["CLIENTE"], idGrupo, negID);

                //Parametrizando los Documentos
                Documentos d = new Documentos
                {
                    DocId = this.bCap.ObtenerDocumentoActual((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"], idGrupo)
                };

                Documentos documentos2 = new Documentos
                {
                    DocId = this.bCap.ObtenerSiguienteDocumento((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"], idGrupo)
                };

                int num2 = 0;

                ///DOCUMENTO ACTUAL = ULTIMO DOCUMENTO Y EL GRUPO INICIAL = ULTIMO GRUPO 
                /// O SIGUIENTE DOCUMENTO = DOCUMENTO ACTUAL (REVISAR ASI ESTABA LA VALIDACION )
                int gactual = this.bCap.ObtenerGrupoActual((Captura)this.Session["NEGOCIO"], d);
                int gsiguiente = this.bCap.ObtenerUltimoGrupo(d);

                ///Se debe validar que 
                if ((d.DocId == num) & (gactual == gsiguiente))
                {
                    AsignacionTareas a = new AsignacionTareas
                    {
                        IdEstado = 30,
                        HoraTerminacion = new DateTime?(DateTime.Now),
                        NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                        IdEtapa = ((P_Etapas)this.Session["ETAPA"]).IdEtapa
                    };
                    this.bAsig.insertarAsignacion(a);

                    /*************************/
                    string tipoCaptura = base.Request.QueryString["Captura"].ToString();
                    num2 = 1;
                }
                else
                {
                    int num6;
                    int num3 = this.bCap.ObtenerGrupoActual((Captura)this.Session["NEGOCIO"], d);
                    int num4 = this.bCap.ObtenerUltimoGrupo(d);
                    if (num3 == num4)
                    {
                        //Obtenemos el grupo asociado al negocio en proceso
                        int negID_docum = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                        int idGrupo_docum = bCap.obtenerSubGrupo(negID_docum);

                        int num5 = this.bCap.ObtenerSiguienteDocumento((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"], idGrupo_docum);
                        this.lstDocumentos.SelectedIndex = this.lstDocumentos.Items.IndexOf(this.lstDocumentos.Items.FindByValue(num5.ToString()));
                        Documentos documentos3 = new Documentos
                        {
                            DocId = num5
                        };
                        this.CargarGruposDocs(documentos3);
                        num6 = this.bCap.ObtenerSiguienteGrupo((Captura)this.Session["NEGOCIO"], documentos3);
                        this.lstGrupos.SelectedIndex = this.lstGrupos.Items.IndexOf(this.lstGrupos.Items.FindByValue(num6.ToString()));
                    }
                    else
                    {
                        //Obtenemos el grupo asociado al negocio en proceso
                        int negID2 = (int)((Captura)this.Session["NEGOCIO"]).NegId;
                        int idGrupo2 = bCap.obtenerSubGrupo(negID2);

                        Documentos documentos4 = new Documentos
                        {
                            DocId = this.bCap.ObtenerDocumentoActual((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"], idGrupo2)
                        };
                        num6 = this.bCap.ObtenerSiguienteGrupo((Captura)this.Session["NEGOCIO"], documentos4);
                        this.lstGrupos.SelectedIndex = this.lstGrupos.Items.IndexOf(this.lstGrupos.Items.FindByValue(num6.ToString()));
                    }
                }

                this.UpdatePanel1.Update();
                this.pControls.Controls.Clear();
                this.CargarCampos();
                this.UpdatePanel2.Update();

                Documentos documentos5 = new Documentos
                {
                    DocId = int.Parse(this.lstDocumentos.SelectedValue)
                };

                this.bdoc = new GestorDocumental.Controllers.DocumentosController();
                this.NumPagina.Value = this.bdoc.ObtenrPaginaDocumento(documentos5, (Captura)this.Session["NEGOCIO"]).ToString();

                if (num2 == 1)
                {
                    string tipoCaptura = base.Request.QueryString["Captura"].ToString();
                    if (tipoCaptura.Equals("3"))
                    {
                        decimal negID2 = (decimal)((Captura)this.Session["NEGOCIO"]).NegId;
                    }

                    ///Si se inserta la ultima captura 1 se hace la ejecucion del sp de capturaRadicacion a captura
                    if (tipoCaptura.Equals("1"))
                    {
                        CapturaController controlador = new CapturaController();
                        decimal negId = ((Captura)this.Session["NEGOCIO"]).NegId;
                        controlador.spCapturaRadicacionACaptura(negId);
                    }

                    Session["CamposCCalidad"] = null;
                    Session["Id_Grilla"] = null;

                    //Crea el archivo XML cuando termina Control de Calidad con el negocio que finaliza
                    //Solo para las facturas
                    if (tipoCaptura.Equals("3"))
                    {
                        string negId = ((Captura)this.Session["NEGOCIO"]).NegId.ToString();
                        string usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario.ToString();

                        //Volcar los datos de la captura a las tablas del WebService
                        WebServiceModel ws = new WebServiceModel();
                        ws.volcarDatosCaptura_WebService(Convert.ToInt32(negId));

                        //Crear el archivo XML y guardarlo en una ruta.
                        string rutaXML = CrearXML.CrearXMLFile(Convert.ToInt32(negId));

                        if (File.Exists(rutaXML))
                        {
                            //La operación para enviar la factura XML mediante el web Service a SAP
                            //TransmicionXMLController operacionesSAP = new TransmicionXMLController();
                            //string respuestaSAP = operacionesSAP.ExecuteWebService(rutaXML);
                            //string resultadoFinal = operacionesSAP.guardarResultadoSAP(respuestaSAP, usuario, negId);

                            Thread t = new Thread(() => new WebServiceController().llamadoWebService(rutaXML, usuario.ToString(), negId));
                            t.Start();
                            t.Join();
                        }
                    }

                    base.Response.Redirect("/Captura/confirmaPagina?Captura=" + tipoCaptura);
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo Button1_Click1 " + exception.Message);
                this.lblError.Text = exception.Message;
                this.UpdatePanel1.Update();
            }
            #endregion
        }

        private void CargarCampos()
        {
            try
            {
                if (this.lstGrupos.SelectedIndex > 0)
                {
                    GruposDocumentos g = new GruposDocumentos
                    {
                        GDocId = Convert.ToInt32(this.lstGrupos.SelectedValue)
                    };
                    P_Etapas e = new P_Etapas
                    {
                        IdEtapa = int.Parse(base.Request.QueryString["CAPTURA"])
                    };
                    Captura n = (Captura)this.Session["NEGOCIO"];
                    List<Campos> lstCampos = this.bCampos.ObtenerCamposCliente(g, e, n);
                    if (lstCampos.Count == 0)
                    {
                        Campos camp = new Campos()
                        {
                            Activo = false
                        };
                        lstCampos.Add(camp);
                    }

                    ///SI LA ETAPA ES 3 , CONTROL DE CALIDAD SE DEBE CARGAR LOS CAMPOS  DE RESPUESTAS ANTERIORES 
                    List<spObtenerRespuestasAnteriores_Result> lstRespAnt = null;

                    if (e.IdEtapa == 3 && this.lstGrupos.SelectedItem.Text.Contains("Posiciones") && lstCampos[0].Activo == false)
                    {
                        Session["tId_Etapa"] = e.IdEtapa;

                        Campos tiposCampo = new Campos();
                        tiposCampo.TcId = 13;
                        tiposCampo.GDocId = g.GDocId;
                        tiposCampo.ControlCalidad = true;

                        List<Campos> listasCCalidad = new List<Campos>();
                        listasCCalidad.Add(tiposCampo);
                        this.GenerarCampos(this.tblControls, listasCCalidad, lstRespAnt);
                    }
                    else if (e.IdEtapa == 3 && this.lstGrupos.SelectedItem.Text.Contains("Impuestos") && lstCampos[0].Activo == false)
                    {
                        Session["tId_Etapa"] = e.IdEtapa;

                        Campos tiposCampo = new Campos();
                        tiposCampo.TcId = 14;
                        tiposCampo.GDocId = g.GDocId;
                        tiposCampo.ControlCalidad = true;

                        List<Campos> listasCCalidad = new List<Campos>();
                        listasCCalidad.Add(tiposCampo);
                        this.GenerarCampos(this.tblControls, listasCCalidad, lstRespAnt);
                    }
                    else if (e.IdEtapa == 3 && this.lstGrupos.SelectedItem.Text.Contains("Cabecera") && lstCampos[0].Activo == true || e.IdEtapa == 3 && this.lstGrupos.SelectedItem.Text.Contains("Posiciones") && lstCampos[0].Activo == true
                        || e.IdEtapa == 3 && this.lstGrupos.SelectedItem.Text.Contains("Impuestos") && lstCampos[0].Activo == true)
                    {
                        lstRespAnt = bCampos.ObtenerRespuestasAnteriores(n);
                        this.GenerarCampos(this.tblControls, lstCampos, lstRespAnt);
                        Session["tId_Etapa"] = null;
                    }
                    else
                    {
                        Session["tId_Etapa"] = null;
                        lstRespAnt = bCampos.ObtenerRespuestasAnteriores(n);
                        this.GenerarCampos(this.tblControls, lstCampos, lstRespAnt);
                        Campos campos = lstCampos.Find(c => c.CampId == 5);

                        Hashtable hashtable = this.bCampos.CamposPresentados(this.bCampos.ObtenerCamposCliente(g, e, n));
                        this.UpdatePanel2.Update();
                    }
                    ///***************************************************************************************
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo CargarCampos " + exception.Message);
                throw;
            }
        }

        protected void cargarCiudades(object sender, EventArgs e, object Id)
        {
            try
            {
                P_Departamentos d = new P_Departamentos
                {
                    DeptId = Convert.ToInt32(((DropDownList)sender).SelectedValue)
                };
                List<P_Ciudad> list = this.bCampos.obtenerCiudades(d);
                DropDownList list2 = (DropDownList)this.Panel1.FindControl("lstCiu_" + ((DropDownList)sender).ID.Split(new char[] { '_' }).GetValue(2));
                list2.DataSource = list;
                list2.DataTextField = "CiuNombre";
                list2.DataValueField = "CiuId";
                list2.DataBind();
                list2.Focus();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo cargarCiudades " + exception.Message);
                throw exception;
            }
        }

        private void cargarDepartamentos(DropDownList lst, List<P_Departamentos> lstCods)
        {
            try
            {
                lst.DataSource = lstCods;
                lst.DataTextField = "DeptNombre";
                lst.DataValueField = "DeptId";
                lst.DataBind();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo cargarDepartamentos " + exception.Message);
                throw exception;
            }
        }

        private void CargarDocumentos(Clientes c)
        {
            try
            {
                this.bdoc = new GestorDocumental.Controllers.DocumentosController();
                List<Documentos> list = this.bdoc.obtenerDocumentosCliente(c);
                this.lstDocumentos.Items.Clear();
                this.lstDocumentos.DataSource = list;
                this.lstDocumentos.DataTextField = "DocDescripcion";
                this.lstDocumentos.DataValueField = "DocId";
                this.lstDocumentos.DataBind();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo CargarDocumentos " + exception.Message);
                throw;
            }

        }

        private void CargarGruposDocs(Documentos D)
        {
            try
            {
                this.bdoc = new GestorDocumental.Controllers.DocumentosController();
                List<GruposDocumentos> list = this.bdoc.obtenerGruposDocumentos(D);
                this.lstGrupos.Items.Clear();
                this.lstGrupos.DataSource = list;
                this.lstGrupos.DataTextField = "GDocDescripcion";
                this.lstGrupos.DataValueField = "GDocId";
                this.lstGrupos.DataBind();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo CargarGruposDocs " + exception.Message);
                throw;
            }

        }

        private void cargarListasCodigos(DropDownList lst, List<CodigosCampo> lstCods)
        {
            try
            {
                lst.DataSource = lstCods;
                lst.DataTextField = "CodDescripcion";
                lst.DataValueField = "CodiD";
                lst.DataBind();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo cargarListasCodigos " + exception.Message);
                throw exception;
            }
        }

        private void cargarListasCodigos(CheckBoxList lst, List<CodigosCampo> lstCods)
        {
            try
            {
                lst.DataSource = lstCods;
                lst.DataTextField = "CodDescripcion";
                lst.DataValueField = "CodiD";
                lst.DataBind();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo cargarListasCodigos " + exception.Message);
                throw exception;
            }
        }

        private void GenerarCampos(Table Ta, List<Campos> lstCampos, List<spObtenerRespuestasAnteriores_Result> lstRespAnt)
        {
            try
            {
                int num = 0;
                Table child = new Table
                {
                    ID = "TBL_PRICIPAL" + this.lstGrupos.SelectedValue,
                    EnableViewState = true
                };

                this.pControls.Controls.Add(child);
                if (((P_Etapas)this.Session["ETAPA"]).IdEtapa == 50)
                    child.Attributes.Add("data-work", "tablaPrincipalControlCalidad");
                else
                    child.Attributes.Add("data-work", "tablaPrincipal");

                using (List<Campos>.Enumerator enumerator = lstCampos.GetEnumerator())
                {
                    EventHandler handler = null;
                    Campos c;
                    while (enumerator.MoveNext())
                    {
                        RequiredFieldValidator validator2;
                        ValidatorCalloutExtender extender3;
                        c = enumerator.Current;
                        TableRow row = new TableRow
                        {
                            ID = "Trow_" + c.CampId
                        };

                        TableCell cell = new TableCell
                        {
                            ID = "TcellDesc_" + c.CampId

                        };
                        TableCell cell2 = new TableCell
                        {
                            ID = "TcellCtrl_" + c.CampId
                        };
                        TableCell cell3 = new TableCell
                        {
                            ID = "TcellError_" + c.CampId
                        };
                        row.Cells.Add(cell);
                        row.Cells.Add(cell2);
                        row.Cells.Add(cell3);

                        ///DE DEBEN CREAR DOS NUEVAS CELDAS EN LA PARTE IZQUIERDA DONDE SE DESPLIEGUEN LAS RESPUESTAS 1 Y 2
                        ///SE DEBE VALIDAR  QUE CORRESPONDA A LA ETAPA 3 
                        ///
                        if (lstRespAnt != null)
                        {

                            TableCell cellR1 = new TableCell
                            {
                                ID = "TcellResp1_" + c.CampId,
                                Text = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : ""),
                                ForeColor = Color.Brown
                            };
                            TableCell cellR2 = new TableCell
                            {
                                ID = "TcellResp2_" + c.CampId,
                                Text = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : ""),
                                ForeColor = Color.DarkBlue
                            };

                            row.Cells.Add(cellR1);
                            row.Cells.Add(cellR2);

                        }
                        ///***************************************************************************************************

                        child.Rows.Add(row);
                        WebServiceModel s = new WebServiceModel();
                        IndicesGrillas indiceGrid = new IndicesGrillas();

                        switch (lstCampos[num].TcId)
                        {
                            case 1:
                                {

                                    TextBox box = new TextBox
                                    {
                                        ID = "txt_" + c.CampId,
                                        Width = c.CampAncho,
                                        MaxLength = (int)c.LongMax
                                    };

                                    box.Attributes.Add("onfocus", "scrollVisor(" + c.PosX + "," + c.PosY + ")");

                                    RoundedCornersExtender extender = new RoundedCornersExtender
                                    {
                                        TargetControlID = "txt_" + c.CampId.ToString(),
                                        Radius = 6,
                                        ID = "RoundedExtender_rtxt" + num.ToString(),
                                        Corners = BoxCorners.All,
                                        Enabled = true
                                    };
                                    RegularExpressionValidator validator = new RegularExpressionValidator
                                    {
                                        ID = "txtRegular" + c.CampId.ToString(),
                                        ControlToValidate = "txt_" + c.CampId.ToString(),
                                        ValidationExpression = @"\d{0," + c.LongMax + "}",
                                        ErrorMessage = "<B><i>El campo debe ser numerico</i></B>",
                                        SetFocusOnError = true,
                                        Display = ValidatorDisplay.Dynamic,
                                        ForeColor = Color.Red
                                    };

                                    /*funcion de validacion personalizada*/
                                    CustomValidator CustomVal_txt;
                                    if (c.ValidationFunction != null)
                                    {
                                        CustomVal_txt = new CustomValidator
                                        {
                                            ID = "customval_" + c.CampId.ToString(),
                                            ControlToValidate = "txt_" + c.CampId,
                                            ClientValidationFunction = c.ValidationFunction,
                                            ErrorMessage = "<B><i>" + c.ErrorMessage + "</i></B>",
                                            SetFocusOnError = true,
                                            Display = ValidatorDisplay.Dynamic,
                                            ForeColor = Color.Red

                                        };
                                        row.Cells[1].Controls.Add(CustomVal_txt);
                                    }

                                    /***********************************************/
                                    row.Cells[0].Text = c.CampDescripcion + ": ";
                                    row.Cells[1].Controls.Add(box);
                                    row.Cells[1].Controls.Add(extender);
                                    row.Cells[1].Controls.Add(validator);

                                    if (c.CampObligatorio)
                                    {
                                        row.Cells[0].Font.Bold = true;

                                        validator2 = new RequiredFieldValidator
                                        {
                                            ID = "txtReq" + c.CampId.ToString(),
                                            ControlToValidate = "txt_" + c.CampId,
                                            ErrorMessage = "<B><i>Campo requerido</i></B>",
                                            Display = ValidatorDisplay.Dynamic,
                                            SetFocusOnError = true,
                                            ForeColor = Color.Red
                                        };
                                        row.Cells[1].Controls.Add(validator2);
                                    }
                                    if (num == 0)
                                    {
                                        box.Focus();
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    TextBox box2 = new TextBox
                                    {
                                        ID = "txt_" + c.CampId,
                                        Width = c.CampAncho,
                                        MaxLength = (int)c.LongMax

                                    };

                                    box2.Attributes.Add("onfocus", "scrollVisor(" + c.PosX + "," + c.PosY + ")");
                                    RoundedCornersExtender extender4 = new RoundedCornersExtender
                                    {
                                        TargetControlID = "txt_" + c.CampId.ToString(),
                                        Radius = 6,
                                        ID = "RoundedExtender_rtxt" + num.ToString(),
                                        Corners = BoxCorners.All,
                                        Enabled = true

                                    };
                                    box2.CssClass = "texto";
                                    row.Cells[0].Text = c.CampDescripcion + ": ";
                                    row.Cells[1].Controls.Add(box2);
                                    row.Cells[1].Controls.Add(extender4);
                                    if (c.CampObligatorio)
                                    {
                                        row.Cells[0].Font.Bold = true;

                                        validator2 = new RequiredFieldValidator
                                        {
                                            ID = "txtReq" + c.CampId.ToString(),
                                            ControlToValidate = "txt_" + c.CampId,
                                            ErrorMessage = "<B><i>Campo requerido</i></B>",
                                            Display = ValidatorDisplay.Dynamic,
                                            SetFocusOnError = true,
                                            ForeColor = Color.Red
                                        };
                                        row.Cells[1].Controls.Add(validator2);
                                    }

                                    /*funcion de validacion personalizada*/
                                    CustomValidator CustomVal_txt;
                                    if (c.ValidationFunction != null)
                                    {
                                        CustomVal_txt = new CustomValidator
                                        {
                                            ID = "customval_" + c.CampId.ToString(),
                                            ControlToValidate = "txt_" + c.CampId,
                                            ClientValidationFunction = c.ValidationFunction,
                                            ErrorMessage = "<B><i>" + c.ErrorMessage + "</i></B>",
                                            SetFocusOnError = true,
                                            Display = ValidatorDisplay.Dynamic,
                                            ForeColor = Color.Red

                                        };
                                        row.Cells[1].Controls.Add(CustomVal_txt);
                                    }
                                    if (num == 0)
                                    {
                                        box2.Focus();
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    TextBox box3 = new TextBox
                                    {
                                        ID = "txt_" + c.CampId,
                                        Width = c.CampAncho
                                    };
                                    box3.Attributes.Add("onfocus", "scrollVisor(" + c.PosX + "," + c.PosY + ")");
                                    RoundedCornersExtender extender5 = new RoundedCornersExtender
                                    {
                                        TargetControlID = "txt_" + c.CampId.ToString(),
                                        Radius = 6,
                                        ID = "RoundedExtender_rtxt" + num.ToString(),
                                        Corners = BoxCorners.All,
                                        Enabled = true
                                    };
                                    MaskedEditExtender extender6 = new MaskedEditExtender
                                    {
                                        MaskType = MaskedEditType.Date,
                                        UserDateFormat = MaskedEditUserDateFormat.MonthDayYear,
                                        ID = "Mext" + num.ToString(),
                                        Mask = "99/99/9999",
                                        TargetControlID = "txt_" + c.CampId.ToString()
                                    };
                                    RangeValidator validator3 = new RangeValidator
                                    {
                                        ID = "rangeV" + c.CampId.ToString(),
                                        Type = ValidationDataType.Date,
                                        MinimumValue = "01/01/1900",
                                        MaximumValue = string.Concat(DateTime.Now.Day.ToString("00"), "/", DateTime.Now.Month.ToString("00"), "/", DateTime.Now.Year),
                                        ControlToValidate = "txt_" + c.CampId,
                                        ErrorMessage = "<B><i>El fomato de fecha es incorrecto</i></B>",
                                        Display = ValidatorDisplay.Dynamic,
                                        ForeColor = Color.Red
                                    };

                                    row.Cells[0].Text = c.CampDescripcion + ": ";
                                    row.Cells[1].Controls.Add(box3);
                                    row.Cells[1].Controls.Add(extender5);
                                    row.Cells[1].Controls.Add(extender6);
                                    row.Cells[1].Controls.Add(validator3);
                                    if (c.CampObligatorio)
                                    {
                                        row.Cells[0].Font.Bold = true;
                                        validator2 = new RequiredFieldValidator
                                        {
                                            ID = "txtReq" + c.CampId.ToString(),
                                            ControlToValidate = "txt_" + c.CampId,
                                            ErrorMessage = "<B><i>Campo requerido</i></B>",
                                            Display = ValidatorDisplay.Dynamic,
                                            SetFocusOnError = true,
                                            ForeColor = Color.Red
                                        };
                                        row.Cells[1].Controls.Add(validator2);
                                    }

                                    /*funcion de validacion personalizada*/
                                    CustomValidator CustomVal_txt;
                                    if (c.ValidationFunction != null)
                                    {
                                        CustomVal_txt = new CustomValidator
                                        {
                                            ID = "customval_" + c.CampId.ToString(),
                                            ControlToValidate = "txt_" + c.CampId,
                                            ClientValidationFunction = c.ValidationFunction,
                                            ErrorMessage = "<B><i>" + c.ErrorMessage + "</i></B>",
                                            SetFocusOnError = true,
                                            Display = ValidatorDisplay.Dynamic,
                                            ForeColor = Color.Red

                                        };
                                        row.Cells[1].Controls.Add(CustomVal_txt);
                                    }

                                    /***********************************************/
                                    if (num == 0)
                                    {
                                        box3.Focus();
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    TextBox box4 = new TextBox
                                    {
                                        ID = "txt_" + c.CampId,
                                        Width = c.CampAncho,
                                        MaxLength = (int)c.LongMax
                                    };
                                    box4.Attributes.Add("onfocus", "scrollVisor(" + c.PosX + "," + c.PosY + ")");
                                    RoundedCornersExtender extender8 = new RoundedCornersExtender
                                    {
                                        TargetControlID = "txt_" + c.CampId.ToString(),
                                        Radius = 6,
                                        ID = "RoundedExtender_rtxt" + num.ToString(),
                                        Corners = BoxCorners.All,
                                        Enabled = true
                                    };
                                    row.Cells[0].Text = c.CampDescripcion + ": ";
                                    row.Cells[1].Controls.Add(box4);
                                    row.Cells[1].Controls.Add(extender8);

                                    /*funcion de validacion personalizada*/
                                    CustomValidator CustomVal_txt;
                                    if (c.ValidationFunction != null)
                                    {
                                        CustomVal_txt = new CustomValidator
                                        {
                                            ID = "customval_" + c.CampId.ToString(),
                                            ControlToValidate = "txt_" + c.CampId,
                                            ErrorMessage = "<B><i>" + c.ErrorMessage + "</i></B>",
                                            SetFocusOnError = true,
                                            ForeColor = Color.Red,
                                            Display = ValidatorDisplay.Dynamic

                                        };
                                        row.Cells[1].Controls.Add(CustomVal_txt);
                                    }

                                    /***********************************************/
                                    if (num == 0)
                                    {
                                        box4.Focus();
                                    }
                                    break;
                                }
                            case 5:
                                {
                                    DropDownList lst = new DropDownList
                                    {
                                        ID = "lst_" + c.CampId.ToString(),
                                        EnableViewState = true
                                    };
                                    lst.Attributes.Add("onfocus", "scrollVisor(" + c.PosX + "," + c.PosY + ")");

                                    RoundedCornersExtender extender9 = new RoundedCornersExtender
                                    {
                                        TargetControlID = "lst_" + c.CampId.ToString(),
                                        Radius = 6,
                                        ID = "RoundedExtender_Lst" + num.ToString(),
                                        Corners = BoxCorners.All,
                                        Enabled = true
                                    };
                                    ListSearchExtender extender10 = new ListSearchExtender
                                    {
                                        ID = "lsExtender" + c.CampId.ToString(),
                                        TargetControlID = "lst_" + c.CampId.ToString(),
                                        QueryPattern = ListSearchQueryPattern.Contains,
                                        PromptText = "Digite el texto a buscar",
                                        PromptPosition = ListSearchPromptPosition.Top
                                    };
                                    Campos camp = new Campos
                                    {
                                        CampId = c.CampId
                                    };
                                    List<CodigosCampo> lstCods = this.bCampos.obtenerCodigosCampo(camp);
                                    this.cargarListasCodigos(lst, lstCods);
                                    row.Cells[0].Text = c.CampDescripcion + ": ";
                                    row.Cells[1].Controls.Add(lst);
                                    row.Cells[1].Controls.Add(extender9);
                                    row.Cells[1].Controls.Add(extender10);
                                    if (c.CampObligatorio)
                                    {
                                        row.Cells[0].Font.Bold = true;
                                        validator2 = new RequiredFieldValidator
                                        {
                                            ID = "lstReq" + c.CampId.ToString(),
                                            ControlToValidate = "lst_" + c.CampId.ToString(),
                                            ErrorMessage = "<B><i>Seleccione una opcion</i></B>",
                                            Display = ValidatorDisplay.Dynamic,
                                            SetFocusOnError = true,
                                            ForeColor = Color.Red,
                                            InitialValue = "-1"
                                        };
                                        row.Cells[1].Controls.Add(validator2);
                                    }

                                    /*funcion de validacion personalizada*/
                                    CustomValidator CustomVal_txt;
                                    if (c.ValidationFunction != null)
                                    {
                                        CustomVal_txt = new CustomValidator
                                        {
                                            ID = "customval_" + c.CampId.ToString(),
                                            ControlToValidate = "lst_" + c.CampId.ToString(),
                                            ClientValidationFunction = c.ValidationFunction,
                                            ErrorMessage = "<B><i>" + c.ErrorMessage + "</i></B>",
                                            SetFocusOnError = true,
                                            Display = ValidatorDisplay.Dynamic,
                                            ForeColor = Color.Red

                                        };
                                        row.Cells[1].Controls.Add(CustomVal_txt);
                                    }

                                    /***********************************************/
                                    if (num == 0)
                                    {
                                        lst.Focus();
                                    }
                                    break;
                                }
                            case 6:
                                {
                                    DropDownList list3 = new DropDownList
                                    {
                                        EnableViewState = true,
                                        ID = "lst_" + c.CampId.ToString() + "_" + c.CampDependiente.ToString(),
                                        AutoPostBack = true
                                    };
                                    list3.Attributes.Add("onfocus", "scrollVisor(" + c.PosX + "," + c.PosY + ")");
                                    RoundedCornersExtender extender11 = new RoundedCornersExtender
                                    {
                                        TargetControlID = "lst_" + c.CampId.ToString() + "_" + c.CampDependiente.ToString(),
                                        Radius = 6,
                                        ID = "RoundedExtender_Lst" + num.ToString(),
                                        Corners = BoxCorners.All,
                                        Enabled = true
                                    };
                                    ListSearchExtender extender12 = new ListSearchExtender
                                    {
                                        ID = "lsExtender" + c.CampId.ToString(),
                                        TargetControlID = "lst_" + c.CampId.ToString() + "_" + c.CampDependiente.ToString(),
                                        QueryPattern = ListSearchQueryPattern.Contains,
                                        PromptText = "Digite el texto a buscar",
                                        PromptPosition = ListSearchPromptPosition.Top
                                    };
                                    List<P_Departamentos> list4 = this.bCampos.obtenerDepartamentos();
                                    this.cargarDepartamentos(list3, list4);
                                    row.Cells[0].Text = c.CampDescripcion + ": ";
                                    if (c.CampDependiente > 0)
                                    {
                                        if (handler == null)
                                        {
                                            handler = (sender, args) => this.cargarCiudades(sender, args, c.CampDependiente.ToString());
                                        }
                                        list3.SelectedIndexChanged += handler;
                                    }
                                    row.Cells[1].Controls.Add(list3);
                                    row.Cells[1].Controls.Add(extender11);
                                    row.Cells[1].Controls.Add(extender12);
                                    if (c.CampObligatorio)
                                    {
                                        row.Cells[0].Font.Bold = true;

                                        validator2 = new RequiredFieldValidator
                                        {
                                            ID = "lstReq" + c.CampId.ToString(),
                                            ControlToValidate = "lst_" + c.CampId.ToString() + "_" + c.CampDependiente.ToString(),
                                            ErrorMessage = "<B><i>Seleccione una opcion</i></B>",
                                            Display = ValidatorDisplay.Dynamic,
                                            ForeColor = Color.Red,
                                            SetFocusOnError = true,
                                            InitialValue = "-1"
                                        };
                                        row.Cells[1].Controls.Add(validator2);
                                    }

                                    /*funcion de validacion personalizada*/
                                    CustomValidator CustomVal_txt;
                                    if (c.ValidationFunction != null)
                                    {
                                        CustomVal_txt = new CustomValidator
                                        {
                                            ID = "customval_" + c.CampId.ToString(),
                                            ControlToValidate = "lst_" + c.CampId.ToString() + "_" + c.CampDependiente.ToString(),
                                            ErrorMessage = "<B><i>" + c.ErrorMessage + "</i></B>",
                                            SetFocusOnError = true,
                                            Display = ValidatorDisplay.Dynamic,
                                            ForeColor = Color.Red
                                        };
                                        row.Cells[1].Controls.Add(CustomVal_txt);
                                    }

                                    /***********************************************/
                                    if (num == 0)
                                    {
                                        list3.Focus();
                                    }
                                    break;
                                }
                            case 7:
                                {
                                    DropDownList list5 = new DropDownList
                                    {
                                        EnableViewState = true,
                                        ID = "lstCiu_" + c.CampId.ToString()
                                    };

                                    list5.Attributes.Add("onfocus", "scrollVisor(" + c.PosX + "," + c.PosY + ")");
                                    RoundedCornersExtender extender13 = new RoundedCornersExtender
                                    {
                                        TargetControlID = "lstCiu_" + c.CampId.ToString(),
                                        Radius = 6,
                                        ID = "RoundedExtender_Lst" + num.ToString(),
                                        Corners = BoxCorners.All,
                                        Enabled = true
                                    };

                                    ListSearchExtender extender14 = new ListSearchExtender
                                    {
                                        ID = "lsExtender" + c.CampId.ToString(),
                                        TargetControlID = "lstCiu_" + c.CampId.ToString(),
                                        QueryPattern = ListSearchQueryPattern.Contains,
                                        PromptText = "Digite el texto a buscar",
                                        PromptPosition = ListSearchPromptPosition.Top
                                    };

                                    P_Departamentos d = new P_Departamentos
                                    {
                                        DeptId = 5
                                    };
                                    List<P_Ciudad> list6 = this.bCampos.obtenerCiudades(d);
                                    row.Cells[0].Text = c.CampDescripcion + ": ";

                                    row.Cells[1].Controls.Add(list5);
                                    row.Cells[1].Controls.Add(extender13);
                                    row.Cells[1].Controls.Add(extender14);
                                    if (c.CampObligatorio)
                                    {
                                        row.Cells[0].Font.Bold = true;

                                        validator2 = new RequiredFieldValidator
                                        {
                                            ID = "lstReq" + c.CampId.ToString(),
                                            ControlToValidate = "lstCiu_" + c.CampId.ToString(),
                                            ErrorMessage = "<B><i>Seleccione una opcion</i></B>",
                                            Display = ValidatorDisplay.Dynamic,
                                            ForeColor = Color.Red,
                                            SetFocusOnError = true,
                                            InitialValue = "-1"
                                        };
                                        row.Cells[1].Controls.Add(validator2);
                                    }

                                    /*funcion de validacion personalizada*/
                                    CustomValidator CustomVal_txt;
                                    if (c.ValidationFunction != null)
                                    {
                                        CustomVal_txt = new CustomValidator
                                        {
                                            ID = "customval_" + c.CampId.ToString(),
                                            ControlToValidate = "lstCiu_" + c.CampId.ToString(),
                                            ClientValidationFunction = c.ValidationFunction,
                                            ErrorMessage = "<B><i>" + c.ErrorMessage + "</i></B>",
                                            SetFocusOnError = true,
                                            Display = ValidatorDisplay.Dynamic,
                                            ForeColor = Color.Red
                                        };
                                        row.Cells[1].Controls.Add(CustomVal_txt);
                                    }
                                    /***********************************************/
                                    if (num == 0)
                                    {
                                        list5.Focus();
                                    }
                                    break;
                                }

                            case 11:
                                {
                                    CheckBoxList chkLst = new CheckBoxList
                                    {
                                        EnableViewState = true,
                                        ID = "chkLst_" + c.CampId.ToString()
                                    };

                                    RoundedCornersExtender extenderchklst = new RoundedCornersExtender
                                    {
                                        TargetControlID = "chkLst_" + c.CampId.ToString(),
                                        Radius = 6,
                                        ID = "RoundedExtender_chkLst" + num.ToString(),
                                        Corners = BoxCorners.All,
                                        Enabled = true
                                    };

                                    Campos CampChk = new Campos
                                    {
                                        CampId = c.CampId
                                    };

                                    List<CodigosCampo> lstCodsChk = this.bCampos.obtenerCodigosCampoChk(CampChk);
                                    this.cargarListasCodigos(chkLst, lstCodsChk);
                                    /*Teniendo en cuenta que ecxiste un bug conel contro checkbox 
                                    y no se le puede asignar el evento onfocus
                                    se debe asiganar usando jquery*/

                                    foreach (ListItem i in chkLst.Items)
                                    {
                                        i.Value = i.Value + "_" + c.PosX + "_" + c.PosY;
                                    }

                                    /*funcion de validacion personalizada*/
                                    CustomValidator CustomVal_txt;
                                    if (c.ValidationFunction != null)
                                    {
                                        CustomVal_txt = new CustomValidator
                                        {
                                            ID = "customval_" + c.CampId.ToString(),
                                            ControlToValidate = "chkLst_" + c.CampId.ToString(),
                                            ErrorMessage = "<B><i>" + c.ErrorMessage + "</i></B>",
                                            SetFocusOnError = true,
                                            ForeColor = Color.Red,
                                            Display = ValidatorDisplay.Dynamic

                                        };
                                        row.Cells[1].Controls.Add(CustomVal_txt);
                                    }

                                    /***********************************************/
                                    row.Cells[0].Text = c.CampDescripcion + ": ";
                                    row.Cells[1].Controls.Add(chkLst);
                                    row.Cells[1].Controls.Add(extenderchklst);

                                    if (num == 0)
                                    {
                                        chkLst.Focus();
                                    }
                                    break;
                                }

                            case 13:
                                #region Grilla Control de Calidad Posiciones
                                //Case 13 significa la Grilla de las posiciones.
                                //Habra que especificar que tipo de documentos viene para determinar que campos y que grilla se pintara.
                                //Obtenemos el valor del Documento actualmente en proceso
                                int idDocumento = Convert.ToInt32(this.lstDocumentos.SelectedValue);

                                int idEtapaPos = 0;
                                bool valorCCalidadPos = false;
                                if (Session["tId_Etapa"] != null)
                                {
                                    idEtapaPos = Convert.ToInt32(Session["tId_Etapa"].ToString());
                                    valorCCalidadPos = Convert.ToBoolean(lstCampos[num].ControlCalidad);
                                }

                                //Validar si esta en control de calidad o en alguna de las capturas.
                                if (valorCCalidadPos && idEtapaPos == 3)
                                {
                                    int idGruposDocumentosPos = Convert.ToInt32(lstCampos[num].GDocId);
                                    int negId = (int)((Captura)this.Session["NEGOCIO"]).NegId;

                                    // Se crean las listas de captura 1 y captura 2
                                    List<Captura> listaCapturaUnoPos = s.obtenerCamposCCalidad(1, idGruposDocumentosPos, negId);
                                    List<Captura> listaCapturaDosPos = s.obtenerCamposCCalidad(2, idGruposDocumentosPos, negId);

                                    int totalCapturaUnoPos = listaCapturaUnoPos.Count;
                                    int totalCapturaDosPos = listaCapturaDosPos.Count;

                                    //Validamos cual de las dos capturas tiene mas valores
                                    if (totalCapturaUnoPos > totalCapturaDosPos)
                                    {
                                        int diferencia = totalCapturaUnoPos - totalCapturaDosPos;
                                        int contador = totalCapturaDosPos;
                                        for (int i = 0; i < diferencia; i++)
                                        {
                                            //Poblar la captura dos con las filas faltantes
                                            Captura datos = new Captura()
                                            {
                                                NegId = listaCapturaUnoPos[contador].NegId,
                                                NumCaptura = listaCapturaUnoPos[contador].NumCaptura,
                                                CampId = listaCapturaUnoPos[contador].CampId,
                                                Indice = listaCapturaUnoPos[contador].Indice,
                                                NegValor = "No hay datos",
                                                Usuario = listaCapturaUnoPos[contador].Usuario,
                                                FechaRegistro = listaCapturaUnoPos[contador].FechaRegistro,
                                                DocId = listaCapturaUnoPos[contador].DocId
                                            };

                                            listaCapturaDosPos.Add(datos);
                                            contador++;
                                        }
                                    }
                                    else if (totalCapturaDosPos > totalCapturaUnoPos)
                                    {
                                        int diferencia = totalCapturaDosPos - totalCapturaUnoPos;
                                        int contador = totalCapturaUnoPos;
                                        for (int i = 0; i < diferencia; i++)
                                        {
                                            //Poblar la captura dos con las filas faltantes
                                            Captura datos = new Captura()
                                            {
                                                NegId = listaCapturaDosPos[contador].NegId,
                                                NumCaptura = listaCapturaDosPos[contador].NumCaptura,
                                                CampId = listaCapturaDosPos[contador].CampId,
                                                Indice = listaCapturaDosPos[contador].Indice,
                                                NegValor = "No hay datos",
                                                Usuario = listaCapturaDosPos[contador].Usuario,
                                                FechaRegistro = listaCapturaDosPos[contador].FechaRegistro,
                                                DocId = listaCapturaDosPos[contador].DocId
                                            };

                                            listaCapturaUnoPos.Add(datos);
                                            contador++;
                                        }
                                    }

                                    List<Captura> listaCamposCapturadosPos = new List<Captura>();

                                    int contadorFilasPos = 1;
                                    bool nuevaFilaPos = true;

                                    pControls.Controls.Add(new LiteralControl("<h3>&nbspGrilla Control de Calidad - Posiciones</h3>"));
                                    pControls.Controls.Add(new LiteralControl("<table data-work='idGrillaControlCaldImp'>"));
                                    pControls.Controls.Add(new LiteralControl("<tr>"));

                                    int contadorCheck = 1;
                                    for (int i = 0; i < listaCapturaUnoPos.Count; i++)
                                    {
                                        if (listaCapturaUnoPos[i].Indice == listaCapturaDosPos[i].Indice)
                                        {
                                            if (listaCapturaUnoPos[i].CampId == listaCapturaDosPos[i].CampId)
                                            {
                                                if (listaCapturaUnoPos[i].NegValor != listaCapturaDosPos[i].NegValor && listaCapturaUnoPos[i].NegValor != " " && listaCapturaDosPos[i].NegValor != " ")
                                                {
                                                    //Pintar los campos que tienen valores diferentes con el Tooltip
                                                    //Extraer el titulo de los campos y sus diferencias en el tooltip
                                                    if (contadorFilasPos >= 6)
                                                    {
                                                        contadorFilasPos = 1;
                                                        pControls.Controls.Add(new LiteralControl("</td></tr>"));
                                                        if (nuevaFilaPos)
                                                        {
                                                            pControls.Controls.Add(new LiteralControl("<tr>"));
                                                            nuevaFilaPos = false;
                                                        }

                                                        string nombreCampo = s.obtenerNomCampo(listaCapturaUnoPos[i].CampId);

                                                        pControls.Controls.Add(new LiteralControl("<td>"));
                                                        pControls.Controls.Add(new LiteralControl("&nbsp <b>Fila: " + listaCapturaUnoPos[i].Indice + " </b><br>"));
                                                        pControls.Controls.Add(new LiteralControl("&nbsp " + nombreCampo + " <br>"));

                                                        WebServiceModel ws = new WebServiceModel();

                                                        //****************************************************************************
                                                        int _idCampo = listaCapturaUnoPos[i].CampId;

                                                        int _tipoCampo = ws.obtenerTipoCampo(_idCampo);
                                                        int longMax = ws.obtenerLongitudCampo(listaCapturaUnoPos[i].CampId);

                                                        bool esLista = ws.validaLista(listaCapturaUnoPos[i].CampId);
                                                        bool esListaCDos = ws.validaLista(listaCapturaDosPos[i].CampId);

                                                        //Validamos el tipo de campo a pintar.
                                                        switch (_tipoCampo)
                                                        {
                                                            //Tipo de campo Númerico
                                                            case 1:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" required maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Alfanúmerico
                                                            case 2:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" required maxlength=" + longMax + " runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Fecha
                                                            case 3:
                                                                pControls.Controls.Add(new LiteralControl("<input runat='server' required name=\"CapturaTres\" class=\"dp msk valFecha\" onkeypress=\"return bloqTeclado(event);\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Decimal
                                                            case 16:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" class=\"dec\" maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" required runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Check
                                                            case 11:
                                                                pControls.Controls.Add(new LiteralControl("<td><input id=\"ck" + contadorCheck + "\" name=\"ck" + contadorCheck + "\" onclick='asignarValorCheck(" + contadorCheck + ");' runat='server' type=\"checkbox\" value=\"1\" ></td>"));
                                                                pControls.Controls.Add(new LiteralControl("<td><input id=\"CapturaTres" + contadorCheck + "\" name=\"CapturaTres\" runat='server' value=\"0\" type=\"hidden\" ></td>"));
                                                                contadorCheck++;
                                                                break;

                                                            //Tipo de campo Lista desplegable
                                                            case 5:
                                                                List<CodigosCampo> ListaCodigosCampo = ws.consultarOpcionesLista(listaCapturaUnoPos[i].CampId);
                                                                pControls.Controls.Add(new LiteralControl("<select name=\"CapturaTres\" id=\"CapturaTres\" required>"));
                                                                foreach (CodigosCampo campos in ListaCodigosCampo)
                                                                {
                                                                    pControls.Controls.Add(new LiteralControl("<Option value =" + campos.CodId + ">" + campos.CodDescripcion + "</Option>"));
                                                                }
                                                                pControls.Controls.Add(new LiteralControl("</select>"));
                                                                break;
                                                        }
                                                        //****************************************************************************

                                                        if (esLista || esListaCDos)
                                                        {
                                                            int campid_UNO = listaCapturaUnoPos[i].CampId;
                                                            string codid_UNO = listaCapturaUnoPos[i].NegValor;

                                                            int campid_DOS = listaCapturaDosPos[i].CampId;
                                                            string codid_DOS = listaCapturaDosPos[i].NegValor;

                                                            string valorUNO = ws.obtenerValorLista(campid_UNO, codid_UNO);
                                                            string valorDOS = ws.obtenerValorLista(campid_DOS, codid_DOS);

                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Blue\">&nbsp Captura UNO: " + valorUNO + "</div>"));
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Red\">&nbsp Captura DOS: " + valorDOS + "</div><br>"));
                                                        }
                                                        else
                                                        {
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Blue\">&nbsp Captura UNO: " + listaCapturaUnoPos[i].NegValor + "</div>"));
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Red\">&nbsp Captura DOS: " + listaCapturaDosPos[i].NegValor + "</div><br>"));
                                                        }

                                                        pControls.Controls.Add(new LiteralControl("</td>"));
                                                    }
                                                    else
                                                    {
                                                        string nombreCampo = s.obtenerNomCampo(listaCapturaUnoPos[i].CampId);

                                                        pControls.Controls.Add(new LiteralControl("<td>"));
                                                        pControls.Controls.Add(new LiteralControl("&nbsp <b>Fila: " + listaCapturaUnoPos[i].Indice + " </b><br>"));
                                                        pControls.Controls.Add(new LiteralControl("&nbsp " + nombreCampo + " <br>"));

                                                        WebServiceModel ws = new WebServiceModel();

                                                        //****************************************************************************
                                                        int _idCampo = listaCapturaUnoPos[i].CampId;

                                                        int _tipoCampo = ws.obtenerTipoCampo(_idCampo);
                                                        int longMax = ws.obtenerLongitudCampo(listaCapturaUnoPos[i].CampId);

                                                        bool esLista = ws.validaLista(listaCapturaUnoPos[i].CampId);
                                                        bool esListaCDos = ws.validaLista(listaCapturaDosPos[i].CampId);

                                                        //Validamos el tipo de campo a pintar.
                                                        switch (_tipoCampo)
                                                        {
                                                            //Tipo de campo Númerico
                                                            case 1:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" required maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Alfanúmerico
                                                            case 2:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" required maxlength=" + longMax + " runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Fecha
                                                            case 3:
                                                                pControls.Controls.Add(new LiteralControl("<input runat='server' required name=\"CapturaTres\" class=\"dp msk valFecha\" onkeypress=\"return bloqTeclado(event);\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Decimal
                                                            case 16:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" class=\"dec\" maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" required runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Check
                                                            case 11:
                                                                pControls.Controls.Add(new LiteralControl("<td><input id=\"ck" + contadorCheck + "\" name=\"ck" + contadorCheck + "\" onclick='asignarValorCheck(" + contadorCheck + ");' runat='server' type=\"checkbox\" value=\"1\" ></td>"));
                                                                pControls.Controls.Add(new LiteralControl("<td><input id=\"CapturaTres" + contadorCheck + "\" name=\"CapturaTres\" runat='server' value=\"0\" type=\"hidden\" ></td>"));
                                                                contadorCheck++;
                                                                break;

                                                            //Tipo de campo Lista desplegable
                                                            case 5:
                                                                List<CodigosCampo> ListaCodigosCampo = ws.consultarOpcionesLista(listaCapturaUnoPos[i].CampId);
                                                                pControls.Controls.Add(new LiteralControl("<select name=\"CapturaTres\" id=\"CapturaTres\" required>"));
                                                                foreach (CodigosCampo campos in ListaCodigosCampo)
                                                                {
                                                                    pControls.Controls.Add(new LiteralControl("<Option value =" + campos.CodId + ">" + campos.CodDescripcion + "</Option>"));
                                                                }
                                                                pControls.Controls.Add(new LiteralControl("</select>"));
                                                                break;
                                                        }
                                                        //****************************************************************************

                                                        if (esLista || esListaCDos)
                                                        {
                                                            int campid_UNO = listaCapturaUnoPos[i].CampId;
                                                            string codid_UNO = listaCapturaUnoPos[i].NegValor;

                                                            int campid_DOS = listaCapturaDosPos[i].CampId;
                                                            string codid_DOS = listaCapturaDosPos[i].NegValor;

                                                            string valorUNO = ws.obtenerValorLista(campid_UNO, codid_UNO);
                                                            string valorDOS = ws.obtenerValorLista(campid_DOS, codid_DOS);

                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Blue\">&nbsp Captura UNO: " + valorUNO + "</div>"));
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Red\">&nbsp Captura DOS: " + valorDOS + "</div><br>"));
                                                        }
                                                        else
                                                        {
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Blue\">&nbsp Captura UNO: " + listaCapturaUnoPos[i].NegValor + "</div>"));
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Red\">&nbsp Captura DOS: " + listaCapturaDosPos[i].NegValor + "</div><br>"));
                                                        }

                                                        pControls.Controls.Add(new LiteralControl("</td>"));
                                                    }

                                                    Captura datos = new Captura
                                                    {
                                                        NegId = listaCapturaUnoPos[i].NegId,
                                                        NumCaptura = 3,
                                                        CampId = listaCapturaUnoPos[i].CampId,
                                                        Indice = listaCapturaUnoPos[i].Indice,
                                                        Usuario = listaCapturaUnoPos[i].Usuario,
                                                        FechaRegistro = DateTime.Now,
                                                        DocId = listaCapturaUnoPos[i].DocId
                                                    };

                                                    listaCamposCapturadosPos.Add(datos);

                                                    contadorFilasPos++;
                                                }
                                            }
                                        }
                                    }
                                    pControls.Controls.Add(new LiteralControl("</td></tr><table>"));
                                    Session["CamposCCalidad"] = listaCamposCapturadosPos;
                                    break;
                                }
                                #endregion
                                #region Grilla Capturas 1 y 2 Posiciones
                                else
                                {
                                    int valorGrillaPos = lstCampos[num].CampId;
                                    Session["Id_Grilla"] = valorGrillaPos;
                                    int idDocumentoPos = Convert.ToInt32(this.lstDocumentos.SelectedValue);
                                    int idCaptura = Convert.ToInt32(base.Request.QueryString["CAPTURA"]);

                                    //Obteniendo la lista de campos asociados al Id de la grilla seleccionada
                                    List<Campos> listaCampos = s.obtenerCamposGrilla(valorGrillaPos, idDocumentoPos);
                                    indiceGrid.grillaPosiciones = valorGrillaPos;

                                    //Pintar los titulos de la grilla
                                    pControls.Controls.Add(new LiteralControl("<h3>&nbsp Ingresar Posiciones</h3>"));
                                    pControls.Controls.Add(new LiteralControl("<table data-work='idGrilla' id='" + lstCampos[num].CampDescripcion + "'>"));
                                    pControls.Controls.Add(new LiteralControl("<tr>"));
                                    pControls.Controls.Add(new LiteralControl("<td></td>"));

                                    foreach (Campos campo in listaCampos)
                                    {
                                        pControls.Controls.Add(new LiteralControl("<td>" + campo.CampDescripcion.ToString() + "</td>"));
                                    }

                                    //Se crea la fila en donde van a ir pintados los campos que se crearan desde JavaScript
                                    pControls.Controls.Add(new LiteralControl("</tr>"));
                                    pControls.Controls.Add(new LiteralControl("<tr>"));

                                    pControls.Controls.Add(new LiteralControl("</tr>"));
                                    pControls.Controls.Add(new LiteralControl("</table>"));

                                    //Para llenar los nombres de los campos
                                    string nombreCampos = "";

                                    string disponible = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (nombreCampos.Trim() == "")
                                        {
                                            nombreCampos = campo.NomBizagi;

                                            //Validamos si estamos en captura Uno o Captura Dos
                                            //En Captura 2 Es cuando comenzamos a deshabilitar
                                            if (idCaptura == 2)
                                            {
                                                if (campo.CampNumCaptura2 == true)
                                                {
                                                    disponible = "1";
                                                }
                                                else
                                                {
                                                    disponible = "0";
                                                }
                                                //disponible = campo.CampNumCaptura2.ToString();
                                            }

                                            if (idCaptura == 1)
                                            {
                                                disponible = campo.CampNumCaptura.ToString();
                                            }
                                        }
                                        else
                                        {
                                            nombreCampos = nombreCampos + "," + campo.NomBizagi;

                                            //Validamos si estamos en captura Uno o Captura Dos
                                            //En Captura 2 Es cuando comenzamos a deshabilitar
                                            if (idCaptura == 2)
                                            {
                                                if (campo.CampNumCaptura2 == true)
                                                {
                                                    disponible = disponible + "," + "1";
                                                }
                                                else
                                                {
                                                    disponible = disponible + "," + "0";
                                                }
                                                //disponible = disponible + "," + campo.CampNumCaptura2.ToString();
                                            }

                                            if (idCaptura == 1)
                                            {
                                                disponible = disponible + "," + campo.CampNumCaptura.ToString();
                                            }
                                        }
                                    }

                                    //Para llenar los tipos de campos a pintar desde Javascript
                                    string tiposCampo = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (tiposCampo.Trim() == "")
                                        {
                                            tiposCampo = campo.TcId.ToString();
                                        }
                                        else
                                        {
                                            tiposCampo = tiposCampo + "," + campo.TcId.ToString();
                                        }
                                    }

                                    //Para concatenar todas las opciones de los campos que sean una lista desplegable
                                    string idCampos = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (idCampos.Trim() == "")
                                        {
                                            idCampos = campo.CampId.ToString();
                                        }
                                        else
                                        {
                                            idCampos = idCampos + "," + campo.CampId.ToString();
                                        }
                                    }

                                    //Para Crear las opciones de Maxima longitud
                                    string maxLongitud = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (maxLongitud.Trim() == "")
                                        {
                                            maxLongitud = campo.LongMax.ToString();
                                        }
                                        else
                                        {
                                            maxLongitud = maxLongitud + "," + campo.LongMax.ToString();
                                        }
                                    }

                                    //Para especificar si los campos son obligatorios o no.
                                    string campoObligatorio = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (campoObligatorio.Trim() == "")
                                        {
                                            campoObligatorio = campo.CampObligatorio.ToString();
                                        }
                                        else
                                        {
                                            campoObligatorio = campoObligatorio + "," + campo.CampObligatorio.ToString();
                                        }
                                    }

                                    //Crear el boton de agregar mas campos y se les envia cuantos campos son los nombres de los campos y el tipo de campo que se va a pintar.
                                    pControls.Controls.Add(new LiteralControl("<input type=\"button\" class=\"btn btn-login\" id=\"Crear\" value=\"Nuevo Registro\" onclick=\"agregarFila(" + listaCampos.Count + ",'" + nombreCampos + "','" + tiposCampo + "','" + lstCampos[num].CampDescripcion + "','" + disponible + "','" + idCampos + "','" + maxLongitud + "','" + campoObligatorio + "')\" /></br>"));
                                }
                                #endregion
                                break;

                            case 14:
                                #region Grilla Control Calidad Impuestos
                                int idEtapa = 0;
                                bool valorCCalidad = false;
                                if (Session["tId_Etapa"] != null)
                                {
                                    idEtapa = Convert.ToInt32(Session["tId_Etapa"].ToString());
                                    valorCCalidad = Convert.ToBoolean(lstCampos[num].ControlCalidad);
                                }

                                if (valorCCalidad && idEtapa == 3)
                                {
                                    int idGruposDocumentos = Convert.ToInt32(lstCampos[num].GDocId);
                                    int negId = (int)((Captura)this.Session["NEGOCIO"]).NegId;

                                    // Se crean las listas de captura 1 y captura 2
                                    List<Captura> listaCapturaUno = s.obtenerCamposCCalidad(1, idGruposDocumentos, negId);
                                    List<Captura> listaCapturaDos = s.obtenerCamposCCalidad(2, idGruposDocumentos, negId);

                                    int totalCapturaUnoPos = listaCapturaUno.Count;
                                    int totalCapturaDosPos = listaCapturaDos.Count;

                                    //Validamos cual de las dos capturas tiene mas valores
                                    if (totalCapturaUnoPos > totalCapturaDosPos)
                                    {
                                        int diferencia = totalCapturaUnoPos - totalCapturaDosPos;
                                        int contador = totalCapturaDosPos;
                                        for (int i = 0; i < diferencia; i++)
                                        {
                                            //Poblar la captura dos con las filas faltantes
                                            Captura datos = new Captura()
                                            {
                                                NegId = listaCapturaUno[contador].NegId,
                                                NumCaptura = listaCapturaUno[contador].NumCaptura,
                                                CampId = listaCapturaUno[contador].CampId,
                                                Indice = listaCapturaUno[contador].Indice,
                                                NegValor = "No hay datos",
                                                Usuario = listaCapturaUno[contador].Usuario,
                                                FechaRegistro = listaCapturaUno[contador].FechaRegistro,
                                                DocId = listaCapturaUno[contador].DocId
                                            };

                                            listaCapturaDos.Add(datos);
                                            contador++;
                                        }
                                    }
                                    else if (totalCapturaDosPos > totalCapturaUnoPos)
                                    {
                                        int diferencia = totalCapturaDosPos - totalCapturaUnoPos;
                                        int contador = totalCapturaUnoPos;
                                        for (int i = 0; i < diferencia; i++)
                                        {
                                            //Poblar la captura dos con las filas faltantes
                                            Captura datos = new Captura()
                                            {
                                                NegId = listaCapturaDos[contador].NegId,
                                                NumCaptura = listaCapturaDos[contador].NumCaptura,
                                                CampId = listaCapturaDos[contador].CampId,
                                                Indice = listaCapturaDos[contador].Indice,
                                                NegValor = "No hay datos",
                                                Usuario = listaCapturaDos[contador].Usuario,
                                                FechaRegistro = listaCapturaDos[contador].FechaRegistro,
                                                DocId = listaCapturaDos[contador].DocId
                                            };

                                            listaCapturaUno.Add(datos);
                                            contador++;
                                        }
                                    }

                                    List<Captura> listaCamposCapturados = new List<Captura>();

                                    int contadorFilas = 1;
                                    bool nuevaFila = true;

                                    pControls.Controls.Add(new LiteralControl("<h3>&nbspGrilla Control de Calidad - Impuestos</h3>"));
                                    pControls.Controls.Add(new LiteralControl("<table data-work='idGrillaControlCaldImp'>"));
                                    pControls.Controls.Add(new LiteralControl("<tr>"));

                                    int contadorCheck = 1;
                                    for (int i = 0; i < listaCapturaUno.Count; i++)
                                    {
                                        if (listaCapturaUno[i].Indice == listaCapturaDos[i].Indice)
                                        {
                                            if (listaCapturaUno[i].CampId == listaCapturaDos[i].CampId)
                                            {
                                                if (listaCapturaUno[i].NegValor != listaCapturaDos[i].NegValor && listaCapturaDos[i].NegValor.Trim() != "")
                                                {
                                                    //Pintar los campos que tienen valores diferentes con el Tooltip
                                                    //Extraer el titulo de los campos y sus diferencias en el tooltip
                                                    if (contadorFilas >= 6)
                                                    {
                                                        contadorFilas = 1;
                                                        pControls.Controls.Add(new LiteralControl("</td></tr>"));
                                                        if (nuevaFila)
                                                        {
                                                            pControls.Controls.Add(new LiteralControl("<tr>"));
                                                            nuevaFila = false;
                                                        }

                                                        string nombreCampo = s.obtenerNomCampo(listaCapturaUno[i].CampId);

                                                        pControls.Controls.Add(new LiteralControl("<td>"));
                                                        pControls.Controls.Add(new LiteralControl("&nbsp <b>Fila: " + listaCapturaUno[i].Indice + " </b><br>"));
                                                        pControls.Controls.Add(new LiteralControl("&nbsp " + nombreCampo + " <br>"));

                                                        //****************************************************************************
                                                        WebServiceModel ws = new WebServiceModel();

                                                        int _campID = listaCapturaUno[i].CampId;

                                                        int _tcID = ws.obtenerTipoCampo(_campID);
                                                        int longMax = ws.obtenerLongitudCampo(listaCapturaUno[i].CampId);

                                                        bool esLista = ws.validaLista(listaCapturaUno[i].CampId);
                                                        bool esListaCDos = ws.validaLista(listaCapturaDos[i].CampId);

                                                        switch (_tcID)
                                                        {
                                                            //Tipo de campo Númerico
                                                            case 1:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" required maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Alfanúmerico
                                                            case 2:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" required maxlength=" + longMax + " runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Fecha
                                                            case 3:
                                                                pControls.Controls.Add(new LiteralControl("<input runat='server' required name=\"CapturaTres\" class=\"dp msk valFecha\" onkeypress=\"return bloqTeclado(event);\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Decimal
                                                            case 16:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" class=\"dec\" maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" required runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Check
                                                            case 11:
                                                                pControls.Controls.Add(new LiteralControl("<td><input id=\"ck" + contadorCheck + "\" name=\"ck" + contadorCheck + "\" onclick='asignarValorCheck(" + contadorCheck + ");' runat='server' type=\"checkbox\" value=\"1\" ></td>"));
                                                                pControls.Controls.Add(new LiteralControl("<td><input id=\"CapturaTres" + contadorCheck + "\" name=\"CapturaTres\" runat='server' value=\"0\" type=\"hidden\" ></td>"));
                                                                contadorCheck++;
                                                                break;

                                                            //Tipo de campo Lista Desplegable
                                                            case 5:
                                                                List<CodigosCampo> ListaCodigosCampo = ws.consultarOpcionesLista(listaCapturaUno[i].CampId);
                                                                pControls.Controls.Add(new LiteralControl("<select name=\"CapturaTres\" id=\"CapturaTres\" required>"));
                                                                foreach (CodigosCampo campos in ListaCodigosCampo)
                                                                {
                                                                    pControls.Controls.Add(new LiteralControl("<Option value =" + campos.CodId + ">" + campos.CodDescripcion + "</Option>"));
                                                                }
                                                                pControls.Controls.Add(new LiteralControl("</select>"));
                                                                break;
                                                        }
                                                        //****************************************************************************

                                                        if (esLista || esListaCDos)
                                                        {
                                                            int campid_UNO = listaCapturaUno[i].CampId;
                                                            string codid_UNO = listaCapturaUno[i].NegValor;

                                                            int campid_DOS = listaCapturaDos[i].CampId;
                                                            string codid_DOS = listaCapturaDos[i].NegValor;

                                                            string valorUNO = ws.obtenerValorLista(campid_UNO, codid_UNO);
                                                            string valorDOS = ws.obtenerValorLista(campid_DOS, codid_DOS);

                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Blue\">&nbsp Captura UNO: " + valorUNO + "</div>"));
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Red\">&nbsp Captura DOS: " + valorDOS + "</div><br>"));
                                                        }
                                                        else
                                                        {
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Blue\">&nbsp Captura UNO: " + listaCapturaUno[i].NegValor + "</div>"));
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Red\">&nbsp Captura DOS: " + listaCapturaDos[i].NegValor + "</div><br>"));
                                                        }

                                                        pControls.Controls.Add(new LiteralControl("</td>"));
                                                    }
                                                    else
                                                    {
                                                        string nombreCampo = s.obtenerNomCampo(listaCapturaUno[i].CampId);

                                                        pControls.Controls.Add(new LiteralControl("<td>"));
                                                        pControls.Controls.Add(new LiteralControl("&nbsp <b>Fila: " + listaCapturaUno[i].Indice + " </b><br>"));
                                                        pControls.Controls.Add(new LiteralControl("&nbsp " + nombreCampo + " <br>"));

                                                        //****************************************************************************
                                                        WebServiceModel ws = new WebServiceModel();

                                                        int _campID = listaCapturaUno[i].CampId;

                                                        int _tcID = ws.obtenerTipoCampo(_campID);
                                                        int longMax = ws.obtenerLongitudCampo(listaCapturaUno[i].CampId);

                                                        bool esLista = ws.validaLista(listaCapturaUno[i].CampId);
                                                        bool esListaCDos = ws.validaLista(listaCapturaDos[i].CampId);

                                                        switch (_tcID)
                                                        {
                                                            //Tipo de campo Númerico
                                                            case 1:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" required maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Alfanúmerico
                                                            case 2:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" required maxlength=" + longMax + " runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Fecha
                                                            case 3:
                                                                pControls.Controls.Add(new LiteralControl("<input runat='server' required name=\"CapturaTres\" class=\"dp msk valFecha\" onkeypress=\"return bloqTeclado(event);\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Decimal
                                                            case 16:
                                                                pControls.Controls.Add(new LiteralControl("<input id=\"CapturaTres\" class=\"dec\" maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" required runat='server' name=\"CapturaTres\" type=\"text\" /><br>"));
                                                                break;

                                                            //Tipo de campo Check
                                                            case 11:
                                                                pControls.Controls.Add(new LiteralControl("<td><input id=\"ck" + contadorCheck + "\" name=\"ck" + contadorCheck + "\" onclick='asignarValorCheck(" + contadorCheck + ");' runat='server' type=\"checkbox\" value=\"1\" ></td>"));
                                                                pControls.Controls.Add(new LiteralControl("<td><input id=\"CapturaTres" + contadorCheck + "\" name=\"CapturaTres\" runat='server' value=\"0\" type=\"hidden\" ></td>"));
                                                                contadorCheck++;
                                                                break;

                                                            //Tipo de campo Lista Desplegable
                                                            case 5:
                                                                List<CodigosCampo> ListaCodigosCampo = ws.consultarOpcionesLista(listaCapturaUno[i].CampId);
                                                                pControls.Controls.Add(new LiteralControl("<select name=\"CapturaTres\" id=\"CapturaTres\" required>"));
                                                                foreach (CodigosCampo campos in ListaCodigosCampo)
                                                                {
                                                                    pControls.Controls.Add(new LiteralControl("<Option value =" + campos.CodId + ">" + campos.CodDescripcion + "</Option>"));
                                                                }
                                                                pControls.Controls.Add(new LiteralControl("</select>"));
                                                                break;
                                                        }
                                                        //****************************************************************************

                                                        if (esLista || esListaCDos)
                                                        {
                                                            int campid_UNO = listaCapturaUno[i].CampId;
                                                            string codid_UNO = listaCapturaUno[i].NegValor;

                                                            int campid_DOS = listaCapturaDos[i].CampId;
                                                            string codid_DOS = listaCapturaDos[i].NegValor;

                                                            string valorUNO = ws.obtenerValorLista(campid_UNO, codid_UNO);
                                                            string valorDOS = ws.obtenerValorLista(campid_DOS, codid_DOS);

                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Blue\">&nbsp Captura UNO: " + valorUNO + "</div>"));
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Red\">&nbsp Captura DOS: " + valorDOS + "</div><br>"));
                                                        }
                                                        else
                                                        {
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Blue\">&nbsp Captura UNO: " + listaCapturaUno[i].NegValor + "</div>"));
                                                            pControls.Controls.Add(new LiteralControl("<div style=\"color:Red\">&nbsp Captura DOS: " + listaCapturaDos[i].NegValor + "</div><br>"));
                                                        }

                                                        pControls.Controls.Add(new LiteralControl("</td>"));
                                                    }

                                                    Captura datos = new Captura
                                                    {
                                                        NegId = listaCapturaUno[i].NegId,
                                                        NumCaptura = 3,
                                                        CampId = listaCapturaUno[i].CampId,
                                                        Indice = listaCapturaUno[i].Indice,
                                                        Usuario = listaCapturaUno[i].Usuario,
                                                        FechaRegistro = DateTime.Now,
                                                        DocId = listaCapturaUno[i].DocId
                                                    };

                                                    listaCamposCapturados.Add(datos);

                                                    contadorFilas++;
                                                }
                                            }
                                        }
                                    }
                                    pControls.Controls.Add(new LiteralControl("</td></tr><table>"));
                                    Session["CamposCCalidad"] = listaCamposCapturados;
                                }
                                #endregion
                                #region Grilla Capturas 1 y 2 Impuestos
                                else
                                {
                                    int valorGrillaImp = lstCampos[num].CampId;
                                    Session["Id_Grilla"] = valorGrillaImp;
                                    int idDocumentoImp = Convert.ToInt32(this.lstDocumentos.SelectedValue);
                                    int idCaptura = Convert.ToInt32(base.Request.QueryString["CAPTURA"]);

                                    List<Campos> listaCImpuestos = s.obtenerCamposGrilla(valorGrillaImp, idDocumentoImp);
                                    indiceGrid.grillaImpuestos = valorGrillaImp;

                                    //Pintar los titulos de la grilla
                                    pControls.Controls.Add(new LiteralControl("<h3>&nbsp Ingresar Impuestos</h3>"));
                                    pControls.Controls.Add(new LiteralControl("<table data-work='idGrilla' id='" + lstCampos[num].CampDescripcion + "'>"));
                                    pControls.Controls.Add(new LiteralControl("<tr>"));
                                    pControls.Controls.Add(new LiteralControl("<td></td>"));

                                    foreach (Campos campo in listaCImpuestos)
                                    {
                                        pControls.Controls.Add(new LiteralControl("<td>" + campo.CampDescripcion.ToString() + "</td>"));
                                    }

                                    pControls.Controls.Add(new LiteralControl("</tr>"));
                                    pControls.Controls.Add(new LiteralControl("<tr>"));

                                    pControls.Controls.Add(new LiteralControl("</tr>"));
                                    pControls.Controls.Add(new LiteralControl("</table>"));

                                    //Para llenar los nombres de los campos
                                    string nombreCamposImp = "";

                                    string disponible = "";
                                    foreach (Campos campo in listaCImpuestos)
                                    {
                                        if (nombreCamposImp.Trim() == "")
                                        {
                                            nombreCamposImp = campo.NomBizagi;

                                            //Validamos si estamos en captura Uno o Captura Dos
                                            //En Captura 2 Es cuando comenzamos a deshabilitar
                                            if (idCaptura == 2)
                                            {
                                                if (campo.CampNumCaptura2 == true)
                                                {
                                                    disponible = "1";
                                                }
                                                else
                                                {
                                                    disponible = "0";
                                                }
                                                //disponible = campo.CampNumCaptura2.ToString();
                                            }

                                            if (idCaptura == 1)
                                            {
                                                disponible = campo.CampNumCaptura.ToString();
                                            }
                                        }
                                        else
                                        {
                                            nombreCamposImp = nombreCamposImp + "," + campo.NomBizagi;

                                            //Validamos si estamos en captura Uno o Captura Dos
                                            //En Captura 2 Es cuando comenzamos a deshabilitar
                                            if (idCaptura == 2)
                                            {
                                                if (campo.CampNumCaptura2 == true)
                                                {
                                                    disponible = disponible + "," + "1";
                                                }
                                                else
                                                {
                                                    disponible = disponible + "," + "0";
                                                }
                                                //disponible = disponible + "," + campo.CampNumCaptura2.ToString();
                                            }

                                            if (idCaptura == 1)
                                            {
                                                disponible = disponible + "," + campo.CampNumCaptura.ToString();
                                            }
                                        }
                                    }

                                    //Para llenar los tipos de campos a pintar desde Javascript
                                    string tiposCampoImp = "";
                                    foreach (Campos campo in listaCImpuestos)
                                    {
                                        if (tiposCampoImp.Trim() == "")
                                        {
                                            tiposCampoImp = campo.TcId.ToString();
                                        }
                                        else
                                        {
                                            tiposCampoImp = tiposCampoImp + "," + campo.TcId.ToString();
                                        }
                                    }

                                    //Para concatenar todas las opciones de los campos que sean una lista desplegable
                                    string idCampos = "";
                                    foreach (Campos campo in listaCImpuestos)
                                    {
                                        if (idCampos.Trim() == "")
                                        {
                                            idCampos = campo.CampId.ToString();
                                        }
                                        else
                                        {
                                            idCampos = idCampos + "," + campo.CampId.ToString();
                                        }
                                    }

                                    //Para Crear las opciones de Maxima longitud
                                    string maxLongitud = "";
                                    foreach (Campos campo in listaCImpuestos)
                                    {
                                        if (maxLongitud.Trim() == "")
                                        {
                                            maxLongitud = campo.LongMax.ToString();
                                        }
                                        else
                                        {
                                            maxLongitud = maxLongitud + "," + campo.LongMax.ToString();
                                        }
                                    }

                                    //Para especificar si los campos son obligatorios o no.
                                    string campoObligatorio = "";
                                    foreach (Campos campo in listaCImpuestos)
                                    {
                                        if (campoObligatorio.Trim() == "")
                                        {
                                            campoObligatorio = campo.CampObligatorio.ToString();
                                        }
                                        else
                                        {
                                            campoObligatorio = campoObligatorio + "," + campo.CampObligatorio.ToString();
                                        }
                                    }

                                    pControls.Controls.Add(new LiteralControl("<input type=\"button\" id=\"Crear\"  class=\"btn btn-login\" value=\"Nuevo Registro\" onclick=\"agregarFila(" + listaCImpuestos.Count + ",'" + nombreCamposImp + "','" + tiposCampoImp + "','" + lstCampos[num].CampDescripcion + "','" + disponible + "','" + idCampos + "','" + maxLongitud + "','" + campoObligatorio + "')\" />"));
                                }
                                #endregion
                                break;
                            case 16:
                                {
                                    TextBox box = new TextBox
                                    {
                                        ID = "txt_" + c.CampId,
                                        Width = c.CampAncho,
                                        MaxLength = (int)c.LongMax,
                                        CssClass = "dec"
                                    };

                                    box.Attributes.Add("onfocus", "scrollVisor(" + c.PosX + "," + c.PosY + ")");
                                    RoundedCornersExtender extender = new RoundedCornersExtender
                                    {
                                        TargetControlID = "txt_" + c.CampId.ToString(),
                                        Radius = 6,
                                        ID = "RoundedExtender_rtxt" + num.ToString(),
                                        Corners = BoxCorners.All,
                                        Enabled = true
                                    };

                                    /*funcion de validacion personalizada*/
                                    CustomValidator CustomVal_txt;
                                    if (c.ValidationFunction != null)
                                    {
                                        CustomVal_txt = new CustomValidator
                                        {
                                            ID = "customval_" + c.CampId.ToString(),
                                            ControlToValidate = "txt_" + c.CampId,
                                            ClientValidationFunction = c.ValidationFunction,
                                            ErrorMessage = "<B><i>" + c.ErrorMessage + "</i></B>",
                                            SetFocusOnError = true,
                                            Display = ValidatorDisplay.Dynamic,
                                            ForeColor = Color.Red

                                        };
                                        row.Cells[1].Controls.Add(CustomVal_txt);
                                    }

                                    /***********************************************/
                                    row.Cells[0].Text = c.CampDescripcion + ": ";
                                    row.Cells[1].Controls.Add(box);
                                    row.Cells[1].Controls.Add(extender);

                                    if (c.CampObligatorio)
                                    {
                                        row.Cells[0].Font.Bold = true;

                                        validator2 = new RequiredFieldValidator
                                        {
                                            ID = "txtReq" + c.CampId.ToString(),
                                            ControlToValidate = "txt_" + c.CampId,
                                            ErrorMessage = "<B><i>Campo requerido</i></B>",
                                            Display = ValidatorDisplay.Dynamic,
                                            SetFocusOnError = true,
                                            ForeColor = Color.Red
                                        };
                                        row.Cells[1].Controls.Add(validator2);
                                    }
                                    if (num == 0)
                                    {
                                        box.Focus();
                                    }
                                    break;
                                }
                        }
                        num++;
                    }
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo GenerarCampos " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        public void guardarControlCalidad()
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();

            //Insertar valores en las Posiciones.
            string[] listaValoresNuevos = Request.Form.GetValues("CapturaTres");
            List<Captura> listaCapturaTres = (List<Captura>)Session["CamposCCalidad"];

            for (int i = 0; i < listaCapturaTres.Count; i++)
            {
                //Inserta la Captura Tres
                Captura capturaTres = new Captura
                {
                    NegId = listaCapturaTres[i].NegId,
                    NumCaptura = listaCapturaTres[i].NumCaptura,
                    CampId = listaCapturaTres[i].CampId,
                    Indice = listaCapturaTres[i].Indice,
                    NegValor = listaValoresNuevos[i],
                    Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                    FechaRegistro = listaCapturaTres[i].FechaRegistro,
                    DocId = listaCapturaTres[i].DocId
                };

                data.AddToCaptura(capturaTres);
                data.SaveChanges();
            }
            Session["CamposCCalidad"] = null;
        }

        public void guardarValoresGrillaPosiciones()
        {
            GestorDocumentalEnt data = new GestorDocumentalEnt();

            //Insertar valores en las Posiciones
            int valorGrilla = Convert.ToInt32(Session["Id_Grilla"]);
            int idDocumento = Convert.ToInt32(this.lstDocumentos.SelectedValue);

            WebServiceModel s = new WebServiceModel();
            List<Campos> listaCampos = s.obtenerCamposGrilla(valorGrilla, idDocumento);

            int indice = 1;
            string[] listaIndicesMapea = Request.Form.GetValues("indicePos");

            for (int i = 0; i < listaIndicesMapea.Length; i++)
            {
                listaIndicesMapea[i] = indice.ToString();
                indice++;
            }

            string[] campos;
            foreach (Campos datos in listaCampos)
            {
                campos = Request.Form.GetValues(datos.NomBizagi);
                string[] listaIndices = listaIndicesMapea;

                if (campos.Length < listaIndices.Length)
                {
                    int totalCampos = campos.Length;
                    int totalIndices = listaIndices.Length;

                    int dif = totalIndices - totalCampos;
                    List<string> _listaCampos = campos.ToList();

                    for (int i = 0; i < dif; i++)
                    {
                        _listaCampos.Add("0");
                    }

                    campos = _listaCampos.ToArray();
                }

                for (int i = 0; i < listaIndices.Length; i++)
                {
                    if (campos == null)
                    {
                        campos = new string[listaIndices.Length];
                        for (int a = 0; a < campos.Length; a++)
                        {
                            campos[a] = "";
                        }
                    }

                    string campoGuardar = campos[i];

                    //Insert a la tabla capturas
                    Captura nuevaCaptura = new Captura
                    {
                        Indice = Convert.ToInt32(listaIndices[i]),
                        CampId = Convert.ToInt32(datos.CampId),
                        NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                        NegValor = campoGuardar,
                        Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                        DocId = Convert.ToInt32(this.lstDocumentos.SelectedValue),
                        FechaRegistro = DateTime.Now,
                        NumCaptura = Convert.ToInt32(base.Request.QueryString["Captura"])
                    };
                    data.AddToCaptura(nuevaCaptura);
                    data.SaveChanges();
                }
            }
            Session["Id_Grilla"] = null;
        }

        private void InsertarCampos(Control Parent, List<Captura> lstCaptura)
        {
            try
            {
                foreach (Control control in Parent.Controls)
                {
                    Captura captura2;
                    string str = control.GetType().ToString();

                    switch (str)
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            Captura item = new Captura
                            {
                                CampId = int.Parse(control.ID.Split(new char[] { '_' }).GetValue(1).ToString()),
                                NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                                NegValor = ((TextBox)control).Text,
                                Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                                DocId = int.Parse(this.lstDocumentos.SelectedValue),
                                FechaRegistro = DateTime.Now,
                                NumCaptura = int.Parse(base.Request.QueryString["Captura"])
                            };
                            lstCaptura.Add(item);
                            break;

                        case "System.Web.UI.WebControls.DropDownList":
                            captura2 = new Captura();
                            captura2.CampId = int.Parse(control.ID.Split(new char[] { '_' }).GetValue(1).ToString());
                            captura2.NegId = ((Captura)this.Session["NEGOCIO"]).NegId;
                            captura2.NegValor = ((DropDownList)control).SelectedValue;
                            captura2.Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                            captura2.DocId = int.Parse(this.lstDocumentos.SelectedValue);
                            captura2.FechaRegistro = DateTime.Now;
                            captura2.NumCaptura = int.Parse(base.Request.QueryString["Captura"]);
                            lstCaptura.Add(captura2);
                            break;

                        case "System.Web.UI.WebControls.CheckBoxList":

                            captura2 = new Captura();
                            captura2.CampId = int.Parse(control.ID.Split(new char[] { '_' }).GetValue(1).ToString());
                            captura2.NegId = ((Captura)this.Session["NEGOCIO"]).NegId;
                            captura2.NegValor = ObtenerRespuestasCHK((CheckBoxList)control);
                            captura2.Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                            captura2.DocId = int.Parse(this.lstDocumentos.SelectedValue);
                            captura2.FechaRegistro = DateTime.Now;
                            captura2.NumCaptura = int.Parse(base.Request.QueryString["Captura"]);
                            lstCaptura.Add(captura2);
                            break;
                    }

                    if (control.Controls.Count > 0)
                    {
                        this.InsertarCampos(control, lstCaptura);
                    }
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo InsertarCampos " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        private string ObtenerRespuestasCHK(CheckBoxList chk)
        {
            try
            {
                string resp = "";
                foreach (ListItem i in chk.Items)
                {
                    if (i.Selected == true)
                    {
                        resp = resp + i.Value + ";";
                    }
                }

                return resp.Trim(new char[] { ';' }).Split(new char[] { '_' }).GetValue(0).ToString();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo ObtenerRespuestasCHK " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }

        }

        protected void lstDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.lstDocumentos.SelectedIndex > 0)
                {
                    Documentos d = new Documentos
                    {
                        DocId = Convert.ToInt32(this.lstDocumentos.SelectedItem.Value),
                        DocDescripcion = this.lstDocumentos.SelectedItem.Text
                    };
                    this.CargarGruposDocs(d);
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo lstDocumentos_SelectedIndexChanged " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }

        }

        protected void lstGrupos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}