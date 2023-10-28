using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrincipalObjects.Enums;

namespace PrincipalObjects.Objects
{
    public class Employee
    {
        public long empId { get; set; }
        public string empName { get; set; }
        public string empSurName { get; set; }
        public string empLegajo { get; set; }
        public string empCard { get; set; }
        public long empIdHikVision { get; set; }
        public bool empDelete { get; set; }
        public long turId { get; set; }
        public string empDocumento { get; set; }
        public int empSueldo { get; set; }
        public int empSueldoRecibo { get; set; }

        public string NombreCompleto { get; set; }

        public WorkShift Turno { get; set; }

        public string HuellaBase64 { get; set; }
        public eDedo DedoEnrolado { get; set; }

        #region dbObject
        string TableName = "oEmployee";
        string[] ColNames = new string[13] {
            "empId",
            "empName",
            "empSurname",
            "empLegajo",
            "empCard",
            "empIdHikVision",
            "empDelete",
            "turId",
            "empHuella",
            "empDedo",
            "empDocumento",
            "empSueldo",
            "empSueldoRecibo"
        };
        #endregion

        public Employee() { }

        //CONSTRUCTORS
        public Employee GetEmployeeById(long id)
        {
            dynamic userFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where empId = " + id }), (false, "", false));

            try
            {

                Employee employee = new Employee()
                {
                    empId = Convert.ToInt64(userFromDB.rows[0].empId.Value),
                    empName = userFromDB.rows[0].empName.Value.ToString(),
                    empSurName = userFromDB.rows[0].empSurname.Value.ToString(),
                    empLegajo = userFromDB.rows[0].empLegajo.Value.ToString(),
                    empCard = userFromDB.rows[0].empCard.Value.ToString(),
                    empIdHikVision = Convert.ToInt64(userFromDB.rows[0].empIdHikVision.Value.ToString()),
                    empDelete = Convert.ToBoolean(userFromDB.rows[0].empDelete.Value.ToString()),
                    empDocumento = userFromDB.rows[0].empDocumento.Value.ToString()
                };
                employee.NombreCompleto = employee.empName + ", " + employee.empSurName;
                employee.Turno = new WorkShift().GetTurById(Convert.ToInt64((String.IsNullOrEmpty(userFromDB.rows[0].turId.Value.ToString()) ? "-1" : userFromDB.rows[0].turId.Value.ToString())));
                employee.turId = Convert.ToInt64(String.IsNullOrEmpty(userFromDB.rows[0].turId.Value.ToString()) ? "-1" : userFromDB.rows[0].turId.Value.ToString());

                return employee;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public Employee GetEmployeeById_Huella(long id)
        {
            dynamic userFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where empId = " + id }), (false, "", false));

            try
            {

                Employee employee = new Employee()
                {
                    empId = Convert.ToInt64(userFromDB.rows[0].empId.Value),
                    empName = userFromDB.rows[0].empName.Value.ToString(),
                    empSurName = userFromDB.rows[0].empSurname.Value.ToString(),
                    empLegajo = userFromDB.rows[0].empLegajo.Value.ToString(),
                    empCard = userFromDB.rows[0].empCard.Value.ToString(),
                    empIdHikVision = Convert.ToInt64(userFromDB.rows[0].empIdHikVision.Value.ToString()),
                    empDelete = Convert.ToBoolean(userFromDB.rows[0].empDelete.Value.ToString()),
                    empDocumento = userFromDB.rows[0].empDocumento.Value.ToString()
                };
                employee.NombreCompleto = employee.empName + ", " + employee.empSurName;

                employee.HuellaBase64 = userFromDB.rows[0]["empHuella"].ToString();
                employee.DedoEnrolado = (eDedo)Convert.ToInt32((userFromDB.rows[0]["empDedo"] == "" ? "0" : userFromDB.rows[0]["empDedo"]));

                employee.empSueldo = Convert.ToInt32(userFromDB.rows[0]["empSueldo"] == "" ? "0" : userFromDB.rows[0]["empSueldo"]) ?? 0; 
                employee.empSueldoRecibo = Convert.ToInt32(userFromDB.rows[0]["empSueldoRecibo"] == "" ? "0" : userFromDB.rows[0]["empSueldoRecibo"]) ?? 0;

                return employee;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public List<Employee> GetEmployees_Sueldos()
        {
            dynamic employeeFromDB = SQLInteract.GetDataFromDataBase((false, -1), new string[] { "empId", "empName", "empSurname", "empLegajo", "empDocumento", "empSueldo", "empSueldoRecibo" }, TableName, (true, new string[1] { "where empDelete = 0 " }), (false, "", false));
            List<Employee> employees = new List<Employee>();
            try
            {
                foreach (dynamic row in employeeFromDB.rows)
                {
                    try
                    {
                        Employee employee = new Employee()
                        {
                            empId = Convert.ToInt64(row.empId.Value),
                            empLegajo = row.empLegajo.Value.ToString(),
                            empDocumento = row.empDocumento.Value.ToString()
                        };
                        employee.NombreCompleto = row["empName"] + ", " + row["empSurname"];

                        employee.empSueldo = Convert.ToInt32(row["empSueldo"] == "" ? "0" : row["empSueldo"]);

                        employee.empSueldoRecibo = Convert.ToInt32(row["empSueldoRecibo"] == "" ? "0" : row["empSueldoRecibo"]);

                        employees.Add(employee);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("ERROR AL CREAR EMPLEADO: " + ex.Message);
                        Utilities.WriteLog(" => " + row.ToString());
                    }
                }

                return employees;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public List<Employee> GetEmployees()
        {
            dynamic employeeFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] { }), (false, "", false));
            List<Employee> employees = new List<Employee>();
            try
            {
                foreach (dynamic row in employeeFromDB.rows)
                {
                    try
                    {
                        Employee employee = new Employee()
                        {
                            empId = Convert.ToInt64(row.empId.Value),
                            empName = row.empName.Value.ToString(),
                            empSurName = row.empSurname.Value.ToString(),
                            empLegajo = row.empLegajo.Value.ToString(),
                            empCard = row.empCard.Value.ToString(),
                            empIdHikVision = Convert.ToInt64(row.empIdHikVision.Value.ToString()),
                            empDelete = Convert.ToBoolean(row.empDelete.Value.ToString()),
                            empDocumento = row.empDocumento.Value.ToString()
                        };
                        employee.NombreCompleto = employee.empName + ", " + employee.empSurName;
                        employee.Turno = new WorkShift().GetTurById(Convert.ToInt64((String.IsNullOrEmpty(row.turId.Value.ToString()) ? "-1" : row.turId.Value.ToString())));

                        employee.empSueldo = Convert.ToInt32(row["empSueldo"] == "" ? "0" : row["empSueldo"]);

                        employee.empSueldoRecibo = Convert.ToInt32(row["empSueldoRecibo"] == "" ? "0" : row["empSueldoRecibo"]);

                        employees.Add(employee);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("ERROR AL CREAR EMPLEADO: " + ex.Message);
                        Utilities.WriteLog(" => " + row.ToString());
                    }
                }

                return employees;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public List<Employee> GetEmployeesToReport(string empleados)
        {
            empleados = empleados.TrimStart(',');
            dynamic employeeFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where empId in(" + (String.IsNullOrEmpty(empleados) ? "0" : empleados) + ")" }), (true, "empName", false));
            List<Employee> employees = new List<Employee>();
            try
            {
                foreach (dynamic row in employeeFromDB.rows)
                {
                    try
                    {
                        Employee employee = new Employee()
                        {
                            empId = Convert.ToInt64(row.empId.Value),
                            empName = row.empName.Value.ToString(),
                            empSurName = row.empSurname.Value.ToString(),
                            empLegajo = row.empLegajo.Value.ToString(),
                            empCard = row.empCard.Value.ToString(),
                            empIdHikVision = Convert.ToInt64(row.empIdHikVision.Value.ToString()),
                            empDelete = Convert.ToBoolean(row.empDelete.Value.ToString()),
                            empDocumento = row.empDocumento.Value.ToString()
                        };
                        employee.NombreCompleto = employee.empName + ", " + employee.empSurName;
                        employee.Turno = new WorkShift().GetTurById(Convert.ToInt64((String.IsNullOrEmpty(row.turId.Value.ToString()) ? "-1" : row.turId.Value.ToString())));

                        employees.Add(employee);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("ERROR AL CREAR EMPLEADO: " + ex.Message);
                        Utilities.WriteLog(" => " + row.ToString());
                    }
                }

                return employees;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public Employee SaveEmp(Employee empleado)
        {
            List<(string, eDataType)> dataToSend = new List<(string, eDataType)>();
            long LastId = SQLInteract.GetLastIdFromInsertedElement(TableName, "empId") + 1;

            dataToSend.Add((LastId.ToString(), eDataType.number));
            dataToSend.Add((empleado.empName.ToString(), eDataType.text));
            dataToSend.Add((empleado.empSurName.ToString(), eDataType.text));
            dataToSend.Add((empleado.empLegajo.ToString(), eDataType.text));
            dataToSend.Add((empleado.empCard.ToString(), eDataType.text));
            dataToSend.Add((empleado.empIdHikVision.ToString(), eDataType.text));
            dataToSend.Add((empleado.empSueldo.ToString(), eDataType.number));
            dataToSend.Add((empleado.empSueldoRecibo.ToString(), eDataType.number));
            dataToSend.Add(("0", eDataType.number));

            dataToSend.Add((empleado.turId != -1 ? empleado.turId.ToString() : "-1", eDataType.number));

            dataToSend.Add((empleado.HuellaBase64, eDataType.text));
            dataToSend.Add((((int)empleado.DedoEnrolado).ToString(), eDataType.number));
            dataToSend.Add((empleado.empDocumento, eDataType.text));

            bool rest = SQLInteract.InsertDataInDatabase(TableName, ColNames, dataToSend);

            if (rest)
            {
                return empleado;
            }
            else
            {
                return null;
            }
        }

        public Employee EditarEmpleado(Employee empleado)
        {
            List<(string, string, eDataType)> dataToSend = new List<(string, string, eDataType)>();

            dataToSend.Add(("empId", empleado.empId.ToString(), eDataType.number));
            dataToSend.Add(("empName", empleado.empName.ToString(), eDataType.text));
            dataToSend.Add(("empSurname", empleado.empSurName.ToString(), eDataType.text));
            dataToSend.Add(("empLegajo", empleado.empLegajo.ToString(), eDataType.text));
            dataToSend.Add(("empCard", empleado.empCard.ToString(), eDataType.text));
            dataToSend.Add(("empIdHikVision", empleado.empIdHikVision.ToString(), eDataType.text));
            dataToSend.Add(("empDelete", "0", eDataType.number));
            dataToSend.Add(("turId", empleado.turId.ToString(), eDataType.number));
            dataToSend.Add(("empHuella", empleado.HuellaBase64, eDataType.text));
            dataToSend.Add(("empDedo", ((int)empleado.DedoEnrolado).ToString(), eDataType.number));
            dataToSend.Add(("empDocumento", empleado.empDocumento, eDataType.text));
            dataToSend.Add(("empSueldo", empleado.empSueldo.ToString(), eDataType.number));
            dataToSend.Add(("empSueldoRecibo", empleado.empSueldoRecibo.ToString(), eDataType.number));

            bool rest = SQLInteract.UpdateDataInDataBase(TableName, dataToSend, (true, new string[1] { "empId = " + empleado.empId }));

            if (rest)
            {
                return empleado;
            }
            else
            {
                return null;
            }
        }

        public Employee GetEmployeeByCardId(string cardId)
        {
            dynamic userFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where empCard = '" + cardId + "'" }), (false, "", false));

            try
            {
                Employee employee = new Employee()
                {
                    empId = Convert.ToInt64(userFromDB.rows[0].empId.Value),
                    empName = userFromDB.rows[0].empName.Value.ToString(),
                    empSurName = userFromDB.rows[0].empSurname.Value.ToString(),
                    empLegajo = userFromDB.rows[0].empLegajo.Value.ToString(),
                    empCard = userFromDB.rows[0].empCard.Value.ToString(),
                    empIdHikVision = Convert.ToInt64(userFromDB.rows[0].empIdHikVision.Value.ToString()),
                    empDelete = Convert.ToBoolean(userFromDB.rows[0].empDelete.Value.ToString()),
                    empDocumento = userFromDB.rows[0].empDocumento.Value.ToString()
                };
                employee.NombreCompleto = employee.empName + ", " + employee.empSurName;
                employee.Turno = new WorkShift().GetTurById(Convert.ToInt64((String.IsNullOrEmpty(userFromDB.rows[0].turId.Value.ToString()) ? "-1" : userFromDB.rows[0].turId.Value.ToString())));
                employee.turId = Convert.ToInt64(String.IsNullOrEmpty(userFromDB.rows[0].turId.Value.ToString()) ? "-1" : userFromDB.rows[0].turId.Value.ToString());

                return employee;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public bool DeleteEmployee(int id)
        {
            bool isDelete = SQLInteract.DeleteDataInDatabase(TableName, (true, new string[1] { "empId = " + id }));
            return isDelete;
        }
    }
}
