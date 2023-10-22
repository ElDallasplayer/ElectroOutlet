using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Intelektron.Common.DataTransfer.Entities.Standard;
//using Intelektron.Business.SDK;
using System.Security.Principal;
//using Intelektron.Environment;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using Microsoft.VisualBasic.FileIO;
//using Intelektron.Environment;

namespace Servicio.Enrolador
{
    internal class Utilidades
    {

        #region MENSAJES DE LOG
        public static void MensajeLog(string mensaje, string archivo = "ServicioDeEnrolador", string extension = ".log")
        {
            Console.WriteLine("[" + DateTime.Now.ToString("") + "] " + mensaje);
            try
            {
                string path = @"C:/ProgramData/ServicioBiostar/";
                string file = @"C:/ProgramData/ServicioBiostar/ServicioDeEnrolador";

                if (File.Exists(file))
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Length >= 500000) //si pesa más de 500KB, lo renombro
                        System.IO.File.Move(file, path + archivo + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                }

                using (StreamWriter fileLog = new StreamWriter(file, true))
                {
                    fileLog.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] " /*+ GetCurrentMethod() + " | "*/ + mensaje);
                    fileLog.Close();
                }

                #region LOG GLOBAL
                string path_global = @"C:/ProgramData/ServicioBiostar/";
                string file_global = path_global + "Servicio_Enrolador" + extension;

                if (File.Exists(file_global))
                {
                    FileInfo fi = new FileInfo(file_global);
                    if (fi.Length >= 500000) //si pesa más de 500KB, lo renombro
                        System.IO.File.Move(file_global, path_global + "Servicio_Enrolador_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                }

                using (StreamWriter fileLog = new StreamWriter(file_global, true))
                {
                    fileLog.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] " /*+ GetCurrentMethod() + " | "*/ + mensaje);
                    fileLog.Close();
                }
                #endregion
            }
            catch (Exception ex) { }
        }

        public static void LoguearExcepcion(Exception e, bool inner = false)
        {
            //MensajeLog((inner ? " --> " : "") + "[" + e.Message + "]", "ERROR");
            if (e.InnerException != null)
                LoguearExcepcion(e.InnerException, true);
        }
        #endregion
    }

    public static class Extensions
    {
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
        {
            var queue = new Queue<T>();

            using (var e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (queue.Count == count)
                    {
                        do
                        {
                            yield return queue.Dequeue();
                            queue.Enqueue(e.Current);
                        } while (e.MoveNext());
                    }
                    else
                    {
                        queue.Enqueue(e.Current);
                    }
                }
            }
        }
    }
}
