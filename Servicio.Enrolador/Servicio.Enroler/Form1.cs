using Entities.Standard;
using Servicio.Enroler.Server;
using Suprema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Servicio.Enrolador
{
    public partial class Form1 : Form
    {
        //SERVICIO DE SERVICE1.CS
        ENRService service;

        //ELEMENTOS GLOBALES
        //UFScannerManager m_ScannerManager;
        //UFMatcher m_Matcher;

        string m_strError;

        Form m_Form1 = Form1.ActiveForm;

        const int MAX_TEMPLATE_SIZE = 1024;
        const int MAX_TEMPLATE_NUM = 50;

        //TO ENROLL
        byte[][] m_template1;
        int m_template_num;
        int[] m_template_size1;


        public Form1()
        {
            InitializeComponent();
            service = new ENRService("-form");

            string[] var = new string[1];

            m_template1 = new byte[MAX_TEMPLATE_NUM][];
            for (int i = 0; i < MAX_TEMPLATE_NUM; i++)
            {
                m_template1[i] = new byte[MAX_TEMPLATE_SIZE];
            }
            m_template_size1 = new int[MAX_TEMPLATE_NUM];

            //service.StartService(new string[] { });

            tbxMessage.AppendText("MAX_TEMPLATE_SIZE: " + MAX_TEMPLATE_SIZE + "\r\n");
            tbxMessage.AppendText("MAX_TEMPLATE_NUM: " + MAX_TEMPLATE_NUM + "\r\n");
            tbxMessage.AppendText("Se inicio el servicio \r\n");

            service.StartService(new string[] { });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            service.StartService(new string[] { });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            service.StopService();
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string result;
            service.IniciarYseleccionarLector(this, out result);

            //ESCRIBO EN LA VISTA
            tbxMessage.Text = "UFMatcher: " + result + "\r\n";

            //ACTUALIZAR SCANNERS
            lbScannerList.Items.Clear();
            UFScannerManager toItem = service.UpdateScannerList();

            if (toItem != null)
                for (int i = 0; i < toItem.Scanners.Count; i++)
                {
                    UFScanner itemToScanner = toItem.Scanners[i];
                    UFS_SCANNER_TYPE ScannerType = itemToScanner.ScannerType;
                    string strID = itemToScanner.ID;

                    lbScannerList.Items.Add(i + ": " + ScannerType + "ID: " + strID);
                }

            if (lbScannerList.Items.Count > 0)
                lbScannerList.SetSelected(0, true);

            //UFScanner scanner = new UFScanner();
            //SE LE PASA EL NUMERO DE INDEX DE LA SELECCION. ESTE SE ELEGIRA DEL LISTADO DE CONECTADOS PARA UTILIZAR
            UFScanner scanner = new UFScanner();
            string result1 = ENRService.GetCurrentScannerSettings(0, out scanner);
            Console.WriteLine("");

            button3.Enabled = false;
        }

        public void btnStartCapturing_Click(object sender, EventArgs e)
        {
            int EnrollQuality;
            UFScanner Scanner = new UFScanner();
            UFS_STATUS ufs_res;

            ENRService.GetCurrentScannerSettings(0, out Scanner);

            //CUANDO SE DISPARA EL EVENTO, SE LLAMA A LA FUNCION CAPTUREEVENT
            try
            {
                Scanner.CaptureEvent += new UFS_CAPTURE_PROC(ENRService.CaptureEvent);


                Scanner.Timeout = 10000; //DEFINE EN MILISEGUNDOS CUANTO TIEMPO TOMARA LA CAPTURA

                ufs_res = Scanner.CaptureSingleImage();

                if (ufs_res == UFS_STATUS.OK)
                {
                    try
                    {
                        ufs_res = Scanner.ExtractEx(MAX_TEMPLATE_SIZE, m_template1[m_template_num], out m_template_size1[m_template_num], out EnrollQuality);

                        while (Scanner.IsCapturing)
                        {
                            //UpdatePictureBox(service.FingerPrintImage);
                            tbxMessage.AppendText("CAPTURANDO \r\n");
                        }

                        tbxMessage.AppendText("StartCapturing: OK \r\n");
                        UpdatePictureBox(ENRService.FingerPrintImage);

                        CalidadDeHuellaEnum quality = ENRService.ObtainQualityLevel(EnrollQuality);
                        tbxMessage.AppendText("Calidad de la HUELLA: " + quality.ToString() + "\r\n");

                        DrawCapturedImage(Scanner);
                    }
                    catch (Exception ex)
                    {
                        tbxMessage.AppendText("ERROR: " + ex.Message + "\r\n");
                    }
                }
                else
                {
                    UFScanner.GetErrorString(ufs_res, out m_strError);
                    tbxMessage.AppendText("UFScanner StartCapturing: " + m_strError + "\r\n");
                }
            }
            catch (Exception ex)
            {
                tbxMessage.AppendText("ERROR LECTOR: " + "No se encontro el lector seleccionado." + "\r\n");
            }
        }


        #region EVENTOS
        private void DrawCapturedImage(UFScanner Scanner)
        {
            Graphics g = pbImageFrame.CreateGraphics();
            Rectangle rect = new Rectangle(0, 0, pbImageFrame.Width, pbImageFrame.Height);
            try
            {
                Scanner.DrawCaptureImageBuffer(g, rect, cbDetectCore.Checked);
            }
            finally
            {
                g.Dispose();

            }
        }

        public void UpdatePictureBox(Image image)
        {
            if (image != null)
            {
                if (pbImageFrame.InvokeRequired)
                {
                    UpdatePictureBox(ENRService.FingerPrintImage);
                }
                else
                {
                    //ASIGNA LA IMAGEN
                    if (pbImageFrame.Image == null)
                    {
                        pbImageFrame.Image = image;
                    }
                    else
                    {
                        pbImageFrame.Image = null;
                        pbImageFrame.Image = image;
                    }
                }
            }
        }
        #endregion

        private void btnEnroll_Click(object sender, EventArgs e)
        {
            ENRService.EnrolarHuella();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            service.StopService();
        }
    }
}
