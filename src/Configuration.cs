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
        public string DestinationDirectory { get; set; } = string.Empty;
        
        public string DiscoveryDirectory { get; set; } = string.Empty;
        internal bool DiscDirectorySet => !string.IsNullOrEmpty(DiscoveryDirectory) && Directory.Exists(DiscoveryDirectory);

        public int PasswordLength { get; set; } = 16;
        public bool UseEmail { get; set; } = false;
        public string SmtpServer { get; set; } = string.Empty;

        public string SmtpUsername { get; set; } = string.Empty;
        public bool SmtpUsernameSet => !string.IsNullOrEmpty(SmtpUsername);

        private string smtpPassword = string.Empty;
        public string SmtpPassword
        {
            get
            {
                return string.IsNullOrEmpty(smtpPassword)
                    ? string.Empty
                    : smtpPassword.UnProtect();
            }
            set
            {
                smtpPassword = value;
            }
        }
        public bool SmtpPasswordSet => !string.IsNullOrEmpty(smtpPassword);

        public int SmtpPort { get; set; } = 25;

        public bool AttachToEmail { get; set; } = false;
        public bool DeleteKeyAfterSending { get; set; } = false;

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
            if (File.Exists(Constants.ConfFile))
            {
                File.Delete(Constants.ConfFile);
            }

            var unprettyJson = JsonConvert.SerializeObject(this);
            var formattedJson = JToken.Parse(unprettyJson).ToString(Formatting.Indented);

            string folderPath = Path.GetDirectoryName(Constants.ConfFile);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(Constants.ConfFile, formattedJson);
        }
    }

}
