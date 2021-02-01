using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class Log
 {

#region Static Log level Identifiers Defined
    public static readonly int _MASSIVE = 5;
    public static readonly int _DEBUG = 4;
    public static readonly int _INFO = 3;
    public static readonly int _WARNING = 2;
    public static readonly int _ERROR = 1;
    public static readonly int _CRITICAL = 0;
#endregion

#region Attribute Declarations

    private string file;
    private int logLevel;

    #endregion

#region Constructors
    public Log(string filePath)
    {
        file = filePath;
        logLevel = _INFO;
    }

    public Log(string filePath, int levelAsInt)
    {
        file = filePath;
        logLevel = levelAsInt;
    }

    public Log(string filePath, string levelAsString)
    {
        file = filePath;
        set_level(levelAsString);
    }
#endregion

#region Log Level Manipulation
    public void set_level(int level)
     {
         logLevel = level;
     }

     public void set_level(string lvl)
     {
         switch (lvl.ToUpper().Trim())
         {
             case "CRITICAL":
                 logLevel = _CRITICAL;
                 break;
             case "ERROR":
                 logLevel = _ERROR;
                 break;
             case "WARNING":
                 logLevel = _WARNING;
                 break;
             case "INFO":
                 logLevel = _INFO;
                 break;
             case "DEBUG":
                 logLevel = _DEBUG;
                 break;
             default:
                 logLevel = _ERROR;
                 break;
         }
     }

     public int get_level()
     {
         return logLevel;
     }

     public string get_levelLabel()
     {
         switch (logLevel)
         {
             case 0:
                 return "CRITICAL";
             case 1:
                 return "ERROR";
             case 2:
                 return "WARNING";
             case 3:
                 return "INFO";
             case 4:
                 return "DEBUG";
         }

         return "INFO";
     }

     private int get_level(string level)
     {
         switch (level.ToUpper().Trim())
         {
             case "CRITICAL":
                 return _CRITICAL;
             case "ERROR":
                 return _ERROR;
             case "WARNING":
                 return _WARNING;
             case "INFO":
                 return _INFO;
             case "DEBUG":
                 return _DEBUG;
             default:
                 return _INFO;
         }
     }
    #endregion

#region Writing Methods
     public void writeSeparator(string message)
     {
         if (File.Exists(file))
         {
             StreamWriter sw = File.AppendText(file);
             writeSeparator(message, sw);
             sw.Close();
         }
         else
         {
             StreamWriter sw = File.CreateText(file);
             writeSeparator(message, sw);
             sw.Close();
         }
     }

     private void writeSeparator(string message, StreamWriter sw)
     {
         DateTime CurrTime = DateTime.Now;
         string time = CurrTime.ToString();
         string bumper = "*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~";
         sw.Write(sw.NewLine);
         sw.WriteLine(bumper);
         sw.WriteLine(message.Replace("[DATE]", time));
         sw.Write(sw.NewLine);
         sw.WriteLine(bumper);
         sw.Write(sw.NewLine);
     }

     public void exception(string level, Exception e, bool ignoreLogLevel)
     {
         if ((!ignoreLogLevel) || (get_level(level) >= logLevel))
         {
             write(level, "An exception occurred:", ignoreLogLevel);
             string[] lines = Regex.Split(e.ToString(), "\r\n");
             foreach (string line in lines)
                 echo("          " + line);
         }
     }

     public void exception(int level, Exception e, bool ignoreLogLevel)
     {
         if ((!ignoreLogLevel) || (level >= logLevel))
         {
             write(level, "An exception occurred:", ignoreLogLevel);
             string[] lines = Regex.Split(e.ToString(), "\r\n");
             foreach (string line in lines)
                 echo("          " + line);
         }
     }

     public void exception(string level, string message, Exception e, bool ignoreLogLevel)
     {
         if ((!ignoreLogLevel) || (get_level(level) >= logLevel))
         {
             write(level, message, ignoreLogLevel);
             string[] lines = Regex.Split(e.ToString(), "\r\n");
             foreach (string line in lines)
                 echo("          " + line);
         }
     }

     public void exception(int level, string message, Exception e, bool ignoreLogLevel)
     {
         if ((!ignoreLogLevel) || (level >= logLevel))
         {
             write(level, message, ignoreLogLevel);
             string[] lines = Regex.Split(e.ToString(), "\r\n");
             foreach (string line in lines)
                 echo("          " + line);
         }
     }

     public void write(string level, string message, bool ignoreLogLevel)
     {
         write(get_level(level), message, ignoreLogLevel);
     }

     public void write(int level, string message, bool ignoreLogLevel)
     {
         if ((logLevel >= level) || (ignoreLogLevel))
             write(level, message);
     }

     public void write(int level, string message)
     {
         string levelLabel;

         switch (level)
         {
             //DEBUG
             case 4:
                 levelLabel = "<Debug>   ";
                 break;

             //INFO
             case 3:
                 levelLabel = "<Info>    ";
                 break;

             //WARNING
             case 2:
                 levelLabel = "<Warning> ";
                 break;

             //ERROR
             case 1:
                 levelLabel = "<Error>   ";
                 break;

             //CRITICAL
             case 0:
                 levelLabel = "<Critical>";
                 break;

             default:
                 levelLabel = "<Unknown> ";
                 break;
         }

         if (File.Exists(file))
             AppendLog(levelLabel, message);

         else
             initializeLog(levelLabel, message);
     }

     public void echo(string message)
     {
         if (File.Exists(file))
             AppendLog(message);
         else
             initializeLog(message);
     }

     public void echo(string message, bool indent)
     {
         string spacing = "";
         if (indent)
             spacing = "          ";
         if (File.Exists(file))
             AppendLog(spacing + message);
         else
             initializeLog(spacing + message);
     }

     public void echo(string message, int level, bool indent)
     {
         if (level >= logLevel)
         {
             string spacing = "";
             if (indent)
                 spacing = "          ";

             if (File.Exists(file))
                 AppendLog("", spacing + message);
             else
                 initializeLog("", spacing + message);
         }
     }

     public void echo(string message, string level, bool indent)
     {
         if (get_level(level) >= logLevel)
         {
             string spacing = "";
             if (indent)
                 spacing = "          ";

             if (File.Exists(file))
                 AppendLog("", spacing + message);
             else
                 initializeLog("", spacing + message);
         }
     }

     private void initializeLog(string level, string message)
     {
         DateTime CurrTime = DateTime.Now;
         StreamWriter sw = File.CreateText(file);
         sw.WriteLine("{0:R}", CurrTime + " " + level + " - " + message);
         sw.Close();
     }

     private void initializeLog(string message)
     {
         StreamWriter sw = File.CreateText(file);
         sw.WriteLine(message);
         sw.Close();
     }

     private void AppendLog(string type, string message)
     {
         DateTime CurrTime = DateTime.Now;
         StreamWriter sw = File.AppendText(file);
         sw.WriteLine("{0:R}", CurrTime + " " + type + " - " + message);
         sw.Close();
     }

     private void AppendLog(string message)
     {
         StreamWriter sw = File.AppendText(file);
         sw.WriteLine(message);
         sw.Close();
     }
     #endregion

#region Miscellaneous Methods

     public string getFilePath()
     {
         return file;
     }

     public bool clear()
     {
         try
         {
             File.Create(file).Close();
             return true;
         }

         catch (Exception)
         {
             return false;
         }
     }
     #endregion
 }
