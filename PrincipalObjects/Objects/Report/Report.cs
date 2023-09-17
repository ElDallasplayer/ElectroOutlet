using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region dbObject
        string TableName = "oReports";
        string[] ColNames = new string[6] {
            "repId", 
            "repName", 
            "repDescription",
            "empsId", 
            "repType", 
            "repDeleted" 
        };
        #endregion

        public Report() { }

        //CONSTRUCTORS
        public Report GetReportById(long id)
        {
            dynamic repFromDB = SQLInteract.GetDataFromDataBase((false,-1),ColNames, TableName, (true,new string[1] { "where id = " + id }),(false,"",false));
            try
            {
                Report report = new Report()
                {
                    Id = Convert.ToInt64(repFromDB.rows[0].repId.Value),
                    Name = repFromDB.rows[0].repName.Value.ToString(),
                    Description = repFromDB.rows[0].repDescription.Value.ToString(),
                    EmpsIds = repFromDB.rows[0].empsId.Value.ToString(),
                    ReportType = Convert.ToInt32(repFromDB.rows[0].repType.Value.ToString()),
                    Delete = Convert.ToBoolean(repFromDB.rows[0].repDeleted.Value.ToString())
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
                            Delete = Convert.ToBoolean(report.repDeleted.Value.ToString())
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
    }
}
