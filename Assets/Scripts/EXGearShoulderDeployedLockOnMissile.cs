using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearShoulderDeployedLockOnMissile : EXGearShoulderLockOnMissile
{
    [SerializeField]
    Animator MyAnimator;

    public override void Equip(bool a)
    {
        base.Equip(a);

        if (MyAnimator)
            MyAnimator.SetBool("Deployed", a);
    }

}
