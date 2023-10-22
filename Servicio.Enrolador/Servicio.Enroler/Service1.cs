using Suprema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Servicio.Enrolador;
using System.Drawing;
using System.Threading;
using Entities.Standard;
using System.IO;
using System.Drawing.Imaging;
using Servicio.Enroler.Server;
using System.Windows.Threading;

namespace Servicio.Enrolador
{
    public partial class ENRService : ServiceBase
    {
        static UFScannerManager m_ScannerManager;
        static UFMatcher m_Matcher;

        //TO TEMPLATE
        const int MAX_TEMPLATE_SIZE = 384;
        const int MAX_TEMPLATE_NUM = 50;

        //TO ENROLL
        static byte[][] m_template1;
        static int m_template_num;
        static int[] m_template_size1;

        static byte[] Template0 = new byte[MAX_TEMPLATE_SIZE];
        static byte[] Template1 = new byte[MAX_TEMPLATE_SIZE];

        static public Image FingerPrintImage;

        public ENRService(string call)
        {
            InitializeComponent();

            if (call == "-service")
            {
                OnStart(new string[1]);

                string result;
                IniciarYseleccionarLector(new Form1(), out result);
            }
            else if (call == "-form")
            {
            }
            else if (call == "-window")
            {

            }
        }

        public void StartService(string[] args)
        {
            OnStart(args);
        }

        public void StopService()
        {
            this.Stop();
        }

        protected override async void OnStart(string[] args)
        {
            string result;
            string pcName = Environment.MachineName;
        }

        protected override async void OnStop()
        {
            string pcName = Environment.MachineName;
            //await (await ServicioEnrolador.ObtenerServicio(this, pcName)).DesconectarServicioEnroladorAsync(ServicioEnrolador.IDWebServiceController);
        }

