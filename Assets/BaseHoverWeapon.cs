using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHoverWeapon : MonoBehaviour
{
    [SerializeField]
    Transform FollowPoint;
    [SerializeField]
    Vector2 FollowRange = new Vector2(1, 2);
    [SerializeField]
    Vector2 SpeedRange = new Vector2(0.2f, 1);
    [SerializeField]
    Rigidbody MyRB;
    [SerializeField]
    float Force;

    [SerializeField]
    GameObject Target;
    [SerializeField]
    float TurnSpeed;
    [SerializeField]
    BaseShoot MyWeapon;




    private void Update()
    {
        Movement();
        Turn();
    }

    private void Turn()
    {


        if (Target != null)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, Target.transform.position - transform.position, TurnSpeed * Time.deltaTime, 0.0f);
            //Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }

    }

    private void Movement()
    {
        float Dis = Vector3.Distance(transform.position, FollowPoint.position);

        if (Dis > FollowRange.x)
        {
            MyRB.AddForce((FollowPoint.position - transform.position).normalized * Force * GetSpeed(), ForceMode.VelocityChange);
        }
    }

    private float GetSpeed()
    {
        return Mathf.Lerp(SpeedRange.x, SpeedRange.y, GetSpeedPercentage());
    }

    private float GetSpeedPercentage()
    {
        float Dis = Vector3.Distance(transform.position, FollowPoint.position);

        if (Dis < FollowRange.x)
            return 0;
        else if (Dis > FollowRange.y)
            return 1;
        else
            return (Dis - FollowRange.x) / (FollowRange.y - FollowRange.x);
    }

}
