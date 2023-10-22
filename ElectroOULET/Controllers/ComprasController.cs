using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;
using PrincipalObjects;

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

        [HttpPost]
        public JsonResult ValidarFechas(DateTime fechaDesde, DateTime fechaHasta, int userId)
        {
            string bearerToken = Contabilium.GetAuthentication("ahumadamonica@hotmail.com", "aa2cf6a8c098461090f92bf9cc96c284");

            int compras = Contabilium.ValidarComprasEntreFechas(fechaDesde, fechaHasta, bearerToken);

            return new JsonResult(new { Result = "OK", Message = "Se reportaron " + compras + "nuevas compras" });
        }
    }
}
