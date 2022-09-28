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

    //List


    public void CreateMission(string Mission, List<MissionGoal> a)
    {
        UITaskDisplay Temp = Instantiate(TaskPrefab, transform).GetComponent<UITaskDisplay>();

        Temp.Create(Mission,a);
    }

}
