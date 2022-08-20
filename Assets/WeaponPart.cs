using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponPart 
{

    public string Name;
    public string Description;
    public string PrefabPath;

    public WeaponPart(WeaponPart Temp)
    {
        Name = Temp.Name;
        Description = Temp.Description;
        PrefabPath = Temp.PrefabPath;
    }


}
