using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechPartArm : BaseMechPart
{
    [SerializeField]
    Transform SideMountedEXGSlot;
    [SerializeField]
    Transform AimedPart;
    [SerializeField]
    Transform HandSlot;
    [SerializeField]
    Animator ArmAnimator;

    [Space(10)]

    [SerializeField]
    protected BaseEXGear ArmEXG;
    [SerializeField]
    protected BaseMainSlotEquipment EquippedGear;

    public float AimSpeed = 1;

    public override void Assemble(BaseMechMain Mech, Transform JointPosition)
    {
        base.Assemble(Mech, JointPosition);

        
        EquipEquipment(EquippedGear);
        EquipEXG(ArmEXG);
    }

    public Transform GetSideMountEXGSlot()
    {
        return SideMountedEXGSlot;
    }

    public Transform GetHandSlot()
    {
        return HandSlot;
    }

    public Transform GetAimedPart()
    {
        return AimedPart;
    }

    public void SetArmAnimator(int State)
    {
        if (ArmAnimator)
            ArmAnimator.SetInteger("ArmState", State);
    }

    public void TargetArm(Vector3 Pos)
    {
        if (EquippedGear == null)
        {
            if (EquippedGear is BaseMainSlotEquipment)
                UnequipEquipment();

            TargetArmEmpty();
            return;
        }

        Vector3 AimDir;

        if(EquippedGear.RequireAiming)
            AimDir = Vector3.RotateTowards(AimedPart.forward, Pos - HandSlot.transform.position, AimSpeed * Time.deltaTime, 0.0f);
        else
            AimDir = Vector3.RotateTowards(AimedPart.forward, transform.forward, AimSpeed * Time.deltaTime, 0.0f);

        AimedPart.rotation = Quaternion.LookRotation(AimDir, this.transform.up);
    }

    public void TargetArm(EnergySignal Tar)
    {
        if (EquippedGear == null)
        {
            if (EquippedGear is BaseMainSlotEquipment)
                UnequipEquipment();

            TargetArmEmpty();
            return;
        }

        Vector3 AimDir;

        if (EquippedGear.RequireAiming)
        {
            Vector3 MoveDelta;
            if (EquippedGear.GetBulletSpeed() <= 0)
                MoveDelta = Vector3.zero;
            else
            {
                MoveDelta =  Tar.GetSpeed() * (Vector3.Distance(transform.position, Tar.transform.position) / EquippedGear.GetBulletSpeed());
            }


            AimDir = Vector3.RotateTowards(AimedPart.forward, (Tar.transform.position+MoveDelta) - HandSlot.transform.position, AimSpeed * Time.deltaTime, 0.0f);
        }
        else
            AimDir = Vector3.RotateTowards(AimedPart.forward, transform.forward, AimSpeed * Time.deltaTime, 0.0f);

        AimedPart.rotation = Quaternion.LookRotation(AimDir, this.transform.up);
    }

    public void TargetArmEmpty()
    {
        Vector3 AimDir;
        AimDir = Vector3.RotateTowards(AimedPart.forward, transform.forward, AimSpeed * Time.deltaTime, 0.0f);
        AimedPart.rotation = Quaternion.LookRotation(AimDir, this.transform.up);
    }

    public BaseMainSlotEquipment UnequipEquipment()
    {
        BaseMainSlotEquipment a = EquippedGear;

        EquippedGear = null;
        SetArmAnimator(0);
        return a;
    }

    public void EquipEquipment(BaseMainSlotEquipment a)
    {
        EquippedGear = a;
        if (a)
        {
            a.transform.parent = HandSlot;
            a.transform.localPosition = Vector3.zero;
            Debug.Log((int)EquippedGear.HoldStyle);
            SetArmAnimator((int) EquippedGear.HoldStyle);
        }

    }

    public BaseMainSlotEquipment SwitchWeapon(BaseMainSlotEquipment a)
    {
        if (!a)
            return null;

        BaseMainSlotEquipment b = EquippedGear;

        EquippedGear = a;
        a.transform.parent = HandSlot;
        a.transform.localPosition = Vector3.zero;
        Debug.Log((int)EquippedGear.HoldStyle);
        SetArmAnimator((int)EquippedGear.HoldStyle);

        return b;
    }

    public virtual void EquipEXG(BaseEXGear a)
    {
        if (ArmEXG == null && SideMountedEXGSlot)
        {
            if (a)
            {
                a.transform.parent = SideMountedEXGSlot;
                a.transform.localPosition = Vector3.zero;
                a.InitializeGear(MyMech, SideMountedEXGSlot, IsRightArm());
            }
            ArmEXG = a;
        }

    }

    public BaseEXGear AttemptEquipEXGAndGet(BaseEXGear EXG)
    {
        EquipEXG(EXG);

        return GetEXG();
    }

    public BaseEXGear GetEXG()
    {
        return ArmEXG;
    }

    public BaseMainSlotEquipment GetMainEquipment()
    {
        return EquippedGear;
    }


    public virtual bool IsRightArm()
    {
        return true;
    }
}
