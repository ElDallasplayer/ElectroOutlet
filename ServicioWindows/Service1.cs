using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicioWindows
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread funcion = new Thread(new ParameterizedThreadStart(delegate
            {
                Task<bool> task = Service.StartService();
            }));
            funcion.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
