namespace QuantPipe
{
    partial class MainForm
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
            this.flowLayoutPanelRun = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Tcomment = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Nim = new System.Windows.Forms.NumericUpDown();
            this.Cim = new System.Windows.Forms.CheckBox();
            this.Cipf = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button7 = new System.Windows.Forms.Button();
            this.Lin = new System.Windows.Forms.ListBox();
            this.Twin = new System.Windows.Forms.TextBox();
            this.Tmod = new System.Windows.Forms.TextBox();
            this.Tlib = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.Tdb = new System.Windows.Forms.TextBox();
            this.Tpy = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.Tirt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fopen = new System.Windows.Forms.OpenFileDialog();
            this.fsave = new System.Windows.Forms.SaveFileDialog();
            this.runner = new System.ComponentModel.BackgroundWorker();
            this.pro = new System.Diagnostics.Process();
            this.flowLayoutPanelRun.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nim)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanelRun
            // 
            this.flowLayoutPanelRun.AutoScroll = true;
            this.flowLayoutPanelRun.AutoSize = true;
            this.flowLayoutPanelRun.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelRun.Controls.Add(this.groupBox1);
            this.flowLayoutPanelRun.Controls.Add(this.groupBox3);
            this.flowLayoutPanelRun.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelRun.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelRun.Name = "flowLayoutPanelRun";
            this.flowLayoutPanelRun.Size = new System.Drawing.Size(774, 419);
            this.flowLayoutPanelRun.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Tcomment);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(768, 114);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preset";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Comment";
            // 
            // Tcomment
            // 
            this.Tcomment.Location = new System.Drawing.Point(9, 39);
            this.Tcomment.Multiline = true;
            this.Tcomment.Name = "Tcomment";
            this.Tcomment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Tcomment.Size = new System.Drawing.Size(598, 51);
            this.Tcomment.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(687, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 52);
            this.button2.TabIndex = 1;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(613, 38);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.Nim);
            this.groupBox3.Controls.Add(this.Cim);
            this.groupBox3.Controls.Add(this.Cipf);
            this.groupBox3.Controls.Add(this.progressBar1);
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.Lin);
            this.groupBox3.Controls.Add(this.Twin);
            this.groupBox3.Controls.Add(this.Tmod);
            this.groupBox3.Controls.Add(this.Tlib);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.Tdb);
            this.groupBox3.Controls.Add(this.Tpy);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.Tirt);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(3, 123);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(768, 293);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Path";
            // 
            // Nim
            // 
            this.Nim.DecimalPlaces = 2;
            this.Nim.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Nim.Location = new System.Drawing.Point(624, 256);
            this.Nim.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Nim.Name = "Nim";
            this.Nim.Size = new System.Drawing.Size(56, 25);
            this.Nim.TabIndex = 6;
            this.Nim.Value = new decimal(new int[] {
            4,
            0,
            0,
            131072});
            // 
            // Cim
            // 
            this.Cim.AutoSize = true;
            this.Cim.Location = new System.Drawing.Point(541, 257);
            this.Cim.Name = "Cim";
            this.Cim.Size = new System.Drawing.Size(77, 19);
            this.Cim.TabIndex = 5;
            this.Cim.Text = "use im";
            this.Cim.UseVisualStyleBackColor = true;
            // 
            // Cipf
            // 
            this.Cipf.AutoSize = true;
            this.Cipf.Location = new System.Drawing.Point(708, 20);
            this.Cipf.Name = "Cipf";
            this.Cipf.Size = new System.Drawing.Size(53, 19);
            this.Cipf.TabIndex = 5;
            this.Cipf.Text = "IPF";
            this.Cipf.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(218, 248);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(317, 33);
            this.progressBar1.TabIndex = 4;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(686, 248);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 34);
            this.button7.TabIndex = 3;
            this.button7.Text = "run!";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Lin
            // 
            this.Lin.FormattingEnabled = true;
            this.Lin.HorizontalScrollbar = true;
            this.Lin.ItemHeight = 15;
            this.Lin.Location = new System.Drawing.Point(6, 133);
            this.Lin.Name = "Lin";
            this.Lin.ScrollAlwaysVisible = true;
            this.Lin.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.Lin.Size = new System.Drawing.Size(756, 109);
            this.Lin.TabIndex = 2;
            // 
            // Twin
            // 
            this.Twin.Location = new System.Drawing.Point(472, 80);
            this.Twin.Name = "Twin";
            this.Twin.Size = new System.Drawing.Size(289, 25);
            this.Twin.TabIndex = 1;
            this.Twin.DoubleClick += new System.EventHandler(this.Twin_Click);
            // 
            // Tmod
            // 
            this.Tmod.Location = new System.Drawing.Point(472, 18);
            this.Tmod.Name = "Tmod";
            this.Tmod.Size = new System.Drawing.Size(230, 25);
            this.Tmod.TabIndex = 1;
            this.Tmod.DoubleClick += new System.EventHandler(this.Tmod_Click);
            // 
            // Tlib
            // 
            this.Tlib.Location = new System.Drawing.Point(75, 49);
            this.Tlib.Name = "Tlib";
            this.Tlib.Size = new System.Drawing.Size(290, 25);
            this.Tlib.TabIndex = 1;
            this.Tlib.DoubleClick += new System.EventHandler(this.Tlib_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(76, 248);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(65, 34);
            this.button5.TabIndex = 1;
            this.button5.Text = "delete";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Tdb
            // 
            this.Tdb.Location = new System.Drawing.Point(472, 49);
            this.Tdb.Name = "Tdb";
            this.Tdb.Size = new System.Drawing.Size(289, 25);
            this.Tdb.TabIndex = 1;
            this.Tdb.DoubleClick += new System.EventHandler(this.Tdb_Click);
            // 
            // Tpy
            // 
            this.Tpy.Location = new System.Drawing.Point(75, 80);
            this.Tpy.Name = "Tpy";
            this.Tpy.Size = new System.Drawing.Size(290, 25);
            this.Tpy.TabIndex = 1;
            this.Tpy.DoubleClick += new System.EventHandler(this.Tpy_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(5, 248);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(65, 34);
            this.button6.TabIndex = 0;
            this.button6.Text = "clear";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(371, 83);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(95, 15);
            this.label21.TabIndex = 0;
            this.label21.Text = "SwathWindow";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(147, 248);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(65, 34);
            this.button3.TabIndex = 0;
            this.button3.Text = "add";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(371, 21);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(55, 15);
            this.label20.TabIndex = 0;
            this.label20.Text = "UniMod";
            // 
            // Tirt
            // 
            this.Tirt.Location = new System.Drawing.Point(75, 18);
            this.Tirt.Name = "Tirt";
            this.Tirt.Size = new System.Drawing.Size(290, 25);
            this.Tirt.TabIndex = 1;
            this.Tirt.DoubleClick += new System.EventHandler(this.Tirt_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Library";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(371, 52);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(71, 15);
            this.label19.TabIndex = 0;
            this.label19.Text = "Database";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "mzXML/mzML file";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Python";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "iRT";
            // 
            // runner
            // 
            this.runner.WorkerReportsProgress = true;
            this.runner.DoWork += new System.ComponentModel.DoWorkEventHandler(this.runner_DoWork);
            this.runner.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.runner_ProgressChanged);
            this.runner.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.runner_RunWorkerCompleted);
            // 
            // pro
            // 
            this.pro.EnableRaisingEvents = true;
            this.pro.StartInfo.CreateNoWindow = true;
            this.pro.StartInfo.Domain = "";
            this.pro.StartInfo.LoadUserProfile = false;
            this.pro.StartInfo.Password = null;
            this.pro.StartInfo.RedirectStandardError = true;
            this.pro.StartInfo.RedirectStandardOutput = true;
            this.pro.StartInfo.StandardErrorEncoding = null;
            this.pro.StartInfo.StandardOutputEncoding = null;
            this.pro.StartInfo.UserName = "";
            this.pro.StartInfo.UseShellExecute = false;
            this.pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            this.pro.SynchronizingObject = this;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(812, 450);
            this.Controls.Add(this.flowLayoutPanelRun);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(830, 497);
            this.MinimumSize = new System.Drawing.Size(830, 497);
            this.Name = "MainForm";
            this.Text = "QuantPipe v1.0";
            this.flowLayoutPanelRun.ResumeLayout(false);
            this.flowLayoutPanelRun.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelRun;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Tcomment;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ListBox Lin;
        private System.Windows.Forms.TextBox Twin;
        private System.Windows.Forms.TextBox Tmod;
        private System.Windows.Forms.TextBox Tlib;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox Tdb;
        private System.Windows.Forms.TextBox Tpy;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox Tirt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog fopen;
        private System.Windows.Forms.SaveFileDialog fsave;
        private System.ComponentModel.BackgroundWorker runner;
        private System.Diagnostics.Process pro;
        private System.Windows.Forms.CheckBox Cipf;
        private System.Windows.Forms.CheckBox Cim;
        private System.Windows.Forms.NumericUpDown Nim;
    }
}