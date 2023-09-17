using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region dbObject
        string TableName = "oWorkShift_Segment";
        string[] ColNames = new string[7] {
            "wsId",
            "turId",
            "wsName",
            "wsDescription",
            "wsInit",
            "wsEnd",
            "wsDelete"
        };
        #endregion

        public WorkShiftSegments() { }

        public List<WorkShiftSegments> GetWorkShiftSegmentsByTurId(long turId) 
        {
            dynamic segmentsFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where turId = " + turId }), (true, "wsInit", false));
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
