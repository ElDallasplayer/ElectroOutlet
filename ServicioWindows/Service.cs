using Newtonsoft.Json;
using PrincipalObjects;
using PrincipalObjects.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ServicioWindows
{
    public class Service
    {
        public async Task<bool> StartService()
        {
            List<Device> devices = new Device().GetDevices();
            foreach (Device device in devices)
            {
                ObtenerMarcacionesAsync(device);
            }
            Utilities.WriteLog("INICIADO CORRECTAMENTE");

            return true;
        }

        public async Task<bool> ObtenerMarcacionesAsync(Device device)
        {
            while (true)
            {
                try
                {
                    Marcations marcUltima = new Marcations().GetLastMarcation();
                    string jonToSend = "{\"AcsEventCond\":{\"searchID\":\"0\",\"responseStatusStrg\":\"OK\",\"searchResultPosition\":"+ marcUltima.marcHikId + ",\"maxResults\":10,\"major\":5,\"minor\":75,\"time\":\"" + (DateTime.Now.ToString("yyyy-M-ddTHH:mm:ss") + "-03:00") + "\"}}";
                    
                    string jsonExample = "{\"AcsEvent\": {\"searchID\": \"0\",\"totalMatches\": 1132,\"responseStatusStrg\": \"OK\",\"numOfMatches\": 1,\"InfoList\": [{\"major\": 5,\"minor\": 75,\"time\": \"2023-09-23T17:03:26-03:00\",\"cardNo\": \"18446744073609551886\",\"cardType\": 1,\"name\": \"Urtado Matias\",\"cardReaderNo\": 1,\"doorNo\": 1,\"employeeNoString\": \"20\",\"type\": 0,\"serialNo\": 3506,\"userType\": \"normal\",\"currentVerifyMode\": \"cardOrFaceOrFp\",\"mask\": \"no\",\"pictureURL\": \"http://192.168.0.17/LOCALS/pic/acsLinkCap/202309_00/23_170326_30075_0.jpeg@WEB000000000833\",\"FaceRect\": {\"height\": 0.211,\"width\": 0.118,\"x\": 0.502,\"y\": 0.6}}]}}";

                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    dynamic retExample = JsonConvert.DeserializeObject(jsonExample, settings);

                    //dynamic jsonRes = CallISAPIFunction(device.devUser, device.devPassword, device.devHost, "/ISAPI/AccessControl/AcsEvent?format=json", "POST", jonToSend);
                    //long lastId = Convert.ToInt64(jsonRes.AcsEvent.totalMatches.Value.ToString());

                    long lastId = Convert.ToInt64(retExample.AcsEvent.totalMatches.Value.ToString());
                    List<Marcations> marcsToSave = new List<Marcations>();
                    try
                    {
                        foreach (dynamic marcFromDev in retExample.AcsEvent.InfoList)
                        {
                            string date = marcFromDev.time.Value.ToString().Split('-')[0];

                            Marcations marc = new Marcations();
                            marc.Deleted = false;
                            marc.marcCard = marcFromDev.cardNo.Value.ToString();
                            marc.devId = device.devId;
                            marc.marcDate = (Convert.ToDateTime(date));
                            marc.marcEdited = false;
                            marc.marcHikId = lastId + 1;
                            marc.marcEditedValue = Convert.ToDateTime(date);
                            marc.marcDirection = PrincipalObjects.Enums.mDirection.In;

                            Employee employee = new Employee().GetEmployeeByCardId(marc.marcCard);

                            if (employee != null)
                            {
                                marc.empId = employee.empId;
                            }
                            else
                            {
                                employee = new Employee();
                                employee.empId = Convert.ToInt64(marcFromDev.serialNo.Value.ToString());
                                employee.empName = marcFromDev.name.Value.ToString().Split(' ')[1];
                                employee.empSurName = marcFromDev.name.Value.ToString().Split(' ')[0];
                                employee.empCard = marc.marcCard;
                                employee.turId = -1;
                                employee.empLegajo = marcFromDev.serialNo.Value.ToString();
                                employee.empIdHikVision = Convert.ToInt64(marcFromDev.serialNo.Value.ToString());

                                var task = new Task(() => employee.SaveEmp(employee));
                                task.Start();
                                await task;

                                employee = new Employee().GetEmployeeByCardId(marc.marcCard);
                                marc.empId = employee.empId;
                            }

                            marcsToSave.Add(marc);
                            lastId++;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Utilities.WriteLog("EROR AL CREAR MARCACIONES: " + jsonRes);
                    }

                    foreach (Marcations marc in marcsToSave)
                    {
                        var task = new Task(() => marc.SaveMarcation(marc));
                        task.Start();
                    }

                    Thread.Sleep(20000);
                }
                catch(Exception ex)
                {
                    Utilities.WriteLog("EROR AL CREAR MARCACIONES: " + ex.Message);
                }

                return true;
            }
        }

        #region ISAPI
        public dynamic CallISAPIFunction(string User, string Password, string IP, string funcion, string action, string data)
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
