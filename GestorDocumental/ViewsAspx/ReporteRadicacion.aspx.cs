using System;
using System.Web.UI.WebControls;
using GestorDocumental.Controllers;
using Microsoft.Reporting.WebForms;

using System.Linq;
using GestorDocumental.Models;

namespace GestorDocumental.ViewsAspx
{
    public partial class ReporteRadicacion : System.Web.UI.Page
    {
        private ReportesController breport;
        GestorDocumentalEnt data = new GestorDocumentalEnt();
        protected void Page_Load(object sender, EventArgs e)
        {
            //<<JFPancho;6-abril-2017;  
            //---valida que el usuario no este activo en mas de una máquina
            LogUsuarios x = new LogUsuarios();
            x.ActualizaSesion(((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
            //---valida si el usuario logueado tiene accceso al modulo
            int? idRol = ((Usuarios)Session["USUARIO_LOGUEADO"]).RolId;
            var result = data.spValidaAccesoModulo(idRol, "/ViewsAspx/ReporteRadicacion.aspx").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            if (Session["CLIENTE"] != null)
            {
                this.Title = "Reporte Radicación";
                ((Label)base.Master.FindControl("lblNegocio")).Text = "";
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
            }
        }

        protected void tnGenerar_Click(object sender, EventArgs e)
        {
            this.breport = new ReportesController();
            ConsultaDocsNegocios negocios = new ConsultaDocsNegocios();
            ReportDataSource item = new ReportDataSource();
            this.ReportViewer1.LocalReport.ReportPath = base.Server.MapPath("../Reportes/Radicacion.rdlc");
            item.Name = "DataSet1";

            //Busca por codigo de barras
            if ((txtCodigoBarras.Value != "") /*|| (txtCodigoBarras.Value != null)*/)
            {
                item.Value = this.breport.ReporteRadicacion(null, null, null,
                                                           null, null, null, null,
                                                           null, null, null, null, txtCodigoBarras.Value, null);
            }
            //Busca por No documento
            else if (txtNoDocumento.Value != string.Empty /*|| txtNoDocumento.Value != null*/)
            {
                item.Value = this.breport.ReporteRadicacion(null, null, null,
                                                          null, null, null, null,
                                                          null, null, txtNoDocumento.Value, null, null, null);
            }
            //Busca por neg id
            else if (txtNegId.Value != string.Empty)
            {
                item.Value = this.breport.ReporteRadicacion(null, null, null,
                                                        null, null, null, null,
                                                        null, null, null, null, null, txtNegId.Value);
            }
            //Busca por causal
            else if (txtCausal.Value != string.Empty)
            {
                item.Value = this.breport.ReporteRadicacion(null, null, null,
                                                        null, null, null, null,
                                                        null, null, null, txtCausal.Value, null, null);
            }
            //Busca por  los demas filtros
            else
            {
                item.Value = this.breport.ReporteRadicacion(txtFechaIni.Text, txtFechaFin.Text,
                                                           validCampo(hClientes.Value),
                                                           validCampo(hOficinas.Value),
                                                           validCampo(hProductos.Value),
                                                           validCampo(hSubProductos.Value),
                                                           validCampo(hEstados.Value),
                                                           validCampo(hSociedad.Value),
                                                           validCampo(txtProveedor.Value), null, null, null, null);
            }

            this.ReportViewer1.LocalReport.EnableHyperlinks = true;
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(item);
            this.ReportViewer1.LocalReport.Refresh();

        }

        public string validCampo(string valor)
        {
            if (valor == string.Empty)
            {
                valor = null;
            }
            return valor;
        }

        protected void ReportViewer1_Drillthrough(object sender, DrillthroughEventArgs e)
        {

        }

    }
}