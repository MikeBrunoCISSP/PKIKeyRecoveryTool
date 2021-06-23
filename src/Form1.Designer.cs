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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnRecoverFromList = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rbtnUser = new System.Windows.Forms.RadioButton();
            this.rbtnEDiscovery = new System.Windows.Forms.RadioButton();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnRecoverKeys = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCN = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblMessages = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboCA = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTemplate = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRecoverFromList
            // 
            this.btnRecoverFromList.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecoverFromList.Location = new System.Drawing.Point(260, 310);
            this.btnRecoverFromList.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnRecoverFromList.Name = "btnRecoverFromList";
            this.btnRecoverFromList.Size = new System.Drawing.Size(233, 88);
            this.btnRecoverFromList.TabIndex = 7;
            this.btnRecoverFromList.Text = "Bulk Recovery";
            this.toolTip1.SetToolTip(this.btnRecoverFromList, "Recover keys for a group of individuals for legal discovery purposes.  ");
            this.btnRecoverFromList.UseVisualStyleBackColor = true;
            // 
            // rbtnUser
            // 
            this.rbtnUser.AutoSize = true;
            this.rbtnUser.Checked = true;
            this.rbtnUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnUser.Location = new System.Drawing.Point(9, 41);
            this.rbtnUser.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.rbtnUser.Name = "rbtnUser";
            this.rbtnUser.Size = new System.Drawing.Size(83, 30);
            this.rbtnUser.TabIndex = 4;
            this.rbtnUser.TabStop = true;
            this.rbtnUser.Text = "User";
            this.toolTip1.SetToolTip(this.rbtnUser, "Recover all archived certificates and keys on behalf of an individual for install" +
        "ation on their laptop or desktop");
            this.rbtnUser.UseVisualStyleBackColor = true;
            // 
            // rbtnEDiscovery
            // 
            this.rbtnEDiscovery.AutoSize = true;
            this.rbtnEDiscovery.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnEDiscovery.Location = new System.Drawing.Point(9, 91);
            this.rbtnEDiscovery.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.rbtnEDiscovery.Name = "rbtnEDiscovery";
            this.rbtnEDiscovery.Size = new System.Drawing.Size(192, 30);
            this.rbtnEDiscovery.TabIndex = 5;
            this.rbtnEDiscovery.Text = "Legal Discovery";
            this.toolTip1.SetToolTip(this.rbtnEDiscovery, "Recover all archived certificates and keys for an individual on behalf of legal d" +
        "iscovery");
            this.rbtnEDiscovery.UseVisualStyleBackColor = true;
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(171, 56);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(281, 32);
            this.txtUserName.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtUserName, "Enter the Active Directory SAMAccountName of the individual whose keys are to be " +
        "recovered");
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // btnValidate
            // 
            this.btnValidate.Enabled = false;
            this.btnValidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidate.Location = new System.Drawing.Point(483, 56);
            this.btnValidate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(233, 88);
            this.btnValidate.TabIndex = 2;
            this.btnValidate.Text = "Confirm Username";
            this.toolTip1.SetToolTip(this.btnValidate, "Check whether the entered username exists in the Active Directory");
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnRecoverKeys
            // 
            this.btnRecoverKeys.Enabled = false;
            this.btnRecoverKeys.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecoverKeys.Location = new System.Drawing.Point(5, 310);
            this.btnRecoverKeys.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnRecoverKeys.Name = "btnRecoverKeys";
            this.btnRecoverKeys.Size = new System.Drawing.Size(233, 88);
            this.btnRecoverKeys.TabIndex = 6;
            this.btnRecoverKeys.Text = "Individual Recovery";
            this.toolTip1.SetToolTip(this.btnRecoverKeys, "Recover keys for an individual");
            this.btnRecoverKeys.UseVisualStyleBackColor = true;
            this.btnRecoverKeys.Click += new System.EventHandler(this.btnRecoverKeys_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCN);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(483, 156);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(824, 78);
            this.panel1.TabIndex = 3;
            // 
            // lblCN
            // 
            this.lblCN.AutoSize = true;
            this.lblCN.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCN.Location = new System.Drawing.Point(12, 19);
            this.lblCN.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCN.Name = "lblCN";
            this.lblCN.Size = new System.Drawing.Size(0, 26);
            this.lblCN.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnRecoverKeys);
            this.groupBox1.Controls.Add(this.btnRecoverFromList);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.btnValidate);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(19, 205);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupBox1.Size = new System.Drawing.Size(1317, 426);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Key Recovery";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtnUser);
            this.groupBox2.Controls.Add(this.rbtnEDiscovery);
            this.groupBox2.Location = new System.Drawing.Point(17, 134);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(436, 148);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recovery Type";
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(32, 331);
            this.lblMessages.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 25);
            this.lblMessages.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(225, 26);
            this.label2.TabIndex = 11;
            this.label2.Text = "Certification Authority:";
            // 
            // cboCA
            // 
            this.cboCA.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCA.FormattingEnabled = true;
            this.cboCA.Location = new System.Drawing.Point(325, 52);
            this.cboCA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboCA.Name = "cboCA";
            this.cboCA.Size = new System.Drawing.Size(1009, 34);
            this.cboCA.TabIndex = 12;
            this.cboCA.SelectedIndexChanged += new System.EventHandler(this.cboCA_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 130);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(212, 26);
            this.label3.TabIndex = 13;
            this.label3.Text = "Certificate Template:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboTemplate
            // 
            this.cboTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTemplate.FormattingEnabled = true;
            this.cboTemplate.Location = new System.Drawing.Point(325, 130);
            this.cboTemplate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboTemplate.Name = "cboTemplate";
            this.cboTemplate.Size = new System.Drawing.Size(1009, 34);
            this.cboTemplate.TabIndex = 14;
            this.cboTemplate.SelectedIndexChanged += new System.EventHandler(this.cboTemplate_SelectedIndexChanged);
            // 
            // menuStrip
            // 
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1356, 36);
            this.menuStrip.TabIndex = 15;
            this.menuStrip.Text = "Menu Strip";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1356, 649);
            this.Controls.Add(this.cboTemplate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboCA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "KRTool";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRecoverFromList;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCN;
        private System.Windows.Forms.RadioButton rbtnUser;
        private System.Windows.Forms.RadioButton rbtnEDiscovery;
        private System.Windows.Forms.Button btnRecoverKeys;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboCA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTemplate;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MenuStrip menuStrip;
    }
}

