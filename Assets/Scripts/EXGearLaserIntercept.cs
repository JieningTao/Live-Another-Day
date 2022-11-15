using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearLaserIntercept : BaseEXGear
{
    [SerializeField]
    AntiMissileLaser InterceptSystem;

    bool IsOn = false;
    float PowerDraw;


    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        InterceptSystem.InitializeGear(Mech);
        PowerDraw = InterceptSystem.GetPowerDraw();
    }

    public override void TriggerGear(bool Down)
    {
        if (Down)
        {
            if (IsOn)
            {
                InterceptSystem.ToggleOn(false);
                IsOn = false;
            }
            else
            {
                InterceptSystem.ToggleOn(true);
                IsOn = true;
            }
        }

    }

    public override float GetReadyPercentage()
    {
        return 1;
    }


    public override string GetBBMainText()
    {
        if (IsOn)
            return "Active";
        else
            return "Standby";
    }

    public override string GetBBSubText()
    {
        string Temp = "";

        if (IsOn)
            Temp += "Active\n";
        else
            Temp += "Inactive\n";

        if(InterceptSystem.IsLaserOn)
            Temp += "Drawing " + PowerDraw.ToString("F1") + "EU";
        else
            Temp += "Drawing 0.0 EU";

        return Temp;
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Range: ");
        Temp.Add(InterceptSystem.GetInterceptRange);

        Temp.Add("Power draw: ");
        Temp.Add(InterceptSystem.GetEnergyDraw);

        Temp.Add("Damage: ");
        Temp.Add(InterceptSystem.GetDPS);

        return Temp;
    }
}
