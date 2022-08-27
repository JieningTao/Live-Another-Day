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
    BaseMechPartArm MPLArm;
    [SerializeField]
    BaseMechPartArm MPRArm;
    [SerializeField]
    BaseMechPartLegs MPLegs;
    [SerializeField]
    BaseMechPartPack MPPack;

    private List<BaseMechPart> AllParts;

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



    private void Start()
    {
        SpawnParts();
        AssembleVisual();



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
        MPLArm.transform.parent = null;
        MPRArm.transform.parent = null;
        MPLegs.transform.parent = null;
        MPPack.transform.parent = null;
    }

    private void SpawnParts()
    {
        MPTorso = Instantiate(MPTorso.gameObject, transform).GetComponent<BaseMechPartTorso>();


        MPHead = Instantiate(MPHead.gameObject, transform).GetComponent<BaseMechPartHead>();
        MPLegs = Instantiate(MPLegs.gameObject, transform).GetComponent<BaseMechPartLegs>();
        MPPack = Instantiate(MPPack.gameObject, transform).GetComponent<BaseMechPartPack>();
        MPLArm = Instantiate(MPLArm.gameObject, transform).GetComponent<BaseMechPartArm>();
        MPRArm = Instantiate(MPRArm.gameObject, transform).GetComponent<BaseMechPartArm>();

        BoostSystem = Instantiate(BoostSystem.gameObject, transform).GetComponent<BaseBoostSystem>();
        EnergySystem = Instantiate(EnergySystem.gameObject, transform).GetComponent<BasePowerSystem>();

        SpawnWeapons();
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
                Destroy(CurrentPrimary.gameObject);
                CurrentPrimary = PartToFit.GetComponent<BaseMainSlotEquipment>();
                MPRArm.EquipEquipment(CurrentPrimary);
            }
            else
            {
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

                    Destroy(MPLArm.gameObject);
                    Destroy(MPRArm.gameObject);

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
