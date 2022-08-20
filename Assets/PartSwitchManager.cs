using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSwitchManager : MonoBehaviour
{
    public BigCataGory CurrentCatagory;
    public int CurrentPosition;

    public enum BigCataGory
    {
        MechPart,
        Weapon,
        EXG,
    }

    public static event Action<int, int, GameObject> InstallNewPart; // 0 - MechPart, 1 - MainWeapon, 2 - EXG 




    public void InstallPart(GameObject Part)
    {
        InstallNewPart.Invoke((int)CurrentCatagory, CurrentPosition, Part);
    }

    public BigCataGory GetCurrentBigCatagory()
    {
        return CurrentCatagory;
    }

}
