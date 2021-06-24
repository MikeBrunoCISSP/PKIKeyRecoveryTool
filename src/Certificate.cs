using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MJBLogger;
using MoreLinq;

namespace PKIKeyRecovery
{
    public class Certificate
    {
        string BLOBFile,
               CAConfig;

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
                           int i)
        {
            Template = tmpl;
            SerialNumber = sn;
            CAConfig = certificateAuthority;
            index = i;

            BLOBFile = $"{BLOBDirectory}{username}_{Template}({index}).BLOB";
            KeyFile = $"{keyDirectory}{username}_{Template}({index}).pfx";

            recovered = false;
            RuntimeContext.Log.PropertyReport(this, @"Certificate details", level: LogLevel.Verbose);
        }

        public bool RecoverKey(string password)
        {
            if (MakeBLOB())
            {
                string[] output;
                string command = $"certutil -recoverkey -p {password} \"{BLOBFile}\" \"{KeyFile}\"";
                output = Shell.Exec(command, command.Replace(password, "[password]"));
                if (File.Exists(KeyFile))
                {
                    RuntimeContext.Log.Info($"Key successfully recovered for {Template} certificate {SerialNumber} as {KeyFile}");
                    recovered = true;
                }
                else
                {
                    RuntimeContext.Log.Error($"Key could not be recovered for {Template} certificate {SerialNumber}");
                }
            }
            return recovered;
        } //recoverKey

        internal void Report()
        {
            RuntimeContext.Log.Echo($"       Serial Number: {SerialNumber}");
            RuntimeContext.Log.Echo($"Certificate Template: {Template}");
        }

        private bool MakeBLOB()
        {
            string command = $"certutil -config \"{CAConfig}\" -getkey {SerialNumber} \"{BLOBFile}\"";
            Shell.Exec(command);

            if (File.Exists(BLOBFile))
            {
                RuntimeContext.Log.Info($"BLOB for {Template} certificate {SerialNumber} saved as {BLOBFile} from CA {CAConfig}");
                return true;
            }
            else
            {
                RuntimeContext.Log.Error($"Unable to retreive BLOB for {Template} certificate {SerialNumber} from CA {CAConfig}");
                return false;
            }
        }

    } //class Certificate
}
