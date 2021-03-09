using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MJBLogger;

namespace PKIKeyRecovery
{
    public class User
    {
        Configuration conf;

        public string mergedPFX,
                      keyDirectory,
                      keyRetrievalLocation,
                      legalDiscoveryKeyRetrievalLocation;
        string SAM,
               UPN,
               CN,
               Email,
               BLOBDirectory;

        List<Certificate> certs;

        public List<string> keyFiles;

        public bool valid,
                    keysMerged,
                    existsAndNotDisabled,
                    fullSuccess;
        bool hasArchivedCerts,
             hasUnrecoverableKeys;

        int recoveredKeys;

        ActiveDirectoryExplorer ade;

        //public User(string username,  Configuration config)
        //{
        //    conf = config;
        //    if (username == null)
        //    {
        //        valid = false;
        //        return;
        //    }

        //    ade = new ActiveDirectoryExplorer(conf.ADDS_Domain, conf.ADDS_ContainerDN);
        //    keyFiles = new List<string>();

        //    existsAndNotDisabled = ade.UserExists(username, true);
        //    hasArchivedCerts = false;
        //    hasUnrecoverableKeys = false;
        //    keysMerged = false;
        //    fullSuccess = true;
        //    recoveredKeys = 0;
        //    int count;
        //    conf.Log.Info("Current User: " + username);

        //    SAM = username;
        //    UPN = SAM + "@" + conf.ADDS_Domain;
        //    CN = ade.GetCN(SAM);
        //    Email = ade.GetEmail(SAM);
        //    BLOBDirectory = conf.workingDirectory + SAM + "_BLOBs\\";
        //    keyDirectory = conf.workingDirectory + SAM + "_Keys\\";
        //    mergedPFX = conf.workingDirectory + SAM + ".pfx";
        //    keyRetrievalLocation = conf.PC_KeyRetrievalLocation + SAM + ".pfx";
        //    legalDiscoveryKeyRetrievalLocation = conf.Legal_KeyRetrievalLocation + SAM + ".pfx";

        //    if (conf.Log.Level.GE(LogLevel.Verbose))
        //    {
        //        conf.Log.Verbose("SAMAccountName: " + SAM);
        //        conf.Log.Verbose("User Principal Name: " + UPN);
        //        conf.Log.Verbose("Common Name: " + CN);
        //        conf.Log.Verbose("Email Address: " + Email);
        //        conf.Log.Verbose("BLOB Directory: " + BLOBDirectory);
        //        conf.Log.Verbose("Key Directory: " + keyDirectory);
        //    }
        //    count = getCertificates();
        //    switch (count)
        //    {
        //        case -1:
        //            conf.Log.Error("Problem encountered enumerating certificates for user " + SAM);
        //            valid = false;
        //            return;

        //        case 0:
        //            conf.Log.Info("No certificates with archived keys found for user " + SAM);
        //            valid = true;
        //            return;

        //        default:
        //            conf.Log.Info(count.ToString() + " certificates found with archived keys for user " + SAM);
        //            hasArchivedCerts = true;
        //            valid = true;
        //            return;
        //    }
        //}

        //private int getCertificates()
        //{
        //    Certificate crt;
        //    string command,
        //           currentSN,
        //           templateName,
        //           templateOID;

        //    int index;
        //    int count = 0;
        //    certs = new List<Certificate>();

        //    string[] tmp;

        //    conf.Log.Info("Serial Numbers of Certificates for which to recover keys:");
        //    foreach (KeyValuePair<string,string> template in conf.templates)
        //    {
        //        index = 1;
        //        templateName = template.Key;
        //        templateOID = template.Value;

        //        command = "certutil -config " + conf.CA + " -view -restrict " + "\"UPN=" + UPN + ",CertificateTemplate=" + templateOID + "\" -out SerialNumber";
        //        Array SNs = Shell.exec(command, command, conf.Log);
        //        foreach (string record in SNs)
        //        {
        //            try
        //            {
        //                if (stdlib.InString(record, "Serial Number:"))
        //                {
        //                    count++;
        //                    currentSN = stdlib.clean(stdlib.Split(record, ':', 1));
        //                    if (!(String.Equals(currentSN, "EMPTY")))
        //                    {
        //                        conf.Log.Info("     " + currentSN);
        //                        crt = new Certificate(currentSN,
        //                                              templateName,
        //                                              conf.CA,
        //                                              SAM,
        //                                              BLOBDirectory,
        //                                              keyDirectory,
        //                                              index,
        //                                              conf.Log);

        //                        certs.Add(crt);
        //                        index++;
        //                    }

        //                    else
        //                        conf.Log.Error("This entry in the certificate database for user \"" + SAM + "\" has \"EMPTY\" listed as the serial number.  Ignoring.");
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                conf.Log.Exception(e, "An exception was encountered while enumerating certificates for user \"" + SAM + "\"");
        //                fullSuccess = false;
        //                return -1;
        //            }
        //        }
        //    }
        //    return count;
        //} //getCertificates

        //public bool recoverKeysFromCA(string password, bool bulkRecovery, bool eDiscovery)
        //{
        //    string copyLocation;
        //    Folder.Create(BLOBDirectory, true);
        //    Folder.Create(keyDirectory, true);
        //    foreach (Certificate crt in certs)
        //    {
        //        if (!(crt.recoverKey(password)))
        //            hasUnrecoverableKeys = true;
        //        else
        //        {
        //            recoveredKeys++;
        //            keyFiles.Add(crt.keyFile);
        //        }
        //    }

        //    if (mergePFX(password))
        //    {
        //        keysMerged = true;

