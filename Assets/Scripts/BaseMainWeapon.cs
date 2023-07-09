using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMainWeapon : BaseMainSlotEquipment
{
    //[SerializeField]
    //class MainSlotSingleWeapon
    //{
    //    [SerializeField]
    //    protected BaseShoot Weapon;
    //    [SerializeField]
    //    protected string WeaponSN = "N-000";
    //    [SerializeField]
    //    protected string WeaponName = "Null";
    //    [SerializeField]
    //    protected Color WeaponGaugeColor;
    //    [Tooltip("Leave as 0 for any weapon except locking ones")]
    //    [SerializeField]
    //    int LockNum = 0;
    //    [SerializeField]
    //    int LockBurstAmount = 1;

    //    protected BaseMissileLauncher Launcher
    //    {
    //        get
    //        {
    //            if (_Launcher)
    //                return _Launcher;
    //            else
    //            {
    //                _Launcher = Weapon as BaseMissileLauncher;
    //                return _Launcher;
    //            }
    //        }
    //    }
    //    protected BaseMissileLauncher _Launcher;
    //    protected bool AmmoWarning = false;
    //    protected bool EnergyWarning = false;


    //    public override void Fire(bool Fire)
    //    {
    //        if (LockNum > 0)
    //        {
    //            MissileLauncherControls(Launcher, LockNum, LockBurstAmount, Fire);
    //        }
    //        else
    //            Weapon.Trigger(Fire);
    //    }

    //    protected virtual void CheckWarnings()
    //    {
    //        if (Weapon.LowAmmoWarning() && !AmmoWarning)
    //            Operator.SetWeaponWarning(Right, true, true, true);
    //        else if (!Weapon.LowAmmoWarning() && AmmoWarning)
    //            Operator.SetWeaponWarning(Right, true, true, false);

    //        AmmoWarning = Weapon.LowAmmoWarning();

    //        if (Weapon.LowEnergyWarning() && !EnergyWarning)
    //            Operator.SetWeaponWarning(Right, true, false, true);
    //        else if (!Weapon.LowEnergyWarning() && EnergyWarning)
    //            Operator.SetWeaponWarning(Right, true, false, false);

    //        EnergyWarning = Weapon.LowEnergyWarning();
    //    }
    //}



    [SerializeField]
    protected BaseShoot MainWeapon;
    [SerializeField]
    protected string MainWeaponSN = "N-000";
    [SerializeField]
    protected string MainWeaponName= "Null";
    [SerializeField]
    protected Color MainWeaponGaugeColor;
    [Tooltip("Leave as 0 for any weapon except locking ones")]
    [SerializeField]
    int MainLockNum = 0;
    [SerializeField]
    int MainLockBurstAmount = 1;

    protected BaseMissileLauncher MainLauncher
    {
        get {
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

    [Space(20)]

    [SerializeField]
    protected BaseShoot SecondaryWeapon;
    [SerializeField]
    protected string SecondaryWeaponSN = "";
    [SerializeField]
    protected string SecondaryWeaponName = "";
    [SerializeField]
    protected Color SecondaryWeaponGaugeColor;
    [Tooltip("Leave as 0 for any weapon except locking ones")]
    [SerializeField]
    int SecondaryLockNum = 0;
    [SerializeField]
    int SecondaryLockBurstAmount = 1;

    protected BaseMissileLauncher SecondaryLauncher
    {
        get
        {
            if (_SecondaryLauncher)
                return _SecondaryLauncher;
            else
            {
                _SecondaryLauncher = SecondaryWeapon as BaseMissileLauncher;
                return _SecondaryLauncher;
            }
        }
    }
    protected BaseMissileLauncher _SecondaryLauncher;
    protected bool SecondaryAmmoWarning = false;
    protected bool SecondaryEnergyWarning = false;

    protected bool Locking = false;

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
        if (HasSecondary)
        {
            if ( SecondaryLockNum > 0)
            {
                MissileLauncherControls(SecondaryLauncher, SecondaryLockNum, SecondaryLockBurstAmount, Fire);
            }
            else
                SecondaryWeapon.Trigger(Fire);
        }

    }

    public override void OperatorInit()
    {
        base.OperatorInit();

        if (MainWeapon is BaseKineticShoot)
            KineticWeaponInitAmmo(MainWeapon as BaseKineticShoot);

        if (SecondaryWeapon is BaseKineticShoot)
            KineticWeaponInitAmmo(SecondaryWeapon as BaseKineticShoot);

    }

    public void KineticWeaponInitAmmo(BaseKineticShoot a)
    {
        if (a.GetAmmoIdentifier!="")
        {
            AttributeManager.ExtraAttribute EA = Operator.FetchExtraAttribute(a.GetAmmoIdentifier);
            //Debug.Log(EA.AttributeName);
            if (EA!=null)
                a.SetAttributeExtraAmmo(EA.TributeAmount);

        }
    }

    protected virtual void Update()
    {
        if(Operator)
        CheckWarnings();
    }

    public bool HasSecondary
    {
        get
        {
            return SecondaryWeapon != null;
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

    protected virtual void CheckWarnings()
    {
        if (MainWeapon.LowAmmoWarning() && !MainAmmoWarning)
            Operator.SetWeaponWarning(Right, true, true, true);
        else if (!MainWeapon.LowAmmoWarning() && MainAmmoWarning)
            Operator.SetWeaponWarning(Right, true, true, false);

        MainAmmoWarning = MainWeapon.LowAmmoWarning();

        if (MainWeapon.LowEnergyWarning() && !MainEnergyWarning)
            Operator.SetWeaponWarning(Right, true, false, true);
        else if (!MainWeapon.LowEnergyWarning() && MainEnergyWarning)
            Operator.SetWeaponWarning(Right, true, false, false);

        MainEnergyWarning = MainWeapon.LowEnergyWarning();

        if (HasSecondary)
        {
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

    }

    public override void Equip(bool _Equip, BaseMechMain _Operator,bool Right)
    {
        base.Equip(_Equip, _Operator,Right);

        MainWeapon.EquipWeapon();
        if (MainWeapon is BaseEnergyShoot)
        {
            (MainWeapon as BaseEnergyShoot).GetPowerSource(_Operator);
        }


        if (HasSecondary)
        {
            SecondaryWeapon.EquipWeapon();
            if (SecondaryWeapon is BaseEnergyShoot)
            {
                (SecondaryWeapon as BaseEnergyShoot).GetPowerSource(_Operator);
            }
        }

    }

    public override void GetInitializeDate(out string MainFunction, out Color MainColor, out string SecondaryFunction, out Color SecondaryColor)
    {
        MainFunction = MainWeaponSN + "\n" + MainWeaponName;
        MainColor = MainWeaponGaugeColor;

        SecondaryFunction = SecondaryWeaponSN + "\n" + SecondaryWeaponName;
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
            if (HasSecondary)
            {
                BarFillPercentage = SecondaryWeapon.GetAmmoGauge();
                TextDisplay = SecondaryWeapon.GetAmmoText();
            }
            else
            {
                BarFillPercentage = 0;
                TextDisplay = "";
            }


        }
    }


    //public virtual float ReadyPercentage
    //{ get { return 0; } }

    //public virtual string ReadyText
    //{ get { return ""; } }

    public override float GetBulletSpeed()
    {
        return MainWeapon.GetProjectileSpeed();
    }

    public virtual string GetName()
    {
        return MainWeaponName;
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Damage: ");
        Temp.Add(MainWeapon.GetDamage);

        Temp.Add("Accuracy: ");
        Temp.Add(MainWeapon.GetAccuracy);

        Temp.Add("Fire Rate: ");
        Temp.Add(MainWeapon.GetFireRate);

        Temp.Add("Fire Mode: ");
        Temp.Add(MainWeapon.GetFireMode);

        if (MainWeapon is BaseKineticShoot)
        {
            Temp.Add("Magazine: ");
            Temp.Add(MainWeapon.GetMag);

            Temp.Add("Reload: ");
            Temp.Add(MainWeapon.GetReload);
        }
        else if (MainWeapon is BaseEnergyShoot)
        {
            Temp.Add("Charge: ");
            Temp.Add(MainWeapon.GetMag);

            Temp.Add("Recharge: ");
            Temp.Add(MainWeapon.GetReload);
        }



        return Temp;
    }

    public override int GetSecondaryDisplayConfig
    { get {
            if (SecondaryWeaponGaugeColor == Color.black|| SecondaryWeaponName == "")
                return 0;
            else
                return 1;
        } }
}
