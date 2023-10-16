using PrincipalObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects
{
    public class Enrolador
    {
        public static bool ValidarEmpleado(int empId)
        {
            Employee emp = new Employee().GetEmployeeById_Huella(empId);

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://192.168.0.8:8082");
            request.Headers.Add("x-action", "ObtenerHuella");
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            string responseFromServer = response.Content.ReadAsStringAsync().Result;

            return true;
        }
    }
}
