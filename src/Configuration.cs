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
                    pc_displayPasswordOnScreen,
                    pc_displayPasswordOnScreenGLOBAL,
                    mobile_displayPasswordOnScreen,
                    mobile_displayPasswordOnScreenGLOBAL,
                    legal_displayPasswordOnScreen,
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

        public int MinPasswordLength,
                   MaxPasswordLength;

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

        internal static List<ADCertificateTemplate> ADTemplates = ADCertificateTemplate.GetAll()
                                                                                       .Where(p => p.RequiresPrivateKeyArchival)
                                                                                       .ToList();

        private bool artifactsFound;

        public MJBLog Log;

        #endregion

        #region Constructor

        public Configuration()
        {
            try
            {
                //Initialize the Log
                if (!Directory.Exists(".\\logs"))
                    Directory.CreateDirectory(".\\logs");
                Log = new MJBLog();
                Log.SetLevel(GetString(AppSettings.LogLevel));
                Log.Banner();


                //Define CA
                CAHost = GetString(AppSettings.ADCS_Host);
                CACN = GetString(AppSettings.ADCS_CN);
                CA = $"\"{CAHost}\\{CACN}\"";

                //Define Certificate Templates
                templates = new Dictionary<string, string>();
                if (!GetTemplates(GetString(AppSettings.ADCS_Templates), ref templates))
                {
                    conf_valid = false;
                    return;
                }

                //Define Mobile Templates
                mobileTemplates = new Dictionary<string, string>();
                if (!GetTemplates(GetString(AppSettings.ADCS_Mobile_Templates), ref mobileTemplates))
                {
                    conf_valid = false;
                    return;
                }

                //Get Active Directory Domain
                ADdomain = GetString(AppSettings.ADDS_Domain);

                //Get Active Directory Search container DN
                ADscopeDN = GetString(AppSettings.ADDS_ContainerDN);

                //Get PC Password Distribution Method
                if (!GetPasswordDistMethod(GetString(AppSettings.PC_PasswordDistributionMethod), ref pcMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Get Mobile Password Distribution Method
                if (!GetPasswordDistMethod(GetString(AppSettings.Mobile_PasswordDistributionMethod), ref mobileMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Get Legal Password Distribution Method
                if (!GetPasswordDistMethod(GetString(AppSettings.Mobile_PasswordDistributionMethod), ref legalMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Get Values for UseStrongPasswords (Default Value is FALSE)
                pc_strongPasswords = GetBool(AppSettings.PC_UseStrongPasswords);
                mobile_strongPasswords = GetBool(AppSettings.Mobile_UseStrongPasswords);
                legal_strongPasswords = GetBool(AppSettings.Legal_UseStrongPasswords);

                //Get Values for AttachToEmail (Default Value is FALSE)
                pc_attachKeyToEmail = GetBool(AppSettings.PC_AttachKeyToEmail);
                mobile_attachKeyToEmail = GetBool(AppSettings.Mobile_AttachKeyToEmail);
                legal_attachKeyToEmail = GetBool(AppSettings.Legal_AttachKeyToEmail);

                //Get Values for DeleteAfterSending (Default Value is FALSE)
                pc_deleteKeyAfterSending = GetBool(AppSettings.PC_DeleteKeyAfterSending);
                mobile_deleteKeyAfterSending = GetBool(AppSettings.Mobile_DeleteKeyAfterSending);
                legal_deleteKeyAfterSending = GetBool(AppSettings.Legal_DeleteKeyAfterSending);

                //Get Values for KeyRetreivalPath
                pc_keyRetrievalLocation = string.Empty;
                mobile_keyRetrievalLocation = string.Empty;
                legal_keyRetrievalLocation = string.Empty;

                pc_keyRetrievalLocation = GetString(AppSettings.PC_KeyRetrievalLocation);
                mobile_keyRetrievalLocation = GetString(AppSettings.Mobile_KeyRetrievalLocation);
                legal_keyRetrievalLocation = GetString(AppSettings.Legal_KeyRetrievalLocation);

                /*** Email Settings ***/
                mailHost = GetString(AppSettings.mailhost);
                mailSender = GetString(AppSettings.mailSender);
                legal_email = GetString(AppSettings.Legal_Email);

                //PC Email Message Text
                if (!GetMessageBody(GetString(AppSettings.PC_Message), "PC", ref pc_MessageBody, ref pcMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Mobile Email Message Text
                if (!GetMessageBody(GetString(AppSettings.Mobile_Message), "Mobile", ref mobile_MessageBody, ref mobileMethod))
                {
                    conf_valid = false;
                    return;
                }

                //Legal Email Message Text
                if (!GetMessageBody(GetString(AppSettings.Legal_Message), "Legal", ref legal_MessageBody, ref legalMethod))
                {
                    conf_valid = false;
                    return;
                }

                MinPasswordLength = GetInt(nameof(MinPasswordLength), Default.MinPasswordLength);
                MaxPasswordLength = GetInt(nameof(MaxPasswordLength), Default.MaxPasswordLength);

            }

            catch (Exception e)
            {
                conf_valid = false;
                ex = e;
            }


        }
        
        #endregion

        private bool GetTemplates(string expression, ref Dictionary<string, string> templates)
        {
            string[] parts = expression.Split(',');

            foreach (string entry in parts)
            {
                ADCertificateTemplate Template = null;
                bool templateFound = false;

                Template = ADTemplates.FirstOrDefault(p => string.Equals(entry, p.DisplayName, StringComparison.OrdinalIgnoreCase));
                if (null != Template)
                {
                    templateFound = true;
                }
                else
                {
                    Template = ADTemplates.FirstOrDefault(p => string.Equals(entry, p.Name, StringComparison.OrdinalIgnoreCase));
                    if (null != Template)
                    {
                        templateFound = true;
                    }
                    else
                    {
                        Template = ADTemplates.FirstOrDefault(p => string.Equals(entry, p.Oid, StringComparison.OrdinalIgnoreCase));
                        if (null != Template)
                        {
                            templateFound = true;
                        }
                    }
                }

                if (templateFound)
                {
                    templates.Add(Template.DisplayName, Template.Oid);
                }
                else
                {
                    throw new ArgumentException($"No certificate template could be found in Active Directory with the identifier \"{expression}\"");
                }
            }

            return (templates.Count >= 1);
        }

        private bool GetPasswordDistMethod(string selectedMethod, ref PasswordDistMethod configItem)
        {
            switch (selectedMethod.ToUpper().Trim())
            {
                case "SCREEN":
                    configItem = PasswordDistMethod.OnScreen;
                    break;
                case "EMAIL":
                    configItem = PasswordDistMethod.ByEmail;
                    break;
                default:
                    return false;
            }

            return true;
        }

        private bool GetMessageBody(string path, string recipientType, ref string messageBody, ref PasswordDistMethod method)
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

        internal static string GetString(string key, bool required = false)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (required && string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"Required application setting \"{key}\" is not defined.");
            }

            return value;
        }

        internal static bool GetBool(string key)
        {
            string sValue = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(sValue))
            {
                return false;
            }
            else
            {
                try
                {
                    bool value = Convert.ToBoolean(sValue);
                    return value;
                }
                catch
                {
                    throw new ArgumentException($"Application setting \"{key}\" must be set to \"true\" or \"false\"");
                }
            }
        }

        internal static int GetInt(string key, int? defaultValue = null)
        {
            string sValue = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(sValue))
            {
                try
                {
                    int result = Convert.ToInt32(sValue);
                    return result;
                }
                catch { }
            }

            if (null != defaultValue)
            {
                return (int)defaultValue;
            }
            else
            {
                throw new ArgumentException($"Application setting \"{key}\" must be an integer");
            }
        }

    }

}
