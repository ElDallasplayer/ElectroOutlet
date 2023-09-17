using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;

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

            DateTime yesterday = DateTime.Today.AddDays(-2);
            DateTime today = DateTime.Today;
            List<Marcations> marcationsToView = new Marcations().GetMarcations(yesterday, today.AddDays(1));

            return View("Marcaciones", marcationsToView);
        }

        // GET: TurnoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TurnoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TurnoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TurnoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TurnoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TurnoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TurnoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
