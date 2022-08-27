using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOutPart : MonoBehaviour
{

    public string Name;
    public string Description;
    public PartSwitchManager.BigCataGory PartCatagory;
    public string PrefabPath;



    public LoadOutPart LoadPrefab()
    {
        Debug.Log(Resources.Load<LoadOutPart>(PrefabPath));
        return Resources.Load<LoadOutPart>(PrefabPath);
    }

    public bool IsPrefab()
    {
        return this == LoadPrefab();
    }

    public void GetPrefabPath()
    {
        PrefabPath = gameObject.name;
    }

    public void GetPrefabPath(string a)
    {
        PrefabPath = a + gameObject.name;
    }
}
