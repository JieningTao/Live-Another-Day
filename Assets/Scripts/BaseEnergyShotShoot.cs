using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnergyShotShoot : BaseEnergyShoot
{
    [SerializeField]
    protected int ShotAmount;

    protected override void Fire1()
    {
        if (CurrentCapacitorPercentage > PercentageConsumedPerShot)
        {
            CurrentCapacitorPercentage -= PercentageConsumedPerShot;

            int SlotNum = GetNextBulletSpawn();

            for (int i = 0; i < ShotAmount; i++)
            {
                GameObject NewBullet = GameObject.Instantiate(ProjectilePrefab, BulletSpawns[SlotNum].position, BulletSpawns[SlotNum].rotation);
                NewBullet.SetActive(true);
                NewBullet.transform.Rotate(new Vector3(Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), 0), Space.Self);
            }

            ChargeDelayRemaining = ChargeDelay;

        }
    }

    public override string GetDamage
    { get { return ShotAmount + " * " + ProjectileScript.GetDamage; } }
}
