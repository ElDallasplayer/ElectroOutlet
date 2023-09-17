using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects.Objects
{
    public class Notification
    {
        public long Id { get; set; }
        public long userId { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public Enums.nType NotificationType { get; set; }

        #region dbObject
        string TableName = "oNotifications";
        string[] ColNames = new string[5] {
            "notId",
            "userId",
            "notMessage",
            "notDescription",
            "notType"
        };
        #endregion

        public Notification() { }

        public List<Notification> GetNotifications()
        {
            dynamic notiFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] { }), (false, "", false));
            List<Notification> notifications = new List<Notification>();
            try
            {
                foreach (dynamic row in notiFromDB.rows)
                {
                    try
                    {
                        Notification noti = new Notification()
                        {
                            Id = Convert.ToInt64(row.notId.Value.ToString()),
                            userId = Convert.ToInt64(row.userId.Value.ToString()),
                            Message = row.notMessage.Value.ToString(),
                            Description = row.notDescription.Value.ToString(),
                            NotificationType = (Enums.nType)Convert.ToInt32(row.notType.Value.ToString())
                        };
                        notifications.Add(noti);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("ERROR AL CREAR SEGMENTO: " + ex.Message);
                        Utilities.WriteLog(" => " + row.ToString());
                    }
                }

                return notifications;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }
    }
}
