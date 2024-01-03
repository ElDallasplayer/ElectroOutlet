using Newtonsoft.Json;
using System;
using System.IO;

namespace PrincipalObjects
{
    public class Utilities
    {
        static string ProgramDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        static string AplicationPath = AppDomain.CurrentDomain.BaseDirectory;

        public static dynamic GetDataFromConfig()
        {
            string filePath = "";
            dynamic dataToReturn = null;
            if (WriteConfigDataIfDontExits(out filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                        dynamic toResponse = JsonConvert.DeserializeObject(line, settings);
                        dataToReturn = toResponse;
                    }
                }

                return dataToReturn;
            }
            else
            {
                return null;
            }
        }

        public static bool WriteConfigDataIfDontExits(out string filePath)
        {
            StreamWriter stream = null;

            string folderName = @"\Aplication\Configuration\";
            string fileName = @"Configuration.json";
            filePath = ProgramDataPath + folderName + fileName;
            string fileContent = "{\"instance\":\"\",\"dataBase\":\"\",\"user\":\"\",\"password\":\"\"}";

            try
            {
                if (!DirectoryExists(ProgramDataPath + folderName))
                {
                    return false;
                }
                
                if (!File.Exists(filePath) || File.ReadAllLines(filePath).Length == 0)
                {
                    File.Create(filePath).Dispose();

                    stream = File.AppendText(filePath);
                    stream.WriteLine(fileContent);
                    stream.Close();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DirectoryExists(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool WriteLog(string logText)
        {
            StreamWriter stream = null;
            string folderName = @"\Aplication\Logs\";
            string fileName = @"ApplicattionLog";
            try
            {
                string sLogFile = ProgramDataPath + folderName + fileName + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                if (DirectoryExists(ProgramDataPath + folderName))
                {
                    File.Create(sLogFile).Dispose();
                }

                stream = File.AppendText(sLogFile);
                stream.WriteLine(string.Format("{0} - {1}.", DateTime.Now, logText));
                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el LOG");
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return true;
        }

        public static dynamic ConvertToDynamic(string stringAsJson)
        {
            try
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                dynamic toResponse = JsonConvert.DeserializeObject(stringAsJson, settings);
                return toResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
