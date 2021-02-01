using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
        Log log;

        public Certificate(string sn, 
                           string tmpl, 
                           string certificateAuthority, 
                           string username, 
                           string BLOBDirectory, 
                           string keyDirectory, 
                           int i, 
                           Log l)
        {
            template = tmpl;
            serialNumber = sn;
            CA = certificateAuthority;
            index = i;
            log = l;

            BLOBFile = BLOBDirectory + username + "_" + template + "(" + index + ").BLOB";
            keyFile = keyDirectory + username + "_" + template + "(" + index + ").pfx";

            recovered = false;

            if (log.get_level() >= Log._DEBUG)
            {
                log.write(Log._DEBUG, "Serial Number: " + serialNumber, false);
                log.write(Log._DEBUG, "Certificate Template Name: " + template, false);
                log.write(Log._DEBUG, "Index: " + index, false);
                log.write(Log._DEBUG, "BLOB File Name: " + BLOBFile, false);
                log.write(Log._DEBUG, "Key File Name: " + keyFile, false);
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
                    log.write(Log._INFO, "Key successfully recovered for " + template + " certificate " + serialNumber + " as " + keyFile, false);
                    recovered = true;
                }
                else
                {
                    log.write(Log._ERROR, "Key could not be recovered for " + template + " certificate " + serialNumber, false);
                    if (log.get_level() >= Log._ERROR)
                    {
                        log.echo("Result of command:");
                        try
                        {
                            foreach (string line in output)
                                log.echo(line.Replace(password, "[password]"), true);
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
                log.write(Log._INFO, "BLOB for " + template + " certificate " + serialNumber + " saved as " + BLOBFile, false);
                return true;
            }
            else
            {
                log.write(Log._ERROR, "Unable to retreive BLOB for " + template + " certificate " + serialNumber, false);
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
