using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using TransmisionService;

namespace GestorTransmisionService
{
    public partial class Service1 : ServiceBase
    {
        private System.Timers.Timer timer;
        public basdatDataContext basdat = new basdatDataContext();

        public Service1()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("RegistroTransmision"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "RegistroTransmision", "TransmisionServiceLog");
            }
            eventLog1.Source = "RegistroTransmision";
            eventLog1.Log = "TransmisionServiceLog";

            eventLog1.WriteEntry("Ingresa el constructor");
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // TODO: agregar código aquí para iniciar el servicio.
                this.timer = new System.Timers.Timer(Double.Parse(basdat.Parametros.Where(c => c.codigo == "DUR_TRANSM").First().valor));  // 30000 milliseconds = 30 seconds
                this.timer.AutoReset = true;
                this.timer.Elapsed += new System.Timers.ElapsedEventHandler(transmision);
                this.timer.Start();
                eventLog1.WriteEntry("Servicio iniciado.");
            }
            catch (Exception exception)
            {
                eventLog1.WriteEntry("Error en metodo OnStart " + exception.Message);
                throw;
            }

        }

        //public void deleteLine(String ruta)
        //{
        //    StreamReader sr = new StreamReader(ruta);
        //    List<string> quotelist = File.ReadAllLines(ruta).ToList();
        //    sr.Close();

        //    StreamWriter sw = new StreamWriter(ruta);
        //    int i = 1;

        //    foreach (string item in quotelist)
        //    {
        //        if (i == quotelist.Count)
        //        {
        //            sw.Write(item);
        //        }
        //        else
        //        {
        //            sw.WriteLine(item);
        //        }
        //        i++;
        //    }
        //    sw.Close();
        //}


        public string obtenerFecha()
        {
            string fecha="";

            fecha = (DateTime.Now.Year.ToString()).Substring(2,2);
                             
            if (DateTime.Now.Month < 10)
             {
                fecha = fecha + "0" + DateTime.Now.Month;
             }
             else
                fecha = fecha + DateTime.Now.Month;

            if (DateTime.Now.Day < 10)
            {
                fecha = fecha + "0" + DateTime.Now.Day;
            }
            else
                fecha = fecha + DateTime.Now.Day;

            if (DateTime.Now.Hour < 10)
            {
                fecha = fecha + "_0" + DateTime.Now.Hour;
            }
            else
                fecha = fecha + "_"+DateTime.Now.Hour;

            if (DateTime.Now.Minute< 10)
            {
                fecha = fecha + "0" + DateTime.Now.Minute;
            }
            else
                fecha = fecha + DateTime.Now.Minute;

            return fecha;

        }


       private void transmision(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string nombreArchivo = "";
                string nuevoNombre = "";
                String consecutivo = "";
                string fecha = "";
                int reg = 0;
                string hora = DateTime.Now.Hour.ToString();
                basdatDataContext basdat1 = new basdatDataContext();
                basdat1.CommandTimeout = 0;                
                
                List<Transmision> procesar = basdat1.Transmision.OrderBy(c => c.orden).Where(c => c.activo == true && c.hora == hora && c.procesado == false).ToList();
                eventLog1.WriteEntry("lleno el LIst con" + procesar.Count.ToString() + " SP`s para ejecutar");

                foreach (Transmision proceso in procesar)
                {

                    using (SqlConnection connection = new SqlConnection(basdat1.Parametros.Where(c => c.codigo == "CON_BASDAT").First().valor))
                    {
                        
                        connection.Open();
                        
                        String rutaEjeComp = basdat1.Parametros.Where(c => c.codigo == "PATH_COMPRESOR").First().valor;

                        //using (TransactionScope scope = new TransactionScope())
                        //{
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandTimeout = 0;
                            command.CommandText = proceso.storeProcedure;
                            command.CommandType = CommandType.StoredProcedure;

                            SqlDataReader reader = command.ExecuteReader();

                            //ARMO EL NOMBRE DEL ARCHIVO CONCATENANDO LA FECHA Y LA HORA
                            nombreArchivo = proceso.nomArchivo + "_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute+proceso.extArchivo;

                            // ORGANIZAMOS LA FECHA Y LA HORA EN UNA VARIABLE
                            
                            using (CsvFileWriter writer = new CsvFileWriter(@proceso.rutaArchivo + @"\" + nombreArchivo))
                            {

                                int CantReg = 0;

                                while (reader.Read())
                                {

                                    List<string> linea = new List<string>();

                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        string dato = reader.GetString(i);
                                        if (dato == null)
                                        {
                                            dato = "";
                                        }
                                        else if (string.IsNullOrEmpty(dato))
                                        {
                                            dato = "";
                                        }
                                        else if (string.IsNullOrWhiteSpace(dato))
                                        {
                                            dato = "";
                                        }
                                        linea.Add(dato);
                                    }

                                    CsvRow row = new CsvRow();
                                    row.separator = Char.Parse(proceso.delimitador);
                                    row.AddRange(linea);
                                    writer.WriteRow(row);
                                    CantReg = CantReg + 1;
                                }
                                reg = CantReg;
                                eventLog1.WriteEntry("se termino de escribir el archivo");


                            }

                            //A EL ARCHIVO RECIEN GENERADO LE BORRO LA ULTIMA LINEA
                           // deleteLine(@proceso.rutaArchivo + @"\" + nombreArchivo);

                    //        //traemos el numero del consecutivo de transmision
                    //        if (proceso.id == 1 || proceso.id == 3 || proceso.id == 4)
                    //        {
                    //            consecutivo = (basdat1.Consecutivos.Where(c => c.ConId == 4).First().ConValor).ToString();
                    //            fecha = obtenerFecha();

                    //            switch (proceso.id.ToString())
                    //            {
                                    
                    //                case "1":
                    //                    nuevoNombre = @proceso.rutaArchivo + @"\" + "CLI_" +fecha+ "_EVE_"
                    //                        + consecutivo + "_" + reg.ToString() + ".DAT";

                    //                    File.Move(@proceso.rutaArchivo + @"\" + nombreArchivo, nuevoNombre);
                    //                    eventLog1.WriteEntry("se genero el archivo de clientes");

                    //                    break;

                    //                case "3":
                    //                    nuevoNombre = @proceso.rutaArchivo + @"\" + "CLD_" + fecha + "_EVE_"
                    //                      + consecutivo + "_" + reg.ToString() + ".DAT";

                    //                    File.Move(@proceso.rutaArchivo + @"\" + nombreArchivo, nuevoNombre);
                    //                    eventLog1.WriteEntry("se genero el archivo demografico");

                    //                    break;

                    //                case "4":
                    //                    nuevoNombre = @proceso.rutaArchivo + @"\" + "ASC_" + fecha + "_EVE_"
                    //                        + consecutivo + "_" + reg.ToString() + ".DAT";

                    //                    File.Move(@proceso.rutaArchivo + @"\" + nombreArchivo, nuevoNombre);

                    //                    eventLog1.WriteEntry("se genero el archivo de asociados");

                    //                    break;

                    //            }



                    //            //ahora vamos a comprimir el archivo

                    //            //Process.Start(rutaEjeComp, nuevoNombre);
                    //            eventLog1.WriteEntry("se comprimio el archivo");

                    //            //Ahora creamos el archivo de conteo.

                    //            using (CsvFileWriter writer = new CsvFileWriter(@proceso.rutaArchivo + @"\" + nombreArchivo))
                    //            {
                    //                List<string> linea = new List<string>();
                    //                CsvRow row = new CsvRow();
                    //                row.Add(reg.ToString());
                    //                writer.WriteRow(row);
                    //            }
                    //            eventLog1.WriteEntry("se termino de escribir el archivo de control");

                    //            //A EL ARCHIVO RECIEN GENERADO LE BORRO LA ULTIMA LINEA
                    //            //deleteLine(@proceso.rutaArchivo + @"\" + nombreArchivo);

                    //            switch (proceso.id.ToString())
                    //            {
                    //                case "1":
                    //                    File.Move(@proceso.rutaArchivo + @"\" + nombreArchivo,
                    //                          @proceso.rutaArchivo + @"\" + "CLI_" + fecha + "_EVE_"
                    //                        + consecutivo + "_" + reg.ToString() + ".CTL");
                    //                    eventLog1.WriteEntry("se genero el archivo de control de clientes");
                    //                    break;

                    //                case "3":
                    //                    File.Move(@proceso.rutaArchivo + @"\" + nombreArchivo,
                    //                         @proceso.rutaArchivo + @"\" + "CLD_" + fecha + "_EVE_"
                    //                        + consecutivo + "_" + reg.ToString() + ".CTL");
                    //                    eventLog1.WriteEntry("se genero el archivo de control demografico");
                    //                    break;

                    //                case "4":
                    //                    File.Move(@proceso.rutaArchivo + @"\" + nombreArchivo,
                    //                         @proceso.rutaArchivo + @"\" + "ASC_" + fecha + "_EVE_"
                    //                        + consecutivo + "_" + reg.ToString() + ".CTL");
                    //                    eventLog1.WriteEntry("se genero el archivo de control de asociados");
                    //                    break;
                    //            }
                    //        }


                    //        else
                    //            File.Move(@proceso.rutaArchivo + @"\" + nombreArchivo, @proceso.rutaArchivo + @"\" + nombreArchivo + proceso.extArchivo);
                    //        eventLog1.WriteEntry("se genero el archivo");

                    //        //}
                    //        //scope.Complete();
                    //        eventLog1.WriteEntry("se cerro el scope");

                        }
                    //    // connection.Close();
                    }


                    //eventLog1.WriteEntry("cambiamos los datos en la tabla Transmision de la BD");

                }

                foreach (Transmision procesoTerminado in procesar)
                {
                    eventLog1.WriteEntry("iNGRESA AL CICLO DE CAMBIO DE ESTADO");
                    procesoTerminado.procesado = true;
                    eventLog1.WriteEntry("cambia estado a true");
                    procesoTerminado.fecProceso = DateTime.Now;
                    basdat1.SubmitChanges();
                    eventLog1.WriteEntry("hace el submit");
                }

                basdat1.Dispose();
                eventLog1.WriteEntry("libera recursos");

            }
            catch (Exception exception)
            {
                eventLog1.WriteEntry("Error en metodo transmision " + exception.Message + exception.StackTrace);
                throw;
            }
        }


        protected override void OnStop()
        {
        }
    }
}
