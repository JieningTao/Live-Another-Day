using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyAlignShotShoot : BaseEnergyShotShoot
{
    protected override void Fire1()
    {
        (ProjectileScript as AlignBullet).RecieveAlignmentDirection(BulletSpawns[0].forward);
        base.Fire1();
    }
}
