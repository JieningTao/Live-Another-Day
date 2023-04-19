using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelectButton : MonoBehaviour
{
    [SerializeField]
    Image Icon;
    [SerializeField]
    GameObject NewAnimatedIcon;
    [SerializeField]
    Text SerialNumber;
    [SerializeField]
    Text MissionName;

    [Space(20)]

    [SerializeField]
    Image Background;
    [SerializeField]
    Color UncompletedColor;
    [SerializeField]
    Color CompletedColor;
    

    MMMissionSelect MyManager;
    string LinkedSceneName;
    string Serial;

    public void Init(MMMissionSelect Manager, Color IconColor, string _Serial,string Name, string SceneName)
    {
        MyManager = Manager;
        Icon.color = IconColor;

        SerialNumber.text = _Serial;
        Serial = _Serial;

        MissionName.text = Name;
        LinkedSceneName = SceneName; 

        if(MissionCompletionTracker.Instance.GetMissionStatus(Serial))
        {
            NewAnimatedIcon.SetActive(false);
            Background.color = CompletedColor;
        }
        else
        {
            Background.color = UncompletedColor;
        }
    }

    public void Click()
    {
        Debug.Log(LinkedSceneName);
        MissionCompletionTracker.Instance.LoadPlayingMission(Serial);
        MyManager.LoadScene(LinkedSceneName);
    }

}
