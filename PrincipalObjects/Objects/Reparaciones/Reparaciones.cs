using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PrincipalObjects.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace PrincipalObjects.Objects
{
    public class Reparacion
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public long CodProducto { get; set; }
        public reState Estado { get; set; }
        public string Repuesto { get; set; }
        public string ReparacionRealizada { get; set; }
        public string Trazabilidad { get; set; }
        public bool Eliminado { get; set; }
        public int Empleado { get; set; }
        public int Usuario { get; set; }

        public string NombreEmpelado { get; set; }

        public string FechaAsString { get; set; }
        public string EstadoAsString { get; set; }
        public string CodigoProductoAsString { get; set; }

        #region dbObject
        string TableName = "oReparaciones";
        string[] ColNames = new string[10] {
            "reId",
            "reFecha",
            "reCodProd",
            "reEstado",
            "reRepuesto",
            "reTrazabilidad",
            "reEliminado",
            "reEmpleado",
            "reUsuario",
            "reReparacion"
        };
        #endregion

        public Reparacion() { }

        public List<Reparacion> GetReparaciones()
        {
            List<Employee> employees = new Employee().GetEmployees();
            List<CodigoProducto> codigosProd = new CodigoProducto().GetCodigoProductos();

            dynamic repReturn = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] { }), (false, "", false));

            List<Reparacion> ret = new List<Reparacion>();

            foreach (dynamic row in repReturn.rows)
            {
                try
                {
                    Reparacion rep = new Reparacion()
                    {
                        Id = Convert.ToInt64(row.reId.Value.ToString()),
                        Fecha = Convert.ToDateTime(row.reFecha.Value.ToString()),
                        CodProducto = Convert.ToInt64(row.reCodProd.Value.ToString()),
                        Estado = (reState)Convert.ToInt32(row.reEstado.Value.ToString()),
                        Repuesto = row.reRepuesto.Value.ToString(),
                        Trazabilidad = row.reTrazabilidad.Value.ToString(),
                        Eliminado = Convert.ToBoolean(row.reEliminado.Value.ToString()),
                        Empleado = Convert.ToInt32(row.reEmpleado.Value.ToString()) ?? -1,
                        Usuario = Convert.ToInt32(row.reUsuario.Value.ToString()) ?? -1,
                        ReparacionRealizada = row.reReparacion.Value?.ToString() ?? "",
                    };
                    try
                    {
                        rep.NombreEmpelado = employees.Where(x => x.empId == rep.Empleado).FirstOrDefault().NombreCompleto;
                    }
                    catch (Exception ex)
                    {
                        rep.NombreEmpelado = "Sin asignar";
                    }

                    rep.FechaAsString = rep.Fecha.ToString("dd/MM/yyyy");
                    rep.EstadoAsString = rep.Estado.ToString();
                    if (codigosProd.Any(x => x.Id == rep.CodProducto))
                    {
                        rep.CodigoProductoAsString = codigosProd.Where(x => x.Id == rep.CodProducto).FirstOrDefault().CodProducto;
                    }
                    else
                    {
                        rep.CodigoProductoAsString = "-";
                    }
                    ret.Add(rep);
                }
                catch (Exception ex)
                {
                    Utilities.WriteLog("ERROR AL CREAR REPARACION => " + ex.Message);
                    Utilities.WriteLog("JSON => " + row.ToString());
                }
            }
            return ret;
        }

        //Recibe un valor maximo de busqueda, y su distribucion de paginado
        public List<Reparacion> GetReparacionesMaxValue(int valorMaximo, int cantidadBloquesAEliminar)
        {
            int offsetValue = valorMaximo * cantidadBloquesAEliminar;
            List<Employee> employees = new Employee().GetEmployees();
            List<CodigoProducto> codigosProd = new CodigoProducto().GetCodigoProductos();

            string query = $"SELECT {String.Join(",",ColNames)} FROM {TableName} ORDER BY reId desc OFFSET {offsetValue} ROWS FETCH NEXT {valorMaximo} ROWS ONLY";

            dynamic repReturn = SQLInteract.GetDataFromDataBase_Special(query, ColNames);

            List<Reparacion> ret = new List<Reparacion>();

            foreach (dynamic row in repReturn.rows)
            {
                try
                {
                    Reparacion rep = new Reparacion()
                    {
                        Id = Convert.ToInt64(row.reId.Value.ToString()),
                        Fecha = Convert.ToDateTime(row.reFecha.Value.ToString()),
                        CodProducto = Convert.ToInt64(row.reCodProd.Value.ToString()),
                        Estado = (reState)Convert.ToInt32(row.reEstado.Value.ToString()),
                        Repuesto = row.reRepuesto.Value.ToString(),
                        Trazabilidad = row.reTrazabilidad.Value.ToString(),
                        Eliminado = Convert.ToBoolean(row.reEliminado.Value.ToString()),
                        Empleado = Convert.ToInt32(row.reEmpleado.Value.ToString()) ?? -1,
                        Usuario = Convert.ToInt32(row.reUsuario.Value.ToString()) ?? -1,
                        ReparacionRealizada = row.reReparacion.Value?.ToString() ?? "",
                    };
                    try
                    {
                        rep.NombreEmpelado = employees.Where(x => x.empId == rep.Empleado).FirstOrDefault().NombreCompleto;
                    }
                    catch (Exception ex)
                    {
                        rep.NombreEmpelado = "Sin asignar";
                    }

                    rep.FechaAsString = rep.Fecha.ToString("dd/MM/yyyy");
                    rep.EstadoAsString = rep.Estado.ToString();
                    if (codigosProd.Any(x => x.Id == rep.CodProducto))
                    {
                        rep.CodigoProductoAsString = codigosProd.Where(x => x.Id == rep.CodProducto).FirstOrDefault().CodProducto;
                    }
                    else
                    {
                        rep.CodigoProductoAsString = "-";
                    }
                    ret.Add(rep);
                }
                catch (Exception ex)
                {
                    Utilities.WriteLog("ERROR AL CREAR REPARACION => " + ex.Message);
                    Utilities.WriteLog("JSON => " + row.ToString());
                }
            }
            return ret;
        }

        public Reparacion GetReparacionById(long id)
        {
            dynamic repReturn = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where reId = " + id }), (false, "", false));
            Reparacion rep = new Reparacion();
            try
            {
                rep = new Reparacion()
                {
                    Id = Convert.ToInt64(repReturn.rows[0].reId.Value.ToString()),
                    Fecha = Convert.ToDateTime(repReturn.rows[0].reFecha.Value.ToString()),
                    CodProducto = Convert.ToInt64(repReturn.rows[0].reCodProd.Value.ToString()),
                    Estado = (reState)Convert.ToInt32(repReturn.rows[0].reEstado.Value.ToString()),
                    Repuesto = repReturn.rows[0].reRepuesto.Value.ToString(),
                    Trazabilidad = repReturn.rows[0].reTrazabilidad.Value.ToString(),
                    Eliminado = Convert.ToBoolean(repReturn.rows[0].reEliminado.Value.ToString()),
                    Empleado = Convert.ToInt32(repReturn.rows[0].reEmpleado.Value.ToString()) ?? -1,
                    Usuario = Convert.ToInt32(repReturn.rows[0].reUsuario.Value.ToString()) ?? -1,
                    ReparacionRealizada = repReturn.rows[0].reReparacion.Value.ToString() ?? "",
                };
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("ERROR AL CREAR REPARACION => " + ex.Message);
                Utilities.WriteLog("JSON => " + repReturn.rows[0].ToString());
            }
            return rep;
        }

        public List<Reparacion> GetReparacionByFechas(DateTime desde, DateTime hasta)
        {
            List<Employee> employees = new Employee().GetEmployees();
            List<CodigoProducto> codigosProd = new CodigoProducto().GetCodigoProductos();

            dynamic repReturn = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[2] { "where reFecha > '" + desde.ToString("yyyyMMdd 00:00:00") + "'", " reFecha < '" + hasta.AddDays(1).ToString("yyyyMMdd 00:00:00") + "'"}), (true, "reFecha", false));
            //Reparacion rep = new Reparacion();
            try
            {
                List<Reparacion> ret = new List<Reparacion>();

                foreach (dynamic row in repReturn.rows)
                {
                    try
                    {
                        Reparacion rep = new Reparacion()
                        {
                            Id = Convert.ToInt64(row.reId.Value.ToString()),
                            Fecha = Convert.ToDateTime(row.reFecha.Value.ToString()),
                            CodProducto = Convert.ToInt64(row.reCodProd.Value.ToString()),
                            Estado = (reState)Convert.ToInt32(row.reEstado.Value.ToString()),
                            Repuesto = row.reRepuesto.Value.ToString(),
                            Trazabilidad = row.reTrazabilidad.Value.ToString(),
                            Eliminado = Convert.ToBoolean(row.reEliminado.Value.ToString()),
                            Empleado = Convert.ToInt32(row.reEmpleado.Value.ToString()) ?? -1,
                            Usuario = Convert.ToInt32(row.reUsuario.Value.ToString()) ?? -1,
                            ReparacionRealizada = row.reReparacion.Value?.ToString() ?? "",
                        };
                        try
                        {
                            rep.NombreEmpelado = employees.Where(x => x.empId == rep.Empleado).FirstOrDefault().NombreCompleto;
                        }
                        catch (Exception ex)
                        {
                            rep.NombreEmpelado = "Sin asignar";
                        }

                        rep.FechaAsString = rep.Fecha.ToString("dd/MM/yyyy");
                        rep.EstadoAsString = rep.Estado.ToString();
                        if (codigosProd.Any(x => x.Id == rep.CodProducto))
                        {
                            rep.CodigoProductoAsString = codigosProd.Where(x => x.Id == rep.CodProducto).FirstOrDefault().CodProducto;
                        }
                        else
                        {
                            rep.CodigoProductoAsString = "-";
                        }
                        ret.Add(rep);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("ERROR AL CREAR REPARACION => " + ex.Message);
                        Utilities.WriteLog("JSON => " + row.ToString());
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("ERROR AL CREAR REPARACION => " + ex.Message);
                Utilities.WriteLog("JSON => " + repReturn.rows[0].ToString());
                return new List<Reparacion>();
            }
        }

        public Reparacion EditarReparacion(Reparacion reparacion)
        {
            List<(string, string, eDataType)> dataToSend = new List<(string, string, eDataType)>();

            dataToSend.Add(("reId", reparacion.Id.ToString(), eDataType.number));
            dataToSend.Add(("reFecha", reparacion.Fecha.ToString("yyyyMMdd HH:mm:ss"), eDataType.text));
            dataToSend.Add(("reCodProd", reparacion.CodProducto.ToString(), eDataType.number));
            dataToSend.Add(("reEstado", ((int)reparacion.Estado).ToString(), eDataType.number));
            dataToSend.Add(("reRepuesto", reparacion.Repuesto.ToString(), eDataType.text));
            dataToSend.Add(("reTrazabilidad", reparacion.Trazabilidad.ToString(), eDataType.text));
            dataToSend.Add(("reEliminado", "0", eDataType.number));
            dataToSend.Add(("reEmpleado", reparacion.Empleado.ToString(), eDataType.number));
            dataToSend.Add(("reUsuario", reparacion.Usuario.ToString(), eDataType.number));
            dataToSend.Add(("reReparacion", reparacion.ReparacionRealizada?.ToString() ?? "", eDataType.text));

            bool rest = SQLInteract.UpdateDataInDataBase(TableName, dataToSend, (true, new string[1] { "reId = " + reparacion.Id }));

            if (rest)
            {
                return reparacion;
            }
            else
            {
                return null;
            }
        }

        public Reparacion SaveReparacion(Reparacion reparacion)
        {
            List<(string, eDataType)> dataToSend = new List<(string, eDataType)>();
            long LastId = SQLInteract.GetLastIdFromInsertedElement(TableName, "reId") + 1;

            dataToSend.Add((LastId.ToString(), eDataType.number));
            dataToSend.Add((reparacion.Fecha.ToString("yyyyMMdd HH:mm:ss"), eDataType.text));
            dataToSend.Add((reparacion.CodProducto.ToString(), eDataType.number));
            dataToSend.Add((((int)reparacion.Estado).ToString(), eDataType.number));
            dataToSend.Add(((reparacion.Repuesto != null ? reparacion.Repuesto.ToString() : "Sin dato"), eDataType.text));
            dataToSend.Add(((reparacion.Trazabilidad != null ? reparacion.Trazabilidad.ToString() : "Sin dato"), eDataType.text));
            dataToSend.Add(("0", eDataType.number));
            dataToSend.Add((reparacion.Empleado.ToString(), eDataType.number));
            dataToSend.Add((reparacion.Usuario.ToString(), eDataType.number));
            dataToSend.Add(((reparacion.ReparacionRealizada != null ? reparacion.ReparacionRealizada.ToString() : "Sin dato"), eDataType.text));

            bool rest = SQLInteract.InsertDataInDatabase(TableName, ColNames, dataToSend);

            if (rest)
            {
                return reparacion;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteReparacion(int id)
        {
            bool isDelete = SQLInteract.DeleteDataInDatabase(TableName, (true, new string[1] { "reId = " + id }));
            return isDelete;
        }
    }
}
