using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGMoveTo : MissionGoal
{

    bool Arrived = false;


    public override void UpdateProgress(string UpdateMarker, object Content)
    {
        if (Marker != UpdateMarker)
            return;


        if ((bool)Content)
            Arrived = (bool)Content;
    }

    public override bool Completed()
    {
        return Arrived;
    }

    public override string GetMissionProgress()
    {
        return "";
    }

    public override float GetMissionPercentageProgress()
    {
        if (Arrived)
            return 1;
        else
            return 0;
    }
}
