using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandMLShield : MainHandShield
{
    [SerializeField]
    BaseMissileLauncher MyMissileLauncher;
    [SerializeField]
    protected string MissileGearSN = "N-000";
    [SerializeField]
    protected string MissileGearName = "Null";
    [SerializeField]
    protected Color MissileGearGaugeColor;
    [Tooltip("Leave as 0 for any weapon except locking ones")]
    [SerializeField]
    int MLLockNum = 0;
    [SerializeField]
    int MLLockBurstAmount = 1;

    protected bool Locking = false;


    public override void Equip(bool _Equip, BaseMechMain Operator, bool Right)
    {
        base.Equip(_Equip, Operator, Right);

        if (Operator)
            MyFCS = Operator.GetFCS();
        if (!_Equip)
            MyFCS = null;
    }

    public override void PrimaryFire(bool Fire)
    {
        //Debug.Log(MyFCS);
        if (MLLockNum > 0)
        {
            MissileLauncherControls(MyMissileLauncher, MLLockNum, MLLockBurstAmount, Fire);
        }
    }

    private void MissileLauncherControls(BaseMissileLauncher Launcher, int LockCount, int BurstAmount, bool Fire)
    {
        if (LockCount == 1)
        {
            if (Fire)
                Launcher.FireFocusedVolley(MyFCS.GetMainTarget(), BurstAmount);
        }
        else if (LockCount > 1)
        {
            if (Fire)
            {
                if (Locking)
                {
                    Launcher.FireVolley(MyFCS.GetLockedList());
                    Locking = false;
                    MyFCS.RequestLocks(0, this);
                }
                else
                {
                    Locking = true;
                    MyFCS.RequestLocks(LockCount, this);
                }


            }
        }

    }


    public override void GetInitializeDate(out string MainFunction, out Color MainColor, out string SecondaryFunction, out Color SecondaryColor)
    {
        MainFunction = MissileGearSN +"\n" + MissileGearName;
        MainColor = MissileGearGaugeColor;
        SecondaryFunction = GearSN + "\n" + GearName;
        SecondaryColor =  GearGaugeColor;
    }

    public override void GetUpdateData(bool Main, out float BarFillPercentage, out string TextDisplay)
    {
        if (!Main)
        {
            BarFillPercentage = Shield.GetHealthPercent();
            TextDisplay = Shield.GetHealthText();
        }
        else
        {
            BarFillPercentage = MyMissileLauncher.GetAmmoGauge();
            TextDisplay = MyMissileLauncher.GetAmmoText();
        }


    }

}
