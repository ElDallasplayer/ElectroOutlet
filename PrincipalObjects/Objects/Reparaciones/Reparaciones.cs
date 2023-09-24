using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrincipalObjects.Enums;

namespace PrincipalObjects.Objects
{
    public class Reparacion
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public long CodProducto { get; set; }
        public reState Estado { get; set; }
        public string Repuesto { get; set; }
        public string Trazabilidad { get; set; }
        public bool Eliminado { get; set; }
        public int Empleado { get; set; }
        public int Usuario { get; set; }

        public string FechaAsString { get; set; }
        public string EstadoAsString { get; set; }
        public string CodigoProductoAsString { get; set; }

        #region dbObject
        string TableName = "oReparaciones";
        string[] ColNames = new string[9] {
            "reId",
            "reFecha",
            "reCodProd",
            "reEstado",
            "reRepuesto",
            "reTrazabilidad",
            "reEliminado",
            "reEmpleado",
            "reUsuario",
        };
        #endregion

        public Reparacion() { }

        public List<Reparacion> GetReparaciones()
        {
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
                        Usuario = Convert.ToInt32(row.reUsuario.Value.ToString()) ?? -1
                    };

                    rep.FechaAsString = rep.Fecha.ToString("dd/MM/yyyy");
                    rep.EstadoAsString = rep.Estado.ToString();
                    rep.CodigoProductoAsString = codigosProd.Where(x => x.Id == rep.CodProducto).FirstOrDefault().CodProducto??"Sin codigo";

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
                    Empleado = Convert.ToInt32(repReturn.rows[0].reEmpelado.Value.ToString()) ?? -1,
                    Usuario = Convert.ToInt32(repReturn.rows[0].reUsuario.Value.ToString()) ?? -1
                };
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("ERROR AL CREAR REPARACION => " + ex.Message);
                Utilities.WriteLog("JSON => " + repReturn.rows[0].ToString());
            }
            return rep;
        }
    }
}
