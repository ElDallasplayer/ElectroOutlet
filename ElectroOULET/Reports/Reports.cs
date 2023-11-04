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
using SpreadsheetLight;
using System.Reflection;

namespace ElectroOULET
{
    public class Reports
    {
        public class ListaDatos
        {
            public string? NUM1 { get; set; }
            public string? NUM2 { get; set; }
            public string? NUM3 { get; set; }
        }

        public class BloqueDatos
        {
            public string? BLOK1 { get; set; }
            public string? BLOK2 { get; set; }
            public string? BLOK3 { get; set; }
        }

        public class ClasesDatos
        {
        }

        #region MAKE REPORT
        public static void HacerReporte(List<dynamic> Datos, string Template, bool HavePicture/*, PictureToReport PictureConfig*/)
        {
            var datos = Datos.FirstOrDefault();

            string path = Directory.GetCurrentDirectory() + @"\Templates\" + Template + ".xlsx";
            string ToSave = Directory.GetCurrentDirectory() + @"\ReportesGenerados\" + Template +"_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

            var dataRead = "";

            SLDocument documento = new SLDocument();

            SLDocument sl = new SLDocument(path);

            int Row = 0;
            while (dataRead != "%end_repeat%")
            {
                for (var Column = 1; Column < 50; Column++)
                {
                    dataRead = sl.GetCellValueAsString(Row, Column);

                    if (dataRead != "")
                        if (dataRead != "%end_repeat%")
                            if (dataRead.Contains('%'))
                            {
                                if (!dataRead.Contains("%end_"))
                                {
                                    if (dataRead.Contains("%begin_list_"))
                                    {
                                        var dataReplaced = dataRead.Replace("begin_list_", "");
                                        var PropValue = (GetPorpValue(datos, dataReplaced.Replace("%", "")));
                                        int iteration = 0;
                                        
                                        int[] array = new int[(PropValue[0].GetType().DeclaredProperties).Length];
                                        var dataFromCell = string.Empty;

                                        for (var i = 0; i < array.Length; i++)
                                        {
                                            var Coli = 0;
                                            //ESTO DA MIEDO, PERO ESTA BIEN
                                            //while (true)
                                            //{
                                                Coli = Column + (i+1);
                                                dataFromCell = sl.GetCellValueAsString(Row, Coli);
                                                if (dataFromCell.Contains('%'))
                                                {
                                                    if (!dataFromCell.Contains("%end_list"))
                                                    {
                                                        var dataWhitNoPercentage = dataFromCell.Replace("%", "");
                                                        var nameFromPropertie = PropValue[0].GetType().DeclaredProperties[i].Name;

                                                        if (dataWhitNoPercentage == nameFromPropertie)
                                                        {
                                                            array[i] = Coli;
                                                        }
                                                    }
                                                }
                                            //}
                                        }

                                        foreach (var Prop in PropValue)
                                        {
                                            var properties = Prop.GetType().DeclaredProperties;
                                            int iter = properties.Length;
                                            for (var col = 0; col < iter; col++)
                                            {
                                                documento.SetCellValue(Row + iteration, array[col], (GetPorpValue(PropValue[iteration], properties[col]?.Name)));
                                            }
                                            iteration++;
                                        }

                                        Console.WriteLine("");
                                    }
                                    else if (dataRead.Contains("%begin_block_"))
                                    {

                                    }
                                    else
                                    {
                                        //documento.SetCellValue(Row, Column, (GetPorpValue(datos, dataRead.Replace("%", ""))).ToString());
                                    }
                                }
                                else
                                {
                                    break; 
                                }
                            }
                            else
                                documento.SetCellValue(Row, Column, dataRead);
                        else
                            break;
                    //else
                    //documento.SetCellValue(Row, Column, "");
                }
                Row++;
            }
            Row = 1;

            documento.SaveAs(ToSave);
        }

        public static Object GetPorpValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        #endregion

