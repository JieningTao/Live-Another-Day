using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearShoulderSingleMissile : EXGearShoulder
{
    protected BaseMechFCS MyFCS;
    [SerializeField]
    BaseMissileLauncher MyLauncher;

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyFCS = Mech.GetFCS();
        MyLauncher.EquipWeapon();
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        if (Down)
            MyLauncher.Fire1(MyFCS.GetMainTarget());

    }

    public override float GetReadyPercentage()
    {
        return MyLauncher.GetAmmoGauge();
    }
}
