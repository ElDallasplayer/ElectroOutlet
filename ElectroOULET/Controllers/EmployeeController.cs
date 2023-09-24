using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;

namespace ElectroOULET.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: EmployeeController
        public ActionResult Index(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById(id);

            List<Employee> employees = new Employee().GetEmployees();

            return View(employees);
        }

        public ActionResult ObtenerEmpleadosGrid()
        {
            List<Employee> employees = new Employee().GetEmployees();

            return View("Partials/_employees",employees);
        }

        public ActionResult EditEmployee(int id, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)userId);

            Employee employee = new Employee(); ;
            employee.empId = -1;
            employee.Turno = new WorkShift() { turId = -1 };
            employee.turId = -1;
            if (id != -1)
            {
                employee = new Employee().GetEmployeeById(id);
            }

            return View(employee);
        }

        public ActionResult GuardarEmpleado(Employee employeeToSave, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)userId);
            Employee employees = employeeToSave;
            if(employeeToSave.empId != -1)
            {
                employees = employees.EditarEmpleado(employees);
            }
            else
            {
                if(employeeToSave.turId == -1|| employeeToSave.turId == 0)
                {
                    employeeToSave.turId = -1;
                }
                employees = employees.SaveEmp(employees);
            }

            return View("Index", new Employee().GetEmployees());
        }
    }
}
