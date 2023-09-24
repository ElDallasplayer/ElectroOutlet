using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;
using static PrincipalObjects.Enums;

namespace ElectroOULET.Controllers
{
    public class TurnoController : Controller
    {
        // GET: TurnoController
        public ActionResult Index(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById(id);

            return View();
        }

        public ActionResult WorkShiftList(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById(id);

            List<WorkShift> workShifts = new WorkShift().GetTurnos();

            return View("Turnos", workShifts);
        }

        public ActionResult MarcationsList(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById(id);

            DateTime yesterday = DateTime.Today.AddDays(-30);
            DateTime today = DateTime.Today;
            List<Marcations> marcationsToView = new Marcations().GetMarcations(yesterday, today.AddDays(1));

            return View("Marcaciones", marcationsToView);
        }

        public ActionResult EditTurno(int id, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById(userId);

            WorkShift workShift = new WorkShift().GetTurById(id);

            return View(workShift);
        }

        public ActionResult GuardarTurno(WorkShift workToSave, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById(userId);

            WorkShift workShift = workToSave;

            return View(workShift);
        }

        public IActionResult LoadSegment((int,int) idTurnoIdDay)
        {
            WorkShift workShift = new WorkShift().GetTurById(idTurnoIdDay.Item1);
            ViewBag.Day = (eDayWeek)idTurnoIdDay.Item2;

            return PartialView("Partials/_segSelector", workShift);
        }
    }
}
