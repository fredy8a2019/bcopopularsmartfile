using System;
using System.Web.UI.WebControls;
using GestorDocumental.Controllers;
using GestorDocumental.Models;

namespace GestorDocumental.ViewsAspx
{
    public partial class CrearUsuarios : System.Web.UI.Page
    {
        private ClienteController bCliente;
        private UsuariosController bUsuario;

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        private void cargarClientes()
        {
            try
            {
                this.bCliente = new ClienteController();
                this.lstCliente.DataSource = this.bCliente.obtenerClientes();
                this.lstCliente.DataTextField = "CliNombre";
                this.lstCliente.DataValueField = "CliNit";
                this.lstCliente.DataBind();
            }
            catch (Exception exception)
            {
                this.lblerror.Text = exception.Message;
            }
        }

        private void cargarRoles()
        {
            try
            {
                this.bUsuario = new UsuariosController();
                this.lstRol.DataSource = this.bUsuario.obtenerRolesUsuario();
                this.lstRol.DataTextField = "DescRol";
                this.lstRol.DataValueField = "RolId";
                this.lstRol.DataBind();
            }
            catch (Exception exception)
            {
                this.lblerror.Text = exception.Message;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "";
                if (!this.Page.IsPostBack)
                {
                    this.cargarClientes();
                    this.cargarRoles();
                    ((Label)base.Master.FindControl("lblNegocio")).Text = "Creacion de usuarios";
                }
                else
                {
                    this.lblerror.Text = "";
                }
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
            }
        }

        protected void txtIdentificacion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.bUsuario = new UsuariosController();
                Usuarios u = new Usuarios
                {
                    IdUsuario = decimal.Parse(this.txtIdentificacion.Text)
                };
                if (this.bUsuario.obtenerUsuario(u) != null)
                {
                    u = this.bUsuario.obtenerUsuario(u);
                    this.txtNombres.Text = u.NomUsuario;
                    this.txtPswd.Text = u.PassCodeUsuario;
                    this.lstCliente.SelectedIndex = this.lstCliente.Items.IndexOf(this.lstCliente.Items.FindByValue(u.CliNit.ToString()));
                    this.lstRol.SelectedIndex = this.lstRol.Items.IndexOf(this.lstRol.Items.FindByValue(u.RolId.ToString()));
                    this.txtPswd.CausesValidation = false;
                    this.txtNombres.Focus();
                }
                else
                {
                    this.txtNombres.Text = "";
                    this.lstCliente.SelectedIndex = 0;
                    this.lstRol.SelectedIndex = 0;
                    this.txtPswd.CausesValidation = true;
                    this.txtNombres.Focus();
                }
            }
            catch (Exception exception)
            {
                this.lblerror.Text = exception.Message;
            }
        }
    }
}