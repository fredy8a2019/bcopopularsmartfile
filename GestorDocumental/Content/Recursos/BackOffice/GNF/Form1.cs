using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GNF
{
    public partial class Form1 : Form
    {
        GestorDocumentalGNFEntities bd = new GestorDocumentalGNFEntities();
        public Form1()
        {
            InitializeComponent(); 
            
        }
       
               
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    bd.sp_DetenerNegocios(int.Parse(textBox1.Text));
                    MessageBox.Show("Negocio Detenido Correctamente " + textBox1.Text);
                    bd.sp_InsertarAuditoria(int.Parse(label8.Text),"Actualiacion aplicativo", "Detener Negocio", int.Parse(textBox1.Text));
                    textBox1.Clear();
                    textBox1.Focus();
                }
                else 
                {
                    MessageBox.Show("Digite el Número de Negocio", "Alerta");
                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso", "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    int negocio = int.Parse(textBox1.Text);
                    CargueLotes cl = bd.CargueLotes.Where(x => x.NegId == negocio).First();
                    cl.Terminado = false;
                    bd.sp_InsertarAuditoria(int.Parse(label8.Text),"Actualiacion aplicativo","Reactivar Negocio", int.Parse(textBox1.Text));
                    bd.SaveChanges();
                    MessageBox.Show("Negocio Iniciado Correctamente", "Aviso");
                    textBox1.Clear();
                    textBox1.Focus();
                }
                else
                {
                    MessageBox.Show("Digite el Número de Negocio", "Alerta");
                }
              }
            catch
            {
                MessageBox.Show("Error en el proceso", "Error");
             }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    bd.spCargueTablasWS_SAP(int.Parse(textBox1.Text));
                    bd.sp_InsertarAuditoria(int.Parse(label8.Text),"Actualiacion aplicativo","Repaso por SP_Carguetablas WS", int.Parse(textBox1.Text));
                    bd.SaveChanges();
                    MessageBox.Show("Negocio Analizado ");
                    textBox1.Clear();
                    textBox1.Focus();
                }
                else
                {
                    MessageBox.Show("Digite el Número de Negocio", "Alerta");
                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso", "Error");
 
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text != "")
                {
                    bd.sp_desbloquearUsuario(int.Parse(textBox2.Text));
                    bd.sp_InsertarAuditoria(int.Parse(label8.Text),"Actualiacion aplicativo", "Desbloqueo de usuario", int.Parse(textBox2.Text));
                    bd.SaveChanges();
                    MessageBox.Show("Usuario Desbloquado Correctamente");
                    textBox2.Clear();
                    textBox2.Focus();
                }
                else
                {
                    MessageBox.Show("Digite el Número de Usuario", "Alerta");
                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso", "Error");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int algo33 = 0;
            var consultaestadotrasmi = from consola in bd.Parametros
                                       where consola.codigo == "EST_TRANSM"
                                       select consola;

            foreach (var x in consultaestadotrasmi)
            {
                algo33 = int.Parse(x.valor.ToString());
            }

            if (algo33==0)
            {
                button4.Enabled = false;
                button3.Enabled = true;
                label15.Text = "Consola Activado";
            }
            else if (algo33==2)
            {
                button4.Enabled = true;
                button3.Enabled = false;
                label15.Text = "Consola Desactivado";
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text != "")
                {
                    int negocio = int.Parse(textBox3.Text);
                    var consulta = bd.sp_NegocioDuplicado(negocio);
                    dataGridView1.DataSource = consulta;
                }
                else
                {
                    MessageBox.Show("Digite el Número de Negocio", "Alerta");
                    textBox3.Focus();

                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso", "Error");
            }
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Close();
            Login log = new Login();
            log.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
            try
            {
                bd.sp_ActivarTransmisionWS(2, int.Parse(label8.Text));
                MessageBox.Show("Transmision Detenida con Exito", "Transacción Exitosa");
                button3.Enabled = false;
                button4.Enabled = true;
            }

            catch
            {
                MessageBox.Show("Error en el proceso", "Error");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                bd.sp_ActivarTransmisionWS(0, int.Parse(label8.Text));
                MessageBox.Show("Transmision Activada con Exito", "Transacción Exitosa");
                button4.Enabled = false;
                button3.Enabled = true;
            }

            catch
            {
                MessageBox.Show("Error en el proceso", "Error");
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.DataSource = bd.spEstadoPendientes(dateTimePicker1.Text, dateTimePicker2.Text, null, null);
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.ToString());
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_oficina_SelectedIndexChanged(object sender, EventArgs e)
        {

            int x = int.Parse(comboBox_oficina.SelectedValue.ToString());
                     

            dataGridView2.DataSource = bd.spEstadoPendientes(dateTimePicker1.Text, dateTimePicker2.Text, null, comboBox_oficina.Text.Trim());
          
        }

        private void comboBox_Producto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int x = int.Parse(comboBox_Producto.SelectedValue.ToString());

                var subproductos = from prod in bd.Grupos
                                   where prod.GruIdPadre == x
                                   select prod;

                comboBox_subProduct.DisplayMember = "GruDescripcion";
                comboBox_subProduct.ValueMember = "Gruid";
                comboBox_subProduct.DataSource = subproductos;
            }
            catch
            {
                MessageBox.Show("Error en el proceso");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked == true)
                {

                    var oficinas = from ofic in bd.P_Oficinas
                                   select ofic;

                    comboBox_oficina.DisplayMember = "OFI_Nombre";
                    comboBox_oficina.ValueMember = "OFI_codNit";
                    comboBox_oficina.DataSource = oficinas;
                    comboBox_oficina.Enabled = true;
                    comboBox_Producto.Enabled = false;
                    comboBox_subProduct.Enabled = false;
                }
                else
                {
                    comboBox_oficina.Enabled = false;

                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso");
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
         
            

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = true;
             dateTimePicker2.Enabled = true;
             button8.Enabled = true;
        }

        private void comboBox_subProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.DataSource = bd.spEstadoPendientes(dateTimePicker1.Text, dateTimePicker2.Text, comboBox_subProduct.Text.Trim(), comboBox_oficina.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Error en el proceso");

            }
        }


        private void checkBox2_CheckedChanged_2(object sender, EventArgs e)
        {
            try
            {
                if (checkBox2.Checked == true)
                {
                    comboBox_Producto.Enabled = true;
                    comboBox_subProduct.Enabled = true;
                    comboBox_oficina.Enabled = true;
                    int x = int.Parse(comboBox_oficina.SelectedValue.ToString());
                     
                    var productos = from prod in bd.Grupos
                                    where prod.CliNit == x
                                    select prod;

                    comboBox_Producto.DisplayMember = "GruDescripcion";
                    comboBox_Producto.ValueMember = "Gruid";
                    comboBox_Producto.DataSource = productos;
                }
                else
                {
                    comboBox_Producto.Enabled = false;
                    comboBox_subProduct.Enabled = false;

                }
            }
            catch
            {

                MessageBox.Show("Error en el proceso");
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text != "")
                {
                    bd.sp_descontabilizar(textBox4.Text, int.Parse(label8.Text));
                    MessageBox.Show("Recontabilización Realizada.");
                    textBox4.Clear();
                    textBox4.Focus();
                }
                else
                {
                    MessageBox.Show("Digite el Número de Código de Barras", "Alerta");
                }
            }
            catch
            {

                MessageBox.Show("Error en el proceso");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            this.Close();
            Login log = new Login();
            log.Show();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

       
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox8.Text != "")
                {
                    textBox10.Clear();
                    textBox5.Clear();
                    textBox9.Clear();

                    dataGridView4.Columns.Clear();
                    int valoringresado = int.Parse(textBox8.Text.Trim());

                    var lotes = from lot in bd.Recepcion
                                join rece in bd.Recepcion_Detalle
                                on lot.id equals rece.DET_idrecepcion
                                where lot.numeroLote == valoringresado
                                select new
                                {
                                    Principales = lot.principales,
                                    Anexos = rece.DET_Anexo
                                };


                    dataGridView3.DataSource = lotes;
                }
                else
                {
                    MessageBox.Show("Número de Lote no existe, verifique la información ingresada.");

                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso");
            }

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox6.Text != "" && textBox8.Text != "")
                {
                    int numerolotes = int.Parse(textBox8.Text);
                    var consul = bd.Recepcion.Where(x => x.numeroLote == numerolotes).First();
                    consul.principales = int.Parse(textBox6.Text);
                    bd.SaveChanges();
                    DateTime hoy = DateTime.Today;
                    string fechactual = hoy.ToString();
                    string valoranterior = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                    bd.sp_InsertarAuditoria(int.Parse(label8.Text.Trim()), "Actualiacion aplicativo","valoranterior" + valoranterior + "NuevoValor" + textBox6.Text + "Negocio", +numerolotes);
                    bd.SaveChanges();
                    MessageBox.Show("Actualización Realizada");
                    textBox6.Clear();
                    textBox6.Focus();
                }
                else
                {
                    MessageBox.Show("Número de principales no tiene el formato correcto, verifique la información ingresada.");

                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso, verifique la información ingresada.");
            }


        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (textBox7.Text !="" && textBox8.Text !="")
                {
                int numerolotes = int.Parse(textBox8.Text);
                var consultaid = (from recepd in bd.Recepcion_Detalle
                                  join rec in bd.Recepcion
                                  on recepd.DET_idrecepcion equals rec.id
                                  where rec.numeroLote == numerolotes
                                  select rec.id).Take(1).ToArray()[0];


                var consul = bd.Recepcion_Detalle.Where(x => x.DET_idrecepcion == consultaid).First();
                consul.DET_Anexo = int.Parse(textBox7.Text);
                bd.SaveChanges();
                DateTime hoy = DateTime.Today;
                string fechactual = hoy.ToString();
                string valoranterior = dataGridView3.CurrentRow.Cells[1].Value.ToString();
                bd.sp_InsertarAuditoria(int.Parse(label8.Text.Trim()), "Actualiacion aplicativo","valoranterior" + valoranterior + "NuevoValor" + textBox7.Text + "Negocio", +numerolotes);
                bd.SaveChanges();
                MessageBox.Show("Actualización Realizada");
                textBox7.Clear();
                textBox7.Focus();
                }
                else
                {
                    MessageBox.Show("Anexos no tiene el formato correcto, verifique la información ingresada.");
                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso, verifique la información ingresada.");
            }
        }

        private void button14_Click(object sender, EventArgs e)
       {
           try
           {
               if (textBox5.Text != "")
               {
                   textBox9.Clear();
                   textBox8.Clear();
                   textBox10.Clear();
                   dataGridView3.Columns.Clear();

                   var consulta = from con in bd.CargueLotes
                                  where con.LoteScaner == textBox5.Text.Trim()
                                  select new
                                  {
                                      Negocio = con.NegId,
                                      Imagen = con.NomArchivo,
                                      CodigoBarras = con.CodBarras
                                  };

                   dataGridView4.DataSource = consulta;
               }
               else 
               {
                   MessageBox.Show("Número de Lote no existe, verifique la información ingresada.");
               }
           }
           catch
           {
               MessageBox.Show("Error en el proceso, verifique la información ingresada.");
           }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int algo = 0;
            try
            {
                if (textBox5.Text != "")
                {
                    textBox8.Clear();
                    textBox9.Clear();
                    textBox10.Clear();


                long valor = long.Parse(textBox5.Text.Trim());
                var consultaid = (from rece in bd.Recepcion
                                     where  rece.numeroLote == valor
                                      select rece.id).Take(1).ToArray()[0];

                var consultaanula = from carg in bd.CargueLotes
                                     join rece in bd.Recepcion
                                     on carg.idRecepcion equals rece.id
                                     where carg.LoteScaner == textBox5.Text.Trim()
                                     select  carg;

                                              
                foreach (var y in consultaanula)
                {
                        
                    algo= int.Parse(y.NegId.ToString());
                    var camb = bd.CargueLotes.Where(x => x.NegId == algo).First();
                    camb.Terminado = true;
                    bd.spAnulacionNegocios(decimal.Parse(algo.ToString()), 999999, null, 2);
                    
                 }
                bd.SaveChanges();
                var cambrece = bd.Recepcion.Where(x => x.id == consultaid).First();
                cambrece.activo = false;
                bd.SaveChanges();
                
                DateTime hoy = DateTime.Today;
                string fechactual = hoy.ToString();
                bd.sp_InsertarAuditoria(int.Parse(label8.Text.Trim()),"Actualiacion aplicativo","Anulacion Lote"+textBox5.Text.Trim(),int.Parse(textBox5.Text.Trim()));
                MessageBox.Show("Lote anulado correctamente" + textBox5.Text);
              }
              else
               {
                 MessageBox.Show("Número de Lote no existe, verifique la información ingresada.");
               }
            }
            catch
            {

                MessageBox.Show("Error en el proceso, verifique la información ingresada.");
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox9.Text != "")
                {
                    textBox5.Clear();
                    textBox8.Clear();
                    textBox10.Clear();

                    dataGridView3.Columns.Clear();
                    int valoringresado = int.Parse(textBox9.Text);
                    var consultalotes = from lt in bd.CargueLotes
                                        where lt.NegId == valoringresado
                                        select new
                                        {

                                            Lote = lt.LoteScaner,
                                            Imagen = lt.NomArchivo,
                                            CodigodeBarras = lt.CodBarras

                                        };

                    dataGridView4.DataSource = consultalotes;
                }

                else
                {
                    MessageBox.Show("Número de Negocio no existe, verifique la información ingresada.");
                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso, verifique la información ingresada.");
            }

        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar) < 48 && e.KeyChar != 8) || e.KeyChar > 57)
            {
                MessageBox.Show("Sólo se permiten Números", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox10.Text !="")
                {
                    textBox5.Clear();
                    textBox8.Clear();
                    textBox9.Clear();
                    dataGridView4.Columns.Clear();
                    var consultacod = from lot in bd.CargueLotes
                                      where lot.CodBarras == textBox10.Text
                                      select new
                                      {

                                          Negocio = lot.NegId,
                                          Lote =lot.LoteScaner,
                                          Imagen = lot.NomArchivo,
                                          CodigodeBarras = lot.CodBarras

                                      };
                    dataGridView3.DataSource = consultacod;

                }
            }
            catch
            {
                MessageBox.Show("Error en el proceso, verifique la información ingresada.");
            }
        }

        //Consultar los meses de caducidad del usuario
        public int ConsultarMesesCaducidad()
        {
            GestorDocumentalGNFEntities dbo = new GestorDocumentalGNFEntities();
            var query = (from a in dbo.Parametros
                         where a.codigo == "FECHA_CADUCIDAD"
                         select a).FirstOrDefault();
            return Convert.ToInt32(query.valor);
        }

        private void button17_Click(object sender, EventArgs e)
        {
         try
            {
            if (textBox2.Text != "")
            {
            int valoringresado = int.Parse(textBox2.Text.Trim());
            int mesCaducidad = this.ConsultarMesesCaducidad();

            Usuarios us = bd.Usuarios.Where(x => x.IdUsuario == valoringresado).First();
            us.FechaCaducidad = DateTime.Now.AddMonths(mesCaducidad);
            us.PassCodeUsuario = "202cb962ac59075b964b07152d234b70";

                if (bd.UsuariosBloqueados.Where(a => a.IdUsuario == valoringresado && a.Bloqueado == true).Count() >= 1)
            {
                UsuariosBloqueados usBloq = bd.UsuariosBloqueados.Where(a => a.IdUsuario == valoringresado && a.Bloqueado == true).First();
                usBloq.Bloqueado = false;
            }

            bd.sp_InsertarAuditoria(int.Parse(label8.Text.Trim()),"ActualizacionAplicativo","ReinicioClaveUsuario"+us.IdUsuario,int.Parse(textBox2.Text));
            MessageBox.Show("Actualización Realizada, se cambio la contraseña del usuario a 123");
            }
            else
              {
                  MessageBox.Show("Número de Negocio no existe, verifique la información ingresada.");
              }
            }
            catch
            {
                MessageBox.Show("Error en el proceso, verifique la información ingresada.");
            
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                bd.CommandTimeout = 0;
                Thread t = new Thread(() => bd.spIndexacionAutomatica());                
                t.Start();
                t.Join();

                button18.Enabled = false;
                MessageBox.Show("Se ha iniciado el proceso de indexacion automatica ... Recuerde que el proceso puede durar hasta 5 minutos hasta su finalizacion.");
            }
            catch
            {
                MessageBox.Show("Error en el proceso");
            }

        }

     }
}
