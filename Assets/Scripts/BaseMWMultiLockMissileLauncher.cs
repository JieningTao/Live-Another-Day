using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMWMultiLockMissileLauncher : BaseMainWeapon
{
    BaseMechFCS MyFCS;
    BaseMissileLauncher MyMissileLauncher;

    [SerializeField]
    int LockCount;
    bool Locking = false;

    public override void Equip(bool _Equip, BaseMechMain Operator)
    {

        base.Equip(_Equip, Operator);
        MyMissileLauncher = MainWeapon as BaseMissileLauncher;
        MyFCS = Operator.GetFCS();
    }

    public override void PrimaryFire(bool Fire)
    {
        //Debug.Log(MyFCS);
        if (Fire)
        {
            if (Locking)
            {
                MyMissileLauncher.FireVolley(MyFCS.GetLockedList());
                Locking = false;
            }
            else
            {
                Locking = true;
                MyFCS.RequestLocks(LockCount);
            }
        }
    }
}