        //        if (!bulkRecovery)
        //        {
        //            if (!conf.PC_AttachKeyToEmail & conf.pc_keyRetrievalLocationDefined)
        //            {
        //                conf.Log.Verbose("Copying merged PFX file \"" + mergedPFX + "\" to Key Retreival location: \"" + keyRetrievalLocation + "\"");
        //                try
        //                {
        //                    if (eDiscovery)
        //                        copyLocation = legalDiscoveryKeyRetrievalLocation;
        //                    else
        //                        copyLocation = keyRetrievalLocation;

        //                    File.Copy(mergedPFX, copyLocation, true);
        //                    if (!File.Exists(keyRetrievalLocation))
        //                    {
        //                        conf.Log.Error("A problem was encountered when copying the merged PFX file to the Key Retreival Location");
        //                        return false;
        //                    }
        //                }

        //                catch (Exception e)
        //                {
        //                    conf.Log.Exception(e, "A problem was encountered when copying the merged PFX file to the Key Retreival Location");
        //                    return false;
        //                }
        //            }
        //        }

        //        conf.Log.Verbose("Deleting key directory for user \"" + SAM + "\"");
        //        Folder.Delete(keyDirectory);
        //    }

        //    conf.Log.Verbose("Deleting BLOB directory for user \"" + SAM + "\"");
        //    Folder.Delete(BLOBDirectory);
        //    if (hasUnrecoverableKeys)
        //        reportUnrecoveredKeys();

        //    return fullSuccess;
        //}

        //private bool mergePFX(string password)
        //{
        //    string keyList = String.Empty;
        //    string command, sanitizedCommand;
        //    if (recoveredKeys > 1)
        //    {
        //        #region Create_keyList
        //        foreach (Certificate c in certs)
        //        {
        //            if (c.recovered)
        //            {
        //                if (keyList == String.Empty)
        //                    keyList += "\"" + c.getKeyFile();
        //                else
        //                    keyList += "," + c.getKeyFile();
        //            }
        //        }
        //        keyList += "\"";
        //        #endregion

        //        conf.Log.Info("Attempting to merge PFX files for user \"" + SAM + "\"");
        //        command = "certutil -p \"" + password + "," + password + "\" -mergepfx -user " + keyList + " \"" + mergedPFX + "\"";
        //        sanitizedCommand = command.Replace(password, "[password]");
        //        Shell.executeAndLog(command, sanitizedCommand, conf.Log);
        //    }

        //    else
        //    {
        //        if (recoveredKeys == 1)
        //        {
        //            foreach (Certificate d in certs)
        //            {
        //                if (!File.Exists(d.getKeyFile()))
        //                {
        //                    conf.Log.Error("File not found \"" + d.getKeyFile() + "\"");
        //                    return false;
        //                }
        //                File.Copy(d.getKeyFile(), mergedPFX);
        //            }
        //        }

        //        else
        //        {
        //            conf.Log.Error("No keys could be recovered for user \"" + SAM + "\"");
        //            return false;
        //        }
        //    }

        //    if (File.Exists(mergedPFX))
        //    {
        //        conf.Log.Info("Successfully created merged PFX file \"" + mergedPFX + "\"");
        //        return true;
        //    }
        //    else
        //    {
        //        conf.Log.Error("Could not merge PFX files");
        //        return false;
        //    }
        //}

        //private void reportUnrecoveredKeys()
        //{
        //    conf.Log.Error("There were unrecovered keys for " + SAM);
        //    foreach (Certificate crt in certs)
        //    {
        //        if (!crt.recovered)
        //            conf.Log.Info("     Serial Number: " + crt.getSerialNumber() + "     Certificate Template: " + crt.getTemplate());
        //    }
        //}

        //public string getCN()
        //{
        //    return CN;
        //}

        //public string getEmail()
        //{
        //    return Email;
        //}

        //public string getSAM()
        //{
        //    return SAM;
        //}

        //public bool HasArchivedCerts()
        //{
        //    return hasArchivedCerts;
        //}

        //public bool hasMergedPFX()
        //{
        //    return keysMerged;
        //}

        //public bool AnyKeysRecovered()
        //{
        //    if (recoveredKeys > 0)
        //        return true;
        //    else
        //        return false;
        //}

        //public bool exists()
        //{
        //    return ade.UserExists(SAM, false);
        //}

        //public string firstName()
        //{
        //    return ade.GetFirstName(SAM);
        //}

        //public void cleanUp(bool isBulkRecovery)
        //{
        //    conf.Log.Info("Removing temporary files and folders created during key recovery for \"" + SAM + "\"");

        //    conf.Log.Verbose("Attempting to delete folder \"" + BLOBDirectory + "\"");
        //    if (!Folder.Delete(BLOBDirectory))
        //        conf.Log.Warning("Unabled to delete folder \"" + BLOBDirectory + "\"");

        //    conf.Log.Verbose("Attempting to delete folder \"" + keyDirectory + "\"");
        //    if (!Folder.Delete(BLOBDirectory))
        //        conf.Log.Warning("Unabled to delete folder \"" + keyDirectory + "\"");

        //    if (!isBulkRecovery & conf.PC_DeleteKeyAfterSending)
        //    {
        //        conf.Log.Verbose("Attempting to delete file \"" + mergedPFX + "\"");
        //        if (!stdlib.DeleteFile(mergedPFX, conf.Log))
        //            conf.Log.Warning("Unabled to delete file \"" + mergedPFX + "\"");
        //        else
        //        {
        //            conf.Log.Verbose("Attempting to delete working directory \"" + conf.workingDirectory + "\"");
        //            if (!Folder.Delete(conf.workingDirectory))
        //                conf.Log.Warning("Unabled to delete working directory");
        //        }
        //    }
        //}

    } //class User
}
