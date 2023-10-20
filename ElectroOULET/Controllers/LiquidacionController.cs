using ElectroOULET.Models;
using Microsoft.AspNetCore.Mvc;
using PrincipalObjects.Objects;
using PrincipalObjects;

namespace ElectroOULET.Controllers
{
    public class LiquidacionController : Controller
    {
        public ActionResult Index(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)id);

            return View();
        }

        public ActionResult _tablaLiquidacion(List<ArchivoViewModel> archivosParaLiquidar)
        {
            return View(archivosParaLiquidar);
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile FormFile, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById((long)userId);
            List<Employee> empleados = new Employee().GetEmployees();
            List<ArchivoViewModel> archivosParaLiquidar = new List<ArchivoViewModel>();
            if (FormFile.Length > 0)
            {
                Stream read = FormFile.OpenReadStream();
                StreamReader reader = new StreamReader(read);

                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        if (line.Contains(","))
                        {
                            string[] datosLine = line.Split(',');
                            ArchivoViewModel linea = new ArchivoViewModel();//CREAR OBJETO Y PASAR A MVC
                            linea.DNI = datosLine[0];
                            linea.Sueldo = Convert.ToInt32(datosLine[1]);

                            linea.NombreEmpleado = empleados.Where(x => x.empDocumento == linea.DNI).FirstOrDefault()?.NombreCompleto ?? "Sin Asignar";
                            linea.MontoTotalAPagar = linea.Sueldo;
                            if (linea.NombreEmpleado != "Sin Asignar")
                            {
                                long empiId = empleados.Where(x => x.empDocumento == linea.DNI).FirstOrDefault().empId;
                                List<Compra> compras = new Compra().GetComprasByEmpleadoId(empiId);
                                foreach(Compra cadaCompra in compras)
                                {
                                    linea.MontoTotalAPagar = linea.MontoTotalAPagar - cadaCompra.comTotalNeto;
                                }

                                List<Vale> vales = new Vale().GetValesByEmpId(empiId);
                                foreach (Vale cadaVale in vales)
                                {
                                    linea.MontoTotalAPagar = linea.MontoTotalAPagar - cadaVale.Monto;
                                }

                            }

                            archivosParaLiquidar.Add(linea);
                        }
                    }catch (Exception ex)
                    {
                        Utilities.WriteLog("ERROR EN DATO: " + line);
                        Utilities.WriteLog("ERROR EN DATO: " + ex.Message);
                    }
                }

                return View("Partials/_tablaLiquidacion", archivosParaLiquidar);

            }

            return View();
        }
    }
}
