using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects.Objects
{
    public class Marcations
    {
        public long marcId { get; set; }
        public long empId { get; set; }
        public string marcCard { get; set; }
        public long marcHikId { get; set; }
        public Enums.mDirection marcDirection { get; set; }
        public DateTime marcDate { get; set; }
        public bool marcEdited { get; set; }
        public DateTime marcEditedValue { get; set; }
        public string marcDescription { get; set; }
        public bool Deleted { get; set; }
        public long devId { get; set; }

        public string EmpleadoNombre { get; set; }

        #region dbObject
        string TableName = "oMarcations";
        string[] ColNames = new string[11] {
            "marcId",
            "empId",
            "marcCard",
            "marcHikId",
            "marcDirection",
            "marcDate",
            "marcEdited",
            "marcEditedValue",
            "marcDescription",
            "marcDelete",
            "devId"
        };
        #endregion

        public Marcations() { }

        public List<Marcations> GetMarcations(DateTime dateInit, DateTime dateFinish)
        {
            dynamic marcsFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[2] {"where marcDate >= '" + dateInit.ToString("yyyy-MM-dd HH:mm:ss") + "'", " marcDate <= '" + dateFinish.ToString("yyyy-MM-dd HH:mm:ss") + "'" }), (true, "marcDate", false));
            List<Marcations> marcList = new List<Marcations>();

            foreach (dynamic row in marcsFromDB.rows)
            {
                try
                {
                    Marcations marc = new Marcations()
                    {
                        marcId = Convert.ToInt64(row.marcId.Value.ToString()),
                        empId = Convert.ToInt64(row.empId.Value.ToString()),
                        marcCard = row.marcCard.Value.ToString(),
                        marcHikId = Convert.ToInt64(row.marcHikId.Value.ToString()),
                        marcDirection = (Enums.mDirection)Convert.ToInt32(row.marcDirection.Value.ToString()),
                        marcDate = Convert.ToDateTime(row.marcDate.Value.ToString()),
                        marcEdited = Convert.ToBoolean(row.marcEdited.Value.ToString()),
                        marcEditedValue = Convert.ToDateTime(String.IsNullOrEmpty(row.marcEditedValue.Value.ToString()) ? "1900-01-01 00:00:00" : row.marcEditedValue.Value.ToString()),
                        marcDescription = row.marcDescription.Value.ToString(),
                        Deleted = Convert.ToBoolean(row.marcDelete.Value.ToString()),
                        devId = Convert.ToInt32(row.devId.Value.ToString())
                    };

                    Employee emp = new Employee().GetEmployeeById(marc.empId);
                    marc.EmpleadoNombre = emp.empSurName + " ," + emp.empName;
                    marcList.Add(marc);
                }
                catch (Exception ex)
                {
                    Utilities.WriteLog("ERROR AL CREAR MARCACION: " + ex.Message);
                    Utilities.WriteLog(" => " + row.ToString());
                }
            }

            return marcList;
        }

        public List<Marcations> GetMarcsByEmpId(long empId,DateTime dateInit,DateTime dateFinish)
        {
            dynamic marcsFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[3] { "where empId = " + empId, " marcDate >= '" + dateInit.ToString("yyyy-MM-dd HH:mm:ss")+"'", " marcDate <= '" + dateFinish.ToString("yyyy-MM-dd HH:mm:ss") + "'" }), (true, "marcDate", false));
            List<Marcations> marcList = new List<Marcations>();

            foreach (dynamic row in marcsFromDB.rows)
            {
                try
                {
                    Marcations marc = new Marcations()
                    {
                        marcId = Convert.ToInt64(row.marcId.Value.ToString()),
                        empId = Convert.ToInt64(row.empId.Value.ToString()),
                        marcCard = row.marcCard.Value.ToString(),
                        marcHikId = Convert.ToInt64(row.marcHikId.Value.ToString()),
                        marcDirection = (Enums.mDirection)Convert.ToInt32(row.marcDirection.Value.ToString()),
                        marcDate = Convert.ToDateTime(row.marcDate.Value.ToString()),
                        marcEdited = Convert.ToBoolean(row.marcEdited.Value.ToString()),
                        marcEditedValue = Convert.ToDateTime(String.IsNullOrEmpty(row.marcEditedValue.Value.ToString())?"1900-01-01 00:00:00": row.marcEditedValue.Value.ToString()),
                        marcDescription = row.marcDescription.Value.ToString(),
                        Deleted = Convert.ToBoolean(row.marcDelete.Value.ToString()),
                        devId = Convert.ToInt32(row.devId.Value.ToString())
                    };

                    Employee emp = new Employee().GetEmployeeById(marc.empId);
                    marc.EmpleadoNombre = emp.empSurName + " ," + emp.empName;
                    marcList.Add(marc);
                }
                catch (Exception ex)
                {
                    Utilities.WriteLog("ERROR AL CREAR MARCACION: " + ex.Message);
                    Utilities.WriteLog(" => " + row.ToString());
                }
            }

            return marcList;
        }

    }
}
