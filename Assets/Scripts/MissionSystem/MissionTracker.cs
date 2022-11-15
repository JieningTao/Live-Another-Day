using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    public static MissionTracker _instance;
    public static MissionTracker Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MissionTracker>();
            }
            return _instance;
        }
    }

    [Serializable]
    class Stage
    {
        [SerializeField]
        public String Stagename;
        [SerializeField]
        public List<MissionGoal> Objectives;

        public bool StageCompleted()
        {
            for (int i = 0; i < Objectives.Count; i++)
            {
                if (!Objectives[i].Completed())
                    return false;
            }
            return true;
        }
    }

    [SerializeField]
    private List<Stage> MissionStages;
    private int CurrentStage = 0;

    private void Start()
    {
        CreateUIElement(MissionStages[CurrentStage]);
    }

    public void UpdateProgress(string Marker, object Content)
    {

        foreach (MissionGoal a in MissionStages[CurrentStage].Objectives)
        {
            a.UpdateProgress(Marker, Content);

            if (a.Completed())
            {
                if (MissionStages[CurrentStage].StageCompleted())
                {
                    NextStage();
                }
            }
            //check for stage completion
        }
    }

    private void CreateUIElement(Stage a)
    {
        for(int i=0;i< MissionStages[CurrentStage].Objectives.Count;i++)
        {
            if (MissionStages[CurrentStage].Objectives[i])
                MissionStages[CurrentStage].Objectives[i].Init(this);
            else
            {
                MissionStages[CurrentStage].Objectives.RemoveAt(i);
                i--;
            }
        }

        UIObjectiveTrackManager.Instance.CreateMission(a.Stagename, a.Objectives);
    }
    private void NextStage()
    {
        if (CurrentStage < MissionStages.Count - 1)
        {
            UILockManager.Instance.ClearMissionTrackers();
            UIObjectiveTrackManager.Instance.ClearMissions();

            CurrentStage++;
            CreateUIElement(MissionStages[CurrentStage]);




        }
        else
        {
            PauseMiniMenu.Instance.ShowLevelEndUI(true);
            //mission completed
        }

    }

}
