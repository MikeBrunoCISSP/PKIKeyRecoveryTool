using System;

namespace PKIKeyRecovery
{
    partial class SetUp
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
            this.cboCA = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.txtScope = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSMTPServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkSMTPAuthRequired = new System.Windows.Forms.CheckBox();
            this.txtSMTPUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSMTPPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSMTPConfirmPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblMustMatch = new System.Windows.Forms.Label();
            this.txtSenderEmail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtLegalEmail = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtuserBody = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnUserBrowse = new System.Windows.Forms.Button();
            this.btnLegalBrowse = new System.Windows.Forms.Button();
            this.txtLegalBody = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblSenderEmailValid = new System.Windows.Forms.Label();
            this.lblLegalEmailValid = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Certification Authority:";
            // 
            // cboCA
            // 
            this.cboCA.FormattingEnabled = true;
            this.cboCA.Location = new System.Drawing.Point(151, 93);
            this.cboCA.Name = "cboCA";
            this.cboCA.Size = new System.Drawing.Size(321, 21);
            this.cboCA.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Active Directory Domain:";
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(151, 128);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(186, 20);
            this.txtDomain.TabIndex = 3;
            this.txtDomain.TextChanged += new System.EventHandler(this.txtDomain_TextChanged);
            // 
            // txtScope
            // 
            this.txtScope.Location = new System.Drawing.Point(151, 163);
            this.txtScope.Name = "txtScope";
            this.txtScope.Size = new System.Drawing.Size(186, 20);
            this.txtScope.TabIndex = 5;
            this.txtScope.TextChanged += new System.EventHandler(this.txtScope_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Active Directory Scope:";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(10, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 130);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Certificate and Directory Server Settings";
            // 
            // txtSMTPServer
            // 
            this.txtSMTPServer.Location = new System.Drawing.Point(151, 222);
            this.txtSMTPServer.Name = "txtSMTPServer";
            this.txtSMTPServer.Size = new System.Drawing.Size(186, 20);
            this.txtSMTPServer.TabIndex = 8;
            this.txtSMTPServer.TextChanged += new System.EventHandler(this.txtSMTPServer_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "SMTP Server:";
            // 
            // chkSMTPAuthRequired
            // 
            this.chkSMTPAuthRequired.AutoSize = true;
            this.chkSMTPAuthRequired.Location = new System.Drawing.Point(351, 224);
            this.chkSMTPAuthRequired.Name = "chkSMTPAuthRequired";
            this.chkSMTPAuthRequired.Size = new System.Drawing.Size(140, 17);
            this.chkSMTPAuthRequired.TabIndex = 10;
            this.chkSMTPAuthRequired.Text = "Authentication Required";
            this.chkSMTPAuthRequired.UseVisualStyleBackColor = true;
            this.chkSMTPAuthRequired.CheckedChanged += new System.EventHandler(this.chkSMTPAuthRequired_CheckedChanged);
            // 
            // txtSMTPUser
            // 
            this.txtSMTPUser.Enabled = false;
            this.txtSMTPUser.Location = new System.Drawing.Point(154, 15);
            this.txtSMTPUser.Name = "txtSMTPUser";
            this.txtSMTPUser.Size = new System.Drawing.Size(186, 20);
            this.txtSMTPUser.TabIndex = 12;
            this.txtSMTPUser.TextChanged += new System.EventHandler(this.txtSMTPUser_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Username:";
            // 
            // txtSMTPPassword
            // 
            this.txtSMTPPassword.Enabled = false;
            this.txtSMTPPassword.Location = new System.Drawing.Point(154, 41);
            this.txtSMTPPassword.Name = "txtSMTPPassword";
            this.txtSMTPPassword.Size = new System.Drawing.Size(186, 20);
            this.txtSMTPPassword.TabIndex = 14;
            this.txtSMTPPassword.TextChanged += new System.EventHandler(this.txtSMTPPassword_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Password:";
            // 
            // txtSMTPConfirmPassword
            // 
            this.txtSMTPConfirmPassword.Enabled = false;
            this.txtSMTPConfirmPassword.Location = new System.Drawing.Point(154, 67);
            this.txtSMTPConfirmPassword.Name = "txtSMTPConfirmPassword";
            this.txtSMTPConfirmPassword.Size = new System.Drawing.Size(186, 20);
            this.txtSMTPConfirmPassword.TabIndex = 16;
            this.txtSMTPConfirmPassword.TextChanged += new System.EventHandler(this.txtSMTPConfirmPassword_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Confirm Password:";
            // 
            // lblMustMatch
            // 
            this.lblMustMatch.AutoSize = true;
            this.lblMustMatch.ForeColor = System.Drawing.Color.Red;
            this.lblMustMatch.Location = new System.Drawing.Point(158, 90);
            this.lblMustMatch.Name = "lblMustMatch";
            this.lblMustMatch.Size = new System.Drawing.Size(0, 13);
            this.lblMustMatch.TabIndex = 17;
            // 
            // txtSenderEmail
            // 
            this.txtSenderEmail.Location = new System.Drawing.Point(151, 368);
            this.txtSenderEmail.Name = "txtSenderEmail";
            this.txtSenderEmail.Size = new System.Drawing.Size(186, 20);
            this.txtSenderEmail.TabIndex = 19;
            this.txtSenderEmail.TextChanged += new System.EventHandler(this.txtSenderEmail_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 371);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Sender Email:";
            // 
            // txtLegalEmail
            // 
            this.txtLegalEmail.Location = new System.Drawing.Point(151, 394);
            this.txtLegalEmail.Name = "txtLegalEmail";
            this.txtLegalEmail.Size = new System.Drawing.Size(186, 20);
            this.txtLegalEmail.TabIndex = 21;
            this.txtLegalEmail.TextChanged += new System.EventHandler(this.txtLegalEmail_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 397);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Legal Discovery Email:";
            // 
            // txtuserBody
            // 
            this.txtuserBody.Location = new System.Drawing.Point(151, 420);
            this.txtuserBody.Name = "txtuserBody";
            this.txtuserBody.Size = new System.Drawing.Size(186, 20);
            this.txtuserBody.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 423);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "User Message Body:";
            // 
            // btnUserBrowse
            // 
            this.btnUserBrowse.Location = new System.Drawing.Point(364, 416);
            this.btnUserBrowse.Name = "btnUserBrowse";
            this.btnUserBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnUserBrowse.TabIndex = 24;
            this.btnUserBrowse.Text = "Browse";
            this.btnUserBrowse.UseVisualStyleBackColor = true;
            this.btnUserBrowse.Click += new System.EventHandler(this.btnUserBrowse_Click);
            // 
            // btnLegalBrowse
            // 
            this.btnLegalBrowse.Location = new System.Drawing.Point(364, 453);
            this.btnLegalBrowse.Name = "btnLegalBrowse";
            this.btnLegalBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnLegalBrowse.TabIndex = 27;
            this.btnLegalBrowse.Text = "Browse";
            this.btnLegalBrowse.UseVisualStyleBackColor = true;
            this.btnLegalBrowse.Click += new System.EventHandler(this.btnLegalBrowse_Click);
            // 
            // txtLegalBody
            // 
            this.txtLegalBody.Location = new System.Drawing.Point(151, 455);
            this.txtLegalBody.Name = "txtLegalBody";
            this.txtLegalBody.Size = new System.Drawing.Size(186, 20);
            this.txtLegalBody.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 449);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 26);
            this.label11.TabIndex = 25;
            this.label11.Text = "Legal Discovery\r\nMessage Body:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblMustMatch);
            this.groupBox2.Controls.Add(this.txtSMTPConfirmPassword);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtSMTPPassword);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtSMTPUser);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(132, 248);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 110);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SMTP Authentication";
            // 
            // lblSenderEmailValid
            // 
            this.lblSenderEmailValid.AutoSize = true;
            this.lblSenderEmailValid.ForeColor = System.Drawing.Color.Red;
            this.lblSenderEmailValid.Location = new System.Drawing.Point(344, 370);
            this.lblSenderEmailValid.Name = "lblSenderEmailValid";
            this.lblSenderEmailValid.Size = new System.Drawing.Size(0, 13);
            this.lblSenderEmailValid.TabIndex = 29;
            // 
            // lblLegalEmailValid
            // 
            this.lblLegalEmailValid.AutoSize = true;
            this.lblLegalEmailValid.ForeColor = System.Drawing.Color.Red;
            this.lblLegalEmailValid.Location = new System.Drawing.Point(344, 397);
            this.lblLegalEmailValid.Name = "lblLegalEmailValid";
            this.lblLegalEmailValid.Size = new System.Drawing.Size(0, 13);
            this.lblLegalEmailValid.TabIndex = 30;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(421, 490);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 31;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // SetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 525);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblLegalEmailValid);
            this.Controls.Add(this.lblSenderEmailValid);
            this.Controls.Add(this.btnLegalBrowse);
            this.Controls.Add(this.txtLegalBody);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnUserBrowse);
            this.Controls.Add(this.txtuserBody);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtLegalEmail);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtSenderEmail);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkSMTPAuthRequired);
            this.Controls.Add(this.txtSMTPServer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtScope);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboCA);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Enabled = false;
            this.Name = "SetUp";
            this.Text = "KRTool Setup";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.TextBox txtScope;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSMTPServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkSMTPAuthRequired;
        private System.Windows.Forms.TextBox txtSMTPUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSMTPPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSMTPConfirmPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblMustMatch;
        private System.Windows.Forms.TextBox txtSenderEmail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtLegalEmail;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtuserBody;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnUserBrowse;
        private System.Windows.Forms.Button btnLegalBrowse;
        private System.Windows.Forms.TextBox txtLegalBody;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblSenderEmailValid;
        private System.Windows.Forms.Label lblLegalEmailValid;
        private System.Windows.Forms.Button btnOK;
    }
}