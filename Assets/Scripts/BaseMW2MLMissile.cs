﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMW2MLMissile : Shoot2MainWeapon
{
    //functions have been integrated into base main weapon

    //[SerializeField]
    //int MainLockNum;
    //[SerializeField]
    //int SecondaryLockNum;



    //BaseMechFCS MyFCS;
    //BaseMissileLauncher MainLauncher;
    //BaseMissileLauncher SecondaryLauncher;

    //bool Locking = false;



    //public override void Equip(bool _Equip, BaseMechMain Operator,bool Right)
    //{
    //    base.Equip(_Equip, Operator,Right);
    //    MainLauncher = MainWeapon as BaseMissileLauncher;
    //    SecondaryLauncher = SecondaryWeapon as BaseMissileLauncher;
    //    if(Operator)
    //    MyFCS = Operator.GetFCS();
    //}

    //public override void PrimaryFire(bool Fire)
    //{
    //    if (Fire)
    //    {
    //        if (Locking)
    //        {
    //            MainLauncher.FireVolley(MyFCS.GetLockedList());
    //            Locking = false;
    //            MyFCS.RequestLocks(0, this);
    //        }
    //        else
    //        {
    //            Locking = true;
    //            MyFCS.RequestLocks(MainLockNum,this);
    //        }
    //    }
    //}

    //public override void SecondaryFire(bool Fire)
    //{
    //    if (Fire)
    //    {
    //        if (Locking)
    //        {
    //            SecondaryLauncher.FireVolley(MyFCS.GetLockedList());
    //            Locking = false;
    //            MyFCS.RequestLocks(0, this);
    //        }
    //        else
    //        {
    //            Locking = true;
    //            MyFCS.RequestLocks(SecondaryLockNum,this);
    //        }
    //    }
    //}

}
