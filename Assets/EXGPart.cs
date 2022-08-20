using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EXGPart 
{

    public string Name;
    public string Description;
    public string PrefabPath;

    public EXGPart(EXGPart Temp)
    {
        Name = Temp.Name;
        Description = Temp.Description;
        PrefabPath = Temp.PrefabPath;
    }
}
