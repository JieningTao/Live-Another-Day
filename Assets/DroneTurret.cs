using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTurret : BaseDrone
{
    [SerializeField]
    AimablePart LeftWeapon;
    [SerializeField]
    AimablePart RightWeapon;
    [SerializeField]
    Transform TurretBody;
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
            AimWeapon(TurretBody, MTargetSignal.transform.position - transform.position, new Vector3(0, 360, 0), SelfTurnSpeed);
        }
        else
        {
            AimWeapon(TurretBody, transform.forward, new Vector3(0, 360, 0), SelfTurnSpeed);
        }
    }







}
