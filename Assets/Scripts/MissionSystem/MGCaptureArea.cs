using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGCaptureArea : MissionGoal
{

    [SerializeField]
    float CaptureTime;

    bool Capturing;
    float CapturedTime;

    bool CaptureCompleted = false;

    private void Update()
    {
        if ((!CaptureCompleted) && Capturing)
        {
            CapturedTime += Time.deltaTime;
            if (CapturedTime > CaptureTime)
            {
                CaptureCompleted = true;
                CapturedTime = CaptureTime;
                MyTracker.UpdateProgress("",null);
                InvokeMGProgress(false);
            }
        }
    }

    public override void UpdateProgress(string UpdateMarker, object Content)
    {
        if (Marker != UpdateMarker)
            return;

        Debug.Log(UpdateMarker + "---" + Content);

        if ((bool)Content)
            Capturing = true;
        else
            Capturing = false;
    }


    public override bool Completed()
    {
        return CaptureCompleted;
    }

    public override void Init(MissionTracker a)
    {
        base.Init(a);
        CapturedTime = 0;
    }

    public override string GetMissionProgress()
    {
        return (CapturedTime/CaptureTime *100).ToString("F1")+"%";
    }

    public override float GetMissionPercentageProgress()
    {
        return CapturedTime / CaptureTime;
    }
}
