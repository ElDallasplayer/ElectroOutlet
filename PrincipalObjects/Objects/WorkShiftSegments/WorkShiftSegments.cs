using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrincipalObjects.Enums;

namespace PrincipalObjects.Objects
{
    public class WorkShiftSegments
    {
        public long turId { get; set; }
        public long wsId { get; set; }
        public string wsName { get; set; }
        public string wsDescription { get; set; }
        public DateTime wsInit { get; set; }
        public DateTime wsEnd { get; set; }
        public bool wsDelete { get; set; }
        public int wsDay { get; set; } //DEL 0 AL 6 => 0;Lunes,1:Martes,2:Miercoles,3:Jueves,4:Viernes,5:Sabado,6:Domingo
        public int wsOrder { get; set; }

        #region dbObject
        string TableName = "oWorkShift_Segment";
        string[] ColNames = new string[9] {
            "wsId",
            "turId",
            "wsName",
            "wsDescription",
            "wsInit",
            "wsEnd",
            "wsDelete",
            "swDay",
            "swOrder"
        };
        #endregion

        public WorkShiftSegments() { }

        public List<WorkShiftSegments> GetWorkShiftSegmentsByTurId(long turId, eDayWeek dayOfWeek) 
        {
            dynamic segmentsFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[2] { "where turId = " + turId, " swDay = " + (int)dayOfWeek }), (true, "wsInit", false));
            List<WorkShiftSegments> segments = new List<WorkShiftSegments>();
            try
            {
                foreach(dynamic row in segmentsFromDB.rows)
                {
                    try
                    {
                        WorkShiftSegments workShiftSegment = new WorkShiftSegments()
                        {
                            wsId = Convert.ToInt64(row.wsId.Value.ToString()),
                            turId = Convert.ToInt64(row.turId.Value.ToString()),
                            wsName = row.wsName.Value.ToString(),
                            wsDescription = row.wsDescription.Value.ToString(),
                            wsInit = Convert.ToDateTime(row.wsInit.Value.ToString()),
                            wsEnd = Convert.ToDateTime(row.wsEnd.Value.ToString()),
                            wsDelete = Convert.ToBoolean(row.wsDelete.Value.ToString()),
                            wsDay = Convert.ToInt32(row.swDay.Value.ToString()),
                            wsOrder = Convert.ToInt32(row.swOrder.Value.ToString()),
                        };
                        segments.Add(workShiftSegment);
                    }
                    catch(Exception ex)
                    {
                        Utilities.WriteLog("ERROR AL CREAR SEGMENTO: " + ex.Message);
                        Utilities.WriteLog(" => " + row.ToString());
                    }
                }

                return segments;
            }catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }
    }
}
