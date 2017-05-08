using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Folder
{
    public static bool CreateStructure(string path)
    {
        if (Directory.Exists(path))
            return true;
        try
        {
            Directory.CreateDirectory(path);
            return Directory.Exists(path);
        }

        catch (Exception)
        {
            return false;
        }
    }

    public static bool Create(string folderName)
    {
        string parentDirectory = Directory.GetCurrentDirectory() + "\\" + folderName;
        return CreateFolder(folderName);
    }

    public static bool Create(string foldername, bool deleteIfAlreadyPresent)
    {
        if (deleteIfAlreadyPresent)
        {
            if (Delete(foldername))
            {
                return Create(foldername);
            }
            else
                return false;
        }
        else
            return Create(foldername);
    }

    /* public static bool Create(string parentDirectory, string folderName)
    {
        char last = parentDirectory[parentDirectory.Length - 1];
        if (!(last == '\\'))
            parentDirectory = parentDirectory + "\\";
        string absolutePath = parentDirectory + folderName;
        return CreateFolder(absolutePath);
    } */

    private static bool CreateFolder(string folderName)
    {
        if (Directory.Exists(folderName))
            return true;

        else
        {
            Directory.CreateDirectory(folderName);
            if (Directory.Exists(folderName))
                return true;
            else
                return false;
        }
    }

    public static bool Delete(string folder)
    {

        if (!(Directory.Exists(folder)))
            return true;
        else
        {
            try
            {
                Directory.Delete(folder, true);
            }
            catch (System.IO.IOException)
            {
                return false;
            }

            if (Directory.Exists(folder))
                return false;
            else
                return true;
        }
    }

    public static List<string> ListFiles(string folder)
    {
        if (Directory.Exists(folder))
        {
            List<string> files = new List<string>();
            DirectoryInfo di = new DirectoryInfo(folder);
            FileInfo[] rgFiles = di.GetFiles("*");
            foreach (FileInfo fi in rgFiles)
                files.Add(fi.Name);
            return files;
        }
        else
            return null;
    }

    public static List<string> ListFiles(string folder, string regex)
    {
        if (Directory.Exists(folder))
        {
            List<string> files = new List<string>();
            DirectoryInfo di = new DirectoryInfo(folder);
            FileInfo[] rgFiles = di.GetFiles(regex);
            foreach (FileInfo fi in rgFiles)
                files.Add(fi.Name);
            return files;
        }
        else
            return null;
    }
}
