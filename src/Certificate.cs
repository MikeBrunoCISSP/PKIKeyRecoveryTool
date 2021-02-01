using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MJBLogger;

namespace PKIKeyRecovery
{
    public class Certificate
    {
        string BLOBFile,
               template,
               serialNumber,
               CA;

        public string keyFile;

        int index;
        public bool recovered;
        MJBLog log;

        public Certificate(string sn, 
                           string tmpl, 
                           string certificateAuthority, 
                           string username, 
                           string BLOBDirectory, 
                           string keyDirectory, 
                           int i, 
                           MJBLog l)
        {
            template = tmpl;
            serialNumber = sn;
            CA = certificateAuthority;
            index = i;
            log = l;

            BLOBFile = BLOBDirectory + username + "_" + template + "(" + index + ").BLOB";
            keyFile = keyDirectory + username + "_" + template + "(" + index + ").pfx";

            recovered = false;

            if (log.Level.GE(LogLevel.Verbose))
            {
                log.Verbose("Serial Number: " + serialNumber);
                log.Verbose("Certificate Template Name: " + template);
                log.Verbose("Index: " + index);
                log.Verbose("BLOB File Name: " + BLOBFile);
                log.Verbose("Key File Name: " + keyFile);
            }
        }

        public bool recoverKey(string password)
        {
            if (makeBLOB())
            {
                string[] output;
                string command = "certutil -recoverkey -p " + password + " \"" + BLOBFile + "\" \"" + keyFile + "\"";
                output = Shell.exec(command, command.Replace(password, "[password]"), log);
                if (File.Exists(keyFile))
                {
                    log.Info("Key successfully recovered for " + template + " certificate " + serialNumber + " as " + keyFile);
                    recovered = true;
                }
                else
                {
                    log.Error("Key could not be recovered for " + template + " certificate " + serialNumber);
                    if (log.Level.GE(LogLevel.Error))
                    {
                        log.Echo("Result of command:");
                        try
                        {
                            foreach (string line in output)
                                log.Echo(line.Replace(password, "[password]"));
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
            Shell.exec(command, command, log);

            if (File.Exists(BLOBFile))
            {
                log.Info("BLOB for " + template + " certificate " + serialNumber + " saved as " + BLOBFile);
                return true;
            }
            else
            {
                log.Error("Unable to retreive BLOB for " + template + " certificate " + serialNumber);
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

        public string getKeyFile()
        {
            return keyFile;
        }
    } //class Certificate
}
