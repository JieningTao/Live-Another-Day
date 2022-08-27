using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LOPPathTool : EditorWindow
{
    [SerializeField]
    string Path;


    [MenuItem("Window/LOPPathTool")]
    public static void ShowWindow()
    {
        GetWindow<LOPPathTool>("LOPPathTool");
    }


    private void OnGUI()
    {
        if (GUILayout.Button("Execute"))
        {
            Execute();
        }
    }

    public void Execute()
    {
        Debug.Log("Running LOPPathTool");

        List<LoadOutPart> Temp = new List<LoadOutPart>();

        Temp.AddRange(Resources.LoadAll<LoadOutPart>(""));

        Debug.Log(Temp.Count);

        foreach (LoadOutPart a in Temp)
        {
            a.GetPrefabPath(Path);

            PrefabUtility.ApplyPrefabInstance(a.gameObject, InteractionMode.AutomatedAction);
        }

        Debug.Log("LOPPathTool Ran");
    }

}
