using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseTurretMK2))]
public class DroneSubTurret : DroneSubWeapon
{

    [SerializeField]
    BaseTurretMK2 MyTurret;
    [SerializeField]
    BaseShoot MyWeapon;
    bool Firing = false;
    [SerializeField]
    float MaxAllowedAngleDeviation;
    [SerializeField]
    Vector2 FireRange = new Vector2(10, 100);
    

    [SerializeField]
    float BurstDuration;
    [SerializeField]
    float BurstIntermission;
    [SerializeField]
    float BurstStatusCD;


    private void Update()
    {

        if (Firing)
        {
            BurstStatusCD -= Time.deltaTime;
            if (BurstStatusCD <= 0)
            {
                Firing = false;
                MyWeapon.Trigger(false);
                BurstStatusCD = BurstIntermission;
            }
        }
        else
        {

            if (BurstStatusCD <= 0 && MyTarget)
            {
                DecideTrigger();
            }
            else
                BurstStatusCD -= Time.deltaTime;
        }
    }

    public override void RecieveTarget(EnergySignal Target)
    {
        base.RecieveTarget(Target);
        if (Target)
            MyTurret.RecieveTarget(Target.transform);
        else
            MyTurret.TurnToRest();
    }

    private void DecideTrigger()
    {
        if (MyTurret.GetTargetAngleDeviation <= MaxAllowedAngleDeviation && RangeTest(MyTarget.transform.position,FireRange))
        {
            Firing = true;
            MyWeapon.Trigger(true);
            BurstStatusCD = BurstDuration;
        }

    }

    public override bool CurrentlyTargeting()
    {
        if (MyTarget&& CanTargetPosition(MyTarget.transform.position))
            return true;
        return false;
    }

    public override bool CanTargetPosition(Vector3 Pos)
    {
        if (RangeTest(Pos, FireRange))
            return MyTurret.WithinAimAngle(Pos);
        else
            return false;
    }

    private bool RangeTest(Vector3 Pos, Vector2 Range)
    {
        float a = Vector3.Distance(transform.position, Pos);
        if (a < Range.y && a > Range.x)
            return true;
        return false;

    }

}
