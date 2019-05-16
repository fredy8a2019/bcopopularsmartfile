using System;
using System.Web.UI.WebControls;
using GestorDocumental.Controllers;
using Microsoft.Reporting.WebForms;

using System.Linq;
using GestorDocumental.Models;

namespace GestorDocumental.ViewsAspx
{
    public partial class ReporteContabilizacion : System.Web.UI.Page
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
            var result = data.spValidaAccesoModulo(idRol, "/ViewsAspx/ReporteContabilizacion.aspx").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            if (Session["CLIENTE"] != null)
            {
                this.Title = "Reporte Contabilización";
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
            this.ReportViewer1.LocalReport.ReportPath = base.Server.MapPath("../Reportes/ReporteContabilizacion.rdlc");
            item.Name = "DataSet1";

            //Busca por codigo de barras
            if ((txtCodigoBarras.Value != "") /*|| (txtCodigoBarras.Value != null)*/)
            {
                item.Value = this.breport.ReporteContabilizacion(null, null, null,
                                                           null, null, null, null,
                                                           null, txtCodigoBarras.Value, null);
            }
            //Busca por No documento
            else if (txtNoDocumento.Value != string.Empty /*|| txtNoDocumento.Value != null*/)
            {
                item.Value = this.breport.ReporteContabilizacion(null, null, null,
                                                          null, null, null, null,
                                                          null, null, txtNoDocumento.Value);
            }
            //Busca por  los demas filtros
            else
            {
                item.Value = this.breport.ReporteContabilizacion(txtFechaIni.Text, txtFechaFin.Text,
                                                           validCampo(hClientes.Value),
                                                           validCampo(hOficinas.Value),
                                                           validCampo(hProductos.Value),
                                                           validCampo(hSubProductos.Value),
                                                           validCampo(txtProveedor.Value),
                                                           validCampo(hSociedad.Value), null, null);
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