using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

        public User(string username,  Configuration config)
        {
            conf = config;
            if (username == null)
            {
                valid = false;
                return;
            }

            ade = new ActiveDirectoryExplorer(conf.ADdomain, conf.ADscopeDN);
            keyFiles = new List<string>();

            existsAndNotDisabled = ade.UserExists(username, true);
            hasArchivedCerts = false;
            hasUnrecoverableKeys = false;
            keysMerged = false;
            fullSuccess = true;
            recoveredKeys = 0;
            int count;
            conf.log.write(Log._INFO, "Current User: " + username, false);

            SAM = username;
            UPN = SAM + "@" + conf.ADdomain;
            CN = ade.GetCN(SAM);
            Email = ade.GetEmail(SAM);
            BLOBDirectory = conf.workingDirectory + SAM + "_BLOBs\\";
            keyDirectory = conf.workingDirectory + SAM + "_Keys\\";
            mergedPFX = conf.workingDirectory + SAM + ".pfx";
            keyRetrievalLocation = conf.pc_keyRetrievalLocation + SAM + ".pfx";
            legalDiscoveryKeyRetrievalLocation = conf.legal_keyRetrievalLocation + SAM + ".pfx";

            if (conf.log.get_level() >= Log._DEBUG)
            {
                conf.log.write(Log._DEBUG, "SAMAccountName: " + SAM, false);
                conf.log.write(Log._DEBUG, "User Principal Name: " + UPN, false);
                conf.log.write(Log._DEBUG, "Common Name: " + CN, false);
                conf.log.write(Log._DEBUG, "Email Address: " + Email, false);
                conf.log.write(Log._DEBUG, "BLOB Directory: " + BLOBDirectory, false);
                conf.log.write(Log._DEBUG, "Key Directory: " + keyDirectory, false);
            }
            count = getCertificates();
            switch (count)
            {
                case -1:
                    conf.log.write(Log._ERROR, "Problem encountered enumerating certificates for user " + SAM, false);
                    valid = false;
                    return;

                case 0:
                    conf.log.write(Log._INFO, "No certificates with archived keys found for user " + SAM, false);
                    valid = true;
                    return;

                default:
                    conf.log.write(Log._INFO, count.ToString() + " certificates found with archived keys for user " + SAM, false);
                    hasArchivedCerts = true;
                    valid = true;
                    return;
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

            conf.log.write(Log._INFO, "Serial Numbers of Certificates for which to recover keys:", false);
            foreach (string template in conf.templates)
            {
                index = 1;
                tmp = template.Split(',');
                templateName = tmp[0];
                templateOID = tmp[1];

                command = "certutil -config " + conf.CA + " -view -restrict " + "\"UPN=" + UPN + ",CertificateTemplate=" + templateOID + "\" -out SerialNumber";
                Array SNs = Shell.exec(command, command, conf.log);
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
                                conf.log.write(Log._INFO, "     " + currentSN, false);
                                crt = new Certificate(currentSN,
                                                      templateName,
                                                      conf.CA,
                                                      SAM,
                                                      BLOBDirectory,
                                                      keyDirectory,
                                                      index,
                                                      conf.log);

                                certs.Add(crt);
                                index++;
                            }

                            else
                                conf.log.write(Log._ERROR, "This entry in the certificate database for user \"" + SAM + "\" has \"EMPTY\" listed as the serial number.  Ignoring.", false);
                        }
                    }
                    catch (Exception e)
                    {
                        conf.log.exception(Log._CRITICAL, "An exception was encountered while enumerating certificates for user \"" + SAM + "\"", e, false);
                        fullSuccess = false;
                        return -1;
                    }
                }
            }
            return count;
        } //getCertificates

        public bool recoverKeysFromCA(string password, bool bulkRecovery, bool eDiscovery)
        {
            string copyLocation;
            Folder.Create(BLOBDirectory, true);
            Folder.Create(keyDirectory, true);
            foreach (Certificate crt in certs)
            {
                if (!(crt.recoverKey(password)))
                    hasUnrecoverableKeys = true;
                else
                {
                    recoveredKeys++;
                    keyFiles.Add(crt.keyFile);
                }
            }

            if (mergePFX(password))
            {
                keysMerged = true;

                if (!bulkRecovery)
                {
                    if (!conf.pc_attachKeyToEmail & conf.pc_keyRetrievalLocationDefined)
                    {
                        conf.log.write(Log._DEBUG, "Copying merged PFX file \"" + mergedPFX + "\" to Key Retreival location: \"" + keyRetrievalLocation + "\"", false);
                        try
                        {
                            if (eDiscovery)
                                copyLocation = legalDiscoveryKeyRetrievalLocation;
                            else
                                copyLocation = keyRetrievalLocation;

                            File.Copy(mergedPFX, copyLocation, true);
                            if (!File.Exists(keyRetrievalLocation))
                            {
                                conf.log.write(Log._ERROR, "A problem was encountered when copying the merged PFX file to the Key Retreival Location", false);
                                return false;
                            }
                        }

                        catch (Exception e)
                        {
                            conf.log.exception(Log._ERROR, "A problem was encountered when copying the merged PFX file to the Key Retreival Location", e, false);
                            return false;
                        }
                    }
                }

                conf.log.write(Log._DEBUG, "Deleting key directory for user \"" + SAM + "\"", false);
                Folder.Delete(keyDirectory);
            }

            conf.log.write(Log._DEBUG, "Deleting BLOB directory for user \"" + SAM + "\"", false);
            Folder.Delete(BLOBDirectory);
            if (hasUnrecoverableKeys)
                reportUnrecoveredKeys();

            return fullSuccess;
        }

        private bool mergePFX(string password)
        {
            string keyList = String.Empty;
            string command, sanitizedCommand;
            if (recoveredKeys > 1)
            {
                #region Create_keyList
                foreach (Certificate c in certs)
                {
                    if (c.recovered)
                    {
                        if (keyList == String.Empty)
                            keyList += "\"" + c.getKeyFile();
                        else
                            keyList += "," + c.getKeyFile();
                    }
                }
                keyList += "\"";
                #endregion

                conf.log.write(Log._INFO, "Attempting to merge PFX files for user \"" + SAM + "\"", false);
                command = "certutil -p \"" + password + "," + password + "\" -mergepfx -user " + keyList + " \"" + mergedPFX + "\"";
                sanitizedCommand = command.Replace(password, "[password]");
                Shell.executeAndLog(command, sanitizedCommand, conf.log);
            }

            else
            {
                if (recoveredKeys == 1)
                {
                    foreach (Certificate d in certs)
                    {
                        if (!File.Exists(d.getKeyFile()))
                        {
                            conf.log.write(Log._ERROR, "File not found \"" + d.getKeyFile() + "\"", false);
                            return false;
                        }
                        File.Copy(d.getKeyFile(), mergedPFX);
                    }
                }

                else
                {
                    conf.log.write(Log._ERROR, "No keys could be recovered for user \"" + SAM + "\"", false);
                    return false;
                }
            }

            if (File.Exists(mergedPFX))
            {
                conf.log.write(Log._INFO, "Successfully created merged PFX file \"" + mergedPFX + "\"", false);
                return true;
            }
            else
            {
                conf.log.write(Log._ERROR, "Could not merge PFX files", false);
                return false;
            }
        }

        private void reportUnrecoveredKeys()
        {
            conf.log.write(Log._ERROR, "There were unrecovered keys for " + SAM, true);
            foreach (Certificate crt in certs)
            {
                if (!crt.recovered)
                    conf.log.write(Log._INFO, "     Serial Number: " + crt.getSerialNumber() + "     Certificate Template: " + crt.getTemplate(), true);
            }
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

        public bool hasMergedPFX()
        {
            return keysMerged;
        }

        public bool AnyKeysRecovered()
        {
            if (recoveredKeys > 0)
                return true;
            else
                return false;
        }

        public bool exists()
        {
            return ade.UserExists(SAM, false);
        }

        public string firstName()
        {
            return ade.GetFirstName(SAM);
        }

        public void cleanUp(bool isBulkRecovery)
        {
            conf.log.write(Log._INFO, "Removing temporary files and folders created during key recovery for \"" + SAM + "\"", false);

            conf.log.write(Log._DEBUG, "Attempting to delete folder \"" + BLOBDirectory + "\"", false);
            if (!Folder.Delete(BLOBDirectory))
                conf.log.write(Log._WARNING, "Unabled to delete folder \"" + BLOBDirectory + "\"", false);

            conf.log.write(Log._DEBUG, "Attempting to delete folder \"" + keyDirectory + "\"", false);
            if (!Folder.Delete(BLOBDirectory))
                conf.log.write(Log._WARNING, "Unabled to delete folder \"" + keyDirectory + "\"", false);

            if (!isBulkRecovery & conf.pc_deleteKeyAfterSending)
            {
                conf.log.write(Log._DEBUG, "Attempting to delete file \"" + mergedPFX + "\"", false);
                if (!stdlib.DeleteFile(mergedPFX, conf.log))
                    conf.log.write(Log._WARNING, "Unabled to delete file \"" + mergedPFX + "\"", false);
                else
                {
                    conf.log.write(Log._DEBUG, "Attempting to delete working directory \"" + conf.workingDirectory + "\"", false);
                    if (!Folder.Delete(conf.workingDirectory))
                        conf.log.write(Log._WARNING, "Unabled to delete working directory", false);
                }
            }
        }

    } //class User
}
