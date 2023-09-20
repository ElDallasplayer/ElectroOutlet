using ClosedXML.Excel;
using PrincipalObjects.Objects;
using System.Data;
using System.Web.Mvc;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PrincipalObjects.Objects;
using System.Data;
using System.Xml.Serialization;

namespace ElectroOULET
{
    public class Reports
    {
        public static async Task<DataTable> ReporteDeMarcaciones(string employeesId)
        {
            List<Employee> empleadosToReport = new Employee().GetEmployeesToReport(employeesId);

            DataTable dataTable = new DataTable("ReporteDeMarcaciones");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("NombreEmpleado"),
                new DataColumn("Tarjeta"),
                new DataColumn("Sentido"),
                new DataColumn("Fecha"),
                new DataColumn("Hora")
            });

            foreach (Employee emp in empleadosToReport.OrderBy(x => x.empName))
            {
                dataTable.Rows.Add(emp.empName + ", " + emp.empSurName, emp.empCard, "", "", "");

                List<Marcations> marcs = new Marcations().GetMarcsByEmpId(emp.empId, DateTime.Now.AddDays(-10), DateTime.Now);

                foreach (Marcations mc in marcs.OrderBy(x => x.marcDate))
                {
                    dataTable.Rows.Add("", mc.marcCard, (mc.marcDirection == PrincipalObjects.Enums.mDirection.In? "Entrada":"Salida"), mc.marcDate.ToString("dd/MM/yyyy"), mc.marcDate.ToString("HH:mm:ss"));
                }
            }
            return dataTable;
        }
    }
}
