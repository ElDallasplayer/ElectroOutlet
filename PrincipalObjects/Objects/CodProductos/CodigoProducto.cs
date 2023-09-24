using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects.Objects
{
    public class CodigoProducto
    {
        public long Id { get; set; }
        public string CodProducto { get; set; }
        public string Descripcion { get; set; }
        public bool Eliminado { get; set; }

        #region dbObject
        string TableName = "oCodProductos";
        string[] ColNames = new string[4] {
            "codId",
            "codCodProducto",
            "codDescripcion",
            "codEliminado"
        };
        #endregion

        public CodigoProducto() { }

        public List<CodigoProducto> GetCodigoProductos()
        {
            dynamic codReturn = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] { }), (false, "", false));

            List<CodigoProducto> ret = new List<CodigoProducto>();

            foreach (dynamic row in codReturn.rows)
            {
                try
                {
                    CodigoProducto rep = new CodigoProducto()
                    {
                        Id = Convert.ToInt64(row.codId.Value.ToString()),
                        CodProducto = row.codCodProducto.Value.ToString(),
                        Descripcion = row.codDescripcion.Value.ToString(),
                        Eliminado = Convert.ToBoolean(row.codEliminado.Value.ToString()),
                    };

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

        public CodigoProducto GetCodRepById(long id)
        {
            dynamic codReturn = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (true, new string[1] { "where codId = " + id }), (false, "", false));
            CodigoProducto rep = new CodigoProducto();
            try
            {
                rep = new CodigoProducto()
                {
                    Id = Convert.ToInt64(codReturn.rows[0].codId.Value.ToString()),
                    CodProducto = codReturn.rows[0].codCodProd.Value.ToString(),
                    Descripcion = codReturn.rows[0].codDescripcion.Value.ToString(),
                    Eliminado = Convert.ToBoolean(codReturn.rows[0].codEliminado.Value.ToString()),
                };

            }
            catch (Exception ex)
            {
                Utilities.WriteLog("ERROR AL CREAR REPARACION => " + ex.Message);
                Utilities.WriteLog("JSON => " + codReturn.rows[0].ToString());
            }
            return rep;
        }
    }
}
