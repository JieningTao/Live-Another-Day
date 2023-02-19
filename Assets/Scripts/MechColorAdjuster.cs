using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechColorAdjuster : MonoBehaviour
{
    [SerializeField]
    public Material Main;
    [SerializeField]
    public Material Secondary;
    [SerializeField]
    public Material Frame;


    public void switchColor()
    {
        switchColor(gameObject);
    }

    public void RecieveMaterials(List<Material> Mats)
    {
        Main = Mats[0];
        Secondary = Mats[1];
        Frame = Mats[2];
    }

    public List<Material> ExtractMaterials()
    {
        List<Material> Temp = new List<Material>();

        Temp.Add(Main);
        Temp.Add(Secondary);
        Temp.Add(Frame);

        return Temp;
    }

    public void switchColor(GameObject Target)
    {
        List<MeshRenderer> Temp = new List<MeshRenderer>();

        Temp.AddRange(Target.GetComponentsInChildren<MeshRenderer>());

        //Debug.Log(Temp.Count);

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
