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
    Transform LeftShoulderEXGSlot;
    [SerializeField]
    Transform RightShoulderEXGSlot;

    [Space(20)]

    [SerializeField]
    BaseEXGear LeftShoulderEXG;
    [SerializeField]
    BaseEXGear RightShoulderEXG;
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

    public Transform GetLeftShoulderEXGSlot()
    {
        return LeftShoulderEXGSlot;
    }

    public Transform GetRightShoulderEXGSlot()
    {
        return RightShoulderEXGSlot;
    }

    public void EquipShoulderEXGs(BaseEXGear Left, BaseEXGear Right)
    {
        LeftShoulderEXG = Left;
        if (Left != null)
            Left.InitializeGear(MyMech, LeftShoulderEXGSlot, false);

        RightShoulderEXG = Right;
        if (Right != null)
            Right.InitializeGear(MyMech, RightShoulderEXGSlot, true);
    }

    public void EquipShoulderEXG(BaseEXGear EXG, bool Right)
    {
        if (Right)
        {
            RightShoulderEXG = EXG;
            if (EXG != null)
                EXG.InitializeGear(MyMech, RightShoulderEXGSlot, Right);
        }
        else
        {
            LeftShoulderEXG = EXG;
            if (EXG != null)
                EXG.InitializeGear(MyMech, LeftShoulderEXGSlot, Right);
        }
    }

    public BaseEXGear GetBuilInEXG()
    {
        return TorsoBuiltInEXG;
    }

}
