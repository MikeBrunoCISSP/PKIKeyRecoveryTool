namespace PKIKeyRecovery
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
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCN = new System.Windows.Forms.Label();
            this.rbtnUser = new System.Windows.Forms.RadioButton();
            this.rbtnEDiscovery = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRecoverKeys = new System.Windows.Forms.Button();
            this.btnRecoverFromList = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnAbout = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(67, 19);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(107, 20);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(232, 19);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(126, 24);
            this.btnValidate.TabIndex = 2;
            this.btnValidate.Text = "Confirm Username";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCN);
            this.panel1.Location = new System.Drawing.Point(232, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(165, 27);
            this.panel1.TabIndex = 3;
            // 
            // lblCN
            // 
            this.lblCN.AutoSize = true;
            this.lblCN.Location = new System.Drawing.Point(6, 6);
            this.lblCN.Name = "lblCN";
            this.lblCN.Size = new System.Drawing.Size(0, 13);
            this.lblCN.TabIndex = 0;
            // 
            // rbtnUser
            // 
            this.rbtnUser.AutoSize = true;
            this.rbtnUser.Location = new System.Drawing.Point(10, 38);
            this.rbtnUser.Name = "rbtnUser";
            this.rbtnUser.Size = new System.Drawing.Size(185, 17);
            this.rbtnUser.TabIndex = 4;
            this.rbtnUser.TabStop = true;
            this.rbtnUser.Text = "Send Recovery Password to User";
            this.rbtnUser.UseVisualStyleBackColor = true;
            // 
            // rbtnEDiscovery
            // 
            this.rbtnEDiscovery.AutoSize = true;
            this.rbtnEDiscovery.Location = new System.Drawing.Point(10, 61);
            this.rbtnEDiscovery.Name = "rbtnEDiscovery";
            this.rbtnEDiscovery.Size = new System.Drawing.Size(216, 17);
            this.rbtnEDiscovery.TabIndex = 5;
            this.rbtnEDiscovery.TabStop = true;
            this.rbtnEDiscovery.Text = "Send Recovery Password to eDiscovery";
            this.rbtnEDiscovery.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRecoverKeys);
            this.groupBox1.Controls.Add(this.rbtnEDiscovery);
            this.groupBox1.Controls.Add(this.rbtnUser);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.btnValidate);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(11, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(407, 125);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recover Keys for a Single User";
            // 
            // btnRecoverKeys
            // 
            this.btnRecoverKeys.Location = new System.Drawing.Point(12, 89);
            this.btnRecoverKeys.Name = "btnRecoverKeys";
            this.btnRecoverKeys.Size = new System.Drawing.Size(203, 26);
            this.btnRecoverKeys.TabIndex = 6;
            this.btnRecoverKeys.Text = "Recover Keys for Single User ";
            this.btnRecoverKeys.UseVisualStyleBackColor = true;
            this.btnRecoverKeys.Click += new System.EventHandler(this.btnRecoverKeys_Click);
            // 
            // btnRecoverFromList
            // 
            this.btnRecoverFromList.Location = new System.Drawing.Point(23, 150);
            this.btnRecoverFromList.Name = "btnRecoverFromList";
            this.btnRecoverFromList.Size = new System.Drawing.Size(203, 31);
            this.btnRecoverFromList.TabIndex = 7;
            this.btnRecoverFromList.Text = "Recover Keys for a List of Users";
            this.btnRecoverFromList.UseVisualStyleBackColor = true;
            this.btnRecoverFromList.Click += new System.EventHandler(this.btnRecoverFromList_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblStatus);
            this.groupBox2.Location = new System.Drawing.Point(12, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(410, 68);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Messages";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(11, 21);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 0;
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(346, 266);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(75, 23);
            this.btnAbout.TabIndex = 9;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 291);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnRecoverFromList);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "KRTool";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbtnUser;
        private System.Windows.Forms.RadioButton rbtnEDiscovery;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRecoverKeys;
        private System.Windows.Forms.Button btnRecoverFromList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCN;
        private System.Windows.Forms.Button btnAbout;
    }
}

