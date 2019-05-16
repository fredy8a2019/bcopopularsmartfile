using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AjaxControlToolkit;
using GestorDocumental.Controllers;
using GestorDocumental.Models;
using GestorDocumental.WebService;
using System.Collections;
using System.Web.UI.WebControls;
using System.Drawing;

namespace GestorDocumental.Controllers
{
    public class CrearFormulariosCaptura
    {
        private GestorDocumental.Controllers.AsignacionesController bAsig = new Controllers.AsignacionesController();
        private GestorDocumental.Controllers.CamposController bCampos = new Controllers.CamposController();
        private GestorDocumental.Controllers.CapturaController bCap;
        private GestorDocumental.Controllers.DocumentosController bdoc;
        private GestorDocumentalEnt consultas = new GestorDocumentalEnt();
        //agrega Camilo Padilla; junio-2016
        string fechaminima;

        /// <summary>
        /// Carga los Documentos para la Captura
        /// </summary>
        /// <param name="c">El tipo de clientes asociados a los documentos que se van a cargar</param>
        /// <returns>Lista con los documentos asociados con los clientes</returns>
        public List<Documentos> CargarDocumentos(Clientes c)
        {
            try
            {
                this.bdoc = new GestorDocumental.Controllers.DocumentosController();
                List<Documentos> list = this.bdoc.obtenerDocumentosCliente(c);

                //agrega Camilo Padilla; junio-2016
                var t = consultas.sp_fechaMinima().ToList();
                fechaminima = t[0];
                return list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo CargarDocumentos " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Carga los grupos de Documentos para la Captura
        /// </summary>
        /// <param name="D">El tipo de documentos que se van a cargar</param>
        /// <returns>Lista con los grupos de documentos asociados a la captura general</returns>
        public List<GruposDocumentos> CargarGruposDocs(Documentos D)
        {
            try
            {
                this.bdoc = new GestorDocumental.Controllers.DocumentosController();
                List<GruposDocumentos> list = this.bdoc.obtenerGruposDocumentos(D);
                return list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo CargarGruposDocs " + exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Genera los diferentes tipos de campos a Capturar en el formulario.
        /// </summary>
        /// <param name="Ta">La Tabla donde se almacenaran la descripcion de los campos</param>
        /// <param name="lstCampos">La lista de los campos a generar para el formulario</param>
        /// <param name="lstRespAnt">Si la Etapa es control de calidad se pintaran los resultados de las capturas 1 y 2</param>
        /// <param name="idGrupoDocumentos">El ID del grupo de documento asociado para pintar los campos</param>
        /// <param name="idEtapa">La etapa de la captura en la que se encuentra actualmente</param>
        /// <param name="noCaptura">El número de la captura actual</param>
        /// <param name="idDocumento">El documento actual que se va a pintar</param>
        /// <param name="NegID">El número del negocio que se esta pintando actualmente</param>
        /// <returns>Cadena de texto con formato HTML para pintar con los campos generados</returns>
        public string GenerarCampos(Table Ta, List<Campos> lstCampos, List<spObtenerRespuestasAnteriores_Result> lstRespAnt, int idGrupoDocumentos, int idEtapa, string noCaptura, int idDocumento, int NegID)

        {
            Panel pControls = new Panel();
            string cadenaCampos = "";
            try
            {
                int num = 0;
                Table child = new Table
                {
                    ID = "TBL_PRICIPAL" + idGrupoDocumentos,
                    EnableViewState = true
                };

                pControls.Controls.Add(child);
                if (idEtapa == 50)
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
                        TableRow row = new TableRow();
                        row.ID = "Trow_" + c.CampId;

                        TableCell cell = new TableCell();
                        cell.ID = "TcellDesc_" + c.CampId;

                        TableCell cell2 = new TableCell();
                        cell2.ID = "TcellCtrl_" + c.CampId;

                        TableCell cell3 = new TableCell();
                        cell3.ID = "TcellError_" + c.CampId;

                        row.Cells.Add(cell);
                        row.Cells.Add(cell2);
                        row.Cells.Add(cell3);

                        ///DE DEBEN CREAR DOS NUEVAS CELDAS EN LA PARTE IZQUIERDA DONDE SE DESPLIEGUEN LAS RESPUESTAS 1 Y 2
                        ///SE DEBE VALIDAR  QUE CORRESPONDA A LA ETAPA 3 
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

                        string obligatorio = "";
                        string funcionJavascript = "";
                        string funcionNegrilla = "";
                        string javascriptFocus = "";
                        string campoOculto = "";
                        string javascriptFechas = "";
                        string javascriptFunction = "";//JFP ; se agrega
                        string inactivo = "";//JFP; agrega variable

                        //JFP; agrega
                        if (c.idPadre != null && c.idPadre > 0)
                        //if (c.ColConciliacion != null)
                        {
                            inactivo = " disabled='disabled'";
                        }

                        switch (lstCampos[num].TcId)
                        {
                            //Textbox - Solo acepta Numeros
                            
                            case 1:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                //campoOculto = "<td><span class=\"lbl_" + c.CampId + "\"></span></td>";

                                List<sp_ObtenerValidacionesMinimoCarac_Result> valMini = consultas.sp_ObtenerValidacionesMinimoCarac(c.CampId).ToList();

                                string MinimoLongCamp = valMini[0].valor.ToString();

                                string txt1 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    //JFP; modifica
                                    //"<td><input class=\"form-control cmenu1\" type='Text' " + obligatorio + " " + javascriptFocus + " onkeypress='return numbersonly(event);' onblur='infoAdicional(this.value, '_lbl" + c.CampId + "');' maxlength='" +
                                    "<td><input class=\"form-control \" type='Text' " + obligatorio + " " + javascriptFocus + inactivo + " onkeypress='return numbersonly(event);'  maxlength='" +
                                    c.LongMax + "' name = '" + c.CampId + "' id='txt_" +
                                    c.CampId + "' Width = '" +
                                    c.CampAncho + "' onKeyUp=validarCaracteres('txt_" + c.CampId + "') onmousedown = ilegibleFaltante('txt_" + c.CampId + "',event) onblur= ValidarMinCampo('txt_" + c.CampId + "','" + MinimoLongCamp + "','" + c.CampId + "')  />" + campoOculto + "</td>";

                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt1 = txt1 + "<td><center>" +
                                        "<a style=\"color:red\" onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt1_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaUNO + "</a>" + "<input type=\"hidden\" id=\"cpt1_" + c.CampId + "\" value=" + capturaUNO + " />" +
                                        "</center></td><td><center>" +
                                        "<a style=\"color:blue\" onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt2_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaDOS + "</a></center></td>" + "<input type=\"hidden\" id=\"cpt2_" + c.CampId + "\" value=" + capturaDOS + " />";
                                }

                                txt1 = txt1 + "</tr>";

                                cadenaCampos = cadenaCampos + txt1;
                                break;

                            //Textbox - Campo Alfanumerico
                            case 2:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                //JFP; se agrega
                                if (!string.IsNullOrEmpty(c.ValidationFunction))
                                {
                                    javascriptFunction = c.ValidationFunction;
                                }

                                string txt2 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    //"<td><input class=\"form-control cmenu1\" type='Text' " + obligatorio + " " + javascriptFocus + " onblur=\"infoAdicional(this.value, 'lbl_" + c.CampId + "');\" maxlength='" +
                                    "<td><input class=\"form-control \" type='Text' " + obligatorio + " " + javascriptFocus + javascriptFunction + inactivo + " maxlength='" + //JFP; se agrega
                                    c.LongMax + "' name = '" + c.CampId + "' id='txt_" +
                                    c.CampId + "' Width = '" +
                                    c.CampAncho + "' onmousedown = ilegibleFaltante('txt_" + c.CampId + "',event)  />" + campoOculto + "</td>";

                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt2 = txt2 + "<td><center>" +
                                        "<a style=\"color:red\" onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt1_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaUNO + "</a>" + "<input type=\"hidden\" id=\"cpt1_" + c.CampId + "\" value=" + capturaUNO + " />" +
                                        "</center></td><td><center>" +
                                        "<a style=\"color:blue\" onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt2_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaDOS + "</a></center></td>" + "<input type=\"hidden\" id=\"cpt2_" + c.CampId + "\" value=" + capturaDOS + " />";
                                }

                                txt2 = txt2 + "</tr>";

                                cadenaCampos = cadenaCampos + txt2;
                                break;

                            //Textbox - Fecha
                            case 3:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                if (!string.IsNullOrEmpty(c.ValidationFunction))
                                {
                                    javascriptFechas = c.ValidationFunction;
                                }

                                //modifica Camilo Padilla; junio-2016
                                //string txt3 = "<tr>" +
                                //    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                //    //JFP; modifica
                                //    //"<td><input class=\"form-control dpms cmenu1\" type='Text'" + obligatorio + " " + javascriptFocus + " " + javascriptFechas + " maxlength='" +
                                //    "<td><input class=\"form-control dpms cmenu1\" type='Text'" + obligatorio + inactivo + " " + javascriptFocus + " " + javascriptFechas + " maxlength='" +
                                //    c.LongMax + "' name = '" + c.CampId + "' id='txt_" +
                                //    c.CampId + "' Width = '" +
                                //    c.CampAncho + "' /></td>";

                                //=======================================================
                                //Modifica Ivan Rodriguez; julio-2016
                                //obtiene el rango de fechas parametrizado
                                List<sp_ObtenerValidacionesFechas_Result> valFecha = consultas.sp_ObtenerValidacionesFechas(c.CampId).ToList();
                                string operadorMin = valFecha[0].Operador.ToString();
                                string operadorMax = valFecha[1].Operador.ToString();
                                if (operadorMin.Equals(">"))
                                {
                                    operadorMin = "mayorQue";
                                }
                                else operadorMin = "menorQue";
                                if (operadorMax.Equals(">"))
                                {
                                    operadorMax = "mayorQue";
                                }
                                else operadorMax = "menorQue";

                                int? fechaMinima;
                                int? fechaMaxima;
                                fechaMinima = (int?)valFecha[0].valor;
                                fechaMaxima = (int?)valFecha[1].valor;
                                string txt3 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    "<td><input title ='error en la fecha ' class=\"form-control dpms \" type='text'" + obligatorio + " " + javascriptFocus + " " + javascriptFechas + " maxlength='" +
                                    c.LongMax + "' " + inactivo + " name = '" + c.CampId + "' id='txt_" +
                                    c.CampId + "' Width = '" +
                                    c.CampAncho + "' onkeyup=validarFormatoFecha('txt_" + c.CampId + "')   onblur=validarFechaCaptura('txt_" + c.CampId + "','" + fechaMinima + "','" + fechaMaxima + "','" + operadorMin + "','" + operadorMax + "')  onmousedown=ilegibleFaltante('txt_" + c.CampId + "',event)  /></td>";
                                //=======================================================


                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt3 = txt3 + "<td><center>" +
                                        "<a style=\"color:red\" onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt1_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaUNO + "</a>" + "<input type=\"hidden\" id=\"cpt1_" + c.CampId + "\" value=" + capturaUNO + " />" +
                                        "</center></td><td><center>" +
                                        "<a style=\"color:blue\" onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt2_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaDOS + "</a></center></td>" + "<input type=\"hidden\" id=\"cpt2_" + c.CampId + "\" value=" + capturaDOS + " />";
                                }

                                txt3 = txt3 + "</tr>";

                                cadenaCampos = cadenaCampos + txt3;
                                break;

                            //Textbox - Hora
                            case 4:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                string txt4 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    "<td><input class=\"form-control hpms \" type='Text'" + obligatorio + " " + javascriptFocus + " maxlength='" +
                                    c.LongMax + "' name = '" + c.CampId + "' id='txt_" +
                                    c.CampId + "' Width = '" +
                                    c.CampAncho + "' onmousedown=ilegibleFaltante('txt_" + c.CampId + "',event) /></td>";

                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt4 = txt4 + "<td><center>" +
                                        "<a style=\"color:red\" onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt1_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaUNO + "</a>" + "<input type=\"hidden\" id=\"cpt1_" + c.CampId + "\" value=" + capturaUNO + " />" +
                                        "</center></td><td><center>" +
                                        "<a style=\"color:blue\"  onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt2_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaDOS + "</a></center></td>" + "<input type=\"hidden\" id=\"cpt2_" + c.CampId + "\" value=" + capturaDOS + " />";
                                }

                                txt4 = txt4 + "</tr>";

                                cadenaCampos = cadenaCampos + txt4;
                                break;

                            //Listas de seleccion
                            case 5:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                //JFP; se agrega
                                if (!string.IsNullOrEmpty(c.ValidationFunction))
                                {
                                    javascriptFunction = c.ValidationFunction;
                                }
                                else
                                {
                                    javascriptFunction = "";
                                }

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }


                                Campos camp = new Campos();
                                camp.CampId = c.CampId;
                                string opcionesLista = "";

                                string txt5;
                                string javascriptPadre = "";
                                List<spConsultarHijos_Result> campoHijo = consultas.spConsultarHijos(c.CampId).ToList();
                                if (campoHijo.Count > 0)
                                {
                                    int _campoHijo = Convert.ToInt32(campoHijo[0].CampId);
                                    //javascriptPadre = "onchange=\"llenarListasPadreHijo('lst_" + _campoHijo + "','lst_" + c.CampId + "')\"";
                                    javascriptPadre = "onchange=\"llenarListasPadreHijo('lst_" + _campoHijo + "','lst_" + c.CampId + "','" + c.CampId + "' )\"";
                                    //inactivo = " disabled='disabled'";
                                }
                                else
                                    javascriptPadre = "";

                                if (c.CampDependiente == null)
                                {
                                    List<CodigosCampo> lstCodigos = bCampos.obtenerCodigosCampo(camp);
                                    for (int i = 0; i < lstCodigos.Count; i++)
                                    {
                                        if (obligatorio == " required " && i == 0)
                                        {
                                            opcionesLista = "<option value=''>" + lstCodigos[i].CodDescripcion + "</option>";
                                        }
                                        else if (opcionesLista == "")
                                            opcionesLista = "<option value='" + lstCodigos[i].CodId + "'>" + lstCodigos[i].CodDescripcion + "</option>";
                                        else
                                            opcionesLista = opcionesLista + "<option value='" + lstCodigos[i].CodId + "'>" + lstCodigos[i].CodDescripcion + "</option>";
                                    }

                                    txt5 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                        //JFP; modifica
                                        //"<td><select " + javascriptPadre + " class=\"form-control\"" + obligatorio + " name = '" + c.CampId + "' id = 'lst_" + c.CampId + "'>" +
                                    "<td><select " + javascriptPadre + " class=\"form-control\"" + obligatorio + javascriptFocus + javascriptFunction + inactivo + " name = '" + c.CampId + "' id = 'lst_" + c.CampId + "'>" +
                                    opcionesLista +
                                    "</select></td>";
                                }
                                else
                                {
                                    txt5 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    "<td><select class=\"form-control\"" + obligatorio + javascriptFunction+javascriptFocus+ " name = '" + c.CampId + "' id = 'lst_" + c.CampId + "'>" +
                                    "</select></td>";
                                }

                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt5 = txt5 + "<td><center><a style=\"color:red\">" + capturaUNO + "</a></center></td>" +
                                        "<td><center><a style=\"color:blue\">" + capturaDOS + "</a></center></td>";
                                }

                                txt5 = txt5 + "</tr>";

                                cadenaCampos = cadenaCampos + txt5;
                                break;

                            //Lista de Selección de Departamentos
                            case 6:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                Campos campDept = new Campos();
                                campDept.CampId = c.CampId;
                                string opcionesListaDept = "";

                                List<P_Departamentos> lstDepartamentos = bCampos.obtenerDepartamentos();
                                for (int i = 0; i < lstDepartamentos.Count; i++)
                                {
                                    if (opcionesListaDept == "")
                                        opcionesListaDept = "<option value='" + lstDepartamentos[i].DeptId + "'>" + lstDepartamentos[i].DeptNombre + "</option>";
                                    else
                                        opcionesListaDept = opcionesListaDept + "<option value='" + lstDepartamentos[i].DeptId + "'>" + lstDepartamentos[i].DeptNombre + "</option>";
                                }

                                string txt6 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    "<td><select onchange=\"llenarListaHijo('lst_Ciudad','lst_" + c.CampId + "')\" class=\"form-control\"" + obligatorio + " name = '" + c.CampId + "' id = 'lst_" + c.CampId + "'>" +
                                    opcionesListaDept +
                                    "</select></td>";

                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt6 = txt6 + "<td><center><a style=\"color:red\">" + capturaUNO + "</a></center></td>" +
                                        "<td><center><a style=\"color:blue\">" + capturaDOS + "</a></center></td>";
                                }