        #region REPORTE MARCACIONES
        public class classReporteDeMarcaciones
        {
            public string Name = "ListadoDeMarcaciones";
            public List<dynamic>? Listado { get; set; } //CON ESTE NOMBRE DEBO HACER EL TEMPLATE
        }
        public class classReporteDeMarcaciones_row
        {
            public string FECHA { get; set; }
            public string EMPLEADO { get; set; }
            public string TARJETA { get; set; }
            public string SENTIDO { get; set; }
        }
        public static async Task<DataTable> ReporteDeMarcaciones(string employeesId, DateTime desde, DateTime hasta)
        {
            classReporteDeMarcaciones marcsList = new classReporteDeMarcaciones();
            marcsList.Listado = new List<dynamic>();

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

                List<Marcations> marcs = new Marcations().GetMarcsByEmpId(emp.empId, desde, hasta);

                foreach (Marcations mc in marcs.OrderBy(x => x.marcDate))
                {
                    marcsList.Listado.Add(new classReporteDeMarcaciones_row() { EMPLEADO = emp.NombreCompleto, FECHA = mc.marcDate.ToString("dd-MM-yyyy HH:mm"), SENTIDO = "Entrada", TARJETA = mc.marcCard });

                    dataTable.Rows.Add("", mc.marcCard, (mc.marcDirection == PrincipalObjects.Enums.mDirection.In ? "Entrada" : "Salida"), mc.marcDate.ToString("dd/MM/yyyy"), mc.marcDate.ToString("HH:mm:ss"));
                }
            }

            List<dynamic> lista = new List<dynamic>();
            lista.Add(marcsList);

            HacerReporte(lista, "ListadoDeMarcaciones",false);

            return dataTable;
        }
        #endregion

        public static async Task<List<DataTable>> ReporteDeRegistros(string employeesId, DateTime desde, DateTime hasta)
        {
            List<Reparacion> reparacionesParaReporte = new Reparacion().GetReparaciones();
            List<DataTable> tablas = new List<DataTable>();

            List<Employee> employees = new Employee().GetEmployeesToReport(employeesId);
            foreach (Employee employee in employees)
            {
                DataTable dataTable = new DataTable("ReporteDeMarcaciones");
                dataTable.Columns.AddRange(new DataColumn[]
                {
                new DataColumn(employee.NombreCompleto),
                new DataColumn(""),
                new DataColumn(""),
                new DataColumn(""),
                new DataColumn(""),
                new DataColumn("")
                });

                for (DateTime d = desde; d <= hasta; d = d.AddDays(1))
                {
                    List<Reparacion> reparacionesDelDia = reparacionesParaReporte.Where(x => x.Fecha.Date == d.Date).Where(x => x.Empleado == employee.empId).ToList();
                    dataTable.Rows.Add("FECHA", "COD. PRODUCTO", "REPARACION", "REPUESTO", "TRAZABILIDAD", "EMPLEADO");

                    if (reparacionesDelDia.Count > 0)
                    {
                        int unico = 0;
                        foreach (Reparacion repa in reparacionesDelDia.OrderBy(x => x.Fecha))
                        {
                            if (unico == 0)
                            {
                                dataTable.Rows.Add(d.ToString("dd") + "-" + d.ToString("MMMM"), repa.CodigoProductoAsString, repa.ReparacionRealizada, repa.Repuesto, repa.Trazabilidad, repa.NombreEmpelado);
                                unico++;
                            }
                            else
                            {
                                dataTable.Rows.Add("", repa.CodigoProductoAsString, repa.ReparacionRealizada, repa.Repuesto, repa.Trazabilidad, repa.NombreEmpelado);
                            }
                        }
                        dataTable.Rows.Add("", "", "", "", "", "");
                    }
                    else
                    {
                        dataTable.Rows.Add(d.ToString("dd") + "-" + d.ToString("MMMM"), "", "", "", "", "");
                    }
                }
                tablas.Add(dataTable);
            }
            
            return tablas;
        }

