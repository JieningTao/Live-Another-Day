using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMMissionSelect : MonoBehaviour
{
    [SerializeField]
    MissionSelectButton Button;
    [SerializeField]
    Transform ButtonParent;

    [SerializeField]
    List<Level> AllLevels;

    [Serializable]
    public class Level
    {
        [SerializeField]
        public string SerialNumber;
        [SerializeField]
        public string MissionName;
        [SerializeField]
        public string MissionSceneName;
    }

    private void Start()
    {
        foreach (Level a in AllLevels)
        {
            CreateButton(a);
        }

    }

    public void LoadScene(string a)
    {
        Debug.Log(a);
        SceneManager.LoadScene(a);
    }

    private void CreateButton(Level a)
    {
        MissionSelectButton NewButton = GameObject.Instantiate(Button.gameObject,ButtonParent).GetComponent<MissionSelectButton>();
        NewButton.Init(this, Color.green, a.SerialNumber, a.MissionName,a.MissionSceneName);

    }



}
