using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string NombreCompleto { get; set; }

        public WorkShift Turno { get; set; }

        #region dbObject
        string TableName = "oEmployee";
        string[] ColNames = new string[7] {
            "empId",
            "empName",
            "empSurname",
            "empLegajo",
            "empCard",
            "empIdHikVision",
            "empDelete"
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
                    empDelete = Convert.ToBoolean(userFromDB.rows[0].empDelete.Value.ToString())
                };
                employee.NombreCompleto = employee.empName + ", " + employee.empSurName;
                employee.Turno = new WorkShift().GetTurByEmpId(employee.empId);
                
                return employee;
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
                            empDelete = Convert.ToBoolean(row.empDelete.Value.ToString())
                        };
                        employee.NombreCompleto = employee.empName + ", " + employee.empSurName;
                        employee.Turno = new WorkShift().GetTurByEmpId(employee.empId);

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
            dynamic employeeFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where empId in(" + empleados +")" }), (true, "empName", false));
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
                            empDelete = Convert.ToBoolean(row.empDelete.Value.ToString())
                        };
                        employee.NombreCompleto = employee.empName + ", " + employee.empSurName;
                        employee.Turno = new WorkShift().GetTurByEmpId(employee.empId);

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
    }
}
