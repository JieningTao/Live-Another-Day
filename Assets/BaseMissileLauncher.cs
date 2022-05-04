using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMissileLauncher : BaseShoot
{

    [SerializeField]
    GameObject ExplosionPrefab;
    [SerializeField]
    float ExplosiveDamage;
    [SerializeField]
    float explosiveForce;
    [SerializeField]
    protected float TrackingSpeed = 4;
    [SerializeField]
    protected float ActivationDelay = 0.5f;


    List<Transform> SpawnPoints;
    protected BaseMissile MissileScript;


    //public override void InitializeGear(BaseMechFCS FCS)
    //{
    //    base.InitializeGear(FCS);

    //    GetSpawns();
    //    InitializeMissile();

    //}

    protected void GetSpawns()
    {
        SpawnPoints = new List<Transform>();
        SpawnPoints.AddRange( GetComponentsInChildren<Transform>());
        SpawnPoints.Remove(transform);
    }

    protected override void InitializeBullet()
    {
        ProjectilePrefab = Instantiate(ProjectilePrefab, transform);
        ProjectilePrefab.SetActive(false);
        ExplosionPrefab = Instantiate(ExplosionPrefab, ProjectilePrefab.transform);
        ExplosionPrefab.SetActive(false);

        MissileScript = ProjectilePrefab.GetComponent<BaseMissile>();
        BaseExplosion ExplosionScript = ExplosionPrefab.GetComponent<BaseExplosion>();

        int SetLayer = 0;
        if (gameObject.layer == 9)
            SetLayer = 10;
        else if (gameObject.layer == 11)
            SetLayer = 12;

        MissileScript.InitializeProjectile(PerShotDamage, SetLayer, MyDamageType, MyDamageTags, TrackingSpeed, ActivationDelay, ExplosiveDamage, explosiveForce, ExplosionScript);
    }

    public void Fire1(EnergySignal Target) //public for testing
    {
        MissileScript.RecieveTarget(Target);
        base.Fire1();
        MissileScript.RecieveTarget(null);
    }


}
