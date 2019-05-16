using System;
using System.Web.UI.WebControls;
using GestorDocumental.Controllers;
using Microsoft.Reporting.WebForms;
using GestorDocumental;

using System.Linq;
using GestorDocumental.Models;

namespace GestorDocumental.ViewsAspx
{
    public partial class ReporteRecepcion : System.Web.UI.Page
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
            var result = data.spValidaAccesoModulo(idRol, "/ViewsAspx/ReporteRecepcion.aspx").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>

            if (Session["CLIENTE"] != null)
            {
                this.Title = "Reporte Recepción";
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
            this.ReportViewer1.LocalReport.ReportPath = base.Server.MapPath("../Reportes/Recepcion.rdlc");
            item.Name = "DataSet1";

            if (txtLote.Value != "")
            {
                item.Value = this.breport.getSpReporteRecepcion(null, null, null, null, null, null, null, txtLote.Value);
            }
            else
            {
                item.Value = this.breport.getSpReporteRecepcion(txtFechaIni.Text, txtFechaFin.Text,
                                                                 validCampo(hClientes.Value),
                                                                 validCampo(hOficinas.Value),
                                                                 validCampo(hProductos.Value),
                                                                 validCampo(hSubProductos.Value),
                                                                 validCampo(hEstado.Value), null);
            }

            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(item);
        }

        public string validCampo(string valor)
        {
            if (valor == string.Empty)
            {
                valor = null;
            }
            return valor;
        }
    }
}