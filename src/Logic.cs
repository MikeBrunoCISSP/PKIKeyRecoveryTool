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
        private List<ADCertificationAuthority> CAs = new List<ADCertificationAuthority>();
        private Configuration Config;

        private void InitializeContext()
        {
            //Config = new Configuration();
            RuntimeContext.Log = new MJBLog();
            RuntimeContext.Log.SetLevel(ConfigurationManager.AppSettings[AppSettings.LogLevel]);
            RuntimeContext.Log.Banner();
            RuntimeContext.Log.Verbose(@"Enumerating all Enterprise CAs existing in AD...");

            cboCA.DataSource = RuntimeContext.CAs;
            cboCA.DisplayMember = nameof(ADCertificationAuthority.Config);
        }
    }
}
