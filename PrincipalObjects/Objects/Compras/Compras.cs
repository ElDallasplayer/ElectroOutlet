using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrincipalObjects.Enums;

namespace PrincipalObjects.Objects
{
    public class Compra
    {
        public long comId { get; set; }
        public DateTime comFechaEmision { get; set; }
        public long comEmpleado { get; set; }
        public int comTotalNeto { get; set; } //=> DESCUENTO FINAL
        public int comTotalNeto_Decimal { get; set; }
        public int comTotalBruto { get; set; }
        public int comTotalBruto_Decimal { get; set; }
        public long comIdCliente { get; set; }
        public long comIdCompra { get; set; }

        public string NombreEmpleado { get; set; }

        #region dbObject
        string TableName = "oCompras";
        string[] ColNames = new string[9] {
            "comId",
            "comFechaEmision",
            "comEmpleado",
            "comTotalNeto",
            "comTotalNeto_Decimal",
            "comTotalBruto",
            "comTotalBruto_Decimal",
            "comIdCliente",
            "comIdCompra"
        };
        #endregion

        public Compra() { }

        public List<Compra> GetCompras(bool filtraPorFecha,DateTime desde, DateTime hasta)
        {
            dynamic comprasFromDB = null;

            if (filtraPorFecha)
            {
                comprasFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[2] { "where comFechaEmision >= " + desde.ToString("yyyyMMdd 00:00:00"), " and comFechaEmision <= " + hasta.ToString("yyyyMMdd 00:00:00") }), (false, "", false));
            }
            else
            {
                comprasFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] { }), (false, "", false));
            }

            List<Compra> compras = new List<Compra>();
            try
            {
                foreach (dynamic row in comprasFromDB.rows)
                {
                    try
                    {
                        Compra compra = new Compra()
                        {
                            comId = Convert.ToInt64(row.comId.Value),
                            comFechaEmision = Convert.ToDateTime(row.comFechaEmision.Value.ToString()),
                            comEmpleado = Convert.ToInt64(row.comEmpleado.Value.ToString()),
                            comTotalNeto = Convert.ToInt32(row.comTotalNeto.Value.ToString()),
                            comTotalNeto_Decimal = Convert.ToInt32(row.comTotalNeto_Decimal.Value.ToString()),
                            comTotalBruto = Convert.ToInt32(row.comTotalBruto.Value.ToString()),
                            comTotalBruto_Decimal = Convert.ToInt32(row.comTotalBruto_Decimal.Value.ToString()),
                            comIdCliente = Convert.ToInt64(row.comIdCliente.Value.ToString()),
                            comIdCompra = Convert.ToInt64(row.comIdCompra.Value.ToString())
                        };
                        Employee emp = new Employee().GetEmployeeById(compra.comEmpleado);
                        compra.NombreEmpleado = emp.NombreCompleto;

                        compras.Add(compra);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("ERROR AL CREAR COMPRA: " + ex.Message);
                        Utilities.WriteLog(" => " + row.ToString());
                    }
                }

                return compras;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public bool ExisteEnLaBase(long compraId)
        {
            dynamic comprasFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where comIdCompra = " + compraId }), (false, "", false));

            if (comprasFromDB.rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Compra CrearCompra(Compra compraAGuardar)
        {
            if (!ExisteEnLaBase(compraAGuardar.comIdCompra))
            {
                List<(string, eDataType)> dataToSend = new List<(string, eDataType)>();
                long LastId = SQLInteract.GetLastIdFromInsertedElement(TableName, "comId") + 1;

                dataToSend.Add((LastId.ToString(), eDataType.number));
                dataToSend.Add((compraAGuardar.comFechaEmision.ToString("yyyyMMdd 00:00:00"), eDataType.text));
                dataToSend.Add((compraAGuardar.comEmpleado.ToString(), eDataType.number));
                dataToSend.Add((compraAGuardar.comTotalNeto.ToString(), eDataType.number));
                dataToSend.Add((compraAGuardar.comTotalNeto_Decimal.ToString(), eDataType.number));
                dataToSend.Add((compraAGuardar.comTotalBruto.ToString(), eDataType.number));
                dataToSend.Add((compraAGuardar.comTotalBruto_Decimal.ToString(), eDataType.number));
                dataToSend.Add((compraAGuardar.comIdCliente.ToString(), eDataType.number));
                dataToSend.Add((compraAGuardar.comIdCompra.ToString(), eDataType.number));
                
                bool rest = SQLInteract.InsertDataInDatabase(TableName, ColNames, dataToSend);

                if (rest)
                {
                    return compraAGuardar;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public List<Compra> GetComprasByEmpleadoId(long empId)
        {
            dynamic comprasFromDB = null;

            comprasFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where comEmpleado = " + empId}), (false, "", false));
            
            List<Compra> compras = new List<Compra>();
            try
            {
                foreach (dynamic row in comprasFromDB.rows)
                {
                    try
                    {
                        Compra compra = new Compra()
                        {
                            comId = Convert.ToInt64(row.comId.Value),
                            comFechaEmision = Convert.ToDateTime(row.comFechaEmision.Value.ToString()),
                            comEmpleado = Convert.ToInt64(row.comEmpleado.Value.ToString()),
                            comTotalNeto = Convert.ToInt32(row.comTotalNeto.Value.ToString()),
                            comTotalNeto_Decimal = Convert.ToInt32(row.comTotalNeto_Decimal.Value.ToString()),
                            comTotalBruto = Convert.ToInt32(row.comTotalBruto.Value.ToString()),
                            comTotalBruto_Decimal = Convert.ToInt32(row.comTotalBruto_Decimal.Value.ToString()),
                            comIdCliente = Convert.ToInt64(row.comIdCliente.Value.ToString()),
                            comIdCompra = Convert.ToInt64(row.comIdCompra.Value.ToString())
                        };

                        compras.Add(compra);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("ERROR AL CREAR COMPRA: " + ex.Message);
                        Utilities.WriteLog(" => " + row.ToString());
                    }
                }

                return compras;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }
    }
}
