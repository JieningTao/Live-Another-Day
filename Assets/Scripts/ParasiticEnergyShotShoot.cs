using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiticEnergyShotShoot : ParasiticEnergyShoot
{
    [SerializeField]
    protected int ShotAmount;

    protected override void Fire1()
    {

        if (HostEShoot.GetFirable())
        {
            HostEShoot.ConsumeCharge(ChargePerShot);

            int SlotNum = GetNextBulletSpawn();


            for (int i = 0; i < ShotAmount; i++)
            {
                GameObject NewBullet = GameObject.Instantiate(ProjectilePrefab, BulletSpawns[SlotNum].position, BulletSpawns[SlotNum].rotation);
                NewBullet.SetActive(true);
                NewBullet.transform.Rotate(new Vector3(Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), 0), Space.Self);
            }

            if (ShotSounds.Count > 0)
            {
                PlayShotSound(SlotNum);
            }

            FireCooldown = TBS;
        }
        else
            MyBurstSettings.BurstRemaining = 0;

    }

}
