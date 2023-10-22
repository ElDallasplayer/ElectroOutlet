using Azure.Core;
using PrincipalObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects
{
    public class Enrolador
    {
        public static string ObtenerHuellaEnrolador(int empId)
        {
            Employee emp = new Employee().GetEmployeeById_Huella(empId);

            string localIP = "";

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());// objeto para guardar la ip
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    if (localIP == "")
                    {
                        localIP = ip.ToString();// esta es nuestra ip
                    }
                }
            }

            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "http://" + localIP + ":8082");
                request.Headers.Add("x-action", "ObtenerHuella");
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();

                string responseFromServer = response.Content.ReadAsStringAsync().Result;

                return responseFromServer;
            }catch (Exception ex)
            {
                return "{\"Response\":\"ERROR\",\"Calidad\":\"\",\"DeviceStatus\":\"Error\",\"Template_Biostar\":\"\"}";
            }
        }

        public static string ValidarHuella(string templateEmp)
        {
            string localIP = "";

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());// objeto para guardar la ip
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    if (localIP == "")
                    {
                        localIP = ip.ToString();// esta es nuestra ip
                    }
                }
            }

            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "http://" + localIP + ":8082");
                request.Headers.Add("x-action", "ValidarHuella");
                var content = new StringContent(templateEmp, null, "text/plain");
                request.Content = content;

                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();

                string responseFromServer = response.Content.ReadAsStringAsync().Result;

                return responseFromServer;
            }
            catch (Exception ex)
            {
                return "{\"Response\":\"ERROR\",\"Calidad\":\"\",\"DeviceStatus\":\"Error\",\"Template_Biostar\":\"\"}";
            }
        
            
        }
    }
}
