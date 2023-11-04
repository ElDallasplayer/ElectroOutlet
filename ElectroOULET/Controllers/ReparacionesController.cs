using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;

namespace ElectroOULET.Controllers
{
    public class ReparacionesController : Controller
    {
        public ActionResult Index(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)id);

            return View(new Reparacion().GetReparacionesMaxValue(400,0));
        }

        public ActionResult EditReparacion(int id, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)userId);

            Reparacion reparacion = new Reparacion(); ;
            reparacion.Id = -1;
            reparacion.Fecha = DateTime.Now;
            if (id != -1)
            {
                ViewData["EsAgregado"] = 0;
                reparacion = new Reparacion().GetReparacionById(id);
            }
            else
            {
                ViewData["EsAgregado"] = 1;
            }

            return View(reparacion);
        }

        public JsonResult GuardarReparacion(Reparacion reparacion, int userId, int esAgregar)
        {
            try
            {

                ViewData["ActiveUser"] = new User().GetUserById((long)userId);

                Reparacion repToSave = reparacion;
                repToSave.Usuario = userId;

                if (repToSave.Id != -1)
                {
                    repToSave = repToSave.EditarReparacion(repToSave);
                }
                else
                {
                    repToSave = repToSave.SaveReparacion(repToSave);

                    if (repToSave != null)
                    {
                        return new JsonResult(new { Result = "OK", Message = "Registro agregado correctamente" });
                    }
                }

                return new JsonResult(new { Result = "OK", Message = "Registro editado correctamente" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Result = "ERROR", Message = "Error al guardar el registro" });
            }
        }

        public JsonResult EliminarReparacion(int reparacionId, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)userId);
            bool eliminado = new Reparacion().DeleteReparacion(reparacionId);

            if (eliminado)
            {
                return new JsonResult(new { Result = "OK", Message = "Registro eliminado correctamente" });
            }
            else
            {
                return new JsonResult(new { Result = "ERROR", Message = "Error al eliminar" });
            }
        }

        public JsonResult AgregarCodigoProducto(string nombreDeCodigo)
        {
            CodigoProducto cod = new CodigoProducto().AgregarCodigoDeProducto(nombreDeCodigo);

            return new JsonResult(new { Result = "OK", Message = "Guardado correctamente", Codigo = cod.Id, Nombre = cod.CodProducto });
        }
    }
}
