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
               CA;

        public string KeyFile { get; private set; }
        public string SerialNumber { get; private set; }
        public string Template { get; private set; }

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
            Template = tmpl;
            SerialNumber = sn;
            CA = certificateAuthority;
            index = i;
            log = l;

            BLOBFile = BLOBDirectory + username + "_" + Template + "(" + index + ").BLOB";
            KeyFile = keyDirectory + username + "_" + Template + "(" + index + ").pfx";

            recovered = false;

            if (log.Level.GE(LogLevel.Verbose))
            {
                log.Verbose("Serial Number: " + SerialNumber);
                log.Verbose("Certificate Template Name: " + Template);
                log.Verbose("Index: " + index);
                log.Verbose("BLOB File Name: " + BLOBFile);
                log.Verbose("Key File Name: " + KeyFile);
            }
        }

        public bool recoverKey(string password)
        {
            if (makeBLOB())
            {
                string[] output;
                string command = "certutil -recoverkey -p " + password + " \"" + BLOBFile + "\" \"" + KeyFile + "\"";
                output = Shell.exec(command, command.Replace(password, "[password]"), log);
                if (File.Exists(KeyFile))
                {
                    log.Info("Key successfully recovered for " + Template + " certificate " + SerialNumber + " as " + KeyFile);
                    recovered = true;
                }
                else
                {
                    log.Error("Key could not be recovered for " + Template + " certificate " + SerialNumber);
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

        internal void Report()
        {
            RuntimeContext.Log.Echo($"       Serial Number: {SerialNumber}");
            RuntimeContext.Log.Echo($"Certificate Template: {Template}");
        }

        private bool makeBLOB()
        {
            string command = "certutil -config " + CA + " -getkey " + SerialNumber + " " + "\"" + BLOBFile + "\"";
            Shell.exec(command, command, log);

            if (File.Exists(BLOBFile))
            {
                log.Info("BLOB for " + Template + " certificate " + SerialNumber + " saved as " + BLOBFile);
                return true;
            }
            else
            {
                log.Error("Unable to retreive BLOB for " + Template + " certificate " + SerialNumber);
                return false;
            }
        }

    } //class Certificate
}
