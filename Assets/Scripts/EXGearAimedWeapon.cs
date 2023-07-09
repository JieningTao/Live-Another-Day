using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearAimedWeapon : EXGearWeapon
{

    [SerializeField]
    List<Transform> AimedParts;
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
            foreach(Transform a in AimedParts)
            AimWeapon(a);
        }
    }

    //protected virtual void AimWeapon(Transform a, Vector3 Dir, Vector3 Limits, float TurnSpeed)
    //{
    //    if (Vector3.Angle(a.forward, Dir) == 0)
    //        return;

    //    Vector3 TempDir = Vector3.RotateTowards(a.forward, Dir, TurnSpeed * Time.deltaTime, 0.0f);

    //    if (TempDir != null && TempDir != Vector3.zero)
    //    {
    //        //Debug.Log(TempDir);

    //        if (float.IsNaN(TempDir.x) || float.IsInfinity(TempDir.x))
    //        {

    //        }
    //        else
    //            a.rotation = Quaternion.LookRotation(TempDir, a.up);
    //    }

    //    //Debug.Log("Ping");

    //    Vector3 bruh = a.localRotation.eulerAngles; //variable named bruh to commemerate me taking half an hour to realize it wasn't working because turn speed was never changed from initial 0... Fucking idiot.

    //    bruh.z = 0;

    //    if (Limits.y == 0)
    //        bruh.y = 0;
    //    else
    //    {
    //        if (bruh.y < 180)
    //            bruh.y = Mathf.Clamp(bruh.y, -Limits.y, Limits.y);
    //        else
    //            bruh.y = Mathf.Clamp(bruh.y, 360 - Limits.y, 360 + Limits.y);
    //    }

    //    if (Limits.x == 0)
    //        bruh.x = 0;
    //    else
    //    {
    //        if (bruh.x < 180)
    //            bruh.x = Mathf.Clamp(bruh.x, -Limits.x, Limits.x);
    //        else
    //            bruh.x = Mathf.Clamp(bruh.x, 360 - Limits.x, 360 + Limits.x);
    //    }

    //    a.localRotation = Quaternion.Euler(bruh);


    //}

    //public virtual void AimAtTarget(Vector3 Target)
    //{
    //    AimWeapon(AimedPart, Target - AimedPart.position, new Vector3(TargetingAngle,TargetingAngle,0), TargetSpeed);
    //}

    //public virtual void AimEmpty()
    //{
    //    AimWeapon(AimedPart, AimedPart.forward, new Vector3(TargetingAngle, TargetingAngle, 0), TargetSpeed);
    //}

    protected void AimWeapon(Transform Weapon)
    {
        Vector3 AimDir;

        if (MyFCS.GetMainTarget() != null && TargetingAngle>0 && Vector3.Angle(MyFCS.transform.forward, MyFCS.GetMainTarget().transform.position - MyFCS.transform.position) < TargetingAngle)
        {
            //aim at target
            if (Target != MyFCS.GetMainTarget())
            {
                MyFCS.EXGAimed(MyFCS.GetMainTarget());
                Target = MyFCS.GetMainTarget();
            }

            Vector3 PridictedPosition = Target.transform.position + (Target.GetSpeed() * (Vector3.Distance(Weapon.transform.position, Target.transform.position) / MyWeapon.GetProjectileSpeed()));

            AimDir = Vector3.RotateTowards(Weapon.forward, PridictedPosition - Weapon.transform.position, TargetSpeed * Time.deltaTime, 0.0f);

            if (!Aimed)
            {
                MyFCS.EXGAimed(MyFCS.GetMainTarget());
                Aimed = true;
            }
        }
        else
        {
            //reset aim to forward
            AimDir = Vector3.RotateTowards(Weapon.forward, MyFCS.GetLookDirection(), TargetSpeed * Time.deltaTime, 0.0f);

            if (Target)
                Target = null;

            if (Aimed)
            {
                MyFCS.EXGAimed(null);
                Aimed = false;
            }
        }

        Weapon.rotation = Quaternion.LookRotation(AimDir, transform.up);

    }

    protected void AimAt(Vector3 Position)
    {


    }


    public override void Equip(bool a)
    {
        base.Equip(a);

        if (a)
            MyFCS.EXGReticle(true);
        else

        {
            foreach(Transform b in AimedParts)
            b.localRotation = Quaternion.Euler(0, 0, 0);
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
