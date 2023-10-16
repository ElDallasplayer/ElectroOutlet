using PrincipalObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects
{
    public class Contabilium
    {
        //DEVUELV EN STRING EL VALIDATION BEARER QUE SE USA PARA HACER PETICIONES
        public static string GetAuthentication(string client_id, string client_secret)
        {
            //client_id = ahumadamonica@hotmail.com
            //client_secret = aa2cf6a8c098461090f92bf9cc96c284
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://rest.contabilium.com/token");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            collection.Add(new KeyValuePair<string, string>("client_id", client_id));
            collection.Add(new KeyValuePair<string, string>("client_secret", client_secret));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            
            string ResponseFromContabilium = response.Content.ReadAsStringAsync().Result;

            dynamic resJson = Utilities.ConvertToDynamic(ResponseFromContabilium);
            string devolver = resJson["access_token"];

            return devolver;
        }

        //DEVUELVE SI UN CLIENTE EXISTE UY SI EXISTE, ENTONCES DEVUELVE EL CLIENT ID
        public static KeyValuePair<bool,long> ClienteExiste(string documento, string bearerToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://rest.contabilium.com//api/clientes/search?pageSize=0&filtro=" + documento);
            request.Headers.Add("Authorization", "Bearer " + bearerToken);
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            string ResponseFromContabilium = response.Content.ReadAsStringAsync().Result;
            dynamic resJson = Utilities.ConvertToDynamic(ResponseFromContabilium);
            int devolver = resJson["TotalItems"];

            if (devolver > 0)
            {
                return new KeyValuePair<bool, long>(true, (long)resJson["Items"][0]["Id"]);
            }
            else
            {
                return new KeyValuePair<bool, long>(false, -1);
            }
        }

        //DEVUELVE LA CANTIDAD DE COMPRAS ENCONTRADAS
        public static int ValidarComprasEntreFechas(DateTime desde, DateTime hasta, string bearerToken)
        {
            List<Employee> empleados = new Employee().GetEmployees();

            foreach (Employee empToSearch in empleados.OrderBy(x => x.NombreCompleto))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://rest.contabilium.com/api/comprobantes/search?fechaDesde=2023-06-01&fechaHasta=2023-10-14&filtro=" + empToSearch.empDocumento);
                request.Headers.Add("Authorization", "Bearer " + bearerToken);
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();

                string ResponseFromContabilium = response.Content.ReadAsStringAsync().Result;
                dynamic resJson = Utilities.ConvertToDynamic(ResponseFromContabilium);

                //AQUI VOY A OBTENER CADA ARTICULO DE LA VENTA
                //CUANDO LOS TENGA, TENGO QUE CREARSELOS AL EMPLEADO EN UNA LISTA 
                //LA LISTA LA VOY A PONER EN LA VISTA DEL EMPLEADO 
                //CREAR NUEVA TABLA PARA ESOS ELEMENTOS, CON LOS DATOS PRINCIPALES NECESARIOS
            }
            
            return 1;
        }
    }
}
