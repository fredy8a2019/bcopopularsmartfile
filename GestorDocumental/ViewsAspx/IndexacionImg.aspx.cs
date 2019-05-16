using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using GestorDocumental.Controllers;
using GestorDocumental.Models;

namespace GestorDocumental.ViewsAspx
{
    public partial class IndexacionImg : System.Web.UI.Page
    {

        private AsignacionesController bAsig = new AsignacionesController();
        private CargueController bCargue = new CargueController();
        private DocumentosController bdoc = new DocumentosController();
        private List<Documentos> doc = new List<Documentos>();

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            int flag = 0;
            try
            {

                AsignacionTareas a = new AsignacionTareas
                {
                    IdEstado = 30,
                    HoraTerminacion = new DateTime?(DateTime.Now),
                    NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                    IdEtapa = 20
                };
                this.bAsig.insertarAsignacion(a);
                //this.Session["NEGOCIO"] = null;
                flag = 1;

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo btnFinalizar_Click " + exception.Message);
                this.lblError.Text = exception.Message;
            }
            finally
            {
                if (flag == 1)
                { base.Response.Redirect("IndexacionImg.aspx"); }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Varifica que el numero de documento que digita este en la lista asignada
                int NedId = int.Parse(((Captura)this.Session["NEGOCIO"]).NegId.ToString());
                this.doc = this.bdoc.obtenerDocumentosIndexacion(NedId);
                var DocumentosIdex = doc.Find(x => x.DocId == int.Parse(this.txtDocumento.Text));


                if ((this.txtDocumento.Text.Trim() != string.Empty) & (this.txtPagina.Text.Trim() != string.Empty))
                {
                    if (DocumentosIdex != null)
                    {
                        if (Convert.ToInt32(this.txtPagina.Text) <= this.bdoc.ObtenerNumPaginasNegocio((Captura)this.Session["NEGOCIO"]))
                        {
                            ArchivosAnexos c = new ArchivosAnexos
                            {
                                NegId = ((Captura)this.Session["NEGOCIO"]).NegId,
                                AADescripcion = ((Captura)this.Session["NEGOCIO"]).NegId.ToString(),
                                DocId = Convert.ToInt32(this.txtDocumento.Text),
                                NumPagina = Convert.ToInt32(this.txtPagina.Text),
                                FechaRegistro = DateTime.Now,
                                Usuario = new decimal?(((Usuarios)this.Session["USUARIO"]).IdUsuario)
                            };
                            if (!this.bdoc.buscarPaginaDigitada(c))
                            {
                                this.bdoc.insertarDocsIndexados(c);
                                this.cargarPaginasIndexadas();
                                this.txtPagina.Text = (int.Parse(this.txtPagina.Text) + 1).ToString();
                                this.txtDocumento.Focus();
                                if (this.bdoc.IndexacionTerminada(c))
                                {
                                    this.btnFinalizar.Visible = true;
                                    this.Button1.Visible = false;
                                    this.txtDocumento.Enabled = false;
                                    this.txtDocumento.Text = "";
                                }
                            }
                            else
                            {
                                this.lblError.Text = "La pagina ingresada ya se encuentra asignada a otro documento";
                                this.cargarPaginasIndexadas();
                                this.UpdatePanel2.Update();
                                this.txtPagina.Focus();
                            }
                        }
                        else
                        {
                            this.lblError.Text = "El numero de pagina es mayor al total de paginas del archivo";
                            this.UpdatePanel2.Update();
                        }
                    }
                    else
                    {
                        this.lblError.Text = "Digite un documento valido de la lista";
                        this.UpdatePanel2.Update();
                    }
                }
                else
                {
                    this.lblError.Text = "Digite el documento y la pagina correspondiente.";
                    this.UpdatePanel2.Update();
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo Button1_Click " + exception.Message);
                this.lblError.Text = "Error en el metodo Button1_Click en indexacion " + exception.InnerException.ToString();
                this.UpdatePanel2.Update();
            }
        }

