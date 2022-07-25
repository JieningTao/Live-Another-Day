using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearShoulderDeployedLockOnMissile : EXGearShoulderLockOnMissile
{

    public override void Equip(bool a)
    {
        base.Equip(a);

        if (MyAnimator)
            MyAnimator.SetBool("Deployed", a);
    }

}
