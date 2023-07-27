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
    [SerializeField]
    private Animator MyAnimator;

    bool Completed = false;


    public void Create(MissionGoal a)
    {
        TrackedGoal = a;

        Target.text = TrackedGoal.GetDisplayTarget();
        Progress.text = TrackedGoal.GetMissionProgress();
    }


    private void Update()
    {
        if (!Completed)
        {
            Progress.text = TrackedGoal.GetMissionProgress();

            if (TrackedGoal.GetMissionPercentageProgress() == 1)
            {
                Complete();
                Completed = true;
            }
        }

    }

    private void Complete()
    {
        MyAnimator.SetTrigger("Complete");
    }
}
