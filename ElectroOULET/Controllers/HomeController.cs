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

        public string NotificacionesParaVista()
        {
            ViewData["ActiveUser"] = new User().GetUsers().FirstOrDefault();
            string htmlToReturn = "<div class='dropdown-item'>";

            string tableClass = "";
            List<Notification> notificaciones = new Notification().GetNotifications();

            if (notificaciones.Count > 0)
            {
                foreach (Notification noti in notificaciones)
                {
                    if (noti.NotificationType == PrincipalObjects.Enums.nType.Aviso)
                    {
                        tableClass = "primary";
                    }
                    else if (noti.NotificationType == PrincipalObjects.Enums.nType.Alerta)
                    {
                        tableClass = "warning";
                    }
                    else if (noti.NotificationType == PrincipalObjects.Enums.nType.Error)
                    {
                        tableClass = "danger";
                    }
                    else if (noti.NotificationType == PrincipalObjects.Enums.nType.Exitoso)
                    {
                        tableClass = "success";
                    }

                    htmlToReturn = htmlToReturn + "<div class='row p-0 m-0'><div class='col-12 p-0 m-0'>" +
                        "<table class='table table-" + tableClass + "'>" +
                        "<thead><tr><div scope='col' class='fw-bolder tableUp p-1 titlenot-" + tableClass + " text-white'>" + noti.Message.ToUpper() + "<i class='mdi mdi-close-box fw-bolder NotificationButton' id='" + noti.Id + "' style='font-size:20px; cursor: pointer;'></i></div></tr>" +
                        "</thead><tbody><tr><div class='table-light tableDown p-1  bodynot-" + tableClass + "' scope='row'>" + noti.Description + "</div></tr></tbody></table></div></div>";
                }
            }
            else
            {
                htmlToReturn = htmlToReturn + "<div class='row m-2'><div class='col-12 p-0 m-0'><div><span>Sin notificaciones</span></div></div></div>";
            }

            htmlToReturn = htmlToReturn + "</div>";

            return htmlToReturn;
        }
    }
}