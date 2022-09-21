using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveCoder
{
    //  | --- seperates same catagory parts
    //  [c] --- seperates bigger catagorys

    public static string ConvertLoadoutToString(List<List<LoadOutPart>>Loadout)
    {
        string Temp = "";

        foreach (List<LoadOutPart> a in Loadout)
        {
            Temp += ConvertPartsToString(a) + "[c]";
        }

        Temp.Remove(Temp.Length - 3);

        return Temp;
    }

    public static string ConvertPartsToString(List<LoadOutPart> Parts)
    {
        string Temp = "";

        foreach (LoadOutPart a in Parts)
        {
            if (a)
            {
                Temp += a.PrefabPath + "|";
            }
            else
                Temp += "|";
        }

        Temp.Remove(Temp.Length - 1);

        return Temp;
    }

    public static List<LoadOutPart> LoadParts(string PartsPaths)
    {
        List<string> Temp = new List<string>();

        Temp.AddRange( PartsPaths.Split(new string[] { "|" }, System.StringSplitOptions.None));

        List<LoadOutPart> ReturnList = new List<LoadOutPart>();

        foreach (string a in Temp)
        {
            ReturnList.Add(Resources.Load<LoadOutPart>(a));
        }

        return ReturnList;
    }

    public static List<List<LoadOutPart>> LoadLoadout(string LoadoutPaths)
    {
        List<string> Temp = new List<string>();

        Temp.AddRange(LoadoutPaths.Split(new string[] { "[c]" }, System.StringSplitOptions.None));

        List<List<LoadOutPart>> ReturnList = new List<List<LoadOutPart>>();

        ReturnList.Add(LoadParts(Temp[0]));
        ReturnList.Add(LoadParts(Temp[1]));
        ReturnList.Add(LoadParts(Temp[2]));

        return ReturnList;
    }











}