                                txt6 = txt6 + "</tr>";

                                cadenaCampos = cadenaCampos + txt6;
                                break;

                            //Lista de Selección de las Ciudades
                            case 7:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                Campos campCiudad = new Campos();
                                campCiudad.CampId = c.CampId;

                                string txt7 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    "<td><select class=\"form-control\"" + obligatorio + " name = '" + c.CampId + "' id = 'lst_Ciudad'>" +
                                    "</select></td>";

                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt7 = txt7 + "<td><center><a style=\"color:red\">" + capturaUNO + "</a></center></td>" +
                                        "<td><center><a style=\"color:blue\">" + capturaDOS + "</a></center></td>";
                                }

                                txt7 = txt7 + "</tr>";
                                cadenaCampos = cadenaCampos + txt7;
                                break;

                            //Campo Multilinea
                            case 8:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = " onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                campoOculto = "<td><span class=\"lbl_" + c.CampId + "\"></span></td>";

                                string txt8 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td></tr>" +
                                    "<tr><td><textarea class=\"form-control \"" + obligatorio + " " + javascriptFocus + " onblur=\"infoAdicional(this.value, 'lbl_" + c.CampId + "');\" maxlength='" +
                                    c.LongMax + "' name = '" + c.CampId + "' id='txt_" +
                                    c.CampId + "' Width = '" +
                                    c.CampAncho + "' onmousedown=ilegibleFaltante('txt_" + c.CampId + "',event)  />" + campoOculto + "</td>";

