using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeRailGun : BaseKineticShoot
{
    [SerializeField]
    float FullChargeTime = 2f;
    [SerializeField]
    Vector2 ProjectileSpeedChange;
    [SerializeField]
    Vector2 ProjectileDamageChange;
    [SerializeField]
    ParticleSystem FireEffect;
    [SerializeField]
    ParticleSystem ChargeEffect;

    protected float ChargePercent;
    protected bool Charging;



    protected override void Update()
    {
        base.Update();

        if (Charging)
        {
            if (ChargePercent < 1)
            {
                ChargePercent += Time.deltaTime / FullChargeTime;
                if (ChargePercent > 1)
                    ChargePercent = 1;
            }
        }

    }

    public override void Trigger(bool Fire)
    {
        if (Fire)
            Charging = true;
        else
        {
            Fire1();
            Charging = false;
        }


    }

    protected override void Fire1()
    {
        if (ChargePercent > 0.01 && Charging) // trigger false seem to be called on stopping play, can result in scene not cleaning up properly, stray bullet
        {
            if (MagazineRemaining > 0 && ReloadTimeRemaining <= 0)
            {
                MagazineRemaining--;
                Fire1(ChargePercent);
                ChargePercent = 0;

                if (MagazineRemaining <= 0)
                    Reload();
            }
            else
            {
                Reload();
            }
        }
    }

    protected virtual void Fire1(float Charge)
    {
        int SlotNum = GetNextBulletSpawn();

        ProjectileScript.SetSpeed(Mathf.Lerp(ProjectileSpeedChange.x, ProjectileSpeedChange.y, Charge));
        ProjectileScript.SetDamage(Mathf.Lerp(ProjectileDamageChange.x,ProjectileDamageChange.y, Charge));
        
        GameObject NewBullet = GameObject.Instantiate(ProjectilePrefab, BulletSpawns[SlotNum].position, BulletSpawns[SlotNum].rotation);
        NewBullet.SetActive(true);
        NewBullet.transform.Rotate(new Vector3(Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), 0), Space.Self);

        if (FireEffect)
        FireEffect.Play();
    }

    public override float GetAmmoGauge()
    {
        if (Charging)
            return ChargePercent;
        return base.GetAmmoGauge();
    }
}
