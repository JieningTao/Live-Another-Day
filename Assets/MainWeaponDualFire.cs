using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeaponDualFire : Shoot2MainWeapon
{
    [SerializeField]
    int SecondaryBurst =3;

    BaseMechFCS MyFCS;
    BaseMissileLauncher SecondaryLauncher;



    public override void SecondaryFire(bool Fire)
    {
        //Debug.Log("2");
        if (Fire)
        {
            SecondaryLauncher.FireFocusedVolley(MyFCS.GetMainTarget(), SecondaryBurst);
        }
    }

    public override void Equip(bool _Equip, BaseMechMain Operator)
    {
        MyFCS = Operator.GetFCS();
        base.Equip(_Equip, Operator);
        SecondaryLauncher = SecondaryWeapon as BaseMissileLauncher;
    }
}
