using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PKIKeyRecovery
{
    public partial class Form1 : Form
    {
        const int DEBUG = 4;
        const int INFO = 3;
        const int WARNING = 2;
        const int ERROR = 1;
        const int CRITICAL = 0;

        public static string CA = "";
        public static List<string> certificateTemplates;
        public static string ADdomain = "";
        public static string ADUserStoreDN = "";
        public static string workingDirectory = "";
        public static string logFile = "";
        public static string logLevel = "";
        public static int logLevelNum;
        public static string eMailSendingAddress = "";
        public static string eDiscoveryEmail = "";
        public static string userMessage = "";
        public static string eDiscoveryMessage = "";
        public static string email = "";
        public static string mailHost = "";
        public static string KRAgent = "";
        public static string logDirectory = "";
        public static List<string> SerialNumbers;
        public static List<string> unrecoverableKeys;
        public static Log log;
        public static ActiveDirectoryExplorer ade;

        public Form1()
        {
            InitializeComponent();
            if (!(ReadConfFile("KRTool.conf")))
            {
                lblStatus.Text = "Error reading configuration file.";
                btnRecoverKeys.Enabled = false;
                btnRecoverFromList.Enabled = false;
                txtUserName.Enabled = false;
            }

            ade = new ActiveDirectoryExplorer(ADdomain, ADUserStoreDN);
        }

        public class Certificate
        {
            string BLOBFile,
                   keyFile,
                   template,
                   serialNumber;

            int index;
            public bool recovered;

            public void create(string sn, string tmpl, string username, string BLOBDirectory, string keyDirectory, int i)
            {
                template = tmpl;
                serialNumber = sn;
                index = i;

                BLOBFile = BLOBDirectory + username + "_" + template + "(" + index + ").BLOB";
                keyFile = keyDirectory + username + "_" + template + "(" + index + ").pfx";

                recovered = false;

                if (logLevelNum >= DEBUG)
                {
                    log.write(DEBUG, "Serial Number: " + serialNumber, false);
                    log.write(DEBUG, "Certificate Template Name: " + template, false);
                    log.write(DEBUG, "Index: " + index, false);
                    log.write(DEBUG, "BLOB File Name: " + BLOBFile, false);
                    log.write(DEBUG, "Key File Name: " + keyFile, false);
                }
            }

            public bool recoverKey(string password)
            {
                if (makeBLOB())
                {
                    string[] output;
                    string command = "certutil -recoverkey -p " + password + " \"" + BLOBFile + "\" \"" + keyFile + "\"";
                    log.write(DEBUG, "command: " + command.Replace(password, "[password]"), false);
                    output = Shell.exec(command);
                    if (File.Exists(keyFile))
                    {
                        log.write(INFO, "Key successfully recovered for " + template + " certificate " + serialNumber + " as " + keyFile, false);
                        recovered = true;
                    }
                    else
                    {
                        log.write(ERROR, "Key could not be recovered for " + template + " certificate " + serialNumber, false);
                        if (logLevelNum >= ERROR)
                        {
                            log.write("Result of command:");
                            try
                            {
                                foreach (string line in output)
                                    log.write(line.Replace(password, "[password]"));
                            }
                            catch (Exception) { }
                        }
                    }
                }
                return recovered;
            } //recoverKey

            private bool makeBLOB()
            {
                string command = "certutil -config " + CA + " -getkey " + serialNumber + " " + "\"" + BLOBFile + "\"";
                log.write(DEBUG, "command: " + command, false);
                Shell.exec(command);

                if (File.Exists(BLOBFile))
                {
                    log.write(INFO, "BLOB for " + template + " certificate " + serialNumber + " saved as " + BLOBFile, false);
                    return true;
                }
                else
                {
                    log.write(ERROR, "Unable to retreive BLOB for " + template + " certificate " + serialNumber, false);
                    return false;
                }
            }

            public string getTemplate()
            {
                return template;
            }

            public string getSerialNumber()
            {
                return serialNumber;
            }

            public string getKeyFile() { return keyFile; }

        } //class Certificate

        public class User
        {
            string SAM,
                   UPN,
                   CN,
                   Email,
                   BLOBDirectory,
                   keyDirectory;
            List<Certificate> certs;
            bool hasArchivedCerts;
            int recoveredKeys;

            public bool create(string username)
            {
                if (username == null)
                    return false;

                if (ade.UserExists(username))
                {
                    hasArchivedCerts = false;
                    recoveredKeys = 0;
                    int count;
                    log.write(INFO, "Current User: " + username, false);

                    SAM = username;
                    UPN = SAM + "@" + ADdomain;
                    CN = ade.GetCN(SAM);
                    Email = ade.GetEmail(SAM);
                    BLOBDirectory = workingDirectory + SAM + "_BLOBs\\";
                    keyDirectory = workingDirectory + SAM + "_Keys\\";

                    if (logLevelNum >= DEBUG)
                    {
                        log.write(DEBUG, "SAMAccountName: " + SAM, false);
                        log.write(DEBUG, "User Principal Name: " + UPN, false);
                        log.write(DEBUG, "Common Name: " + CN, false);
                        log.write(DEBUG, "Email Address: " + Email, false);
                        log.write(DEBUG, "BLOB Directory: " + BLOBDirectory, false);
                        log.write(DEBUG, "Key Directory: " + keyDirectory, false);
                    }
                    count = getCertificates();
                    switch (count)
                    {
                        case -1:
                            log.write(ERROR, "Problem encountered enumerating certificates for user " + SAM, false);
                            return false;

                        case 0:
                            log.write(INFO, "No certificates with archived keys found for user " + SAM, false);
                            return true;

                        default:
                            log.write(INFO, count.ToString() + " certificates found with archived keys for user " + SAM, false);
                            hasArchivedCerts = true;
                            return true;
                    }
                }
                else
                {
                    log.write(ERROR, "User " + username + " could not be found in the Active Directory", false);
                    return false;
                }
            }

            public bool create(string username, bool verifyAccount)
            {
                if (username == null)
                    return false;

                    hasArchivedCerts = false;
                    recoveredKeys = 0;
                    int count;
                    log.write(INFO, "Current User: " + username, false);

                    SAM = username;
                    UPN = SAM + "@" + ADdomain;
                    BLOBDirectory = workingDirectory + SAM + "_BLOBs\\";
                    keyDirectory = workingDirectory + SAM + "_Keys\\";

                    if (logLevelNum >= DEBUG)
                    {
                        log.write(DEBUG, "SAMAccountName: " + SAM, false);
                        log.write(DEBUG, "User Principal Name: " + UPN, false);
                        log.write(DEBUG, "BLOB Directory: " + BLOBDirectory, false);
                        log.write(DEBUG, "Key Directory: " + keyDirectory, false);
                    }
                    count = getCertificates();
                    switch (count)
                    {
                        case -1:
                            log.write(ERROR, "Problem encountered enumerating certificates for user " + SAM, false);
                            return false;

                        case 0:
                            log.write(INFO, "No certificates with archived keys found for user " + SAM, false);
                            return true;

                        default:
                            log.write(INFO, count.ToString() + " certificates found with archived keys for user " + SAM, false);
                            hasArchivedCerts = true;
                            return true;
                    }
            }


            private int getCertificates()
            {
                Certificate crt;
                string command,
                       currentSN,
                       templateName,
                       templateOID;

                int index;
                int count = 0;
                certs = new List<Certificate>();

                string[] tmp;

                log.write(INFO, "Serial Numbers of Certificates for which to recover keys:", false);
                foreach (string template in certificateTemplates)
                {
                    index = 1;
                    tmp = template.Split(',');
                    templateName = tmp[0];
                    templateOID = tmp[1];

                    command = "certutil -config " + CA + " -view -restrict " + "\"UPN=" + UPN + ",CertificateTemplate=" + templateOID + "\" -out SerialNumber";
                    log.write(DEBUG, "command: " + command, false);
                    Array SNs = Shell.exec(command);
                    foreach (string record in SNs)
                    {
                        try
                        {
                            if (stdlib.InString(record, "Serial Number:"))
                            {
                                count++;
                                currentSN = stdlib.clean(stdlib.Split(record, ':', 1));
                                if (!(String.Equals(currentSN, "EMPTY")))
                                {
                                    log.write(INFO, "     " + currentSN, false);
                                    crt = new Certificate();
                                    crt.create(currentSN, templateName, SAM, BLOBDirectory, keyDirectory, index);
                                    certs.Add(crt);
                                    index++;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            WriteExceptionToLog(e);
                            return -1;
                        }
                    }
                }
                return count;
            } //getCertificates

            public bool recoverKeysFromCA(string password)
            {
                bool fullSuccess = true;

                Folder.Create(BLOBDirectory, true);
                Folder.Create(keyDirectory, true);
                foreach (Certificate crt in certs)
                {
                    if (!(crt.recoverKey(password)))
                        fullSuccess = false;
                    else
                        recoveredKeys++;
                }
                if (!combineKeys(password))
                    fullSuccess = false;

                Folder.Delete(BLOBDirectory);
                if (!fullSuccess)
                    reportUnrecoveredKeys();

                return fullSuccess;
            }

            private void reportUnrecoveredKeys()
            {
                log.write(ERROR, "There were unrecovered keys for " + SAM, true);
                foreach (Certificate crt in certs)
                {
                    if (!crt.recovered)
                        log.write(INFO, "     Serial Number: " + crt.getSerialNumber() + "     Certificate Template: " + crt.getTemplate(), true);
                }
            }

            private bool combineKeys(string password)
            {
                string combinedKeyFile,
                       keyList,
                       command,
                       deleteFile;
                 string[] output;

                combinedKeyFile = keyDirectory + SAM + ".pfx"; 
                keyList = String.Empty;

                log.write(INFO, "Combining keys for " + SAM + " into a single PFX file.", false);
                foreach (Certificate c in certs)
                {
                    if (keyList != String.Empty) keyList += ",";
                    keyList = keyList + c.getKeyFile();
                }

                command = "certutil -p \"" + password + "," + password + "\" -mergepfx -user \"" + keyList + "\" \"" + combinedKeyFile + "\"";
                log.write(DEBUG, "command: " + command.Replace(password, "[password]"), false);
                output = Shell.exec(command);
                if (logLevelNum >= ERROR)
                {
                    log.write("Result of command:");
                    try
                    {
                        foreach (string line in output)
                            log.write(line.Replace(password, "[password]"));
                    }
                    catch (Exception) { }
                }

                if (!File.Exists(combinedKeyFile))
                {
                    log.write(ERROR, "Merged PFX file for user " + SAM + " could not be created.", false);
                    return false;
                }
                log.write(INFO, "Merged PFX file for " + SAM + " was created: " + combinedKeyFile, false);

                log.write(INFO, "Deleting individual key files...", false);
                foreach (Certificate d in certs)
                {
                    deleteFile = d.getKeyFile();
                    File.Delete(deleteFile);
                    if (File.Exists(deleteFile))
                        log.write(WARNING, "Could not delete file:" + deleteFile, false);
                    else
                        log.write(DEBUG, deleteFile + " was deleted successfully.", false);
                }

                return true;
            }

            public string getCN()
            {
                return CN;
            }

            public string getEmail()
            {
                return Email;
            }

            public string getSAM()
            {
                return SAM;
            }

            public bool HasArchivedCerts()
            {
                return hasArchivedCerts;
            }

            public bool AnyKeysRecovered()
            {
                if (recoveredKeys > 0)
                    return true;
                else
                    return false;
            }
        } //class User

        private bool RecoverKeysForUser(string username, string password)
        {
            User user = new User();

            if (user.create(username))
            {
                if (user.HasArchivedCerts())
                {
                    if (user.recoverKeysFromCA(password))
                    {
                        SendEmail(user, password, rbtnEDiscovery.Checked);
                        return true;
                    }
                    else
                    {
                        if (user.AnyKeysRecovered())
                            SendEmail(user, password, rbtnEDiscovery.Checked);
                        return false;
                    }
                }
                else
                    return true;
            }
            else
                return false;
        }

        private int RecoverKeysFromList(List<String> usernames, string password)
        {
            int fullSuccess,
                anyKeysRecovered;

            bool userCreated;
            User currentUser;

            fullSuccess = 1;
            anyKeysRecovered = 0;

            foreach (string username in usernames)
            {
                currentUser = new User();
                if (ade.UserExists(username))
                    userCreated = currentUser.create(username);
                else
                {
                    log.write(WARNING, "User \"" + username + "\" was not found in the specified Active Directory scope (" + ADUserStoreDN + ")", false);
                    userCreated = currentUser.create(username, false);
                }
                if (userCreated)
                {
                    if (currentUser.recoverKeysFromCA(password))
                    {
                        if (currentUser.HasArchivedCerts())
                            anyKeysRecovered = 1;
                    }
                    else
                    {
                        fullSuccess = 0;
                        if (currentUser.AnyKeysRecovered())
                            anyKeysRecovered = 1;
                    }
                }
            }
            return fullSuccess + anyKeysRecovered;
        }

        private bool SendEmail(User user, string password, bool eDiscovery)
        {
            string email,
                   from,
                   subject,
                   firstname,
                   message;

            from = eMailSendingAddress;

            if (eDiscovery)
            {
                email = eDiscoveryEmail;
                subject = "Recovered Encryption Keys for (" + user.getCN() + ")";
                message = eDiscoveryMessage.Replace("[PASSWORD]", password);
            }
            else
            {
                email = user.getEmail();
                if (email == null)
                {
                    log.write(ERROR, "Email address for " + user.getSAM() + " not found in the Active Directory.", false);
                    return false;
                }
                subject = "Your recovered encryption keys";
                firstname = ade.GetFirstName(user.getSAM());
                message = userMessage.Replace("[PASSWORD]", password).Replace("[NAME]",ade.GetFirstName(user.getSAM()));
            }
            stdlib.SendMail(from, email, subject, message, mailHost);
            log.write(INFO, "Email sent to " + email, false);
            return true;
        }

        private void SendEmail(string password)
        {
            string email,
                   from,
                   subject,
                   message;

            from = eMailSendingAddress;
            email = eDiscoveryEmail;
            subject = "Recovered Encryption Keys";
            message = eDiscoveryMessage.Replace("[PASSWORD]",password);
            stdlib.SendMail(from, email, subject, message, mailHost);
            log.write(INFO, "Email sent to " + email, false);
        }

        private static bool ReadConfFile(string confFileName)
        {
            string confIdentifier;
            List<String> confFile = stdlib.ReadFile(confFileName);
            String[] tmp;
            certificateTemplates = new List<string>();

            string logFile = Directory.GetCurrentDirectory() + @"\KRTool.log";
            TextReader tr;

            if (confFile == null)
                return false;
            else
            {

                foreach (string record in confFile)
                {
                    try
                    {
                        if (record.IndexOf("#") != 0)
                        {
                            confIdentifier = stdlib.Split(record, ' ', 0).Trim().ToLower();

                            switch (confIdentifier)
                            {
                                case "ca":
                                    CA = "\"" + record.Substring(record.IndexOf(' ') + 1).Trim() + "\"";
                                    break;
                                case "template":
                                    tmp = record.Split(' ');
                                    certificateTemplates.Add(tmp[1].Trim() + "," + tmp[2].Trim());
                                    break;
                                case "domain":
                                    ADdomain = record.Substring(record.IndexOf(' ') + 1).Trim();
                                    break;
                                case "adscopedn":
                                    ADUserStoreDN = record.Substring(record.IndexOf(' ') + 1).Trim();
                                    break;
                                case "loglevel":
                                    logLevel = record.Substring(record.IndexOf(' ') + 1).Trim();
                                    break;
                                case "legaldiscovery":
                                    eDiscoveryEmail = record.Substring(record.IndexOf(' ') + 1).Trim();
                                    break;
                                case "usermessage":
                                    //userMessage = record.Substring(record.IndexOf(' ') + 1).Trim().Replace('/', '\n');
                                    tr = new StreamReader(record.Substring(record.IndexOf(' ') + 1).Trim());
                                    userMessage = tr.ReadToEnd();
                                    tr.Close();
                                    break;
                                case "legaldiscoverymessage":
                                    //eDiscoveryMessage = record.Substring(record.IndexOf(' ') + 1).Trim().Replace('/', '\n');
                                    tr = new StreamReader(record.Substring(record.IndexOf(' ') + 1).Trim());
                                    eDiscoveryMessage = tr.ReadToEnd();
                                    tr.Close();
                                    break;
                                case "mailhost":
                                    mailHost = record.Substring(record.IndexOf(' ') + 1).Trim();
                                    break;
                                case "mailsender":
                                    eMailSendingAddress = record.Substring(record.IndexOf(' ') + 1).Trim();
                                    break;
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }

            bool allDefined = true;
            workingDirectory = Directory.GetCurrentDirectory() + "\\" + Environment.UserName + "_WorkingDirectory\\";
            Folder.Create(workingDirectory);

            if (!(String.Equals(logLevel, "")))
                logLevelNum = Convert.ToInt16(logLevel);
            else
                logLevelNum = ERROR;

            log = new Log(logFile, logLevelNum);
            log.WriteSeparator("PKI Key Recovery Initializing");

            if (String.Equals(CA, ""))
            {
                log.write(ERROR, "Certification Authority (CA) not defined in configuration file", false);
                allDefined = false;
            }
            else
                log.write(INFO, "CA: " + CA, false);

            if (certificateTemplates.Count == 0)
            {
                log.write(ERROR, "No certificate templates defined in configuration file", false);
                allDefined = false;
            }
            else
            {
                foreach (string template in certificateTemplates)
                {
                    tmp = template.Split(',');
                    log.write(INFO, "Certificate Template:", false);
                    log.write(INFO, "Name: " + tmp[0], false);
                    log.write(INFO, "OID: " + tmp[1], false);

                }
            }
            if (String.Equals(ADdomain, ""))
            {
                log.write(ERROR, "Active Directory Domain (Domain): not defined in configuration file", false);
                allDefined = false;
            }
            else
                log.write(INFO, "Active Directory Domain: " + ADdomain, false);
            if (String.Equals(ADUserStoreDN, ""))
            {
                log.write(ERROR, "DN of AD user container (ADScopeDN): not defined in configuration file", false);
                allDefined = false;
            }
            else
                log.write(INFO, "DN of AD user container: " + ADUserStoreDN, false);
            if (String.Equals(mailHost, ""))
            {
                log.write(ERROR, "SMTP Relay host (mailhost): not defined in configuration file", false);
                allDefined = false;
            }
            else
                log.write(INFO, "SMTP Relay host: " + mailHost, false);
            if (String.Equals(eMailSendingAddress, ""))
            {
                log.write(ERROR, "Email sending address (MailSender): not defined in configuration file", false);
                allDefined = false;
            }
            else
                log.write(INFO, "Email sending address: " + eMailSendingAddress, false);
            if (String.Equals(eDiscoveryEmail, ""))
            {
                log.write(ERROR, "Legal Discovery Email Address (LegalDiscovery): not defined in configuration file", false);
                allDefined = false;
            }
            else
                log.write(INFO, "Legal Discovery Email Address: " + eDiscoveryEmail, false);

            if (String.Equals(userMessage, ""))
            {
                log.write(ERROR, "User message body text (userMessage): not defined in the configuration file", false);
                allDefined = false;
            }
            else
            {
                if (!stdlib.InString(userMessage, "[PASSWORD]"))
                {
                    log.write(ERROR, "User message body text does not contain recovery password placeholder (\"[PASSWORD]\")", false);
                    allDefined = false;
                }
                else
                    log.write(INFO, "User message body text defined.", false);
            }
            if (String.Equals(eDiscoveryMessage, ""))
            {
                log.write(ERROR, "Legal Discovery message body text (LegalDiscoveryMessage): not defined in the configuration file", false);
                allDefined = false;
            }
            else
            {
                if (!stdlib.InString(eDiscoveryMessage, "[PASSWORD]"))
                {
                    log.write(ERROR, "Legal Discovery message body text does not contain recovery password placeholder (\"[PASSWORD]\")", false);
                    allDefined = false;
                }
                else
                    log.write(INFO, "Legal Discovery message body text defined.", false);
            }

            if (allDefined)
            {
                log.write(INFO, "Initialization successful.", false);
                Shell.exec("regsvr32.exe /s certadm.dll");
                return true;
            }

            return false;
        }

        public static void WriteExceptionToLog(Exception e)
        {
            log.write(CRITICAL, "Exception Encountered:", false);
            log.write(e.ToString());
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            lblCN.Text = "";
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            lblCN.Text = "";
            if (!(Equals(txtUserName.Text, "")))
            {
                if (ade.UserExists(txtUserName.Text))
                    lblCN.Text = ade.GetCN(txtUserName.Text);
                else
                    lblStatus.Text = "Please enter a valid username.";
            }
            else
                lblCN.Text = "Please enter a username.";
        }

        private void btnRecoverKeys_Click(object sender, EventArgs e)
        {
            if (RecoverKeysForUser(txtUserName.Text, crypto.GenerateRandomPassword()))
                lblStatus.Text = "Key recovery succeeded for " + txtUserName.Text + ".  Check log for details.";
            else
                lblStatus.Text = "Key recovery for " + txtUserName.Text + " was not completely successfully.\nCheck log for details";
        }

        private void btnRecoverFromList_Click(object sender, EventArgs e)
        {
            string sourceFile,
                   title,
                   filter,
                   password;

            title = "Select username list";
            filter = "Text files (*.txt)|*.txt";
            lblStatus.Text = "";

            try
            { 
                lblStatus.Text = "Recovery in progress...";
                sourceFile = stdlib.FileSelecter(title, filter);
                log.write(DEBUG, "source file name: " + sourceFile, false);
                List<string> usernames = stdlib.ReadFile(sourceFile);
                password = crypto.GenerateRandomPassword();

                switch (RecoverKeysFromList(usernames, password))
                {
                    case 0:
                        lblStatus.Text = "No keys were recovered.  Check log for details";
                        break;
                    case 1:
                        lblStatus.Text = "Not all keys were recovered.  Check log for full details.";
                        SendEmail(password);
                        break;
                    case 2:
                        lblStatus.Text = "All keys recovered successfully.  Check log for details.";
                        SendEmail(password);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.write(CRITICAL, "Exception encountered:", false);
                WriteExceptionToLog(ex);
                MessageBox.Show("Error opening file", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
            a.ShowDialog();
        }
    }
}
