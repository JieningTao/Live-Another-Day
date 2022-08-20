using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechColorAdjuster : MonoBehaviour
{
    [SerializeField]
    Material Main;
    [SerializeField]
    Material Secondary;
    [SerializeField]
    Material Frame;


    public void switchColor()
    {
        List<MeshRenderer> Temp = new List<MeshRenderer>();

        Temp.AddRange(GetComponentsInChildren<MeshRenderer>());

        Debug.Log(Temp.Count);

        foreach (MeshRenderer a in Temp)
        {
            Material[] TempML = a.materials;

            for (int i = 0; i < a.materials.Length; i++)
            {
                if (Main)
                {
                    if (a.materials[i].name == "Body (Instance)")
                        TempML[i] = Main;
                }

                if (Secondary)
                {
                    if (a.materials[i].name == "Brown (Instance)")
                        TempML[i] = Secondary;
                }

                if (Frame)
                {
                    if (a.materials[i].name == "Gray (Instance)")
                        TempML[i] = Frame;
                }
            }

            a.materials = TempML;
        }


    }
}
