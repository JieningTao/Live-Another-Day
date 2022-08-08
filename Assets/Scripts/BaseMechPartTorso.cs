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



    public void AssembleMech(BaseMechMain Mech, BaseMechPart H, BaseMechPart RA, BaseMechPart LA, BaseMechPart L, BaseMechPart BP)
    {
        H.Assemble(Mech, HeadSocket);
        RA.Assemble(Mech, RightArmSocket);
        LA.Assemble(Mech, LeftArmSocket);
        L.Assemble(Mech, LegsSocket);
        BP.Assemble(Mech, BackPackSocket);
    }

    public BaseEXGear GetBuilInEXG()
    {
        return TorsoBuiltInEXG;
    }

}
