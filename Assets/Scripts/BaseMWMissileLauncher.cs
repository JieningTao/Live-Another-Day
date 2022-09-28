using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMWMissileLauncher : BaseMainWeapon
{
    BaseMechFCS MyFCS;
    BaseMissileLauncher MyMissileLauncher;

    public override void Equip(bool _Equip, BaseMechMain Operator, bool Right)
    {
        base.Equip(_Equip, Operator, Right);
        MyMissileLauncher = MainWeapon as BaseMissileLauncher;
        if(Operator)
        MyFCS = Operator.GetFCS();
    }

    public override void PrimaryFire(bool Fire)
    {
        //Debug.Log(MyFCS);
        if (Fire)
                MyMissileLauncher.Fire1(MyFCS.GetMainTarget());

    }
}
