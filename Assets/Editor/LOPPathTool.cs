using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LOPPathTool : EditorWindow
{

    private bool Patch = true;
    private bool Weapon = false;
    private bool MechPart = false;
    private bool BoostSystems = false;
    private bool FCSChip = false;
    private bool EXG = false;


    [MenuItem("Window/LOPPathTool")]
    public static void ShowWindow()
    {
        GetWindow<LOPPathTool>("LOPPathTool");
    }


    private void OnGUI()
    {
        Patch = GUILayout.Toggle(Patch, "Patch");
        Weapon = GUILayout.Toggle(Weapon, "Weapon");
        MechPart = GUILayout.Toggle(MechPart, "MechPart");
        BoostSystems = GUILayout.Toggle(BoostSystems, "BoostSystems");
        FCSChip = GUILayout.Toggle(FCSChip, "FCSChips");
        EXG = GUILayout.Toggle(EXG, "EXG");

        if (GUILayout.Button("Repath"))
        {
            if (Weapon)
                WeaponRePath();
            if (MechPart)
                MechPartRePath();
            if (BoostSystems)
                BoostSystemsRePath();
            if (FCSChip)
                FCSChipRePath();
            if (EXG)
                EXGRePath();
        }

        //if (GUILayout.Button("FullRepath"))
        //{
        //    WeaponRePath();
        //    MechPartRePath();
        //    BoostSystemsRePath();
        //    FCSChipRePath();
        //}

        //if (GUILayout.Button("Weapon Repath"))
        //{
        //    WeaponRePath();
        //}

        //if (GUILayout.Button("Mech Part Repath"))
        //{
        //    MechPartRePath();
        //}

        //if (GUILayout.Button("Mech Systems Repath"))
        //{
        //    BoostSystemsRePath();
        //    FCSChipRePath();
        //}

        //if (GUILayout.Button("EXG Part Repath"))
        //{
        //    EXGRePath();
        //}

    }
    public void WeaponRePath()
    {
        Debug.Log("Running MainSlotGear Repath");

        List<LOPMainGear> Temp = new List<LOPMainGear>();

        Temp.AddRange(Resources.LoadAll<LOPMainGear>(""));

        Debug.Log(Temp.Count);



        foreach (LOPMainGear a in Temp)
        {
            if (!Patch || a.PrefabPath == "")
            {
                string Path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(a.gameObject);

                Path = Path.Replace("Assets/Prefabs/Resources/", "");

                Path = Path.Replace(".prefab", "");

                a.PrefabPath = Path;

                a.PartCatagory = PartSwitchManager.BigCataGory.MainWeapon;

                if (a.Name == "")
                    a.Name = "** " + a.gameObject.name;

                if(!a.MyEquipment)
                a.MyEquipment = a.GetComponent<BaseMainSlotEquipment>();


                PrefabUtility.SavePrefabAsset(a.gameObject);
            }

        }

        Debug.Log("MainSlotGear Repath Ran");
    }

    public void MechPartRePath()
    {
        Debug.Log("Running MechPart Repath");

        List<LOPMechPart> Temp = new List<LOPMechPart>();

        Temp.AddRange(Resources.LoadAll<LOPMechPart>(""));

        Debug.Log(Temp.Count);



        foreach (LOPMechPart a in Temp)
        {
            if (!Patch || a.PrefabPath == "")
            {
                string Path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(a.gameObject);

                Path = Path.Replace("Assets/Prefabs/Resources/", "");

                Path = Path.Replace(".prefab", "");

                a.PrefabPath = Path;

                if (a.Name == "")
                    a.Name = "** " + a.gameObject.name;

                if (!a.MyPart)
                    a.MyPart = a.GetComponent<BaseMechPart>();


                PrefabUtility.SavePrefabAsset(a.gameObject);
            }

        }

        Debug.Log("MechPart Repath Ran");
    }

    public void BoostSystemsRePath()
    {
        Debug.Log("Running BoostSystem Repath");

        List<LOPBoostSystem> Temp = new List<LOPBoostSystem>();

        Temp.AddRange(Resources.LoadAll<LOPBoostSystem>(""));

        Debug.Log(Temp.Count);



        foreach (LOPBoostSystem a in Temp)
        {
            if (!Patch || a.PrefabPath == "")
            {
                string Path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(a.gameObject);

                Path = Path.Replace("Assets/Prefabs/Resources/", "");

                Path = Path.Replace(".prefab", "");

                a.PartCatagory = PartSwitchManager.BigCataGory.BoostSystem;

                a.PrefabPath = Path;

                if (a.Name == "")
                    a.Name = "** " + a.gameObject.name;

                if (!a.MyBS)
                    a.MyBS = a.GetComponent<BaseBoostSystem>();


                PrefabUtility.SavePrefabAsset(a.gameObject);
            }

        }

        Debug.Log("BoostSystem Repath Ran");
    }

    public void FCSChipRePath()
    {
        Debug.Log("Running FCSChip Repath");

        List<LOPFCSChip> Temp = new List<LOPFCSChip>();

        Temp.AddRange(Resources.LoadAll<LOPFCSChip>(""));

        Debug.Log(Temp.Count);



        foreach (LOPFCSChip a in Temp)
        {
            if (!Patch || a.PrefabPath == "")
            {
                string Path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(a.gameObject);

                Path = Path.Replace("Assets/Prefabs/Resources/", "");

                Path = Path.Replace(".prefab", "");

                a.PartCatagory = PartSwitchManager.BigCataGory.FCSChip;

                a.PrefabPath = Path;

                if (a.Name == "")
                    a.Name = "** " + a.gameObject.name;

                if (!a.MyChip)
                    a.MyChip = a.GetComponent<BaseFCSChip>();


                PrefabUtility.SavePrefabAsset(a.gameObject);
            }

        }

        Debug.Log("FCSChip Repath Ran");
    }

    public void EXGRePath()
    {
        Debug.Log("Running EXG Repath");

        List<LOPEXG> Temp = new List<LOPEXG>();

        Temp.AddRange(Resources.LoadAll<LOPEXG>(""));

        Debug.Log(Temp.Count);



        foreach (LOPEXG a in Temp)
        {
            if (!Patch || a.PrefabPath == "")
            {
                string Path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(a.gameObject);

                Path = Path.Replace("Assets/Prefabs/Resources/", "");

                Path = Path.Replace(".prefab", "");

                a.PrefabPath = Path;

                if (a.Name == "")
                {
                    if (a.name.Contains("'"))
                        a.Name = a.gameObject.name;
                    else
                        a.Name = "** " + a.gameObject.name;
                }

                if (!a.MyEXG)
                    a.MyEXG = a.GetComponent<BaseEXGear>();


                PrefabUtility.SavePrefabAsset(a.gameObject);
            }

        }

        Debug.Log("EXG Repath Ran");
    }

    //public void ShieldRePath()
    //{
    //    Debug.Log("Running Shield Repath");

    //    List<LOPMainGear> Temp = new List<LOPMainGear>();

    //    Temp.AddRange(Resources.LoadAll<LOPMainGear>(""));

    //    Debug.Log(Temp.Count);



    //    foreach (LOPMainGear a in Temp)
    //    {
    //        if (!Patch || a.PrefabPath == "")
    //        {
    //            string Path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(a.gameObject);

    //            Path = Path.Replace("Assets/Prefabs/Resources/", "");

    //            Path = Path.Replace(".prefab", "");

    //            a.PrefabPath = Path;

    //            if (a.Name == "")
    //            {
    //                if (a.name.Contains("'"))
    //                    a.Name = a.gameObject.name;
    //                else
    //                    a.Name = "** " + a.gameObject.name;
    //            }

    //            if (!a.MyEquipment)
    //                a.MyEXG = a.GetComponent<BaseEXGear>();


    //            PrefabUtility.SavePrefabAsset(a.gameObject);
    //        }

    //    }

    //    Debug.Log("EXG Repath Ran");
    //}

}
