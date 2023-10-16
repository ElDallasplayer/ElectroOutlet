using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;

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
    }
}
