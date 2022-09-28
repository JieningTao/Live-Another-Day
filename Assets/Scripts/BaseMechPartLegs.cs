using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechPartLegs : BaseMechPart
{


    [SerializeField]
    Transform LeftEXGSlot;
    [SerializeField]
    Transform RightEXGSlot;
    [SerializeField]
    Transform GroundDetection;

    [SerializeField]
    float MovingDrag;
    [SerializeField]
    float StoppingDrag;
    [SerializeField]
    float OverSpeedDrag;

    [SerializeField]
    BaseEXGear LeftEXG;
    [SerializeField]
    BaseEXGear RightEXG;

    public float MovementForce;
    public float SpeedCap;
    public float JumpForce;

    [Space(20)]
    [SerializeField]
    Animator LegsAnimator;
    [SerializeField]
    List<ParticleSystem> DustTrails;
    bool grounded = false;

    BaseMechMovement MyMovement;


    public Transform GetLeftEXGSlot()
    {
        return LeftEXGSlot;
    }

    public Transform GetRightEXGSlot()
    {
        return RightEXGSlot;
    }

    public bool HadLeftEXGSlot()
    {
        if (LeftEXGSlot)
            return true;
        else
            return false;
    }

    public bool HadRightEXGSlot()
    {
        if (RightEXGSlot)
            return true;
        else
            return false;
    }

    public Transform GetGroundDetection()
    {
        return GroundDetection;
    }

    public Vector3 GetDrag()
    {
        return new Vector3(MovingDrag, StoppingDrag, OverSpeedDrag);
    }



    private void Update()
    {
        if (MyMovement)
        {
            Vector3 MovementSpeed = MyMovement.MovementInput;
            //Debug.Log(MovementSpeed);
            LegsAnimator.SetFloat("LSpeed", MovementSpeed.z);
            LegsAnimator.SetFloat("HSpeed", MovementSpeed.x);

            Ground(MyMovement.grounded());
        }

    }

    #region EXG Related
    public void EquipLegEXGs(BaseEXGear Left, BaseEXGear Right)
    {

        EquipLegEXG(Left, false);
        EquipLegEXG(Right, true);

    }

    public void EquipLegEXG(BaseEXGear EXG, bool Right)
    {
        if (Right)
        {
            if (RightEXG == null && RightEXGSlot)
            {
                RightEXG = EXG;
                if (EXG != null)
                    EXG.InitializeGear(MyMech, RightEXGSlot, Right);
            }

        }
        else
        {
            if (LeftEXG == null && LeftEXGSlot)
            {
                LeftEXG = EXG;
                if (EXG != null)
                    EXG.InitializeGear(MyMech, LeftEXGSlot, Right);
            }
        }
    }

    public BaseEXGear AttemptEquipEXGAndGet(BaseEXGear EXG, bool Right)
    {
        EquipLegEXG(EXG, Right);

        return GetEXG(Right);
    }

    public BaseEXGear GetEXG(bool Right)
    {
        if (Right)
            return RightEXG;
        else
            return LeftEXG;
    }

    public void PlaceEXGVisual(BaseEXGear Left, BaseEXGear Right)
    {
        if (LeftEXGSlot && Left)
        {
            LeftEXG = Left;
            Left.PositionGear(LeftEXGSlot, false);
        }
        if (RightEXGSlot && Right)
        {
            RightEXG = Right;
            Right.PositionGear(RightEXGSlot, true);
        }
    }
    #endregion

    private void Ground(bool Landed)
    {
        if (!grounded && Landed)
        {
            grounded = Landed;
            foreach (ParticleSystem a in DustTrails)
                a.Play();
        }
        else if(!Landed&&grounded)
        {
            grounded = Landed;
            foreach (ParticleSystem a in DustTrails)
                a.Stop();
        }

    }

    public override void Assemble(BaseMechMain Mech, Transform JointPosition)
    {
        base.Assemble(Mech, JointPosition);
        MyMovement = Mech.GetMovement();
    }


    public override float GetWeight(bool IncludeGear)
    {
        if (!IncludeGear)
            return base.GetWeight(IncludeGear);
        else
        {
            float TW = Weight;

            if (LeftEXGSlot&&LeftEXG)
                TW += LeftEXG.GetWeight();

            if (RightEXGSlot&&RightEXG)
                TW += RightEXG.GetWeight();

            return TW;
        }

    }
}
