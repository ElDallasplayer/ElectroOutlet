using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;

namespace ElectroOULET.Controllers
{
    public class ReparacionesController : Controller
    {
        public IActionResult Index(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)id);

            return View(new Reparacion().GetReparaciones());
        }
    }
}
