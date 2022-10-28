using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearLaserIntercept : BaseEXGear
{
    [SerializeField]
    AntiMissileLaser InterceptSystem;

    bool IsOn = false;



    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        InterceptSystem.InitializeGear(Mech);
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
}
