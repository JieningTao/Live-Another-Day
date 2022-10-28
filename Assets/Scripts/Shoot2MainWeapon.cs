using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot2MainWeapon : BaseMainWeapon
{

    [SerializeField]
    protected BaseShoot SecondaryWeapon;
    [SerializeField]
    protected string SecondaryWeaponSN = "N-000";
    [SerializeField]
    protected string SecondaryWeaponName = "Null";
    [SerializeField]
    protected Color SecondaryWeaponGaugeColor;

    bool SecondaryAmmoWarning = false;
    bool SecondaryEnergyWarning = false;

    public override void PrimaryFire(bool Fire)
    {
        MainWeapon.Trigger(Fire);
    }

    public override void SecondaryFire(bool Fire)
    {
        //Debug.Log("2");
        SecondaryWeapon.Trigger(Fire);
    }

    public override void Equip(bool _Equip, BaseMechMain Operator,bool Right)
    {
        base.Equip(_Equip, Operator,Right);

        SecondaryWeapon.EquipWeapon();
        if (SecondaryWeapon is BaseEnergyShoot)
        {
            (SecondaryWeapon as BaseEnergyShoot).GetPowerSource(Operator);
        }
    }

    protected override void CheckWarnings()
    {
        base.CheckWarnings();

        if (SecondaryWeapon.LowAmmoWarning() && !SecondaryAmmoWarning)
            Operator.SetWeaponWarning(Right, false, true, true);
        else if (!SecondaryWeapon.LowAmmoWarning() && SecondaryAmmoWarning)
            Operator.SetWeaponWarning(Right, false, true, false);

        SecondaryAmmoWarning = SecondaryWeapon.LowAmmoWarning();

        if (SecondaryWeapon.LowEnergyWarning() && !SecondaryEnergyWarning)
            Operator.SetWeaponWarning(Right, false, false, true);
        else if (!SecondaryWeapon.LowEnergyWarning() && SecondaryEnergyWarning)
            Operator.SetWeaponWarning(Right, false, false, false);

        SecondaryEnergyWarning = SecondaryWeapon.LowEnergyWarning();
    }

    public override void GetInitializeDate(out string MainFunction, out Color MainColor, out string SecondaryFunction, out Color SecondaryColor)
    {
        MainFunction = MainWeaponName;
        MainColor = MainWeaponGaugeColor;
        SecondaryFunction = SecondaryWeaponName;
        SecondaryColor = SecondaryWeaponGaugeColor;
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
        else
        {
            BarFillPercentage = SecondaryWeapon.GetAmmoGauge();
            TextDisplay = SecondaryWeapon.GetAmmoText();
        }
    }
}
