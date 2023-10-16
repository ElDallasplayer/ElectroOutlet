using Newtonsoft.Json;
using PrincipalObjects;
using PrincipalObjects.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ServicioWindows
{
    public class Service
    {
        public static async Task<bool> StartService()
        {
            List<Device> devices = new Device().GetDevices();
            foreach (Device device in devices)
            {
                ObtenerMarcacionesAsync(device);
            }
            Utilities.WriteLog("INICIADO CORRECTAMENTE");

            return true;
        }

        public static dynamic ObtenerEjemplo()
        {
            string jsonExample = "{\"AcsEvent\": {\"searchID\": \"0\",\"totalMatches\": 1132,\"responseStatusStrg\": \"OK\",\"numOfMatches\": 2,\"InfoList\": [{\"major\": 5,\"minor\": 75,\"time\": \"2023-09-23T17:55:26-03:00\",\"cardNo\": \"18446744073609551886\",\"cardType\": 1,\"name\": \"Urtado Matias\",\"cardReaderNo\": 1,\"doorNo\": 1,\"employeeNoString\": \"20\",\"type\": 0,\"serialNo\": 3506,\"userType\": \"normal\",\"currentVerifyMode\": \"cardOrFaceOrFp\",\"mask\": \"no\",\"pictureURL\": \"http://192.168.0.17/LOCALS/pic/acsLinkCap/202309_00/23_170326_30075_0.jpeg@WEB000000000833\",\"FaceRect\": {\"height\": 0.211,\"width\": 0.118,\"x\": 0.502,\"y\": 0.6}},{\"major\": 5,\"minor\": 75,\"time\": \"2023-09-23T17:03:26-03:00\",\"cardNo\": \"18446744073609551886\",\"cardType\": 1,\"name\": \"Urtado Matias\",\"cardReaderNo\": 1,\"doorNo\": 1,\"employeeNoString\": \"20\",\"type\": 0,\"serialNo\": 3506,\"userType\": \"normal\",\"currentVerifyMode\": \"cardOrFaceOrFp\",\"mask\": \"no\",\"pictureURL\": \"http://192.168.0.17/LOCALS/pic/acsLinkCap/202309_00/23_170326_30075_0.jpeg@WEB000000000833\",\"FaceRect\": {\"height\": 0.211,\"width\": 0.118,\"x\": 0.502,\"y\": 0.6}}]}}";

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            dynamic retExample = JsonConvert.DeserializeObject(jsonExample, settings);

            return retExample;
        }

        public static async Task<bool> ObtenerMarcacionesAsync(Device device)
        {
            while (true)
            {
                try
                {
                    Marcations marcUltima = new Marcations().GetLastMarcation();
                    string jonToSend = "{\"AcsEventCond\":{\"searchID\":\"0\",\"responseStatusStrg\":\"OK\",\"searchResultPosition\":" + marcUltima.marcHikId + ",\"maxResults\":10,\"major\":5,\"minor\":75,\"time\":\"" + (DateTime.Now.ToString("yyyy-M-ddTHH:mm:ss") + "-03:00") + "\"}}";


                    dynamic jsonRes = CallISAPIFunction(device.devUser, device.devPassword, device.devHost, "AccessControl/AcsEvent?format=json", "POST", jonToSend).Result;
                    //dynamic jsonRes = ObtenerEjemplo();

                    long totalMatches = Convert.ToInt64(jsonRes.AcsEvent.totalMatches.Value.ToString());
                    long matchNum = Convert.ToInt64(jsonRes.AcsEvent.numOfMatches.Value.ToString());

                    try
                    {
                        if (marcUltima.marcHikId == totalMatches)
                        {

                        }
                        else
                        {
                            if (jsonRes.AcsEvent.InfoList.Count == 0)
                            {
                                if (marcUltima.marcHikId == totalMatches)
                                {

                                }
                                else
                                {
                                    jonToSend = "{\"AcsEventCond\":{\"searchID\":\"0\",\"responseStatusStrg\":\"OK\",\"searchResultPosition\":0,\"maxResults\":10,\"major\":5,\"minor\":75,\"time\":\"" + (DateTime.Now.ToString("yyyy-M-ddTHH:mm:ss") + "-03:00") + "\"}}";
                                    jsonRes = CallISAPIFunction(device.devUser, device.devPassword, device.devHost, "AccessControl/AcsEvent?format=json", "POST", jonToSend).Result;
                                }
                            }
                            else
                            {

                            }

                            long idBase = marcUltima.marcHikId;

                            for (int i = 0; i < matchNum; i++)
                            {
                                dynamic element = jsonRes.AcsEvent.InfoList[i];
                                string date = element.time.Value.ToString().Split('-')[0];
                                Utilities.WriteLog("FECHA = " + date);

                                bool sinTarjeta = false;

                                Marcations marc = new Marcations();
                                marc.Deleted = false;
                                try
                                {
                                    marc.marcCard = element.cardNo.Value.ToString();
                                }catch (Exception ex)
                                {
                                    sinTarjeta = true;
                                    marc.marcCard = element.employeeNoString.Value.ToString();
                                }
                                marc.devId = device.devId;
                                marc.marcDate = (Convert.ToDateTime(date));
                                marc.marcEdited = false;
                                marc.marcHikId = idBase + 1 + i;
                                marc.marcEditedValue = Convert.ToDateTime(date);
                                marc.marcDirection = PrincipalObjects.Enums.mDirection.In;

                                Employee employee = new Employee();

                                if (sinTarjeta)
                                {
                                    employee = new Employee().GetEmployeeByCardId(marc.marcCard);
                                    if (employee == null)
                                    {
                                        employee = new Employee().GetEmployeeByCardId(element.serialNo.Value.ToString());
                                    }
                                }
                                else
                                {
                                    employee = new Employee().GetEmployeeByCardId(marc.marcCard);
                                }

                                try
                                {
                                    if (employee != null)
                                    {
                                        marc.empId = employee.empId;
                                    }
                                    else
                                    {
                                        employee = new Employee();
                                        employee.empId = Convert.ToInt64(element.serialNo.Value.ToString());
                                        try
                                        {
                                            employee.empName = element.name.Value.ToString().Split(' ')[1];
                                        }
                                        catch (Exception ex)
                                        {
                                            employee.empName = "Sin nombre";
                                        }
                                        try
                                        {
                                            employee.empSurName = element.name.Value.ToString().Split(' ')[0];
                                        }
                                        catch (Exception ex)
                                        {
                                            employee.empSurName = "Sin apellido";
                                        }

                                        employee.empCard = marc.marcCard;
                                        employee.turId = -1;
                                        employee.empLegajo = element.serialNo.Value.ToString();
                                        employee.empIdHikVision = Convert.ToInt64(element.serialNo.Value.ToString());

                                        employee.SaveEmp(employee);
                                        employee = new Employee().GetEmployeeByCardId(marc.marcCard);
                                        marc.empId = employee.empId;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Utilities.WriteLog("ERROR AL CREAR EMPLEADO:" + ex.Message);
                                }

                                try
                                {
                                    marc.SaveMarcation(marc);
                                }
                                catch (Exception ex)
                                {
                                    Utilities.WriteLog(ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("EROR AL GESTIONAR MARCACION: " + jsonRes + " --- " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteLog("EROR AL CREAR MARCACIONES: " + ex.Message);
                }
                Thread.Sleep(30000);
            }

            return true;
        }

        #region ISAPI
        public static async Task<dynamic> CallISAPIFunction(string User, string Password, string IP, string funcion, string action, string data)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create("http://" + IP + "/ISAPI/" + funcion);
            wr.Method = action;
            wr.ContentType = "application/json";
            wr.Accept = "*/*";
            wr.ReadWriteTimeout = 100000;
            wr.KeepAlive = true;
            wr.Credentials = new NetworkCredential(User, Password);

            if (!string.IsNullOrEmpty(data))
            {
                var _data = Encoding.ASCII.GetBytes(data);
                using (var stream = wr.GetRequestStream())
                {
                    stream.Write(_data, 0, _data.Length);
                }
            }

            try
            {
                var httpResponse = (HttpWebResponse)wr.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    dynamic ret = JsonConvert.DeserializeObject(result, settings);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public dynamic CallISAPIFunction(string User, string Password, string IP, string funcion, string action, string data, string fileName)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create("http://" + IP + "/ISAPI/" + funcion);
            wr.Method = action;
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Accept = "*/*";
            wr.ReadWriteTimeout = 100000;
            wr.KeepAlive = true;
            wr.Credentials = new NetworkCredential(User, Password);

            var _data = Encoding.ASCII.GetBytes(data);
            using (var stream = wr.GetRequestStream())
            {
                #region json
                string formdataTemplateJson = "Content-Disposition: form-data; name=\"{0}\";\r\nContent-Type: aplication/json\r\n\r\n";
                stream.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem_json = string.Format(formdataTemplateJson, "FaceDataRecord");
                byte[] formbytes_json = Encoding.UTF8.GetBytes(formitem_json);
                stream.Write(formbytes_json, 0, formbytes_json.Length);
                stream.Write(_data, 0, _data.Length);
                byte[] trailer_json = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                stream.Write(trailer_json, 0, trailer_json.Length);
                #endregion

                #region imagen
                string formdataTemplateImage = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\";\r\nContent-Type: image/jpeg\r\n\r\n";
                stream.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplateImage, "img", "C:\\temp\\img.jpeg");
                byte[] formbytes = Encoding.UTF8.GetBytes(formitem);
                stream.Write(formbytes, 0, formbytes.Length);

                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    stream.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();
                #endregion

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                stream.Write(trailer, 0, trailer.Length);
            }

            try
            {
                var httpResponse = (HttpWebResponse)wr.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    dynamic ret = JsonConvert.DeserializeObject(result, settings);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
