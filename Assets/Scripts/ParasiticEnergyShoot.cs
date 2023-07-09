using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiticEnergyShoot : BaseShoot
{
    [SerializeField]
    protected BaseEnergyShoot HostEShoot;
    [SerializeField]
    protected float ChargePerShot=0.01f;

    protected override void Fire1()
    {
        if (HostEShoot.GetFirable())
        {
            HostEShoot.ConsumeCharge(ChargePerShot);

            base.Fire1();
        }
        else
            MyBurstSettings.BurstRemaining = 0;
    }

    public override float GetAmmoGauge()
    {
        return HostEShoot.GetAmmoGauge();
    }

    public override string GetAmmoText()
    {
        return HostEShoot.GetAmmoText();
    }
}
