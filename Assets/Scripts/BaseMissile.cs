using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMissile : BaseBullet
{
    [SerializeField]
    public EnergySignal Target;

    [SerializeField]
    protected float TrackingSpeed = 4;

    [SerializeField]
    protected float ActivationDelay = 0.5f;

    // Update is called once per frame
    protected override void Update()
    {


        if (ActivationDelay < 0)
            TrackTarget();
        else
            ActivationDelay -= Time.deltaTime;
        

        FlightCheck();
    }

    //public virtual void InitializeProjectile(float _Damage, int Layer, DamageSystem.DamageType DamageType, List<DamageSystem.DamageTag> DamageTags, float _TrackingSpeed, float _ActivationDelay)
    //{
    //    base.InitializeProjectile(_Damage, Layer, DamageType, DamageTags);

    //    TrackingSpeed = _TrackingSpeed;
    //    ActivationDelay = _ActivationDelay;

    //}

    public void RecieveTarget(EnergySignal NewTarget)
    {
        Target = NewTarget;
    }

    protected void TrackTarget()
    {
        if (Target != null)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, Target.transform.position - transform.position, TrackingSpeed * Time.deltaTime, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

}
