using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using GestorDocumental.Controllers;
using GestorDocumental.Models;

namespace GestorDocumental
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        private MenuController  bMenu = new MenuController ();
        
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            //base.Session.Abandon();
            base.Response.Redirect("/Seguridad/Logout");
        }

        private void crearArbol(List<MenuPadre_Result> L)
        {
           // Usuarios xx = (Usuarios)base.Session["USUARIO"];
            //  string eso = xx.CliNit.ToString();
            try
            {
                int imagen = 1;
                foreach (MenuPadre_Result result in L)
                {
                    TreeNode r = new TreeNode();
                    MenuItem a = new MenuItem();
                    
                    Models.Menu  m = new Models.Menu ();
                    r.Text = result.DescMenu;
                    r.NavigateUrl = result.Url;
                    r.ToolTip = result.DescMenu;
                    m.IdPadre = new int?(result.IdMenu);
                    this.CrearHijos(this.bMenu.ObtenerHijos((Usuarios)base.Session["USUARIO"], m), r, a);
                  //  this.TreeView1.Nodes.Add(r);

                    a.Text = result.DescMenu;
                    a.NavigateUrl = "#";
                    a.ToolTip = result.DescMenu;
                    a.ImageUrl = ".." + result.UrlImagen ;
                    this.NavigationMenu.Items.Add(a);

                    imagen++;
                }
                //this.TreeView1.Nodes;
                
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void CrearHijos(List<MenuHijos_Result> L, TreeNode R, MenuItem a)
        {
            try
            {
                foreach (MenuHijos_Result result in L)
                {
                    TreeNode child = new TreeNode
                    {
                        Text = result.DescMenu,
                        NavigateUrl = result.Url
                    };
                    R.ChildNodes.Add(child);

                    MenuItem hijos = new MenuItem
                    {
                        Text = result.DescMenu,
                        NavigateUrl = result.Url
                    };
                    a.ChildItems.Add(hijos);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {

                this.crearArbol(All());
                //Cierro el menu todos los items
                //this.TreeView1.CollapseAll();
            }
        }

        //solo consulto la primera vez la BD de alli en adelante se genera persistencia en session
        public List<MenuPadre_Result> All()
        {
            List<MenuPadre_Result> result =
                (List<MenuPadre_Result>)HttpContext.Current.Session["PADRES_MENU"];
            if (result == null)
            {
                HttpContext.Current.Session["PADRES_MENU"] = result = this.bMenu.ObtenerPadres((Usuarios)base.Session["USUARIO"]);
            }

            return result;
        }

    }
}