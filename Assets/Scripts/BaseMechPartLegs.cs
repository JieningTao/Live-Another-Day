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

    public Transform GetGroundDetection()
    {
        return GroundDetection;
    }

    public Vector3 GetDrag()
    {
        return new Vector3(MovingDrag, StoppingDrag, OverSpeedDrag);
    }

    public void EquipLegEXGs(BaseEXGear Left, BaseEXGear Right)
    {
        LeftEXG = Left;
        if (Left != null)
            Left.InitializeGear(MyMech, LeftEXGSlot, false);

        RightEXG = Right;
        if (Right != null)
            Right.InitializeGear(MyMech, RightEXGSlot, true);
    }

    private void Update()
    {
        Vector3 MovementSpeed = MyMovement.MovementInput;
        //Debug.Log(MovementSpeed);
        LegsAnimator.SetFloat("LSpeed", MovementSpeed.z);
        LegsAnimator.SetFloat("HSpeed", MovementSpeed.x);

        Ground(MyMovement.grounded());
    }

    public void EquipLegEXG(BaseEXGear EXG, bool Right)
    {
        if (Right)
        {
            RightEXG = EXG;
            if (EXG != null)
                EXG.InitializeGear(MyMech, RightEXGSlot, Right);
        }
        else
        {
            LeftEXG = EXG;
            if (EXG != null)
                EXG.InitializeGear(MyMech, LeftEXGSlot, Right);
        }
    }

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
}
