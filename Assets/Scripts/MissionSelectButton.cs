using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelectButton : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image Icon;
    [SerializeField]
    UnityEngine.UI.Text SerialNumber;
    [SerializeField]
    UnityEngine.UI.Text MissionName;

    MMMissionSelect MyManager;
    string LinkedSceneName;

    public void Init(MMMissionSelect Manager, Color IconColor, string Serial,string Name, string SceneName)
    {
        MyManager = Manager;
        Icon.color = IconColor;
        SerialNumber.text = Serial;
        MissionName.text = Name;
        LinkedSceneName = SceneName; 
    }

    public void Click()
    {
        Debug.Log(LinkedSceneName);
        MyManager.LoadScene(LinkedSceneName);
    }

}
