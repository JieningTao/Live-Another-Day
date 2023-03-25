using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMWMultiLockMissileLauncher : BaseMainWeapon
{
    BaseMissileLauncher MyMissileLauncher;

    [SerializeField]
    int LockCount;
    bool Locking = false;

    public override void Equip(bool _Equip, BaseMechMain Operator, bool Right)
    {
        base.Equip(_Equip, Operator, Right);
        MyMissileLauncher = MainWeapon as BaseMissileLauncher;
        if (Operator)
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
                MyFCS.RequestLocks(LockCount,this);
            }
        }
    }
}
