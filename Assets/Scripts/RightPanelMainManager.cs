using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPanelMainManager : MonoBehaviour
{
    [SerializeField]
    GameObject PartAndGearScreen;
    [SerializeField]
    GameObject ColorScreen;
    [SerializeField]
    GameObject LoadoutScreen;



    public void SetPartAndGear()
    {
        if (!PartAndGearScreen.active)
        {
            PartAndGearScreen.SetActive(true);
            ColorScreen.SetActive(false);
            LoadoutScreen.SetActive(false);
        }
    }

    public void SetColor()
    {
        if (!ColorScreen.active)
        {
            PartAndGearScreen.SetActive(false);
            ColorScreen.SetActive(true);
            LoadoutScreen.SetActive(false);
        }
    }

    public void SetLoadout()
    {
        if(!LoadoutScreen.active)
        {
            LoadoutScreen.SetActive(true);
            PartAndGearScreen.SetActive(false);
            ColorScreen.SetActive(false);
        }
    }
}
