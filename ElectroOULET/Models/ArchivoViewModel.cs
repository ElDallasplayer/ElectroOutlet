using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroOULET.Models
{
    public class ArchivoViewModel
    {
        public string DNI { get; set; }
        public int Sueldo { get; set; }

        public string NombreEmpleado { get; set; }
        public int MontoTotalAPagar { get; set; }
    }
}
