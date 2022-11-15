using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechPartTorso : BaseMechPart
{
    [SerializeField]
    Transform HeadSocket;
    [SerializeField]
    Transform RightArmSocket;
    [SerializeField]
    Transform LeftArmSocket;
    [SerializeField]
    Transform LegsSocket;
    [SerializeField]
    Transform BackPackSocket;
    [Space(20)]





    [SerializeField]
    BaseEXGear TorsoBuiltInEXG;
    [SerializeField]
    float AmmoAmount = 200;


    BasePowerSystem PowerSystem
    {
        get {
            if (!_PowerSystem)
                _PowerSystem = GetComponent<BasePowerSystem>();
            return _PowerSystem;
        }
    }
    BasePowerSystem _PowerSystem;




    public void AssembleMech(BaseMechMain Mech, BaseMechPart H, BaseMechPart RA, BaseMechPart LA, BaseMechPart L, BaseMechPart BP)
    {
        H.Assemble(Mech, HeadSocket);
        RA.Assemble(Mech, RightArmSocket);
        LA.Assemble(Mech, LeftArmSocket);

        //Debug.Log(RA.transform.parent.name);

        L.Assemble(Mech, LegsSocket);
        BP.Assemble(Mech, BackPackSocket);
    }

    public void VisualAssembleMech(BaseMechPart H, BaseMechPart RA, BaseMechPart LA, BaseMechPart L, BaseMechPart BP)
    {
        H.VisualAssemble(HeadSocket);
        RA.VisualAssemble(RightArmSocket);
        LA.VisualAssemble(LeftArmSocket);
        L.VisualAssemble(LegsSocket);
        BP.VisualAssemble(BackPackSocket);
    }

    public BaseEXGear GetBuilInEXG()
    {
        return TorsoBuiltInEXG;
    }

    public override string GetBIEXG
    {
        get {
            if (TorsoBuiltInEXG)
            {
                return TorsoBuiltInEXG.GetName();
            }
            else
                return "None";
        }
    }

    public BasePowerSystem GetPowerSystem()
    {
        return PowerSystem;
    }

}
