using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShoot : MonoBehaviour
{
    [SerializeField]
    protected List<Transform> BulletSpawns;

    [SerializeField]
    protected GameObject ProjectilePrefab;
    protected BaseBullet ProjectileScript;

    [SerializeField]
    protected float AccuracyDeviation;


    [SerializeField]
    protected float TBS = 0.1f;

    //[SerializeField]
    //protected float PerShotDamage = 10;

    //[SerializeField]
    //public DamageSystem.DamageType MyDamageType;
    //[SerializeField]
    //public List<DamageSystem.DamageTag> MyDamageTags;

    [SerializeField]
    protected FireMode MyFireMode;
    [SerializeField]
    protected GameObject MuzzleFlarePrefab;
    protected List<ParticleSystem> MuzzleFlares;

    public enum FireMode
    {
        SemiAuto,
        FullAuto,
        Charge,
    }


    protected bool Firing = false;
    protected float FireCooldown = 0;
    protected LayerMask BulletHitMask;
    protected int ShotsFired = 0;

    protected virtual void Start()
    {
        foreach(Transform a in BulletSpawns)
            a.gameObject.SetActive(false);
        InitializeBullet();
        InitializeMuzzleFlare();
        //MyStatus = WeaponStatus.Normal;
    }

    public virtual void EquipWeapon()
    {
        SetLayerAndBullet(gameObject.layer);
    }


    protected virtual int GetNextBulletSpawn()
    {
        if (BulletSpawns.Count == 1)
            return 0;

        return GetNextBulletSpawn(true);
    }

    protected virtual int GetNextBulletSpawn(bool count)
    {
        int a = ShotsFired % BulletSpawns.Count;

        if (count)
            ShotsFired++;

        return a;

    }

    protected virtual void InitializeMuzzleFlare()
    {
        MuzzleFlares = new List<ParticleSystem>();

        foreach (Transform a in BulletSpawns)
        {
            GameObject NewMuzzleFlare = Instantiate(MuzzleFlarePrefab, a.position,MuzzleFlarePrefab.transform.rotation, a.transform.parent);
            MuzzleFlares.Add(NewMuzzleFlare.GetComponent<ParticleSystem>());
        }
    }

    protected virtual void InitializeBullet()
    {

        ProjectileScript = ProjectilePrefab.GetComponent<BaseBullet>();

        int SetLayer = 0;

        if (gameObject.layer == 9)
            SetLayer = 10;
        else if (gameObject.layer == 11)
            SetLayer = 12;

        ProjectileScript.SetLayerAndMask(SetLayer);
    }

    protected virtual void SetLayerAndBullet(int Layer)
    {
        gameObject.layer = Layer;

        if(ProjectileScript == null)
            ProjectileScript = ProjectilePrefab.GetComponent<BaseBullet>();

        int SetLayer = 0;

        if (gameObject.layer == 9)
            SetLayer = 10;
        else if (gameObject.layer == 11)
            SetLayer = 12;

        ProjectileScript.SetLayerAndMask(SetLayer);

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

    public virtual float GetProjectileSpeed()
    {
        return ProjectileScript.GetSpeed();
    }

    protected virtual void Fire1()
    {
        int SlotNum = GetNextBulletSpawn();
        GameObject NewBullet = GameObject.Instantiate(ProjectilePrefab, BulletSpawns[SlotNum].position, BulletSpawns[SlotNum].rotation);
        NewBullet.SetActive(true);
        NewBullet.transform.Rotate(new Vector3(Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), 0), Space.World);

        if (MuzzleFlarePrefab!=null)
            MuzzleFlares[SlotNum].Play();
    }

    public virtual float GetAmmoGauge()
    {
        return 1;
    }

    public virtual bool GetFirable()
    {
        return true;
    }

    public virtual string GetAmmoText()
    {
        return "";
    }


}
