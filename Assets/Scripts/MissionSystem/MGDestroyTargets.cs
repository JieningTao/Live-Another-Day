using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGDestroyTargets : MissionGoal
{


    [SerializeField]
    int TargetAmount;

    int AmountDestroied;




    public override void UpdateProgress(string UpdateMarker, object Content)
    {
        base.UpdateProgress(UpdateMarker, Content);


        if (AmountDestroied<TargetAmount)

            AmountDestroied += (int)Content;
    }


    public virtual bool Completed()
    {
        return AmountDestroied < TargetAmount;
    }

    public override void Init(MissionTracker a)
    {
        base.Init(a);
        AmountDestroied = 0;

    }

    public override string GetMissionProgress()
    {
        return AmountDestroied+"/"+TargetAmount;
    }

    public override float GetMissionPercentageProgress()
    {
        return AmountDestroied/TargetAmount;
    }

}
