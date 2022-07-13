using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearShoulderLockOnMissile : EXGearShoulder
{
    protected BaseMechFCS MyFCS;
    [SerializeField]
    int LockCount = 5;
    [SerializeField]
    BaseMissileLauncher MyLauncher;
    bool Locking = false;


    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyFCS = Mech.GetFCS();
        MyLauncher.EquipWeapon();
    }



    // Update is called once per frame
    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        if (Down&&!Locking)
        {
            MyFCS.RequestLocks(LockCount);
            Locking = true;
        }
        else if (Down && Locking)
        {
            MyLauncher.FireVolley(MyFCS.GetLockedList());
            Locking = false;
        }
            //GetComponent<BaseMissileLauncher>().Fire1(MyFCS.MainTarget);
    }

    public override void Equip(bool a)
    {
        base.Equip(a);
        if (!a)
            MyFCS.RequestLocks(0);
    }

    public override float GetReadyPercentage()
    {
        return MyLauncher.GetAmmoGauge();
    }
}
