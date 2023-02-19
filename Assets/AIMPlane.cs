using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMPlane : AIMovement
{
    [SerializeField]
    private LayerMask GroundDetection;
    [SerializeField]
    private float TargetHeight = 30;
    [SerializeField]
    private float CircleDistance = 30;
    [Space(20)]
    [SerializeField]
    private float TurnSpeed = 2;
    [SerializeField]
    private float MoveSpeed = 5;

    RaycastHit GroundUnder;
    Rigidbody MyRB
    { get {
            if (!_MyRB)
                _MyRB = GetComponent<Rigidbody>();
            return _MyRB;
        } }
    Rigidbody _MyRB;

    private void Update()
    {
        if (Target)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, Target.transform.position - transform.position, TurnSpeed * Time.deltaTime, 0.0f);
            //Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        Fly();
    }

    private void Fly()
    {
        MyRB.AddForce(transform.forward*MoveSpeed,ForceMode.Acceleration);
    }

    private void HeightCheck()
    {
        if (Physics.Raycast(transform.position, -transform.up, out GroundUnder,TargetHeight*3, GroundDetection))
        {
            //float Angle = Vector3.Angle(transform.up, GroundUnder.normal); //gets angle of slope

            //return true;
        }
        //return false;
    }



}
