namespace Servicio.Enrolador
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button btnStartCapturing;
            System.Windows.Forms.Button btnEnroll;
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.lbScannerList = new System.Windows.Forms.ListBox();
            this.pbImageFrame = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudBrightness = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudSensitivity = new System.Windows.Forms.NumericUpDown();
            this.cbDetectCore = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbScanTemplateType = new System.Windows.Forms.ComboBox();
            this.tbxMessage = new System.Windows.Forms.TextBox();
            btnStartCapturing = new System.Windows.Forms.Button();
            btnEnroll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSensitivity)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartCapturing
            // 
            btnStartCapturing.Location = new System.Drawing.Point(224, 59);
            btnStartCapturing.Name = "btnStartCapturing";
            btnStartCapturing.Size = new System.Drawing.Size(218, 37);
            btnStartCapturing.TabIndex = 18;
            btnStartCapturing.Text = "COMENZAR CAPTURA";
            btnStartCapturing.UseVisualStyleBackColor = true;
            btnStartCapturing.Click += new System.EventHandler(this.btnStartCapturing_Click);
            // 
            // btnEnroll
            // 
            btnEnroll.Location = new System.Drawing.Point(224, 307);
            btnEnroll.Name = "btnEnroll";
            btnEnroll.Size = new System.Drawing.Size(218, 37);
            btnEnroll.TabIndex = 35;
            btnEnroll.Text = "ENROLAR";
            btnEnroll.UseVisualStyleBackColor = true;
            btnEnroll.Click += new System.EventHandler(this.btnEnroll_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 41);
            this.button1.TabIndex = 1;
            this.button1.Text = "INICIAR";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(112, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 41);
            this.button2.TabIndex = 2;
            this.button2.Text = "DETENER";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 59);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(194, 37);
            this.button3.TabIndex = 3;
            this.button3.Text = "ESCANEAR E INICIAR";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // lbScannerList
            // 
            this.lbScannerList.FormattingEnabled = true;
            this.lbScannerList.Location = new System.Drawing.Point(12, 102);
            this.lbScannerList.Name = "lbScannerList";
            this.lbScannerList.Size = new System.Drawing.Size(194, 82);
            this.lbScannerList.TabIndex = 5;
            // 
            // pbImageFrame
            // 
            this.pbImageFrame.BackColor = System.Drawing.SystemColors.Control;
            this.pbImageFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImageFrame.Location = new System.Drawing.Point(224, 102);
            this.pbImageFrame.Name = "pbImageFrame";
            this.pbImageFrame.Size = new System.Drawing.Size(218, 199);
            this.pbImageFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImageFrame.TabIndex = 19;
            this.pbImageFrame.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "BRILLO";
            // 
            // nudBrightness
            // 
            this.nudBrightness.Location = new System.Drawing.Point(138, 185);
            this.nudBrightness.Name = "nudBrightness";
            this.nudBrightness.Size = new System.Drawing.Size(68, 20);
            this.nudBrightness.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "SENSIBILIDAD";
            // 
            // nudSensitivity
            // 
            this.nudSensitivity.Location = new System.Drawing.Point(138, 209);
            this.nudSensitivity.Name = "nudSensitivity";
            this.nudSensitivity.Size = new System.Drawing.Size(68, 20);
            this.nudSensitivity.TabIndex = 29;
            // 
            // cbDetectCore
            // 
            this.cbDetectCore.AutoSize = true;
            this.cbDetectCore.Location = new System.Drawing.Point(12, 235);
            this.cbDetectCore.Name = "cbDetectCore";
            this.cbDetectCore.Size = new System.Drawing.Size(131, 17);
            this.cbDetectCore.TabIndex = 30;
            this.cbDetectCore.Text = "DETECTAR NUCLEO";
            this.cbDetectCore.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 260);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "TIPO DE DATA";
            // 
            // cbScanTemplateType
            // 
            this.cbScanTemplateType.FormattingEnabled = true;
            this.cbScanTemplateType.Items.AddRange(new object[] {
            "suprema",
            "iso19794_2",
            "ansi378"});
            this.cbScanTemplateType.Location = new System.Drawing.Point(104, 252);
            this.cbScanTemplateType.Name = "cbScanTemplateType";
            this.cbScanTemplateType.Size = new System.Drawing.Size(102, 21);
            this.cbScanTemplateType.TabIndex = 32;
            // 
            // tbxMessage
            // 
            this.tbxMessage.Location = new System.Drawing.Point(12, 351);
            this.tbxMessage.Multiline = true;
            this.tbxMessage.Name = "tbxMessage";
            this.tbxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMessage.Size = new System.Drawing.Size(430, 87);
            this.tbxMessage.TabIndex = 33;
            this.tbxMessage.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 450);
            this.Controls.Add(btnEnroll);
            this.Controls.Add(this.tbxMessage);
            this.Controls.Add(this.cbScanTemplateType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbDetectCore);
            this.Controls.Add(this.nudSensitivity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudBrightness);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbImageFrame);
            this.Controls.Add(btnStartCapturing);
            this.Controls.Add(this.lbScannerList);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pbImageFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSensitivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox lbScannerList;
        private System.Windows.Forms.PictureBox pbImageFrame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudBrightness;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudSensitivity;
        private System.Windows.Forms.CheckBox cbDetectCore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbScanTemplateType;
        private System.Windows.Forms.TextBox tbxMessage;
    }
}