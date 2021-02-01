using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net.Mail;
using Microsoft.Win32;

public class stdlib
{

    public static string FileSelecter(string title, string filter)
    {
        string selectedFile;
        OpenFileDialog browser = new OpenFileDialog();
        browser.Filter = filter;
        browser.Title = title;

        if (browser.ShowDialog() == DialogResult.Cancel)
            return "";
        try
        {
            selectedFile = browser.FileName;
            return selectedFile;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static bool DeleteFile(string fileName, Log log)
    {

        if (!(File.Exists(fileName)))
            return true;
        else
        {
            foo f = new foo();
            while (f.fileLocked(fileName)) ;

            try
            {
                File.Delete(fileName);
            }
            catch (System.IO.IOException e)
            {
                log.exception(Log._ERROR, e, false);
                return false;
            }

            if (File.Exists(fileName))
            {
                log.write(Log._WARNING, "The file could not be deleted: \"" + fileName + "\"", false);
                return false;
            }
            else
                return true;
        }
    }

    public static List<string> ReadFile(string fileName)
    {
        StreamReader oFile;
        try
        {
            oFile = new StreamReader(fileName);
        }
        catch (Exception)
        {
            return null;
        }
        string record = "";
        List<string> fileToList = new List<string>();

        while (record != null)
        {
            record = oFile.ReadLine();
            if (record != null)
                fileToList.Add(record);
        }
        oFile.Close();
        return fileToList;
    }

    public static string AppendDate(string text)
    {
        return text + "_" + GetDate();
    }

    private static string GetDate()
    {
        DateTime d = DateTime.Now;
        return d.ToString("yyyy-MM-dd_HHmm");
    }

    public static string Split(string record, char separator, int requiredElement)
    {
        try
        {
            string[] tokens = record.Split(separator);
            return tokens[requiredElement];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    public static bool InString(string record, string searchText)
    {
        if (record.IndexOf(searchText) != -1)
            return true;
        else
            return false;
    }

    public static string clean(string record)
    {
        return Regex.Replace(record, @"[^\w\.@-]", "");
    }

    public static string Left(string param, int length)
    {
        if (length > param.Length)
            return null;
        else
            return param.Substring(0, length);
    }

    public static string Right(string param, int index)
    {
        if (index > param.Length)
            return null;
        else
            return param.Substring(index, param.Length - index);
    }

    public static string lastElement(string record, char token)
    {
        string requiredElement = "";
        string[] pieces = record.Split(token);
        foreach (string piece in pieces)
        {
            try
            {
                requiredElement = piece;
            }
            catch (Exception) { }
        }
        return requiredElement;
    }

    public static void SendMail(string sender, string recipient, string subject, string message, string mailhost)
    {
        MailMessage mail = new MailMessage();

        mail.To.Add(new MailAddress(recipient));
        mail.From = new MailAddress(sender);
        mail.Subject = subject;
        mail.Body = message;

        SmtpClient client = new SmtpClient();
        client.Host = "mailhost.merck.com";
        client.Send(mail);
    }

    public static void SendMail(string sender, string recipient, string subject, string message, string attachment, string mailhost)
    {
        MailMessage mail = new MailMessage();

        mail.To.Add(new MailAddress(recipient));
        mail.From = new MailAddress(sender);
        mail.Subject = subject;
        mail.Body = message;
        mail.Attachments.Add(new System.Net.Mail.Attachment(attachment));

        SmtpClient client = new SmtpClient();
        client.Host = "mailhost.merck.com";
        client.Send(mail);
    }

    public static char lastChar(string record)
    {
        return record[record.Length - 1];
    }

    public static char firstChar(string record)
    {
        return record[0];
    }

    public static string getLoggedInUserName()
    {
        return stdlib.Split(System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString(), '\\', 1);
    }

    public static int instancesOfSubString(string text, string searchText)
    {
        int instancesFound = 0;
        int location = 0;
        while (location != -1)
        {
            location = text.IndexOf(searchText);
            if (location != -1)
            {
                instancesFound++;
                text = stdlib.Right(text, location + searchText.Length);
            }
        }
        return instancesFound;
    }

    public static string surroundWithDoubleQuotes(string text)
    {
        try
        {
            return "\"" + text + "\"";
        }
        catch (NullReferenceException)
        {
            return "!EMPTY!";
        }
    }

    public static string pwd()
    {
        return Directory.GetCurrentDirectory() + "\\";
    }

    public static string getFileDirectory(string completeFilePath)
    {
        string[] parts = completeFilePath.Split('\\');
        int length = parts.Length - 2;
        if (length == -1)
            return pwd();

        int index = 0;
        string directory = "";
        while (index <= length)
        {
            directory = directory + parts[index] + "\\";
            index++;
        }

        return directory;
    }

    public static void setPWD(string targetWorkingDirectory)
    {
        if ((targetWorkingDirectory.Length == 0) || (targetWorkingDirectory == null))
            return;

        Environment.CurrentDirectory = targetWorkingDirectory;
    }

    public static string findBetween(string record, string left, string right)
    {
        string rightside = Left(record, record.IndexOf(left) + left.Length);
        string[] parts = rightside.Split(right.ToCharArray());
        return parts[0];
    }

    public static IEnumerable<string> GetSubStringsBetween(string originalRecord, string start, string end)
    {
        Regex r = new Regex(Regex.Escape(start) + "(.*?)" + Regex.Escape(end));
        MatchCollection matches = r.Matches(originalRecord);
        foreach (Match match in matches)
            yield return match.Groups[1].Value;
    }

    public static bool isComment(char commentIndicator, string line)
    {
        try
        {
            char firstChar = line[0];
            if (firstChar == commentIndicator)
                return true;

            return false;
        }

        catch (IndexOutOfRangeException)
        {
            return false;
        }
    }

    public static bool isComment(string line)
    {
        return isComment('#', line);
    }

    public static bool isValidEmailAddress(string inputEmail)
    {
        string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        Regex re = new Regex(strRegex);
        if (re.IsMatch(inputEmail))
            return (true);
        else
            return (false);
    }

    public static void hibernate(int timeInMinutes)
    {
        int slumberTime = timeInMinutes * 60000;
        System.Threading.Thread.Sleep(slumberTime);
    }

    public static bool readFileAndVerify(string fileName, string searchToken, int requiredOccurances)
    {
        int occurances = 0;
        List<string> lines = ReadFile(fileName);
        foreach (string line in lines)
        {
            if (InString(line, searchToken))
                occurances++;
        }

        return (occurances == requiredOccurances);

    }

    public static bool writeStringToRegistry(string hive, string registryRoot, string keyName, string value)
    {
        RegistryKey masterKey;

        switch (hive.ToUpper())
        {
            case "HKLM":
                masterKey = Registry.LocalMachine.CreateSubKey(registryRoot);
                break;
            case "HKCU":
                masterKey = Registry.CurrentUser.CreateSubKey(registryRoot);
                break;
            default:
                return false;
        }

        try
        {
            masterKey.SetValue(keyName, value);
            masterKey.Close();
            return true;
        }

        catch (Exception)
        {
            return false;
        }
    } //writeStringToRegistry

    public static string readStringFromRegistry(string hive, string registryRoot, string keyName)
    {
        string value;
        RegistryKey masterKey;

        switch (hive.ToUpper())
        {
            case "HKLM":
                masterKey = Registry.LocalMachine.CreateSubKey(registryRoot);
                break;
            case "HKCU":
                masterKey = Registry.CurrentUser.CreateSubKey(registryRoot);
                break;
            default:
                return null;
        }

        try
        {
            value = masterKey.GetValue(keyName).ToString();
            if (value == null)
                return null;
            else
                return value;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static bool registryValueExists(string hive_HKLM_or_HKCU, string registryRoot, string valueName)
    {
        RegistryKey root;
        switch (hive_HKLM_or_HKCU.ToUpper())
        {
            case "HKLM":
                root = Registry.LocalMachine.OpenSubKey(registryRoot, false);
                break;
            case "HKCU":
                root = Registry.CurrentUser.OpenSubKey(registryRoot, false);
                break;
            default:
                throw new System.InvalidOperationException("parameter registryRoot must be either \"HKLM\" or \"HKCU\"");
        }

        if (root.GetValue(valueName) == null)
            return false;
        else
            return true;
    }

    public static void grab(string text)
    {
        Clipboard.SetDataObject(text, true);
    }
}

class foo
{
    public foo() { }

    public virtual bool fileLocked(string fileName)
    {
        if (!File.Exists(fileName))
            return false;

        FileInfo file = new FileInfo(fileName);
        FileStream stream = null;

        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }

        catch (IOException)
        {
            return true;
        }

        finally
        {
            if (stream != null)
                stream.Close();
        }

        return false;
    }
}
