using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MissionCompletionTracker : MonoBehaviour
{

    private static MissionCompletionTracker _Instance;
    public static MissionCompletionTracker Instance
    {
        get
        {
            if (!_Instance)
                _Instance = FindObjectOfType<MissionCompletionTracker>();
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

    private static string DefaultSaveName = "SavedMissionData";

    [SerializeField]
    List<string> CompletedMissionSerials = new List<string>();

    [SerializeField]
    List<MissionUnlocks> MissionUnlockedTags = new List<MissionUnlocks>();

    [Serializable]
    class MissionUnlocks
    {
        [SerializeField]
        string LevelName;
        [SerializeField]
        List<string> LinkUnlockTags;

        public void Check(string LevelCompleted)
        {
            Debug.Log(LevelCompleted+"()"+LevelName);
            if (LevelCompleted == LevelName)
            {
                foreach(string a in LinkUnlockTags)
                UnlockTagTracker.Instance.AddUTag(a);
            }
        }
    }

    string CurrentlyPlayingMission;
    bool MissionsLoaded = false;

    private void Start()
    {
        if (MissionCompletionTracker.Instance!=this)
            Destroy(this.gameObject);
        else
        {
            DontDestroyOnLoad(this);
            if (!MissionsLoaded)
                LoadCompletedMissionData();
        }
    }

    private void LoadCompletedMissionData()
    {
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

        if(LoadedData!=null) //fist time opening the game will have no initial file, fresulting in an empty loaded data field, if not accounted for, throws nulls and thanks for playing text remains on screen
            CompletedMissionSerials.AddRange(LoadedData.Split(new string[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries));

        MissionsLoaded = true;
    }

    private void SaveCompletedMissionData()
    {
        string Data = "";

        foreach(string a in CompletedMissionSerials)
        {
            Data += a;
            Data += "|";
        }



        string FullPath = Path.Combine(DefaultSavedPath,DefaultSaveName);

        Directory.CreateDirectory(Path.GetDirectoryName(FullPath));

        using (FileStream Stream = new FileStream(FullPath, FileMode.Create))
        {
            using (StreamWriter Writer = new StreamWriter(Stream))
            {
                Writer.Write(Data);
            }
        }
    }




    public void LoadPlayingMission(string Serial)
    {
        CurrentlyPlayingMission = Serial;
    }

    public void MissionCompletion(bool Victory)
    {
        if(Victory)
        {
            if (CurrentlyPlayingMission != "" || CurrentlyPlayingMission != null)
            {
                if (!CompletedMissionSerials.Contains(CurrentlyPlayingMission))
                    CompletedMissionSerials.Add(CurrentlyPlayingMission);

                foreach (MissionUnlocks a in MissionUnlockedTags)
                    a.Check(CurrentlyPlayingMission);

                CurrentlyPlayingMission = null;
            }
        }
    }

    public bool GetMissionStatus(string Serial)
    {
        if (!MissionsLoaded)
            LoadCompletedMissionData();

        if (CompletedMissionSerials.Contains(Serial))
            return true;
        else
            return false;
    }


    private void OnApplicationQuit()
    {
        SaveCompletedMissionData();
    }
}
