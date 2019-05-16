using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;


namespace GNF
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        GestorDocumentalGNFEntities bd = new GestorDocumentalGNFEntities();


        public string encryptar(string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, password);
                return hash;
            }

        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        #region Descriptores de acceso de atributos de ensamblado

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void bt_ActTipoFormulario_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "")
            {
                var UsuarioIng = int.Parse(textBox3.Text);

                try
                {
                    Usuarios consulta = bd.Usuarios.Where(x => x.IdUsuario == UsuarioIng).First();
                    if (encryptar(textBox2.Text) == consulta.PassCodeUsuario && consulta.RolId == 1)
                    {

                        Form1 rol = new Form1();
                        rol.Show();
                        this.Visible = false;
                        rol.label8.Text = textBox3.Text;
                        rol.label4.Text = consulta.NomUsuario;
                    }
                    else
                    {
                        MessageBox.Show("Los datos ingresados son incorrectos intentelo nuevamente");
                        textBox3.Clear();
                        textBox2.Clear();
                        textBox3.Focus();
                    }
                }

                catch
                {
                    MessageBox.Show("Se ha presentado un error en la validación");

                }
            }
            else 
            {
                MessageBox.Show("Verifique la información ingresada.", "error en la validación");
                textBox2.Clear();
                textBox3.Clear();
            }

        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox3.Focus();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

    }
}
