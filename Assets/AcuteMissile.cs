using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcuteMissile : BaseMissile
{
    [SerializeField]
    private ParticleSystem SnapEffect;
    [SerializeField]
    private float PreSnapSpeed = 0.5f;


    private bool Snapped = false;

    protected override void Start()
    {
        base.Start();
        ActivationDelay += Random.Range(0.2f,-0.2f);
    }

    protected override void Update()
    {

        if (ActivationDelay > 0)
        {
            ActivationDelay -= Time.deltaTime;
            FlightCheck(PreSnapSpeed);
        }
        else
        {
            if (!Snapped)
            {
                Snap();
            }
            else
            {
                TrackTarget();
            }
            FlightCheck();
        }


    }

    protected virtual void Snap()
    {
        transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position,Vector3.up);

        Instantiate(SnapEffect.gameObject, transform.position, transform.rotation);
        Snapped = true;

    }

}
