using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MechPart 
{
    public string Name;
    public MechPartType PartType;
    public enum MechPartType
    {
        Head,
        Torso,
        LArm,
        RArm,
        Legs,
        BackPack,
        Systems,
    }
    public string Description;
    public string PrefabPath;

    public MechPart(MechPart Temp)
    {
        Name = Temp.Name;
        PartType = Temp.PartType;
        Description = Temp.Description;
        PrefabPath = Temp.PrefabPath;
    }

}
