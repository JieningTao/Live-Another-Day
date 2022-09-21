using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LOPPathTool : EditorWindow
{
    [MenuItem("Window/LOPPathTool")]
    public static void ShowWindow()
    {
        GetWindow<LOPPathTool>("LOPPathTool");
    }


    private void OnGUI()
    {
        if (GUILayout.Button("FullRepath"))
        {
            FullRePath();
        }

        if (GUILayout.Button("PatchRepath"))
        {
            PatchRePath();
        }

    }

    public void FullRePath()
    {
        Debug.Log("Running LOPPathTool");

        List<LoadOutPart> Temp = new List<LoadOutPart>();

        Temp.AddRange(Resources.LoadAll<LoadOutPart>(""));

        Debug.Log(Temp.Count);

      

        foreach (LoadOutPart a in Temp)
        {

            string Path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(a.gameObject);

            Path = Path.Replace("Assets/Prefabs/Resources/", "");

            Path = Path.Replace(".prefab", "");

            a.PrefabPath = Path;

            if (a.Name == "")
                a.Name = "** "+ a.gameObject.name;
            

            PrefabUtility.SavePrefabAsset(a.gameObject);
        }

        Debug.Log("LOPPathTool Ran");
    }

    public void PatchRePath()
    {
        Debug.Log("Running LOPPathTool");

        List<LoadOutPart> Temp = new List<LoadOutPart>();

        Temp.AddRange(Resources.LoadAll<LoadOutPart>(""));

        Debug.Log(Temp.Count);



        foreach (LoadOutPart a in Temp)
        {
            if (a.PrefabPath == "")
            {
                string Path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(a.gameObject);

                Path = Path.Replace("Assets/Prefabs/Resources/", "");

                Path = Path.Replace(".prefab", "");

                a.PrefabPath = Path;

                if (a.Name == "")
                    a.Name = "** " + a.gameObject.name;


                PrefabUtility.SavePrefabAsset(a.gameObject);
            }

        }

        Debug.Log("LOPPathTool Ran");
    }



}
