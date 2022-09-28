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

    [SerializeField]
    private string CurrentMission;
    [SerializeField]
    private List<MissionGoal> CurrentObjectives;

    private void Start()
    {
        CreateUIElement();
    }

    public void UpdateProgress(string Marker, object Content)
    {

        foreach (MissionGoal a in CurrentObjectives)
        {
            a.UpdateProgress(Marker, Content);
        }
    }

    private void CreateUIElement()
    {
        for(int i=0;i<CurrentObjectives.Count;i++)
        {
            if (CurrentObjectives[i])
                CurrentObjectives[i].Init(this);
            else
            {
                CurrentObjectives.RemoveAt(i);
                i--;
            }
        }
        UIObjectiveTrackManager.Instance.CreateMission(CurrentMission, CurrentObjectives);
    }


}
