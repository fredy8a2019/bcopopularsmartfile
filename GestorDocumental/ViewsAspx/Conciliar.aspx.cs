using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using GestorDocumental.Models;

namespace GestorDocumental.ViewsAspx
{
    public partial class Conciliar : System.Web.UI.Page
    {
        private GestorDocumentalEnt bd = new GestorDocumentalEnt();
        private GestorDocumental.Controllers.AsignacionesController bAsig = new Controllers.AsignacionesController();
        private GestorDocumental.Controllers.CamposController bCampos = new Controllers.CamposController();
        private GestorDocumental.Controllers.CapturaController bCap;
        private GestorDocumental.Controllers.DocumentosController bdoc;

        //CAPTURA DE DATOS E INSERCION EN LA TABLA
        protected void Button1_Click1(object sender, EventArgs e)
        {
            try
            {
                this.bCap = new GestorDocumental.Controllers.CapturaController();

                List<Captura> lstCaptura = new List<Captura>();
                this.InsertarCampos(this.pControls, lstCaptura);
                bool resultado = false;

                bool validacion = (bool)Session["OMITE_VALIDACION"];
                
                //OMITO LA VALIDACION ?
                if (!validacion)
                {
                    //VALIDO CONTRA LA CONCILIACION DEL ARCHIVO CAPTURADA EN RECEPCION
                    resultado = this.bCap.ValidaConciliacion(lstCaptura, (int)((Captura)this.Session["NEGOCIO"]).NegId, (int)Session["ID_SUBDOC_CONCILIACION"], (int)Session["SW_CONCILIACION"]);

                    if (resultado)
                    {
                        Session["SW_CONCILIACION"] = 0;
                    }
                    else
                    {
                        Session["SW_CONCILIACION"] = 1;
                    }
                }
                else
                {
                    Session["SW_CONCILIACION"] = 0;
                    resultado = true;
                }

                //SI LA VALIDACION CONTRA EL ARCHIVO FUE CORRECTA SIGUE EL PROCESO
                if (resultado)
                {
                    this.bCap.InsertarCaptura(lstCaptura);
                    //int num = this.bCap.obtenerUltimoDocumento((Clientes)this.Session["CLIENTE"]);
                    int num = (int)Session["ID_DOC_CONCILIACION"];
                    Documentos d = new Documentos
                    {
                        //DocId = this.bCap.ObtenerDocumentoActual((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"])
                        DocId = (int)Session["ID_DOC_CONCILIACION"]
                    };
                    Documentos documentos2 = new Documentos
                    {
                        //DocId = this.bCap.ObtenerSiguienteDocumento((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"])
                        DocId = (int)Session["ID_DOC_CONCILIACION"]
                    };
                    int num2 = 0;
                    if ((d.DocId == num) | (documentos2.DocId == num))
                    {
                        AsignacionTareas a = new AsignacionTareas
                        {
                            IdEstado = 30,
                            HoraTerminacion = new DateTime?(DateTime.Now),
                            NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                            IdEtapa = ((P_Etapas)this.Session["ETAPA"]).IdEtapa
                        };
                        this.bAsig.insertarAsignacion(a);
                        num2 = 1;
                    }
                    else
                    {
                        int num6;
                        int num3 = this.bCap.ObtenerGrupoActual((Captura)this.Session["NEGOCIO"], d);
                        int num4 = this.bCap.ObtenerUltimoGrupo(d);
                        if (num3 == num4)
                        {
                            //Parametros param1 = bd.Parametros.First(c => c.codigo == "DOC_CONC");
                            //int num5 = this.bCap.ObtenerSiguienteDocumento((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"]);
                            int num5 = (int)Session["ID_DOC_CONCILIACION"];
                            this.lstDocumentos.SelectedIndex = this.lstDocumentos.Items.IndexOf(this.lstDocumentos.Items.FindByValue(num5.ToString()));
                            Documentos documentos3 = new Documentos
                            {
                                DocId = num5
                            };
                            this.CargarGruposDocs(documentos3);
                            num6 = (int)Session["ID_SUBDOC_CONCILIACION"];
                            //num6 = 6;
                            this.lstGrupos.SelectedIndex = this.lstGrupos.Items.IndexOf(this.lstGrupos.Items.FindByValue(num6.ToString()));
                        }
                        else
                        {
                            //Obtenemos el grupo asociado al negocio en proceso
                            int negID = Convert.ToInt32(Session["NEGOCIO"].ToString());
                            int idGrupo = bCap.obtenerSubGrupo(negID);

                            Documentos documentos4 = new Documentos
                            {
                                DocId = this.bCap.ObtenerDocumentoActual((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"], idGrupo)
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
                        base.Response.Redirect("Conciliar.aspx?Captura=" + base.Request.QueryString["Captura"].ToString());
                    }
                    
                }
                else
                {
                    this.lblError.Text = "Documento no conciliado verifique nuevamente";
                    this.btn_omiteConciliacion.Visible = true;
                    this.UpdatePanel1.Update();
                }
               
            }
            catch (Exception exception)
            {
                this.lblError.Text = exception.Message;                
                this.UpdatePanel1.Update();
            }
        }

        private void CargarCampos()
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
                this.GenerarCampos(this.tblControls, lstCampos);
                Campos campos = lstCampos.Find(c => c.CampId == 5);
                Hashtable hashtable = this.bCampos.CamposPresentados(this.bCampos.ObtenerCamposCliente(g, e, n));
                this.UpdatePanel2.Update();
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
                throw exception;
            }
        }

        private void CargarDocumentos(Clientes c)
        {
            //Parametros param1 = bd.Parametros.First(co => co.codigo == "DOC_CONC");
            //int num5 = this.bCap.ObtenerSiguienteDocumento((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"]);
            
            this.bdoc = new GestorDocumental.Controllers.DocumentosController();
            List<Documentos> list = this.bdoc.obtenerDocumentosClienteConciliacion(c, (int)Session["ID_DOC_CONCILIACION"]);
            this.lstDocumentos.DataSource = list;
            this.lstDocumentos.DataTextField = "DocDescripcion";
            this.lstDocumentos.DataValueField = "DocId";
            this.lstDocumentos.DataBind();
        }

        private void CargarGruposDocs(Documentos D)
        {
            //Parametros param1 = bd.Parametros.First(c => c.codigo == "DOC_CONC");
            //int num5 = this.bCap.ObtenerSiguienteDocumento((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"]);
            int num5 = (int)Session["ID_DOC_CONCILIACION"];
            int num6 = (int)Session["ID_SUBDOC_CONCILIACION"];

            this.bdoc = new GestorDocumental.Controllers.DocumentosController();
            List<GruposDocumentos> list = this.bdoc.obtenerGruposDocumentosConciliacion(D,num6);
            this.lstGrupos.DataSource = list;
            this.lstGrupos.DataTextField = "GDocDescripcion";
            this.lstGrupos.DataValueField = "GDocId";
            this.lstGrupos.DataBind();
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
                throw exception;
            }
        }

        private void GenerarCampos(Table Ta, List<Campos> lstCampos)
        {
            int num = 0;
            Table child = new Table
            {
                ID = "TBL_PRICIPAL" + this.lstGrupos.SelectedValue,
                EnableViewState = true
            };
            this.pControls.Controls.Add(child);
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
                    child.Rows.Add(row);
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
                                    ValidationExpression = @"\d{0,15}",
                                    ErrorMessage = "<B><i>El campo debe ser numerico</i></B>",
                                    Display = ValidatorDisplay.None
                                };
                                ValidatorCalloutExtender extender2 = new ValidatorCalloutExtender
                                {
                                    TargetControlID = "txtRegular" + c.CampId.ToString(),
                                    Enabled = true,
                                    PopupPosition = ValidatorCalloutPosition.Right,
                                    ID = "vCallNum" + c.CampId.ToString(),
                                    ClientIDMode = ClientIDMode.Inherit
                                };
                                row.Cells[0].Text = c.CampDescripcion + ": ";
                                row.Cells[1].Controls.Add(box);
                                row.Cells[1].Controls.Add(extender);
                                row.Cells[1].Controls.Add(validator);
                                row.Cells[1].Controls.Add(extender2);
                                if (c.CampObligatorio)
                                {
                                    validator2 = new RequiredFieldValidator
                                    {
                                        ID = "txtReq" + c.CampId.ToString(),
                                        ControlToValidate = "txt_" + c.CampId,
                                        ErrorMessage = "<B><i>Campo requerido</i></B>",
                                        Display = ValidatorDisplay.None,
                                        SetFocusOnError = true,
                                        ForeColor = Color.Red
                                    };
                                    extender3 = new ValidatorCalloutExtender
                                    {
                                        TargetControlID = "txtReq" + c.CampId.ToString(),
                                        Enabled = true,
                                        PopupPosition = ValidatorCalloutPosition.Right,
                                        ID = "vCall" + c.CampId.ToString(),
                                        ClientIDMode = ClientIDMode.Inherit,
                                        HighlightCssClass = "highlight"
                                    };
                                    row.Cells[1].Controls.Add(validator2);
                                    row.Cells[1].Controls.Add(extender3);
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
                                    validator2 = new RequiredFieldValidator
                                    {
                                        ID = "txtReq" + c.CampId.ToString(),
                                        ControlToValidate = "txt_" + c.CampId,
                                        ErrorMessage = "<B><i>Campo requerido</i></B>",
                                        Display = ValidatorDisplay.None,
                                        SetFocusOnError = true,
                                        ForeColor = Color.Red
                                    };
                                    extender3 = new ValidatorCalloutExtender
                                    {
                                        TargetControlID = "txtReq" + c.CampId.ToString(),
                                        Enabled = true,
                                        PopupPosition = ValidatorCalloutPosition.Right,
                                        ID = "vCall" + c.CampId.ToString(),
                                        ClientIDMode = ClientIDMode.Inherit,
                                        HighlightCssClass = "highlight"
                                    };
                                    row.Cells[1].Controls.Add(validator2);
                                    row.Cells[1].Controls.Add(extender3);
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
                                    Width = c.CampAncho,
                                    MaxLength = (int)c.LongMax
                                };
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
                                    Display = ValidatorDisplay.None
                                };
                                ValidatorCalloutExtender extender7 = new ValidatorCalloutExtender
                                {
                                    TargetControlID = "rangeV" + c.CampId.ToString(),
                                    Enabled = true,
                                    PopupPosition = ValidatorCalloutPosition.Right,
                                    ID = "vCallFecha" + c.CampId.ToString(),
                                    ClientIDMode = ClientIDMode.Inherit,
                                    HighlightCssClass = "highlight"
                                };
                                row.Cells[0].Text = c.CampDescripcion + ": ";
                                row.Cells[1].Controls.Add(box3);
                                row.Cells[1].Controls.Add(extender5);
                                row.Cells[1].Controls.Add(extender6);
                                row.Cells[1].Controls.Add(validator3);
                                row.Cells[1].Controls.Add(extender7);
                                if (c.CampObligatorio)
                                {
                                    validator2 = new RequiredFieldValidator
                                    {
                                        ID = "txtReq" + c.CampId.ToString(),
                                        ControlToValidate = "txt_" + c.CampId,
                                        ErrorMessage = "<B><i>Campo requerido</i></B>",
                                        Display = ValidatorDisplay.None,
                                        SetFocusOnError = true,
                                        ForeColor = Color.Red
                                    };
                                    extender3 = new ValidatorCalloutExtender
                                    {
                                        TargetControlID = "txtReq" + c.CampId.ToString(),
                                        Enabled = true,
                                        PopupPosition = ValidatorCalloutPosition.Right,
                                        ID = "vCall" + c.CampId.ToString(),
                                        ClientIDMode = ClientIDMode.Inherit,
                                        HighlightCssClass = "highlight"
                                    };
                                    row.Cells[1].Controls.Add(validator2);
                                    row.Cells[1].Controls.Add(extender3);
                                }
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
                                    validator2 = new RequiredFieldValidator
                                    {
                                        ID = "lstReq" + c.CampId.ToString(),
                                        ControlToValidate = "lst_" + c.CampId.ToString(),
                                        ErrorMessage = "<B><i>Seleccione una opcion</i></B>",
                                        Display = ValidatorDisplay.None,
                                        SetFocusOnError = true,
                                        InitialValue = "0"
                                    };
                                    extender3 = new ValidatorCalloutExtender
                                    {
                                        TargetControlID = "lstReq" + c.CampId.ToString(),
                                        Enabled = true,
                                        PopupPosition = ValidatorCalloutPosition.Right,
                                        ID = "vCall" + c.CampId.ToString(),
                                        ClientIDMode = ClientIDMode.Inherit,
                                        HighlightCssClass = "highlight"
                                    };
                                    row.Cells[1].Controls.Add(validator2);
                                    row.Cells[1].Controls.Add(extender3);
                                }
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
                                    validator2 = new RequiredFieldValidator
                                    {
                                        ID = "lstReq" + c.CampId.ToString(),
                                        ControlToValidate = "lst_" + c.CampId.ToString() + "_" + c.CampDependiente.ToString(),
                                        ErrorMessage = "<B><i>Seleccione una opcion</i></B>",
                                        Display = ValidatorDisplay.None,
                                        SetFocusOnError = true,
                                        InitialValue = "0"
                                    };
                                    extender3 = new ValidatorCalloutExtender
                                    {
                                        TargetControlID = "lstReq" + c.CampId.ToString(),
                                        Enabled = true,
                                        PopupPosition = ValidatorCalloutPosition.Right,
                                        ID = "vCall" + c.CampId.ToString(),
                                        ClientIDMode = ClientIDMode.Inherit,
                                        HighlightCssClass = "highlight"
                                    };
                                    row.Cells[1].Controls.Add(validator2);
                                    row.Cells[1].Controls.Add(extender3);
                                }
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
                                    validator2 = new RequiredFieldValidator
                                    {
                                        ID = "lstReq" + c.CampId.ToString(),
                                        ControlToValidate = "lstCiu_" + c.CampId.ToString(),
                                        ErrorMessage = "<B><i>Seleccione una opcion</i></B>",
                                        Display = ValidatorDisplay.None,
                                        SetFocusOnError = true,
                                        InitialValue = "0"
                                    };
                                    extender3 = new ValidatorCalloutExtender
                                    {
                                        TargetControlID = "lstReq" + c.CampId.ToString(),
                                        Enabled = true,
                                        PopupPosition = ValidatorCalloutPosition.Right,
                                        ID = "vCall" + c.CampId.ToString(),
                                        ClientIDMode = ClientIDMode.Inherit,
                                        HighlightCssClass = "highlight"
                                    };
                                    row.Cells[1].Controls.Add(validator2);
                                    row.Cells[1].Controls.Add(extender3);
                                }
                                if (num == 0)
                                {
                                    list5.Focus();
                                }
                                break;
                            }
                    }
                    num++;
                }
            }
        }

        private void InsertarCampos(Control Parent, List<Captura> lstCaptura)
        {
            foreach (Control control in Parent.Controls)
            {
                Captura captura2;
                string str = control.GetType().ToString();
                if (str != null)
                {
                    if (!(str == "System.Web.UI.WebControls.TextBox"))
                    {
                        if (str == "System.Web.UI.WebControls.DropDownList")
                        {
                            goto Label_0135;
                        }
                    }
                    else
                    {
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
                    }
                }
                goto Label_020F;
            Label_0135:
                captura2 = new Captura();
                captura2.CampId = int.Parse(control.ID.Split(new char[] { '_' }).GetValue(1).ToString());
                captura2.NegId = ((Captura)this.Session["NEGOCIO"]).NegId;
                captura2.NegValor = ((DropDownList)control).SelectedValue;
                captura2.Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                captura2.DocId = int.Parse(this.lstDocumentos.SelectedValue);
                captura2.FechaRegistro = DateTime.Now;
                captura2.NumCaptura = int.Parse(base.Request.QueryString["Captura"]);
                lstCaptura.Add(captura2);
            Label_020F:
                if (control.Controls.Count > 0)
                {
                    this.InsertarCampos(control, lstCaptura);
                }
            }
        }

        protected void lstDocumentos_SelectedIndexChanged(object sender, EventArgs e)
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


        protected void Page_Load(object sender, EventArgs e)
        {
            Session["OMITE_VALIDACION"] = false;
            Session["TITULO"] = "Conciliación de documentos";
            if (!this.Page.IsPostBack)
            {
                Parametros parametros = bd.Parametros.First(cond => cond.codigo == "DOC_CONC");
                
                Session["ID_DOC_CONCILIACION"] = int.Parse(parametros.valor);
                int docu=int.Parse(parametros.valor);
                int grupo = bd.GruposDocumentos.First(w => w.DocId == docu).GDocId;
                Session["ID_SUBDOC_CONCILIACION"] = grupo;

                this.CargarDocumentos((Clientes)this.Session["CLIENTE"]);
                Documentos D=new Documentos();
                //Parametros param1 = bd.Parametros.First(c => c.codigo == "DOC_CONC");
                //int num5 = this.bCap.ObtenerSiguienteDocumento((Captura)this.Session["NEGOCIO"], (Clientes)this.Session["CLIENTE"]);
                //int num5 = Int32.Parse(param1.valor.ToString());                
                D.DocId = (int)Session["ID_DOC_CONCILIACION"];
                this.CargarGruposDocs( D);
                P_Etapas etapas = new P_Etapas();
                etapas.IdEtapa = 70;
                this.Session["ETAPA"] = etapas;
                //if (base.Request.QueryString["CAPTURA"].ToString() == "1")
                //{
                //    etapas.IdEtapa = 30;
                //    this.Session["ETAPA"] = etapas;
                //}
                //else if (base.Request.QueryString["CAPTURA"].ToString() == "2")
                //{
                //    etapas.IdEtapa = 40;
                //    this.Session["ETAPA"] = etapas;
                //}
                //else if (base.Request.QueryString["CAPTURA"].ToString() == "3")
                //{
                //    etapas.IdEtapa = 50;
                //    this.Session["ETAPA"] = etapas;
                //}
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
                    //int num = this.bCap.ObtenerDocumentoActual(c, (Clientes)this.Session["CLIENTE"]);

                    int num = (int)Session["ID_DOC_CONCILIACION"];
                    Documentos d = new Documentos
                    {
                        DocId = num
                    };
                    //int num2 = this.bCap.ObtenerGrupoActual(c, d);
                    //int num3 = this.bCap.ObtenerUltimoGrupo(d);



                    int num2 = (int)Session["ID_SUBDOC_CONCILIACION"];
                    int num3 = (int)Session["ID_SUBDOC_CONCILIACION"];

                    this.lstDocumentos.SelectedIndex = this.lstDocumentos.Items.IndexOf(this.lstDocumentos.Items.FindByValue(num.ToString()));
                    this.lstGrupos.SelectedIndex = this.lstGrupos.Items.IndexOf(this.lstGrupos.Items.FindByValue(num3.ToString()));

                    //if (num2 == num3)
                    //{
                    //    int num4 = this.bCap.ObtenerSiguienteDocumento(c, (Clientes)this.Session["CLIENTE"]);
                    //    d.DocId = num4;
                    //    this.lstDocumentos.SelectedIndex = this.lstDocumentos.Items.IndexOf(this.lstDocumentos.Items.FindByValue(num4.ToString()));
                    //    this.CargarGruposDocs(d);
                    //    num5 = this.bCap.ObtenerSiguienteGrupo(c, d);
                    //    this.lstGrupos.SelectedIndex = this.lstGrupos.Items.IndexOf(this.lstGrupos.Items.FindByValue(num5.ToString()));
                    //    num6 = this.bCap.obtenerUltimoDocumento((Clientes)this.Session["CLIENTE"]);
                    //}
                    //else
                    //{
                    //    d.DocId = num;
                    //    this.lstDocumentos.SelectedIndex = this.lstDocumentos.Items.IndexOf(this.lstDocumentos.Items.FindByValue(num.ToString()));
                    //    this.CargarGruposDocs(d);
                    //    num5 = this.bCap.ObtenerSiguienteGrupo(c, d);
                    //    this.lstGrupos.SelectedIndex = this.lstGrupos.Items.IndexOf(this.lstGrupos.Items.FindByValue(num5.ToString()));
                    //    num6 = this.bCap.obtenerUltimoDocumento((Clientes)this.Session["CLIENTE"]);
                    //}
                    this.CargarCampos();
                    ((Label)base.Master.FindControl("lblNegocio")).Text = "Negocio:" + c.NegId.ToString();
                }
            }
            else
            {
                this.CargarCampos();
            }
            this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "FileLoad", "neg = '" + ConfigurationManager.AppSettings["ClientFiles"] + "/" + ((Captura)this.Session["NEGOCIO"]).NegId.ToString() + "/" + ((Captura)this.Session["NEGOCIO"]).NegId.ToString() + ".TIF';", true);
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

        protected void lstGrupos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["OMITE_VALIDACION"] = true;
            this.Button1_Click1(sender,e);
        }

    }
}
