using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMissile : BaseBullet
{
    [SerializeField]
    public EnergySignal Target;

    [SerializeField]
    protected Vector2 TrackingSpeedChange = new Vector2(2.5f, 0.8f);

    [SerializeField]
    protected float TrackingChangeTime = 1.5f;

    [SerializeField]
    protected float ActivationDelay = 0.5f;

    [Tooltip("extra percentage chance of not losing tracking to disupts, 0.1 is 10% extra chance to ignore distupt")]
    [SerializeField]
    protected float ExtraTracking = 0;

    protected float Lifetime = 0;
    [HideInInspector]
    [SerializeField]
    protected Transform TrackedObject; //needs to be serialized or keeps reverting to null when missile tracks

    // Update is called once per frame
    protected override void Update()
    { 

        if (Lifetime < TrackingChangeTime)
            Lifetime += Time.deltaTime;

        ActivationDelay -= Time.deltaTime;


        if (ActivationDelay < 0)
            TrackTarget();

        

        FlightCheck();

    }

    // remenant system where missiles are instantiated at play and requires all info about themseles
    //public virtual void InitializeProjectile(float _Damage, int Layer, DamageSystem.DamageType DamageType, List<DamageSystem.DamageTag> DamageTags, float _TrackingSpeed, float _ActivationDelay)
    //{
    //    base.InitializeProjectile(_Damage, Layer, DamageType, DamageTags);

    //    TrackingSpeed = _TrackingSpeed;
    //    ActivationDelay = _ActivationDelay;

    //}

    public virtual void RecieveTarget(EnergySignal NewTarget)
    {

        Target = NewTarget;
        if(NewTarget)
            TrackedObject = Target.transform;
    }

    protected virtual void TrackTarget()
    {
        //
        if (TrackedObject != null)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, TrackedObject.position - transform.position, CurrentTrackingSpeed * Time.deltaTime, 0.0f);
            //Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }


    protected virtual float CurrentTrackingSpeed
    {
        get
        {
            if (Lifetime > TrackingChangeTime)
                return TrackingSpeedChange.y;

            //Debug.Log(Mathf.Lerp(TrackingSpeedChange.x, TrackingSpeedChange.y, Lifetime / TrackingChangeTime));

            return Mathf.Lerp(TrackingSpeedChange.x, TrackingSpeedChange.y, Lifetime / TrackingChangeTime);
        }
    }

    public string GetTracking
    {get {  return TrackingSpeedChange.y + "";  } }

    private void OnEnable()
    {
        EnergySignal.LockDisrupt += LockDisrupt;
    }

    private void OnDisable()
    {
        EnergySignal.LockDisrupt -= LockDisrupt;
    }

    private void LockDisrupt(EnergySignal SignalDisrupted, float DisruptionIntensity,GameObject DivertedObject)
    {
        if (SignalDisrupted != Target)
            return;

        DisruptionIntensity -= ExtraTracking;

        if (Random.Range(0f, 1f) > DisruptionIntensity)
        {
            //did not distupt

        }
        else
        {
            //disrupted
            if (DivertedObject == null)
                Target = null;
            else
            {
                Target = null;
                TrackedObject = DivertedObject.transform;
                //Debug.Log("Distupted");
            }

        }

    }
}
