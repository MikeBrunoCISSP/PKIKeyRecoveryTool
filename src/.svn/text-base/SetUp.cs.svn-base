using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PKIKeyRecovery
{
    public partial class SetUp : Form
    {
        bool passwordsMatch,
             senderEmailValid,
             legalEmailValid,
             domainSpecified,
             scopeDNSpecified,
             smtpServerSpecified,
             smtpUserSpecified,
             userMessageFileValid,
             legalMessageFileValid;

        public SetUp()
        {
            InitializeComponent();

            passwordsMatch = false;
            senderEmailValid = false;
            legalEmailValid = false;
            domainSpecified = false;
            scopeDNSpecified = false;
            smtpServerSpecified = false;
            userMessageFileValid = false;
            legalMessageFileValid = false;
            smtpUserSpecified = false;

            cboCA.DataSource = listCAs();
        }

        void checkInput()
        {
            if ((cboCA.Text != "") & senderEmailValid & legalEmailValid & domainSpecified & scopeDNSpecified & smtpServerSpecified & userMessageFileValid & legalMessageFileValid)
            {
                if (chkSMTPAuthRequired.Checked)
                {
                    if (smtpUserSpecified & passwordsMatch)
                        btnOK.Enabled = true;
                    else
                        btnOK.Enabled = false;
                }
                else
                    btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        static BindingList<string> listCAs()
        {
            string name;
            bool atLeastOne = false;

            string command = "certutil | find \"Config:\"";
            BindingList<string> entries = new BindingList<string>();
            string[] lines = Shell.exec(command);
            foreach (string line in lines)
            {
                if (stdlib.InString(line, "Config:"))
                {
                    atLeastOne = true;
                    name = line.Split('\t')[1].Replace("\'", "").Replace('`', '\0').TrimStart();
                    entries.Add(stdlib.Right(name, name.Length - (name.Length - 1)).Trim());
                }
            }

            if (!atLeastOne)
                return null;

            return entries;
        }

        bool checkPasswords()
        {
            if ((txtSMTPPassword.Text == "") & (txtSMTPConfirmPassword.Text == ""))
            {
                lblMustMatch.Text = "";
                return false;
            }

            if (txtSMTPPassword.Text == txtSMTPConfirmPassword.Text)
            {
                lblMustMatch.Text = "";
                return true;
            }
            else
            {
                lblMustMatch.Text = "Passwords must match.";
                return false;
            }
        }

        private void txtSMTPPassword_TextChanged(object sender, EventArgs e)
        {
            checkPasswords();
            checkInput();
        }

        private void txtSMTPConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            checkPasswords();
            checkInput();
        }

        private void txtSenderEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtSenderEmail.Text == "")
            {
                lblSenderEmailValid.Text = "";
                senderEmailValid = false;
            }
            else
            {

                if (stdlib.isValidEmailAddress(txtSenderEmail.Text))
                {
                    lblSenderEmailValid.Text = "";
                    senderEmailValid = true;
                }
                else
                {
                    lblSenderEmailValid.Text = "Must be a valid Email Address.";
                    senderEmailValid = false;
                }
            }

            checkInput();
        }

        private void txtLegalEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtLegalEmail.Text == "")
            {
                lblLegalEmailValid.Text = "";
                legalEmailValid = false;
            }
            else
            {
                if (stdlib.isValidEmailAddress(txtLegalEmail.Text))
                {
                    lblLegalEmailValid.Text = "";
                    legalEmailValid = true;
                }
                else
                {
                    lblLegalEmailValid.Text = "Must be a valid Email Address.";
                    legalEmailValid = false;
                }
            }

            checkInput();
        }

        private void txtDomain_TextChanged(object sender, EventArgs e)
        {
            if (txtDomain.Text == "")
                domainSpecified = false;
            else
                domainSpecified = true;

            checkInput();
        }

        private void txtScope_TextChanged(object sender, EventArgs e)
        {
            if (txtScope.Text == "")
                scopeDNSpecified = false;
            else
                scopeDNSpecified = true;

            checkInput();
        }

        private void chkSMTPAuthRequired_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSMTPAuthRequired.Checked)
            {
                txtSMTPUser.Enabled = true;
                txtSMTPPassword.Enabled = true;
                txtSMTPConfirmPassword.Enabled = true;
            }
            else
            {
                txtSMTPUser.Enabled = false;
                txtSMTPPassword.Enabled = false;
                txtSMTPConfirmPassword.Enabled = false;
            }

            checkInput();
        }

        private void btnUserBrowse_Click(object sender, EventArgs e)
        {
            string path,
                   contents;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    path = ofd.FileName;
                    TextReader tr = new StreamReader(path);
                    contents = tr.ReadToEnd();

                    if (!stdlib.InString(contents, "[PASSWORD]"))
                    {
                        MessageBox.Show("The selected file does not contain required placeholder text \"[PASSWORD]\".", "KRTool - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        userMessageFileValid = false;
                        txtuserBody.Text = "";
                    }
                    else
                    {
                        if (!stdlib.InString(contents, "[NAME]"))
                            MessageBox.Show("This tool has the ability to address the user by name in Email, however, the required placeholder text \"[NAME]\" must be present.  Currently, it is not.", "KRTool - Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        userMessageFileValid = true;
                        txtuserBody.Text = path;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not open the selected file." + Environment.NewLine + Environment.NewLine + "Error Details:" + Environment.NewLine + ex.Message, "KRTool - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    userMessageFileValid = false;
                    txtuserBody.Text = "";
                }

                finally
                {
                    checkInput();
                }
            }
            
        }

        private void btnLegalBrowse_Click(object sender, EventArgs e)
        {
            string path,
                   contents;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    path = ofd.FileName;
                    TextReader tr = new StreamReader(path);
                    contents = tr.ReadToEnd();

                    if (!stdlib.InString(contents, "[PASSWORD]"))
                    {
                        MessageBox.Show("The selected file does not contain required placeholder text \"[PASSWORD]\".", "KRTool - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        legalMessageFileValid = false;
                        txtLegalBody.Text = "";
                    }
                    else
                    {
                        legalMessageFileValid = true;
                        txtLegalBody.Text = path;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not open the selected file." + Environment.NewLine + Environment.NewLine + "Error Details:" + Environment.NewLine + ex.Message, "KRTool - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    legalMessageFileValid = false;
                    txtLegalBody.Text = "";
                }

                finally
                {
                    checkInput();
                }
            }
        }

        private void txtSMTPUser_TextChanged(object sender, EventArgs e)
        {
            if (txtSMTPUser.Text == "")
                smtpUserSpecified = false;
            else
                smtpUserSpecified = true;
        }

        private void txtSMTPServer_TextChanged(object sender, EventArgs e)
        {
            if (txtSMTPServer.Text == "")
                smtpServerSpecified = false;
            else
                smtpServerSpecified = true;

            checkInput();
        }

    }
}
