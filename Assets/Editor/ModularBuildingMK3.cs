using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ModularBuildingMK3 : EditorWindow
{

    Vector3 Dimensions = new Vector3(10, 10, 10);
    Vector3 Divisions = new Vector3(1, 1, 1);

    List<GameObject> Beams = new List<GameObject>();
    List<GameObject> BeamCaps = new List<GameObject>();
    List<GameObject> Walls = new List<GameObject>();

    bool RoofCap = false;
    [Range(0, 1f)]
    float WallChance = 0.5f;

    [MenuItem("Window/ModularBuilding")]
    public static void ShowWindow()
    {
        GetWindow<ModularBuildingMK3>("MB.MK3");
    }

    private void OnGUI()
    {
        Dimensions = EditorGUILayout.Vector3Field("Dimensions:", Dimensions);
        Divisions = EditorGUILayout.Vector3Field("Divisions:", Divisions);
        RoofCap = GUILayout.Toggle(RoofCap, "RoofCap");
        WallChance = EditorGUILayout.FloatField("Wall Chance", WallChance);



        if (GUILayout.Button("Construct"))
        {
            Construct();
        }

        if (GUILayout.Button("Load Components"))
        {
            GetComponentsFromParent();
        }

    }

    private void GetComponentsFromParent()
    {
        Beams.Clear();
        Beams.AddRange(Resources.LoadAll<GameObject>("ModularBuildingMK3/Beams/"));
        Debug.Log(Beams.Count + " Beams Loaded");

        BeamCaps.Clear();
        BeamCaps.AddRange(Resources.LoadAll<GameObject>("ModularBuildingMK3/Caps/"));
        Debug.Log(BeamCaps.Count + " Caps Loaded");

        Walls.Clear();
        Walls.AddRange(Resources.LoadAll<GameObject>("ModularBuildingMK3/Walls/"));
        Debug.Log(Walls.Count + " Walls Loaded");
    }

    private void Construct()
    {
        List<GameObject> Buildings = new List<GameObject>();
        Buildings.AddRange(Selection.gameObjects);

        foreach (GameObject a in Buildings)
        {
            GameObject Temp = a;


            if (Temp == null)
            {
                Debug.LogError("Please select an empty gameobject to be the building parent");
                return;
            }


            GameObject VBParent = new GameObject("VBParent");
            VBParent.transform.parent = Temp.transform;
            VBParent.transform.localPosition = Vector3.zero;

            GameObject WParent = new GameObject("WParent");
            WParent.transform.parent = Temp.transform;
            WParent.transform.localPosition = Vector3.zero;

            GameObject HBParent = new GameObject("HBParent");
            HBParent.transform.parent = Temp.transform;
            HBParent.transform.localPosition = Vector3.zero;

            VerticalBeams(VBParent);
            HorizontalBeams(HBParent);
            CapEnds(VBParent);
            FillXYWalls(WParent);
            FillYZWalls(WParent);

            if (RoofCap)
                CapRoof(Temp);

        }

    }

    private void VerticalBeams(GameObject Parent)
    {
        for (int i = 0; i <= Divisions.x; i++)
        {
            for (int j = 0; j <= Divisions.z; j++)
            {
                if ((i == 0 || i == (int)Divisions.x) || (j == 0 || j == (int)Divisions.z))
                {
                    GameObject NewBeam = GetBeam(Parent.transform,true);
                    NewBeam.transform.localScale = new Vector3(1, Dimensions.y, 1);
                    NewBeam.transform.localPosition = new Vector3(Dimensions.x / (int)Divisions.x * i, 0, Dimensions.z / (int)Divisions.z * j);
                }
            }
        }

    }

    private void HorizontalBeams(GameObject Parent)
    {

        



        for (int i = 0; i <= Divisions.y; i++)
        {
            GameObject LevelParent = new GameObject("Horizontal Divide "+ i);

            LevelParent.transform.parent = Parent.transform;
            LevelParent.transform.localPosition = new Vector3(0, i * (Dimensions.y / (int)Divisions.y), 0);

            GameObject NewBeam1 = GetBeam(LevelParent.transform, true); 
            NewBeam1.transform.localPosition = new Vector3(0, 0, 0);
            NewBeam1.transform.rotation = Quaternion.Euler(0, 0, -90);
            NewBeam1.transform.localScale = new Vector3(1, Dimensions.x, 1);

            GameObject NewBeam2 = GetBeam(LevelParent.transform, true); 
            NewBeam2.transform.localPosition = new Vector3(0, 0, Dimensions.z);
            NewBeam2.transform.rotation = Quaternion.Euler(0, 0, -90);
            NewBeam2.transform.localScale = new Vector3(1, Dimensions.x, 1);

            GameObject NewBeam3 = GetBeam(LevelParent.transform, true); 
            NewBeam3.transform.localPosition = new Vector3(0, 0, 0);
            NewBeam3.transform.rotation = Quaternion.Euler(90, 0, 0);
            NewBeam3.transform.localScale = new Vector3(1, Dimensions.z, 1);

            GameObject NewBeam4 = GetBeam(LevelParent.transform, true); 
            NewBeam4.transform.localPosition = new Vector3(Dimensions.x, 0, 0);
            NewBeam4.transform.rotation = Quaternion.Euler(90, 0, 0);
            NewBeam4.transform.localScale = new Vector3(1, Dimensions.z, 1);
        }

        foreach (Transform a in Parent.GetComponentsInChildren<Transform>())
        {
            if (a.gameObject.name == "CapEnd")
                a.gameObject.SetActive(false);
        }

    }

    private GameObject GetBeam(Transform Parent,bool AllowRotate)
    {
        GameObject Beam = (GameObject)PrefabUtility.InstantiatePrefab(Beams[Random.Range(0, Beams.Count)], Parent);

        if (AllowRotate)
        {
            Transform VO = Beam.transform.Find("VO");
            VO.localRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
        }
        return Beam;

    }

    private void CapEnds(GameObject Parent)
    {
        List<Transform> UncappedEnds = new List<Transform>();
        foreach (Transform a in Parent.GetComponentsInChildren<Transform>())
        {
            if (a.gameObject.name == "CapEnd")
                UncappedEnds.Add(a);
        }

        GameObject BeamCaps = new GameObject("BCParent");
        BeamCaps.transform.parent = Parent.transform;

        foreach (Transform a in UncappedEnds)
        {
            GameObject Cap = GetCap(BeamCaps.transform, true);
            Cap.transform.position = a.position;
        }

    }

    private GameObject GetCap(Transform Parent, bool AllowRotate)
    {
        GameObject Cap = (GameObject)PrefabUtility.InstantiatePrefab(BeamCaps[Random.Range(0, BeamCaps.Count)],Parent);

        if (AllowRotate)
        {
            Transform VO = Cap.transform.Find("VO");
            VO.localRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
        }
        return Cap;
    }

    //operations have been replaced by fillXY and fillYZ
    //private void FillWalls(GameObject Parent)
    //{
    //    GameObject Wall1 = Instantiate(Walls[Random.Range(0, Walls.Count)], Parent.transform);
    //    Wall1.transform.position = new Vector3(Dimensions.x/2, Dimensions.y/2, 0);
    //    Wall1.transform.localScale = new Vector3(Dimensions.x, Dimensions.y, 1);

    //    GameObject Wall2 = Instantiate(Walls[Random.Range(0, Walls.Count)], Parent.transform);
    //    Wall2.transform.position = new Vector3(Dimensions.x / 2, Dimensions.y / 2, Dimensions.z);
    //    Wall2.transform.localScale = new Vector3(Dimensions.x, Dimensions.y, 1);

    //    GameObject Wall3 = Instantiate(Walls[Random.Range(0, Walls.Count)], Parent.transform);
    //    Wall3.transform.localRotation = Quaternion.Euler(0,90,0);
    //    Wall3.transform.position = new Vector3(Dimensions.x, Dimensions.y / 2, Dimensions.z/2);
    //    Wall3.transform.localScale = new Vector3(Dimensions.z, Dimensions.y, 1);

    //    GameObject Wall4 = Instantiate(Walls[Random.Range(0, Walls.Count)], Parent.transform);
    //    Wall4.transform.localRotation = Quaternion.Euler(0, 90, 0);
    //    Wall4.transform.position = new Vector3(0, Dimensions.y / 2, Dimensions.z / 2);
    //    Wall4.transform.localScale = new Vector3(Dimensions.z, Dimensions.y, 1);
    //}

    private void FillXYWalls(GameObject Parent)
    {
        for (int i = 0; i < Divisions.y; i++)
        {
            for (int j = 0; j < Divisions.x; j++)
            {
                if (Random.Range(0, 1f) < WallChance)
                {
                    GameObject Wall = GetWall(Parent.transform, true);
                    Wall.transform.localPosition = new Vector3((Dimensions.x / (int)Divisions.x) / 2, (Dimensions.y / (int)Divisions.y) / 2, 0) + new Vector3(Dimensions.x / (int)Divisions.x * j, Dimensions.y / (int)Divisions.y * i, 0);
                    Wall.transform.localScale = new Vector3(Dimensions.x / (int)Divisions.x, Dimensions.y / (int)Divisions.y, 1);
                }

                if (Random.Range(0, 1f) < WallChance)
                {
                    GameObject Wall = GetWall(Parent.transform, true);
                    Wall.transform.localPosition = new Vector3((Dimensions.x / (int)Divisions.x) / 2, (Dimensions.y / (int)Divisions.y) / 2, 0) + new Vector3(Dimensions.x / (int)Divisions.x * j, Dimensions.y / (int)Divisions.y * i, Dimensions.z);
                    Wall.transform.localScale = new Vector3(Dimensions.x / (int)Divisions.x, Dimensions.y / (int)Divisions.y, 1);
                }
            }
        }
    }

    private void FillYZWalls(GameObject Parent)
    {
        for (int i = 0; i < Divisions.y; i++)
        {
            for (int j = 0; j < Divisions.z; j++)
            {
                if (Random.Range(0, 1f) < WallChance)
                {
                    GameObject Wall = GetWall(Parent.transform, true);
                    Wall.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    Wall.transform.localPosition = new Vector3(0, (Dimensions.y / (int)Divisions.y) / 2, (Dimensions.z / (int)Divisions.z) / 2) + new Vector3(0, Dimensions.y / (int)Divisions.y * i, Dimensions.z / (int)Divisions.z * j);
                    Wall.transform.localScale = new Vector3(Dimensions.z / (int)Divisions.z, Dimensions.y / (int)Divisions.y, 1);
                }

                if (Random.Range(0, 1f) < WallChance)
                {
                    GameObject Wall = GetWall(Parent.transform, true);
                    Wall.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    Wall.transform.localPosition = new Vector3(Dimensions.x, (Dimensions.y / (int)Divisions.y) / 2, (Dimensions.z / (int)Divisions.z) / 2) + new Vector3(0, Dimensions.y / (int)Divisions.y * i, Dimensions.z / (int)Divisions.z * j);
                    Wall.transform.localScale = new Vector3(Dimensions.z / (int)Divisions.z, Dimensions.y / (int)Divisions.y, 1);
                }
            }
        }
    }

    private GameObject GetWall(Transform Parent, bool AllowRotate)
    {
        GameObject Wall = (GameObject)PrefabUtility.InstantiatePrefab(Walls[Random.Range(0, Walls.Count)], Parent);

        if (AllowRotate)
        {
            Transform VO = Wall.transform.Find("VO");
            VO.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);
        }
        return Wall;
    }

    private void CapRoof(GameObject Parent)
    {
        GameObject Floor = (GameObject)PrefabUtility.InstantiatePrefab(Walls[0], Parent.transform);
        Floor.transform.localRotation = Quaternion.Euler(90, 0, 0);
        Floor.transform.localScale = new Vector3(Dimensions.x, Dimensions.z, 1);
        Floor.transform.localPosition = new Vector3(Dimensions.x / 2, 0, Dimensions.z / 2);

        GameObject Roof = (GameObject)PrefabUtility.InstantiatePrefab(Walls[0], Parent.transform);
        Roof.transform.localRotation = Quaternion.Euler(90, 0, 0);
        Roof.transform.localScale = new Vector3(Dimensions.x, Dimensions.z, 1);
        Roof.transform.localPosition = new Vector3(Dimensions.x / 2, Dimensions.y, Dimensions.z / 2);
    }
}
