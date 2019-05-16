using System;
using System.Linq;
using System.ServiceProcess;
using BLL;
using System.Diagnostics;
using System.IO;

namespace ServiceCopyFile
{
    partial class Service2 : ServiceBase
    {
        private BLLCargue bCargue;
        private System.Timers.Timer timer;
        private BaseDatosDataContext basdat = new BaseDatosDataContext();

        public Service2()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists("RegistroCargue"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "RegistroCargue", "CargueServiceLog");
            }
            eventLog1.Source = "RegistroCargue";
            eventLog1.Log = "CargueServiceLog";

            eventLog1.WriteEntry("Ingresa el constructor");
        }

        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
            this.timer = new System.Timers.Timer(Double.Parse(basdat.Parametros.Where(c => c.codigo == "DUR_CARGUE").First().valor));  // 30000 milliseconds = 30 seconds
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(cargueNegocios);
            this.timer.Start();
            eventLog1.WriteEntry("Servicio iniciado.");
        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.
            eventLog1.WriteEntry("Servicio detenido.");
        }

        private void cargueNegocios(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string str = basdat.Parametros.Where(c => c.codigo == "PATH_ORIGEN").First().valor;
                string str2 = basdat.Parametros.Where(c => c.codigo == "PATH_ORIGEN").First().valor;
                string str3 = basdat.Parametros.Where(c => c.codigo == "PATH_DESTINO").First().valor;

                string rutaOrigen = string.Empty;

                decimal cli_default = Decimal.Parse(basdat.Parametros.Where(c => c.codigo == "CLI_DEFAULT").First().valor);
                decimal user_default = Decimal.Parse(basdat.Parametros.Where(c => c.codigo == "USER_DEFAULT").First().valor);

                bCargue = new BLLCargue(basdat.Parametros.Where(c => c.codigo == "CON_BASDAT").First().valor, eventLog1);

                var ListaClientes = this.bCargue.LotesPendientesPorCliente();

                foreach (var item in ListaClientes)
                {
                    if (item.ContidadLotes != 0)
                    {
                        //LA CONEXION DE ESTE SERVICI   O ESTA POR BASES DE DATOS
                        //PASO EL LOG POR PARAMETRO PARA CAPTURAR LOS ERRORES POR WINDOWS
                        
                        if ((item.RutaOrigen == string.Empty) || item.RutaOrigen == null)
                        {
                            rutaOrigen = str;
                        }
                        else
                        {
                            rutaOrigen = item.RutaOrigen;
                        }
                        
                        this.bCargue.ObtenerArchivoCSV(rutaOrigen, item.Cliente, user_default);
                        eventLog1.WriteEntry("Llamado al metodo ObtenerArchivoCSV()");
                        this.bCargue.CraerDirectoriosNegocios(rutaOrigen, @str3);
                        eventLog1.WriteEntry("Llamado al metodo CraerDirectoriosNegocios()");
                    }
                }

            }
            catch (Exception exception)
            {
                eventLog1.WriteEntry("Error en metodo cargueNegocios " + exception.Message, EventLogEntryType.Error);
            }
        }


    }
}
