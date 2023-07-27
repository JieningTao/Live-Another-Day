using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSubTurret : IDamageable, IDamageSource
{
    [SerializeField]
    BaseTurretMK2 MyTurret;
    [SerializeField]
    BaseShoot MyWeapon;
    bool Firing = false;
    [SerializeField]
    float MaxAllowedAngleDeviation;

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
            BurstStatusCD -= Time.deltaTime;
            if (BurstStatusCD <= 0)
            {
                DecideTrigger();
            }
        }
    }

    private void DecideTrigger()
    {
        if (MyTurret.GetTargetAngleDeviation <= MaxAllowedAngleDeviation)
        {

            Firing = true;
            MyWeapon.Trigger(true);
            BurstStatusCD = BurstDuration;

        }

    }

    public IDamageSource DamageSource()
    {
        return this;
    }
}
