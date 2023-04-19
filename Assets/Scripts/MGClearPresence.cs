using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGClearPresence : MissionGoal
{

    [SerializeField]
    float TargetScore;

    float ClearedScore;


    public override void UpdateProgress(string UpdateMarker, object Content)
    {
        if (Marker != UpdateMarker)
            return;

        //Debug.Log(UpdateMarker + "---" + Content);

        if (ClearedScore < TargetScore)
            ClearedScore += (float)Content;

        //Debug.Log((float)Content+"+"+ GetMissionProgress());
    }

    public override bool Completed()
    {
        return ClearedScore >= TargetScore;
    }

    public override void Init(MissionTracker a)
    {
        base.Init(a);
        ClearedScore = 0;

    }

    public override string GetMissionProgress()
    {
        return ((ClearedScore/TargetScore)*100).ToString("F0")+"%";
    }

    public override float GetMissionPercentageProgress()
    {
        return ClearedScore / TargetScore;
    }
}
