using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseKineticShotShoot : BaseKineticShoot
{
    [SerializeField]
    protected int ShotAmount;





    protected override void Fire1()
    {

        if (MagazineRemaining > 0 && ReloadTimeRemaining <= 0)
        {
            MagazineRemaining--;

            int SlotNum = GetNextBulletSpawn();

            for (int i = 0; i < ShotAmount; i++)
            {
                GameObject NewBullet = GameObject.Instantiate(ProjectilePrefab, BulletSpawns[SlotNum].position, BulletSpawns[SlotNum].rotation);
                NewBullet.SetActive(true);
                NewBullet.transform.Rotate(new Vector3(Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), Random.Range(-AccuracyDeviation / 2, AccuracyDeviation / 2), 0), Space.World);
            }

            if (MuzzleFlarePrefab != null)
                MuzzleFlares[SlotNum].Play();

            if (MagazineRemaining <= 0)
                Reload();
        }
        else
        {
            Reload();
        }
        
    }


}
