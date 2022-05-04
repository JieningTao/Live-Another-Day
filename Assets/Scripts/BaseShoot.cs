using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShoot : MonoBehaviour
{
    [SerializeField]
    protected List<Transform> BulletSpawns;

    [SerializeField]
    protected GameObject ProjectilePrefab;

    [SerializeField]
    protected float AccuracyDeviation;


    [SerializeField]
    protected float TBS = 0.1f;

    [SerializeField]
    protected float PerShotDamage = 10;

    [SerializeField]
    public DamageSystem.DamageType MyDamageType;
    [SerializeField]
    public List<DamageSystem.DamageTag> MyDamageTags;

    [SerializeField]
    protected FireMode MyFireMode;
    [SerializeField]
    protected List<ParticleSystem> MuzzleFlares;


    public enum FireMode
    {
        SemiAuto,
        FullAuto,
    }


    protected bool Firing = false;
    protected float FireCooldown = 0;
    protected LayerMask BulletHitMask;
    private int ShotsFired=0;
    //remnant system
    public WeaponStatus MyStatus { get; protected set; }

    public enum WeaponStatus
    {
        Normal,
        Loading,
        Empty,
        Cooldown,
        Error
    }

    protected virtual void Start()
    {
        foreach(Transform a in BulletSpawns)
            a.gameObject.SetActive(false);
        InitializeBullet();
        MyStatus = WeaponStatus.Normal;
    }

    protected virtual int GetNextBulletSpawn()
    {
        if (BulletSpawns.Count == 1)
            return 0;

        return GetNextBulletSpawn(true);
    }

    protected virtual int GetNextBulletSpawn(bool count)
    {
        if (count)
            ShotsFired++;

        return ShotsFired % BulletSpawns.Count;

    }

    protected virtual void InitializeBullet()
    {
        //readies an instance of bullet at start that can be modified indipendantly from the prefab 
        ProjectilePrefab = Instantiate(ProjectilePrefab,transform);
        ProjectilePrefab.SetActive(false);
        BaseBullet Temp = ProjectilePrefab.GetComponent<BaseBullet>();

        int SetLayer = 0;

        if (gameObject.layer == 9)
            SetLayer = 10;
        else if (gameObject.layer == 11)
            SetLayer = 12;

        Temp.InitializeProjectile(PerShotDamage, SetLayer, MyDamageType, MyDamageTags);

    }

    public virtual void Trigger(bool Fire)
    {

        if (Fire && MyFireMode == FireMode.SemiAuto && FireCooldown <= 0)
            Fire1();
        else
            Firing = Fire;
    }

    protected virtual void Update()
    {

        if (MyFireMode == FireMode.FullAuto)
        {
            if (FireCooldown > 0)
                FireCooldown -= Time.deltaTime;
            else
            {
                if (Firing)
                {
                    Fire1();

                    FireCooldown = TBS;
                }
            }
        }

    }

    protected virtual void Fire1()
    {
        int SlotNum = GetNextBulletSpawn();
        GameObject NewBullet = GameObject.Instantiate(ProjectilePrefab, BulletSpawns[SlotNum].position, BulletSpawns[SlotNum].rotation);
        NewBullet.SetActive(true);
        NewBullet.transform.Rotate(new Vector3(0, Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), 0), Space.World);

        if (MuzzleFlares!=null&&MuzzleFlares.Count>SlotNum)
            MuzzleFlares[SlotNum].Play();
    }

    public virtual float GetAmmoGauge()
    {
        return 1;
    }

    public virtual string GetAmmoText()
    {
        return "";
    }

}
