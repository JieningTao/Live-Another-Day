using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot2MainWeapon : BaseMainWeapon
{

    //functions have been integrated into base main weapon

    //public override void Equip(bool _Equip, BaseMechMain Operator,bool Right)
    //{
    //    base.Equip(_Equip, Operator,Right);

    //    SecondaryWeapon.EquipWeapon();
    //    if (SecondaryWeapon is BaseEnergyShoot)
    //    {
    //        (SecondaryWeapon as BaseEnergyShoot).GetPowerSource(Operator);
    //    }
    //}

    //protected override void CheckWarnings()
    //{
    //    base.CheckWarnings();


    //}

    //public override void GetInitializeDate(out string MainFunction, out Color MainColor, out string SecondaryFunction, out Color SecondaryColor)
    //{
    //    MainFunction = MainWeaponName;
    //    MainColor = MainWeaponGaugeColor;
    //    SecondaryFunction = SecondaryWeaponName;
    //    SecondaryColor = SecondaryWeaponGaugeColor;
    //}

    //public override void GetUpdateData(bool Main, out float BarFillPercentage, out string TextDisplay)
    //{

    //    BarFillPercentage = 0;
    //    TextDisplay = "";

    //    if (Main)
    //    {
    //        BarFillPercentage = MainWeapon.GetAmmoGauge();
    //        TextDisplay = MainWeapon.GetAmmoText();
    //    }
    //    else
    //    {
    //        BarFillPercentage = SecondaryWeapon.GetAmmoGauge();
    //        TextDisplay = SecondaryWeapon.GetAmmoText();
    //    }
    //}
}
