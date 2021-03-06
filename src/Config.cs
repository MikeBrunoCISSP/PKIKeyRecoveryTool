﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace PKIKeyRecovery
{
    public partial class Config : Form
    {
        internal Configuration Conf = null;

        private bool destDirSet = false;
        private bool discDirSet = false;
        private bool useEmail = false;
        private bool smtpServerSet = false;
        private bool smtpPortSet = true;
        private bool smtpUsernameSet = false;
        private bool smtpPasswordSet = false;
        private bool discoveryEmailSet = false;
        private bool senderEmailSet = false;

        private bool ConfigSet => destDirSet && (!useEmail || smtpServerSet && smtpPortSet && discoveryEmailSet && senderEmailSet && !(smtpUsernameSet ^ smtpPasswordSet));

        public Config()
        {
            InitializeComponent();
            if (File.Exists(Constants.ConfFile))
            {
                var conf = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Constants.ConfFile));

                if (!string.IsNullOrWhiteSpace(txtDestDir.Text) && Directory.Exists(txtDestDir.Text))
                {
                    txtDestDir.Enabled = true;
                    txtDestDir.Text = conf.DestinationDirectory;
                    txtDestDir.Enabled = false;
                    destDirSet = true;
                }

                if (!string.IsNullOrWhiteSpace(txtDiscDir.Text) && Directory.Exists(txtDiscDir.Text))
                {
                    txtDiscDir.Enabled = true;
                    txtDiscDir.Text = conf.DestinationDirectory;
                    txtDiscDir.Enabled = false;
                    discDirSet = true;
                }

                trkPwdLength.Value = conf.PasswordLength;
                rbtnEmailYes.Checked = conf.UseEmail;

                txtSmtpServer.Text = conf.SmtpServer;
                smtpServerSet = Uri.CheckHostName(txtSmtpServer.Text) != UriHostNameType.Unknown;

                txtSmtpUser.Text = conf.SmtpUsername;
                txtSmtpPassword.Text = conf.SmtpPassword;

                txtSmtpPort.Text = Math.Abs(conf.SmtpPort).ToString();
                smtpPortSet = true;

                rbtnAttachYes.Checked = conf.AttachToEmail;
                rbtnDeleteYes.Checked = Conf.DeleteKeyAfterSending;

                txtDiscEmail.Text = conf.DiscoveryEmail;
                if (txtDiscEmail.Text.IsValidEmail())
                {
                    discoveryEmailSet = true;
                }
                else
                {
                    lblInvalidDiscovery.Text = Constants.InvalidEmail;
                }

                txtSenderEmail.Text = conf.SenderEmail;
                if (txtSenderEmail.Text.IsValidEmail())
                {
                    senderEmailSet = true;
                }
                else
                {
                    lblInvalidSender.Text = Constants.InvalidEmail;
                }

                btnApply.Enabled = false;
            }

            lblPwdLength.Text = trkPwdLength.Value.ToString();
        }

        private void trkPwdLength_Scroll(object sender, EventArgs e)
        {
            lblPwdLength.Text = trkPwdLength.Value.ToString();
        }

        private void txtSmtpPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnDestBrowse_Click(object sender, EventArgs e)
        {
            if (ChooseFolder(out string path))
            {
                txtDestDir.Text = path;
                destDirSet = true;
            }
            btnApply.Enabled = ConfigSet;
        }

        private void rbtnEmailYes_CheckedChanged(object sender, EventArgs e)
        {
            ToggleEmailFields();
        }

        private void rbtnEmailNo_CheckedChanged(object sender, EventArgs e)
        {
            ToggleEmailFields();
        }

        private void ToggleEmailFields()
        {
            useEmail = rbtnEmailYes.Checked;
            txtSmtpServer.Enabled = rbtnEmailYes.Checked;
            txtSmtpPort.Enabled = rbtnEmailYes.Checked;
            txtSmtpUser.Enabled = rbtnEmailYes.Checked;
            txtSmtpPassword.Enabled = rbtnEmailYes.Checked;
            rbtnAttachYes.Enabled = rbtnEmailYes.Checked;
            rbtnAttachNo.Enabled = rbtnEmailYes.Checked;
            txtDiscEmail.Enabled = rbtnEmailYes.Checked;
            txtSenderEmail.Enabled = rbtnEmailYes.Checked;

            btnApply.Enabled = ConfigSet;
        }

        private void txtSenderEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtSenderEmail.Text.IsValidEmail())
            {
                lblInvalidSender.Text = string.Empty;
                senderEmailSet = true;
            }
            else
            {
                lblInvalidSender.Text = @"Invalid Email";
                senderEmailSet = false;
            }
            btnApply.Enabled = ConfigSet;
        }

        private void txtDiscoveryEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtDiscEmail.Text.IsValidEmail())
            {
                lblInvalidDiscovery.Text = string.Empty;
                discoveryEmailSet = true;
            }
            else
            {
                lblInvalidDiscovery.Text = @"Invalid Email";
                discoveryEmailSet = false;
            }

            btnApply.Enabled = ConfigSet;
        }

        private void txtSmtpServer_TextChanged(object sender, EventArgs e)
        {
            if (Uri.CheckHostName(txtSmtpServer.Text) != UriHostNameType.Unknown)
            {
                smtpServerSet = true;
                lblInvalidServer.Text = string.Empty;
            }
            else
            {
                smtpServerSet = false;
                lblInvalidServer.Text = @"Invalid Host Name";
            }

            btnApply.Enabled = ConfigSet;
        }



        private void txtDestDir_TextChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = ConfigSet;
        }

        private void txtSmtpPort_TextChanged(object sender, EventArgs e)
        {
            smtpPortSet = !string.IsNullOrEmpty(txtSmtpPort.Text);
            btnApply.Enabled = ConfigSet;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Conf = new Configuration()
            {
                DestinationDirectory = txtDestDir.Text,
                PasswordLength = trkPwdLength.Value
            };

            if (rbtnEmailYes.Checked)
            {
                Conf.UseEmail = true;
                Conf.SenderEmail = txtSenderEmail.Text;
                Conf.DiscoveryEmail = txtDiscEmail.Text;
                Conf.SmtpServer = txtSmtpServer.Text;
                Conf.SmtpPassword = txtSmtpPassword.Text.Protect();
                Conf.SmtpPort = Convert.ToInt32(txtSmtpPort.Text);
                Conf.AttachToEmail = rbtnAttachYes.Checked;
                Conf.DeleteKeyAfterSending = rbtnDeleteYes.Checked;
            }

            Conf.Commit();
            MessageBox.Show(@"Configuration saved.", @"KRTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSmtpPassword_TextChanged(object sender, EventArgs e)
        {
            smtpPasswordSet = !string.IsNullOrEmpty(txtSmtpPassword.Text);
            btnApply.Enabled = ConfigSet;
        }

        private void btnDiscBrowse_Click(object sender, EventArgs e)
        {
            if (ChooseFolder(out string path))
            {
                txtDiscDir.Text = path;
                discDirSet = true;
            }
        }

        private bool ChooseFolder(out string path)
        {
            path = string.Empty;
            bool pathChosen = false;
            using (var Chooser = new FolderBrowserDialog())
            {
                var Result = Chooser.ShowDialog();

                if (Result == DialogResult.OK & !string.IsNullOrWhiteSpace(Chooser.SelectedPath))
                {
                    path = Chooser.SelectedPath;
                    pathChosen = true;
                }
            }

            return pathChosen;
        }

        private void txtSmtpUser_TextChanged(object sender, EventArgs e)
        {
            smtpUsernameSet = !string.IsNullOrEmpty(txtSmtpUser.Text);
            btnApply.Enabled = ConfigSet;
        }
    }
}
