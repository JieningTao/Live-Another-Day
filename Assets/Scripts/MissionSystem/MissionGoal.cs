using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGoal : MonoBehaviour
{

    [SerializeField]
    string DisplayTarget;
    [SerializeField]
    public string Marker;
    [SerializeField]
    public List<GameObject> PingTrackerMarkers = new List<GameObject>();

    private MissionTracker MyTracker;








    public virtual bool Completed()
    {
        return false;
    }


    public virtual void UpdateProgress(string UpdateMarker, object Content)
    {
        if (Marker != UpdateMarker)
            return;
        
    }

    public virtual void Init(MissionTracker a)
    {
        MyTracker = a;
        if (PingTrackerMarkers.Count > 0)
            CreateTrackers(PingTrackerMarkers);
    }

    public virtual void CreateTrackers(List<GameObject> a)
    {
        foreach (GameObject b in a)
        {
        UILockManager.Instance.CreateMissionTracker(b.name,b);
        }
    }

    //public virtual string GetMissionName()
    //{
    //    return MissionName;
    //}

    public virtual string GetDisplayTarget()
    {
        return DisplayTarget;
    }

    public virtual string GetMissionProgress()
    {
        return "ERR";
    }

    public virtual float GetMissionPercentageProgress()
    {
        return 0;
    }

}
