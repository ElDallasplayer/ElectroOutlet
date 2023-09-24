using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ServicioWindows
{
    public class XMLConversion
    {
        public static void ObjectToXml(object obj, string path_to_xml)
        {
            ////serialize and persist it to it's file
            //try
            //{
            //    XmlSerializer ser = new XmlSerializer(obj.GetType());
            //    FileStream fs = FileSystem.WaitForFile(path_to_xml, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            //    ser.Serialize(fs, obj);
            //    fs.Close();
            //}
            //catch (Exception ex)
            //{
            //    /*throw new Exception(
            //            "No se pudo serializar el objeto a " + path_to_xml,
            //            ex);*/
            //}
        }
        public static string ObjectToXml(object obj)
        {
            //serialize and persist it to it's file
            try
            {
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                using (StringWriter textWriter = new StringWriter())
                {
                    ser.Serialize(textWriter, obj);
                    return textWriter.ToString();
                }
            }
            catch (Exception)
            {
                return string.Empty;
                /*throw new Exception(
                        "No se pudo serializar el objeto a " + path_to_xml,
                        ex);*/
            }
        }

        //public static T XmlToObject<T>(string path_to_xml)
        //{
        //    StreamReader reader;
        //    try
        //    {
        //        T config = default(T);

        //        FileStream fs = FileSystem.WaitForFile(path_to_xml, FileMode.Open, FileAccess.Read, FileShare.Read);
        //        reader = new StreamReader(fs);
        //        XmlSerializer serializer = new XmlSerializer(typeof(T));
        //        object deserialized = serializer.Deserialize(reader.BaseStream);

        //        config = (T)deserialized;
        //        reader.Close();
        //        return config;
        //    }
        //    catch (Exception ex)
        //    {
        //        return default(T);
        //    }
        //}

        public static dynamic XmlStringToObject(string xml, Type type)
        {
            StreamReader reader;
            try
            {
                dynamic res = Activator.CreateInstance(type);

                reader = new StreamReader(GenerateStreamFromString(xml));
                XmlSerializer serializer = new XmlSerializer(type);
                object deserialized = serializer.Deserialize(reader.BaseStream);

                res = deserialized;
                reader.Close();
                return res;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}