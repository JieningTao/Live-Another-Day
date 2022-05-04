using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearLockOnMissile : BaseEXGear
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TriggerGear(true);
    }

    // Update is called once per frame
    public override void TriggerGear(bool Down)
    {
        if (Down)
            GetComponent<BaseMissileLauncher>().Fire1(MyFCS.MainTarget);
    }
}
