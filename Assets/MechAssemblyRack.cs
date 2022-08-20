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


    private void AssembleVisual()
    {
        MPTorso.VisualAssemble(transform);
        MPTorso.VisualAssembleMech(MPHead, MPRArm, MPLArm, MPLegs, MPPack);
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
    }

    public void FitNewPart()
    {
        FitNewPart(NewPart);
        NewPart = null;
    }

    public void FitNewPart(BaseMechPart A)
    {
        if (A)
        {
            A = Instantiate(A.gameObject, transform).GetComponent<BaseMechPart>();

            if (A is BaseMechPartTorso)
            {
                Destroy(MPTorso.gameObject);
                MPTorso = A as BaseMechPartTorso;
            }
            else if (A is BaseMechPartHead)
            {
                Destroy(MPHead.gameObject);
                MPHead = A as BaseMechPartHead;
            }
            else if (A is BaseMechPartLArm)
            {
                Destroy(MPLArm.gameObject);
                MPLArm = A as BaseMechPartLArm;
            }
            else if (A is BaseMechPartRArm)
            {
                Destroy(MPRArm.gameObject);
                MPRArm = A as BaseMechPartRArm;
            }
            else if (A is BaseMechPartLegs)
            {
                Destroy(MPLegs.gameObject);
                MPLegs = A as BaseMechPartLegs;
            }
            else if (A is BaseMechPartPack)
            {
                Destroy(MPPack.gameObject);
                MPPack = A as BaseMechPartPack;
            }

            AssembleVisual();
        }
    }







}
