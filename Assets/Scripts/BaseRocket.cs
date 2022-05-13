using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRocket : BaseBullet
{
    [SerializeField]
    Vector2 MinMaxSpeed = new Vector2(20, 100);
    [SerializeField]
    float SpeedUpTime = 2;

    protected float Lifetime = 0;

    protected override void Update()
    {
        base.Update();
        if(Lifetime<SpeedUpTime)
        Lifetime += Time.deltaTime;
    }

    protected override void FlightCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, CurrentSpeed * Time.deltaTime, HitMask))
        {
            transform.Translate(Vector3.forward * hit.distance);
            DealDamageTo(hit.collider.gameObject);
        }
        else
        {
            transform.Translate(Vector3.forward * CurrentSpeed * Time.deltaTime);
        }
    }

    protected virtual float CurrentSpeed
    {
        get
        {
            if (Lifetime > SpeedUpTime)
                return MinMaxSpeed.y;

            return Mathf.Lerp(MinMaxSpeed.x, MinMaxSpeed.y, Lifetime / SpeedUpTime);
        }
    }


}
