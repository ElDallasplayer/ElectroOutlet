using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Linq;
using Servicio.Enroler.Server;

namespace Servicio.Enrolador
{
    public static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Any(x => x.Contains("-window")))
            {
                ServerListener.Start();
                //Ejecución en ventana
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                ServerListener.Start();
                //Ejecución como Servicio de Windows
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new ENRService("-service") }; //DONDE SE IMPLEMENTA EL SERVICIO
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
