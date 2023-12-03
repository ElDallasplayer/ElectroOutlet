using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrincipalObjects.Enums;

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
            dynamic marcsFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[2] { "where marcDate >= '" + dateInit.ToString("yyyy-MM-dd 00:00:00") + "'", " marcDate <= '" + dateFinish.ToString("yyyy-MM-dd 23:59:59") + "'" }), (true, "marcDate", false));
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

        public Marcations GetMarcationByID(long id)
        {
            dynamic marcsFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where marcId = " + id }), (true, "marcDate", false));
            Marcations marc = new Marcations();

            try
            {
                marc = new Marcations()
                {
                    marcId = Convert.ToInt64(marcsFromDB.rows[0].marcId.Value.ToString()),
                    empId = Convert.ToInt64(marcsFromDB.rows[0].empId.Value.ToString()),
                    marcCard = marcsFromDB.rows[0].marcCard.Value.ToString(),
                    marcHikId = Convert.ToInt64(marcsFromDB.rows[0].marcHikId.Value.ToString()),
                    marcDirection = (Enums.mDirection)Convert.ToInt32(marcsFromDB.rows[0].marcDirection.Value.ToString()),
                    marcDate = Convert.ToDateTime(marcsFromDB.rows[0].marcDate.Value.ToString()),
                    marcEdited = Convert.ToBoolean(marcsFromDB.rows[0].marcEdited.Value.ToString()),
                    marcEditedValue = Convert.ToDateTime(String.IsNullOrEmpty(marcsFromDB.rows[0].marcEditedValue.Value.ToString()) ? "1900-01-01 00:00:00" : marcsFromDB.rows[0].marcEditedValue.Value.ToString()),
                    marcDescription = marcsFromDB.rows[0].marcDescription.Value.ToString(),
                    Deleted = Convert.ToBoolean(marcsFromDB.rows[0].marcDelete.Value.ToString()),
                    devId = Convert.ToInt32(marcsFromDB.rows[0].devId.Value.ToString())
                };

                Employee emp = new Employee().GetEmployeeById(marc.empId);
                marc.EmpleadoNombre = emp.empSurName + " ," + emp.empName;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("ERROR AL CREAR MARCACION: " + ex.Message);
                Utilities.WriteLog(" => " + marcsFromDB.rows[0].ToString());
            }

            return marc;
        }

        public Marcations GetLastMarcation()
        {
            dynamic marcsFromDB = SQLInteract.GetDataFromDataBase((true, 1), ColNames, TableName, (false, new string[0] { }), (true, "marcId", true));
            Marcations marc = new Marcations();

            try
            {
                marc = new Marcations()
                {
                    marcId = Convert.ToInt64(marcsFromDB.rows[0].marcId.Value.ToString()),
                    empId = Convert.ToInt64(marcsFromDB.rows[0].empId.Value.ToString()),
                    marcCard = marcsFromDB.rows[0].marcCard.Value.ToString(),
                    marcHikId = Convert.ToInt64(marcsFromDB.rows[0].marcHikId.Value.ToString()),
                    marcDirection = (Enums.mDirection)Convert.ToInt32(marcsFromDB.rows[0].marcDirection.Value.ToString()),
                    marcDate = Convert.ToDateTime(marcsFromDB.rows[0].marcDate.Value.ToString()),
                    marcEdited = Convert.ToBoolean(marcsFromDB.rows[0].marcEdited.Value.ToString()),
                    marcEditedValue = Convert.ToDateTime(String.IsNullOrEmpty(marcsFromDB.rows[0].marcEditedValue.Value.ToString()) ? "1900-01-01 00:00:00" : marcsFromDB.rows[0].marcEditedValue.Value.ToString()),
                    marcDescription = marcsFromDB.rows[0].marcDescription.Value.ToString(),
                    Deleted = Convert.ToBoolean(marcsFromDB.rows[0].marcDelete.Value.ToString()),
                    devId = Convert.ToInt32(marcsFromDB.rows[0].devId.Value.ToString())
                };

                Employee emp = new Employee().GetEmployeeById(marc.empId);
                marc.EmpleadoNombre = emp.empSurName + " ," + emp.empName;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("ERROR AL CREAR MARCACION: " + ex.Message);
                Utilities.WriteLog(" => " + marcsFromDB.rows[0].ToString());
            }

            return marc;
        }

        public Marcations GetLastMarcationByDate(DateTime date)
        {
            dynamic marcsFromDB = SQLInteract.GetDataFromDataBase((true, 1), ColNames, TableName, (true, new string[1] { " where marcDate = '" + date.ToString("yyyyMMdd HH:mm:ss") + "'"}), (true, "marcId", true));
            Marcations marc = new Marcations();

            if (marcsFromDB.rows.Count == 0)
            {
                return null;
            }

            try
            {
                marc = new Marcations()
                {
                    marcId = Convert.ToInt64(marcsFromDB.rows[0].marcId.Value.ToString()),
                    empId = Convert.ToInt64(marcsFromDB.rows[0].empId.Value.ToString()),
                    marcCard = marcsFromDB.rows[0].marcCard.Value.ToString(),
                    marcHikId = Convert.ToInt64(marcsFromDB.rows[0].marcHikId.Value.ToString()),
                    marcDirection = (Enums.mDirection)Convert.ToInt32(marcsFromDB.rows[0].marcDirection.Value.ToString()),
                    marcDate = Convert.ToDateTime(marcsFromDB.rows[0].marcDate.Value.ToString()),
                    marcEdited = Convert.ToBoolean(marcsFromDB.rows[0].marcEdited.Value.ToString()),
                    marcEditedValue = Convert.ToDateTime(String.IsNullOrEmpty(marcsFromDB.rows[0].marcEditedValue.Value.ToString()) ? "1900-01-01 00:00:00" : marcsFromDB.rows[0].marcEditedValue.Value.ToString()),
                    marcDescription = marcsFromDB.rows[0].marcDescription.Value.ToString(),
                    Deleted = Convert.ToBoolean(marcsFromDB.rows[0].marcDelete.Value.ToString()),
                    devId = Convert.ToInt32(marcsFromDB.rows[0].devId.Value.ToString())
                };

                Employee emp = new Employee().GetEmployeeById(marc.empId);
                marc.EmpleadoNombre = emp.empSurName + " ," + emp.empName;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("ERROR AL CREAR MARCACION: " + ex.Message);
                Utilities.WriteLog(" => " + marcsFromDB.rows[0].ToString());
            }

            return marc;
        }

        public List<Marcations> GetMarcsByEmpId(long empId, DateTime dateInit, DateTime dateFinish)
        {
            dynamic marcsFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[3] { "where empId = " + empId, " marcDate >= '" + dateInit.ToString("yyyy-MM-dd HH:mm:ss") + "'", " marcDate <= '" + dateFinish.ToString("yyyy-MM-dd HH:mm:ss") + "'" }), (true, "marcDate", false));
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

        public Marcations SaveMarcation(Marcations marcation)
        {
            List<(string, eDataType)> dataToSend = new List<(string, eDataType)>();

            Marcations marcaLast = new Marcations().GetLastMarcationByDate(marcation.marcDate);

            if (marcaLast == null)
            {
                long LastId = SQLInteract.GetLastIdFromInsertedElement(TableName, "marcId") + 1;

                dataToSend.Add((LastId.ToString(), eDataType.number));
                dataToSend.Add((marcation.empId.ToString(), eDataType.number));
                dataToSend.Add(((marcation.marcCard != null? marcation.marcCard.ToString():"Sin tarjeta"), eDataType.text));
                dataToSend.Add((marcation.marcHikId.ToString(), eDataType.text));
                dataToSend.Add((((int)marcation.marcDirection).ToString(), eDataType.number));
                dataToSend.Add((marcation.marcDate.ToString("yyyyMMdd HH:mm:ss"), eDataType.text));
                dataToSend.Add(((marcation.marcEdited ? "1" : "0"), eDataType.number));
                dataToSend.Add((marcation.marcEditedValue.ToString("yyyyMMdd HH:mm:ss"), eDataType.text));
                dataToSend.Add((marcation.marcDescription?.ToString(), eDataType.text));
                dataToSend.Add(("0", eDataType.number));
                dataToSend.Add((marcation.devId.ToString(), eDataType.number));

                bool rest = SQLInteract.InsertDataInDatabase(TableName, ColNames, dataToSend);

                if (rest)
                {
                    return marcation;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
