using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects
{
    public class Enums
    {
        public enum eDataType
        {
            number = 0,
            text = 1
        }
        
        public enum mDirection
        {
            In = 0,
            Out = 1
        }

        public enum pType
        {
            Create = 0,
            Read = 1,
            Update = 2,
            Delete = 4
        }
    }
}
