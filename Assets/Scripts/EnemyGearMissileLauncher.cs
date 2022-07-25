using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGearMissileLauncher : EnemyGear
{
    [SerializeField]
    BaseMissileLauncher MyML;
    [SerializeField]
    int VolleyAmount;

    public override void AssignController(BaseEnemy a)
    {
        base.AssignController(a);
        MyML.EquipWeapon();
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        if (Down)
            MyML.FireVolley(Controller.GetTargets(VolleyAmount));

    }

}
