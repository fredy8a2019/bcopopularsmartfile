using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using GestorDocumental.Models;

namespace GestorDocumental
{
    public partial class ConsultaDocsNegocios : System.Web.UI.Page
    {
        private GestorDocumental.Controllers.ReportesController  _breporte;


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this._breporte = new GestorDocumental.Controllers.ReportesController();
            List<ConsultaPorCedula_Result> lst = this._breporte.ConsultaPorCedula((Clientes)this.Session["CLIENTE"], this.txtCedula.Text);
            if (lst.Count == 1)
            {
                this.lblError.Text = "No existen negocios asociados al numero de cedula digitado";
                this.lstNegocios.SelectedIndex = 0;
                this.lstNegocios.Enabled = false;
                this.lstDocs.Enabled = false;
                this.UpdatePanel3.Update();
                this.UpdatePanel2.Update();
                this.UpdatePanel4.Update();
            }
            else
            {
                this.CargarNegocios(lst);
                this.lstNegocios.Enabled = true;
                this.lstDocs.Enabled = true;
            }
        }

        private void CargarNegocios(List<ConsultaPorCedula_Result> lst)
        {
            try
            {
                this.lstNegocios.DataSource = lst;
                this.lstNegocios.DataTextField = "Nombre";
                this.lstNegocios.DataValueField = "NegId";
                this.lstNegocios.DataBind();
                this.UpdatePanel3.Update();
            }
            catch (Exception exception)
            {
                this.lblError.Text = exception.Message;
                this.UpdatePanel2.Update();
            }
        }

        protected void lstNegocios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstNegocios.SelectedIndex > 0)
            {
                this._breporte = new GestorDocumental.Controllers.ReportesController();
                Captura cap = new Captura
                {
                    NegId = int.Parse(this.lstNegocios.SelectedValue)
                };
                this.lstDocs.DataSource = this._breporte.DocumentosNegocio(cap);
                this.lstDocs.DataTextField = "DocDescripcion";
                this.lstDocs.DataValueField = "DocId";
                this.lstDocs.DataBind();
                this.UpdatePanel4.Update();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["TITULO"] = "";
            if (!base.IsPostBack)
            {
            }
            ((Label)base.Master.FindControl("lblNegocio")).Text = "Consulta por cedula";
            this.lstNegocios.Attributes.Add("onchange", "CargarDocumento();");
            this.lstDocs.Attributes.Add("onchange", "CargarPagina();");
            this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "FileLoad", "path = '" + ConfigurationManager.AppSettings["ClientFiles"] + "';", true);
            this.lblError.Text = "";
            this.UpdatePanel2.Update();
        }

        protected void lstDocs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}