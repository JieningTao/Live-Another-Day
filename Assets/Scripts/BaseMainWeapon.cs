using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMainWeapon : BaseMainSlotEquipment
{
    [SerializeField]
    protected BaseShoot MainWeapon;
    [SerializeField]
    protected string MainWeaponSN = "N-000";
    [SerializeField]
    protected string MainWeaponName= "Null";
    [SerializeField]
    protected Color MainWeaponGaugeColor;


    public override void PrimaryFire(bool Fire)
    {
        MainWeapon.Trigger(Fire);
    }

    public override void Equip(bool _Equip, BaseMechMain Operator,bool Right)
    {
        base.Equip(_Equip, Operator,Right);
        MainWeapon.EquipWeapon();
        if (MainWeapon is BaseEnergyShoot)
        {
            (MainWeapon as BaseEnergyShoot).GetPowerSource(Operator);
        }
    }

    public override void GetInitializeDate(out string MainFunction, out Color MainColor, out string SecondaryFunction, out Color SecondaryColor)
    {
        MainFunction = MainWeaponSN+"\n"+ MainWeaponName;
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

    public override float GetBulletSpeed()
    {
        return MainWeapon.GetProjectileSpeed();
    }
}
