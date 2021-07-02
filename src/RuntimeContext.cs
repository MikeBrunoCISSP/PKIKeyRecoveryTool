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
using MoreLinq;

namespace PKIKeyRecovery
{
    internal static class RuntimeContext
    {
        internal static bool Usable = false;
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
                return charSet;
            }
        }

        internal static void Init()
        {
            Exception ex1 = null;
            Log = new MJBLog();
            try
            {
                Log.SetLevel(ConfigurationManager.AppSettings[AppSettings.LogLevel]);
            }
            catch (Exception caughtEx) 
            {
                ex1 = caughtEx;
            }
            Log.Banner();
            if (null != ex1)
            {
                Log.Exception(ex1);
            }
            Log.Info($"Logging verbosity setting: {Log.GetLevel()}");

            Log.Verbose(@"Loading configuration...");
            GetConf();

            //bool gotConf = false;
            //if (File.Exists(Constants.ConfFile))
            //{
            //    try
            //    {
            //        Conf = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Constants.ConfFile));
            //        if (null == Conf || !Conf.Valid())
            //        {
            //            Log.Warning(@"Configuration file exists but could not be loaded.");
            //        }
            //        else
            //        {
            //            if (Conf.Version == Constants.ConfigurationVersion)
            //            {
            //                gotConf = true;
            //            }
            //            else
            //            {
            //                Log.Warning($"A configuration file was found, but it contains a version {Conf.Version} configuration. This distribution of KRTool requires configuration version {Constants.ConfigurationVersion}");
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Exception(ex, @"Error loading configuration file");
            //    }
            //}

            //if (!gotConf)
            //{
            //    Log.Info(@"Could not find configuration file, or it was unusable. Displaying configuration GUI...");

            //    try
            //    {
            //        using (var ConfWindow = new Config())
            //        {
            //            var Result = ConfWindow.ShowDialog();
            //            if (Result == DialogResult.OK)
            //            {
            //                Conf = ConfWindow.Conf;
            //                gotConf = Conf.Valid();
            //            }
            //        }
            //    }
            //    catch (Exception ex2)
            //    {
            //        Log.Exception(ex2, @"Unable to capture configuration from GUI");
            //    }

            //    if (!gotConf)
            //    {
            //        Log.Critical(@"Failed to initialize configuration");
            //        MessageBox.Show("Failed to initialize configuration.\r\nCheck log for details", @"KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}

            if (null != Conf)
            {
                Log.Verbose(@"Collecting PKI information from Active Directory...");

                try
                {
                    using (var WaitForm = new PleaseWait())
                    {
                        WaitForm.Show();
                        WaitForm.Update();
                        var AllCAs = ADCertificationAuthority.GetAll();
                        Log.Verbose(@"Certification Authorities found in Active Directory:");
                        AllCAs.ForEach(p => Log.Echo(p.DisplayName, level: LogLevel.Verbose));

                        CAs = AllCAs.Where(p => p.Templates.Where(q => q.RequiresPrivateKeyArchival).Any()).ToList();
                        Log.Verbose(@"Certification Authorities advertising certificate template(s) that require key archival:");
                        CAs.ForEach(p => Log.Echo(p.DisplayName, level: LogLevel.Verbose));
                    }
                }
                catch (Exception ex3)
                {
                    Log.Exception(ex3, @"Field to collect PKI information from Active Directory");
                }

                if (CAs.Count >= 1)
                {
                    Usable = true;
                }
                else
                {
                    MessageBox.Show(@"No enterprise certification authorities advertising templates with archived keys were found in Active Directory", @"KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        internal static bool GetConf()
        {
            bool gotConf = false;

            if (File.Exists(Constants.ConfFile))
            {
                try
                {
                    Conf = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Constants.ConfFile));
                    if (null == Conf || !Conf.Valid())
                    {
                        Log.Warning(@"Configuration file exists but could not be loaded.");
                    }
                    else
                    {
                        gotConf = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Exception(ex, @"Error loading configuration file");
                }
            }

            if (!gotConf)
            {

                Log.Warning(@"Could not find configuration file, or it was unusable. Displaying configuration GUI...");
                gotConf = UpdateConf(initialize: false);
            }

            return gotConf;
        }

        internal static bool UpdateConf(bool initialize = false)
        {
            bool newConfValid = false;

            try
            {
                Config ConfWindow = initialize
                    ? new Config(Conf)
                    : new Config();


                var Result = ConfWindow.ShowDialog();
                if (Result == DialogResult.OK)
                {
                    if (ConfWindow.Conf.Valid())
                    {
                        newConfValid = true;
                        Conf = ConfWindow.Conf;
                    }
                }

                return newConfValid;
            }
            catch (Exception ex2)
            {
                Log.Exception(ex2, @"Unable to capture configuration from GUI");
                return false;
            }
        }
    }
}
