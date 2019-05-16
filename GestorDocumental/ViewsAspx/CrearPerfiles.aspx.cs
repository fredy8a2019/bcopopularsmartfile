using System;
using System.Web.UI.WebControls;
using GestorDocumental.Models;

namespace GestorDocumental.ViewsAspx
{
    public partial class CrearPerfiles : System.Web.UI.Page
    {
        private GestorDocumental.Controllers.PerfilesController bRoles;


        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                this.bRoles = new GestorDocumental.Controllers.PerfilesController();
                P_Roles r = new P_Roles
                {
                    DescRol = this.txtDescPerfil.Text
                };
                this.bRoles.InsetarRol(r);
                this.cargarRoles();
                this.UpdatePanel3.Update();
            }
            catch (Exception exception)
            {
                this.lblerror.Text = exception.Message;
                this.UpdatePanel4.Update();
            }
        }

        private void cargarRoles()
        {
            try
            {
                this.bRoles = new GestorDocumental.Controllers.PerfilesController();
                this.gvRoles.DataSource = this.bRoles.obtenerRoles();
                this.gvRoles.DataBind();
            }
            catch (Exception exception)
            {
                this.lblerror.Text = exception.Message;
                this.UpdatePanel4.Update();
            }
        }

        protected void gvRoles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.gvRoles.EditIndex = -1;
            this.cargarRoles();
        }

        protected void gvRoles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.gvRoles.EditIndex = e.NewEditIndex;
            this.cargarRoles();
        }

        protected void gvRoles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                this.bRoles = new GestorDocumental.Controllers.PerfilesController();
                P_Roles r = new P_Roles();
                GridViewRow row = this.gvRoles.Rows[e.RowIndex];
                r.DescRol = ((TextBox)row.Cells[1].Controls[1]).Text;
                r.RolId = int.Parse(((Label)row.Cells[0].Controls[1]).Text);
                this.bRoles.InsetarRol(r);
                this.gvRoles.EditIndex = -1;
                this.cargarRoles();
            }
            catch (Exception exception)
            {
                this.lblerror.Text = exception.Message;
                this.UpdatePanel4.Update();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CLIENTE"] != null)
            {
                this.Title = "Creación de Perfiles";
                if (!this.Page.IsPostBack)
                {
                    //((Label)base.Master.FindControl("lblNegocio")).Text = "Creacion de perfiles";
                    this.cargarRoles();
                }
                this.lblerror.Text = "";
                this.UpdatePanel4.Update();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
            }
        }
    }
}