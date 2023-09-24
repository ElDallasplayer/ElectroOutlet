using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects.Objects
{
    public class WorkShift
    {
        public long empId { get; set; }
        public long turId { get; set; }
        public string turName { get; set; }
        public string turDescription { get; set; }
        public DateTime turInit { get; set; }
        public DateTime turEnd { get; set; }
        public bool turDelete { get; set; }

        public List<WorkShiftSegments> Lunes { get; set; }
        public List<WorkShiftSegments> Martes { get; set; }
        public List<WorkShiftSegments> Miercoles { get; set; }
        public List<WorkShiftSegments> Jueves { get; set; }
        public List<WorkShiftSegments> Viernes { get; set; }
        public List<WorkShiftSegments> Sabado { get; set; }
        public List<WorkShiftSegments> Domingo { get; set; }

        #region dbObject
        string TableName = "oWorkShift";
        string[] ColNames = new string[7] {
            "empId",
            "turId",
            "turName",
            "turDescription",
            "turInit",
            "turEnd",
            "turDelete"
        };
        #endregion

        public WorkShift() { }

        public WorkShift GetTurByEmpId(long empId)
        {
            dynamic turnosFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where empId = " + empId }), (false, "", false));
            try
            {
                WorkShift turn = new WorkShift()
                {
                    empId = Convert.ToInt64(turnosFromDB.rows[0].empId.Value.ToString()),
                    turId = Convert.ToInt64(turnosFromDB.rows[0].turId.Value.ToString()),
                    turName = turnosFromDB.rows[0].turName.Value.ToString(),
                    turDescription = turnosFromDB.rows[0].turDescription.Value.ToString(),
                    turInit = Convert.ToDateTime(turnosFromDB.rows[0].turInit.Value.ToString()),
                    turEnd = Convert.ToDateTime(turnosFromDB.rows[0].turEnd.Value.ToString()),
                    turDelete = Convert.ToBoolean(turnosFromDB.rows[0].turDelete.Value.ToString())
                };
                turn.Lunes = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Lunes);
                turn.Martes = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Martes);
                turn.Miercoles = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Miercoles);
                turn.Jueves = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Jueves);
                turn.Viernes = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Viernes);
                turn.Sabado = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Sabado);
                turn.Domingo = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Domingo);

                return turn;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public WorkShift GetTurById(long Id)
        {
            if (Id == -1)
            {
                return null;
            }

            dynamic turnosFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where turId = " + Id }), (false, "", false));
            try
            {
                WorkShift turn = new WorkShift()
                {
                    empId = Convert.ToInt64(turnosFromDB.rows[0].empId.Value.ToString()),
                    turId = Convert.ToInt64(turnosFromDB.rows[0].turId.Value.ToString()),
                    turName = turnosFromDB.rows[0].turName.Value.ToString(),
                    turDescription = turnosFromDB.rows[0].turDescription.Value.ToString(),
                    turInit = Convert.ToDateTime(turnosFromDB.rows[0].turInit.Value.ToString()),
                    turEnd = Convert.ToDateTime(turnosFromDB.rows[0].turEnd.Value.ToString()),
                    turDelete = Convert.ToBoolean(turnosFromDB.rows[0].turDelete.Value.ToString())
                };

                turn.Lunes = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Lunes);
                turn.Martes = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Martes);
                turn.Miercoles = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Miercoles);
                turn.Jueves = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Jueves);
                turn.Viernes = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Viernes);
                turn.Sabado = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Sabado);
                turn.Domingo = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Domingo);

                return turn;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public List<WorkShift> GetTurnos()
        {
            dynamic turnosFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] {}), (false, "", false));
            List<WorkShift> turnos = new List<WorkShift>();
            try
            {
                foreach (dynamic row in turnosFromDB.rows)
                {
                    WorkShift turn = new WorkShift()
                    {
                        empId = Convert.ToInt64(row.empId.Value.ToString()),
                        turId = Convert.ToInt64(row.turId.Value.ToString()),
                        turName = row.turName.Value.ToString(),
                        turDescription = row.turDescription.Value.ToString(),
                        turInit = Convert.ToDateTime(row.turInit.Value.ToString()),
                        turEnd = Convert.ToDateTime(row.turEnd.Value.ToString()),
                        turDelete = Convert.ToBoolean(row.turDelete.Value.ToString())
                    };
                    turn.Lunes = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Lunes).OrderBy(x => x.wsInit).ToList();
                    turn.Martes = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Martes).OrderBy(x => x.wsInit).ToList();
                    turn.Miercoles = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Miercoles).OrderBy(x => x.wsInit).ToList();
                    turn.Jueves = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Jueves).OrderBy(x => x.wsInit).ToList();
                    turn.Viernes = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Viernes).OrderBy(x => x.wsInit).ToList();
                    turn.Sabado = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Sabado).OrderBy(x => x.wsInit).ToList();
                    turn.Domingo = new WorkShiftSegments().GetWorkShiftSegmentsByTurId(turn.turId, Enums.eDayWeek.Domingo).OrderBy(x => x.wsInit).ToList();

                    //turn.SegmentsToView = "";
                    //foreach (WorkShiftSegments seg in turn.Segments)
                    //{
                    //    turn.SegmentsToView = turn.SegmentsToView + seg.wsName + ",";
                    //}
                    //turn.SegmentsToView = turn.SegmentsToView.TrimEnd(',');
                    turnos.Add(turn);
                }

                return turnos;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public List<WorkShift> GetTurnosReducidos()
        {
            dynamic turnosFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] { }), (false, "", false));
            List<WorkShift> turnos = new List<WorkShift>();
            try
            {
                foreach (dynamic row in turnosFromDB.rows)
                {
                    WorkShift turn = new WorkShift()
                    {
                        empId = Convert.ToInt64(row.empId.Value.ToString()),
                        turId = Convert.ToInt64(row.turId.Value.ToString()),
                        turName = row.turName.Value.ToString(),
                        turDescription = row.turDescription.Value.ToString(),
                        turInit = Convert.ToDateTime(row.turInit.Value.ToString()),
                        turEnd = Convert.ToDateTime(row.turEnd.Value.ToString()),
                        turDelete = Convert.ToBoolean(row.turDelete.Value.ToString())
                    };
                    turnos.Add(turn);
                }

                return turnos;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }
    }
}
