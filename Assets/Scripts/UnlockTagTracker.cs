using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class UnlockTagTracker : MonoBehaviour
{
    private static UnlockTagTracker _Instance;
    public static UnlockTagTracker Instance
    {
        get
        {
            if (!_Instance)
                _Instance = FindObjectOfType<UnlockTagTracker>();
            return _Instance;
        }
    }

    private static string _DefaultSavePath = "";
    private static string DefaultSavedPath
    {
        get
        {
            if (_DefaultSavePath == "")
                _DefaultSavePath = Application.dataPath;
            return _DefaultSavePath;
        }
    }

    private static string DefaultSaveName = "UnlockTagData";

    [SerializeField]
    public bool NewUnlocks = false;
    [SerializeField]
    public List<string> UnlockTags
    { get; protected set; }
    bool TagsLoaded = false;

    private void Start()
    {
        if (UnlockTagTracker.Instance != this)
            Destroy(this.gameObject);
        else
        {
            DontDestroyOnLoad(this);
            if (!TagsLoaded)
                LoadTagData();
        }
    }

    public void AddUTag(string Tag)
    {
        if (!UnlockTags.Contains(tag))
        {
            NewUnlocks = true;
            UnlockTags.Add(Tag);
        }
    }

    public void GarageEntered()
    {
        NewUnlocks = false;
    }

    public void LoadTagData()
    {
        UnlockTags = new List<string>();

        string FullPath = Path.Combine(DefaultSavedPath, DefaultSaveName);

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

        if (LoadedData != null) //fist time opening the game will have no initial file, fresulting in an empty loaded data field, if not accounted for, throws nulls and thanks for playing text remains on screen
            UnlockTags.AddRange(LoadedData.Split(new string[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries));

        TagsLoaded = true;
    }

    public void SaveTagData()
    {
        string Data = "";

        foreach (string a in UnlockTags)
        {
            Data += a;
            Data += "|";
        }

        string FullPath = Path.Combine(DefaultSavedPath, DefaultSaveName);

        Directory.CreateDirectory(Path.GetDirectoryName(FullPath));

        using (FileStream Stream = new FileStream(FullPath, FileMode.Create))
        {
            using (StreamWriter Writer = new StreamWriter(Stream))
            {
                Writer.Write(Data);
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveTagData();
    }
}
