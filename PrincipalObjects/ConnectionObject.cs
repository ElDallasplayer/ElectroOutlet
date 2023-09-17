using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects
{
    public class ConnectionObject
    {
        public string Instance { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool Security { get; set; }

        public ConnectionObject()
        {
        }

        public ConnectionObject GetConnection()
        {
            dynamic jsonObject = Utilities.GetDataFromConfig();
            this.Instance = (jsonObject.instance.Value).ToString().Replace("/", @"\");
            this.Database = (jsonObject.dataBase.Value).ToString();
            this.User = (jsonObject.user.Value).ToString();
            this.Password = (jsonObject.password.Value).ToString();
            this.Security = false;
            return this;
        }
    }
}
