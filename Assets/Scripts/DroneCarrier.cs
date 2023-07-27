using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCarrier : BaseDrone
{

    [SerializeField]
    Transform MoveTarget;




    protected override void Start()
    {
        base.Start();
        (MyMovement as AIMAssistedRB).RecieveTargetPosition(MoveTarget.transform.position);

    }











}