        public static async Task<DataTable> ReporteDeHorasPorPeriodo(string employeesId, DateTime desde, DateTime hasta)
        {
            List<Employee> empleadosToReport = new Employee().GetEmployeesToReport(employeesId);

            DataTable dataTable = new DataTable("ReporteDeMarcaciones");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("NombreEmpleado"),
                new DataColumn("Tarjeta"),
                new DataColumn("Totales"),
                new DataColumn("Tardes"),
                new DataColumn("Anticipadas"),
                new DataColumn("Normales"),
                new DataColumn("Descaso"),
                new DataColumn("Exceso descanso"),
                new DataColumn("Extras"),
                new DataColumn("SalidaAnticipada"),
                new DataColumn("Desde-Hasta")
            });

            foreach (Employee emp in empleadosToReport.OrderBy(x => x.empName))
            {
                List<Marcations> marcs = new Marcations().GetMarcsByEmpId(emp.empId, desde, hasta);
                WorkShift turno = emp.Turno;

                TimeSpan descansoMaximo = new TimeSpan(0, 30, 0);

                TimeSpan Totales = new TimeSpan(0, 0, 0);
                TimeSpan Tardes = new TimeSpan(0, 0, 0);
                TimeSpan Anticipadas = new TimeSpan(0, 0, 0);
                TimeSpan Normales = new TimeSpan(0, 0, 0);
                TimeSpan Descanso = new TimeSpan(0, 0, 0);
                TimeSpan ExcesoDesc = new TimeSpan(0, 0, 0);
                TimeSpan Extras = new TimeSpan(0, 0, 0);
                TimeSpan SalidaAnticipada = new TimeSpan(0, 0, 0);

                bool turnoAsignado = true;

                for (DateTime d = desde; d <= hasta; d = d.AddDays(1))
                {
                    TimeSpan Totales_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Tardes_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Anticipadas_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Normales_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Descanso_dia = new TimeSpan(0, 0, 0);
                    TimeSpan ExcesoDesc_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Extras_dia = new TimeSpan(0, 0, 0);
                    TimeSpan SalidaAnticipada_dia = new TimeSpan(0, 0, 0);

                    List<Marcations> marcsHoy = marcs.Where(x => x.marcDate.Date == d.Date).ToList();
                    //ELIMINO DUPLICADAS
                    marcsHoy = removeDuplicates(marcsHoy);


                    List<WorkShiftSegments> segmentosHoy = new List<WorkShiftSegments>();

                    bool esDomingo = false;

                    switch (d.DayOfWeek)
                    {
                        case DayOfWeek.Monday: segmentosHoy = turno?.Lunes; esDomingo = false; break;
                        case DayOfWeek.Tuesday: segmentosHoy = turno?.Martes; esDomingo = false; break;
                        case DayOfWeek.Wednesday: segmentosHoy = turno?.Miercoles; esDomingo = false; break;
                        case DayOfWeek.Thursday: segmentosHoy = turno?.Jueves; esDomingo = false; break;
                        case DayOfWeek.Friday: segmentosHoy = turno?.Viernes; esDomingo = false; break;
                        case DayOfWeek.Saturday: segmentosHoy = turno?.Sabado; esDomingo = false; break;
                        case DayOfWeek.Sunday: segmentosHoy = turno?.Domingo; esDomingo = true; break;
                    }

                    int SegmentoDeDia = 1;

                    if (marcsHoy.Count == 0)
                    {
                        try
                        {

                            if (!esDomingo)
                            {
                                switch (d.DayOfWeek)
                                {
                                    case DayOfWeek.Monday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Lunes.First().wsEnd.TimeOfDay - turno?.Lunes.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Tuesday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Martes.First().wsEnd.TimeOfDay - turno?.Martes.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Wednesday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Miercoles.First().wsEnd.TimeOfDay - turno?.Miercoles.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Thursday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Jueves.First().wsEnd.TimeOfDay - turno?.Jueves.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Friday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Viernes.First().wsEnd.TimeOfDay - turno?.Viernes.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Saturday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Sabado.First().wsEnd.TimeOfDay - turno?.Sabado.First().wsInit.TimeOfDay); break;
                                }
                            }
                            Tardes = Tardes + Tardes_dia;
                        }
                        catch (Exception EX)
                        {
                            turnoAsignado = false;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < marcsHoy.Count; i++)
                        {
                            try
                            {
                                Marcations marcPrimera = new Marcations();
                                Marcations marcSegunda = new Marcations();
                                try
                                {
                                    marcPrimera = marcsHoy[i];
                                    marcSegunda = marcsHoy[i + 1];
                                }
                                catch (Exception ex)
                                {
                                    marcPrimera = marcsHoy[i];
                                    marcSegunda = marcsHoy[i];
                                }

                                TimeSpan horaInicio = new TimeSpan(marcPrimera.marcDate.Hour, marcPrimera.marcDate.Minute, marcPrimera.marcDate.Second);
                                TimeSpan horaFin = new TimeSpan(marcSegunda.marcDate.Hour, marcSegunda.marcDate.Minute, marcSegunda.marcDate.Second);

                                if (segmentosHoy != null)
                                {
                                    //foreach (WorkShiftSegments segmento in segmentosHoy.OrderBy(x => x.wsInit))
                                    //{

                                    //SOLO DEBE EXISTIR UN SEGMENTO QUE DEFINE EL INICIO Y FIN DE JORNADA
                                    TimeSpan horaInicioS = new TimeSpan(segmentosHoy[0].wsInit.Hour, segmentosHoy[0].wsInit.Minute, segmentosHoy[0].wsInit.Second);
                                    TimeSpan horaFinS = new TimeSpan(segmentosHoy[0].wsEnd.Hour, segmentosHoy[0].wsEnd.Minute, segmentosHoy[0].wsEnd.Second);

                                    if (SegmentoDeDia == 1)
                                    {
                                        if (horaInicio < horaInicioS)
                                        {
                                            Anticipadas_dia = Anticipadas_dia + (horaInicioS - horaInicio);
                                        }
                                        else if (horaInicio > horaInicioS)
                                        {
                                            Tardes_dia = Tardes_dia + (horaInicio - horaInicioS);
                                        }

                                        Normales_dia = Normales_dia + (horaFin - horaInicio) - Anticipadas_dia;
                                        Totales_dia = Totales_dia + (horaFin - horaInicio);
                                        SegmentoDeDia = 2;
                                    }
                                    else if (SegmentoDeDia == 2)
                                    {
                                        if ((horaFin - horaInicio) > descansoMaximo)
                                        {
                                            Descanso_dia = Descanso_dia + descansoMaximo;
                                            ExcesoDesc_dia = ExcesoDesc_dia + (horaFin - (horaFin - descansoMaximo));
                                            SegmentoDeDia = 3;
                                        }
                                        else
                                        {
                                            Descanso_dia = Descanso_dia + (horaFin - horaInicio);
                                            SegmentoDeDia = 3;
                                        }
                                    }
                                    else if (SegmentoDeDia == 3)
                                    {
                                        if (horaFin > horaFinS)
                                        {
                                            if (Extras_dia + (horaFin - horaFinS) < new TimeSpan(1, 0, 0))
                                            {
                                                Normales = Normales + (horaFin - horaFinS);
                                            }
                                            else
                                            {
                                                Extras_dia = Extras_dia + (horaFin - horaFinS);
                                            }
                                        }
                                        else
                                        {
                                            SalidaAnticipada_dia = SalidaAnticipada_dia + (horaFinS - horaFin);
                                        }

                                        Normales_dia = Normales_dia + ((horaFin - horaInicio) - SalidaAnticipada_dia - ExcesoDesc_dia);
                                        Totales_dia = Totales_dia + (horaFin - horaInicio);
                                    }
                                    //}
                                }
                            }
                            catch (Exception ex)
                            {
                                dataTable.Rows.Add(emp.NombreCompleto, "INCONGRUENCIA EN MARCACIONES EN DIA " + d.ToString("dd/MM/yyyy"));
                                //d = d.AddDays(1);
                            }
                        }
                        Totales = Totales + Totales_dia;
                        Normales = Normales + Normales_dia;
                        Tardes = Tardes + Tardes_dia;
                        Anticipadas = Anticipadas + Anticipadas_dia;
                        Descanso = Descanso + Descanso_dia;
                        ExcesoDesc = ExcesoDesc + ExcesoDesc_dia;
                        SalidaAnticipada = SalidaAnticipada + SalidaAnticipada_dia;
                        Extras = Extras + Extras_dia;
                    }
                }

                dataTable.Rows.Add(emp.NombreCompleto, emp.empCard,
                    (Totales == new TimeSpan(0, 0, 0) ? "--" : Totales),
                    (Tardes == new TimeSpan(0, 0, 0) ? "--" : Tardes),
                    (Anticipadas == new TimeSpan(0, 0, 0) ? "--" : Anticipadas),
                    (Normales == new TimeSpan(0, 0, 0) ? "--" : Normales),
                    (Descanso == new TimeSpan(0, 0, 0) ? "--" : Descanso),
                    (ExcesoDesc == new TimeSpan(0, 0, 0) ? "--" : ExcesoDesc),
                    (Extras == new TimeSpan(0, 0, 0) ? "--" : Extras),
                    (SalidaAnticipada == new TimeSpan(0, 0, 0) ? "--" : SalidaAnticipada), (desde.ToString("dd-MM-yyyy") + " " + hasta.ToString("dd-MM-yyyy") + (turnoAsignado == false ? " - SIN TURNO ASIGNADO" : "")));
            }
            return dataTable;
        }

        public static async Task<DataTable> ReporteDeHorasPorPeriodoPorDia(string employeesId, DateTime desde, DateTime hasta)
        {
            List<Employee> empleadosToReport = new Employee().GetEmployeesToReport(employeesId);

            DataTable dataTable = new DataTable("ReporteDeMarcaciones");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("NombreEmpleado"),
                new DataColumn("Tarjeta"),
                new DataColumn("Dia"),
                new DataColumn("Desde1"),
                new DataColumn("Hasta1"),
                new DataColumn("InicioPausa"),
                new DataColumn("FinPausa"),
                new DataColumn("Desde2"),
                new DataColumn("Hasta2"),
                new DataColumn("Totales"),
                new DataColumn("Tardes"),
                new DataColumn("Anticipadas"),
                new DataColumn("Normales"),
                new DataColumn("Descaso"),
                new DataColumn("Exceso descanso"),
                new DataColumn("Extras"),
                new DataColumn("SalidaAnticipada"),
                new DataColumn("Desde-Hasta")
            });

            foreach (Employee emp in empleadosToReport.OrderBy(x => x.empName))
            {
                List<Marcations> marcs = new Marcations().GetMarcsByEmpId(emp.empId, desde, hasta);
                WorkShift turno = emp.Turno;



                TimeSpan descansoMaximo = new TimeSpan(0, 30, 0);

                TimeSpan Totales = new TimeSpan(0, 0, 0);
                TimeSpan Tardes = new TimeSpan(0, 0, 0);
                TimeSpan Anticipadas = new TimeSpan(0, 0, 0);
                TimeSpan Normales = new TimeSpan(0, 0, 0);
                TimeSpan Descanso = new TimeSpan(0, 0, 0);
                TimeSpan ExcesoDesc = new TimeSpan(0, 0, 0);
                TimeSpan Extras = new TimeSpan(0, 0, 0);
                TimeSpan SalidaAnticipada = new TimeSpan(0, 0, 0);

                for (DateTime d = desde; d <= hasta; d = d.AddDays(1))
                {
                    TimeSpan e1 = new TimeSpan(0, 0, 0);
                    TimeSpan s1 = new TimeSpan(0, 0, 0);
                    TimeSpan e2 = new TimeSpan(0, 0, 0);
                    TimeSpan s2 = new TimeSpan(0, 0, 0);
                    TimeSpan e3 = new TimeSpan(0, 0, 0);
                    TimeSpan s3 = new TimeSpan(0, 0, 0);

                    TimeSpan Totales_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Tardes_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Anticipadas_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Normales_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Descanso_dia = new TimeSpan(0, 0, 0);
                    TimeSpan ExcesoDesc_dia = new TimeSpan(0, 0, 0);
                    TimeSpan Extras_dia = new TimeSpan(0, 0, 0);
                    TimeSpan SalidaAnticipada_dia = new TimeSpan(0, 0, 0);

                    List<Marcations> marcsHoy = marcs.Where(x => x.marcDate.Date == d.Date).ToList();
                    //ELIMINO DUPLICADAS
                    marcsHoy = removeDuplicates(marcsHoy);


                    List<WorkShiftSegments> segmentosHoy = new List<WorkShiftSegments>();

                    bool esDomingo = false;

                    switch (d.DayOfWeek)
                    {
                        case DayOfWeek.Monday: segmentosHoy = turno?.Lunes; esDomingo = false; break;
                        case DayOfWeek.Tuesday: segmentosHoy = turno?.Martes; esDomingo = false; break;
                        case DayOfWeek.Wednesday: segmentosHoy = turno?.Miercoles; esDomingo = false; break;
                        case DayOfWeek.Thursday: segmentosHoy = turno?.Jueves; esDomingo = false; break;
                        case DayOfWeek.Friday: segmentosHoy = turno?.Viernes; esDomingo = false; break;
                        case DayOfWeek.Saturday: segmentosHoy = turno?.Sabado; esDomingo = false; break;
                        case DayOfWeek.Sunday: segmentosHoy = turno?.Domingo; esDomingo = true; break;
                    }

                    int SegmentoDeDia = 1;

                    if (marcsHoy.Count == 0)
                    {
                        try
                        {
                            if (!esDomingo)
                            {
                                switch (d.DayOfWeek)
                                {
                                    case DayOfWeek.Monday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Lunes.First().wsEnd.TimeOfDay - turno?.Lunes.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Tuesday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Martes.First().wsEnd.TimeOfDay - turno?.Martes.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Wednesday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Miercoles.First().wsEnd.TimeOfDay - turno?.Miercoles.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Thursday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Jueves.First().wsEnd.TimeOfDay - turno?.Jueves.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Friday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Viernes.First().wsEnd.TimeOfDay - turno?.Viernes.First().wsInit.TimeOfDay); break;
                                    case DayOfWeek.Saturday: Tardes_dia = Tardes_dia + (TimeSpan)(turno?.Sabado.First().wsEnd.TimeOfDay - turno?.Sabado.First().wsInit.TimeOfDay); break;
                                }
                            }
                            Tardes = Tardes + Tardes_dia;
                            dataTable.Rows.Add(emp.NombreCompleto, emp.empCard, d.ToString("dd/MM/yyyy"), e1, s1, e2, s2, e3, s3,
                            (Totales_dia == new TimeSpan(0, 0, 0) ? "--" : Totales_dia),
                            (Tardes_dia == new TimeSpan(0, 0, 0) ? "--" : Tardes_dia),
                            (Anticipadas_dia == new TimeSpan(0, 0, 0) ? "--" : Anticipadas_dia),
                            (Normales_dia == new TimeSpan(0, 0, 0) ? "--" : Normales_dia),
                            (Descanso_dia == new TimeSpan(0, 0, 0) ? "--" : Descanso_dia),
                            (ExcesoDesc_dia == new TimeSpan(0, 0, 0) ? "--" : ExcesoDesc_dia),
                            (Extras_dia == new TimeSpan(0, 0, 0) ? "--" : Extras_dia),
                            (SalidaAnticipada_dia == new TimeSpan(0, 0, 0) ? "--" : SalidaAnticipada_dia), "");
                        }
                        catch (Exception ex)
                        {
                            dataTable.Rows.Add(emp.NombreCompleto, emp.empCard, d.ToString("dd/MM/yyyy"), e1, s1, e2, s2, e3, s3,
                            (Totales_dia == new TimeSpan(0, 0, 0) ? "--" : Totales_dia),
                            (Tardes_dia == new TimeSpan(0, 0, 0) ? "--" : Tardes_dia),
                            (Anticipadas_dia == new TimeSpan(0, 0, 0) ? "--" : Anticipadas_dia),
                            (Normales_dia == new TimeSpan(0, 0, 0) ? "--" : Normales_dia),
                            (Descanso_dia == new TimeSpan(0, 0, 0) ? "--" : Descanso_dia),
                            (ExcesoDesc_dia == new TimeSpan(0, 0, 0) ? "--" : ExcesoDesc_dia),
                            (Extras_dia == new TimeSpan(0, 0, 0) ? "--" : Extras_dia),
                            (SalidaAnticipada_dia == new TimeSpan(0, 0, 0) ? "--" : SalidaAnticipada_dia), "EL EMPLEADO NO CUENTA CON UN TURNO");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < marcsHoy.Count; i++)
                        {
                            try
                            {
                                Marcations marcPrimera = new Marcations();
                                Marcations marcSegunda = new Marcations();
                                try
                                {
                                    marcPrimera = marcsHoy[i];
                                    marcSegunda = marcsHoy[i + 1];
                                }
                                catch (Exception ex)
                                {
                                    marcPrimera = marcsHoy[i];
                                    marcSegunda = marcsHoy[i];
                                }

                                TimeSpan horaInicio = new TimeSpan(marcPrimera.marcDate.Hour, marcPrimera.marcDate.Minute, marcPrimera.marcDate.Second);
                                TimeSpan horaFin = new TimeSpan(marcSegunda.marcDate.Hour, marcSegunda.marcDate.Minute, marcSegunda.marcDate.Second);

                                if (segmentosHoy != null)
                                {
                                    //foreach (WorkShiftSegments segmento in segmentosHoy.OrderBy(x => x.wsInit))
                                    //{

                                    //SOLO DEBE EXISTIR UN SEGMENTO QUE DEFINE EL INICIO Y FIN DE JORNADA
                                    TimeSpan horaInicioS = new TimeSpan(segmentosHoy[0].wsInit.Hour, segmentosHoy[0].wsInit.Minute, segmentosHoy[0].wsInit.Second);
                                    TimeSpan horaFinS = new TimeSpan(segmentosHoy[0].wsEnd.Hour, segmentosHoy[0].wsEnd.Minute, segmentosHoy[0].wsEnd.Second);

                                    if (SegmentoDeDia == 1)
                                    {
                                        if (horaInicio < horaInicioS)
                                        {
                                            Anticipadas_dia = Anticipadas_dia + (horaInicioS - horaInicio);
                                        }
                                        else if (horaInicio > horaInicioS)
                                        {
                                            Tardes_dia = Tardes_dia + (horaInicio - horaInicioS);
                                        }

                                        Normales_dia = Normales_dia + (horaFin - horaInicio) - Anticipadas_dia;
                                        Totales_dia = Totales_dia + (horaFin - horaInicio);
                                        SegmentoDeDia = 2;

                                        e1 = horaInicio;
                                        s1 = horaFin;
                                    }
                                    else if (SegmentoDeDia == 2)
                                    {
                                        if ((horaFin - horaInicio) > descansoMaximo)
                                        {
                                            Descanso_dia = Descanso_dia + descansoMaximo;
                                            ExcesoDesc_dia = ExcesoDesc_dia + (horaFin - (horaFin - descansoMaximo));
                                            SegmentoDeDia = 3;
                                        }
                                        else
                                        {
                                            Descanso_dia = Descanso_dia + (horaFin - horaInicio);
                                            SegmentoDeDia = 3;
                                        }

                                        e2 = horaInicio;
                                        s2 = horaFin;
                                    }
                                    else if (SegmentoDeDia == 3)
                                    {
                                        if (horaFin > horaFinS)
                                        {
                                            if (Extras_dia + (horaFin - horaFinS) < new TimeSpan(1, 0, 0))
                                            {
                                                Normales = Normales + (horaFin - horaFinS);
                                            }
                                            else
                                            {
                                                Extras_dia = Extras_dia + (horaFin - horaFinS);
                                            }
                                        }
                                        else
                                        {
                                            SalidaAnticipada_dia = SalidaAnticipada_dia + (horaFinS - horaFin);
                                        }

                                        Normales_dia = Normales_dia + ((horaFin - horaInicio) - SalidaAnticipada_dia - ExcesoDesc_dia);
                                        Totales_dia = Totales_dia + (horaFin - horaInicio);

                                        e3 = horaInicio;
                                        s3 = horaFin;
                                    }
                                    //}
                                }
                            }
                            catch (Exception ex)
                            {
                                dataTable.Rows.Add(emp.NombreCompleto, "INCONGRUENCIA EN MARCACIONES EN DIA " + d.ToString("dd/MM/yyyy"));
                                //d = d.AddDays(1);
                            }
                        }
                        try
                        {
                            Totales = Totales + Totales_dia;
                            Normales = Normales + Normales_dia;
                            Tardes = Tardes + Tardes_dia;
                            Anticipadas = Anticipadas + Anticipadas_dia;
                            Descanso = Descanso + Descanso_dia;
                            ExcesoDesc = ExcesoDesc + ExcesoDesc_dia;
                            SalidaAnticipada = SalidaAnticipada + SalidaAnticipada_dia;
                            Extras = Extras + Extras_dia;
                            dataTable.Rows.Add(emp.NombreCompleto, emp.empCard, d.ToString("dd/MM/yyyy"), e1, s1, e2, s2, e3, s3,
                            (Totales_dia == new TimeSpan(0, 0, 0) ? "--" : Totales_dia),
                            (Tardes_dia == new TimeSpan(0, 0, 0) ? "--" : Tardes_dia),
                            (Anticipadas_dia == new TimeSpan(0, 0, 0) ? "--" : Anticipadas_dia),
                            (Normales_dia == new TimeSpan(0, 0, 0) ? "--" : Normales_dia),
                            (Descanso_dia == new TimeSpan(0, 0, 0) ? "--" : Descanso_dia),
                            (ExcesoDesc_dia == new TimeSpan(0, 0, 0) ? "--" : ExcesoDesc_dia),
                            (Extras_dia == new TimeSpan(0, 0, 0) ? "--" : Extras_dia),
                            (SalidaAnticipada_dia == new TimeSpan(0, 0, 0) ? "--" : SalidaAnticipada_dia), "");
                        }
                        catch (Exception ex)
                        {
                            dataTable.Rows.Add(emp.NombreCompleto, "INCONGRUENCIA EN MARCACIONES EN DIA " + d.ToString("dd/MM/yyyy"));
                            //d = d.AddDays(1);
                        }
                    }
                }

                dataTable.Rows.Add("", "", "", new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0),
                    (Totales == new TimeSpan(0, 0, 0) ? "--" : Totales),
                    (Tardes == new TimeSpan(0, 0, 0) ? "--" : Tardes),
                    (Anticipadas == new TimeSpan(0, 0, 0) ? "--" : Anticipadas),
                    (Normales == new TimeSpan(0, 0, 0) ? "--" : Normales),
                    (Descanso == new TimeSpan(0, 0, 0) ? "--" : Descanso),
                    (ExcesoDesc == new TimeSpan(0, 0, 0) ? "--" : ExcesoDesc),
                    (Extras == new TimeSpan(0, 0, 0) ? "--" : Extras),
                    (SalidaAnticipada == new TimeSpan(0, 0, 0) ? "--" : SalidaAnticipada), (desde.ToString("dd-MM-yyyy") + " " + hasta.ToString("dd-MM-yyyy")));
            }
            return dataTable;
        }
        
        public static async Task<DataTable> ReporteDeRegistrosReducido(string employeesId, DateTime desde, DateTime hasta)
        {
            List<Employee> empleados = new Employee().GetEmployees();
            List<Reparacion> reparacionesParaReporte = new Reparacion().GetReparacionByFechas(desde, hasta);

            DataTable dataTable = new DataTable("ReporteDeMarcaciones");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("TECNICOS APELLIDO Y NOMBRE"),
                new DataColumn("REPARACIONES DE: " + desde.ToString("dd/MM/yyyy") + " - " + hasta.ToString("dd/MM/yyyy")),
                new DataColumn("ACLARACION")
            });
            
            foreach (Employee emp in empleados.OrderBy(x => x.NombreCompleto))
            {
                List<Reparacion> repDelEmpleado = reparacionesParaReporte.Where(x => x.Empleado != null).Where(y => y.Empleado == emp.empId).ToList();

                int DiasParaReporte = 0;
                for (DateTime d = desde; d <= hasta; d = d.AddDays(1))
                {
                    if (repDelEmpleado.Any(x => x.Fecha.Date == d.Date))
                    {
                        DiasParaReporte++;
                    }
                }
                dataTable.Rows.Add(emp.NombreCompleto,repDelEmpleado.Count,DiasParaReporte.ToString() + " DIAS");
            }

            return dataTable;
        }

        public static List<Marcations> removeDuplicates(List<Marcations> listOriginal)
        {
            List<Marcations> marcationsDuplicate = listOriginal;
            List<Marcations> toReturn = new List<Marcations>();

            foreach (Marcations marc in marcationsDuplicate)
            {
                if (toReturn.Any(x => x.marcDate.Day == marc.marcDate.Day && x.marcDate.Year == marc.marcDate.Year && x.marcDate.Month == marc.marcDate.Month && x.marcDate.Hour == marc.marcDate.Hour && x.marcDate.Minute == marc.marcDate.Minute))
                {

                }
                else
                {
                    toReturn.Add(marc);
                }
            }

            return toReturn;
        }
    }
}
