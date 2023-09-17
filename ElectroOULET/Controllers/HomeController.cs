using ElectroOULET.Models;
using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;
using System.Diagnostics;

namespace ElectroOULET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(User activeUser)
        {
            ViewData["ActiveUser"] = new User().GetUserById(activeUser.Id);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}