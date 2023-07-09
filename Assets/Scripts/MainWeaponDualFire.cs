using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeaponDualFire : Shoot2MainWeapon
{
    [SerializeField]
    int SecondaryBurst =3;

    BaseMissileLauncher SecondaryLauncher;



    public override void SecondaryFire(bool Fire)
    {
        //Debug.Log("2");
        if (Fire)
        {
            SecondaryLauncher.FireFocusedVolley(Operator.GetMainTarget(), SecondaryBurst);
        }
    }

    public override void Equip(bool _Equip, BaseMechMain Operator, bool Right)
    {
        base.Equip(_Equip, Operator, Right);
        SecondaryLauncher = SecondaryWeapon as BaseMissileLauncher;
    }
}
