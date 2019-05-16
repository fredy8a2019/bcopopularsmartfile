using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLL;

namespace TestService
{
    public partial class Form1 : Form
    {
        private BLLCargue bCargue = new BLLCargue("data source=12.109.8.55;initial catalog=GestorDocumentalGNF;persist security info=True;user id=Bizagi;password=Everis1", null);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string str = @"C:\Gas Natural\";
            //string str2 = @"C:\Gas Natural\";
            string str3 = @"C:\inetpub\wwwroot\GestorDocumentalGNFV6\Content\ArchivosCliente";


            var lista = this.bCargue.LotesPendientesPorCliente();

            foreach (var item in lista)
            {
                if (item.ContidadLotes != 0)
                {
                    this.bCargue.ObtenerArchivoCSV(item.RutaOrigen, item.Cliente, 9999999m);

                    this.bCargue.CraerDirectoriosNegocios(item.RutaOrigen, str3);

                }
            }

        }
    }
}
