using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearSideWeapon : EXGearSide
{
    BaseMechFCS MyFCS;
    [SerializeField]
    Transform AimedPart;
    [SerializeField]
    BaseShoot MyWeapon;
    [SerializeField]
    float TargetSpeed = 1;
    [SerializeField]
    float TargetingAngle = 10;

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyFCS = Mech.GetFCS();
    }

    protected override void Update()
    {
        base.Update();

        if (Equipped && ReadyTimer <= 1)
        {
            AimWeapon();
        }


    }

    protected void AimWeapon()
    {
        Vector3 AimDir;

        if (MyFCS.GetMainTarget() != null && Vector3.Angle(MyFCS.transform.forward, MyFCS.GetMainTarget().transform.position - MyFCS.transform.position) < TargetingAngle)
        {
            AimDir = Vector3.RotateTowards(AimedPart.forward, MyFCS.GetMainTarget().transform.position - AimedPart.transform.position, TargetSpeed * Time.deltaTime, 0.0f);
        }
        else
        {
            AimDir = Vector3.RotateTowards(AimedPart.forward, MyFCS.GetLookDirection(), TargetSpeed * Time.deltaTime, 0.0f);
        }

        AimedPart.rotation = Quaternion.LookRotation(AimDir, transform.up);

    }

    public override void Equip(bool a)
    {
        base.Equip(a);

        if (!a)
            AimedPart.localRotation = Quaternion.Euler(0, 0, 0);

        TriggerGear(false);

    }

    public override void TriggerGear(bool Down)
    {

        if (ReadyTimer > 0)
            return;

        if (MyAnimator && Down && MyWeapon.GetFirable())
            MyAnimator.SetTrigger("Fire");

        MyWeapon.Trigger(Down);
    }


    public override float GetReadyPercentage()
    {
        return MyWeapon.GetAmmoGauge();
    }
}