        public static async Task<Entities.Standard.Enrolador> EnrolarHuella()
        {
            Utilidades.MensajeLog("Iniciando captura");
            string pepe = "pepe";
            string m_strError;

            UFS_STATUS ufs_res;

            try
            {
                Form m_Form = new Form();
                m_ScannerManager = new UFScannerManager(m_Form);
                m_Matcher = new UFMatcher();

                ufs_res = m_ScannerManager.Init();
                if (ufs_res == UFS_STATUS.ERR_ALREADY_INITIALIZED)
                {
                    m_ScannerManager.Uninit();
                }
                ufs_res = m_ScannerManager.Init();
                if (ufs_res == UFS_STATUS.OK)
                {
                    if (m_Matcher.InitResult == UFM_STATUS.OK)
                    {
                        Utilidades.MensajeLog("Enrolador conectado");
                    }
                    else
                    {
                        Utilidades.MensajeLog("Error al iniciar enrolador");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidades.MensajeLog("Error al enrolar: " + ex.Message);
            }

            //TO TEMPLATE------------------------------------------------------------------
            string[] var = new string[1];

            m_template1 = new byte[MAX_TEMPLATE_NUM][];
            for (int i = 0; i < MAX_TEMPLATE_NUM; i++)
            {
                m_template1[i] = new byte[MAX_TEMPLATE_SIZE];
            }
            m_template_size1 = new int[MAX_TEMPLATE_NUM];
            //TO TEMPLATE------------------------------------------------------------------

            //var serv = ServicioEnrolador.ObtenerServicio(this).EnrolarHuella(guid,pepe); 
            int EnrollQuality;
            UFScanner Scanner = new UFScanner();

            try
            {
                GetCurrentScannerSettings(0, out Scanner);
            }
            catch (Exception e) { }

            try
            {
                //CUANDO SE DISPARA EL EVENTO, SE LLAMA A LA FUNCION CAPTUREEVENT
                Scanner.CaptureEvent += new UFS_CAPTURE_PROC(CaptureEvent);

                Scanner.Timeout = 10000; //DEFINE EN MILISEGUNDOS CUANTO TIEMPO TOMARA LA CAPTURA

                Utilidades.MensajeLog("Comensazondo captura individual");
                ufs_res = Scanner.CaptureSingleImage();
                
                if (ufs_res == UFS_STATUS.OK)
                {
                    Utilidades.MensajeLog("Captura indivisual tomada correctamente");
                    try
                    {
                        ufs_res = Scanner.ExtractEx(MAX_TEMPLATE_SIZE, Template0, out m_template_size1[m_template_num], out EnrollQuality);

                        //QUALITY 
                        CalidadDeHuellaEnum quality = ObtainQualityLevel(EnrollQuality);
                        Form ap = new Form();
                        Graphics g = ap.CreateGraphics();
                        Rectangle rect = new Rectangle(0, 0, 300, 300);

                        Bitmap bmp = new Bitmap(100, 100, g);
                        int resol = 0;
                        Scanner.GetCaptureImageBuffer(out bmp, out resol);

                        Byte[] data;

                        using (var memoryStream = new MemoryStream())
                        {
                            bmp.Save(memoryStream, ImageFormat.Bmp);

                            data = memoryStream.ToArray();
                        }
                        string base64 = Convert.ToBase64String(data);

                        UFMatcher matcher = new UFMatcher();
                        matcher.RotateTemplate(m_template1[m_template_num], MAX_TEMPLATE_SIZE);

                        Entities.Standard.Enrolador enrolRet = new Entities.Standard.Enrolador();
                        enrolRet.CalidadHuella = quality;
                        enrolRet.Template = Template0;
                        enrolRet.ImageAsBytes = base64;
                        enrolRet.Error = "";
                        enrolRet.EstadoDeServicio = EstadosDevueltosPorServicio.Success;

                        return enrolRet;
                    }
                    catch (Exception ex)
                    {
                        Entities.Standard.Enrolador enrolRet = new Entities.Standard.Enrolador();

                        enrolRet.Error = ex.Message;
                        enrolRet.EstadoDeServicio = EstadosDevueltosPorServicio.Error;

                        return enrolRet;
                    }
                }
                else
                {
                    UFScanner.GetErrorString(ufs_res, out m_strError);
                    Entities.Standard.Enrolador enrolRet = new Entities.Standard.Enrolador();
                    Utilidades.MensajeLog("Captura de huella erronea: ERROR: " + m_strError);
                    enrolRet.Error = m_strError;
                    enrolRet.EstadoDeServicio = EstadosDevueltosPorServicio.Fault;

                    return enrolRet;
                }
            }
            catch (Exception ex)
            {
                Utilidades.MensajeLog("Excepcion ocurre durante ejecucion del metodo: EX: " + ex.Message);
                dynamic enrolRet = new Entities.Standard.Enrolador();

                enrolRet.Error = ex.Message;
                enrolRet.EstadoDeServicio = EstadosDevueltosPorServicio.Fault;

                return enrolRet;

            }

        }

        public static async Task<Entities.Standard.Enrolador> VerificarHuella(string Template)
        {
            Utilidades.MensajeLog("Iniciando captura");
            //string pepe = "pepe";
            string m_strError;

            UFS_STATUS ufs_res;

            try
            {
                Form m_Form = new Form();
                m_ScannerManager = new UFScannerManager(m_Form);
                m_Matcher = new UFMatcher();

                ufs_res = m_ScannerManager.Init();
                if (ufs_res == UFS_STATUS.ERR_ALREADY_INITIALIZED)
                {
                    m_ScannerManager.Uninit();
                }
                ufs_res = m_ScannerManager.Init();
                if (ufs_res == UFS_STATUS.OK)
                {
                    if (m_Matcher.InitResult == UFM_STATUS.OK)
                    {
                        Utilidades.MensajeLog("Enrolador conectado");
                    }
                    else
                    {
                        Utilidades.MensajeLog("Error al iniciar enrolador");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidades.MensajeLog("Error al enrolar: " + ex.Message);
            }

            //TO TEMPLATE------------------------------------------------------------------
            string[] var = new string[1];

            m_template1 = new byte[MAX_TEMPLATE_NUM][];
            for (int i = 0; i < MAX_TEMPLATE_NUM; i++)
            {
                m_template1[i] = new byte[MAX_TEMPLATE_SIZE];
            }
            m_template_size1 = new int[MAX_TEMPLATE_NUM];
            //TO TEMPLATE------------------------------------------------------------------

            //var serv = ServicioEnrolador.ObtenerServicio(this).EnrolarHuella(guid,pepe); 
            int EnrollQuality;
            UFScanner Scanner = new UFScanner();

            try
            {
                GetCurrentScannerSettings(0, out Scanner);
            }
            catch (Exception e) { }

            try
            {
                //CUANDO SE DISPARA EL EVENTO, SE LLAMA A LA FUNCION CAPTUREEVENT
                Scanner.CaptureEvent += new UFS_CAPTURE_PROC(CaptureEvent);

                Scanner.Timeout = 10000; //DEFINE EN MILISEGUNDOS CUANTO TIEMPO TOMARA LA CAPTURA

                Utilidades.MensajeLog("Comensazondo captura individual");
                ufs_res = Scanner.CaptureSingleImage();

                if (ufs_res == UFS_STATUS.OK)
                {
                    Utilidades.MensajeLog("Captura indivisual tomada correctamente");
                    try
                    {
                        ufs_res = Scanner.ExtractEx(MAX_TEMPLATE_SIZE, Template0, out m_template_size1[m_template_num], out EnrollQuality);

                        //QUALITY 
                        CalidadDeHuellaEnum quality = ObtainQualityLevel(EnrollQuality);
                        Form ap = new Form();
                        Graphics g = ap.CreateGraphics();
                        Rectangle rect = new Rectangle(0, 0, 300, 300);

                        Bitmap bmp = new Bitmap(100, 100, g);
                        int resol = 0;
                        Scanner.GetCaptureImageBuffer(out bmp, out resol);

                        Byte[] data;

                        using (var memoryStream = new MemoryStream())
                        {
                            bmp.Save(memoryStream, ImageFormat.Bmp);

                            data = memoryStream.ToArray();
                        }
                        string base64 = Convert.ToBase64String(data);

                        UFMatcher matcher = new UFMatcher();
                        matcher.RotateTemplate(m_template1[m_template_num], MAX_TEMPLATE_SIZE);

                        Entities.Standard.Enrolador enrolRet = new Entities.Standard.Enrolador();
                        enrolRet.CalidadHuella = quality;
                        enrolRet.Template = Template0;
                        enrolRet.ImageAsBytes = base64;
                        enrolRet.Error = "";
                        enrolRet.EstadoDeServicio = EstadosDevueltosPorServicio.Success;


                        //byte[] tempReferencia = StringToByteArray(Template);
                        byte[] tempReferencia = Convert.FromBase64String(Template);
                        
                        bool resultComparation = false;

                        UFM_STATUS reste = m_Matcher.Verify(tempReferencia,384, Template0, 384,out resultComparation);

                        enrolRet.ValorComparacion = resultComparation;

                        return enrolRet;
                    }
                    catch (Exception ex)
                    {
                        Entities.Standard.Enrolador enrolRet = new Entities.Standard.Enrolador();

                        enrolRet.Error = ex.Message;
                        enrolRet.EstadoDeServicio = EstadosDevueltosPorServicio.Error;

                        return enrolRet;
                    }
                }
                else
                {
                    UFScanner.GetErrorString(ufs_res, out m_strError);
                    Entities.Standard.Enrolador enrolRet = new Entities.Standard.Enrolador();
                    Utilidades.MensajeLog("Captura de huella erronea: ERROR: " + m_strError);
                    enrolRet.Error = m_strError;
                    enrolRet.EstadoDeServicio = EstadosDevueltosPorServicio.Fault;

                    return enrolRet;
                }
            }
            catch (Exception ex)
            {
                Utilidades.MensajeLog("Excepcion ocurre durante ejecucion del metodo: EX: " + ex.Message);
                dynamic enrolRet = new Entities.Standard.Enrolador();

                enrolRet.Error = ex.Message;
                enrolRet.EstadoDeServicio = EstadosDevueltosPorServicio.Fault;

                return enrolRet;

            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static async Task<bool> ObtenerEstado()
        {
            return true;
        }

        #region FUNCIONES CON BIOMINI

        public string IniciarYseleccionarLector(Form m_Form, out string result)
        {
            UFS_STATUS ufs_res;
            try
            {
                m_ScannerManager = new UFScannerManager(m_Form);
                m_Matcher = new UFMatcher();

                ufs_res = m_ScannerManager.Init();

                result = "ERROR";

                if (ufs_res == UFS_STATUS.OK)
                {
                    if (m_Matcher.InitResult == UFM_STATUS.OK)
                    {
                        result = "OK";
                    }

                }

                return result;
            }
            catch (Exception ex)
            {
                result = "ERROR: " + ex.Message;
                return result;
            }

        }

        #region Actualizar listado de scanners
        public UFScannerManager UpdateScannerList()
        {
            if (m_ScannerManager != null)
            {

                UFScannerManager Scanner = m_ScannerManager;
                return Scanner;
            }
            return null;
        }
        #endregion

        #region Config actuales
        //SE LE PASA EL IDEX DEL SCANNER SELECCIONADO Y UN OUT QUE DEVOLVERA LA CONFIGURACION DEL ITEM
        public static string GetCurrentScannerSettings(int item, out UFScanner scanner)
        {
            scanner = m_ScannerManager.Scanners[item];

            if (scanner != null)
            {
                return "OK";
            }
            return "ERROR";
        }
        #endregion

        #region LISTADO DE SCANNERS
        //LISTADO DE POSIBLES SCANNERS SEGUN ENUM
        public void GetScannerTypeString(UFS_SCANNER_TYPE ScannerType, out string strScannerType)
        {
            if (ScannerType == UFS_SCANNER_TYPE.SFR200)
            {
                strScannerType = "SFR200";
            }
            else if (ScannerType == UFS_SCANNER_TYPE.SFR300)
            {
                strScannerType = "SFR300";
            }
            else if (ScannerType == UFS_SCANNER_TYPE.SFR300v2)
            {
                strScannerType = "SFR300v2";
            }
            else if (ScannerType == UFS_SCANNER_TYPE.SFR500)
            {
                strScannerType = "BioMini Plus";
            }
            else if (ScannerType == UFS_SCANNER_TYPE.SFR550)
            {
                strScannerType = "BioMini Plus 2";
            }
            else if (ScannerType == UFS_SCANNER_TYPE.SFR600)
            {
                strScannerType = "BioMini SLIM";
            }
            else if (ScannerType == UFS_SCANNER_TYPE.SFR700)
            {
                strScannerType = "BioMini SLIM 2";
            }
            else if (ScannerType == UFS_SCANNER_TYPE.SFR410) //ESTE ES EL BIOMINI
            {
                strScannerType = "BioMini 1";
            }
            else
            {
                strScannerType = "Error";
            }
        }
        #endregion

        #endregion

        #region EVENTOS
        //EL EVENTO CAPTURA LA IMAGEN DEL BIOMETRICO Y LA PASA A LA 
        public static int CaptureEvent(object sender, UFScannerCaptureEventArgs e)
        {
            PictureBox pictureBox = new PictureBox();
            //Se usa indirectamente por los trheads
            FingerPrintImage = e.ImageFrame;

            UFScannerCaptureEventArgs img = e;
            return 1; //DEBERIA DEVOVLER LA IMAGEN A LA VISTA ANTES DE GUARDARLA, DE ESTA FORMA SE PODRIA VER LA CALIDAD
        }

        #region OBTAIN QUALITY

        public static CalidadDeHuellaEnum ObtainQualityLevel(int EnrollQuality)
        {
            QualityResponse resQua = DefineQualityLevel(EnrollQuality);

            return resQua.QualityLevel;
        }

        public class QualityResponse
        {
            public CalidadDeHuellaEnum QualityLevel;
            public int ResponseState; // 0 = error, 1 = OK
        }

        public static QualityResponse DefineQualityLevel(int EnrollQuality)
        {
            QualityResponse response = new QualityResponse();
            response.ResponseState = 1;

            try
            {
                if (EnrollQuality >= 50 && EnrollQuality < 70)
                    response.QualityLevel = CalidadDeHuellaEnum.Buena;
                else if (EnrollQuality >= 70 && EnrollQuality < 80)
                    response.QualityLevel = CalidadDeHuellaEnum.MuyBuena;
                else if (EnrollQuality >= 80 && EnrollQuality <= 100)
                    response.QualityLevel = CalidadDeHuellaEnum.Excelente;
                else
                {
                    response.QualityLevel = CalidadDeHuellaEnum.Error;
                    response.ResponseState = 0;
                }
            }
            catch (Exception ex)
            {
                response.QualityLevel = CalidadDeHuellaEnum.Error;
                response.ResponseState = 0;
            }

            return response;
        }
        #endregion

        #endregion
    }
}
