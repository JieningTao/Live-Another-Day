using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveKineticShoot : BaseKineticShoot
{
    [SerializeField]
    GameObject ExplosionPrefab;
    [SerializeField]
    float ExplosiveDamage;
    [SerializeField]
    float explosiveForce;




    protected override void InitializeBullet()
    {
        //readies an instance of bullet at start that can be modified indipendantly from the prefab 
        ProjectilePrefab = Instantiate(ProjectilePrefab, transform);
        ProjectilePrefab.SetActive(false);

        ExplosiveBullet Temp = ProjectilePrefab.GetComponent<BaseBullet>() as ExplosiveBullet;
        ExplosionPrefab = Instantiate(ExplosionPrefab, ProjectilePrefab.transform);
        BaseExplosion ExplosionScript = ExplosionPrefab.GetComponent<BaseExplosion>();
        ExplosionPrefab.SetActive(false);

        int SetLayer = 0;
        if (gameObject.layer == 9)
            SetLayer = 10;
        else if (gameObject.layer == 11)
            SetLayer = 12;

        Temp.InitializeProjectile(PerShotDamage, SetLayer, MyDamageType, MyDamageTags,ExplosiveDamage,explosiveForce,ExplosionScript);

    }

    protected override void Fire1()
    {
        base.Fire1();
    }

}
