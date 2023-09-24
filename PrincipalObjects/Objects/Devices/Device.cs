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

        #region dbObject
        string TableName = "oDevices";
        string[] ColNames = new string[8] {
            "devId",
            "devHost",
            "devPort",
            "devUser",
            "devPassword",
            "devState",
            "devDescription",
            "devDelete"
        };
        #endregion

        public List<Device> GetDevices()
        {
            dynamic marcsFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[0] { }), (false, "", false));
            List<Device> devList = new List<Device>();

            foreach (dynamic row in marcsFromDB.rows)
            {
                try
                {
                    Device marc = new Device()
                    {
                        devId = Convert.ToInt64(row.devId.Value.ToString()),
                        devHost = row.devHost.Value.ToString(),
                        devPort = row.devPort.Value.ToString(),
                        devUser = row.devUser.Value.ToString(),
                        devPassword = row.devPassword.Value.ToString(),
                        devState = Convert.ToInt32(row.devState.Value.ToString()),
                        devDescription = row.devDescription.Value.ToString(),
                        devDelete = Convert.ToBoolean(row.devDelete.Value.ToString())
                    };
                    devList.Add(marc);
                }
                catch (Exception ex)
                {
                    Utilities.WriteLog("ERROR AL CREAR DISPOSITIVO: " + ex.Message);
                    Utilities.WriteLog(" => " + row.ToString());
                }
            }

            return devList;
        }
    }
}
