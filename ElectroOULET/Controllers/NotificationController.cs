using Microsoft.AspNetCore.Mvc;

namespace ElectroOULET.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult EliminarNotificacion(int id)
        {
            return new JsonResult(new { Result = "OK", Message = "Eliminado" });
        }
    }
}
