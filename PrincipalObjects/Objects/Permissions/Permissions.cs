using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects.Objects
{
    public class Permissions
    {
        public long User_Id { get; set; }
        public long Id { get; set; }
        public Enums.pType Type { get; set; }
        public string Decription { get; set; }
    }
}
