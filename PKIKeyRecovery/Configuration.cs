using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PKIKeyRecovery
{
    public enum PasswordDistMethod
    {
        OnScreen = 0,
        ByEmail = 1
    }

    public class Configuration
    {
        const string confFileName = "KRtool.conf";
        //const string ver = "0.9.0.a";

        #region Attribute Declarations

        public bool conf_valid,
                    mail_valid,
                    pc_keyRetrievalLocationDefined,
                    mobile_keyRetrievalLocationDefined,
                    legal_keyRetrievalLocationDefined,
                    /*pc_displayPasswordOnScreen,
                    pc_displayPasswordOnScreenGLOBAL,
                    mobile_displayPasswordOnScreen,
                    mobile_displayPasswordOnScreenGLOBAL,
                    legal_displayPasswordOnScreen, */
                    pc_attachKeyToEmail,
                    mobile_attachKeyToEmail,
                    legal_attachKeyToEmail,
                    pc_deleteKeyAfterSending,
                    pc_deleteKeyAfterSendingGLOBAL,
                    mobile_deleteKeyAfterSending,
                    mobile_deleteKeyAfterSendingGLOBAL,
                    legal_deleteKeyAfterSending,
                    pc_strongPasswords,
                    mobile_strongPasswords,
                    legal_strongPasswords,
                    mobileTemplateDefined,
                    mobile_MessageDefined,
                    pc_valid,
                    legal_valid,
                    mobile_valid;

        public PasswordDistMethod pcMethod,
                                  mobileMethod,
                                  legalMethod;

        public string CA,
                      ADdomain,
                      ADscopeDN,
                      mailHost,
                      mailSender,
                      legal_email,
                      pc_MessageBody,
                      legal_MessageBody,
                      logLevel,
                      workingDirectory,
                      pc_keyRetrievalLocation,
                      mobile_keyRetrievalLocation,
                      legal_keyRetrievalLocation,
                      mobileTemplate,
                      mobile_MessageBody;

        public Dictionary<string, string> templates;
        public Dictionary<string, string> mobileTemplates;

        public Exception ex = null;

        private string CAHost,
                       CACN;

        public static string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /* private string passwordDistributionMethod,
                       attachKeyToEmailSelection,
                       deleteKeyAfterSendingSelection; */

        private bool artifactsFound;

        public Log log;

        #endregion

        #region Constructor

        public Configuration()
        {
            try
            {
                //Initialize the Log
                if (!Directory.Exists(".\\logs"))
                    Directory.CreateDirectory(".\\logs");
                log = new Log(".\\logs\\KRTool.log");
                log.set_level(Properties.Settings.Default.LogLevel);
                log.writeSeparator("KRTool.exe started by " + Environment.UserDomainName + " at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());


                //Define CA
                CAHost = Properties.Settings.Default.ADCS_Host;
                CACN = Properties.Settings.Default.ADCS_CN;
                this.CA = "\"" + CAHost + "\\" + CACN + "\"";

                //Define Certificate Templates
                templates = new Dictionary<string, string>();
                if (!getTemplates(Properties.Settings.Default.ADCS_Templates, ref templates))
                {
                    conf_valid = false;
                    return;
                }

                //Define Mobile Templates
                mobileTemplates = new Dictionary<string, string>();
                if (!getTemplates(Properties.Settings.Default.ADCS_Mobile_Templates, ref mobileTemplates))
                {
                    conf_valid = false;
                    return;
                }

                //Get Active Directory Domain
                ADdomain = Properties.Settings.Default.ADDS_Domain;

                //Get Active Directory Search container DN
                ADscopeDN = Properties.Settings.Default.ADDS_ContainerDN;

                //Get PC Password Distribution Method
                if (!getPasswordDistMethod(Properties.Settings.Default.PC_PasswordDistributionMethod, ref pcMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Get Mobile Password Distribution Method
                if (!getPasswordDistMethod(Properties.Settings.Default.Mobile_PasswordDistributionMethod, ref mobileMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Get Legal Password Distribution Method
                if (!getPasswordDistMethod(Properties.Settings.Default.Mobile_PasswordDistributionMethod, ref legalMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Get Values for UseStrongPasswords (Default Value is FALSE)
                pc_strongPasswords = Properties.Settings.Default.PC_UseStrongPasswords;
                mobile_strongPasswords = Properties.Settings.Default.Mobile_UseStrongPasswords;
                legal_strongPasswords = Properties.Settings.Default.Legal_UseStrongPasswords;

                //Get Values for AttachToEmail (Default Value is FALSE)
                pc_attachKeyToEmail = Properties.Settings.Default.PC_AttachKeyToEmail;
                mobile_attachKeyToEmail = Properties.Settings.Default.Mobile_AttachKeyToEmail;
                legal_attachKeyToEmail = Properties.Settings.Default.Legal_AttachKeyToEmail;

                //Get Values for DeleteAfterSending (Default Value is FALSE)
                pc_deleteKeyAfterSending = Properties.Settings.Default.PC_DeleteKeyAfterSending;
                mobile_deleteKeyAfterSending = Properties.Settings.Default.Mobile_DeleteKeyAfterSending;
                legal_deleteKeyAfterSending = Properties.Settings.Default.Legal_DeleteKeyAfterSending;

                //Get Values for KeyRetreivalPath
                pc_keyRetrievalLocation = string.Empty;
                mobile_keyRetrievalLocation = string.Empty;
                legal_keyRetrievalLocation = string.Empty;

                pc_keyRetrievalLocation = Properties.Settings.Default.PC_KeyRetrievalLocation;
                mobile_keyRetrievalLocation = Properties.Settings.Default.Mobile_KeyRetrievalLocation;
                legal_keyRetrievalLocation = Properties.Settings.Default.Legal_KeyRetrievalLocation;

                /*** Email Settings ***/
                mailHost = Properties.Settings.Default.mailhost;
                mailSender = Properties.Settings.Default.mailSender;
                legal_email = Properties.Settings.Default.Legal_Email;

                //PC Email Message Text
                if (!getMessageBody(Properties.Settings.Default.PC_Message, "PC", ref pc_MessageBody, ref pcMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Mobile Email Message Text
                if (!getMessageBody(Properties.Settings.Default.Mobile_Message, "Mobile", ref mobile_MessageBody, ref mobileMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Legal Email Message Text
                if (!getMessageBody(Properties.Settings.Default.Legal_Message, "Legal", ref legal_MessageBody, ref legalMethod))
                {
                    conf_valid = false;
                    return;
                }

            }

            catch (Exception e)
            {
                conf_valid = false;
                ex = e;
            }


        }
        
        #endregion

        private bool getTemplates(System.Collections.Specialized.StringCollection entries, ref Dictionary<string, string> templates)
        {
            string[] parts;

            foreach (string entry in entries)
            {
                parts = entry.Split(' ');
                switch (parts.Length)
                {
                    case 1:
                        templates.Add(string.Empty, parts[0]);
                        break;
                    case 2:
                        templates.Add(parts[0], parts[1]);
                        break;
                    default:
                        throw new ConfigurationException("Template", entry);
                }
            }

            return (templates.Count >= 1);
        }

        private bool getPasswordDistMethod(string selectedMethod, ref PasswordDistMethod configItem)
        {
            switch (selectedMethod.ToUpper())
            {
                case "ONSCREEN":
                    configItem = PasswordDistMethod.OnScreen;
                    break;
                case "ON SCREEN":
                    configItem = PasswordDistMethod.OnScreen;
                    break;
                case "BYEMAIL":
                    configItem = PasswordDistMethod.ByEmail;
                    break;
                case "BY EMAIL":
                    configItem = PasswordDistMethod.ByEmail;
                    break;
                default:
                    return false;
            }

            return true;
        }

        private bool getMessageBody(string path, string recipientType, ref string messageBody, ref PasswordDistMethod method)
        {
            if (File.Exists(path))
            {
                messageBody = path;
                return true;
            }
            else
            {
                DialogResult result = MessageBox.Show("File not found: \"" + path + "\".\nCannot distribute key via Email for recipient type " + recipientType + ".  Click [Yes] if you'd like to display the password on-screen for this recipient type.",
                                                      "KRTool",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    method = PasswordDistMethod.OnScreen;
                    return true;
                }
                else
                    return false;
            }
        }

    }

    public class ConfigurationException : Exception
    {
        public ConfigurationException(string element, string value)
            : base("Configuration element \"" + element + "\" has an invalid value: \"" + value + "\"")
        {
        }
    }

}
