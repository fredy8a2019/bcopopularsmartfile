using System;
using System.Web.UI.WebControls;
using GestorDocumental.Controllers;
using Microsoft.Reporting.WebForms;

using System.Linq;
using GestorDocumental.Models;

namespace GestorDocumental.ViewsAspx
{
    public partial class ReporteResumenNegocios : System.Web.UI.Page
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
            var result = data.spValidaAccesoModulo(idRol, "/ViewsAspx/ReporteResumenNegocios.aspx").FirstOrDefault();

            if (result == 0)
            {
                Response.Redirect("../Home/Index");
            }
            //JFPancho >>
            if (Session["CLIENTE"] != null)
            {
                ((Label)base.Master.FindControl("lblNegocio")).Text = "Resumen negocios";
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
            }
        }

        protected void tnGenerar_Click(object sender, EventArgs e)
        {
            this.breport = new ReportesController();
            ReportDataSource item = new ReportDataSource();
            this.ReportViewer1.LocalReport.ReportPath = base.Server.MapPath("../Reportes/ReporteResumenNegocios.rdlc");
            item.Name = "DataSet1";
            //item.Value = this.breport.ReporteEstadosNegocios(Convert.ToDateTime(this.txtFechaIni.Text), Convert.ToDateTime(this.txtFechaFin.Text));
            item.Value = this.breport.ReporteEstadosNegocios((DateTime.Now.AddYears(-1)), DateTime.Now);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(item);
            this.ReportViewer1.LocalReport.Refresh();
        }
    }
}