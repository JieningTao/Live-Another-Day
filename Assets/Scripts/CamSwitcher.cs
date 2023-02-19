using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitcher : MonoBehaviour
{
    [SerializeField]
    List<Camera> Cams;

    private int CurrentActiveCamNum = 0;

    private void Start()
    {
        SwitchToCam(0);
    }

    private void Update()
    {
        //no out of bounds detection for the number button to cameras

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (CurrentActiveCamNum == Cams.Count - 1)
                CurrentActiveCamNum = 0;
            else
                CurrentActiveCamNum++;

            SwitchToCam(CurrentActiveCamNum);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (CurrentActiveCamNum == 0)
                CurrentActiveCamNum = Cams.Count - 1;
            else
                CurrentActiveCamNum--;

            SwitchToCam(CurrentActiveCamNum);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            CurrentActiveCamNum = 0;
            SwitchToCam(CurrentActiveCamNum);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            CurrentActiveCamNum = 1;
            SwitchToCam(CurrentActiveCamNum);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            CurrentActiveCamNum = 2;
            SwitchToCam(CurrentActiveCamNum);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            CurrentActiveCamNum = 3;
            SwitchToCam(CurrentActiveCamNum);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            CurrentActiveCamNum = 4;
            SwitchToCam(CurrentActiveCamNum);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            CurrentActiveCamNum = 5;
            SwitchToCam(CurrentActiveCamNum);
        }
    }


    private void SwitchToCam(int CamNum)
    {
        for (int i = 0; i < Cams.Count; i++)
        {
            if (i == CurrentActiveCamNum)
                Cams[i].gameObject.SetActive(true);
            else
                Cams[i].gameObject.SetActive(false);
        }
    }
}
