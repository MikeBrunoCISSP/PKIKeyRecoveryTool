using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;


class Shell
{
    const int DEBUG = 4;
    const int INFO = 3;
    const int WARNING = 2;
    const int ERROR = 1;
    const int CRITICAL = 0;

    public static string[] exec(string command)
    {
        string output = ExecuteCommand(command);

        if (output != null)
            return output.Split('\n');
        else
            return null;
    }
    
    public static string[] exec(string command, string sanitizedCommand, Log log)
    {
        log.write(Log._DEBUG, "Command: \"" + sanitizedCommand + "\"", false);
        string output = ExecuteCommand(command);

        if (output != null)
        {
            string[] lines = output.Split('\n');
            if (log.get_level() == Log._DEBUG)
            {
                log.write(Log._DEBUG, "Command Result:", false);
                foreach (string line in lines)
                    log.echo(line, true);
            }
            return lines;
        }
        else
            return null;
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

    /*public static bool executeAndVerify(string command, string sanitizedCommand, string successIndicator, int requiredInstancesOfSuccessIndicator, Log log)
    {
        log.write(4, "Command : " + sanitizedCommand, false);
        string output = ExecuteCommand(command);
        log.write(4, "Command Result:", false);
        log.write(4, output, false);
        return (stdlib.instancesOfSubString(output, successIndicator) == requiredInstancesOfSuccessIndicator);
    } */

    public static string executeAndLog(string command, string sanitizedCommand, Log log)
    {
        int logLevel = log.get_level();
        log.write(DEBUG, "Command : " + sanitizedCommand, false);
        string output = ExecuteCommand(command);
        log.write(DEBUG, "Command Result:", false);

        if (log.get_level() == Log._DEBUG)
        {
            string[] lines = Regex.Split(output, "\r\n");
            foreach (string line in lines)
            {
                log.echo(line, true);
            }
        }

        return output;
    }

    public static bool executeAndVerify(string command, string sanitizedCommand, string successIndicator, int requiredInstancesOfSuccessIndicator, Log log)
    {
        log.write(DEBUG, "Command : " + sanitizedCommand, false);
        string output = ExecuteCommand(command);
        log.write(DEBUG, "Command Result:", false);

        if (log.get_level() == Log._DEBUG)
        {
            string[] lines = Regex.Split(output, "\r\n");
            foreach (string line in lines)
            {
                log.echo(line, true);
            }
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

