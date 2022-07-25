using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGearWeapon : EnemyGear
{
    [SerializeField]
    BaseShoot MyWeapon;

    public override void AssignController(BaseEnemy a)
    {

        base.AssignController(a);

        MyWeapon.EquipWeapon();
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);
        MyWeapon.Trigger(Down);
    }

    public override bool GearReady()
    {
        return MyWeapon.GetFirable();
    }

    public float GetOffTargetDegree()
    {
        return Vector3.Angle(transform.forward, Controller.GetMainTarget().transform.position - transform.position);
    }

    public override float GetBulletSpeed()
    {
        return MyWeapon.GetProjectileSpeed();
    }
}
