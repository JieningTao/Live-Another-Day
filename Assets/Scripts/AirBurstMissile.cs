using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBurstMissile : BaseMissile
{
    [SerializeField]
    ParticleSystem BurstEffect;
    [SerializeField]
    float BlastDistance = 10;
    [SerializeField]
    BaseBullet FragmentPrefab;
    [SerializeField]
    float SpreadAngle = 30;
    [SerializeField]
    int FragAmount = 20;


    //public virtual void InitializeProjectile(float _Damage, int Layer, DamageSystem.DamageType DamageType, List<DamageSystem.DamageTag> DamageTags, float _TrackingSpeed, float _ActivationDelay )
    //{ 
    //    base.InitializeProjectile(_Damage, Layer, DamageType, DamageTags, _TrackingSpeed,_ActivationDelay);

    //}

    protected override void Update()
    {


        if (ActivationDelay < 0)
        {
            TrackTarget();
            if (Vector3.Distance(Target.transform.position, transform.position) < BlastDistance && Vector3.Angle(transform.forward, Target.transform.position - transform.position) < SpreadAngle / 2)
                Burst();
        }
        else
            ActivationDelay -= Time.deltaTime;


        FlightCheck();
    }

    public override void SetLayerAndMask(int Layer)
    {
        base.SetLayerAndMask(Layer);
        FragmentPrefab.SetLayerAndMask(Layer);
    }

    private void Burst()
    {
        for (int i = 0; i < FragAmount; i++)
        {
            GameObject NewProjectile = Instantiate(FragmentPrefab.gameObject, transform.position, transform.rotation);
            NewProjectile.transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Random.Range(-SpreadAngle / 2, SpreadAngle / 2));
            NewProjectile.SetActive(true);
        }
        if (BurstEffect)
        {
            BurstEffect.transform.parent = null;
            BurstEffect.Play();
        }
        Destroy(gameObject);

    }

    public override string GetDamage
    { get { return FragAmount + " * "+FragmentPrefab.GetDamage; } }


}
