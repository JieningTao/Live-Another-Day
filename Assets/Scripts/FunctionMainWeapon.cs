using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionMainWeapon : BaseMainSlotEquipment
{
    [SerializeField]
    protected BaseShoot MainWeapon;
    [SerializeField]
    protected string MainWeaponSN = "N-000";
    [SerializeField]
    protected string MainWeaponName = "Null";
    [SerializeField]
    protected Color MainWeaponGaugeColor;
    [Tooltip("Leave as 0 for any weapon except locking ones")]
    [SerializeField]
    int MainLockNum = 0;
    [SerializeField]
    int MainLockBurstAmount = 1;

    [Space(20)]

    [SerializeField]
    BaseSecondaryMainWeaponFunction Function;

    protected BaseMissileLauncher MainLauncher
    {
        get
        {
            if (_MainLauncher)
                return _MainLauncher;
            else
            {
                _MainLauncher = MainWeapon as BaseMissileLauncher;
                return _MainLauncher;
            }
        }
    }
    protected BaseMissileLauncher _MainLauncher;

    protected bool MainAmmoWarning = false;
    protected bool MainEnergyWarning = false;

    protected bool Locking = false;

    public override void Equip(bool _Equip, BaseMechMain _Operator, bool Right)
    {
        base.Equip(_Equip, _Operator, Right);

        MainWeapon.EquipWeapon();
        if (MainWeapon is BaseEnergyShoot)
            (MainWeapon as BaseEnergyShoot).GetPowerSource(_Operator);

        if (Function)
            Function.InitFunction(_Operator);
    }

    public override void PrimaryFire(bool Fire)
    {
        if (MainLockNum > 0)
        {
            MissileLauncherControls(MainLauncher, MainLockNum, MainLockBurstAmount, Fire);
        }
        else
            MainWeapon.Trigger(Fire);
    }

    public override void SecondaryFire(bool Fire)
    {
        if(Function)
        Function.Trigger(Fire);
    }

    public override void GetInitializeDate(out string MainFunction, out Color MainColor, out string SecondaryFunction, out Color SecondaryColor)
    {
        MainFunction = MainWeaponSN + "\n" + MainWeaponName;
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
        else if(Function)
        {
            BarFillPercentage = 0;
            TextDisplay = Function.UpdateText;
        }
    }


    private void MissileLauncherControls(BaseMissileLauncher Launcher, int LockCount, int BurstAmount, bool Fire)
    {
        if (LockCount == 1)
        {
            if (Fire)
                Launcher.FireFocusedVolley(Operator.GetMainTarget(), BurstAmount);
        }
        else if (LockCount > 1)
        {
            if (Fire)
            {
                if (Locking)
                {
                    Launcher.FireVolley(Operator.GetLockedList());
                    Locking = false;
                    Operator.RequestLocks(0, this);
                }
                else
                {
                    Locking = true;
                    Operator.RequestLocks(LockCount, this);
                }


            }
        }

    }

    public override float GetBulletSpeed()
    {
        return MainWeapon.GetProjectileSpeed();
    }
    public override int GetSecondaryDisplayConfig
    {
        get
        {
            if (Function)
                return 3;
            else
                return 0;
        }
    }
}
