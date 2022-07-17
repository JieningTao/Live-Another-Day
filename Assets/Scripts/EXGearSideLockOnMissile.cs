using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearSideLockOnMissile : EXGearSide
{
    [SerializeField]
    bool MultiLock = true;
    [SerializeField]
    int LockCount = 5;
    [SerializeField]
    BaseMissileLauncher MyLauncher;


    protected BaseMechFCS MyFCS;
    bool Locking = false;


    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyFCS = Mech.GetFCS();
        MyLauncher.EquipWeapon();
    }



    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);


        if (MultiLock)
        {
            if (Down && !Locking)
            {
                MyFCS.RequestLocks(LockCount);
                Locking = true;
            }
            else if (Down && Locking)
            {
                MyLauncher.FireVolley(MyFCS.GetLockedList());
                Locking = false;
            }
        }
        else
        {
            if (Down)
            {
                MyLauncher.FireFocusedVolley(MyFCS.GetMainTarget(), LockCount);
            }
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
