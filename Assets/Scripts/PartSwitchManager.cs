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
    private Animator CataAnimator;
    [SerializeField]
    private GameObject WeaponsAndEXGSelection;
    [SerializeField]
    private UnityEngine.UI.Text CatagoryTitle;


    [SerializeField]
    private UnityEngine.UI.Button[] EXGAndWeaponButtons = new UnityEngine.UI.Button[8];

    [SerializeField]
    private UIPartCompare PartCompareDisplay;

    [SerializeField]
    private RandomAudioSelector PartEquipSound;
    [SerializeField]
    private RandomAudioSelector ButtonSound;

    private MechAssemblyRack AssemblyRack;
    private List<AssemblyPartOption> CurrentDisplayedOptions = new List<AssemblyPartOption>();      //needs to impliment a object pool system for the options
    LoadOutPart CurrentSelectedPart;

    public enum BigCataGory
    {
        Head,
        Torso,
        Arms,
        Legs,
        Pack,
        BoostSystem,
        FCSChip,

        MainWeapon,
        ShoulderEXG,
        SideEXG,

    }

    private void Start()
    {
        AssemblyRack = FindObjectOfType<MechAssemblyRack>();

        AssemblyRack.StarterLoad();

        //LoadList();
        //CreateOptionsForCurrentList();
        //SetCatagoryTitle();
        //DetermineEXGAndWeaponSlots();

    }

    public void InstallPart(GameObject Part)
    {
        AssemblyRack.FitNewPart(CurrentCatagory, CurrentPosition, Part);
        PartEquipSound.Play();
    }



    public BigCataGory GetCurrentBigCatagory()
    {
        return CurrentCatagory;
    }

    #region Button Functions

    public void ButtonClickedPart(LoadOutPart Part)
    {
        

        if (Part == CurrentSelectedPart )
        {
            //installs part
            if (Part)
                InstallPart(Part.gameObject);
            else
                InstallPart(null);
            PartCompareDisplay.LoadCurrentPart(AssemblyRack.GetpostionPart(CurrentCatagory, CurrentPosition));
        }
        else
        {
            //selects part
            ButtonSound.Play();
            CurrentSelectedPart = Part;
            PartCompareDisplay.LoadSelectedPart(Part);
        }
    }

    public void SelectCatagory(int Cata)
    {
        CurrentCatagory = (BigCataGory)Cata;

        if (Cata < 7)
        {
            RecreateListButtons();
        }

    }

    public void SelectPosition(int Pos)
    {
        CurrentPosition = Pos;
        RecreateListButtons();
    }

    public void SwitchCatagory()
    {
        //if (BodyPartsSelection.active)
        //{
        //    BodyPartsSelection.SetActive(false);
        //    WeaponsAndEXGSelection.SetActive(true);
        //    DetermineEXGAndWeaponSlots();
        //}
        //else
        //{
        //    BodyPartsSelection.SetActive(true);
        //    WeaponsAndEXGSelection.SetActive(false);
        //}

        if (CataAnimator.GetBool("BP"))
        {
            CataAnimator.SetBool("BP", false);
        }
        else
        {
            CataAnimator.SetBool("BP", true);
            DetermineEXGAndWeaponSlots();
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

    private void RecreateListButtons()
    {
        ClearOptions();
        LoadList();
        CreateOptionsForCurrentList();
        SetCatagoryTitle();

        PartCompareDisplay.LoadCurrentPart(AssemblyRack.GetpostionPart(CurrentCatagory, CurrentPosition));
        CurrentSelectedPart = null;
        PartCompareDisplay.LoadSelectedPart(null);
    }

    #region show display use functions

    public void SlotRandomPart()
    {
        int Cata = UnityEngine.Random.Range(0, 9);
        Debug.Log(Cata);
        CurrentCatagory = (BigCataGory)Cata;
        if (Cata < 5)
        {
            LoadList();
            RandomSelectPart();
            return;
        }

        Cata+=2;

        int Pos = 0;

        if (Cata == 7)
        {
            Pos = UnityEngine.Random.Range(0, 2);
            CurrentCatagory = BigCataGory.MainWeapon;
        }
        else
        {
            Pos = UnityEngine.Random.Range(0, 6);
            if (Pos >= 3)
                Pos += 2;

            if (Pos == 2 || Pos == 5)
                CurrentCatagory = BigCataGory.ShoulderEXG;
            else
                CurrentCatagory = BigCataGory.SideEXG;

        }

        Debug.Log(Pos);
        CurrentPosition = Pos;
        LoadList();
        RandomSelectPart();
        return;

    }

    public void SlotRandomPart(BigCataGory Cata)
    {
        CurrentCatagory = Cata;

        if ((int)Cata < 7)
        {
            LoadList();
            RandomSelectPart();
            return;
        }

        int Pos = 0;

        if ((int)Cata == 7)
        {
            Pos = UnityEngine.Random.Range(0, 2);
            CurrentCatagory = BigCataGory.MainWeapon;
        }
        else
        {
            Pos = UnityEngine.Random.Range(0, 6);
            if (Pos >= 3)
                Pos += 2;

            if (Pos == 2 || Pos == 5)
                CurrentCatagory = BigCataGory.ShoulderEXG;
            else
                CurrentCatagory = BigCataGory.SideEXG;

        }



        Debug.Log(Pos);
        CurrentPosition = Pos;
        LoadList();
        RandomSelectPart();
        return;
    }

    public void SlotRandomEXG(int Pos)
    {
        if (Pos == 3 || Pos == 4 || Pos < 0 || Pos > 7)
        {
            Debug.Log("Random EXG out of bound error");
            return;
        }

        if (Pos == 2 || Pos == 5)
            CurrentCatagory = BigCataGory.ShoulderEXG;
        else
            CurrentCatagory = BigCataGory.SideEXG;

        CurrentPosition = Pos;


        Debug.Log(Pos);
        CurrentPosition = Pos;
        LoadList();
        RandomSelectPart();
        return;
    }

    private void RandomSelectPart()
    {
        int Temp = UnityEngine.Random.Range(0, CurrentListOfParts.Count);
        Debug.Log(CurrentListOfParts[Temp].gameObject.name);
        InstallPart(CurrentListOfParts[Temp].gameObject);
    }



    #endregion

    private void LoadList()
    {
        CurrentListOfParts.Clear();

        string Path = "";

        if (CurrentCatagory == BigCataGory.MainWeapon)
        {
            Path += "MainSlot/";
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
            else if (CurrentCatagory == BigCataGory.FCSChip)
                Path += "FCS/";
        }


        CurrentListOfParts.Clear();

        if (CurrentCatagory == BigCataGory.MainWeapon || CurrentCatagory == BigCataGory.ShoulderEXG || CurrentCatagory == BigCataGory.SideEXG)
            CurrentListOfParts.Add(null);

        CurrentListOfParts.AddRange(Resources.LoadAll<LoadOutPart>(Path));

        Debug.Log(Path +"--"+ CurrentListOfParts.Count);
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

            else if (CurrentCatagory == BigCataGory.FCSChip)
                Temp = "FCS Chip";
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

        AssemblyPartOption NewOption;
        if (a == null)// null option created for empty slots
        {
            NewOption = Instantiate(ListOptionPrefab, ListParent).GetComponent<AssemblyPartOption>();

            CurrentDisplayedOptions.Add(NewOption);

            NewOption.SetUp(this, a);

            return;
        }


        if (a && a.HideForPlayer) //parts that are tagged with this is hidden, mainly used for testing and locking unfinished parts
            return;

        if (UnlockTagTracker.Instance != null && !a.CheckUnlocked())
        {
            return;
        }
        
        NewOption = Instantiate(ListOptionPrefab, ListParent).GetComponent<AssemblyPartOption>();

        CurrentDisplayedOptions.Add(NewOption);

        NewOption.SetUp(this, a);
    }

    

}
