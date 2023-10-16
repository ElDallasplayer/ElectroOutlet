using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrincipalObjects.Enums;

namespace PrincipalObjects.Objects
{
    public class Vale
    {
        public long Id { get; set; }
        public int EmpleadoCodigo { get; set; }
        public string EmpleadoName { get; set; }
        public int Monto { get; set; }
        public string Concepto { get; set; }
        public DateTime Fecha { get; set; }
        public bool Eliminado { get; set; }
        public bool Validado { get; set; }
        public string base64Huella { get; set; }

        #region StringValues
        public string FechaAsString { get; set; }
        public string MontoAsString { get; set; }
        #endregion

        #region dbObject
        string TableName = "oVales";
        string[] ColNames = new string[8] {
            "valId",
            "valEmpleado",
            "valValorPesos",
            "valConcepto",
            "valFecha",
            "valEliminado",
            "valValidado",
            "valHuellaValidadora"
        };
        #endregion

        public Vale() { }

        public List<Vale> GetVales()
        {
            List<Vale> vales = new List<Vale>();

            dynamic valeReturn = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] { }), (false, "", false));

            foreach (dynamic row in valeReturn.rows)
            {
                Vale val = new Vale();
                val.Id = row["valId"];
                val.EmpleadoCodigo = row["valEmpleado"];

                try { val.EmpleadoName = new Employee().GetEmployeeById(val.EmpleadoCodigo).NombreCompleto; } catch (Exception ex) { val.EmpleadoName = "Sin Asignar"; }

                val.Monto = row["valValorPesos"];
                val.Concepto = row["valConcepto"].ToString();
                val.Fecha = Convert.ToDateTime(row["valFecha"].ToString());
                val.Eliminado = Convert.ToBoolean(row["valEliminado"].ToString());
                val.Validado = Convert.ToBoolean(row["valValidado"].ToString());
                val.base64Huella = row["valHuellaValidadora"].ToString();

                val.FechaAsString = val.Fecha.ToString("dd/MM/yyyy");
                val.MontoAsString = "$" + val.Monto.ToString();

                vales.Add(val);
            }

            return vales;
        }

        public Vale GetValeById(long valId)
        {
            dynamic valeReturn = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where valId = " + valId }), (false, "", false));

            Vale val = new Vale();
            val.Id = valeReturn.rows[0]["valId"];
            val.EmpleadoCodigo = valeReturn.rows[0]["valEmpleado"];

            try { val.EmpleadoName = new Employee().GetEmployeeById(val.EmpleadoCodigo).NombreCompleto; } catch (Exception ex) { val.EmpleadoName = "Sin Asignar"; }

            val.Monto = valeReturn.rows[0]["valValorPesos"];
            val.Concepto = valeReturn.rows[0]["valConcepto"].ToString();
            val.Fecha = Convert.ToDateTime(valeReturn.rows[0]["valFecha"].ToString());
            val.Eliminado = Convert.ToBoolean(valeReturn.rows[0]["valEliminado"].ToString());
            val.Validado = Convert.ToBoolean(valeReturn.rows[0]["valValidado"].ToString());
            val.base64Huella = valeReturn.rows[0]["valHuellaValidadora"].ToString();

            val.FechaAsString = val.Fecha.ToString("dd/MM/yyyy");
            val.MontoAsString = "$" + val.Monto.ToString();


            return val;
        }

        public Vale EditarVale(Vale vale)
        {
            List<(string, string, eDataType)> dataToSend = new List<(string, string, eDataType)>();

            dataToSend.Add(("valId", vale.Id.ToString(), eDataType.number));
            dataToSend.Add(("valEmpleado", vale.EmpleadoCodigo.ToString(), eDataType.number));
            dataToSend.Add(("valValorPesos", vale.Monto.ToString(), eDataType.number));
            dataToSend.Add(("valConcepto", vale.Concepto.ToString(), eDataType.number));
            dataToSend.Add(("valFecha", vale.Fecha.ToString("yyyyMMdd HH:mm:ss"), eDataType.text));
            dataToSend.Add(("valEliminado", "0", eDataType.number));
            dataToSend.Add(("valValidado", (vale.Validado?"1":"0"), eDataType.number));
            dataToSend.Add(("valHuellaValidadora", (vale.base64Huella == null?"": vale.base64Huella), eDataType.text));

            bool rest = SQLInteract.UpdateDataInDataBase(TableName, dataToSend, (true, new string[1] { "valId = " + vale.Id }));

            if (rest)
            {
                return vale;
            }
            else
            {
                return null;
            }
        }

        public Vale AgregarVale(Vale vale)
        {
            List<(string, eDataType)> dataToSend = new List<(string, eDataType)>();
            long LastId = SQLInteract.GetLastIdFromInsertedElement(TableName, "valId") + 1;

            dataToSend.Add((LastId.ToString(), eDataType.number));
            dataToSend.Add((vale.EmpleadoCodigo.ToString(), eDataType.number));
            dataToSend.Add((vale.Monto.ToString(), eDataType.number));
            dataToSend.Add((vale.Concepto.ToString(), eDataType.text));
            dataToSend.Add((vale.Fecha.ToString("yyyyMMdd HH:mm:ss"), eDataType.text));
            dataToSend.Add(("0", eDataType.number));
            dataToSend.Add(((vale.Validado ? "1" : "0"), eDataType.number));
            dataToSend.Add(((vale.base64Huella == null ? "" : vale.base64Huella), eDataType.text));


            bool rest = SQLInteract.InsertDataInDatabase(TableName, ColNames, dataToSend);

            if (rest)
            {
                return vale;
            }
            else
            {
                return null;
            }
        }

        public bool EliminarVale(long id)
        {
            bool isDelete = SQLInteract.DeleteDataInDatabase(TableName, (true, new string[1] { "valId = " + id }));
            return isDelete;
        }
    }
}
