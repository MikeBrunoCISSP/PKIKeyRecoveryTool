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

        public static string[] Exec(string command)
        {
            string output = ExecuteCommand(command);

            if (output != null)
                return output.Split('\n');
            else
                return null;
        }

        public static string[] Exec(string command, string sanitizedCommand)
        {
            RuntimeContext.Log.Verbose($"Command: \"{sanitizedCommand}\"");
            string output = ExecuteCommand(command);

            if (output != null)
            {
                string[] lines = output.Split('\n');
                if (RuntimeContext.Log.Level.GE(LogLevel.Verbose))
                {
                    RuntimeContext.Log.Verbose("Command Result:");
                    lines.ForEach(line => RuntimeContext.Log.Echo(line));
                }
                return lines;
            }
            else
            {
                return null;
            }
        }

        public static bool executeAndVerify(string command, string successIndicator)
        {
            string output = ExecuteCommand(command);
            return stdlib.InString(output, successIndicator);
        }

        public static bool executeAndVerify(string command, string successIndicator, int requiredInstancesOfSuccessIndicator)
        {
            string output = ExecuteCommand(command);
            return (stdlib.instancesOfSubString(output, successIndicator) == requiredInstancesOfSuccessIndicator);
        }

        public static string ExecuteAndLog(string command, string sanitizedCommand)
        {
            RuntimeContext.Log.Verbose($"Command : {sanitizedCommand}");
            string output = ExecuteCommand(command);

            if (RuntimeContext.Log.Level.GE(LogLevel.Verbose))
            {
                RuntimeContext.Log.Verbose("Command Result:");

                Regex.Split(output, "\r\n")
                     .ForEach(line => RuntimeContext.Log.Echo(line));
            }

            return output;
        }

        public static bool executeAndVerify(string command, string sanitizedCommand, string successIndicator, int requiredInstancesOfSuccessIndicator)
        {
            RuntimeContext.Log.Verbose($"Command : {sanitizedCommand}");
            string output = ExecuteCommand(command);

            if (RuntimeContext.Log.Level.GE(LogLevel.Verbose))
            {
                RuntimeContext.Log.Verbose("Command Result:");

                Regex.Split(output, "\r\n")
                     .ForEach(line => RuntimeContext.Log.Echo(line));
            }

            return (stdlib.instancesOfSubString(output, successIndicator) == requiredInstancesOfSuccessIndicator);
        }

        private static string ExecuteCommand(string command)
        {
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
                return (output);
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}

