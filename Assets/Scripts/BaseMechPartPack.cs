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

    public bool HadLeftEXGSlot()
    {
        if (LeftShoulderEXGSlot)
            return true;
        else
            return false;
    }

    public bool HadRightEXGSlot()
    {
        if (RightShoulderEXGSlot)
            return true;
        else
            return false;
    }

    public bool RightBuiltInEXG
    {
        get { return (RightShoulderEXGSlot == null && RightShoulderEXG); }
    }

    public bool LeftBuiltInEXG
    {
        get { return (LeftShoulderEXGSlot == null && LeftShoulderEXG); }
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

    public virtual string GetBIEXG
    {
        get
        {
            if (BackPackBuiltInEXG)
            {
                return BackPackBuiltInEXG.GetName();
            }
            else
                return "None";
        }
    }

    public override string GetEXGSlots
    {
        get
        {
            string Temp = "";

            if (LeftShoulderEXGSlot)
                Temp += "Y";
            else
                Temp+= "N";

            Temp += " | ";

            if (RightShoulderEXGSlot)
                Temp += "Y";
            else
                Temp += "N";

            return Temp;
        }
    }
}
