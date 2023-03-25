using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBBullet : BaseBullet
{
    [SerializeField]
    Rigidbody MyRB;


    protected override void Start()
    {
        base.Start();
        MyRB.AddForce(transform.forward * Speed, ForceMode.VelocityChange);
    }

    protected override void Update()
    {
        base.Update();
        TurnToFaceMovingDirection();
    }

    protected virtual void TurnToFaceMovingDirection()
    {
        transform.rotation = Quaternion.LookRotation(MyRB.velocity.normalized);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        DealDamageTo(collision.gameObject);
    }
}
