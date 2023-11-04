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
using OfficeOpenXml.Style;

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

            Report report = new Report();

            if (id != -1)
            {
                report = new Report().GetReportById((long)id);
            }
            else
            {
                report = new Report() { Delete = false, FechaInicio = DateTime.Now.AddDays(-15), FechaFin = DateTime.Now, ReportType = 1, Name = "", Description = "", EmpsIds = "0" };
            }

            return View(report);
        }

        public JsonResult EliminarReporte(int id, int userId)
        {
            ViewData["ActiveUser"] = new User().GetUserById(userId);

            Report report = new Report().GetReportById((long)id);
            bool eliminado = report.EliminarReporte(report.Id);

            return new JsonResult(new { Result = "OK", Message = "Eliminado correctamente" });
        }

        public JsonResult GuardarReporte(Report reporte, string listEmps, int UserId)
        {
            ViewData["ActiveUser"] = new User().GetUserById(UserId);

            reporte.EmpsIds = listEmps;
            Report rep = new Report();
            //HACER EL GUARDAR REPORTE
            if (reporte.Id != 0)
            {
                rep = new Report().EditarReporte(reporte);
            }
            else
            {
                rep = new Report().AgregarReporte(reporte);
            }
            //List<Report> reports = reporte.GetReports();
            return new JsonResult(new { Result = "OK", Message = "Guardado correctamente" });
        }


        public async Task<FileResult> DescargarReporte(int id, string dateInit, string dateEnd)
        {
            Report report = new Report().GetReportById((long)id);

            return GenerarExcel(report.Name + "_" + DateTime.Now.ToString("ddMM_HHmm") + ".xlsx", report.EmpsIds, (Enums.Reports)report.ReportType, dateInit, dateEnd).Result;
        }

        public async Task<FileResult> GenerarExcel(string nombreArchivo, string employeesId, Enums.Reports reportType, string initDate, string endDate)
        {
            DataTable dataTable = new DataTable();
            List<DataTable> listData = new List<DataTable>();
            DateTime dateInit = Convert.ToDateTime(initDate);
            DateTime dateEnd = Convert.ToDateTime(endDate);

            switch (reportType)
            {
                case Enums.Reports.ReporteDeMarcaciones:
                    dataTable = await Reports.ReporteDeMarcaciones(employeesId, dateInit, dateEnd); break;
                case Enums.Reports.ReporteDeHorasPorDia:
                    dataTable = await Reports.ReporteDeHorasPorPeriodoPorDia(employeesId, dateInit, dateEnd); break;
                case Enums.Reports.ReporteDeHorasPorPeriodo:
                    dataTable = await Reports.ReporteDeHorasPorPeriodo(employeesId, dateInit, dateEnd); break;
                case Enums.Reports.ReporteDeRegistros:
                    listData = await Reports.ReporteDeRegistros(employeesId, dateInit, dateEnd); break;
                case Enums.Reports.ReporteDeRegistrosReducido:
                    dataTable = await Reports.ReporteDeRegistrosReducido(employeesId, dateInit, dateEnd); break;
            }

            if (listData.Count > 0)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    foreach (DataTable dataTable1 in listData)
                    {
                        string spreedsheetname = dataTable1.Columns[0].ColumnName.ToString();

                        IXLWorksheet ws = null;
                        try
                        {
                            ws = wb.Worksheets.Add(spreedsheetname);
                        }
                        catch (Exception ex)
                        {
                            ws = wb.Worksheets.Add(spreedsheetname + Guid.NewGuid());
                        }

                        //wb.Worksheets.Add(spreedsheetname);

                        for (int c = 1; c <= dataTable1.Columns.Count; c++)
                        {
                            ws.Cell(1, c).Style.Fill.BackgroundColor = XLColor.FromArgb(202, 207, 210);
                            ws.Cell(1, c).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            ws.Cell(1, c).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            ws.Cell(1, c).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            ws.Cell(1, c).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            ws.Cell(1, c).Value = dataTable1.Columns[c - 1].ToString();
                        }

                        int rowInicio = 2;
                        for (int i = 0; i < dataTable1.Rows.Count; i++)
                        {
                            for (int c = 1; c <= dataTable1.Columns.Count; c++)
                            {
                                ws.Cell(rowInicio + i, c).Style.Fill.BackgroundColor = XLColor.FromArgb(247, 247, 247);
                                ws.Cell(rowInicio + i, c).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                ws.Cell(rowInicio + i, c).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                ws.Cell(rowInicio + i, c).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                ws.Cell(rowInicio + i, c).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                ws.Cell(rowInicio + i, c).Value = dataTable1.Rows[i][c - 1].ToString();
                            }
                        }

                        ws.Columns().AdjustToContents();
                    }

                    //wb.Worksheets.Add(dataTable);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            nombreArchivo);
                    }
                }
            }
            else
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Using Colors");

                    for (int c = 1; c <= dataTable.Columns.Count; c++)
                    {
                        ws.Cell(1, c).Style.Fill.BackgroundColor = XLColor.FromArgb(202, 207, 210);
                        ws.Cell(1, c).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        ws.Cell(1, c).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        ws.Cell(1, c).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        ws.Cell(1, c).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        ws.Cell(1, c).Value = dataTable.Columns[c - 1].ToString();
                    }

                    int rowInicio = 2;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        for (int c = 1; c <= dataTable.Columns.Count; c++)
                        {
                            ws.Cell(rowInicio + i, c).Style.Fill.BackgroundColor = XLColor.FromArgb(234, 237, 237);
                            ws.Cell(rowInicio + i, c).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            ws.Cell(rowInicio + i, c).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            ws.Cell(rowInicio + i, c).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            ws.Cell(rowInicio + i, c).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            ws.Cell(rowInicio + i, c).Value = dataTable.Rows[i][c - 1].ToString();
                        }
                    }

                    ws.Columns().AdjustToContents();

                    //wb.Worksheets.Add(dataTable);

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
}
