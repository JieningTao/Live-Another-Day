using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class SaveLoadManager
{
    private static string _DefaultSavePath = "";
    private static string DefaultSavedPath
    {
        get {
            if (_DefaultSavePath == "")
                _DefaultSavePath = Application.dataPath;
            return _DefaultSavePath;
        }
    }

    private static string DefaultSaveName = "SavedData";

    public static String LoadData(string FileName)
    {
        string FullPath = Path.Combine(DefaultSavedPath, FileName);

        String LoadedData = null;
        if (File.Exists(FullPath))
        {
            using (FileStream Stream = new FileStream(FullPath, FileMode.Open))
            {
                using (StreamReader Reader = new StreamReader(Stream))
                {
                    LoadedData = Reader.ReadToEnd();
                }
            }
        }

        return LoadedData;
    }

    public static String LoadData()
    {
        return LoadData(DefaultSaveName);
    }

    public static void SaveData(string FileName,string Data)
    {
        string FullPath = Path.Combine(DefaultSavedPath, FileName);

        Directory.CreateDirectory(Path.GetDirectoryName(FullPath));

        using (FileStream Stream = new FileStream(FullPath, FileMode.Create))
        {
            using (StreamWriter Writer = new StreamWriter(Stream))
            {
                Writer.Write(Data);
            }
        }
    }

    public static void SaveData(string Data)
    {
        SaveData(DefaultSaveName,Data);
    }



}
