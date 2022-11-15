using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearAimedWeapon : BaseEXGear
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

    bool Aimed = false;
    EnergySignal Target = null;

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);

        MyFCS = Mech.GetFCS();

        if (MyWeapon is BaseEnergyShoot)
        {
            (MyWeapon as BaseEnergyShoot).GetPowerSource(Mech);
            Debug.Log("Is E");
        }
        //Debug.Log(gameObject.name + " Right: " + Right, this);
    }

    protected override void Update()
    {
        base.Update();

        if (Equipped && ReadyTimer<=0)
        {
            AimWeapon();
        }

        
    }

    protected void AimWeapon()
    {
        Vector3 AimDir;

        if (MyFCS.GetMainTarget() != null && Vector3.Angle(MyFCS.transform.forward, MyFCS.GetMainTarget().transform.position - MyFCS.transform.position) < TargetingAngle)
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

    public override void TriggerGear(bool Down)
    {

        base.TriggerGear(Down); //base trigger considers ready time and returns if not ready

        if (MyAnimator && Down && MyWeapon.GetFirable())
            MyAnimator.SetTrigger("Fire");

        MyWeapon.Trigger(Down);
    }


    public override float GetReadyPercentage()
    {
        return MyWeapon.GetAmmoGauge();
    }

    public override string GetBBMainText()
    {
        return MyWeapon.GetAmmoText();
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Damage: ");
        Temp.Add(MyWeapon.GetDamage);

        Temp.Add("Accuracy: ");
        Temp.Add(MyWeapon.GetAccuracy);

        Temp.Add("Fire Rate: ");
        Temp.Add(MyWeapon.GetFireRate);

        Temp.Add("Fire Mode: ");
        Temp.Add(MyWeapon.GetFireMode);

        if (MyWeapon is BaseKineticShoot)
        {
            Temp.Add("Magazine: ");
            Temp.Add(MyWeapon.GetMag);

            Temp.Add("Reload: ");
            Temp.Add(MyWeapon.GetReload);
        }
        else if (MyWeapon is BaseEnergyShoot)
        {
            Temp.Add("Charge: ");
            Temp.Add(MyWeapon.GetMag);

            Temp.Add("Recharge: ");
            Temp.Add(MyWeapon.GetReload);
        }



        return Temp;
    }
}
