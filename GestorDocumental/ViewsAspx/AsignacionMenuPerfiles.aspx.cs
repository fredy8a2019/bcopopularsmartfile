using System;
using System.Collections.Generic;
using GestorDocumental.Models;

namespace GestorDocumental.ViewsAspx
{
    public partial class AsignacionMenuPerfiles : System.Web.UI.Page
    {
        private GestorDocumental.Controllers.MenuController bMenu;
        private GestorDocumental.Controllers.UsuariosController bUsuario;

        private void cargarPadres()
        {
            try
            {
                this.bMenu = new Controllers.MenuController();
                this.rbPadres.DataSource = this.bMenu.ObtenerPadres();
                this.rbPadres.DataTextField = "DescMenu";
                this.rbPadres.DataValueField = "IdMenu";
                this.rbPadres.DataBind();
            }
            catch (Exception exception)
            {
                this.lblError.Text = exception.Message;
                this.UpdatePanel3.Update();
            }
        }

        private void cargarPerfiles()
        {
            try
            {
                this.bUsuario = new Controllers.UsuariosController();
                this.lstPerfil.DataSource = this.bUsuario.obtenerRolesUsuario();
                this.lstPerfil.DataTextField = "DescRol";
                this.lstPerfil.DataValueField = "RolId";
                this.lstPerfil.DataBind();
            }
            catch (Exception exception)
            {
                this.lblError.Text = exception.Message;
                this.UpdatePanel3.Update();
            }
        }

        protected void chkHijos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Models.Menu menu;
                P_Roles roles;
                string s = string.Empty;
                string str2 = string.Empty;
                string[] strArray = base.Request.Form["__EVENTTARGET"].Split(new char[] { '$' });
                int num = int.Parse(strArray[strArray.Length - 1]);
                if (this.chkHijos.Items[num].Selected)
                {
                    s = this.chkHijos.Items[num].Value;
                }
                else
                {
                    str2 = this.chkHijos.Items[num].Value;
                }
                if (s != "")
                {
                    menu = new Models.Menu();
                    roles = new P_Roles();
                    this.bMenu = new GestorDocumental.Controllers.MenuController();
                    menu.IdMenu = int.Parse(s);
                    roles.RolId = int.Parse(this.lstPerfil.SelectedValue);
                    this.bMenu.InsertarOpcionesMenu(roles, menu);
                }
                else
                {
                    menu = new Models.Menu();
                    roles = new P_Roles();
                    this.bMenu = new GestorDocumental.Controllers.MenuController();
                    menu.IdMenu = int.Parse(str2);
                    roles.RolId = int.Parse(this.lstPerfil.SelectedValue);
                    this.bMenu.borrarOpcionesMenu(roles, menu);
                }
            }
            catch (Exception exception)
            {
                this.lblError.Text = exception.Message;
                this.UpdatePanel3.Update();
            }
        }

        protected void lstPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.lstPerfil.SelectedIndex > 0) && (this.rbPadres.SelectedIndex > -1))
            {
                this.bMenu = new GestorDocumental.Controllers.MenuController();
                Models.Menu mP = new Models.Menu
                {
                    IdMenu = int.Parse(this.rbPadres.SelectedValue)
                };
                this.chkHijos.DataSource = this.bMenu.ObtenerHijos(mP);
                P_Roles r = new P_Roles
                {
                    RolId = int.Parse(this.lstPerfil.SelectedValue)
                };
                this.ResaltarOpcionesMenu(bMenu.ObtenerHijosPerfil(r, mP));
                this.UpdatePanel2.Update();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CLIENTE"] != null)
            {
                if (!this.Page.IsPostBack)
                {
                    this.cargarPadres();
                    this.cargarPerfiles();
                }
                this.lblError.Text = "";
                this.UpdatePanel3.Update();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
            }
        }

        protected void rbPadres_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.bMenu = new GestorDocumental.Controllers.MenuController();
                Models.Menu mP = new Models.Menu
                 {
                     IdMenu = int.Parse(this.rbPadres.SelectedValue)
                 };
                this.chkHijos.DataSource = this.bMenu.ObtenerHijos(mP);
                this.chkHijos.DataTextField = "DescMenu";
                this.chkHijos.DataValueField = "IdMenu";
                this.chkHijos.DataBind();
                P_Roles r = new P_Roles
                {
                    RolId = int.Parse(this.lstPerfil.SelectedValue)
                };
                this.ResaltarOpcionesMenu(this.bMenu.ObtenerHijosPerfil(r, mP));
                this.UpdatePanel2.Update();
            }
            catch (Exception exception)
            {
                this.lblError.Text = exception.Message;
                this.UpdatePanel3.Update();
            }
        }

        private void ResaltarOpcionesMenu(List<spObtenerMenuHijosPerfil_Result> L)
        {
            try
            {
                int i = 0;
                if (L.Count > 0)
                {

                    foreach (spObtenerMenuHijosPerfil_Result m in L)
                    {
                        for (i = 0; i < chkHijos.Items.Count; i++)
                        {
                            if (chkHijos.Items[i].Value == m.IdMenu.ToString())
                            {
                                chkHijos.Items[i].Selected = true;

                            }
                            else
                            {
                                chkHijos.Items[i].Selected = false;

                            }

                        }
                    }

                }



                //using (IEnumerator enumerator = this.chkHijos.Items.GetEnumerator())
                //{
                //    Predicate<spObtenerMenuHijosPerfil_Result> match = null;
                //    ListItem m;
                //    while (enumerator.MoveNext())
                //    {
                //        m = (ListItem)enumerator.Current;
                //        if (match == null)
                //        {
                //            match = i => i.IdMenu == int.Parse(m.Value);
                //        }
                //        if (L.Find(match) != null)
                //        {
                //            m.Selected = true;
                //        }
                //        else
                //        {
                //            m.Selected = false;
                //        }
                //        num++;
                //    }
                //}
            }
            catch (Exception exception)
            {
                this.lblError.Text = exception.Message;
                this.UpdatePanel3.Update();
            }
        }
    }
}