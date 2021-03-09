﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MJBLogger;
using EasyPKIView;
using System.Linq;
using Newtonsoft.Json;

namespace PKIKeyRecovery
{
    public partial class Form1 : Form
    {
        Configuration conf;
        public static string KRAgent = "";
        public static string bulkRecoveryMergedPFX,
                             bulkRecoveryRetrievalLocation;
        public static List<string> SerialNumbers;
        public static List<string> unrecoverableKeys;

        public Form1()
        {
            bool configProblems = false;
            InitializeComponent();
            System.Drawing.Icon icon = PKIKeyRecovery.Properties.Resources.pki;
            this.Icon = icon;
            InitializeContext();

            if (!GetConfiguration())
            {
                MessageBox.Show(@"KRTool configuration is missing or invalid. Click OK to open configuration.", @"KRTool", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                var ConfForm = new Config();
                DialogResult Result = ConfForm.ShowDialog(this);
                if (Result == DialogResult.OK)
                {
                    if (!GetConfiguration())
                    {
                        MessageBox.Show(@"Failed to configure KRool", @"KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }

            //conf = new PKIKeyRecovery.Configuration();
            //if (!conf.conf_valid)
            //{
            //    MessageBox.Show("Error reading configuration file.  Check log for details.", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    btnRecoverKeys.Enabled = false;
            //    btnRecoverFromList.Enabled = false;
            //    txtUserName.Enabled = false;
            //    rbtnUser.Enabled = false;
            //    rbtnMobile.Enabled = false;
            //    rbtnEDiscovery.Enabled = false;
            //}

            //else
            //{
            //    if (!conf.mobile_valid)
            //    {
            //        rbtnMobile.Enabled = false;
            //        configProblems = true;
            //    }

            //    if (!conf.legal_valid)
            //    {
            //        rbtnEDiscovery.Enabled = false;
            //        configProblems = true;
            //    }

            //    if (!conf.pc_valid)
            //    {
            //        rbtnUser.Enabled = false;
            //        configProblems = true;
            //    }

            //    if (configProblems)
            //        lblMessages.Text = "Problems found in configuration.\nCheck log for details.";
            //}

            //bulkRecoveryMergedPFX = conf.workingDirectory + stdlib.AppendDate("recovery") + ".pfx";
            //bulkRecoveryRetrievalLocation = conf.Legal_KeyRetrievalLocation + stdlib.AppendDate("recovery") + ".pfx";

            //if (!conf.legal_valid)
            //{
            //    rbtnEDiscovery.Enabled = false;
            //    btnRecoverFromList.Enabled = false;
            //    this.toolTip1.SetToolTip(this.rbtnEDiscovery, "Email settings for Legal Discovery are not properly defined.");
            //    this.toolTip1.SetToolTip(this.btnRecoverFromList, "Recovering keys for a list of users is disabled because Email settings for Legal Discovery are not properly defined.");
            //}

            //ade = new ActiveDirectoryExplorer(ADdomain, ADUserStoreDN);
        }

        private bool GetConfiguration()
        {
            if (!File.Exists(Configuration.ConfFile))
            {
                return false;
            }

            try
            {
                conf = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Configuration.ConfFile));
                return conf.Valid;
            }
            catch
            {
                return false;
            }
        }

        private void validateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                btnRecoverKeys.Enabled = false;
                btnValidate.Enabled = false;
            }
            else
            {
                btnValidate.Enabled = true;
                btnRecoverKeys.Enabled = rbtnUser.Checked || rbtnEDiscovery.Checked || rbtnMobile.Checked;
            }
        }

        private bool RecoverKeysForUser(string username, string password)
        {
            User user = new User(username, conf);

            if (user.valid)
            {
                if (user.HasArchivedCerts())
                {
                    if (!user.recoverKeysFromCA(password, false, rbtnEDiscovery.Checked))
                    {
                        conf.Log.Error($"Problems were encountered when attempting recovery keys for user \"{username}\"");
                        return false;
                    }
                    return deliverKeys(user, password, rbtnEDiscovery.Checked);
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
                anyKeysRecovered,
                mergedKeys;

            User currentUser;

            fullSuccess = 1;
            anyKeysRecovered = 0;
            mergedKeys = 0;

            List<string> pfxFiles = new List<string>();

            foreach (string username in usernames)
            {
                conf.Log.Info("Processing user \"" + username + "\"");
                currentUser = new User(username, conf);
                if (!currentUser.exists())
                {
                    conf.Log.Warning($"User \"{username}\" was not found in the Active Directory");
                }
                else
                {
                    conf.Log.Verbose($"User \"{username}\" found in the specified Active Directory container.");
                }

                if (currentUser.valid)
                {
                    if (currentUser.recoverKeysFromCA(password, true, false))
                    {
                        conf.Log.Verbose("Key recovery for user " + username + " succeeded.");
                        if (currentUser.HasArchivedCerts())
                        {
                            conf.Log.Verbose("User \"" + username + " had one or more archived keys.");
                            anyKeysRecovered = 1;
                            if (currentUser.hasMergedPFX())
                            {
                                conf.Log.Verbose("User \"" + username + " has a combined PFX file: \"" + currentUser.mergedPFX + "\" this PFX file will be merged into the combined PFX file for this recovery request.");
                                mergedKeys++;
                                pfxFiles.Add(currentUser.mergedPFX);
                            }
                            else
                            {
                                conf.Log.Warning("PFX files for user \"" + username + "\" could not be merged.  The individual PFX files for this user will be attempted to be merged into the combined PFX file for this recovery request.");
                                foreach (string pfxFile in currentUser.keyFiles)
                                {
                                    conf.Log.Echo(pfxFile);
                                    pfxFiles.Add(pfxFile);
                                }
                            }
                        }
                    }
                    else
                    {
                        fullSuccess = 0;
                        conf.Log.Warning("Key recovery for user + \"" + username + "\" was not completely successful.");
                        if (currentUser.AnyKeysRecovered())
                        {
                            conf.Log.Warning("Some keys were able to be recovered for user \"" + username + "\"");
                            anyKeysRecovered = 1;
                            if (currentUser.hasMergedPFX())
                            {
                                conf.Log.Verbose("User \"" + username + " has a combined PFX file: \"" + currentUser.mergedPFX + "\" this PFX file will be merged into the combined PFX file for this recovery request.");
                                mergedKeys++;
                                pfxFiles.Add(currentUser.mergedPFX);
                            }
                            else
                            {
                                conf.Log.Warning("PFX files for user \"" + username + "\" could not be merged.  The individual PFX files for this user will be attempted to be merged into the combined PFX file for this recovery request.");
                                foreach (string pfxFile in currentUser.keyFiles)
                                {
                                    conf.Log.Echo(pfxFile);
                                    pfxFiles.Add(pfxFile);
                                }
                            }
                        }
                    }
                }
                else
                    conf.Log.Error("Certificates for user \"" + username + "\" could not be enumerated.  Skipping.");
            }

            if (mergedKeys >= 1)
            {
                conf.Log.Verbose("Attempting to combine all PFX files for the recovery into a single merged PFX file.");
                if (mergePFX(password, pfxFiles, conf, bulkRecoveryMergedPFX))
                {
                    conf.Log.Info("Merged PFX file \"" + bulkRecoveryMergedPFX + "\" created for this recovery.");
                    if (conf.legal_keyRetrievalLocationDefined)
                    {
                        conf.Log.Info("Attempting to copy merged PFX file to Legal Discovery Key Retrieval Location\"" + bulkRecoveryRetrievalLocation + "\"");
                        try
                        {
                            File.Copy(bulkRecoveryMergedPFX, bulkRecoveryRetrievalLocation);
                            if (!File.Exists(bulkRecoveryRetrievalLocation))
                                conf.Log.Error("A problem was encountered when copying the merged PFX file to the Legal Discovery Key Retreival Location: \"" + bulkRecoveryRetrievalLocation + "\"");
                            else
                            {
                                conf.Log.Verbose("Merged PFX file was copied to the Legal Discovery Retrieval Location: \"" + bulkRecoveryRetrievalLocation + "\"");
                                conf.Log.Verbose("Attempting to delete local Merged PFX file \"" + bulkRecoveryMergedPFX + "\"");
                                if (stdlib.DeleteFile(bulkRecoveryMergedPFX, conf.Log))
                                {
                                    conf.Log.Verbose("Attempting to delete working directory \"" + conf.WorkingDirectory + "\"");
                                    if (Folder.Delete(conf.WorkingDirectory))
                                        conf.Log.Verbose("Successfully deleted working directory.");
                                    else
                                        conf.Log.Warning("Unable to delete working directory \"" + conf.WorkingDirectory + "\"");
                                }
                                else
                                    conf.Log.Warning("Unable to delete local Merged PFX file \"" + bulkRecoveryMergedPFX + "\"");
                            }
                        }

                        catch (Exception e)
                        {
                            conf.Log.Exception(e, "A problem was encountered when copying the merged PFX file to the Legal Discovery Key Retreival Location: \"" + bulkRecoveryRetrievalLocation + "\"");
                        }
                    }

                    conf.Log.Info("Merged PFX file \"" + bulkRecoveryMergedPFX + "\" created for this recovery.  Attempting to delete individual PFX files.");
                    foreach (string pfxFile in pfxFiles)
                    {
                        if (stdlib.DeleteFile(pfxFile, conf.Log))
                            conf.Log.Verbose("Successfully deleted \"" + pfxFile + "\"");
                        else
                            conf.Log.Warning("Could not delete \"" + pfxFile + "\"");
                    }
                }
            }
            return fullSuccess + anyKeysRecovered;
        }

        private bool mergePFX(string password, List<string> pfxFiles, string mergedPFX)
        {
            string keyList = string.Empty;
            string command, sanitizedCommand;

            #region Create_keyList
            keyList = string.Join(@",", pfxFiles.Select(p => $"\"{p}\""));
            //foreach (string file in pfxFiles)
            //{
            //    if (string.IsNullOrEmpty(keyList))
            //        keyList += "\"" + file;
            //    else
            //        keyList += "," + file;
            //}
            //keyList += "\"";
            #endregion

            conf.Log.Info("Attempting to merge all PFX files in the working directory");
            command = "certutil -p \"" + password + "," + password + "\" -mergepfx -user " + keyList + " \"" + mergedPFX + "\"";
            sanitizedCommand = command.Replace(password, "[password]");
            Shell.executeAndLog(command, sanitizedCommand, conf.Log);

            if (File.Exists(mergedPFX))
            {
                conf.Log.Info("Successfully created merged PFX file \"" + mergedPFX + "\"");
                return true;
            }
            else
            {
                conf.Log.Error("Could not merge PFX files");
                return false;
            }
        }

        private static void deleteUsersMergedPFXFiles(string mergedPFX, Configuration conf)
        {
            conf.Log.Info("Deleting individual users' merged PFX files");
            bool allDeleted = true;
            string[] files = Directory.GetFiles(conf.WorkingDirectory, "*.pfx");

            foreach (string file in files)
            {
                if (!string.Equals(file, mergedPFX, StringComparison.OrdinalIgnoreCase))
                {
                    conf.Log.Verbose("Attempting to delete \"" + file + "\"");
                    if (stdlib.DeleteFile(file, conf.Log))
                        conf.Log.Verbose("File \"" + file + "\" successfully deleted.");
                    else
                    {
                        conf.Log.Verbose("File \"" + file + "\" could not be deleted.");
                        allDeleted = false;
                    }
                }
            }

            if (allDeleted)
                conf.Log.Info("All individual users' merged PFX files deleted successfully.");
            else
                conf.Log.Warning("Not all individual users' merged PFX files could be deleted.");
        }

        private bool SendEmail(User user, string password, bool eDiscovery)
        {
            string email,
                   from,
                   subject,
                   message;

            from = conf.mailSender;

            if (eDiscovery)
            {
                email = conf.DiscoveryEmail;
                subject = "Recovered Encryption Keys for (" + user.getCN() + ")";
                message = conf.Legal_Message.Replace("[PASSWORD]", password).Replace("[PATH]", user.legalDiscoveryKeyRetrievalLocation);
                if (conf.Legal_AttachKeyToEmail & user.keysMerged)
                {
                    stdlib.SendMail(from, email, subject, message, user.mergedPFX, conf.mailhost);
                    conf.Log.Info("Email sent to \"" + email + "\".  Key file was attached.");
                }

                else
                {
                    stdlib.SendMail(from, email, subject, message, conf.mailhost);
                    conf.Log.Info("Email sent to " + email);
                }
            }
            else
            {
                email = user.getEmail();
                if (email == null)
                {
                    conf.Log.Error("Email address for " + user.getSAM() + " not found in the Active Directory.");
                    return false;
                }
                if (!stdlib.isValidEmailAddress(email))
                {
                    conf.Log.Error("Email address for " + user.getSAM() + " was found in the Active Directory, but it is not in a RFC822-compliant format.");
                    return false;
                }
                subject = "Your recovered encryption keys";
                message = conf.PC_Message.Replace("[PASSWORD]", password).Replace("[NAME]", user.firstName());

                if (conf.pc_keyRetrievalLocationDefined)
                    message = message.Replace("[PATH]", user.keyRetrievalLocation);

                if (conf.PC_AttachKeyToEmail & user.keysMerged)
                {
                    stdlib.SendMail(from, email, subject, message, user.mergedPFX, conf.mailhost);
                    conf.Log.Info("Email sent to \"" + email + "\".  Key file was attached.");
                }
                else
                {
                    stdlib.SendMail(from, email, subject, message, conf.mailhost);
                    conf.Log.Info("Email sent to " + email);
                }
            }


            return true;
        }

        //private bool SendEmail(User user, bool eDiscovery)
        //{
        //    string email,
        //           from,
        //           subject,
        //           message;

        //    from = conf.mailSender;

        //    if (eDiscovery)
        //    {
        //        email = conf.Legal_Email;
        //        subject = "Recovered Encryption Keys for (" + user.getCN() + ")";
        //        message = conf.Legal_Message;

        //        if (conf.Legal_AttachKeyToEmail & user.keysMerged)
        //        {
        //            stdlib.SendMail(from, email, subject, message, user.mergedPFX, conf.mailhost);
        //            conf.Log.Info("Email sent to \"" + email + "\".  Key file was attached.");
        //        }
        //        else
        //        {
        //            stdlib.SendMail(from, email, subject, message, conf.mailhost);
        //            conf.Log.Info("Email sent to " + email);
        //        }
        //    }
        //    else
        //    {
        //        email = user.getEmail();
        //        if (email == null)
        //        {
        //            conf.Log.Error("Email address for " + user.getSAM() + " not found in the Active Directory.");
        //            return false;
        //        }
        //        if (!stdlib.isValidEmailAddress(email))
        //        {
        //            conf.Log.Error("Email address for " + user.getSAM() + " was found in the Active Directory, but it is not in a RFC822-compliant format.");
        //            return false;
        //        }
        //        subject = "Your recovered encryption keys";
        //        message = conf.PC_Message.Replace("[NAME]", user.firstName());

        //        if (conf.PC_AttachKeyToEmail & user.keysMerged)
        //        {
        //            stdlib.SendMail(from, email, subject, message, user.mergedPFX, conf.mailhost);
        //            conf.Log.Info("Email sent to \"" + email + "\".  Key file was attached.");

        //        }
        //        else
        //        {
        //            stdlib.SendMail(from, email, subject, message, conf.mailhost);
        //            conf.Log.Info("Email sent to " + email);
        //        }
        //    }
        //    return true;
        //}

        //private void SendEmail(string password)
        //{
        //    string email,
        //           from,
        //           subject,
        //           message;

        //    from = conf.mailSender;
        //    email = conf.Legal_Email;
        //    subject = "Recovered Encryption Keys";
        //    message = conf.Legal_Message.Replace("[PASSWORD]", password);
        //    stdlib.SendMail(from, email, subject, message, conf.mailhost);
        //    conf.Log.Info("Email sent to " + email);
        //}

        //private bool SendEmailMobile(string email, string SAM, string password, string keyFile)
        //{
        //    string from,
        //           subject,
        //           message;

        //    ActiveDirectoryExplorer ade = new ActiveDirectoryExplorer(conf.ADDS_Domain, conf.ADDS_ContainerDN);

        //    try
        //    {

        //        from = conf.mailSender;
        //        subject = "Your recovered encryption key";
        //        message = conf.Mobile_Message;
        //        if (conf.mobile_keyRetrievalLocationDefined)
        //            message = message.Replace("[PATH]", conf.Mobile_KeyRetrievalLocation);
        //        if (!conf.mobile_displayPasswordOnScreen)
        //            message = message.Replace("[PASSWORD]", password);
        //        message = message.Replace("[NAME]", ade.GetFirstName(SAM));
        //        if (conf.Mobile_AttachKeyToEmail)
        //            stdlib.SendMail(from, email, subject, message, keyFile, conf.mailhost);
        //        else
        //            stdlib.SendMail(from, email, subject, message, conf.mailhost);
        //        conf.Log.Info("Email sent to " + email);
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        conf.Log.Exception(e, "A problem was encountered when attempting to send the Email");
        //        return false;
        //    }
        //}

        //private void SendEmailBulk(string password)
        //{
        //    string email,
        //           from,
        //           subject,
        //           message;

        //    from = conf.mailSender;

        //    email = conf.Legal_Email;
        //    subject = "Recovered Encryption Keys";
        //    message = conf.Legal_Message.Replace("[PASSWORD]", password).Replace("[PATH]", bulkRecoveryRetrievalLocation);
        //    stdlib.SendMail(from, email, subject, message, conf.mailhost);
        //    conf.Log.Info("Email sent to " + email);
        //}

        //private bool deliverKeys(User user, string password, bool eDiscovery)
        //{
        //    string dialogMessage = "";
        //    bool success,
        //         dialogNeeded,
        //         displayPasswordOnScreen,
        //         attachToEmail;

        //    if (eDiscovery)
        //    {
        //        displayPasswordOnScreen = conf.legal_displayPasswordOnScreen;
        //        attachToEmail = conf.Legal_AttachKeyToEmail;
        //    }
        //    else
        //    {
        //        displayPasswordOnScreen = conf.pc_displayPasswordOnScreen;
        //        attachToEmail = conf.PC_AttachKeyToEmail;
        //    }

        //    dialogNeeded = false;

        //    if (user.AnyKeysRecovered())
        //    {

        //        if (user.fullSuccess)
        //            success = true;
        //        else
        //            success = false;

        //        if (!user.keysMerged)
        //        {
        //            dialogMessage = "A single, merged PFX file was unable to be created.  The individual PFX files can be found here: \"" + user.keyDirectory + "\"";
        //            success = false;
        //            dialogNeeded = true;
        //        }

        //        if (attachToEmail)
        //        {
        //            if (displayPasswordOnScreen)
        //            {
        //                dialogNeeded = true;
        //                dialogMessage = dialogMessage + Environment.NewLine + Environment.NewLine + "Password: " + password + Environment.NewLine + Environment.NewLine + "Please provide this password using an out-of-band method (e.g. on the phone or in-person)";
        //            }
        //        }

        //        //Send Email message (if applicable)
        //        if (conf.mail_valid)
        //        {
        //            if (displayPasswordOnScreen)
        //                SendEmail(user, eDiscovery);
        //            else
        //                SendEmail(user, password, eDiscovery);
        //        }

        //        //Show Dialog
        //        if (dialogNeeded)
        //            MessageBox.Show(dialogMessage, "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //        if (user.keysMerged)
        //            user.cleanUp(false);

        //        return success;
        //    }

        //    else
        //    {
        //        if (user.fullSuccess)
        //        {
        //            MessageBox.Show("No keys were found for user \"" + txtUserName.Text + "\"", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return true;
        //        }
        //        else
        //        {
        //            MessageBox.Show("No keys could be recovered for user \"" + txtUserName.Text + "\"", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //}

        //private bool mobileRecovery(string SAM)
        //{
        //    conf.Log.Verbose("Entering method: mobileRecovery(string)");
        //    string outfile = conf.workingDirectory + "mobileRecovery.out";
        //    string command = "certutil -config " + conf.CA + " -view -restrict \"upn=" + SAM + "@" + conf.ADDS_Domain + "\" -out serialnumber,notbefore > " + outfile;
        //    string keyFile = conf.workingDirectory + SAM + "_mobile.pfx";
        //    string BLOBFile = conf.workingDirectory + SAM + "_mobile.BLOB";
        //    string mostRecentSN = "";
        //    string email = "";
        //    string currentRecord,
        //           currentSN,
        //           password;
        //    DateTime mostCurrentlyIssued = new DateTime();
        //    DateTime issuanceDate = new DateTime();
        //    bool anyCertsAnalyzedYet = false;
        //    bool certFound = false;
        //    bool success = true;

        //    ActiveDirectoryExplorer ade = new ActiveDirectoryExplorer(conf.ADDS_Domain, conf.ADDS_ContainerDN);
        //    if (ade.UserExists(SAM, true))
        //    {
        //        email = ade.GetEmail(SAM);
        //        if (email == null)
        //        {
        //            DialogResult dr = MessageBox.Show("No Email address could be found for username \"" + SAM + "\".  Emailing the recovery password to the user will not be possible.\n\nWould you like to have the recovery password displayed on-screen instead?",
        //                        "KRTool",
        //                        MessageBoxButtons.YesNo,
        //                        MessageBoxIcon.Question);

        //            if (dr == DialogResult.Yes)
        //            {
        //                conf.mobile_displayPasswordOnScreen = true;
        //                conf.Mobile_DeleteKeyAfterSending = false;
        //            }
        //            else
        //                return false;
        //        }
        //    }
        //    else
        //    {
        //        DialogResult dr = MessageBox.Show("Username \"" + SAM + "\" is either disabled or was not found in the Active Directory.  Emailing the recovery password to the user will not be possible.\n\nWould you like to have the recovery password displayed on-screen instead?",
        //                        "KRTool",
        //                        MessageBoxButtons.YesNo,
        //                        MessageBoxIcon.Question);

        //        if (dr == DialogResult.Yes)
        //        {
        //            conf.mobile_displayPasswordOnScreen = true;
        //            conf.Mobile_DeleteKeyAfterSending = false;
        //        }
        //        else
        //            return false;
        //    }

        //    Shell.executeAndLog(command, command, conf.Log);
        //    if (!File.Exists(outfile))
        //    {
        //        conf.Log.Error("File not found: \"" + outfile + "\"");
        //        return false;
        //    }
        //    TextReader tr = new StreamReader(outfile);

        //    while (tr.Peek() != -1)
        //    {
        //        if (conf.Log.Level.GE(LogLevel.Verbose))
        //            conf.Log.LineFeed();
        //        currentRecord = tr.ReadLine();
        //        conf.Log.Verbose("Current line: \"" + currentRecord + "\"");
        //        if (!stdlib.InString(currentRecord, "EMPTY"))
        //        {
        //            if (stdlib.InString(currentRecord, "Serial Number:"))
        //            {
        //                conf.Log.Verbose("Serial number found...");
        //                if (!anyCertsAnalyzedYet)
        //                {
        //                    conf.Log.Verbose("This is the first serial number encountered and will be considered the most recently-issued by default.");
        //                    anyCertsAnalyzedYet = true;
        //                    mostRecentSN = currentRecord.Split(':')[1].Trim();
        //                    conf.Log.Verbose("Most recent serial number: \"" + mostRecentSN + "\"");
        //                    currentRecord = tr.ReadLine();
        //                    mostCurrentlyIssued = Convert.ToDateTime(currentRecord.Split(':')[1].Split(' ')[1]);
        //                    conf.Log.Verbose("Issuance date:             \"" + mostCurrentlyIssued.ToString() + "\"");
        //                    certFound = true;

        //                }
        //                else
        //                {
        //                    certFound = true;
        //                    conf.Log.Verbose("New serial number encountered...");
        //                    currentSN = currentRecord.Split(':')[1].Trim();
        //                    if (conf.Log.Level.GE(LogLevel.Verbose))
        //                        conf.Log.Echo("Serial Number: \"" + currentSN + "\"");
        //                    currentRecord = tr.ReadLine();
        //                    issuanceDate = Convert.ToDateTime(currentRecord.Split(':')[1].Split(' ')[1]);
        //                    if (conf.Log.Level.GE(LogLevel.Verbose))
        //                        conf.Log.Echo("Issuance Date:  \"" + issuanceDate.ToString() + "\"");

        //                    if (issuanceDate > mostCurrentlyIssued)
        //                    {
        //                        conf.Log.Verbose("This certificate's issuance date is the most recent encountered so far.");
        //                        mostRecentSN = currentSN;
        //                        mostCurrentlyIssued = issuanceDate;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //            conf.Log.Verbose("EMPTY serial number.  Skipping...");
        //    }

        //    tr.Close();

        //    if (!certFound)
        //    {
        //        conf.Log.Warning("No certificates were found for \"" + SAM + "\"");
        //        conf.Log.Verbose("Attempting to delete the working directory \"" + conf.workingDirectory + "\"");
        //        if (!Folder.Delete(conf.workingDirectory))
        //            conf.Log.Warning("Unable to delete the working directory \"" + conf.workingDirectory + "\"");
        //        return false;
        //    }
        //    else
        //    {
        //        conf.Log.Info("Most recently-issued certificate for \"" + SAM + "\":");
        //        conf.Log.Echo("Serial Number: \"" + mostRecentSN + "\"", level: LogLevel.Info);
        //        conf.Log.Echo("Issuance Date:  \"" + mostCurrentlyIssued.ToString() + "\"", level: LogLevel.Info);
        //        conf.Log.Info("Recovering certificate...");
        //        password = CryptoClass.GeneratePassword(conf.MinPasswordLength, conf.MaxPasswordLength, conf.Mobile_UseStrongPasswords);
        //        if (recoverKeyMobile(mostRecentSN, BLOBFile, keyFile, password))
        //        {
        //            conf.Log.Info("Key successfully recovered as \"" + keyFile + "\".");

        //            if (conf.mail_valid & (conf.mobile_displayPasswordOnScreen || conf.Mobile_AttachKeyToEmail))
        //            {
        //                conf.Log.Info("Sending Email to \"" + email + "\"");
        //                if (SendEmailMobile(email, SAM, password, keyFile))
        //                    conf.Log.Info("Email successfully sent.");
        //                else
        //                {
        //                    conf.Log.Error("Email was unable to be sent.");
        //                    success = false;
        //                }
        //            }

        //            else
        //                MessageBox.Show("Key successfully recovered.\n\nFile: \"" + keyFile + "\"\n\nPassword: " + password,
        //                                "KRTool",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information);

        //            conf.Log.Verbose("Cleaning up...");
        //            conf.Log.Verbose("Attempting to delete the certutil output file \"" + outfile + "\"");
        //            if (!stdlib.DeleteFile(outfile, conf.Log))
        //                conf.Log.Warning("Unable to delete the certutil output file \"" + outfile + "\"");

        //            conf.Log.Verbose("Attempting to delete the BLOB file \"" + BLOBFile + "\"");
        //            if (!stdlib.DeleteFile(BLOBFile, conf.Log))
        //                conf.Log.Warning("Unable to delete the BLOB file \"" + BLOBFile + "\"");

        //            if (conf.Mobile_DeleteKeyAfterSending & (conf.Mobile_AttachKeyToEmail || (conf.mobile_keyRetrievalLocationDefined & stdlib.InString(conf.Mobile_Message, "[PATH]"))))
        //            {
        //                conf.Log.Verbose("Attempting to delete the key file \"" + keyFile + "\"");
        //                if (!stdlib.DeleteFile(keyFile, conf.Log))
        //                    conf.Log.Warning("Unable to delete the key file \"" + keyFile + "\"");
        //                conf.Log.Verbose("Attempting to delete the working directory \"" + conf.workingDirectory + "\"");
        //                if (!Folder.Delete(conf.workingDirectory))
        //                    conf.Log.Warning("Unable to delete the working directory \"" + conf.workingDirectory + "\"");
        //            }

        //            conf.mobile_displayPasswordOnScreen = conf.mobile_displayPasswordOnScreenGLOBAL;
        //            conf.Mobile_DeleteKeyAfterSending = conf.mobile_deleteKeyAfterSendingGLOBAL;
        //            return success;
        //        }
        //        else
        //        {
        //            conf.Log.Error("A problem was encountered when attempting to recover they key");
        //            conf.Log.Verbose("Cleaning up...");
        //            conf.Log.Verbose("Attempting to delete the certutil output file \"" + outfile + "\"");
        //            if (!stdlib.DeleteFile(outfile, conf.Log))
        //                conf.Log.Warning("Unable to delete the certutil output file \"" + outfile + "\"");

        //            conf.Log.Verbose("Attempting to delete the BLOB file \"" + BLOBFile + "\"");
        //            if (!stdlib.DeleteFile(BLOBFile, conf.Log))
        //                conf.Log.Warning("Unable to delete the BLOB file \"" + BLOBFile + "\"");

        //            if (conf.Mobile_DeleteKeyAfterSending)
        //            {
        //                conf.Log.Verbose("Attempting to delete the key file \"" + keyFile + "\"");
        //                if (!stdlib.DeleteFile(keyFile, conf.Log))
        //                    conf.Log.Warning("Unable to delete the key file \"" + keyFile + "\"");
        //                conf.Log.Verbose("Attempting to delete the working directory \"" + conf.workingDirectory + "\"");
        //                if (!Folder.Delete(conf.workingDirectory))
        //                    conf.Log.Warning("Unable to delete the working directory \"" + conf.workingDirectory + "\"");
        //            }
        //            conf.mobile_displayPasswordOnScreen = conf.mobile_displayPasswordOnScreenGLOBAL;
        //            conf.Mobile_DeleteKeyAfterSending = conf.mobile_deleteKeyAfterSendingGLOBAL;
        //            return false;
        //        }
        //    }
        //}

        //private bool recoverKeyMobile(string serialNumber, string BLOBFile, string keyFile, string password)
        //{
        //    bool recovered = false;
        //    conf.Log.Verbose("Entering method \"recoverKeyMobile(string)\"");
        //    if (makeBLOB(serialNumber, BLOBFile))
        //    {
        //        string[] output;
        //        string command = "certutil -recoverkey -p " + password + " \"" + BLOBFile + "\" \"" + keyFile + "\"";
        //        output = Shell.exec(command, command.Replace(password, "[password]"), conf.Log);
        //        if (File.Exists(keyFile))
        //        {
        //            conf.Log.Info("Key successfully recovered for certificate with serial number \"" + serialNumber + "\" as " + keyFile);
        //            recovered = true;
        //        }
        //        else
        //        {
        //            conf.Log.Error("Key could not be recovered for certificate with serial number \"" + serialNumber + "\"");
        //            if (conf.Log.Level.GE(LogLevel.Error))
        //            {
        //                conf.Log.Echo("Result of command:");
        //                try
        //                {
        //                    foreach (string line in output)
        //                        conf.Log.Echo(line.Replace(password, "[password]"));
        //                }
        //                catch (Exception) { }
        //            }
        //        }
        //    }
        //    return recovered;
        //} //recoverKey

        //private bool makeBLOB(string serialNumber, string BLOBFile)
        //{
        //    string command = "certutil -config " + conf.CA + " -getkey " + serialNumber + " " + "\"" + BLOBFile + "\"";
        //    Shell.exec(command, command, conf.Log);

        //    if (File.Exists(BLOBFile))
        //    {
        //        conf.Log.Info("BLOB for certificate with serial number \"" + serialNumber + "\" saved as \"" + BLOBFile + "\"");
        //        return true;
        //    }
        //    else
        //    {
        //        conf.Log.Error("Unable to retreive BLOB for " + conf.mobileTemplate + " certificate " + serialNumber);
        //        return false;
        //    }
        //}

        //private void txtUserName_TextChanged(object sender, EventArgs e)
        //{
        //    lblCN.Text = "";
        //    validateInput();
        //}

        //private void btnValidate_Click(object sender, EventArgs e)
        //{
        //    string CN;
        //    //ActiveDirectoryExplorer ade = new ActiveDirectoryExplorer(conf.ADDS_Domain, conf.ADDS_ContainerDN);
        //    lblCN.Text = "";
        //    if (!string.IsNullOrEmpty(txtUserName.Text))
        //    {
        //        UserStatus status = GetUserStatus(txtUserName.Text);
        //        if (status.exists)
        //        {
        //            CN = status.commonName;
        //            if (CN.Length >= 20)
        //            {
        //                CN = breakUpCN(CN);
        //            }
        //            lblCN.ForeColor = status.enabled
        //                ? Color.Green
        //                : Color.DarkOrange;
        //            lblCN.Text = CN;
        //        }
        //        else
        //        {
        //            lblCN.ForeColor = Color.Red;
        //            lblCN.Text = "USERNAME NOT FOUND";
        //            lblMessages.Text = $"Username \"{txtUserName.Text}\" was not found in Active Directory";
        //        }
        //    }
        //    else
        //    {
        //        lblCN.Text = "Please enter a username.";
        //    }
        //}

        //private string breakUpCN(string CN)
        //{
        //    char splitter;
        //    string brokenUpCN;

        //    if (stdlib.InString(CN, ","))
        //        splitter = ',';
        //    else
        //    {
        //        if (stdlib.InString(CN, " "))
        //            splitter = ' ';
        //        else
        //            return CN;
        //    }

        //    string[] parts = CN.Split(splitter);

        //    brokenUpCN = parts[0] + splitter + "\n";

        //    for (int x = 1; x < parts.Length; x++)
        //    {
        //        if (x < parts.Length - 1)
        //            brokenUpCN = brokenUpCN + parts[x] + splitter;
        //        else
        //            brokenUpCN = brokenUpCN + parts[x];
        //    }

        //    return brokenUpCN;
        //}

        //private void btnRecoverKeys_Click(object sender, EventArgs e)
        //{
        //    Folder.Create(conf.workingDirectory);
        //    if (!Directory.Exists(conf.workingDirectory))
        //    {
        //        conf.Log.Error("Unable to create working directory \"" + conf.workingDirectory + "\"");
        //        return;
        //    }

        //    if (rbtnMobile.Checked)
        //    {
        //        if (mobileRecovery(txtUserName.Text))
        //            MessageBox.Show("Key recovery succeeded for " + txtUserName.Text + ".\nCheck log for details.", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        else
        //            MessageBox.Show("Key recovery for " + txtUserName.Text + " was not successful.\nCheck log for details", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    else
        //    {

        //        if (RecoverKeysForUser(txtUserName.Text, CryptoClass.GenerateRandomPassword(conf.PC_UseStrongPasswords)))
        //            MessageBox.Show("Key recovery succeeded for " + txtUserName.Text + ".\nCheck log for details.", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        else
        //            MessageBox.Show("Key recovery for " + txtUserName.Text + " was not completely successfully.\nCheck log for details", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //}

        //private void btnRecoverFromList_Click(object sender, EventArgs e)
        //{
        //    string sourceFile,
        //           title,
        //           filter,
        //           password;

        //    title = "Select username list";
        //    filter = "Text files (*.txt)|*.txt";

        //    try
        //    {
        //        sourceFile = stdlib.FileSelecter(title, filter);
        //        if (sourceFile == "")
        //            return;

        //        conf.Log.Verbose("source file name: " + sourceFile);
        //        List<string> usernames = stdlib.ReadFile(sourceFile);
        //        password = CryptoClass.GenerateRandomPassword(conf.Legal_UseStrongPasswords);

        //        switch (RecoverKeysFromList(usernames, password))
        //        {
        //            case 0:
        //                MessageBox.Show("No keys were recovered.  Check log for details", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                break;
        //            case 1:
        //                MessageBox.Show("Not all keys were recovered.  Check log for full details.", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //                SendEmailBulk(password);
        //                break;
        //            case 2:
        //                MessageBox.Show("All keys recovered successfully.  Check log for details.", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                SendEmailBulk(password);
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        conf.Log.Exception(ex, "An exception occurred when attempting to recover keys for a list of users");
        //        MessageBox.Show("Error opening file", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void btnAbout_Click(object sender, EventArgs e)
        //{
        //    AboutBox a = new AboutBox();
        //    a.ShowDialog();
        //}

        //private bool DeleteFile(string fileName)
        //{

        //    if (!(File.Exists(fileName)))
        //        return true;
        //    else
        //    {
        //        while (fileLocked(fileName)) ;

        //        try
        //        {
        //            File.Delete(fileName);
        //        }
        //        catch (IOException e)
        //        {
        //            conf.Log.Exception(e);
        //            return false;
        //        }

        //        if (File.Exists(fileName))
        //        {
        //            conf.Log.Warning("The file could not be deleted: \"" + fileName + "\"");
        //            return false;
        //        }
        //        else
        //            return true;
        //    }
        //}

        //protected virtual bool fileLocked(string fileName)
        //{
        //    if (!File.Exists(fileName))
        //        return false;

        //    FileInfo file = new FileInfo(fileName);
        //    FileStream stream = null;

        //    try
        //    {
        //        stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        //    }

        //    catch (IOException)
        //    {
        //        return true;
        //    }

        //    finally
        //    {
        //        if (stream != null)
        //            stream.Close();
        //    }

        //    return false;
        //}

        //private void rbtnUser_CheckedChanged(object sender, EventArgs e)
        //{
        //    validateInput();
        //}

        //private void rbtnEDiscovery_CheckedChanged(object sender, EventArgs e)
        //{
        //    validateInput();
        //}

        //private void cboCA_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    cboTemplate.DataSource = (cboCA.SelectedValue as ADCertificationAuthority).Templates.Where(p => p.RequiresPrivateKeyArchival).ToList();
        //    cboTemplate.DisplayMember = nameof(ADCertificateTemplate.DisplayName);
        //    cboTemplate.Update();
        //    cboTemplate.SelectedIndex = -1;
        //}

        //private void rbtnMobile_CheckedChanged(object sender, EventArgs e)
        //{
        //    validateInput();
        //}
    }
}
