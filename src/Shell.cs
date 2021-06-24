using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using MJBLogger;
using MoreLinq;

namespace PKIKeyRecovery
{
    class Shell
    {
        const int DEBUG = 4;
        const int INFO = 3;
        const int WARNING = 2;
        const int ERROR = 1;
        const int CRITICAL = 0;


        public static string[] Exec(string command, string sanitizedCommand = null)
        {
            string sCommand = string.IsNullOrEmpty(sanitizedCommand)
                ? command
                : sanitizedCommand;

            return ExecuteCommand(command, sCommand);
        }

        private static string[] ExecuteCommand(string command, string sanitizedCommand = null)
        {
            RuntimeContext.Log.Verbose($"Command: \"{(string.IsNullOrEmpty(sanitizedCommand) ? command : sanitizedCommand)}\"");

            try
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();

                string[] result = Regex.Split(output, "\r\n");
                if (RuntimeContext.Log.Level.GE(LogLevel.Verbose) && !string.IsNullOrEmpty(output))
                {
                    RuntimeContext.Log.Verbose(@"Command Result: ");
                    result.ForEach(p => RuntimeContext.Log.Echo(p, level: LogLevel.Verbose));
                }
                return result;
            }

            catch (Exception ex)
            {
                RuntimeContext.Log.Exception(ex);
                return null;
            }
        }
    }
}

