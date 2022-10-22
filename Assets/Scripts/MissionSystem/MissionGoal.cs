using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGoal : MonoBehaviour
{

    [SerializeField]
    string DisplayTarget;
    [SerializeField]
    public string Marker;

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
