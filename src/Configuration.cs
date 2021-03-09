using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MJBLogger;
using System.Configuration;
using EasyPKIView;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MJBLogger;

namespace PKIKeyRecovery
{
    public enum PasswordDistMethod
    {
        OnScreen = 0,
        ByEmail = 1
    }

    public class Configuration
    {
        public MJBLog Log = new MJBLog();

        internal static readonly string ConfFile = @"KRTool.cfg";
        public string DestinationDirectory { get; set; } = string.Empty;
        
        public string DiscoveryDirectory { get; set; } = string.Empty;
        internal bool DiscDirectorySet => !string.IsNullOrEmpty(DiscoveryDirectory) && Directory.Exists(DiscoveryDirectory);

        public int PasswordLength { get; set; } = 16;
        public bool UseEmail { get; set; } = false;
        public string SmtpServer { get; set; } = string.Empty;

        private string smtpPassword = string.Empty;
        public string SmtpPassword
        {
            get
            {
                return smtpPassword;
            }
            set
            {
                smtpPassword = string.IsNullOrEmpty(value)
                    ? string.Empty
                    : Protect(value);
            }
        }
        internal string PlainTextPassword => UnProtect(SmtpPassword);

        public int SmtpPort { get; set; } = 25;

        public bool AttachToEmail { get; set; } = false;

        public string DiscoveryEmail { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;

        internal bool Valid => Directory.Exists(DestinationDirectory) &&
                               (!UseEmail ||
                               (Uri.CheckHostName(SmtpServer) != UriHostNameType.Unknown &&
                               DiscoveryEmail.IsValidEmail() &&
                               SenderEmail.IsValidEmail()));

        internal string WorkingDirectory => Path.Combine(Environment.CurrentDirectory, @"\working");

        public void Commit()
        {
            if (File.Exists(ConfFile))
            {
                File.Delete(ConfFile);
            }

            var unprettyJson = JsonConvert.SerializeObject(this);
            var formattedJson = JToken.Parse(unprettyJson).ToString(Formatting.Indented);
            File.WriteAllText(ConfFile, formattedJson);
        }



        public static string Protect(string plaintext)
        {
            return Convert.ToBase64String(
                ProtectedData.Protect(
                    Encoding.UTF8.GetBytes(plaintext)
                    , null
                    , DataProtectionScope.CurrentUser));
        }

        private static string UnProtect(string ciphertext)
        {
            return Encoding.UTF8.GetString(
                ProtectedData.Unprotect(
                    Convert.FromBase64String(ciphertext)
                    , null
                    , DataProtectionScope.CurrentUser));
        }
    }

}
