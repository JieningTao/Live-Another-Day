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
    private List<MissionGoal> CurrentObjectives;



    public void UpdateProgress(string Marker, object Content)
    {
        foreach (MissionGoal a in CurrentObjectives)
        {
            a.UpdateProgress(Marker, Content);
        }
    }


}
