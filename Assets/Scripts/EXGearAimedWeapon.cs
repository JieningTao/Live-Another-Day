using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearAimedWeapon : EXGearWeapon
{
    [SerializeField]
    Transform AimedPart;
    [SerializeField]
    float TargetSpeed = 1;
    [SerializeField]
    float TargetingAngle = 10;




    bool Aimed = false;
    EnergySignal Target = null;



    protected override void Update()
    {
        base.Update();

        if (Equipped && ReadyTimer<=0&& MyWeapon.GetFirable())//while the weapon is reloading, aiming can fuck with animations
        {
            AimWeapon();
        }
    }

    protected void AimWeapon()
    {
        Vector3 AimDir;

        if (MyFCS.GetMainTarget() != null && TargetingAngle>0 && Vector3.Angle(MyFCS.transform.forward, MyFCS.GetMainTarget().transform.position - MyFCS.transform.position) < TargetingAngle)
        {
            if (Target != MyFCS.GetMainTarget())
            {
                MyFCS.EXGAimed(MyFCS.GetMainTarget());
                Target = MyFCS.GetMainTarget();
            }

            AimDir = Vector3.RotateTowards(AimedPart.forward, MyFCS.GetMainTarget().transform.position - AimedPart.transform.position, TargetSpeed * Time.deltaTime, 0.0f);

            if (!Aimed)
            {
                MyFCS.EXGAimed(MyFCS.GetMainTarget());
                Aimed = true;
            }
        }
        else
        {
            AimDir = Vector3.RotateTowards(AimedPart.forward, MyFCS.GetLookDirection(), TargetSpeed * Time.deltaTime, 0.0f);

            if (Target)
                Target = null;

            if (Aimed)
            {
                MyFCS.EXGAimed(null);
                Aimed = false;
            }
        }

        AimedPart.rotation = Quaternion.LookRotation(AimDir, transform.up);

    }

    public override void Equip(bool a)
    {
        base.Equip(a);

        if (a)
            MyFCS.EXGReticle(true);
        else

        {
            AimedPart.localRotation = Quaternion.Euler(0, 0, 0);
            MyFCS.EXGReticle(false);
            if (Aimed)
            {
                MyFCS.EXGAimed(null);
                Aimed = false;
            }
        }

        TriggerGear(false);

    }

    public override bool IsAimed
    {
        get { return true; }
    }
}
