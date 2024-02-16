using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatFollowTest : MonoBehaviour
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




    private void Update()
    {
        float Dis = Vector3.Distance(transform.position, FollowPoint.position);

        if (Dis > FollowRange.x)
        {
            MyRB.AddForce((FollowPoint.position - transform.position).normalized * Force*GetSpeed(), ForceMode.VelocityChange);
        }

            //transform.position = Vector3.MoveTowards(transform.position, FollowPoint.position, GetSpeedPercentage()*0.5f);
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
