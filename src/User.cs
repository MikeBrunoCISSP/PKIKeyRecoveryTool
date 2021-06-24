using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MJBLogger;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using EasyPKIView;
using MoreLinq;

namespace PKIKeyRecovery
{
    public class User
    {
        private static List<string> domains;
        private static bool domainsSet = false;

        private ADCertificationAuthority SelectedCA = null;
        private ADCertificateTemplate SelectedTemplate = null;

        internal static List<string> Domains
        {
            get
            {
                if (!domainsSet)
                {
                    domains = new List<string>();
                    using (var currentForest = Forest.GetCurrentForest())
                    {
                        foreach (var domain in currentForest.Domains)
                        {
                            domains.Add(domain.ToString());
                        }
                    }
                    domainsSet = true;
                }
                return domains;
            }
        }

        public string DisplayName { get; private set; } = string.Empty;
        public string PrincipalName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string sAMAccountName { get; private set; } = string.Empty;

        public bool KeysMerged { get; private set; } = false;
        public bool HasMergedPfx { get; private set; } = false;
        public bool Exists { get; private set; } = false;
        public bool HasEmailAddress { get; private set; } = false;
        public bool AnyKeysRecovered => recoveredKeys > 0;
        public bool HasArchivedCerts => certs.Count > 0;


        public string MergedPFX,
                      KeyDirectory,
                      KeyRetrievalLocation,
                      LegalDiscoveryKeyRetrievalLocation,
                      BLOBDirectory;

        List<Certificate> certs = new List<Certificate>();

        public List<string> keyFiles;

        public bool valid,
                    fullSuccess;
        bool hasUnrecoverableKeys,
             enabled;

        int recoveredKeys;

        public User(string username, ADCertificationAuthority CA, ADCertificateTemplate Template)
        {
            if (string.IsNullOrEmpty(username))
            {
                valid = false;
                return;
            }

            SelectedCA = CA;
            SelectedTemplate = Template;

            keyFiles = new List<string>();

            GetUserDetails(username);
            hasUnrecoverableKeys = false;
            fullSuccess = true;
            recoveredKeys = 0;
            int count;
            RuntimeContext.Log.Info($"Current User: {username}");

            BLOBDirectory = $"{RuntimeContext.Conf.WorkingDirectory}\\{sAMAccountName}_BLOBs\\";
            KeyDirectory = $"{RuntimeContext.Conf.WorkingDirectory}\\{sAMAccountName}_Keys\\";
            MergedPFX = $"{RuntimeContext.Conf.WorkingDirectory}\\{sAMAccountName}.pfx";
            KeyRetrievalLocation = $"{RuntimeContext.Conf.DestinationDirectory}\\{sAMAccountName}.pfx";
            LegalDiscoveryKeyRetrievalLocation = $"{RuntimeContext.Conf.DiscoveryDirectory}\\{sAMAccountName}.pfx";

            if (RuntimeContext.Log.Level.GE(LogLevel.Verbose))
            {
                RuntimeContext.Log.Verbose($"SAMAccountName: {sAMAccountName}");
                RuntimeContext.Log.Verbose($"User Principal Name: {(Exists? PrincipalName : Constants.Unavailable)}");
                RuntimeContext.Log.Verbose($"Display Name: {(Exists ? DisplayName : Constants.Unavailable)}");
                RuntimeContext.Log.Verbose($"Email Address: {(Exists && HasEmailAddress ? Email : Constants.Unavailable) }");
                RuntimeContext.Log.Verbose($"BLOB Directory: {BLOBDirectory}");
                RuntimeContext.Log.Verbose($"Key Directory: {KeyDirectory}");
            }
            count = GetCertificates();
            switch (count)
            {
                case -1:
                    RuntimeContext.Log.Error($"Problem encountered enumerating certificates for user {sAMAccountName}");
                    valid = false;
                    return;

                case 0:
                    RuntimeContext.Log.Info($"No certificates with archived keys found for user {sAMAccountName}");
                    valid = true;
                    return;

                default:
                    RuntimeContext.Log.Info($"{count} certificates found with archived keys for user {sAMAccountName}");
                    valid = true;
                    return;
            }
        }

