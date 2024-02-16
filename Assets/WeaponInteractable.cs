using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteractable : BaseMechInteractable
{
    [SerializeField]
    LOPMainGear MG;
    [SerializeField]
    BaseMainSlotEquipment MSE;

    public override void InteractMain(BaseMechMain Mech, bool a)
    {
        if(a)
        Mech.GetFCS().RecieveNewPrimaryEquipment(MSE);
    }

    public override void InteractSub(BaseMechMain Mech, bool a)
    {
        if(a)
        Mech.GetFCS().RecieveNewSecondaryEquipment(MSE);
    }

    public void RecieveScripts(LOPMainGear _MG,BaseMainSlotEquipment _MSE)
    {   
        MG = _MG; 
        MSE = _MSE;
    }





    public override string InteractableName
    { get { return MG.Name; } }
    public override string MainInteractName
    { get { return "Equip Primary"; } }
    public override string SubInteractName
    { get { return "Equip Secondary"; } }




}
