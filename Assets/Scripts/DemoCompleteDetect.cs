using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCompleteDetect : MonoBehaviour
{
    private void Start()
    {
        if (MissionCompletionTracker.Instance.GetMissionStatus("1-1") && MissionCompletionTracker.Instance.GetMissionStatus("1-2") && MissionCompletionTracker.Instance.GetMissionStatus("1-3"))
        {
            this.gameObject.SetActive(true);
        }
        else
            this.gameObject.SetActive(false);
    }
}
