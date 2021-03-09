using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace PKIKeyRecovery
{
    public partial class Config : Form
    {
        private bool destDirSet = false;
        private bool discDirSet = false;
        private bool useEmail = false;
        private bool smtpServerSet = false;
        private bool smtpPortSet = true;
        private bool discoveryEmailSet = false;
        private bool senderEmailSet = false;

        private bool ConfigSet => destDirSet && (!useEmail || smtpServerSet && smtpPortSet && discoveryEmailSet && senderEmailSet);

        public Config()
        {
            InitializeComponent();
            if (File.Exists(Configuration.ConfFile))
            {
                var conf = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Configuration.ConfFile));

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

                txtSmtpPort.Text = Math.Abs(conf.SmtpPort).ToString();
                smtpPortSet = true;

                rbtnAttachYes.Checked = conf.AttachToEmail;

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
            var conf = new Configuration()
            {
                DestinationDirectory = txtDestDir.Text,
                PasswordLength = trkPwdLength.Value
            };

            if (rbtnEmailYes.Checked)
            {
                conf.UseEmail = true;
                conf.SenderEmail = txtSenderEmail.Text;
                conf.DiscoveryEmail = txtDiscEmail.Text;
                conf.SmtpServer = txtSmtpServer.Text;
                conf.SmtpPassword = Configuration.Protect(txtSmtpPassword.Text);
                conf.SmtpPort = Convert.ToInt32(txtSmtpPort.Text);
                conf.AttachToEmail = rbtnAttachYes.Checked;
            }

            conf.Commit();
            MessageBox.Show(@"Configuration saved.", @"KRTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSmtpPassword_TextChanged(object sender, EventArgs e)
        {
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
    }
}