        private void cargarPaginasIndexadas()
        {
            try
            {
                Captura n = (Captura)this.Session["NEGOCIO"];
                this.gvDocsIndexados.DataSource = this.bdoc.ObtenerPaginasIndexadas(n);
                this.gvDocsIndexados.DataBind();
                this.UpdatePanel2.Update();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo cargarPaginasIndexadas " + exception.Message);
                this.lblError.Text = exception.Message;
                this.UpdatePanel2.Update();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session["TITULO"] = "Indexación del negocio";
                if (!this.Page.IsPostBack)
                {
                    this.txtDocumento.Focus();
                    //Clientes c = new Clientes
                    //{
                    //    CliNit = 1M
                    //};

                    this.txtPagina.Text = "1";
                    P_Etapas etapas = new P_Etapas
                    {
                        IdEtapa = 20
                    };
                    Captura n = new Captura();
                    decimal dec = ((Usuarios)this.Session["USUARIO"]).IdUsuario;
                    var negId = this.bAsig.ObtenerNegociosXEntrada((Usuarios)this.Session["USUARIO"], etapas);
                    if (negId == 0M)
                    {
                        n.NegId = 0M;
                        this.Session["NEGOCIO"] = n;
                        this.txtPagina.Enabled = false;
                        this.txtDocumento.Enabled = false;
                        this.lblError.Text = "No existen negocios disponibles para esta etapa";
                        this.UpdatePanel2.Update();
                    }
                    else
                    {
                        n.NegId = negId;  //this.bAsig.ObtenerNegociosXEntrada((Usuarios)this.Session["USUARIO"], etapas);
                        this.Session["NEGOCIO"] = n;
                        ((Label)base.Master.FindControl("lblNegocio")).Text = "Negocio:" + n.NegId.ToString();
                        AsignacionTareas a = new AsignacionTareas
                        {
                            NegId = n.NegId,
                            IdEtapa = etapas.IdEtapa,
                            Usuario = ((Usuarios)this.Session["USUARIO"]).IdUsuario,
                            HoraInicio = DateTime.Now,
                            IdEstado = 10
                        };

                        cargarPaginasIndexadas();
                        int num = this.bdoc.obtenerUltimaPagina(n) + 1;
                        this.txtPagina.Text = num.ToString();
                        int num2 = this.bdoc.ObtenerNumPaginasNegocio(n);
                        if (num > num2)
                        {
                            this.btnFinalizar.Visible = true;
                            //Button1.Visible = false;
                        }
                        if (!this.bAsig.ExisteEtapa(a))
                        {
                            this.bAsig.insertarAsignacion(a);
                        }
                    }

                    this.doc = this.bdoc.obtenerDocumentosIndexacion((int)n.NegId);
                    this.GridView1.DataSource = doc;
                    this.GridView1.DataBind();


                    if (doc.Count <= 0)
                    {
                        this.lblError.Text = "No existen documentos asociados a el subgrupo.";
                    }
                }
                else
                {
                    this.lblError.Text = "";
                }
                this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "FileLoad", "neg = '" + ConfigurationManager.AppSettings["ClientFiles"] + "/" + ((Captura)this.Session["NEGOCIO"]).NegId.ToString() + "/" + ((Captura)this.Session["NEGOCIO"]).NegId.ToString() + ".TIF';", true);
                if (((Captura)this.Session["NEGOCIO"]).NegId != 0M)
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "PagNumero", "NumPaginas = '" + this.bdoc.ObtenerNumPaginasNegocio((Captura)this.Session["NEGOCIO"]).ToString() + "';", true);
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo Page_Load " + exception.Message);
                throw;
            }

        }

        protected void txtNegocio_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.cargarPaginasIndexadas();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo txtNegocio_TextChanged " + exception.Message);
                throw;
            }

        }

        protected void txtPagina_TextChanged(object sender, EventArgs e)
        {
        }

        protected void gvDocsIndexados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "DEL":
                        ArchivosAnexos D = new ArchivosAnexos();
                        D.DocId = Convert.ToInt32(gvDocsIndexados.Rows[int.Parse(e.CommandArgument.ToString())].Cells[1].Text);
                        D.NumPagina = Convert.ToInt32(gvDocsIndexados.Rows[int.Parse(e.CommandArgument.ToString())].Cells[3].Text);
                        bdoc.BorrarDocumento(D, ((Captura)Session["NEGOCIO"]));

                        int num = this.bdoc.obtenerUltimaPagina(((Captura)Session["NEGOCIO"])) + 1;
                        this.txtPagina.Text = num.ToString();
                        UpdatePanel1.Update();
                        this.cargarPaginasIndexadas();
                        this.btnFinalizar.Visible = false;
                        Button1.Visible = true;
                        this.txtDocumento.Text = "";
                        this.txtDocumento.Enabled = true;
                        break;
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en IndexacionImg.aspx metodo gvDocsIndexados_RowCommand " + exception.Message);
                throw;
            }
        }

        protected void gvDocsIndexados_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}