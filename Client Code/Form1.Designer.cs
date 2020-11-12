namespace SearchGCS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.cbo = new System.Windows.Forms.ComboBox();
            this.BtnUp = new System.Windows.Forms.Button();
            this.BtnDown = new System.Windows.Forms.Button();
            this.BtnLeft = new System.Windows.Forms.Button();
            this.BtnRight = new System.Windows.Forms.Button();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.MainMap = new GMap.NET.WindowsForms.GMapControl();
            this.label5 = new System.Windows.Forms.Label();
            this.cbMapProviders = new System.Windows.Forms.ComboBox();
            this.BtnLeftTurn = new System.Windows.Forms.Button();
            this.BtnRightTurn = new System.Windows.Forms.Button();
            this.BtnAscend = new System.Windows.Forms.Button();
            this.BtnDescend = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtZ = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtRot = new System.Windows.Forms.TextBox();
            this.BtnConnect = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.txtSig = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtTimeDelay = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.radar_box = new System.Windows.Forms.PictureBox();
            this.btnVoice = new System.Windows.Forms.Button();
            this.pbo = new AForge.Controls.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.radar_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Magenta;
            this.label1.Name = "label1";
            // 
            // BtnStart
            // 
            this.BtnStart.Image = global::Blimp_GCS.Properties.Resources.radar;
            resources.ApplyResources(this.BtnStart, "BtnStart");
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnStop
            // 
            resources.ApplyResources(this.BtnStop, "BtnStop");
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // cbo
            // 
            this.cbo.FormattingEnabled = true;
            resources.ApplyResources(this.cbo, "cbo");
            this.cbo.Name = "cbo";
            this.cbo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbo_KeyPress);
            // 
            // BtnUp
            // 
            this.BtnUp.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.BtnUp, "BtnUp");
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.UseVisualStyleBackColor = false;
            // 
            // BtnDown
            // 
            this.BtnDown.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.BtnDown, "BtnDown");
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.UseVisualStyleBackColor = false;
            // 
            // BtnLeft
            // 
            this.BtnLeft.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.BtnLeft, "BtnLeft");
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.UseVisualStyleBackColor = false;
            // 
            // BtnRight
            // 
            this.BtnRight.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.BtnRight, "BtnRight");
            this.BtnRight.Name = "BtnRight";
            this.BtnRight.UseVisualStyleBackColor = false;
            // 
            // splitter2
            // 
            resources.ApplyResources(this.splitter2, "splitter2");
            this.splitter2.Name = "splitter2";
            this.splitter2.TabStop = false;
            // 
            // MainMap
            // 
            resources.ApplyResources(this.MainMap, "MainMap");
            this.MainMap.Bearing = 0F;
            this.MainMap.CanDragMap = true;
            this.MainMap.EmptyTileColor = System.Drawing.Color.Navy;
            this.MainMap.GrayScaleMode = false;
            this.MainMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.MainMap.LevelsKeepInMemory = 5;
            this.MainMap.MarkersEnabled = true;
            this.MainMap.MaxZoom = 2;
            this.MainMap.MinZoom = 2;
            this.MainMap.MouseWheelZoomEnabled = true;
            this.MainMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.MainMap.Name = "MainMap";
            this.MainMap.NegativeMode = false;
            this.MainMap.PolygonsEnabled = true;
            this.MainMap.RetryLoadTile = 0;
            this.MainMap.RoutesEnabled = true;
            this.MainMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.MainMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.MainMap.ShowTileGridLines = false;
            this.MainMap.Zoom = 0D;
            this.MainMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainMap_MouseClick);
            this.MainMap.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MainMap_MouseDoubleClick);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Yellow;
            this.label5.Name = "label5";
            // 
            // cbMapProviders
            // 
            this.cbMapProviders.FormattingEnabled = true;
            resources.ApplyResources(this.cbMapProviders, "cbMapProviders");
            this.cbMapProviders.Name = "cbMapProviders";
            this.cbMapProviders.SelectedIndexChanged += new System.EventHandler(this.cbMapProviders_SelectedIndexChanged);
            // 
            // BtnLeftTurn
            // 
            this.BtnLeftTurn.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.BtnLeftTurn, "BtnLeftTurn");
            this.BtnLeftTurn.Name = "BtnLeftTurn";
            this.BtnLeftTurn.UseVisualStyleBackColor = false;
            // 
            // BtnRightTurn
            // 
            this.BtnRightTurn.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.BtnRightTurn, "BtnRightTurn");
            this.BtnRightTurn.Name = "BtnRightTurn";
            this.BtnRightTurn.UseVisualStyleBackColor = false;
            // 
            // BtnAscend
            // 
            this.BtnAscend.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.BtnAscend, "BtnAscend");
            this.BtnAscend.Name = "BtnAscend";
            this.BtnAscend.UseVisualStyleBackColor = false;
            // 
            // BtnDescend
            // 
            this.BtnDescend.BackColor = System.Drawing.Color.Yellow;
            resources.ApplyResources(this.BtnDescend, "BtnDescend");
            this.BtnDescend.Name = "BtnDescend";
            this.BtnDescend.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.Yellow;
            this.label6.Name = "label6";
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.SlateGray;
            this.progressBar1.ForeColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Value = 30;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.Yellow;
            this.label7.Name = "label7";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ForeColor = System.Drawing.Color.Yellow;
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Name = "label9";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Name = "label11";
            // 
            // txtX
            // 
            resources.ApplyResources(this.txtX, "txtX");
            this.txtX.Name = "txtX";
            this.txtX.UseWaitCursor = true;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.ForeColor = System.Drawing.Color.Lime;
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.ForeColor = System.Drawing.Color.Lime;
            this.label13.Name = "label13";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.ForeColor = System.Drawing.Color.Lime;
            this.label14.Name = "label14";
            // 
            // txtY
            // 
            resources.ApplyResources(this.txtY, "txtY");
            this.txtY.Name = "txtY";
            this.txtY.UseWaitCursor = true;
            // 
            // txtZ
            // 
            resources.ApplyResources(this.txtZ, "txtZ");
            this.txtZ.Name = "txtZ";
            this.txtZ.UseWaitCursor = true;
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.ForeColor = System.Drawing.Color.Lime;
            this.label15.Name = "label15";
            // 
            // txtRot
            // 
            resources.ApplyResources(this.txtRot, "txtRot");
            this.txtRot.Name = "txtRot";
            this.txtRot.UseWaitCursor = true;
            // 
            // BtnConnect
            // 
            this.BtnConnect.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.BtnConnect, "BtnConnect");
            this.BtnConnect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.UseVisualStyleBackColor = false;
            this.BtnConnect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BtnConnect_MouseClick);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.ForeColor = System.Drawing.Color.Lime;
            this.label16.Name = "label16";
            // 
            // txtSig
            // 
            resources.ApplyResources(this.txtSig, "txtSig");
            this.txtSig.Name = "txtSig";
            this.txtSig.UseWaitCursor = true;
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.ForeColor = System.Drawing.Color.Lime;
            this.label17.Name = "label17";
            // 
            // txtTimeDelay
            // 
            resources.ApplyResources(this.txtTimeDelay, "txtTimeDelay");
            this.txtTimeDelay.Name = "txtTimeDelay";
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // radar_box
            // 
            this.radar_box.BackColor = System.Drawing.Color.Black;
            this.radar_box.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.radar_box, "radar_box");
            this.radar_box.Name = "radar_box";
            this.radar_box.TabStop = false;
            // 
            // btnVoice
            // 
            resources.ApplyResources(this.btnVoice, "btnVoice");
            this.btnVoice.ForeColor = System.Drawing.Color.Black;
            this.btnVoice.Image = global::Blimp_GCS.Properties.Resources.voice;
            this.btnVoice.Name = "btnVoice";
            this.btnVoice.UseVisualStyleBackColor = true;
            this.btnVoice.Click += new System.EventHandler(this.btnVoice_Click);
            // 
            // pbo
            // 
            this.pbo.BackColor = System.Drawing.Color.Black;
            this.pbo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.pbo, "pbo");
            this.pbo.Name = "pbo";
            this.pbo.TabStop = false;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::Blimp_GCS.Properties.Resources.earth;
            this.Controls.Add(this.radar_box);
            this.Controls.Add(this.btnVoice);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtTimeDelay);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtSig);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.BtnConnect);
            this.Controls.Add(this.txtRot);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtZ);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BtnDescend);
            this.Controls.Add(this.BtnAscend);
            this.Controls.Add(this.BtnRightTurn);
            this.Controls.Add(this.BtnLeftTurn);
            this.Controls.Add(this.cbMapProviders);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.MainMap);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.BtnRight);
            this.Controls.Add(this.BtnLeft);
            this.Controls.Add(this.BtnDown);
            this.Controls.Add(this.BtnUp);
            this.Controls.Add(this.cbo);
            this.Controls.Add(this.pbo);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.radar_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Button BtnStop;
        private AForge.Controls.PictureBox pbo;
        private System.Windows.Forms.ComboBox cbo;
        private System.Windows.Forms.Button BtnUp;
        private System.Windows.Forms.Button BtnDown;
        private System.Windows.Forms.Button BtnLeft;
        private System.Windows.Forms.Button BtnRight;
        private System.Windows.Forms.Splitter splitter2;
        private GMap.NET.WindowsForms.GMapControl MainMap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbMapProviders;
        private System.Windows.Forms.Button BtnLeftTurn;
        private System.Windows.Forms.Button BtnRightTurn;
        private System.Windows.Forms.Button BtnAscend;
        private System.Windows.Forms.Button BtnDescend;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtZ;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtRot;
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtSig;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtTimeDelay;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnVoice;
        private System.Windows.Forms.PictureBox radar_box;
    }
}

