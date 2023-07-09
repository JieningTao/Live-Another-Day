using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearSingleMissile : BaseEXGear
{
    protected BaseMechFCS MyFCS;
    [SerializeField]
    BaseMissileLauncher MyLauncher;

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyFCS = Mech.GetFCS();
        MyLauncher.EquipWeapon();
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        if (Down)
            MyLauncher.FireFocusedVolley(MyFCS.GetMainTarget(), 1);

    }

    public override string GetBBMainText()
    {
        return MyLauncher.GetAmmoText();
    }

    public override float GetReadyPercentage()
    {
        return MyLauncher.GetAmmoGauge();
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();


        return Temp;
    }
}
