using System;
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
            InitializeComponent();
            cboCA.DataSource = RuntimeContext.CAs;
            cboCA.DisplayMember = nameof(ADCertificationAuthority.Name);
            System.Drawing.Icon icon = PKIKeyRecovery.Properties.Resources.pki;
            this.Icon = icon;
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
            User user = new User(username, (ADCertificationAuthority)cboCA.SelectedItem);

            if (user.valid)
            {
                if (user.HasArchivedCerts)
                {
                    if (!user.RecoverKeysFromCA(password, false, rbtnEDiscovery.Checked))
                    {
                        RuntimeContext.Log.Error($"Problems were encountered when attempting recovery keys for user \"{username}\"");
                        return false;
                    }
                    return DeliverKeys(user, password, rbtnEDiscovery.Checked);
                }
                else
                    return true;
            }
            else
                return false;
        }

        private int RecoverKeysFromList(List<string> usernames, string password)
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
                RuntimeContext.Log.Info("Processing user \"" + username + "\"");
                currentUser = new User(username, (ADCertificationAuthority)cboCA.SelectedItem);
                if (!currentUser.Exists)
                {
                    RuntimeContext.Log.Warning($"User \"{username}\" was not found in the Active Directory");
                }
                else
                {
                    RuntimeContext.Log.Verbose($"User \"{username}\" found in the specified Active Directory container.");
                }

                if (currentUser.valid)
                {
                    if (currentUser.RecoverKeysFromCA(password, true, false) && currentUser.HasArchivedCerts)
                    {
                        RuntimeContext.Log.Verbose("User \"" + username + " had one or more archived keys.");
                        anyKeysRecovered = 1;
                        if (currentUser.KeysMerged)
                        {
                            RuntimeContext.Log.Verbose("User \"" + username + " has a combined PFX file: \"" + currentUser.MergedPFX + "\" this PFX file will be merged into the combined PFX file for this recovery request.");
                            mergedKeys++;
                            pfxFiles.Add(currentUser.MergedPFX);
                        }
                        else
                        {
                            RuntimeContext.Log.Warning("PFX files for user \"" + username + "\" could not be merged.  The individual PFX files for this user will be attempted to be merged into the combined PFX file for this recovery request.");
                            foreach (string pfxFile in currentUser.keyFiles)
                            {
                                RuntimeContext.Log.Echo(pfxFile);
                                pfxFiles.Add(pfxFile);
                            }
                        }

                        RuntimeContext.Log.Verbose("Key recovery for user " + username + " succeeded.");
                    }
                    else
                    {
                        fullSuccess = 0;
                        RuntimeContext.Log.Warning($"Key recovery for user + \"{username}\" was not completely successful.");
                        if (currentUser.AnyKeysRecovered)
                        {
                            RuntimeContext.Log.Warning($"Some keys were able to be recovered for user \"{username}\"");
                            anyKeysRecovered = 1;
                            if (currentUser.KeysMerged)
                            {
                                RuntimeContext.Log.Verbose($"User \"{username}\" has a combined PFX file: \"{currentUser.MergedPFX}\" this PFX file will be merged into the combined PFX file for this recovery request.");
                                mergedKeys++;
                                pfxFiles.Add(currentUser.MergedPFX);
                            }
                            else
                            {
                                RuntimeContext.Log.Warning($"PFX files for user \"{username}\" could not be merged.  The individual PFX files for this user will be attempted to be merged into the combined PFX file for this recovery request.");
                                foreach (string pfxFile in currentUser.keyFiles)
                                {
                                    RuntimeContext.Log.Echo(pfxFile);
                                    pfxFiles.Add(pfxFile);
                                }
                            }
                        }
                    }
                }
                else
                    RuntimeContext.Log.Error($"Certificates for user \"{username}\" could not be enumerated.  Skipping.");
            }

            if (mergedKeys >= 1)
            {
                RuntimeContext.Log.Verbose("Attempting to combine all PFX files for the recovery into a single merged PFX file.");
                if (MergePFX(password, pfxFiles, bulkRecoveryMergedPFX))
                {
                    RuntimeContext.Log.Info($"Merged PFX file \"{bulkRecoveryMergedPFX}\" created for this recovery.");
                    if (rbtnEDiscovery.Checked)
                    {
                        RuntimeContext.Log.Info($"Attempting to copy merged PFX file to Legal Discovery Key Retrieval Location\"{bulkRecoveryRetrievalLocation}\"");
                        try
                        {
                            File.Copy(bulkRecoveryMergedPFX, bulkRecoveryRetrievalLocation);
                            if (!File.Exists(bulkRecoveryRetrievalLocation))
                            {
                                RuntimeContext.Log.Error($"A problem was encountered when copying the merged PFX file to the Legal Discovery Key Retreival Location: \"{bulkRecoveryRetrievalLocation}\"");
                            }
                            else
                            {
                                RuntimeContext.Log.Verbose($"Merged PFX file was copied to the Legal Discovery Retrieval Location: \"{bulkRecoveryRetrievalLocation}\"");
                                RuntimeContext.Log.Verbose($"Attempting to delete local Merged PFX file \"{bulkRecoveryMergedPFX}\"");
                                if (stdlib.DeleteFile(bulkRecoveryMergedPFX))
                                {
                                    RuntimeContext.Log.Verbose($"Attempting to delete working directory \"{RuntimeContext.Conf.WorkingDirectory}\"");
                                    if (Folder.Delete(RuntimeContext.Conf.WorkingDirectory))
                                    {
                                        RuntimeContext.Log.Verbose("Successfully deleted working directory.");
                                    }
                                    else
                                    {
                                        RuntimeContext.Log.Warning($"Unable to delete working directory \"{RuntimeContext.Conf.WorkingDirectory}\"");
                                    }
                                }
                                else
                                    RuntimeContext.Log.Warning($"Unable to delete local Merged PFX file \"{bulkRecoveryMergedPFX}\"");
                            }
                        }

                        catch (Exception e)
                        {
                            RuntimeContext.Log.Exception(e, $"A problem was encountered when copying the merged PFX file to the Legal Discovery Key Retreival Location: \"{bulkRecoveryRetrievalLocation}\"");
                        }
                    }

                    RuntimeContext.Log.Info($"Merged PFX file \"{bulkRecoveryMergedPFX}\" created for this recovery.  Attempting to delete individual PFX files.");
                    foreach (string pfxFile in pfxFiles)
                    {
                        if (stdlib.DeleteFile(pfxFile))
                        {
                            RuntimeContext.Log.Verbose($"Successfully deleted \"{pfxFile}\"");
                        }
                        else
                        {
                            RuntimeContext.Log.Warning($"Could not delete \"{pfxFile}\"");
                        }
                    }
                }
            }
            return fullSuccess + anyKeysRecovered;
        }

        private bool MergePFX(string password, List<string> pfxFiles, string mergedPFX)
        {
            string keyList = string.Empty;
            string command, sanitizedCommand;

            keyList = string.Join(@",", pfxFiles.Select(p => $"\"{p}\""));

            RuntimeContext.Log.Info("Attempting to merge all PFX files in the working directory");
            command = $"certutil -p \"{password},{password}\" -mergepfx -user {keyList} \"{mergedPFX}\"";
            sanitizedCommand = command.Replace(password, "[password]");
            Shell.ExecuteAndLog(command, sanitizedCommand);

            if (File.Exists(mergedPFX))
            {
                RuntimeContext.Log.Info($"Successfully created merged PFX file \"{mergedPFX}\"");
                return true;
            }
            else
            {
                RuntimeContext.Log.Error("Could not merge PFX files");
                return false;
            }
        }

        private static void deleteUsersMergedPFXFiles(string mergedPFX)
        {
            RuntimeContext.Log.Info("Deleting individual users' merged PFX files");
            bool allDeleted = true;
            string[] files = Directory.GetFiles(RuntimeContext.Conf.WorkingDirectory, "*.pfx");

            foreach (string file in files)
            {
                if (!string.Equals(file, mergedPFX, StringComparison.OrdinalIgnoreCase))
                {
                    RuntimeContext.Log.Verbose($"Attempting to delete \"{file}\"");
                    if (stdlib.DeleteFile(file))
                        RuntimeContext.Log.Verbose($"File \"{file}\" successfully deleted.");
                    else
                    {
                        RuntimeContext.Log.Verbose($"File \"{file}\" could not be deleted.");
                        allDeleted = false;
                    }
                }
            }

            if (allDeleted)
            {
                RuntimeContext.Log.Info("All individual users' merged PFX files deleted successfully.");
            }
            else
            {
                RuntimeContext.Log.Warning("Not all individual users' merged PFX files could be deleted.");
            }
        }

        private bool SendEmail(User user, string password, bool eDiscovery)
        {
            string email,
                   from,
                   subject,
                   message;

            from = RuntimeContext.Conf.SenderEmail;

            if (eDiscovery)
            {
                email = RuntimeContext.Conf.DiscoveryEmail;
                subject = $"Recovered Encryption Keys for {user.DisplayName}";
                message = MessageTemplates.LegalDiscovery.Replace(Placeholders.Password, password)
                                                        .Replace(Placeholders.Path, user.LegalDiscoveryKeyRetrievalLocation);
                if (RuntimeContext.Conf.AttachToEmail && user.KeysMerged)
                {
                    stdlib.SendMail(from, email, subject, message, user.MergedPFX);
                    RuntimeContext.Log.Info($"Email sent to \"{email}\".  Key file was attached.");
                }

                else
                {
                    stdlib.SendMail(from, email, subject, message);
                    RuntimeContext.Log.Info($"Email sent to \"{email}\".");
                }
            }
            else
            {
                email = user.Email;
                if (email == null)
                {
                    RuntimeContext.Log.Error($"Email address for {user.sAMAccountName} not found in the Active Directory.");
                    return false;
                }
                if (!stdlib.isValidEmailAddress(email))
                {
                    RuntimeContext.Log.Error($"Email address for {user.sAMAccountName} was found in the Active Directory, but it is not in a RFC822-compliant format.");
                    return false;
                }
                subject = "Your recovered encryption keys";
                StringBuilder Message = new StringBuilder($"Hello {user.DisplayName},\r\n\r\n");
                message = MessageTemplates.User.Replace(Placeholders.Name, user.DisplayName)
                                               .Replace(Placeholders.Path, user.KeyDirectory)
                                               .Replace(Placeholders.Password, password);

                if (RuntimeContext.Conf.AttachToEmail)
                {
                    Message.AppendLine($"Your recovered encryption certificate(s) and key(s) are attached to this Email.");
                }
                else
                {
                    Message.AppendLine($"Your recovered encryption certificate(s) and key(s) can be obtained here: {user.KeyRetrievalLocation}");
                }

                if (RuntimeContext.Conf.AttachToEmail & user.KeysMerged)
                {
                    stdlib.SendMail(from, email, subject, message, user.MergedPFX);
                    RuntimeContext.Log.Info("Email sent to \"" + email + "\".  Key file was attached.");
                }
                else
                {
                    stdlib.SendMail(from, email, subject, message);
                    RuntimeContext.Log.Info("Email sent to " + email);
                }
            }


            return true;
        }

        private void cboCA_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboTemplate.DataSource = (cboCA.SelectedValue as ADCertificationAuthority).Templates
                .Where(p => p.RequiresPrivateKeyArchival)
                .ToList();
            cboTemplate.DisplayMember = nameof(ADCertificateTemplate.DisplayName);
            cboTemplate.Update();
            cboTemplate.SelectedIndex = -1;
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
        //            RuntimeContext.Log.Info("Email sent to \"" + email + "\".  Key file was attached.");
        //        }
        //        else
        //        {
        //            stdlib.SendMail(from, email, subject, message, conf.mailhost);
        //            RuntimeContext.Log.Info("Email sent to " + email);
        //        }
        //    }
        //    else
        //    {
        //        email = user.getEmail();
        //        if (email == null)
        //        {
        //            RuntimeContext.Log.Error("Email address for " + user.getSAM() + " not found in the Active Directory.");
        //            return false;
        //        }
        //        if (!stdlib.isValidEmailAddress(email))
        //        {
        //            RuntimeContext.Log.Error("Email address for " + user.getSAM() + " was found in the Active Directory, but it is not in a RFC822-compliant format.");
        //            return false;
        //        }
        //        subject = "Your recovered encryption keys";
        //        message = conf.PC_Message.Replace("[NAME]", user.firstName());

        //        if (conf.PC_AttachKeyToEmail & user.keysMerged)
        //        {
        //            stdlib.SendMail(from, email, subject, message, user.mergedPFX, conf.mailhost);
        //            RuntimeContext.Log.Info("Email sent to \"" + email + "\".  Key file was attached.");

        //        }
        //        else
        //        {
        //            stdlib.SendMail(from, email, subject, message, conf.mailhost);
        //            RuntimeContext.Log.Info("Email sent to " + email);
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
        //    RuntimeContext.Log.Info("Email sent to " + email);
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
        //        RuntimeContext.Log.Info("Email sent to " + email);
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        RuntimeContext.Log.Exception(e, "A problem was encountered when attempting to send the Email");
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
        //    RuntimeContext.Log.Info("Email sent to " + email);
        //}

        private bool DeliverKeys(User user, string password, bool eDiscovery)
        {
            var DialogMessage = new StringBuilder(string.Empty);
            bool success,
                 dialogNeeded,
                 displayPasswordOnScreen,
                 attachToEmail;


            dialogNeeded = false;

            if (user.AnyKeysRecovered)
            {

                if (user.fullSuccess)
                    success = true;
                else
                    success = false;

                if (!user.KeysMerged)
                {
                    DialogMessage.AppendLine($"A single, merged PFX file was unable to be created.  The individual PFX files can be found here: \"{user.KeyDirectory}\"\r\n");
                    success = false;
                    dialogNeeded = true;
                }

                if (RuntimeContext.Conf.AttachToEmail)
                {
                    dialogNeeded = true;
                    DialogMessage.AppendLine($"Password: {password}\r\n\r\nPlease provide this password using an out-of-band method (e.g. on the phone or in-person)");
                }

                //Send Email message (if applicable)
                if (RuntimeContext.Conf.UseEmail)
                {
                    SendEmail(user, password, eDiscovery);
                }

                //Show Dialog
                if (dialogNeeded)
                {
                    MessageBox.Show(DialogMessage.ToString(), "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (user.KeysMerged)
                {
                    user.CleanUp(false);
                }

                return success;
            }

            else
            {
                if (user.fullSuccess)
                {
                    MessageBox.Show("No keys were found for user \"" + txtUserName.Text + "\"", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("No keys could be recovered for user \"" + txtUserName.Text + "\"", "KRTool", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        //private bool mobileRecovery(string SAM)
        //{
        //    RuntimeContext.Log.Verbose("Entering method: mobileRecovery(string)");
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
        //        RuntimeContext.Log.Error("File not found: \"" + outfile + "\"");
        //        return false;
        //    }
        //    TextReader tr = new StreamReader(outfile);

        //    while (tr.Peek() != -1)
        //    {
        //        if (RuntimeContext.Log.Level.GE(LogLevel.Verbose))
        //            RuntimeContext.Log.LineFeed();
        //        currentRecord = tr.ReadLine();
        //        RuntimeContext.Log.Verbose("Current line: \"" + currentRecord + "\"");
        //        if (!stdlib.InString(currentRecord, "EMPTY"))
        //        {
        //            if (stdlib.InString(currentRecord, "Serial Number:"))
        //            {
        //                RuntimeContext.Log.Verbose("Serial number found...");
        //                if (!anyCertsAnalyzedYet)
        //                {
        //                    RuntimeContext.Log.Verbose("This is the first serial number encountered and will be considered the most recently-issued by default.");
        //                    anyCertsAnalyzedYet = true;
        //                    mostRecentSN = currentRecord.Split(':')[1].Trim();
        //                    RuntimeContext.Log.Verbose("Most recent serial number: \"" + mostRecentSN + "\"");
        //                    currentRecord = tr.ReadLine();
        //                    mostCurrentlyIssued = Convert.ToDateTime(currentRecord.Split(':')[1].Split(' ')[1]);
        //                    RuntimeContext.Log.Verbose("Issuance date:             \"" + mostCurrentlyIssued.ToString() + "\"");
        //                    certFound = true;

        //                }
        //                else
        //                {
        //                    certFound = true;
        //                    RuntimeContext.Log.Verbose("New serial number encountered...");
        //                    currentSN = currentRecord.Split(':')[1].Trim();
        //                    if (RuntimeContext.Log.Level.GE(LogLevel.Verbose))
        //                        RuntimeContext.Log.Echo("Serial Number: \"" + currentSN + "\"");
        //                    currentRecord = tr.ReadLine();
        //                    issuanceDate = Convert.ToDateTime(currentRecord.Split(':')[1].Split(' ')[1]);
        //                    if (RuntimeContext.Log.Level.GE(LogLevel.Verbose))
        //                        RuntimeContext.Log.Echo("Issuance Date:  \"" + issuanceDate.ToString() + "\"");

        //                    if (issuanceDate > mostCurrentlyIssued)
        //                    {
        //                        RuntimeContext.Log.Verbose("This certificate's issuance date is the most recent encountered so far.");
        //                        mostRecentSN = currentSN;
        //                        mostCurrentlyIssued = issuanceDate;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //            RuntimeContext.Log.Verbose("EMPTY serial number.  Skipping...");
        //    }

        //    tr.Close();

        //    if (!certFound)
        //    {
        //        RuntimeContext.Log.Warning("No certificates were found for \"" + SAM + "\"");
        //        RuntimeContext.Log.Verbose("Attempting to delete the working directory \"" + conf.workingDirectory + "\"");
        //        if (!Folder.Delete(conf.workingDirectory))
        //            RuntimeContext.Log.Warning("Unable to delete the working directory \"" + conf.workingDirectory + "\"");
        //        return false;
        //    }
        //    else
        //    {
        //        RuntimeContext.Log.Info("Most recently-issued certificate for \"" + SAM + "\":");
        //        RuntimeContext.Log.Echo("Serial Number: \"" + mostRecentSN + "\"", level: LogLevel.Info);
        //        RuntimeContext.Log.Echo("Issuance Date:  \"" + mostCurrentlyIssued.ToString() + "\"", level: LogLevel.Info);
        //        RuntimeContext.Log.Info("Recovering certificate...");
        //        password = CryptoClass.GeneratePassword(conf.MinPasswordLength, conf.MaxPasswordLength, conf.Mobile_UseStrongPasswords);
        //        if (recoverKeyMobile(mostRecentSN, BLOBFile, keyFile, password))
        //        {
        //            RuntimeContext.Log.Info("Key successfully recovered as \"" + keyFile + "\".");

        //            if (conf.mail_valid & (conf.mobile_displayPasswordOnScreen || conf.Mobile_AttachKeyToEmail))
        //            {
        //                RuntimeContext.Log.Info("Sending Email to \"" + email + "\"");
        //                if (SendEmailMobile(email, SAM, password, keyFile))
        //                    RuntimeContext.Log.Info("Email successfully sent.");
        //                else
        //                {
        //                    RuntimeContext.Log.Error("Email was unable to be sent.");
        //                    success = false;
        //                }
        //            }

        //            else
        //                MessageBox.Show("Key successfully recovered.\n\nFile: \"" + keyFile + "\"\n\nPassword: " + password,
        //                                "KRTool",
        //                                MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information);

        //            RuntimeContext.Log.Verbose("Cleaning up...");
        //            RuntimeContext.Log.Verbose("Attempting to delete the certutil output file \"" + outfile + "\"");
        //            if (!stdlib.DeleteFile(outfile, conf.Log))
        //                RuntimeContext.Log.Warning("Unable to delete the certutil output file \"" + outfile + "\"");

        //            RuntimeContext.Log.Verbose("Attempting to delete the BLOB file \"" + BLOBFile + "\"");
        //            if (!stdlib.DeleteFile(BLOBFile, conf.Log))
        //                RuntimeContext.Log.Warning("Unable to delete the BLOB file \"" + BLOBFile + "\"");

        //            if (conf.Mobile_DeleteKeyAfterSending & (conf.Mobile_AttachKeyToEmail || (conf.mobile_keyRetrievalLocationDefined & stdlib.InString(conf.Mobile_Message, "[PATH]"))))
        //            {
        //                RuntimeContext.Log.Verbose("Attempting to delete the key file \"" + keyFile + "\"");
        //                if (!stdlib.DeleteFile(keyFile, conf.Log))
        //                    RuntimeContext.Log.Warning("Unable to delete the key file \"" + keyFile + "\"");
        //                RuntimeContext.Log.Verbose("Attempting to delete the working directory \"" + conf.workingDirectory + "\"");
        //                if (!Folder.Delete(conf.workingDirectory))
        //                    RuntimeContext.Log.Warning("Unable to delete the working directory \"" + conf.workingDirectory + "\"");
        //            }

        //            conf.mobile_displayPasswordOnScreen = conf.mobile_displayPasswordOnScreenGLOBAL;
        //            conf.Mobile_DeleteKeyAfterSending = conf.mobile_deleteKeyAfterSendingGLOBAL;
        //            return success;
        //        }
        //        else
        //        {
        //            RuntimeContext.Log.Error("A problem was encountered when attempting to recover they key");
        //            RuntimeContext.Log.Verbose("Cleaning up...");
        //            RuntimeContext.Log.Verbose("Attempting to delete the certutil output file \"" + outfile + "\"");
        //            if (!stdlib.DeleteFile(outfile, conf.Log))
        //                RuntimeContext.Log.Warning("Unable to delete the certutil output file \"" + outfile + "\"");

        //            RuntimeContext.Log.Verbose("Attempting to delete the BLOB file \"" + BLOBFile + "\"");
        //            if (!stdlib.DeleteFile(BLOBFile, conf.Log))
        //                RuntimeContext.Log.Warning("Unable to delete the BLOB file \"" + BLOBFile + "\"");

        //            if (conf.Mobile_DeleteKeyAfterSending)
        //            {
        //                RuntimeContext.Log.Verbose("Attempting to delete the key file \"" + keyFile + "\"");
        //                if (!stdlib.DeleteFile(keyFile, conf.Log))
        //                    RuntimeContext.Log.Warning("Unable to delete the key file \"" + keyFile + "\"");
        //                RuntimeContext.Log.Verbose("Attempting to delete the working directory \"" + conf.workingDirectory + "\"");
        //                if (!Folder.Delete(conf.workingDirectory))
        //                    RuntimeContext.Log.Warning("Unable to delete the working directory \"" + conf.workingDirectory + "\"");
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
        //    RuntimeContext.Log.Verbose("Entering method \"recoverKeyMobile(string)\"");
        //    if (makeBLOB(serialNumber, BLOBFile))
        //    {
        //        string[] output;
        //        string command = "certutil -recoverkey -p " + password + " \"" + BLOBFile + "\" \"" + keyFile + "\"";
        //        output = Shell.exec(command, command.Replace(password, "[password]"), conf.Log);
        //        if (File.Exists(keyFile))
        //        {
        //            RuntimeContext.Log.Info("Key successfully recovered for certificate with serial number \"" + serialNumber + "\" as " + keyFile);
        //            recovered = true;
        //        }
        //        else
        //        {
        //            RuntimeContext.Log.Error("Key could not be recovered for certificate with serial number \"" + serialNumber + "\"");
        //            if (RuntimeContext.Log.Level.GE(LogLevel.Error))
        //            {
        //                RuntimeContext.Log.Echo("Result of command:");
        //                try
        //                {
        //                    foreach (string line in output)
        //                        RuntimeContext.Log.Echo(line.Replace(password, "[password]"));
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
        //        RuntimeContext.Log.Info("BLOB for certificate with serial number \"" + serialNumber + "\" saved as \"" + BLOBFile + "\"");
        //        return true;
        //    }
        //    else
        //    {
        //        RuntimeContext.Log.Error("Unable to retreive BLOB for " + conf.mobileTemplate + " certificate " + serialNumber);
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
        //        RuntimeContext.Log.Error("Unable to create working directory \"" + conf.workingDirectory + "\"");
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

        //        RuntimeContext.Log.Verbose("source file name: " + sourceFile);
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
        //        RuntimeContext.Log.Exception(ex, "An exception occurred when attempting to recover keys for a list of users");
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
        //            RuntimeContext.Log.Exception(e);
        //            return false;
        //        }

        //        if (File.Exists(fileName))
        //        {
        //            RuntimeContext.Log.Warning("The file could not be deleted: \"" + fileName + "\"");
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
