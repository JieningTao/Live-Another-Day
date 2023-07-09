using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectiveTrackManager : MonoBehaviour
{

    public static UIObjectiveTrackManager _instance;
    public static UIObjectiveTrackManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIObjectiveTrackManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private GameObject TaskPrefab;
    [SerializeField]
    private GameObject TaskUIParent;
    [SerializeField]
    private Animator HeaderAnimator;

    List<UITaskDisplay> TasksOnDisplay = new List<UITaskDisplay>();


    public void CreateMission(string Mission, List<MissionGoal> a)
    {
        UITaskDisplay Temp = Instantiate(TaskPrefab, TaskUIParent.transform).GetComponent<UITaskDisplay>();
        TasksOnDisplay.Add(Temp);
        Temp.Create(Mission,a);
    }

    public void ClearMissions()
    {
        for (int i = 0; i < TasksOnDisplay.Count; i++)
        {
            Destroy(TasksOnDisplay[i].gameObject);
        }
        TasksOnDisplay.Clear();
    }

    public void FlashHeader()
    {
        HeaderAnimator.SetTrigger("Flash");
    }

}
