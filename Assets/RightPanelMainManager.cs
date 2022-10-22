using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPanelMainManager : MonoBehaviour
{
    [SerializeField]
    GameObject PartAndGearScreen;
    [SerializeField]
    GameObject ColorScreen;



    public void SetPartAndGear()
    {
        if (!PartAndGearScreen.active)
        {
            PartAndGearScreen.SetActive(true);
            ColorScreen.SetActive(false);
        }
    }

    public void SetColor()
    {
        if (!ColorScreen.active)
        {
            PartAndGearScreen.SetActive(false);
            ColorScreen.SetActive(true);
        }
    }
}
