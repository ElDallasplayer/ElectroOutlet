using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;

namespace ElectroOULET.Controllers
{
    public class ComprasController : Controller
    {
        public ActionResult Index(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById(id);

            List<Compra> compras = new Compra().GetCompras(false, DateTime.Now, DateTime.Now);

            return View(compras);
        }
    }
}
