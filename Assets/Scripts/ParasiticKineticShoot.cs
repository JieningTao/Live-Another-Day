using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiticKineticShoot : BaseShoot
{
    [SerializeField]
    protected BaseKineticShoot HostKShoot;


    protected override void Fire1()
    {
        if (HostKShoot.GetFirable())
        {
            HostKShoot.ConsumeMagazine(1);

            base.Fire1();
        }
        else
            MyBurstSettings.BurstRemaining = 0;
    }

    public override float GetAmmoGauge()
    {
        return HostKShoot.GetAmmoGauge();
    }

    public override string GetAmmoText()
    {
        return HostKShoot.GetAmmoText();
    }


}
