using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects.Objects
{
    public class Device
    {
        public long devId { get; set; }
        public string devHost { get; set; }
        public string devPort { get; set; }
        public string devUser { get; set; }
        public string devPassword { get; set; }
        public int devState { get; set; }
        public string devDescription { get; set; }
        public bool devDelete { get; set; }
    }
}
