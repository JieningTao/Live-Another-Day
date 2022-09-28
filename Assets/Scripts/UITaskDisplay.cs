using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITaskDisplay : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text MissionTitle;

    private MissionGoal TrackedGoal;

    [SerializeField]
    List<UITaskObjective> Objectives;



    public void Create(string Mission,List<MissionGoal> a)
    {
        MissionTitle.text = Mission;

        while (a.Count > Objectives.Count)
        {
            Objectives.Add(Instantiate(Objectives[0], Objectives[0].transform.parent).GetComponent<UITaskObjective>());
        }

        for (int i = 0; i < a.Count; i++)
            Objectives[i].Create(a[i]);

    }
}
