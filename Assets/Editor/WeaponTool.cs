using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WeaponTool : EditorWindow
{

    [MenuItem("Window/WeaponTool")]
    public static void ShowWindow()
    {
        GetWindow<WeaponTool>("WeaponTool");
    }


    private void OnGUI()
    {

        if (GUILayout.Button("Test"))
        {
            GUIUtility.systemCopyBuffer = "Test";
        }

        if (GUILayout.Button("GetProjectileSpawn"))
        {
            GetProjectileSpawn();
        }

    }

    public void Test()
    {
        GameObject Temp = Selection.activeGameObject;
        if (Temp)
        {
            Debug.Log(Temp.gameObject.name);
        }
        else
        {
            Debug.Log("No Selected Object");
        }
        
    }

    public void GetProjectileSpawn()
    {
        GameObject Temp = Selection.activeGameObject;
        if (Temp)
        {
            BaseShoot a = Temp.GetComponent<BaseShoot>();

            if (a)
            {
                List<Transform> BSs = new List<Transform>();

                foreach (Transform t in a.GetComponentsInChildren<Transform>(true))
                {
                    if (t.gameObject.name.Contains("PS"))
                        BSs.Add(t);
                }

                a.RecieveBSs(BSs);

                Debug.Log("Recieved " + BSs.Count + " transforms as projectile spawn");
            }
            else
                Debug.Log("Selected Object have no BS");
        }
        else
        {
            Debug.Log("No Selected Object");
        }

    }

}
