using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyPKIView;
using MJBLogger;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;

namespace PKIKeyRecovery
{
    public partial class Form1
    {
        private static PrincipalContext AdContext = new PrincipalContext(ContextType.Domain);
        private MJBLog Log;
        private List<ADCertificationAuthority> CAs = new List<ADCertificationAuthority>();
        private Configuration Config;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker helperBW = sender as BackgroundWorker;
            e.Result = PleaseWait(helperBW);
            if ((int)e.Result < 1)
            {
                MessageBox.Show(@"No enterprise certification authorities advertising templates with archived keys were found in Active Directory", @"KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private int PleaseWait(BackgroundWorker bw)
        {
            var WaitForm = new PleaseWait();
            WaitForm.ShowDialog();
            CAs = ADCertificationAuthority.GetAll()
                                          .Where(p => p.Templates.Where(q => q.RequiresPrivateKeyArchival).Any()).ToList();
            WaitForm.Close();
            return CAs.Count();
        }

        private void InitializeContext()
        {
            //Config = new Configuration();
            Log = new MJBLog();
            Log.SetLevel(ConfigurationManager.AppSettings[AppSettings.LogLevel]);
            Log.Banner();
            Log.Verbose(@"Enumerating all Enterprise CAs existing in AD...");

            using (var WaitForm = new PleaseWait())
            {
                WaitForm.Show();
                WaitForm.Update();
                CAs = ADCertificationAuthority.GetAll()
                                              .Where(p => p.Templates.Where(q => q.RequiresPrivateKeyArchival).Any()).ToList();
            }

            if (CAs.Count < 1)
            {
                MessageBox.Show(@"No enterprise certification authorities advertising templates with archived keys were found in Active Directory", @"KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            cboCA.DataSource = CAs;
            cboCA.DisplayMember = nameof(ADCertificationAuthority.Config);
        }

        private UserStatus GetUserStatus(string username)
        {
            var searcher = new PrincipalSearcher(new UserPrincipal(AdContext) { SamAccountName = username});
            var Result = searcher.FindOne();

            if (null == Result || !(Result is UserPrincipal))
            {
                return new UserStatus(username, false, false);
            }
            else
            {
                return new UserStatus(Result);
            }

        }
    }
}
