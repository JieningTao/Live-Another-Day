using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearShoulderWeapon : EXGearShoulder
{
    BaseMechFCS MyFCS;
    [SerializeField]
    Transform AimedPart;
    [SerializeField]
    BaseShoot MyWeapon;
    [SerializeField]
    float TargetSpeed = 1;
    [SerializeField]
    Animator MyAnimator;

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyFCS = Mech.GetFCS();
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

        if (MyFCS.GetMainTarget() != null && Vector3.Angle(MyFCS.transform.forward, MyFCS.GetMainTarget().transform.position - MyFCS.transform.position) < 10)
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

        if (MyAnimator)
            MyAnimator.SetBool("Deployed", a);

        if (!a)
            AimedPart.localRotation = Quaternion.Euler(0,0,0);

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



}
