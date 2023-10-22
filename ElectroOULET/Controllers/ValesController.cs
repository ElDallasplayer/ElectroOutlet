using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;
using PrincipalObjects;

namespace ElectroOULET.Controllers
{
    public class ValesController : Controller
    {
        public IActionResult Index(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById(id);

            List<Vale> vales = new Vale().GetVales();

            return View(vales);
        }

        public ActionResult EditVale(int id, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById(userId);

            Vale vale = new Vale();

            if (id != -1)
            {
                vale = new Vale().GetValeById((long)id);
            }
            else
            {
                vale = new Vale() { Id = -1, Eliminado = false, Fecha = DateTime.Now, Concepto = "", Monto = 0, EmpleadoCodigo = -1, Validado = false };
            }

            return View(vale);
        }

        public JsonResult GuardarVale(Vale vale, int UserId)
        {
            ViewData["ActiveUser"] = new User().GetUserById(UserId);

            Vale val = new Vale();
            //HACER EL GUARDAR REPORTE
            if (vale.Id != -1)
            {
                val = new Vale().EditarVale(vale);
            }
            else
            {
                val = new Vale().AgregarVale(vale);
            }

            return new JsonResult(new { Result = "OK", Message = "Guardado correctamente" });
        }

        public JsonResult EliminarVale(int valeId, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById(userId);

            Vale vale = new Vale().GetValeById((long)valeId);
            bool eliminado = vale.EliminarVale(vale.Id);

            return new JsonResult(new { Result = "OK", Message = "Eliminado correctamente" });
        }

        public JsonResult ValidarHuellaEmpleado(int empId)
        {
            //ViewData["ActiveUser"] = new User().GetUserById(userId);

            Employee emp = new Employee().GetEmployeeById_Huella((long)empId);

            if (String.IsNullOrEmpty(emp.HuellaBase64))
            {
                return new JsonResult(new { Result = "ERROR", Message = " El empleado no tiene huella asignada ", Huella = "" });
            }
            string HuellaValidacion = Enrolador.ValidarHuella(emp.HuellaBase64);

            dynamic jsonResult = Utilities.ConvertToDynamic(HuellaValidacion);

            if (jsonResult.Response.Value == "SUCCESS")
            {
                if(jsonResult.Resultado_Comparacion.Value)
                {
                    return new JsonResult(new { Result = "OK", Message = " Huella validada correctamente ", Huella = jsonResult.Resultado_Comparacion.Value.ToString() });
                }
                else
                {
                    return new JsonResult(new { Result = "ERROR", Message = " La huella no se pudo validar ", Huella = jsonResult.Resultado_Comparacion.Value.ToString() });
                }
            }

            return new JsonResult(new { Result = "OK", Message = "Huella validada correctamente", Huella = "" });
        }
    }
}
