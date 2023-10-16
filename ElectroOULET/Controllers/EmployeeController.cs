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

            return View(employees.Where(x => !x.empDelete).ToList());
        }

        public ActionResult ObtenerEmpleadosGrid()
        {
            List<Employee> employees = new Employee().GetEmployees();

            return View("Partials/_employees", employees.Where(x => !x.empDelete).ToList());
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

        public JsonResult GuardarEmpleado(Employee employeeToSave, int userId)
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

            return new JsonResult(new { Result = "OK", Message = "Empleado guardado con exito"});
        }

        public JsonResult EliminarEmpleado(int empleadoId, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)userId);
            bool eliminado = new Employee().DeleteEmployee(empleadoId);

            if (eliminado)
            {
                return new JsonResult(new { Result = "OK", Message = "Empleado eliminado correctamente" });
            }
            else
            {
                return new JsonResult(new { Result = "ERROR", Message = "Error al eliminar" });
            }
        }
    }
}
