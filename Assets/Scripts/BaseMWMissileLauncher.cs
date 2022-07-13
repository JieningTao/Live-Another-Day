using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMWMissileLauncher : BaseMainWeapon
{
    BaseMechFCS MyFCS;
    BaseMissileLauncher MyMissileLauncher;

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
                MyMissileLauncher.Fire1(MyFCS.GetMainTarget());

    }
}
