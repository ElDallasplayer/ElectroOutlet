using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects
{
    public class Enums
    {
        public enum eDataType
        {
            number = 0,
            text = 1
        }
        
        public enum mDirection
        {
            In = 0,
            Out = 1
        }

        public enum pType
        {
            Create = 0,
            Read = 1,
            Update = 2,
            Delete = 4
        }

        public enum nType
        {
            Aviso = 0,
            Alerta = 1,
            Error = 2,
            Exitoso = 4
        }

        //REPORTES
        public enum Reports
        {
            ReporteDeMarcaciones = 0,
            ReporteDeHorasPorDia = 1,
            ReporteDeHorasPorPeriodo = 2,
            ReporteDeRegistros = 3,
            ReporteDeRegistrosReducido = 4
        }

        public enum eDayWeek
        {
            Lunes = 0,
            Martes = 1,
            Miercoles = 2,
            Jueves = 3,
            Viernes = 4,
            Sabado = 5,
            Domingo = 6
        }

        public enum reState
        {
            Reparando = 0,
            Reparado = 1
        }

        public enum eDedo
        {
            SinDefinir = 0,
            PulgarIzquierdo = 1,
            IndiceIzquierdo = 2,
            MedioIzquierdo = 3,
            AnularIzquierdo = 4,
            MeñiqueIzquierdo = 5,
            PulgarDerecho = 6,
            IndiceDerecho = 7,
            MedioDerecho = 8,
            AnularDerecho = 9,
            MeñiqueDerecho = 10
        }
    }
}
