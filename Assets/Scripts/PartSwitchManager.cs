using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSwitchManager : MonoBehaviour
{
    public BigCataGory CurrentCatagory;
    public int CurrentPosition;

    private List<LoadOutPart> CurrentListOfParts = new List<LoadOutPart>();

    [SerializeField]
    private GameObject ListOptionPrefab;
    [SerializeField]
    private Transform ListParent;
    [SerializeField]
    private GameObject BodyPartsSelection;
    [SerializeField]
    private GameObject WeaponsAndEXGSelection;
    [SerializeField]
    private UnityEngine.UI.Text CatagoryTitle;


    [SerializeField]
    private UnityEngine.UI.Button[] EXGAndWeaponButtons = new UnityEngine.UI.Button[8];

    private MechAssemblyRack AssemblyRack;
    private List<AssemblyPartOption> CurrentDisplayedOptions = new List<AssemblyPartOption>();      //needs to impliment a object pool system for the options

    public enum BigCataGory
    {
        Head,
        Torso,
        Arms,
        Legs,
        Pack,
        BoostSystem,
        EnergySystem,

        MainWeapon,
        ShoulderEXG,
        SideEXG,

    }

    private void Start()
    {
        AssemblyRack = FindObjectOfType<MechAssemblyRack>();

        AssemblyRack.StarterLoad();

        LoadList();
        CreateOptionsForCurrentList();
        SetCatagoryTitle();
        DetermineEXGAndWeaponSlots();

    }

    public void InstallPart(GameObject Part)
    {
        AssemblyRack.FitNewPart(CurrentCatagory, CurrentPosition, Part);
    }

    public BigCataGory GetCurrentBigCatagory()
    {
        return CurrentCatagory;
    }

    #region Button Functions

    public void SelectCatagory(int Cata)
    {
        CurrentCatagory = (BigCataGory)Cata;

        if (Cata < 7)
        {
            ClearOptions();
            LoadList();
            CreateOptionsForCurrentList();
            SetCatagoryTitle();
        }

    }

    public void SelectPosition(int Pos)
    {
        CurrentPosition = Pos;

        ClearOptions();
        LoadList();
        CreateOptionsForCurrentList();
        SetCatagoryTitle();
    }

    public void SwitchCatagory()
    {
        if (BodyPartsSelection.active)
        {
            BodyPartsSelection.SetActive(false);
            WeaponsAndEXGSelection.SetActive(true);
            DetermineEXGAndWeaponSlots();



        }
        else
        {
            BodyPartsSelection.SetActive(true);
            WeaponsAndEXGSelection.SetActive(false);
        }
    }

    public void DetermineEXGAndWeaponSlots()
    {
        List<bool> Temp = AssemblyRack.GetEXGAndWeaponSlots();

        for (int i = 0; i < 8; i++)
        {
            EXGAndWeaponButtons[i].interactable = Temp[i];
        }
    }

    #endregion

    private void LoadList()
    {
        CurrentListOfParts.Clear();

        string Path = "";

        if (CurrentCatagory == BigCataGory.MainWeapon)
        {
            Path += "MainWeapons/";
        }
        else if (CurrentCatagory == BigCataGory.ShoulderEXG)
        {
            Path += "ExtraGears/Shoulder/";
        }
        else if (CurrentCatagory == BigCataGory.SideEXG)
        {
            Path += "ExtraGears/Side/";
        }
        else
        {
            Path += "MechParts/";
            if (CurrentCatagory == BigCataGory.Head)
                Path += "Head/";
            else if (CurrentCatagory == BigCataGory.Torso)
                Path += "Torso/";
            else if (CurrentCatagory == BigCataGory.Arms)
                Path += "Arms/";
            else if (CurrentCatagory == BigCataGory.Legs)
                Path += "Legs/";
            else if (CurrentCatagory == BigCataGory.Pack)
                Path += "Pack/";
            else if (CurrentCatagory == BigCataGory.BoostSystem)
                Path += "BoostSystem/";
            else if (CurrentCatagory == BigCataGory.EnergySystem)
                Path += "EnergySystem/";
        }


        CurrentListOfParts.Clear();
        if (CurrentCatagory == BigCataGory.MainWeapon || CurrentCatagory == BigCataGory.ShoulderEXG || CurrentCatagory == BigCataGory.SideEXG)
            CurrentListOfParts.Add(null);
        CurrentListOfParts.AddRange(Resources.LoadAll<LoadOutPart>(Path));
        Debug.Log(CurrentListOfParts.Count);
    }

    private void ClearOptions()
    {
        //needs to impliment a object pool system for the options

        CurrentListOfParts.Clear();

        for (int i = 0; i < CurrentDisplayedOptions.Count; i++)
        {
            Destroy(CurrentDisplayedOptions[i].gameObject);
        }
        CurrentDisplayedOptions.Clear();
    }

    private void SetCatagoryTitle()
    {
        string Temp = "";

        if (CurrentCatagory == BigCataGory.MainWeapon)
        {
            if (CurrentPosition == 0)
                Temp += "Main Hand Equipment";
            else if (CurrentPosition == 1)
                Temp += "Off Hand Equipment";
        }
        else if (CurrentCatagory == BigCataGory.SideEXG || CurrentCatagory == BigCataGory.ShoulderEXG)
        {
            Temp += "EXG: ";

            if (CurrentPosition == 0)
                Temp += "Left Leg";
            else if (CurrentPosition == 1)
                Temp += "Left Arm";
            else if (CurrentPosition == 2)
                Temp += "Left Shoulder";
            else if (CurrentPosition == 5)
                Temp += "Right Shoullder";
            else if (CurrentPosition == 6)
                Temp += "Right Arm";
            else if (CurrentPosition == 7)
                Temp += "Right Leg";
        }
        else
        {
            Temp += "Mech Part: ";

            if (CurrentCatagory == BigCataGory.Head)
                Temp += "Head";
            else if (CurrentCatagory == BigCataGory.Torso)
                Temp += "Torso";
            else if (CurrentCatagory == BigCataGory.Arms)
                Temp += "Arms";
            else if (CurrentCatagory == BigCataGory.Pack)
                Temp += "BackPack";
            else if (CurrentCatagory == BigCataGory.Legs)
                Temp += "Legs";

            else if (CurrentCatagory == BigCataGory.EnergySystem)
                Temp = "Energy Systems";
            else if (CurrentCatagory == BigCataGory.BoostSystem)
                Temp = "Boost Systems";
        }

        CatagoryTitle.text = Temp;
    }

    private void CreateOptionsForCurrentList()
    {
        foreach (LoadOutPart a in CurrentListOfParts)
        {
            CreateLoadOutOption(a);
        }
    }

    private void CreateLoadOutOption(LoadOutPart a)
    {
        //needs to impliment a object pool system for the options

        if (a && a.HideForPlayer) //parts that are tagged with this is hidden, mainly used for testing and locking unfinished parts
            return;

        AssemblyPartOption NewOption = Instantiate(ListOptionPrefab, ListParent).GetComponent<AssemblyPartOption>();

        CurrentDisplayedOptions.Add(NewOption);

        NewOption.SetUp(this, a);
    }


}
