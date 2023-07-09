using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignBullet : BaseBullet
{
    [HideInInspector]
    [SerializeField]
    Vector3 FinalAlignDirection;
    [SerializeField]
    float TurnSpeed = 10;



    public void RecieveAlignmentDirection(Vector3 a)
    {
        FinalAlignDirection = a;
    }

    protected override void FlightCheck()
    {
        base.FlightCheck(1);
        TurnToAlign();
    }

    protected virtual void TurnToAlign()
    {
        if (FinalAlignDirection!=Vector3.zero)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, FinalAlignDirection, TurnSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

}
