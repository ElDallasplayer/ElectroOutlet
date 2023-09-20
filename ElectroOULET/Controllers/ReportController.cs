using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PrincipalObjects.Objects;
using System.Data;
using System.Xml.Serialization;
using ElectroOULET;
using PrincipalObjects;
using System.Reflection;

namespace ElectroOULET.Controllers
{
    public class ReportController : Controller
    {
        // GET: ReportController
        public ActionResult Index(int id)
        {
            ViewData["ActiveUser"] = new User().GetUserById(id);

            List<Report> reports = new Report().GetReports();

            return View(reports);
        }

        public ActionResult EditReport(int id, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById(userId);

            Report report = new Report().GetReportById((long)id);

            return View(report);
        }

        public ActionResult GuardarReporte(Report reporte, string listEmps, int UserId)
        {
            ViewData["ActiveUser"] = new User().GetUserById(UserId);

            reporte.EmpsIds = listEmps;
            Report rep = new Report();
            //HACER EL GUARDAR REPORTE
            if (reporte.Id != -1)
            {
                rep = new Report().EditarReporte(reporte);
            }
            else
            {
                //INSERTAR REPORTE
            }
            List<Report> reports = reporte.GetReports();
            return View("index", reports);
        }

        public async Task<FileResult> DescargarReporte(int id)
        {
            Report report = new Report().GetReportById((long)id);

            return GenerarExcel(report.Name + "_" + DateTime.Now.ToString("ddMM_HHmm") + ".xlsx", report.EmpsIds, (Enums.Reports)report.ReportType).Result;
        }

        public async Task<FileResult> GenerarExcel(string nombreArchivo, string employeesId, Enums.Reports reportType)
        {
            string[] parameters = { employeesId };
            Type thisType = typeof(Reports);
            MethodInfo theMethod = thisType.GetMethod(reportType.ToString());
            DataTable dataTable = await (Task<DataTable>)theMethod.Invoke(null, parameters);


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nombreArchivo);
                }
            }

        }
    }
}
