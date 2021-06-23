using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace PKIKeyRecovery
{
    public partial class RecoveredDialog : Form
    {
        public RecoveredDialog(bool success, string mainMessage, string addlInfo, string password = null)
        {
            InitializeComponent();
            Icon icon = Properties.Resources.pki;
            this.Icon = icon;

            string imageFile = success
                ? ImageFile.Success
                : ImageFile.Error;

            var Image = new Bitmap(imageFile);
            pbxIcon.Image = Image;

            lblSuccessMessage.Text = mainMessage;

            txtAddInfo.Enabled = true;
            txtAddInfo.Text = addlInfo;
            txtAddInfo.Enabled = false;

            if (string.IsNullOrEmpty(password))
            {
                txtPassword.Visible = false;
                btnCopy.Visible = false;
            }
            else
            {
                txtPassword.Enabled = true;
                txtPassword.Text = password;
                txtPassword.Enabled = false;
                txtPassword.BackColor = SystemColors.Window;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtPassword.Text);

            using (var popup = new PopupNotifier())
            {
                popup.TitleText = @"KRTool";
                popup.ContentText = @"PFX Password copied to the system clipboard";
                popup.Popup();
            } 
        }
    }
}
