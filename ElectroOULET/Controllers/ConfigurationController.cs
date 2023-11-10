using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;

namespace ElectroOULET.Controllers
{
    public class ConfigurationController : Controller
    {
        public IActionResult Index(User activeUser)
        {
            ViewData["ActiveUser"] = new User().GetUserById(activeUser.Id);

            return View();
        }

        public IActionResult EditConfiguration(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)id);

            return View("PrincipalConfigurations");
        }

        public IActionResult EditUsers(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)id);

            return View("UserList");
        }
    }
}
