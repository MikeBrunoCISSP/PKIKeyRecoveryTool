
namespace PKIKeyRecovery
{
    partial class Config
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDestDir = new System.Windows.Forms.TextBox();
            this.btnDestBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.trkPwdLength = new System.Windows.Forms.TrackBar();
            this.lblPwdLength = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSmtpPort = new System.Windows.Forms.TextBox();
            this.txtSmtpServer = new System.Windows.Forms.TextBox();
            this.txtSmtpPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSmtpUser = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbtnDeleteNo = new System.Windows.Forms.RadioButton();
            this.rbtnDeleteYes = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbtnAttachNo = new System.Windows.Forms.RadioButton();
            this.rbtnAttachYes = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbtnEmailNo = new System.Windows.Forms.RadioButton();
            this.rbtnEmailYes = new System.Windows.Forms.RadioButton();
            this.lblInvalidServer = new System.Windows.Forms.Label();
            this.lblInvalidSender = new System.Windows.Forms.Label();
            this.txtSenderEmail = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblInvalidDiscovery = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtDiscEmail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnDiscBrowse = new System.Windows.Forms.Button();
            this.txtDiscDir = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkAlpha = new System.Windows.Forms.CheckBox();
            this.chkDigits = new System.Windows.Forms.CheckBox();
            this.chkSymbols = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trkPwdLength)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Destination Directory:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDestDir
            // 
            this.txtDestDir.Enabled = false;
            this.txtDestDir.Location = new System.Drawing.Point(250, 12);
            this.txtDestDir.Name = "txtDestDir";
            this.txtDestDir.Size = new System.Drawing.Size(537, 24);
            this.txtDestDir.TabIndex = 1;
            this.txtDestDir.TextChanged += new System.EventHandler(this.txtDestDir_TextChanged);
            // 
            // btnDestBrowse
            // 
            this.btnDestBrowse.Location = new System.Drawing.Point(793, 12);
            this.btnDestBrowse.Name = "btnDestBrowse";
            this.btnDestBrowse.Size = new System.Drawing.Size(108, 34);
            this.btnDestBrowse.TabIndex = 0;
            this.btnDestBrowse.Text = "Browse";
            this.btnDestBrowse.UseVisualStyleBackColor = true;
            this.btnDestBrowse.Click += new System.EventHandler(this.btnDestBrowse_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password Length:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trkPwdLength
            // 
            this.trkPwdLength.Location = new System.Drawing.Point(250, 60);
            this.trkPwdLength.Maximum = 32;
            this.trkPwdLength.Minimum = 8;
            this.trkPwdLength.Name = "trkPwdLength";
            this.trkPwdLength.Size = new System.Drawing.Size(537, 45);
            this.trkPwdLength.TabIndex = 99;
            this.trkPwdLength.Value = 8;
            this.trkPwdLength.Scroll += new System.EventHandler(this.trkPwdLength_Scroll);
            // 
            // lblPwdLength
            // 
            this.lblPwdLength.AutoSize = true;
            this.lblPwdLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPwdLength.Location = new System.Drawing.Point(824, 60);
            this.lblPwdLength.Name = "lblPwdLength";
            this.lblPwdLength.Size = new System.Drawing.Size(0, 24);
            this.lblPwdLength.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(220, 26);
            this.label3.TabIndex = 6;
            this.label3.Text = "Use Email:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(220, 26);
            this.label4.TabIndex = 9;
            this.label4.Text = "SMTP Server:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 270);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(220, 26);
            this.label5.TabIndex = 10;
            this.label5.Text = "SMTP Port:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSmtpPort
            // 
            this.txtSmtpPort.Enabled = false;
            this.txtSmtpPort.Location = new System.Drawing.Point(239, 270);
            this.txtSmtpPort.Name = "txtSmtpPort";
            this.txtSmtpPort.Size = new System.Drawing.Size(100, 24);
            this.txtSmtpPort.TabIndex = 5;
            this.txtSmtpPort.Text = "25";
            this.txtSmtpPort.TextChanged += new System.EventHandler(this.txtSmtpPort_TextChanged);
            this.txtSmtpPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSmtpPort_KeyPress);
            // 
            // txtSmtpServer
            // 
            this.txtSmtpServer.Enabled = false;
            this.txtSmtpServer.Location = new System.Drawing.Point(240, 120);
            this.txtSmtpServer.Name = "txtSmtpServer";
            this.txtSmtpServer.Size = new System.Drawing.Size(396, 24);
            this.txtSmtpServer.TabIndex = 3;
            this.txtSmtpServer.TextChanged += new System.EventHandler(this.txtSmtpServer_TextChanged);
            // 
            // txtSmtpPassword
            // 
            this.txtSmtpPassword.Enabled = false;
            this.txtSmtpPassword.Location = new System.Drawing.Point(239, 220);
            this.txtSmtpPassword.Name = "txtSmtpPassword";
            this.txtSmtpPassword.PasswordChar = '*';
            this.txtSmtpPassword.Size = new System.Drawing.Size(396, 24);
            this.txtSmtpPassword.TabIndex = 4;
            this.txtSmtpPassword.TextChanged += new System.EventHandler(this.txtSmtpPassword_TextChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(220, 26);
            this.label6.TabIndex = 13;
            this.label6.Text = "SMTP Password:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(10, 320);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(220, 26);
            this.label7.TabIndex = 15;
            this.label7.Text = "Attach PFX File(s):";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSmtpUser);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.lblInvalidServer);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblInvalidSender);
            this.groupBox1.Controls.Add(this.txtSmtpPassword);
            this.groupBox1.Controls.Add(this.txtSenderEmail);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtSmtpServer);
            this.groupBox1.Controls.Add(this.txtSmtpPort);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(10, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(922, 418);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Email";
            // 
            // txtSmtpUser
            // 
            this.txtSmtpUser.Enabled = false;
            this.txtSmtpUser.Location = new System.Drawing.Point(240, 170);
            this.txtSmtpUser.Name = "txtSmtpUser";
            this.txtSmtpUser.Size = new System.Drawing.Size(396, 24);
            this.txtSmtpUser.TabIndex = 103;
            this.txtSmtpUser.TextChanged += new System.EventHandler(this.txtSmtpUser_TextChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(10, 170);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(220, 26);
            this.label12.TabIndex = 104;
            this.label12.Text = "SMTP Username:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbtnDeleteNo);
            this.panel3.Controls.Add(this.rbtnDeleteYes);
            this.panel3.Location = new System.Drawing.Point(240, 360);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(197, 40);
            this.panel3.TabIndex = 102;
            // 
            // rbtnDeleteNo
            // 
            this.rbtnDeleteNo.AutoSize = true;
            this.rbtnDeleteNo.Checked = true;
            this.rbtnDeleteNo.Location = new System.Drawing.Point(109, 3);
            this.rbtnDeleteNo.Name = "rbtnDeleteNo";
            this.rbtnDeleteNo.Size = new System.Drawing.Size(46, 22);
            this.rbtnDeleteNo.TabIndex = 20;
            this.rbtnDeleteNo.TabStop = true;
            this.rbtnDeleteNo.Text = "No";
            this.rbtnDeleteNo.UseVisualStyleBackColor = true;
            // 
            // rbtnDeleteYes
            // 
            this.rbtnDeleteYes.AutoSize = true;
            this.rbtnDeleteYes.Location = new System.Drawing.Point(10, 3);
            this.rbtnDeleteYes.Name = "rbtnDeleteYes";
            this.rbtnDeleteYes.Size = new System.Drawing.Size(51, 22);
            this.rbtnDeleteYes.TabIndex = 19;
            this.rbtnDeleteYes.Text = "Yes";
            this.rbtnDeleteYes.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(-11, 360);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(241, 26);
            this.label11.TabIndex = 101;
            this.label11.Text = "Delete After Sending:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbtnAttachNo);
            this.panel2.Controls.Add(this.rbtnAttachYes);
            this.panel2.Location = new System.Drawing.Point(240, 320);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(197, 40);
            this.panel2.TabIndex = 27;
            // 
            // rbtnAttachNo
            // 
            this.rbtnAttachNo.AutoSize = true;
            this.rbtnAttachNo.Checked = true;
            this.rbtnAttachNo.Enabled = false;
            this.rbtnAttachNo.Location = new System.Drawing.Point(108, 7);
            this.rbtnAttachNo.Name = "rbtnAttachNo";
            this.rbtnAttachNo.Size = new System.Drawing.Size(46, 22);
            this.rbtnAttachNo.TabIndex = 20;
            this.rbtnAttachNo.TabStop = true;
            this.rbtnAttachNo.Text = "No";
            this.rbtnAttachNo.UseVisualStyleBackColor = true;
            // 
            // rbtnAttachYes
            // 
            this.rbtnAttachYes.AutoSize = true;
            this.rbtnAttachYes.Enabled = false;
            this.rbtnAttachYes.Location = new System.Drawing.Point(9, 7);
            this.rbtnAttachYes.Name = "rbtnAttachYes";
            this.rbtnAttachYes.Size = new System.Drawing.Size(51, 22);
            this.rbtnAttachYes.TabIndex = 19;
            this.rbtnAttachYes.Text = "Yes";
            this.rbtnAttachYes.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbtnEmailNo);
            this.panel1.Controls.Add(this.rbtnEmailYes);
            this.panel1.Location = new System.Drawing.Point(240, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(197, 40);
            this.panel1.TabIndex = 26;
            // 
            // rbtnEmailNo
            // 
            this.rbtnEmailNo.AutoSize = true;
            this.rbtnEmailNo.Checked = true;
            this.rbtnEmailNo.Location = new System.Drawing.Point(109, 3);
            this.rbtnEmailNo.Name = "rbtnEmailNo";
            this.rbtnEmailNo.Size = new System.Drawing.Size(46, 22);
            this.rbtnEmailNo.TabIndex = 20;
            this.rbtnEmailNo.TabStop = true;
            this.rbtnEmailNo.Text = "No";
            this.rbtnEmailNo.UseVisualStyleBackColor = true;
            this.rbtnEmailNo.CheckedChanged += new System.EventHandler(this.rbtnEmailNo_CheckedChanged);
            // 
            // rbtnEmailYes
            // 
            this.rbtnEmailYes.AutoSize = true;
            this.rbtnEmailYes.Location = new System.Drawing.Point(10, 3);
            this.rbtnEmailYes.Name = "rbtnEmailYes";
            this.rbtnEmailYes.Size = new System.Drawing.Size(51, 22);
            this.rbtnEmailYes.TabIndex = 19;
            this.rbtnEmailYes.Text = "Yes";
            this.rbtnEmailYes.UseVisualStyleBackColor = true;
            this.rbtnEmailYes.CheckedChanged += new System.EventHandler(this.rbtnEmailYes_CheckedChanged);
            // 
            // lblInvalidServer
            // 
            this.lblInvalidServer.ForeColor = System.Drawing.Color.Red;
            this.lblInvalidServer.Location = new System.Drawing.Point(661, 89);
            this.lblInvalidServer.Name = "lblInvalidServer";
            this.lblInvalidServer.Size = new System.Drawing.Size(220, 26);
            this.lblInvalidServer.TabIndex = 25;
            this.lblInvalidServer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInvalidSender
            // 
            this.lblInvalidSender.ForeColor = System.Drawing.Color.Red;
            this.lblInvalidSender.Location = new System.Drawing.Point(642, 337);
            this.lblInvalidSender.Name = "lblInvalidSender";
            this.lblInvalidSender.Size = new System.Drawing.Size(220, 26);
            this.lblInvalidSender.TabIndex = 24;
            this.lblInvalidSender.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSenderEmail
            // 
            this.txtSenderEmail.Enabled = false;
            this.txtSenderEmail.Location = new System.Drawing.Point(240, 75);
            this.txtSenderEmail.Name = "txtSenderEmail";
            this.txtSenderEmail.Size = new System.Drawing.Size(396, 24);
            this.txtSenderEmail.TabIndex = 7;
            this.txtSenderEmail.TextChanged += new System.EventHandler(this.txtSenderEmail_TextChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(10, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(220, 26);
            this.label10.TabIndex = 22;
            this.label10.Text = "Sender Address:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInvalidDiscovery
            // 
            this.lblInvalidDiscovery.ForeColor = System.Drawing.Color.Red;
            this.lblInvalidDiscovery.Location = new System.Drawing.Point(661, 95);
            this.lblInvalidDiscovery.Name = "lblInvalidDiscovery";
            this.lblInvalidDiscovery.Size = new System.Drawing.Size(220, 26);
            this.lblInvalidDiscovery.TabIndex = 21;
            this.lblInvalidDiscovery.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnApply
            // 
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(824, 721);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(108, 34);
            this.btnApply.TabIndex = 11;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(710, 721);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 34);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtDiscEmail);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.btnDiscBrowse);
            this.groupBox2.Controls.Add(this.txtDiscDir);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.lblInvalidDiscovery);
            this.groupBox2.Location = new System.Drawing.Point(10, 570);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(922, 126);
            this.groupBox2.TabIndex = 100;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "eDiscovery";
            // 
            // txtDiscEmail
            // 
            this.txtDiscEmail.Enabled = false;
            this.txtDiscEmail.Location = new System.Drawing.Point(250, 80);
            this.txtDiscEmail.Name = "txtDiscEmail";
            this.txtDiscEmail.Size = new System.Drawing.Size(396, 24);
            this.txtDiscEmail.TabIndex = 102;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(10, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(220, 26);
            this.label8.TabIndex = 103;
            this.label8.Text = "eDiscovery Email:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDiscBrowse
            // 
            this.btnDiscBrowse.Location = new System.Drawing.Point(793, 35);
            this.btnDiscBrowse.Name = "btnDiscBrowse";
            this.btnDiscBrowse.Size = new System.Drawing.Size(108, 34);
            this.btnDiscBrowse.TabIndex = 101;
            this.btnDiscBrowse.Text = "Browse";
            this.btnDiscBrowse.UseVisualStyleBackColor = true;
            this.btnDiscBrowse.Click += new System.EventHandler(this.btnDiscBrowse_Click);
            // 
            // txtDiscDir
            // 
            this.txtDiscDir.Enabled = false;
            this.txtDiscDir.Location = new System.Drawing.Point(250, 35);
            this.txtDiscDir.Name = "txtDiscDir";
            this.txtDiscDir.Size = new System.Drawing.Size(537, 24);
            this.txtDiscDir.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(10, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(220, 26);
            this.label9.TabIndex = 11;
            this.label9.Text = "Recovery Directory:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkAlpha
            // 
            this.chkAlpha.AutoSize = true;
            this.chkAlpha.Checked = true;
            this.chkAlpha.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlpha.Location = new System.Drawing.Point(250, 100);
            this.chkAlpha.Name = "chkAlpha";
            this.chkAlpha.Size = new System.Drawing.Size(169, 22);
            this.chkAlpha.TabIndex = 102;
            this.chkAlpha.Text = "Include Alpha [a-zA-Z]";
            this.chkAlpha.UseVisualStyleBackColor = true;
            this.chkAlpha.CheckedChanged += new System.EventHandler(this.chkAlpha_CheckedChanged);
            // 
            // chkDigits
            // 
            this.chkDigits.AutoSize = true;
            this.chkDigits.Checked = true;
            this.chkDigits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDigits.Location = new System.Drawing.Point(430, 100);
            this.chkDigits.Name = "chkDigits";
            this.chkDigits.Size = new System.Drawing.Size(147, 22);
            this.chkDigits.TabIndex = 103;
            this.chkDigits.Text = "Include Digits [0-9]";
            this.chkDigits.UseVisualStyleBackColor = true;
            this.chkDigits.CheckedChanged += new System.EventHandler(this.chkDigits_CheckedChanged);
            // 
            // chkSymbols
            // 
            this.chkSymbols.AutoSize = true;
            this.chkSymbols.Checked = true;
            this.chkSymbols.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSymbols.Location = new System.Drawing.Point(595, 100);
            this.chkSymbols.Name = "chkSymbols";
            this.chkSymbols.Size = new System.Drawing.Size(195, 22);
            this.chkSymbols.TabIndex = 104;
            this.chkSymbols.Text = "Include Symbols (!@#$...)";
            this.chkSymbols.UseVisualStyleBackColor = true;
            this.chkSymbols.CheckedChanged += new System.EventHandler(this.chkSymbols_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(596, 721);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 34);
            this.btnOK.TabIndex = 105;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Config
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(942, 767);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkSymbols);
            this.Controls.Add(this.chkDigits);
            this.Controls.Add(this.chkAlpha);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lblPwdLength);
            this.Controls.Add(this.trkPwdLength);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDestBrowse);
            this.Controls.Add(this.txtDestDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Config";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "KRTool - Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Config_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trkPwdLength)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDestDir;
        private System.Windows.Forms.Button btnDestBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trkPwdLength;
        private System.Windows.Forms.Label lblPwdLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSmtpPort;
        private System.Windows.Forms.TextBox txtSmtpServer;
        private System.Windows.Forms.TextBox txtSmtpPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblInvalidDiscovery;
        private System.Windows.Forms.Label lblInvalidSender;
        private System.Windows.Forms.TextBox txtSenderEmail;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblInvalidServer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbtnAttachNo;
        private System.Windows.Forms.RadioButton rbtnAttachYes;
        private System.Windows.Forms.RadioButton rbtnEmailNo;
        private System.Windows.Forms.RadioButton rbtnEmailYes;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDiscBrowse;
        private System.Windows.Forms.TextBox txtDiscDir;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDiscEmail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rbtnDeleteNo;
        private System.Windows.Forms.RadioButton rbtnDeleteYes;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSmtpUser;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkAlpha;
        private System.Windows.Forms.CheckBox chkDigits;
        private System.Windows.Forms.CheckBox chkSymbols;
        private System.Windows.Forms.Button btnOK;
    }
}