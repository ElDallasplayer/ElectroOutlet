using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrincipalObjects.Enums;

namespace PrincipalObjects.Objects
{
    public class Report
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmpsIds { get; set; }
        public int ReportType { get; set; }
        public bool Delete { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        #region dbObject
        string TableName = "oReports";
        string[] ColNames = new string[8] {
            "repId", 
            "repName", 
            "repDescription",
            "empsId", 
            "repType", 
            "repDeleted",
            "repFechaInicio",
            "repFechaFin"
        };
        #endregion

        public Report() { }

        //CONSTRUCTORS
        public Report GetReportById(long id)
        {
            dynamic repFromDB = SQLInteract.GetDataFromDataBase((false,-1),ColNames, TableName, (true,new string[1] { "where repId = " + id }),(false,"",false));
            try
            {
                Report report = new Report()
                {
                    Id = Convert.ToInt64(repFromDB.rows[0].repId.Value),
                    Name = repFromDB.rows[0].repName.Value.ToString(),
                    Description = repFromDB.rows[0].repDescription.Value.ToString(),
                    EmpsIds = repFromDB.rows[0].empsId.Value.ToString(),
                    ReportType = Convert.ToInt32(repFromDB.rows[0].repType.Value.ToString()),
                    Delete = Convert.ToBoolean(repFromDB.rows[0].repDeleted.Value.ToString()),
                    FechaInicio = Convert.ToDateTime(String.IsNullOrEmpty(repFromDB.rows[0].repFechaInicio.Value.ToString()) ? "1900-01-01 00:00:00" : repFromDB.rows[0].repFechaInicio.Value.ToString()),
                    FechaFin = Convert.ToDateTime(String.IsNullOrEmpty(repFromDB.rows[0].repFechaInicio.Value.ToString()) ? "1900-01-01 00:00:00" : repFromDB.rows[0].repFechaInicio.Value.ToString())
                };

                return report;
            }catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public List<Report> GetReports()
        {
            dynamic repFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] {}), (false, "", false));
            List<Report> reports = new List<Report>();

            try
            {
                foreach (dynamic report in repFromDB.rows)
                {
                    try
                    {
                        Report repToList = new Report()
                        {
                            Id = Convert.ToInt64(report.repId.Value),
                            Name = report.repName.Value.ToString(),
                            Description = report.repDescription.Value.ToString(),
                            EmpsIds = report.empsId.Value.ToString(),
                            ReportType = Convert.ToInt32(report.repType.Value.ToString()),
                            Delete = Convert.ToBoolean(report.repDeleted.Value.ToString()),
                            FechaInicio = Convert.ToDateTime(String.IsNullOrEmpty(report.repFechaInicio.Value.ToString())? "1900-01-01 00:00:00": report.repFechaInicio.Value.ToString()),
                            FechaFin = Convert.ToDateTime(String.IsNullOrEmpty(report.repFechaInicio.Value.ToString())? "1900-01-01 00:00:00": report.repFechaInicio.Value.ToString())
                        };
                        reports.Add(repToList);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("FALLO AL CREAR USUARIO: " + ex.Message);
                    }
                }

                return reports;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return new List<Report>(); //SI LA LISTA VIENE VACIA, ES POR QUE ALGO ESTA MAL
            }
        }

        public Report EditarReporte(Report reporteAGuardar)
        {
            try
            {
                List<(string, string, eDataType)> datosAGuardar = new List<(string, string, eDataType)>();
                datosAGuardar.Add(("repName", reporteAGuardar.Name, eDataType.text));
                datosAGuardar.Add(("repDescription", reporteAGuardar.Description, eDataType.text));
                datosAGuardar.Add(("empsId", reporteAGuardar.EmpsIds, eDataType.text));
                datosAGuardar.Add(("repType", reporteAGuardar.ReportType.ToString(), eDataType.number));
                datosAGuardar.Add(("repDeleted", (reporteAGuardar.Delete?"1":"0"), eDataType.number));
                datosAGuardar.Add(("repFechaInicio", reporteAGuardar.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss"), eDataType.text));
                datosAGuardar.Add(("repFechaFin", reporteAGuardar.FechaFin.ToString("yyyy-MM-dd HH:mm:ss"), eDataType.text));

                bool repGuardado = SQLInteract.UpdateDataInDataBase(TableName, datosAGuardar, (true, new string[1] { "repId = " + reporteAGuardar.Id }));

                if (repGuardado)
                {
                    return reporteAGuardar;
                }
                else
                {
                    return new Report() { Id = -1 };
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return new Report() { Id = -1}; //SI LA LISTA VIENE VACIA, ES POR QUE ALGO ESTA MAL
            }
        }

        public Report AgregarReporte(Report reporteAGuardar)
        {
            try
            {
                long LastId = SQLInteract.GetLastIdFromInsertedElement(TableName, "repId") + 1;

                List<(string, eDataType)> datosAGuardar = new List<(string, eDataType)>();
                datosAGuardar.Add((LastId.ToString(), eDataType.number));
                datosAGuardar.Add((reporteAGuardar.Name, eDataType.text));
                datosAGuardar.Add((reporteAGuardar.Description, eDataType.text));
                datosAGuardar.Add((reporteAGuardar.EmpsIds, eDataType.text));
                datosAGuardar.Add((reporteAGuardar.ReportType.ToString(), eDataType.number));
                datosAGuardar.Add(((reporteAGuardar.Delete ? "1" : "0"), eDataType.number));
                datosAGuardar.Add((reporteAGuardar.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss"), eDataType.text));
                datosAGuardar.Add((reporteAGuardar.FechaFin.ToString("yyyy-MM-dd HH:mm:ss"), eDataType.text));

                bool repGuardado = SQLInteract.InsertDataInDatabase(TableName, ColNames, datosAGuardar);

                if (repGuardado)
                {
                    return reporteAGuardar;
                }
                else
                {
                    return new Report() { Id = -1 };
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return new Report() { Id = -1 }; //SI LA LISTA VIENE VACIA, ES POR QUE ALGO ESTA MAL
            }
        }

        public bool EliminarReporte(long id)
        {
            bool isDelete = SQLInteract.DeleteDataInDatabase(TableName, (true, new string[1] { "repId = " + id }));
            return isDelete;
        }
    }
}
