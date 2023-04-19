using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechPartArm : BaseMechPart
{
    [Space(30)]

    [SerializeField]
    Transform SideMountedEXGSlot;
    [SerializeField]
    Transform EXGAimed;

    [SerializeField]
    Transform AimedPart;
    [SerializeField]
    Transform HandSlot;
    [SerializeField]
    Animator ArmAnimator;
    [SerializeField]
    MainEquipmentFilter EquipmentFilter;

    [Space(10)]

    [SerializeField]
    protected BaseEXGear ArmEXG;
    [SerializeField]
    protected BaseMainSlotEquipment EquippedGear;
    [Tooltip("innate weapon that gets equipped when equipped weapon is dropped, leave blank for none")]
    [SerializeField]
    protected BaseMainSlotEquipment FallbackWeapon; 

    public float AimSpeed = 1;

    [System.Serializable]
    public class MainEquipmentFilter
    {
        public BaseMainSlotEquipment.MainEquipmentSize SizeLimit = BaseMainSlotEquipment.MainEquipmentSize.ExtraLarge;

        public bool Default = true;
        public bool Shoulder = true;
        public bool Shield = true;
        public bool Under = true;
        public bool Arm = true;
    }

    public override void Assemble(BaseMechMain Mech, Transform SocketPosition)
    {
        //base.Assemble(Mech, SocketPosition);
        //transform.parent = SocketPosition;
        //transform.localPosition = Displacement;
        //transform.localRotation = Quaternion.Euler(Vector3.zero);
        base.VisualAssemble(SocketPosition);

        MyMech = Mech;
        SetLayer(Mech.gameObject.layer);

        EquipEquipment(EquippedGear);
        EquipEXG(ArmEXG);
    }

    public override void VisualAssemble(Transform SocketPosition)
    {
        Transform Temp = transform.parent;
        base.VisualAssemble(SocketPosition);
        transform.parent = Temp;

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

    public bool HasEquipmentSlot()
    {
        if (HandSlot)
            return true;
        else
            return false;
    }

    public bool HasEXGSlot()
    {
        if (SideMountedEXGSlot)
            return true;
        else
            return false;

    }

    public void SetArmAnimator(int State)
    {
        //Debug.Log(State);
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
            SetArmAnimator(0);

            return;
        }

        Vector3 AimDir;

        if (EquippedGear.RequireAiming)
            AimDir = Vector3.RotateTowards(AimedPart.forward, Pos - HandSlot.transform.position, AimSpeed * Time.deltaTime, 0.0f);
        else
            AimDir = Vector3.RotateTowards(AimedPart.forward, transform.forward, AimSpeed * Time.deltaTime, 0.0f);

        AimedPart.rotation = Quaternion.LookRotation(AimDir, this.transform.up);
    }

    public void TargetArm(EnergySignal Tar)
    {
        if (Time.timeScale == 0) //this check stops the "look vector is zero" when game is paused, not a fix"
            return;

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
            if (EquippedGear.GetBulletSpeed() <= 0 || EquippedGear.GetBulletSpeed() == Mathf.Infinity)
                MoveDelta = Vector3.zero;
            else
            {
                MoveDelta = Tar.GetSpeed() * (Vector3.Distance(transform.position, Tar.transform.position) / EquippedGear.GetBulletSpeed());
            }


            AimDir = Vector3.RotateTowards(AimedPart.forward, (Tar.transform.position + MoveDelta) - HandSlot.transform.position, AimSpeed * Time.deltaTime, 0.0f);
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
        if (FallbackWeapon == null)
        {
            EquippedGear = null;
            SetArmAnimator(0);
        }
        else
        {
            EquipEquipment(FallbackWeapon);
        }


        return a;
    }

    public void EquipEquipment(BaseMainSlotEquipment a)
    {
        //Debug.Log(a);
        EquippedGear = a;
        if (a)
        {
            a.Equip(true, MyMech, IsRightArm());
            a.transform.parent = HandSlot;
            a.transform.localPosition = Vector3.zero;
            a.transform.localRotation = Quaternion.Euler(Vector3.zero);
            //Debug.Log((int)EquippedGear.HoldStyle);
            SetArmAnimator((int)EquippedGear.HoldStyle);
        }
        else
        {
            UnequipEquipment();
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
                if(a.IsAimed)
                    a.InitializeGear(MyMech, EXGAimed, IsRightArm());
                else
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

    public virtual void PlaceEXGVisual(BaseEXGear EXG)
    {
        if (SideMountedEXGSlot && EXG)
        {
            ArmEXG = EXG;
            EXG.PositionGear(SideMountedEXGSlot, IsRightArm());
        }

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
    public override float GetWeight(bool IncludeGear)
    {
        if (!IncludeGear)
            return base.GetWeight(IncludeGear);
        else
        {
            float TW = Weight;

            if (SideMountedEXGSlot && ArmEXG)
                TW += ArmEXG.GetWeight();

            if (HandSlot && EquippedGear)
                TW += EquippedGear.GetWeight();

            return TW;
        }
    }

    public virtual bool ArmEmpty
    { get { return EquippedGear == null || EquippedGear == FallbackWeapon; } }

    public override string GetBIEXG
    {
        get {
            if (ArmEXG && SideMountedEXGSlot == null)
                return ArmEXG.GetName();
            else
                return null;
        }
    }

    public override string GetEXGSlots
    {
        get {
            if (SideMountedEXGSlot)
                return "Y";
            else
                return "N";
        }
    }
}
