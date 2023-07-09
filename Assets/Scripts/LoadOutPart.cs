using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOutPart : MonoBehaviour
{

    public string Name;
    public string Classification;
    public string Description;
    public Sprite[] InfoSprites = new Sprite[4];
    public bool HideForPlayer = false;
    [SerializeField]
    public List<string> UnlockRequiredTags;
    public PartSwitchManager.BigCataGory PartCatagory;
    public string PrefabPath;



    public LoadOutPart LoadPrefab()
    {
        //Debug.Log(Resources.Load<LoadOutPart>(PrefabPath));
        return Resources.Load<LoadOutPart>(PrefabPath);
    }

    public bool IsPrefab()
    {
        return this == LoadPrefab();
    }

    //public void GetPrefabPath()
    //{
    //    PrefabPath = gameObject.name;
    //}

    //public void GetPrefabPath(string a)
    //{
    //    PrefabPath = a + gameObject.name;
    //}

    public virtual bool CheckUnlocked()
    {
        if (UnlockRequiredTags.Count == 0)
            return true;

        foreach (string a in UnlockRequiredTags)
        {
            if (!UnlockTagTracker.Instance.UnlockTags.Contains(a))
                return false;
        }

        return true;
    }

    public virtual List<string> GetStats()
    {
        return null;
    }
}