        private int GetCertificates()
        {
            Certificate crt;
            string command,
                   currentSN,
                   templateName,
                   templateOID;

            int index;

            RuntimeContext.Log.Info("Serial Numbers of Certificates for which to recover keys:");
            foreach (ADCertificateTemplate Template in SelectedCA.Templates.Where(p => p.RequiresPrivateKeyArchival))
            {
                index = 1;
                templateName = SelectedTemplate.DisplayName;
                templateOID = SelectedTemplate.Oid;

                command = $"certutil -config \"{SelectedCA.Config}\" -view -restrict \"UPN={PrincipalName},CertificateTemplate={templateOID}\" -out SerialNumber";
                var Output = Shell.Exec(command);
                foreach (string record in Output)
                {
                    try
                    {
                        if (record.Contains("Serial Number:"))
                        {
                            currentSN = record.Split(':')[1].OnlyHex();
                            if (!record.Contains(@"EMPTY"))
                            {
                                RuntimeContext.Log.Info($"Current serial number: {currentSN}");
                                crt = new Certificate(currentSN,
                                                      templateName,
                                                      SelectedCA.Config,
                                                      sAMAccountName,
                                                      BLOBDirectory,
                                                      KeyDirectory,
                                                      index);

                                certs.Add(crt);
                                index++;
                            }

                            else
                                RuntimeContext.Log.Error("This entry in the certificate database for user \"" + sAMAccountName + "\" has \"EMPTY\" listed as the serial number.  Ignoring.");
                        }
                    }
                    catch (Exception e)
                    {
                        RuntimeContext.Log.Exception(e, "An exception was encountered while enumerating certificates for user \"" + sAMAccountName + "\"");
                        fullSuccess = false;
                        return -1;
                    }
                }
            }
            return certs.Count;
        }

        public bool RecoverKeysFromCA(string password, bool bulkRecovery, bool eDiscovery)
        {
            string copyLocation;
            Folder.Create(BLOBDirectory, true);
            Folder.Create(KeyDirectory, true);
            foreach (Certificate crt in certs)
            {
                if (!(crt.RecoverKey(password)))
                    hasUnrecoverableKeys = true;
                else
                {
                    recoveredKeys++;
                    keyFiles.Add(crt.KeyFile);
                }
            }

            if (MergePFX(password))
            {
                KeysMerged = true;

                if (!bulkRecovery)
                {
                    if (!RuntimeContext.Conf.AttachToEmail)
                    {
                        RuntimeContext.Log.Verbose($"Copying merged PFX file \"{MergedPFX}\" to Key Retreival location: \"{KeyRetrievalLocation}\"");
                        try
                        {
                            copyLocation = eDiscovery
                                ? LegalDiscoveryKeyRetrievalLocation
                                : KeyRetrievalLocation;

                            File.Copy(MergedPFX, copyLocation, true);
                            if (!File.Exists(KeyRetrievalLocation))
                            {
                                RuntimeContext.Log.Error("A problem was encountered when copying the merged PFX file to the Key Retreival Location");
                                return false;
                            }
                        }

                        catch (Exception e)
                        {
                            RuntimeContext.Log.Exception(e, "A problem was encountered when copying the merged PFX file to the Key Retreival Location");
                            return false;
                        }
                    }
                }

                RuntimeContext.Log.Verbose($"Deleting key directory for user \"{sAMAccountName}\"");
                Folder.Delete(KeyDirectory);
            }

            RuntimeContext.Log.Verbose($"Deleting BLOB directory for user \"{sAMAccountName}\"");
            Folder.Delete(BLOBDirectory);
            if (hasUnrecoverableKeys)
                ReportUnrecoveredKeys();

            return fullSuccess;
        }

