using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSoldier : BaseDrone
{
    [SerializeField]
    AimablePart LeftWeapon;
    [SerializeField]
    AimablePart RightWeapon;
    [SerializeField]
    Transform AIBody;
    [SerializeField]
    float SelfTurnSpeed;


    private void Start()
    {
        base.Start();
        LeftWeapon.GetWeapon(this);
        RightWeapon.GetWeapon(this);
    }

    private void Update()
    {
        LeftWeapon.Target = MTargetSignal;
        LeftWeapon.UpdatePart();
        RightWeapon.Target = MTargetSignal;
        RightWeapon.UpdatePart();

        AimSelf();
    }


    private void AimSelf()
    {
        if (MTargetSignal)
        {
            AimWeapon(AIBody, MTargetSignal.transform.position - transform.position, new Vector3(0, 360, 0), SelfTurnSpeed);
        }
        else
        {
            AimWeapon(AIBody, transform.forward, new Vector3(0, 360, 0), SelfTurnSpeed);
        }


    }

}
