using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAssemblyRack : MonoBehaviour
{
    [SerializeField]
    BaseMechPart NewPart;

    [Space(10)]

    [SerializeField]
    BaseMechPartHead MPHead;
    [SerializeField]
    BaseMechPartTorso MPTorso;

    [SerializeField]
    GameObject MPArms;
    BaseMechPartArm MPLArm;
    BaseMechPartArm MPRArm;
    [SerializeField]
    BaseMechPartLegs MPLegs;
    [SerializeField]
    BaseMechPartPack MPPack;

    //private List<BaseMechPart> AllParts;

    [Space(10)]

    [SerializeField]
    BaseBoostSystem BoostSystem;
    [SerializeField]
    BaseEnergySource EnergySystem;

    [Space(10)]

    [SerializeField]
    private BaseMainSlotEquipment CurrentPrimary;

    [SerializeField]
    private BaseMainSlotEquipment CurrentSecondary;

    [Space(10)]

    [SerializeField]
    protected BaseEXGear[] EquipedEXGear = new BaseEXGear[8];


    public void StarterLoad()
    {
        LoadPlayerPrefLoadout();
        SpawnParts();
        AssembleVisual();
    }

    public void LoadPlayerPrefLoadout()
    {
        if (PlayerPrefs.HasKey("PlayerLoadout"))
        {
            Debug.Log("Saved Loadout detected, loading...");

            string a = PlayerPrefs.GetString("PlayerLoadout");
            Debug.Log(a);

            List<List<LoadOutPart>> LoadedLoadout = SaveCoder.LoadLoadout(a);

            List<LoadOutPart> LoadoutBodyPart = LoadedLoadout[0];
            List<LoadOutPart> LoadoutMainEquipment = LoadedLoadout[1];
            List<LoadOutPart> LoadoutEXGs = LoadedLoadout[2];

            //Debug.Log(LoadoutBodyPart.Count + ":" + LoadoutMainEquipment.Count + ":" + LoadoutEXGs.Count);
            //Debug.Log(LoadoutBodyPart);
            //Debug.Log(LoadoutMainEquipment);
            //Debug.Log(LoadoutEXGs);


            MPHead = LoadoutBodyPart[0].GetComponent<BaseMechPartHead>();
            MPTorso = LoadoutBodyPart[1].GetComponent<BaseMechPartTorso>();
            MPArms = LoadoutBodyPart[2].gameObject;
            //MPLArm = LoadoutBodyPart[2].GetComponent<BaseMechPartLArm>();
            //MPRArm = LoadoutBodyPart[2].GetComponent<BaseMechPartRArm>();

            MPLegs = LoadoutBodyPart[3].GetComponent<BaseMechPartLegs>();
            MPPack = LoadoutBodyPart[4].GetComponent<BaseMechPartPack>();
            
            BoostSystem = LoadoutBodyPart[5].GetComponent<BaseBoostSystem>();
            EnergySystem = LoadoutBodyPart[6].GetComponent<BasePowerSystem>();

            if (LoadoutMainEquipment[0])
                CurrentPrimary = LoadoutMainEquipment[0].GetComponent<BaseMainSlotEquipment>();
            else
                CurrentPrimary = null;

            if (LoadoutMainEquipment[1])
                CurrentSecondary = LoadoutMainEquipment[1].GetComponent<BaseMainSlotEquipment>();
            else
                CurrentSecondary = null;

            //loaded null EXG will revert to default loadout, it's ok if the default loadout is empty in that slot

            if (LoadoutEXGs[0])
                EquipedEXGear[0] = LoadoutEXGs[0].GetComponent<BaseEXGear>();
            if (LoadoutEXGs[1])
                EquipedEXGear[1] = LoadoutEXGs[1].GetComponent<BaseEXGear>();
            if (LoadoutEXGs[2])
                EquipedEXGear[2] = LoadoutEXGs[2].GetComponent<BaseEXGear>();

            if (LoadoutEXGs[3])
                EquipedEXGear[5] = LoadoutEXGs[3].GetComponent<BaseEXGear>();
            if (LoadoutEXGs[4])
                EquipedEXGear[6] = LoadoutEXGs[4].GetComponent<BaseEXGear>();
            if (LoadoutEXGs[5])
                EquipedEXGear[7] = LoadoutEXGs[5].GetComponent<BaseEXGear>();

            Debug.Log("Loadout loaded");
        }
    }

    public void SavePlayerPrefLoadout()
    {
        Debug.Log("Saving Loadout...");

        List<List<LoadOutPart>> Temp = new List<List<LoadOutPart>>();
        List<LoadOutPart> LoadoutBodyPart = new List<LoadOutPart>();
        List<LoadOutPart> LoadoutMainEquipment = new List<LoadOutPart>();
        List<LoadOutPart> LoadoutEXGs = new List<LoadOutPart>();

        LoadoutBodyPart.Add(MPHead.GetComponent<LoadOutPart>());
        LoadoutBodyPart.Add(MPTorso.GetComponent<LoadOutPart>());
        LoadoutBodyPart.Add(MPLArm.GetComponentInParent<LoadOutPart>()); // only need to fetch arm LOP from 1 arm
        LoadoutBodyPart.Add(MPLegs.GetComponent<LoadOutPart>());
        LoadoutBodyPart.Add(MPPack.GetComponent<LoadOutPart>());

        LoadoutBodyPart.Add(BoostSystem.GetComponent<LoadOutPart>());
        LoadoutBodyPart.Add(EnergySystem.GetComponent<LoadOutPart>());

        if (CurrentPrimary)
            LoadoutMainEquipment.Add(CurrentPrimary.GetComponent<LoadOutPart>());
        else
            LoadoutMainEquipment.Add(null);

        if(CurrentSecondary)
            LoadoutMainEquipment.Add(CurrentSecondary.GetComponent<LoadOutPart>());
        else
            LoadoutMainEquipment.Add(null);

        for (int i = 0; i < EquipedEXGear.Length; i++)
        {
            if (i != 3 && i != 4)
            {
                if (EquipedEXGear[i])
                    LoadoutEXGs.Add(EquipedEXGear[i].GetComponent<LoadOutPart>());
                else
                    LoadoutEXGs.Add(null);
            }

        }

        Temp.Add(LoadoutBodyPart);
        Temp.Add(LoadoutMainEquipment);
        Temp.Add(LoadoutEXGs);

        string a = SaveCoder.ConvertLoadoutToString(Temp);

        Debug.Log("Player loadout: "+a);

        PlayerPrefs.SetString("PlayerLoadout", a);
        PlayerPrefs.Save();

    }

    private void ReassembleVisual()
    {
        UnassembleVisual();
        AssembleVisual();
    }

    private void AssembleVisual()
    {


        MPTorso.VisualAssemble(transform);
        MPTorso.VisualAssembleMech(MPHead, MPRArm, MPLArm, MPLegs, MPPack);

        EquipWeapons();
        EquipEXGs();
    }

    private void UnassembleVisual()
    {
        foreach (BaseEXGear a in EquipedEXGear)
        {
            if(a)
            a.transform.parent = null;
        }

        if(CurrentPrimary)
        CurrentPrimary.transform.parent = null;
        if(CurrentSecondary)
        CurrentSecondary.transform.parent = null;

        MPHead.transform.parent = null;
        MPTorso.transform.parent = null;

        MPArms.transform.parent = null;

        //MPLArm.transform.parent = null;
        //MPRArm.transform.parent = null;
        MPLegs.transform.parent = null;
        MPPack.transform.parent = null;
    }

    private void SpawnParts()
    {
        MPTorso = Instantiate(MPTorso.gameObject, transform).GetComponent<BaseMechPartTorso>();


        MPHead = Instantiate(MPHead.gameObject, transform).GetComponent<BaseMechPartHead>();
        MPLegs = Instantiate(MPLegs.gameObject, transform).GetComponent<BaseMechPartLegs>();
        MPPack = Instantiate(MPPack.gameObject, transform).GetComponent<BaseMechPartPack>();

        MPArms = Instantiate(MPArms.gameObject, transform);
        MPLArm = MPArms.GetComponentInChildren<BaseMechPartLArm>();
        MPRArm = MPArms.GetComponentInChildren<BaseMechPartRArm>();
        //MPLArm = Instantiate(MPLArm.gameObject, transform).GetComponent<BaseMechPartArm>();
        //MPRArm = Instantiate(MPRArm.gameObject, transform).GetComponent<BaseMechPartArm>();

        BoostSystem = Instantiate(BoostSystem.gameObject, transform).GetComponent<BaseBoostSystem>();
        EnergySystem = Instantiate(EnergySystem.gameObject, transform).GetComponent<BasePowerSystem>();

        SpawnWeapons();
        SpawnEXGs();
    }

    private void SpawnWeapons()
    {
        if (CurrentPrimary)
            CurrentPrimary = Instantiate(CurrentPrimary, transform).GetComponent<BaseMainSlotEquipment>();

        if (CurrentSecondary)
            CurrentSecondary = Instantiate(CurrentSecondary, transform).GetComponent<BaseMainSlotEquipment>();
    }

    private void EquipWeapons()
    {
        MPRArm.EquipEquipment(CurrentPrimary);
        MPLArm.EquipEquipment(CurrentSecondary);
    }

    private void SpawnEXGs()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i != 3 && i != 4)
            {
                if (EquipedEXGear[i])
                    EquipedEXGear[i] = Instantiate(EquipedEXGear[i].gameObject, transform).GetComponent<BaseEXGear>();
                
            }


        }
    }

    private void EquipEXGs()
    {

        //if (MPLegs.GetLeftEXGSlot())
        //    MPLegs.AttemptEquipEXGAndGet(EquipedEXGear[0], false);

        //if(MPLegs.GetRightEXGSlot())
        //    EquipedEXGear[7] = MPLegs.AttemptEquipEXGAndGet(EquipedEXGear[7], true);

        MPLegs.PlaceEXGVisual(EquipedEXGear[0], EquipedEXGear[7]);

        MPPack.PlaceEXGVisual(EquipedEXGear[2], EquipedEXGear[5]);

        MPLArm.PlaceEXGVisual(EquipedEXGear[1]);

        MPRArm.PlaceEXGVisual(EquipedEXGear[6]);

        //if (MPLArm.GetSideMountEXGSlot())
        //    MPLArm.AttemptEquipEXGAndGet(EquipedEXGear[1]);

        //if(MPRArm.GetSideMountEXGSlot())
        //    EquipedEXGear[6] = MPRArm.AttemptEquipEXGAndGet(EquipedEXGear[6]);

        //if(MPPack.GetLeftShoulderEXGSlot())
        //    MPPack.AttemptEquipEXGAndGet(EquipedEXGear[2], false);


        //if(MPPack.GetRightShoulderEXGSlot())
        //    EquipedEXGear[5] = MPPack.AttemptEquipEXGAndGet(EquipedEXGear[5], true);




    }

    public void FitNewPart(PartSwitchManager.BigCataGory PartType,int Position, GameObject PartToFit)
    {
        if (PartToFit)
        {
            PartToFit = Instantiate(PartToFit, null);
        }

        if (PartType == PartSwitchManager.BigCataGory.MainWeapon)
        {
            if (Position == 0)
            {
                if(CurrentPrimary)
                Destroy(CurrentPrimary.gameObject);
                CurrentPrimary = PartToFit.GetComponent<BaseMainSlotEquipment>();
                MPRArm.EquipEquipment(CurrentPrimary);
            }
            else
            {
                if(CurrentSecondary)
                Destroy(CurrentSecondary.gameObject);
                CurrentSecondary = PartToFit.GetComponent<BaseMainSlotEquipment>();
                MPLArm.EquipEquipment(CurrentSecondary);
            }
        }
        else if (PartType == PartSwitchManager.BigCataGory.ShoulderEXG || PartType == PartSwitchManager.BigCataGory.SideEXG)
        {
            if(EquipedEXGear[Position])
                Destroy(EquipedEXGear[Position].gameObject);

            EquipedEXGear[Position] = PartToFit.GetComponent<BaseEXGear>();

        }
        else
        {
            switch (PartType)
            {
                case PartSwitchManager.BigCataGory.Head:
                    Destroy(MPHead.gameObject);
                    MPHead = PartToFit.GetComponent<BaseMechPartHead>();
                    break;

                case PartSwitchManager.BigCataGory.Torso:
                    Destroy(MPTorso.gameObject);
                    MPTorso = PartToFit.GetComponent<BaseMechPartTorso>();
                    break;

                case PartSwitchManager.BigCataGory.Arms:

                    Destroy(MPArms.gameObject);

                    MPArms = PartToFit;

                    MPLArm = PartToFit.GetComponentInChildren<BaseMechPartLArm>();

                    MPRArm = PartToFit.GetComponentInChildren<BaseMechPartRArm>();

                    break;

                case PartSwitchManager.BigCataGory.Pack:
                    Destroy(MPPack.gameObject);
                    MPPack = PartToFit.GetComponent<BaseMechPartPack>();
                    break;

                case PartSwitchManager.BigCataGory.Legs:
                    Destroy(MPLegs.gameObject);
                    MPLegs = PartToFit.GetComponent<BaseMechPartLegs>();
                    break;

            }
        }

       ReassembleVisual();


    }

    //public void FitNewPart() //legacy function used during testing
    //{
    //    FitNewPart(NewPart);
    //    NewPart = null;
    //}

    //public void FitNewPart(BaseMechPart A) //legacy function that detects what the body part being fitted is
    //{
    //    if (A)
    //    {
    //        A = Instantiate(A.gameObject, transform).GetComponent<BaseMechPart>();

    //        if (A is BaseMechPartTorso)
    //        {
    //            Destroy(MPTorso.gameObject);
    //            MPTorso = A as BaseMechPartTorso;
    //        }
    //        else if (A is BaseMechPartHead)
    //        {
    //            Destroy(MPHead.gameObject);
    //            MPHead = A as BaseMechPartHead;
    //        }
    //        else if (A is BaseMechPartLArm)
    //        {
    //            Destroy(MPLArm.gameObject);
    //            MPLArm = A as BaseMechPartLArm;
    //        }
    //        else if (A is BaseMechPartRArm)
    //        {
    //            Destroy(MPRArm.gameObject);
    //            MPRArm = A as BaseMechPartRArm;
    //        }
    //        else if (A is BaseMechPartLegs)
    //        {
    //            Destroy(MPLegs.gameObject);
    //            MPLegs = A as BaseMechPartLegs;
    //        }
    //        else if (A is BaseMechPartPack)
    //        {
    //            Destroy(MPPack.gameObject);
    //            MPPack = A as BaseMechPartPack;
    //        }

    //        ReassembleVisual();
    //    }
    //}







}
