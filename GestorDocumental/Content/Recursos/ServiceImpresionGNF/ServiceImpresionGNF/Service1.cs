using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Transactions;
using Com.SharpZebra.Printing;

namespace ServiceImpresionGNF
{
    public partial class Service1 : ServiceBase
    {
        private BaseDatosDataContext basdat = new BaseDatosDataContext();
        private System.Timers.Timer timer;

        public Service1()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists("RegistroImpresiones"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "RegistroImpresiones", "ImpresionesServiceLog");
            }
            eventLog1.Source = "RegistroImpresiones";
            eventLog1.Log = "ImpresionesServiceLog";

            eventLog1.WriteEntry("Ingresa el constructor");
        }

        // #if DEBUG
        //public void OnStart()
        //{
        //    // TODO: agregar código aquí para iniciar el servicio.
        //    this.timer = new System.Timers.Timer(Double.Parse(basdat.Parametros.Where(c => c.codigo == "SERV_IMPRE").First().valor));  // 30000 milliseconds = 30 seconds
        //    this.timer.AutoReset = true;
        //    this.timer.Elapsed += new System.Timers.ElapsedEventHandler(servicioImpresion);
        //    this.timer.Start();
        //    eventLog1.WriteEntry("Servicio iniciado.");
        //}
        // #else

        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
            this.timer = new System.Timers.Timer(Double.Parse(basdat.Parametros.Where(c => c.codigo == "SERV_IMPRE").First().valor));  // 30000 milliseconds = 30 seconds
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(servicioImpresion);
            this.timer.Start();
            eventLog1.WriteEntry("Servicio iniciado.");
        }

        //        #endif

        private void servicioImpresion(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //AQUI ESCRIBE EL CODIGO USA HILOS
                BaseDatosDataContext db = new BaseDatosDataContext();
                string impresoraLocal = ConfigurationManager.AppSettings["NombreZebraLocal"];

                List<CodigosBarras> Codigos = db.CodigosBarras.Where(x => x.estado == 0 && x.impresora.Trim() == impresoraLocal.Trim()).ToList<CodigosBarras>();

                eventLog1.WriteEntry("Cantidad de codigos " + Codigos.Count);

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (CodigosBarras item in Codigos)
                    {
                        eventLog1.WriteEntry("Envia el hilo " + item.IdCodBarras);               
                        Thread th = new Thread(() => new ZebraPrinter(item.impresora).Print(item.CodigoZPL));
                        th.Start();
               
                        eventLog1.WriteEntry("Sube cambios");               
                        item.estado = 1;
                        item.FechaImpreso = DateTime.Now;
                        db.SubmitChanges();
                        
                    }
                    scope.Complete();
                }
               
                eventLog1.WriteEntry("Termina ciclos");
            }
            catch (Exception es)
            {
                eventLog1.WriteEntry("error en servicioImpresion" + es.Message + " stack trace " + es.StackTrace);
                throw;
            }
        }

        protected override void OnStop()
        {
        }
    }
}
