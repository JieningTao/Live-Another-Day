using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearLockOnMissile : BaseEXGear
{
    [SerializeField]
    BaseMissileLauncher MyLauncher;
    bool Locking = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TriggerGear(true);
    }

    // Update is called once per frame
    public override void TriggerGear(bool Down)
    {
        if (Down&&!Locking)
        {
            MyFCS.RequestLocks(15);
            Locking = true;
        }
        else if (Down && Locking)
        {
            MyLauncher.FireVolly(MyFCS.GetLockedList());
            Locking = false;
        }
            //GetComponent<BaseMissileLauncher>().Fire1(MyFCS.MainTarget);
    }
}
