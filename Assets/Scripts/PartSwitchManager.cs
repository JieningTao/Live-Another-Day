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
        }

    }

    public void SelectPosition(int Pos)
    {
        CurrentPosition = Pos;

        ClearOptions();
        LoadList();
        CreateOptionsForCurrentList();
    }

    public void SwitchCatagory()
    {
        if (BodyPartsSelection.active)
        {
            BodyPartsSelection.SetActive(false);
            WeaponsAndEXGSelection.SetActive(true);
        }
        else
        {
            BodyPartsSelection.SetActive(true);
            WeaponsAndEXGSelection.SetActive(false);
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

        AssemblyPartOption NewOption = Instantiate(ListOptionPrefab, ListParent).GetComponent<AssemblyPartOption>();

        CurrentDisplayedOptions.Add(NewOption);

        NewOption.SetUp(this, a);
    }


}
