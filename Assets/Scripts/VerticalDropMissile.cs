using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDropMissile : BaseMissile
{
    [SerializeField]
    float DropHeight = 50;
    [SerializeField]
    float DropDecisionTan = 1; //the factor that decides how close to the exact drop site the missile starts to drop, 1 gives a drop starting point with equal sides and a perfect tracking missile a 45 degree dive.
    [SerializeField]
    bool Dropping = false;


    protected override void TrackTarget()
    {
        if (Target != null)
        {
            Vector3 newDir;

            if (!Dropping)
            {
                newDir = Vector3.RotateTowards(transform.forward, (Target.transform.position + new Vector3(0, DropHeight, 0)) - transform.position, TrackingSpeed * Time.deltaTime, 0.0f);
                CheckToDrop();
            }
            else
                newDir = Vector3.RotateTowards(transform.forward, Target.transform.position - transform.position, TrackingSpeed * Time.deltaTime, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    protected virtual void CheckToDrop()
    {
        if (Vector3.Distance(transform.position, Target.transform.position + new Vector3(0, DropHeight, 0)) <= DropHeight * DropDecisionTan)
            Dropping = true;
    }


}
