using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class has been replaced in function by BaseMainWeapon
public class BaseMWMissileLauncher : BaseMainWeapon
{
    BaseMissileLauncher MyMissileLauncher;


    [SerializeField]
    int MainBurstAmount =1;

    public override void Equip(bool _Equip, BaseMechMain Operator, bool Right)
    {
        base.Equip(_Equip, Operator, Right);
        MyMissileLauncher = MainWeapon as BaseMissileLauncher;
    }

    public override void PrimaryFire(bool Fire)
    {
        //Debug.Log(MyFCS);
        if (Fire)
            MyMissileLauncher.FireFocusedVolley(Operator.GetMainTarget(),MainBurstAmount);
    }
}
