using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITaskObjective : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.UI.Text Target;
    [SerializeField]
    public UnityEngine.UI.Text Progress;


    private MissionGoal TrackedGoal;
    List<UITaskObjective> Objectives;



    public void Create(MissionGoal a)
    {
        TrackedGoal = a;

        Target.text = TrackedGoal.GetDisplayTarget();
        Progress.text = TrackedGoal.GetMissionProgress();
    }


    private void Update()
    {
        Progress.text = TrackedGoal.GetMissionProgress();
    }
}
