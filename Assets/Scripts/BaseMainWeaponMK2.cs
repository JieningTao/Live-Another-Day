using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMainWeaponMK2 : BaseMainSlotEquipment
{
    [SerializeField]
    class MainSlotSingleWeapon
    {
        [SerializeField]
        public BaseShoot Weapon;
        [SerializeField]
        public string WeaponSN = "N-000";
        [SerializeField]
        public string WeaponName = "Null";
        [SerializeField]
        public Color WeaponGaugeColor;
        [Tooltip("Leave as 0 for any weapon except locking ones")]
        [SerializeField]
        public int LockNum = 0;
        [SerializeField]
        public int LockBurstAmount = 1;

        public BaseMissileLauncher Launcher
        {
            get
            {
                if (_Launcher)
                    return _Launcher;
                else
                {
                    _Launcher = Weapon as BaseMissileLauncher;
                    return _Launcher;
                }
            }
        }
        public BaseMissileLauncher _Launcher;
        public bool AmmoWarning = false;
        public bool EnergyWarning = false;

        public BaseMainWeaponMK2 WeaponEquipmentMaster;

        public virtual void Fire(bool Fire)
        {
            if (LockNum > 0)
            {
                MissileLauncherControls(Launcher, LockNum, LockBurstAmount, Fire);
            }
            else
                Weapon.Trigger(Fire);
        }

        private void MissileLauncherControls(BaseMissileLauncher Launcher, int LockCount, int BurstAmount, bool Fire)
        {
            if (LockCount == 1)
            {
                if (Fire)
                    Launcher.FireFocusedVolley(WeaponEquipmentMaster.Operator.GetMainTarget(), BurstAmount);
            }
            else if (LockCount > 1)
            {
                if (Fire)
                {
                    if (WeaponEquipmentMaster.Locking)
                    {
                        Launcher.FireVolley(WeaponEquipmentMaster.Operator.GetLockedList());
                        WeaponEquipmentMaster.Locking = false;
                        WeaponEquipmentMaster.Operator.RequestLocks(0, this);
                    }
                    else
                    {
                        WeaponEquipmentMaster.Locking = true;
                        WeaponEquipmentMaster.Operator.RequestLocks(LockCount, this);
                    }
                }
            }
        }

        protected virtual void CheckWarnings()
        {
            if (Weapon.LowAmmoWarning() && !AmmoWarning)
                WeaponEquipmentMaster.Operator.SetWeaponWarning(WeaponEquipmentMaster.Right, true, true, true);
            else if (!Weapon.LowAmmoWarning() && AmmoWarning)
                WeaponEquipmentMaster.Operator.SetWeaponWarning(WeaponEquipmentMaster.Right, true, true, false);

            AmmoWarning = Weapon.LowAmmoWarning();

            if (Weapon.LowEnergyWarning() && !EnergyWarning)
                WeaponEquipmentMaster.Operator.SetWeaponWarning(WeaponEquipmentMaster.Right, true, false, true);
            else if (!Weapon.LowEnergyWarning() && EnergyWarning)
                WeaponEquipmentMaster.Operator.SetWeaponWarning(WeaponEquipmentMaster.Right, true, false, false);

            EnergyWarning = Weapon.LowEnergyWarning();
        }
    }

    protected BaseMechFCS MyFCS;
    public bool Locking;// { get; private set; }


}