                                txt8 = txt8 + "</tr>";

                                cadenaCampos = cadenaCampos + txt8;
                                break;

                            //Campo CheckBox
                            case 11:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                //JFP; se agrega
                                if (!string.IsNullOrEmpty(c.ValidationFunction))
                                {
                                    javascriptFunction = c.ValidationFunction;
                                }
                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = " onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }
                                string txt11 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    //JFP; modifica
                                    //"<td><input type='checkbox' " + obligatorio + " name = '" +
                                    "<td><input type='checkbox'  " + obligatorio + javascriptFunction + javascriptFocus + inactivo +
                                    " name = '" + c.CampId + "' id='chk_" + c.CampId + "' Width = '" +
                                    c.CampAncho + "' /></td>";

                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt11 = txt11 + "<td><center><a style=\"color:red\">" + capturaUNO + "</a></center></td>" +
                                        "<td><center><a style=\"color:blue\">" + capturaDOS + "</a></center></td>";
                                }

                                txt11 = txt11 + "</tr>";

                                cadenaCampos = cadenaCampos + txt11;
                                break;

                            //Grilla de Posiciones - Control de Calidad
                            case 13:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;width: 100px;\"  align=\"center\"  ";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                int idEtapaPos = 0;
                                bool valorCCalidadPos = false;

                                if (idEtapa != null)
                                {
                                    idEtapaPos = Convert.ToInt32(idEtapa);
                                    valorCCalidadPos = Convert.ToBoolean(lstCampos[num].ControlCalidad);
                                }

                                //Validar si esta en control de calidad o en alguna de las capturas.
                                #region Grilla Control de Calidad Posiciones
                                if (valorCCalidadPos && idEtapaPos == 3)
                                {
                                    int idGruposDocumentosPos = Convert.ToInt32(lstCampos[num].GDocId);
                                    int negId = NegID;

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
                                                NegValor = "No Hay datos",
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
                                                Indice = listaCapturaDosPos[contador].CampId,
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

                                    string txt13 = "<h3>Grilla Control  de Calidad - Posiciones</h3>" +
                                        "<table data-work='idGrillaControlCaldImp'>" +
                                        "<tr>";

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
                                                        txt13 = txt13 + "</td></tr>";
                                                        if (nuevaFilaPos)
                                                        {
                                                            txt13 = txt13 + "<tr>";
                                                            nuevaFilaPos = false;
                                                        }

                                                        string nombreCampo = s.obtenerNomCampo(listaCapturaUnoPos[i].CampId);

                                                        txt13 = txt13 + "<td>" +
                                                            "&nbsp <b>Fila: " + listaCapturaUnoPos[i].Indice + " </b><br>" +
                                                            "&nbsp " + nombreCampo + " <br>";

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
                                                                txt13 = txt13 +
                                                                    "<input id=\"CapturaTres\" required maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Alfanúmerico
                                                            case 2:
                                                                txt13 = txt13 +
                                                                    "<input id=\"CapturaTres\" required maxlength=" + longMax + " runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Fecha
                                                            case 3:
                                                                txt13 = txt13 +
                                                                    "<input runat='server' required name=\"CapturaTres\" class=\"dp msk valFecha\" onkeypress=\"return bloqTeclado(event);\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Decimal
                                                            case 16:
                                                                txt13 = txt13 +
                                                                    "<input id=\"CapturaTres\" class=\"dec\" maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" required runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Check
                                                            case 11:
                                                                txt13 = txt13 +
                                                                    "<td><input id=\"ck" + contadorCheck + "\" name=\"ck" + contadorCheck + "\" onclick='asignarValorCheck(" + contadorCheck + ");' runat='server' type=\"checkbox\" value=\"1\" ></td>" +
                                                                    "<td><input id=\"CapturaTres" + contadorCheck + "\" name=\"CapturaTres\" runat='server' value=\"0\" type=\"hidden\" ></td>";
                                                                contadorCheck++;
                                                                break;

                                                            //Tipo de campo Lista desplegable
                                                            case 5:
                                                                List<CodigosCampo> ListaCodigosCampo = ws.consultarOpcionesLista(listaCapturaUnoPos[i].CampId);
                                                                txt13 = txt13 +
                                                                    "<select name=\"CapturaTres\" id=\"CapturaTres\" required>";
                                                                foreach (CodigosCampo campos in ListaCodigosCampo)
                                                                {
                                                                    txt13 = txt13 +
                                                                        "<Option value =" + campos.CodId + ">" + campos.CodDescripcion + "</Option>";
                                                                }
                                                                txt13 = txt13 + "</select>";
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

                                                            txt13 = txt13 + "<div style=\"color:Blue\">&nbsp Captura UNO: " + valorUNO + "</div>" +
                                                                "<div style=\"color:Red\">&nbsp Captura DOS: " + valorDOS + "</div><br>";
                                                        }
                                                        else
                                                        {
                                                            txt13 = txt13 + "<div style=\"color:Blue\">&nbsp Captura UNO: " + listaCapturaUnoPos[i].NegValor + "</div>" +
                                                                "<div style=\"color:Red\">&nbsp Captura DOS: " + listaCapturaDosPos[i].NegValor + "</div><br>";
                                                        }
                                                        txt13 = txt13 + "</td>";
                                                    }
                                                    else
                                                    {
                                                        string nombreCampo = s.obtenerNomCampo(listaCapturaUnoPos[i].CampId);

                                                        txt13 = txt13 + "<td>" +
                                                            "&nbsp <b>Fila: " + listaCapturaUnoPos[i].Indice + " </b><br>" +
                                                            "&nbsp " + nombreCampo + " <br>";

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
                                                                txt13 = txt13 +
                                                                    "<input id=\"CapturaTres\" required maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Alfanúmerico
                                                            case 2:
                                                                txt13 = txt13 +
                                                                    "<input id=\"CapturaTres\" required maxlength=" + longMax + " runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Fecha
                                                            case 3:
                                                                txt13 = txt13 +
                                                                    "<input runat='server' required name=\"CapturaTres\" class=\"dp msk valFecha\" onkeypress=\"return bloqTeclado(event);\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Decimal
                                                            case 16:
                                                                txt13 = txt13 +
                                                                    "<input id=\"CapturaTres\" class=\"dec\" maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" required runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Check
                                                            case 11:
                                                                txt13 = txt13 +
                                                                    "<td><input id=\"ck" + contadorCheck + "\" name=\"ck" + contadorCheck + "\" onclick='asignarValorCheck(" + contadorCheck + ");' runat='server' type=\"checkbox\" value=\"1\" ></td>" +
                                                                    "<td><input id=\"CapturaTres" + contadorCheck + "\" name=\"CapturaTres\" runat='server' value=\"0\" type=\"hidden\" ></td>";
                                                                contadorCheck++;
                                                                break;

                                                            //Tipo de campo Lista desplegable
                                                            case 5:
                                                                List<CodigosCampo> ListaCodigosCampo = ws.consultarOpcionesLista(listaCapturaUnoPos[i].CampId);
                                                                txt13 = txt13 + "<select name=\"CapturaTres\" id=\"CapturaTres\" required>";
                                                                foreach (CodigosCampo campos in ListaCodigosCampo)
                                                                {
                                                                    txt13 = txt13 + "<Option value =" + campos.CodId + ">" + campos.CodDescripcion + "</Option>";
                                                                }
                                                                txt13 = txt13 + "</select>";
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

                                                            txt13 = txt13 + "<div style=\"color:Blue\">&nbsp Captura UNO: " + valorUNO + "</div>" +
                                                                "<div style=\"color:Red\">&nbsp Captura DOS: " + valorDOS + "</div><br>";
                                                        }
                                                        else
                                                        {
                                                            txt13 = txt13 + "<div style=\"color:Blue\">&nbsp Captura UNO: " + listaCapturaUnoPos[i].NegValor + "</div>" +
                                                                "<div style=\"color:Red\">&nbsp Captura DOS: " + listaCapturaDosPos[i].NegValor + "</div><br>";
                                                        }
                                                        txt13 = txt13 + "</td>";
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
                                    txt13 = txt13 + "</td></tr><table>";
                                    System.Web.HttpContext.Current.Session["CamposCCalidad"] = listaCamposCapturadosPos;

                                    cadenaCampos = cadenaCampos + txt13;
                                    break;
                                }
                                #endregion
                                #region Grilla Capturas 1 y 2 Posiciones
                                else
                                {
                                    int valorGrillaPos = lstCampos[num].CampId;
                                    System.Web.HttpContext.Current.Session["Id_Grilla"] = valorGrillaPos;
                                    int idDocumentoPos = Convert.ToInt32(idDocumento);
                                    int idCaptura = Convert.ToInt32(noCaptura);


                                    //Obteniendo la lista de campos asociados al Id de la grilla seleccionada
                                    List<Campos> listaCampos = s.obtenerCamposGrilla(valorGrillaPos, idDocumentoPos);
                                    indiceGrid.grillaPosiciones = valorGrillaPos;

                                    //Pintar los titulos de la grilla
                                    string txt13 = "<h3>Ingresar Posiciones</h3>" +
                                        "<table data-work='idGrilla' id='" + lstCampos[num].CampDescripcion + "'>" +
                                        "<tr>" + "<td style = \"text-decoration: underline; font-weight: bold;width: 0px;\" ></td>";

                                    string _txt13 = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (string.IsNullOrEmpty(_txt13))
                                            _txt13 = "<td" + funcionNegrilla + ">" + campo.CampDescripcion.ToString() + "</td>";
                                        else
                                            _txt13 = _txt13 + "<td" + funcionNegrilla + ">" + campo.CampDescripcion.ToString() + "</td>";
                                    }

                                    txt13 = txt13 + _txt13;

                                    //Se crea la fila en donde van a ir pintados los campos que se crearan desde JavaScript
                                    txt13 = txt13 + "</tr>" +
                                        "<tr>" +
                                        "</tr>" +
                                        "</table>";

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
                                                    disponible = "1";
                                                else
                                                    disponible = "0";
                                            }

                                            if (idCaptura == 1)
                                                disponible = campo.CampNumCaptura.ToString();
                                        }
                                        else
                                        {
                                            nombreCampos = nombreCampos + "," + campo.NomBizagi;

                                            //Validamos si estamos en captura Uno o Captura Dos
                                            //En Captura 2 Es cuando comenzamos a deshabilitar
                                            if (idCaptura == 2)
                                            {
                                                if (campo.CampNumCaptura2 == true)
                                                    disponible = disponible + "," + "1";
                                                else
                                                    disponible = disponible + "," + "0";
                                            }

                                            if (idCaptura == 1)
                                                disponible = disponible + "," + campo.CampNumCaptura.ToString();
                                        }
                                    }

                                    //Para llenar los tipos de campos a pintar desde Javascript
                                    string tiposCampo = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (tiposCampo.Trim() == "")
                                            tiposCampo = campo.TcId.ToString();
                                        else
                                            tiposCampo = tiposCampo + "," + campo.TcId.ToString();
                                    }

                                    //Para concatenar todas las opciones de los campos que sean una lista desplegable
                                    string idCampos = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (idCampos.Trim() == "")
                                            idCampos = campo.CampId.ToString();
                                        else
                                            idCampos = idCampos + "," + campo.CampId.ToString();
                                    }

                                    //Para Crear las opciones de Maxima longitud
                                    string maxLongitud = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (maxLongitud.Trim() == "")
                                            maxLongitud = campo.LongMax.ToString();
                                        else
                                            maxLongitud = maxLongitud + "," + campo.LongMax.ToString();
                                    }

                                    //Para especificar si los campos son obligatorios o no.
                                    string campoObligatorio = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (campoObligatorio.Trim() == "")
                                            campoObligatorio = campo.CampObligatorio.ToString();
                                        else
                                            campoObligatorio = campoObligatorio + "," + campo.CampObligatorio.ToString();
                                    }
                                    //william Cicua

                                    string cordenadasX = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (cordenadasX.Trim() == "")
                                            cordenadasX = campo.PosX.ToString();
                                        else
                                            cordenadasX = cordenadasX + "," + campo.PosX.ToString();
                                    }

                                    string cordenadasY = "";
                                    foreach (Campos campo in listaCampos)
                                    {
                                        if (cordenadasY.Trim() == "")
                                            cordenadasY = campo.PosY.ToString();
                                        else
                                            cordenadasY = cordenadasY + "," + campo.PosY.ToString();
                                    }

                                    //***************************************************************************************************
                                    //Creado po Ivan Rodriguez metodo que envia los parametros de validacion de fecha para los campos de una grilla
                                    List<sp_ObtenerValidacionesFechasGrillas_Result> valFechaGr = consultas.sp_ObtenerValidacionesFechasGrillas(idDocumento).ToList();
                                    string operadorMingr = "";
                                    string operadorMaxgr = "";
                                    string fechaMinimagr = "";
                                    string fechaMaximagr = "";
                                    char[] MyChar = { ',' };
                                    string operadorMi = "", operadorMa = "";

                                    foreach (sp_ObtenerValidacionesFechasGrillas_Result fila in valFechaGr)
                                    {
                                        if (fila.descripcion.Equals("Minimo Años"))
                                        {
                                            operadorMingr += cambiarOperadorMayoMenor(fila.Operador) + ",";
                                            fechaMinimagr += fila.valor + ",";
                                        }
                                        else if (fila.descripcion.Equals("Maximo Años"))
                                        {
                                            operadorMaxgr += cambiarOperadorMayoMenor(fila.Operador) + ",";
                                            fechaMaximagr += fila.valor + ",";
                                        }
                                        else if (fila.descripcion.IndexOf("Parametrizar el minimo") != -1)
                                        {
                                            operadorMingr += cambiarOperadorMayoMenor(fila.Operador) + ",";
                                            fechaMinimagr += fila.valor + ",";
                                        }
                                        else
                                        {
                                            operadorMaxgr += cambiarOperadorMayoMenor(fila.Operador) + ",";
                                            fechaMaximagr += fila.valor + ",";
                                        }
                                    }
                                    operadorMingr = operadorMingr.TrimEnd(MyChar);
                                    operadorMaxgr = operadorMaxgr.TrimEnd(MyChar);
                                    fechaMinimagr = fechaMinimagr.TrimEnd(MyChar);
                                    fechaMaximagr = fechaMaximagr.TrimEnd(MyChar);
                                    //Fin cambio Ivan Rodriguez
                                    //*****************************************************************************************************
                                    
                                    //***************************************************************************************************
                                    //metodo que envia los parametros de validacion para los campos numericos minimos
                                    //Creado po Ivan Rodriguez  
                                    List<sp_ObtenerValidacionesMinimoCaracGrillas_Result> valMinNum = consultas.sp_ObtenerValidacionesMinimoCaracGrillas(idDocumento).ToList();
                                    string minimoCampo = "";

                                    foreach (sp_ObtenerValidacionesMinimoCaracGrillas_Result fila in valMinNum)
                                    {
                                        minimoCampo += fila.valor + ",";
                                    }
                                    minimoCampo = minimoCampo.TrimEnd(MyChar);

                                    //Fin cambio Ivan Rodriguez
                                    //*****************************************************************************************************

                                    //Crear el boton de agregar mas campos y se les envia cuantos campos son los nombres de los campos y el tipo de campo que se va a pintar.
                                    txt13 = txt13 +
                                        "</br><input type=\"button\" class=\"btn btn-login\" id=\"Crear\" value=\"Nuevo Registro\" onclick=\"agregarFila(" + listaCampos.Count + ",'" + nombreCampos + "','" + tiposCampo + "','" + lstCampos[num].CampDescripcion + "','" + disponible + "','" + idCampos + "','" + maxLongitud + "','" + campoObligatorio + "','" + operadorMingr + "','" + operadorMaxgr + "','" + fechaMinimagr + "','" + fechaMaximagr + "','" + cordenadasX + "','" + cordenadasY + "' ,'" + minimoCampo + "')\" />" +
                                        "&nbsp&nbsp<input type=\"button\" value=\"Clonar Registro\" id=\"agregarFila_btn\" class=\"btn btn-login\" onclick=\"addTableRow('" + lstCampos[num].CampDescripcion + "')\" /></br></br>";

                                    cadenaCampos = cadenaCampos + txt13;
                                }
                                #endregion
                                break;

                            //Grilla Impuestos - Control de Calidad
                            case 14:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                int idEtapaImp = 0;
                                bool valorCCalidad = false;
                                if (idEtapa != null)
                                {
                                    idEtapaImp = Convert.ToInt32(idEtapa);
                                    valorCCalidad = Convert.ToBoolean(lstCampos[num].ControlCalidad);
                                }

                                //Validamos si estamos o no en la etapa de Control de Calidad.
                                #region Grilla Control Calidad Impuestos
                                if (valorCCalidad && idEtapaImp == 3)
                                {
                                    int idGruposDocumentos = Convert.ToInt32(lstCampos[num].GDocId);
                                    int negId = NegID;

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

                                    string txt14 = "<h3>Grilla Control de Calidad - Impuestos</h3>" +
                                        "<table data-work='idGrillaControlCaldImp'>" +
                                        "<tr>";

                                    int contadorCheck = 1;
                                    for (int i = 0; i < listaCapturaUno.Count; i++)
                                    {
                                        if (listaCapturaUno[i].Indice == listaCapturaDos[i].Indice)
                                        {
                                            if (listaCapturaUno[i].CampId == listaCapturaDos[i].CampId)
                                            {
                                                if (listaCapturaUno[i].NegValor != listaCapturaDos[i].NegValor && listaCapturaUno[i].NegValor != " " && listaCapturaDos[i].NegValor != " ")
                                                {
                                                    //Pintar los campos que tienen valores diferentes con el Tooltip
                                                    //Extraer el titulo de los campos y sus diferencias en el tooltip
                                                    if (contadorFilas >= 6)
                                                    {
                                                        contadorFilas = 1;
                                                        txt14 = txt14 + "</td></tr>";
                                                        if (nuevaFila)
                                                        {
                                                            txt14 = txt14 + "<tr>";
                                                            nuevaFila = false;
                                                        }

                                                        string nombreCampo = s.obtenerNomCampo(listaCapturaUno[i].CampId);

                                                        txt14 = txt14 + "<td>" +
                                                            "&nbsp <b>Fila: " + listaCapturaUno[i].Indice + " </b><br>" +
                                                            "&nbsp " + nombreCampo + " <br>";

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
                                                                txt14 = txt14 +
                                                                    "<input id=\"CapturaTres\" required maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Alfanúmerico
                                                            case 2:
                                                                txt14 = txt14 +
                                                                    "<input id=\"CapturaTres\" required maxlength=" + longMax + " runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Fecha
                                                            case 3:
                                                                txt14 = txt14 +
                                                                    "<input runat='server' required name=\"CapturaTres\" class=\"dp msk valFecha\" onkeypress=\"return bloqTeclado(event);\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Decimal
                                                            case 16:
                                                                txt14 = txt14 +
                                                                    "<input id=\"CapturaTres\" class=\"dec\" maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" required runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Check
                                                            case 11:
                                                                txt14 = txt14 +
                                                                    "<td><input id=\"ck" + contadorCheck + "\" name=\"ck" + contadorCheck + "\" onclick='asignarValorCheck(" + contadorCheck + ");' runat='server' type=\"checkbox\" value=\"1\" ></td>" +
                                                                    "<td><input id=\"CapturaTres" + contadorCheck + "\" name=\"CapturaTres\" runat='server' value=\"0\" type=\"hidden\" ></td>";
                                                                contadorCheck++;
                                                                break;

                                                            //Tipo de campo Lista Desplegable
                                                            case 5:
                                                                List<CodigosCampo> ListaCodigosCampo = ws.consultarOpcionesLista(listaCapturaUno[i].CampId);
                                                                txt14 = txt14 + "<select name=\"CapturaTres\" id=\"CapturaTres\" required>";
                                                                foreach (CodigosCampo campos in ListaCodigosCampo)
                                                                {
                                                                    txt14 = txt14 + "<Option value =" + campos.CodId + ">" + campos.CodDescripcion + "</Option>";
                                                                }
                                                                txt14 = txt14 + "</select>";
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

                                                            txt14 = txt14 + "<div style=\"color:Blue\">&nbsp Captura UNO: " + valorUNO + "</div>" +
                                                                "<div style=\"color:Red\">&nbsp Captura DOS: " + valorDOS + "</div><br>";
                                                        }
                                                        else
                                                        {
                                                            txt14 = txt14 + "<div style=\"color:Blue\">&nbsp Captura UNO: " + listaCapturaUno[i].NegValor + "</div>" +
                                                                "<div style=\"color:Red\">&nbsp Captura DOS: " + listaCapturaDos[i].NegValor + "</div><br>";
                                                        }

                                                        txt14 = txt14 + "</td>";
                                                    }
                                                    else
                                                    {
                                                        string nombreCampo = s.obtenerNomCampo(listaCapturaUno[i].CampId);
                                                        txt14 = txt14 + "<td>" +
                                                            "&nbsp <b>Fila: " + listaCapturaUno[i].Indice + " </b><br>" +
                                                            "&nbsp " + nombreCampo + " <br>";

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
                                                                txt14 = txt14 +
                                                                    "<input id=\"CapturaTres\" required maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Alfanúmerico
                                                            case 2:
                                                                txt14 = txt14 +
                                                                    "<input id=\"CapturaTres\" required maxlength=" + longMax + " runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Fecha
                                                            case 3:
                                                                txt14 = txt14 +
                                                                    "<input runat='server' required name=\"CapturaTres\" class=\"dp msk valFecha\" onkeypress=\"return bloqTeclado(event);\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Decimal
                                                            case 16:
                                                                txt14 = txt14 +
                                                                    "<input id=\"CapturaTres\" class=\"dec\" maxlength=" + longMax + " onkeypress=\"return numbersonly(event);\" required runat='server' name=\"CapturaTres\" type=\"text\" /><br>";
                                                                break;

                                                            //Tipo de campo Check
                                                            case 11:
                                                                txt14 = txt14 +
                                                                    "<td><input id=\"ck" + contadorCheck + "\" name=\"ck" + contadorCheck + "\" onclick='asignarValorCheck(" + contadorCheck + ");' runat='server' type=\"checkbox\" value=\"1\" ></td>" +
                                                                    "<td><input id=\"CapturaTres" + contadorCheck + "\" name=\"CapturaTres\" runat='server' value=\"0\" type=\"hidden\" ></td>";
                                                                contadorCheck++;
                                                                break;

                                                            //Tipo de campo Lista Desplegable
                                                            case 5:
                                                                List<CodigosCampo> ListaCodigosCampo = ws.consultarOpcionesLista(listaCapturaUno[i].CampId);
                                                                txt14 = txt14 + "<select name=\"CapturaTres\" id=\"CapturaTres\" required>";
                                                                foreach (CodigosCampo campos in ListaCodigosCampo)
                                                                {
                                                                    txt14 = txt14 +
                                                                        "<Option value =" + campos.CodId + ">" + campos.CodDescripcion + "</Option>";
                                                                }
                                                                txt14 = txt14 + "</select>";
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

                                                            txt14 = txt14 + "<div style=\"color:Blue\">&nbsp Captura UNO: " + valorUNO + "</div>" +
                                                                "<div style=\"color:Red\">&nbsp Captura DOS: " + valorDOS + "</div><br>";
                                                        }
                                                        else
                                                        {
                                                            txt14 = txt14 +
                                                                "<div style=\"color:Blue\">&nbsp Captura UNO: " + listaCapturaUno[i].NegValor + "</div>" +
                                                                "<div style=\"color:Red\">&nbsp Captura DOS: " + listaCapturaDos[i].NegValor + "</div><br>";
                                                        }

                                                        txt14 = txt14 + "</td>";
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
                                    txt14 = txt14 + "</td></tr><table>";
                                    System.Web.HttpContext.Current.Session["CamposCCalidad"] = listaCamposCapturados;
                                    cadenaCampos = cadenaCampos + txt14;
                                }
                                #endregion
                                #region Grilla Capturas 1 y 2 Impuestos
                                else
                                {
                                    int valorGrillaImp = lstCampos[num].CampId;
                                    System.Web.HttpContext.Current.Session["Id_Grilla"] = valorGrillaImp;
                                    int idDocumentoImp = Convert.ToInt32(idDocumento);
                                    int idCaptura = Convert.ToInt32(noCaptura);

                                    List<Campos> listaCImpuestos = s.obtenerCamposGrilla(valorGrillaImp, idDocumentoImp);
                                    indiceGrid.grillaImpuestos = valorGrillaImp;

                                    //Pintar los titulos de la grilla
                                    string txt14 = "<h3>Ingresar Impuestos</h3>" +
                                        "<table data-work='idGrilla' id='" + lstCampos[num].CampDescripcion + "'>" +
                                        "<tr>" + "<td></td>";

                                    string _txt14 = "";
                                    foreach (Campos campo in listaCImpuestos)
                                    {
                                        if (string.IsNullOrEmpty(_txt14))
                                            _txt14 = "<td" + funcionNegrilla + ">" + campo.CampDescripcion.ToString() + "</td>";
                                        else
                                            _txt14 = _txt14 + "<td" + funcionNegrilla + ">" + campo.CampDescripcion.ToString() + "</td>";
                                    }

                                    txt14 = txt14 + _txt14;

                                    //Se crea la fila en donde van a ir pintados los campos que se crearan desde JavaScript
                                    txt14 = txt14 + "</tr>" +
                                        "<tr>" +
                                        "</tr>" +
                                        "</table>";

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

                                    //Crear el boton de agregar mas campos y se les envia cuantos campos son los nombres de los campos y el tipo de campo que se va a pintar.
                                    txt14 = txt14 +
                                        "</br><input type=\"button\" class=\"btn btn-login\" id=\"Crear\" value=\"Nuevo Registro\" onclick=\"agregarFila(" + listaCImpuestos.Count + ",'" + nombreCamposImp + "','" + tiposCampoImp + "','" + lstCampos[num].CampDescripcion + "','" + disponible + "','" + idCampos + "','" + maxLongitud + "','" + campoObligatorio + "')\" />" +
                                        "&nbsp&nbsp<input type=\"button\" value=\"Clonar Registro\" id=\"agregarFila_btn\" class=\"btn btn-login\" onclick=\"addTableRow('" + lstCampos[num].CampDescripcion + "')\" /></br></br>";

                                    cadenaCampos = cadenaCampos + txt14;
                                }
                                #endregion
                                break;

                            //Campo valor Decimal
                            case 16:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                if (!String.IsNullOrEmpty(c.ValidationFunction))
                                    funcionJavascript = c.ValidationFunction;

                                string txt16 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    "<td><input type='Text' class=\"form-control \" " + obligatorio + " " + javascriptFocus + " "+ inactivo+" onkeypress='return numbersonly(event);' onblur='infoAdicional(this.value, '_lbl" + c.CampId + "');' maxlength='" +
                                    c.LongMax + "' name = '" + c.CampId + "' id='txt_" +
                                    c.CampId + "' Width = '" +
                                    c.CampAncho + "'" + funcionJavascript + " onmousedown=ilegibleFaltante('txt_" + c.CampId + "',event)  onchange='decimal(txt_" + c.CampId + ")'  onKeyUp=\"validarCaracteres('txt_" + c.CampId + "')\" /></td>";

                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt16 = txt16 + "<td><center>" +
                                        "<a style=\"color:red\" onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt1_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaUNO + "</a>" + "<input type=\"hidden\" id=\"cpt1_" + c.CampId + "\" value=" + capturaUNO + " />" +
                                        "</center></td><td><center>" +
                                        "<a style=\"color:blue\" onmouseover=\"this.style.cursor='pointer'\" onclick=\"CopiarValor('cpt2_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaDOS + "</a></center></td>" + "<input type=\"hidden\" id=\"cpt2_" + c.CampId + "\" value=" + capturaDOS + " />";
                                }

                                txt16 = txt16 + "</tr>";

                                cadenaCampos = cadenaCampos + txt16;
                                break;
                            //=========================================================================================================
                            //Campo Texto
                            //JFPancho;05Mayo2017; se crea nuevo campo que solo permite la entrada de minúsculas, mayúsculas y Ññ, "-"
                            case 17:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"text-decoration: underline; font-weight: bold;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                //JFP; se agrega
                                if (!string.IsNullOrEmpty(c.ValidationFunction))
                                {
                                    javascriptFunction = c.ValidationFunction;
                                }

                                string txt_cadena = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.CampDescripcion + "</td>" +
                                    //"<td><input class=\"form-control cmenu1\" type='Text' " + obligatorio + " " + javascriptFocus + " onblur=\"infoAdicional(this.value, 'lbl_" + c.CampId + "');\" maxlength='" +
                                    "<td><input class=\"form-control \" type='Text' pattern='[A-Za-z]+' " + obligatorio + " " + javascriptFocus + javascriptFunction + inactivo + " maxlength='" + //JFP; se agrega
                                    c.LongMax + "' name = '" + c.CampId + "' id='txt_" +
                                    c.CampId + "' Width = '" +
                                    c.CampAncho + "' onmousedown = ilegibleFaltante('txt_" + c.CampId + "',event) onclick=\"validaTexto('txt_" + c.CampId + "')\">" + campoOculto + "</td>";

                                if (lstRespAnt != null)
                                {
                                    string capturaUNO = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP1).First().ToString() : "");
                                    string capturaDOS = ((lstRespAnt.Any(R => R.CAMPO == c.CampId)) ? (from t in lstRespAnt where t.CAMPO == c.CampId select t.RESP2).First().ToString() : "");

                                    txt_cadena = txt_cadena + "<td><center>" +
                                        "<a style=\"color:red\" onmouseover=\"this.style.cursor='pointer'\" onblur=\"validaTexto('txt_" + c.CampId + "')\" onclick=\"CopiarValor('cpt1_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaUNO + "</a>" + "<input type=\"hidden\" id=\"cpt1_" + c.CampId + "\" value=" + capturaUNO + " />" +
                                        "</center></td><td><center>" +
                                        "<a style=\"color:blue\" onmouseover=\"this.style.cursor='pointer'\" onblur=\"validaTexto('txt_" + c.CampId + "')\" onclick=\"CopiarValor('cpt2_" + c.CampId + "','txt_" + c.CampId + "')\">" + capturaDOS + "</a></center></td>" + "<input type=\"hidden\" id=\"cpt2_" + c.CampId + "\" value=" + capturaDOS + " />";
                                }

                                txt_cadena = txt_cadena + "</tr>";

                                cadenaCampos = cadenaCampos + txt_cadena;
                                break;
                            //=========================================================================================================
                        }
                        num++;
                    }
                }

                string tablaContenido = "<table class=\"TablaFormulario\">";

                if (num < 1 && noCaptura.Equals("3"))
                {
                    return "";
                }
                
                if (String.IsNullOrEmpty(cadenaCampos))
                {
                    return "";
                }
                
                if (lstRespAnt != null && lstRespAnt.Count > 0)
                {
                    string ccCampos = "<tr>" +
                        "<td></td><td></td>" +
                    "<td><center><b><a style=\" color:red \">Datos Captura Uno</a></b></center></td>" +
                    "<td><center><b><a style=\" color:blue \">Datos Captura Dos</a></b></center></td>" +
                        "</tr>";
                    return tablaContenido + ccCampos + cadenaCampos + "</table>";
                }

                return "<table class=\"TablaFormulario\">" + cadenaCampos + "</table>";
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo GenerarCampos " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// JFPancho; Validacion Dcoumental; abril/2016
        /// se crea funcion que pinta dinamicamente los campos para el  modulo VALIDACION DOCUMENTAL
        /// </summary>
        /// <param name="Ta">La Tabla donde se almacenaran la descripcion de los campos</param>
        /// <param name="lstCausales">La lista de los campos a generar para el formulario</param>
        /// <param name="idGrupoDocumentos">El ID del grupo de documento asociado para pintar los campos</param>
        /// <param name="idEtapa"></param>
        /// <param name="idDocumento">El documento actual que se va a pintar</param>
        /// <param name="NegID"></param>
        /// <returns>Cadena de texto con formato HTML para pintar con los campos generados</returns>
        public string GenerarCamposValDoc(Table Ta, List<tvalDoc_Causales> lstCausales, int idGrupoDocumentos, int idEtapa, int idDocumento, int NegID, int Modulo)
        {
            Panel pControls = new Panel();
            string cadenaCampos = "";
            try
            {
                int num = 0;
                Table child = new Table
                {
                    ID = "TBL_PRICIPAL" + idGrupoDocumentos,
                    EnableViewState = true
                };

                pControls.Controls.Add(child);
                if (idEtapa == 50)
                    child.Attributes.Add("data-work", "tablaPrincipalControlCalidad");
                else
                    child.Attributes.Add("data-work", "tablaPrincipal");

                using (List<tvalDoc_Causales>.Enumerator enumerator = lstCausales.GetEnumerator())
                {
                    tvalDoc_Causales c;

                    //modifica Camilo Padilla; junio-2016
                    List<spValDoc_ObtenerCausalesRegistradas_Result> lstExistentes = consultas.spValDoc_ObtenerCausalesRegistradas(NegID,idDocumento,Modulo).ToList();

                    while (enumerator.MoveNext())
                    {
                        c = enumerator.Current;
                        TableRow row = new TableRow();
                        row.ID = "Trow_" + c.cod_causal;

                        TableCell cell = new TableCell();
                        cell.ID = "TcellDesc_" + c.cod_causal;

                        TableCell cell2 = new TableCell();
                        cell2.ID = "TcellCtrl_" + c.cod_causal;

                        TableCell cell3 = new TableCell();
                        cell3.ID = "TcellError_" + c.cod_causal;

                        row.Cells.Add(cell);
                        row.Cells.Add(cell2);
                        row.Cells.Add(cell3);


                        ///***************************************************************************************************

                        child.Rows.Add(row);
                        WebServiceModel s = new WebServiceModel();
                        IndicesGrillas indiceGrid = new IndicesGrillas();

                        string obligatorio = "";
                        string campBloqueado = "";
                        string funcionJavascript = "";
                        string funcionNegrilla = "";
                        string javascriptFocus = "";
                        string campoOculto = "";
                        string javascriptFechas = "";

                        switch (lstCausales[num].TcId)
                        {
                            //Textbox - Solo acepta Numeros
                            case 1:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:bolder;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                campoOculto = "<td><span class=\"lbl_" + c.cod_causal + "\"></span></td>";

                                string txt1 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.nom_causal + "</td>" +
                                    "<td><input class=\"form-control \" type='Text' " + obligatorio + " " + javascriptFocus + "   onkeypress='return numbersonly(event);' onblur='infoAdicional(this.value, '_lbl" + c.cod_causal + "');' maxlength='" +
                                    c.LongMax + "' name = '" + c.cod_causal + "' id='txt_" +
                                    c.cod_causal + "' Width = '" +
                                    c.CampAncho + "' />" + campoOculto + "</td>";

                                txt1 = txt1 + "</tr>";

                                cadenaCampos = cadenaCampos + txt1;
                                break;

                            //Textbox - Campo Alfanumerico
                            case 2:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:bolder;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                string txt2 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.nom_causal + "</td>" +
                                    "<td><input class=\"form-control \" type='Text' " + obligatorio + " " + javascriptFocus + "  onblur=\"infoAdicional(this.value, 'lbl_" + c.cod_causal + "');\" maxlength='" +
                                    c.LongMax + "' name = '" + c.cod_causal + "' id='txt_" +
                                    c.cod_causal + "' Width = '" +
                                    c.CampAncho + "' />" + campoOculto + "</td>";


                                txt2 = txt2 + "</tr>";

                                cadenaCampos = cadenaCampos + txt2;
                                break;

                            //Textbox - Fecha
                            case 3:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:bolder;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                string txt3 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.nom_causal + "</td>" +
                                    "<td><input class=\"form-control dpms \" type='Text'" + obligatorio + " " + javascriptFocus + " " + javascriptFechas + " maxlength='" +
                                    c.LongMax + "' name = '" + c.nom_causal + "' id='txt_" +
                                    c.cod_causal + "' Width = '" +
                                    c.CampAncho + "' onblur=\"validaFechaDDMMAAAA(this.value,this.id)\"/></td>";

                                txt3 = txt3 + "</tr>";

                                cadenaCampos = cadenaCampos + txt3;
                                break;

                            //Textbox - Hora
                            case 4:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:bolder;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                string txt4 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.nom_causal + "</td>" +
                                    "<td><input class=\"form-control hpms \" type='Text'" + obligatorio + " " + javascriptFocus + " maxlength='" +
                                    c.LongMax + "' name = '" + c.cod_causal + "' id='txt_" +
                                    c.cod_causal + "' Width = '" +
                                    c.CampAncho + "' /></td>";

                                txt4 = txt4 + "</tr>";

                                cadenaCampos = cadenaCampos + txt4;
                                break;

                            //Listas de seleccion
                            case 5:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:inherit; font-size: small; \"";
                                }
                                else
                                {
                                    funcionNegrilla = " style = \"font-weight:normal;\"";
                                }

                                Boolean a = c.Bloqueado.Value;

                                if (a)
                                {
                                    campBloqueado = " disabled ";
                                }
                                else
                                {
                                    campBloqueado = "";
                                }

                                tvalDoc_Causales camp = new tvalDoc_Causales();
                                camp.cod_causal = c.cod_causal;
                                string opcionesLista = "";

                                string txt5;
                                string javascriptPadre = "";
                                List<spConsultarHijos_Result> campoHijo = consultas.spConsultarHijos(c.cod_causal).ToList();
                                if (campoHijo.Count > 0)
                                {
                                    int _campoHijo = Convert.ToInt32(campoHijo[0].CampId);
                                    javascriptPadre = "onchange=\"llenarListasPadreHijo('lst_" + _campoHijo + "','lst_" + c.cod_causal + "')\"";
                                }
                                else
                                    javascriptPadre = "";


                                List<CodigosCampo> lstCodigos = bCampos.obtenerCodigosCampoVD(camp);
                                for (int i = 0; i < lstCodigos.Count; i++)
                                {
                                    if (obligatorio == " required " && i == 0)
                                    {
                                        //opcionesLista = "<option value=''>" + lstCodigos[i].CodDescripcion + "</option>";
                                        opcionesLista = "<option value='-1'>" + lstCodigos[i].CodDescripcion + "</option>";
                                    }
                                    else if (opcionesLista == "")
                                        opcionesLista = "<option value='" + lstCodigos[i].CodId + "'>" + lstCodigos[i].CodDescripcion + "</option>";
                                    else
                                    {
                                        //modifica Camilo Padilla; junio-2016
                                        //opcionesLista = opcionesLista + "<option value='" + lstCodigos[i].CodId + "'>" + lstCodigos[i].CodDescripcion + "</option>";
                                        
                                        if (lstExistentes.Count > 0 && lstExistentes[num].sn_causal + "" == lstCodigos[i].CodId && lstExistentes[num].cod_Causal == lstCausales[num].cod_causal)
                                        {
                                            opcionesLista = opcionesLista + "<option value='" + lstCodigos[i].CodId + "' selected >" + lstCodigos[i].CodDescripcion + "</option>";
                                        }
                                        else
                                        {
                                            opcionesLista = opcionesLista + "<option value='" + lstCodigos[i].CodId + "'>" + lstCodigos[i].CodDescripcion + "</option>";
                                        }
                                    }
                                        
                                }

                                txt5 = "<tr>" +
                                "<td" + funcionNegrilla + ">" + c.nom_causal + "</td>" +
                                    //JFP; modifica: se agrega estilo para el ancho de las listas; mayo/2016
                                    //"<td><select " + javascriptPadre + " class=\"form-control\"" + obligatorio + " " + campBloqueado + " name = '" + c.cod_causal + "' id = 'lst_" + c.cod_causal + "'>" +
                                "<td><select " + javascriptPadre + " class=\"form-control\"" + obligatorio + " " + campBloqueado + " name = '" + c.cod_causal + "' id = 'lst_" + c.cod_causal + "' style=\"width:90px;height: 25px; font-size: x-small;\">" +
                                opcionesLista +
                                "</select></td>";

                                txt5 = txt5 + "</tr>";

                                cadenaCampos = cadenaCampos + txt5;
                                break;

                            //Lista de Selección de Departamentos
                            //no va para validacion documental

                            //Lista de Selección de las Ciudades
                            //no va para validacion documental

                            //Campo Multilinea
                            case 8:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:bolder;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }

                                campoOculto = "<td><span class=\"lbl_" + c.cod_causal + "\"></span></td>";

                                string txt8 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.nom_causal + "</td></tr>" +
                                    "<tr><td><textarea class=\"form-control \"" + obligatorio + " " + javascriptFocus + "  onblur=\"infoAdicional(this.value, 'lbl_" + c.cod_causal + "');\" maxlength='" +
                                    c.LongMax + "' name = '" + c.cod_causal + "' id='txt_" +
                                    c.cod_causal + "' Width = '" +
                                    c.CampAncho + "' />" + campoOculto + "</td>";

                                txt8 = txt8 + "</tr>";

                                cadenaCampos = cadenaCampos + txt8;
                                break;

                            //Campo CheckBox
                            case 11:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:bolder;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                string txt11 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.nom_causal + "</td>" +
                                    "<td><input type='checkbox' " + obligatorio + " name = '" +
                                    c.cod_causal + "' id='chk_" + c.cod_causal + "' Width = '" +
                                    c.CampAncho + "' /></td>";

                                txt11 = txt11 + "</tr>";

                                cadenaCampos = cadenaCampos + txt11;
                                break;

                            //Grilla de Posiciones - Control de Calidad
                            case 13:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:bolder;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                int idEtapaPos = 0;
                                bool valorCCalidadPos = false;

                                //Validar si esta en control de calidad o en alguna de las capturas.
                                //no va para validacion documental

                                break;

                            //Grilla Impuestos - Control de Calidad
                            case 14:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:bolder;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                int idEtapaImp = 0;
                                bool valorCCalidad = false;

                                break;

                            //Campo valor Decimal
                            case 16:
                                if (c.CampObligatorio)
                                {
                                    obligatorio = " required ";
                                    funcionNegrilla = " style = \"font-weight:bolder;\"";
                                }
                                else
                                    funcionNegrilla = " style = \"font-weight:normal;\"";

                                if (c.PosX != null && c.PosY != null)
                                {
                                    javascriptFocus = "onfocus=\"scrollVisor(" + c.PosX + "," + c.PosY + ")\"";
                                }


                                string txt16 = "<tr>" +
                                    "<td" + funcionNegrilla + ">" + c.nom_causal + "</td>" +
                                    "<td><input type='Text' class=\"form-control dec \" " + obligatorio + " " + javascriptFocus + " maxlength='" +
                                    c.LongMax + "' name = '" + c.cod_causal + "' id='txt_" +
                                    c.cod_causal + "' Width = '" +
                                    c.CampAncho + "'" + funcionJavascript + " /></td>";


                                txt16 = txt16 + "</tr>";

                                cadenaCampos = cadenaCampos + txt16;
                                break;
                        }
                        num++;
                    }
                }

                return "<table class=\"TablaFormulario table table-striped\">" + cadenaCampos + "</table>";
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CapturaInformacion.aspx metodo GenerarCampos " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
        }
        public string cambiarOperadorMayoMenor(string operador)
        {
            if (operador.Equals(">"))
            {
                return "mayorQue";
            }
            else return "menorQue";

        }
    }
}