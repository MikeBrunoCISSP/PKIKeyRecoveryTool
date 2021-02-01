using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using MJBLogger;


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
    
    public static string[] exec(string command, string sanitizedCommand, MJBLog Log)
    {
        Log.Verbose("Command: \"" + sanitizedCommand + "\"");
        string output = ExecuteCommand(command);

        if (output != null)
        {
            string[] lines = output.Split('\n');
            if (Log.Level.GE(LogLevel.Verbose))
            {
                Log.Verbose("Command Result:");
                foreach (string line in lines)
                {
                    Log.Echo(line);
                }
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

    public static string executeAndLog(string command, string sanitizedCommand, MJBLog Log)
    {
        Log.Verbose("Command : " + sanitizedCommand);
        string output = ExecuteCommand(command);

        if (Log.Level.GE(LogLevel.Verbose))
        {
            Log.Verbose("Command Result:");

            string[] lines = Regex.Split(output, "\r\n");
            foreach (string line in lines)
            {
                Log.Echo(line);
            }
        }

        return output;
    }

    public static bool executeAndVerify(string command, string sanitizedCommand, string successIndicator, int requiredInstancesOfSuccessIndicator, MJBLog Log)
    {
        Log.Verbose("Command : " + sanitizedCommand);
        string output = ExecuteCommand(command);
        Log.Verbose("Command Result:");

        if (Log.Level.GE(LogLevel.Verbose))
        {
            string[] lines = Regex.Split(output, "\r\n");
            foreach (string line in lines)
            {
                Log.Echo(line);
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

