﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Map_Generator.Parsing;

public class PathHandler
{
    private static readonly string
        _basePath = Directory.GetParent(FindDirectory(Application.StartupPath, "Data", 3))?.FullName ??
                   throw new Exception("Could not find base path");

    private static readonly string _localLowPath =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "LocalLow");

    private static readonly string _underminePath = _localLowPath + @"\Thorium Entertainment\UnderMine\";


    //savedata
    public static readonly string UndermineSavePath = Path.Combine(_underminePath, @"Saves\");
    public static string SavePath(int saveNumber) => Path.Combine(UndermineSavePath, @$"Save{saveNumber}.json");

    public static readonly string DataPath = Path.Combine(_basePath, @"Data\");
    
    //tests
    public static readonly string TestsPath = Path.Combine(_basePath, @"Tests\");

    //json
    public static readonly string JsonPath = Path.Combine(DataPath, @"Json\");

    //images
    public static readonly string ImagesPath = Path.Combine(DataPath, @"Images\");
    public static readonly string MapPath = Path.Combine(ImagesPath, @"Maps\");
    public static readonly string EnemyPath = Path.Combine(ImagesPath, @"Enemies\");
    public static readonly string ItemPath = Path.Combine(ImagesPath, @"Items\");
    public static readonly string DoorPath = Path.Combine(ImagesPath, @"Doors\");

    public static string FindDirectory(string baseDirectory, string targetDirectory, int backwards)
    {
        FindDirectory(baseDirectory, targetDirectory, backwards, out var result);
        return result;
    }

    public static bool FindDirectory(string? baseDirectory, string targetDirectory, int backwards, out string result)
    {
        //TODO: thinking about it, everytime it goes backwards it checks every directory again probably fix that, currently it's not a problem
        result = "";
        if (baseDirectory == null)
            return false;

        if (!Directory.Exists(baseDirectory))
            throw new DirectoryNotFoundException($"Could not find directory {targetDirectory} in {baseDirectory}");

        string[] directories = Directory.GetDirectories(baseDirectory);
        foreach (string directory in directories)
        {
            if (Path.GetFileName(directory).Equals(targetDirectory, StringComparison.OrdinalIgnoreCase))
            {
                result = directory;
                return true;
            }

            if (FindDirectory(directory, targetDirectory, 0, out result))
                return true;
        }

        if (backwards != 0 && !FindDirectory(Directory.GetParent(baseDirectory)?.FullName, targetDirectory,
                --backwards, out result)) return false;

        return !string.IsNullOrEmpty(result);
    }

    public static void WriteAllToFile(string path, string outputString)
    {
        FileInfo file = new FileInfo(path);
        file.Directory?.Create();
        File.WriteAllText(file.FullName, outputString);
    }
}