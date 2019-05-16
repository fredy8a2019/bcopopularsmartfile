using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ServiceImpresionGNF
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
//#if DEBUG
//            Service1 mysrv = new Service1();
//            mysrv.OnStart();
//            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite); 
//#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new Service1() 
			};
            ServiceBase.Run(ServicesToRun);
//#endif
        }
           
    }
}
