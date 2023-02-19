using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearLockOnMissile : BaseEXGear
{
    [SerializeField]
    int LockCount = 5;
    [SerializeField]
    bool MultiLock = true;
    [SerializeField]
    BaseMissileLauncher MyLauncher;

    protected BaseMechFCS MyFCS;
    bool Locking = false;

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyFCS = Mech.GetFCS();
        MyLauncher.EquipWeapon();
    }



    // Update is called once per frame
    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        if (MultiLock)
        {
            if (Down && !Locking)
            {
                MyFCS.RequestLocks(LockCount, this);
                Locking = true;
            }
            else if (Down && Locking)
            {
                MyLauncher.FireVolley(MyFCS.GetLockedList());
                Locking = false;
                MyFCS.RequestLocks(0, this);
            }
        }
        else
        {
            if (Down && MyLauncher.GetFirable())
                MyLauncher.FireFocusedVolley(MyFCS.GetMainTarget(), LockCount);
        }

    }

    public override void Equip(bool a)
    {
        base.Equip(a);
        if (!a)
            MyFCS.RequestLocks(0,this);
    }

    public override float GetReadyPercentage()
    {
        return MyLauncher.GetAmmoGauge();
    }

    public override string GetBBMainText()
    {
        return MyLauncher.GetAmmoText();
    }

    public override string GetBBSubText()
    {
        if (Locking)
        {
            return "Lock: " + MyFCS.GetLockedAmount() +"/"+LockCount;
        }
        else
        return base.GetBBSubText();
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Damage: ");
        Temp.Add(MyLauncher.GetDamage);

        Temp.Add("Max Lock: ");
        Temp.Add(LockCount+"");

        Temp.Add("Tracking: ");
        Temp.Add(MyLauncher.GetTracking);

        Temp.Add("Reload: ");
        Temp.Add(MyLauncher.GetReload);

        return Temp;
    }

    public override bool IsAimed
    {
        get { return true; }
    }

}
