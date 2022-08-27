using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechPartPack : BaseMechPart
{
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
    BaseEXGear BackPackBuiltInEXG;



    public BaseEXGear GetBuilInEXG()
    {
        return BackPackBuiltInEXG;
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
        EquipShoulderEXG(Left, false);
        EquipShoulderEXG(Right, true);
    }

    public void EquipShoulderEXG(BaseEXGear EXG, bool Right)
    {
        if (Right)
        {
            if (RightShoulderEXG == null && RightShoulderEXGSlot)
            {
                RightShoulderEXG = EXG;
                if (EXG != null)
                    EXG.InitializeGear(MyMech, RightShoulderEXGSlot, Right);
            }
        }
        else
        {
            if (LeftShoulderEXG == null && LeftShoulderEXGSlot)
            {
                LeftShoulderEXG = EXG;
                if (EXG != null)
                    EXG.InitializeGear(MyMech, LeftShoulderEXGSlot, Right);
            }
        }
    }

    public BaseEXGear GetEXG(bool Right)
    {
        if (Right)
            return RightShoulderEXG; 
        else
            return LeftShoulderEXG;
    }

    public BaseEXGear AttemptEquipEXGAndGet(BaseEXGear EXG, bool Right)
    {
        EquipShoulderEXG(EXG, Right);

        return GetEXG(Right);
    }

    public void PlaceEXGVisual(BaseEXGear Left, BaseEXGear Right)
    {
        if (LeftShoulderEXGSlot && Left)
        {
            LeftShoulderEXG = Left;
            Left.PositionGear(LeftShoulderEXGSlot, false);
        }

        if (RightShoulderEXGSlot && Right)
        {
            RightShoulderEXG = Right;
            Right.PositionGear(RightShoulderEXGSlot, true);
        }
    }

    public override float GetWeight(bool IncludeGear)
    {
        if (!IncludeGear)
            return base.GetWeight(IncludeGear);
        else
        {
            float TW = Weight;

            if (LeftShoulderEXGSlot && LeftShoulderEXG)
                TW += LeftShoulderEXG.GetWeight();
            
            if (RightShoulderEXGSlot && RightShoulderEXG)
                TW += RightShoulderEXG.GetWeight();

            return TW;
        }

    }
}