        private bool MergePFX(string password)
        {
            var KeyList = new List<string>();
            string command, sanitizedCommand;

            switch (recoveredKeys)
            {
                case 0:
                    break;
                case 1:
                    var recoveredCert = certs.First();
                    if (!File.Exists(recoveredCert.KeyFile))
                    {
                        RuntimeContext.Log.Error($"File not found \"{recoveredCert.KeyFile}\"");
                        return false;
                    }
                    File.Copy(recoveredCert.KeyFile, MergedPFX);
                    break;
                default:
                    certs.Where(p => p.recovered)
                     .ForEach(q => KeyList.Add(q.KeyFile));

                    RuntimeContext.Log.Info($"Attempting to merge PFX files for user \"{sAMAccountName}\"");
                    command = $"certutil -p \"{password},{password}\" -mergepfx -user \"{string.Join(@",", KeyList)}\" \"{MergedPFX}\"";
                    sanitizedCommand = command.Replace(password, "[password]");
                    Shell.Exec(command, sanitizedCommand);
                    HasMergedPfx = true;
                    break;
            }

            if (File.Exists(MergedPFX))
            {
                RuntimeContext.Log.Info($"Successfully created merged PFX file \"{MergedPFX}\"");
                return true;
            }
            else
            {
                RuntimeContext.Log.Error("Could not merge PFX files");
                return false;
            }
        }

        private void ReportUnrecoveredKeys()
        {
            RuntimeContext.Log.Warning($"There were unrecovered keys for {sAMAccountName}");
            certs.Where(p => !p.recovered)
                 .ForEach(q => q.Report());
        }

        public void CleanUp(bool isBulkRecovery)
        {
            RuntimeContext.Log.Info($"Removing temporary files and folders created during key recovery for \"{sAMAccountName}\"");

            RuntimeContext.Log.Verbose($"Attempting to delete folder \"{BLOBDirectory}\"");
            if (!Folder.Delete(BLOBDirectory))
            {
                RuntimeContext.Log.Warning($"Unabled to delete folder \"{BLOBDirectory}\"");
            }

            RuntimeContext.Log.Verbose($"Attempting to delete folder \"{KeyDirectory}\"");
            if (!Folder.Delete(BLOBDirectory))
            {
                RuntimeContext.Log.Warning($"Unabled to delete folder \"{KeyDirectory}\"");
            }

            if (!isBulkRecovery && RuntimeContext.Conf.DeleteKeyAfterSending)
            {
                RuntimeContext.Log.Verbose($"Attempting to delete file \"{MergedPFX}\"");
                if (!stdlib.DeleteFile(MergedPFX))
                {
                    RuntimeContext.Log.Warning($"Unabled to delete file \"{MergedPFX}\"");
                }
                else
                {
                    RuntimeContext.Log.Verbose($"Attempting to delete working directory \"{RuntimeContext.Conf.WorkingDirectory}\"");
                    if (!Folder.Delete(RuntimeContext.Conf.WorkingDirectory))
                    {
                        RuntimeContext.Log.Warning("Unabled to delete working directory");
                    }
                }
            }
        }

        private void GetUserDetails(string sAMAccountName)
        {
            this.sAMAccountName = sAMAccountName;

            foreach(string domain in Domains)
            {
                using (var DomainContext = new PrincipalContext(ContextType.Domain, domain))
                using (var Result = UserPrincipal.FindByIdentity(DomainContext, IdentityType.SamAccountName, sAMAccountName))
                {
                    if (null != Result)
                    {
                        Exists = true;
                        this.sAMAccountName = Result.SamAccountName;
                        try
                        {
                            Email = Result.EmailAddress;
                            HasEmailAddress = true;
                        }
                        catch
                        {
                            //No Email
                        }
                        PrincipalName = Result.UserPrincipalName;
                        DisplayName = Result.DisplayName;

                        try
                        {
                            enabled = (bool)Result.Enabled;
                        }
                        catch
                        {
                            enabled = false;
                        }
                        break;
                    }
                }
            }
        }

    } //class User
}
