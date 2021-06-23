using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using EasyPKIView;
using MJBLogger;
using Newtonsoft.Json;


namespace PKIKeyRecovery
{
    internal static class RuntimeContext
    {
        internal static Configuration Conf;
        internal static MJBLog Log;
        internal static List<ADCertificationAuthority> CAs;

        private static string charSet = string.Empty;
        private static bool gotCharSet = false;
        internal static string CharSet
        {
            get
            {
                if (!gotCharSet)
                {
                    charSet = Conf.UseAlphas ? CharacterSet.Alpha : string.Empty;
                    charSet += Conf.UseDigits ? CharacterSet.Digit : string.Empty;
                    charSet += Conf.UseSymbols ? CharacterSet.Symbol : string.Empty;
                    gotCharSet = true;
                }
                return CharSet;
            }
        }

        internal static void Init()
        {
            bool gotConf = false;
            if (File.Exists(Constants.ConfFile))
            {
                try
                {
                    Conf = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Constants.ConfFile));
                    gotConf = null != Conf;
                }
                catch { }
            }

            if (!gotConf)
            {
                using (var ConfWindow = new Config())
                {
                    var Result = ConfWindow.ShowDialog();
                    if (Result == DialogResult.OK)
                    {
                        Conf = ConfWindow.Conf;
                        gotConf = true;
                    }
                }

                if (!gotConf)
                {
                    MessageBox.Show(@"Failed to initialize configuration", @"KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }

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
        }
    }
}
