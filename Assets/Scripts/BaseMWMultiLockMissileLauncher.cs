using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMWMultiLockMissileLauncher : BaseMainWeapon
{
    BaseMissileLauncher MyMissileLauncher;

    [SerializeField]
    int LockCount;
    bool Locking = false;

    public override void Equip(bool _Equip, BaseMechMain _Operator, bool Right)
    {
        base.Equip(_Equip, _Operator, Right);
        MyMissileLauncher = MainWeapon as BaseMissileLauncher;
    }

    public override void PrimaryFire(bool Fire)
    {
        //Debug.Log(MyFCS);
        if (Fire)
        {
            if (Locking)
            {
                MyMissileLauncher.FireVolley(Operator.GetLockedList());
                Locking = false;
            }
            else
            {
                Locking = true;
                Operator.RequestLocks(LockCount,this);
            }
        }
    }
}
