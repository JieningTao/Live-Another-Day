using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMainWeapon : BaseMainSlotEquipment
{
    [SerializeField]
    protected BaseShoot MainWeapon;
    [SerializeField]
    protected string MainWeaponName;
    [SerializeField]
    protected Color MainWeaponGaugeColor;


    public override void PrimaryFire(bool Fire)
    {
        MainWeapon.Trigger(Fire);
    }

    public override void GetInitializeDate(out string MainFunction, out Color MainColor, out string SecondaryFunction, out Color SecondaryColor)
    {
        MainFunction = MainWeaponName;
        MainColor = MainWeaponGaugeColor;
        SecondaryFunction = "";
        SecondaryColor = Color.black;
    }

    public override void GetUpdateData(bool Main, out float BarFillPercentage, out string TextDisplay)
    {

        BarFillPercentage = 0;
        TextDisplay = "";

        if (Main)
        {
            BarFillPercentage = MainWeapon.GetAmmoGauge();
            TextDisplay = MainWeapon.GetAmmoText();
        }
    }
}
