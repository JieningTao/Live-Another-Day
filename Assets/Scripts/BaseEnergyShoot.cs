using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnergyShoot : BaseShoot
{
    [Tooltip("remember, percentage, 0.01 = 1%")]
    [SerializeField]
    protected float PercentageConsumedPerShot;
    [Tooltip("same thing, percentage")]
    [SerializeField]
    protected float ChargePerSecond;
    [SerializeField]
    protected float ChargeDelay;
    [SerializeField]
    private float ChargePowerDraw;

    private float CurrentCapacitorPercentage;
    private float ChargeDelayRemaining;
    protected BaseMechMain EnergySource;
    bool WasCharging = false;

    protected override void Start()
    {
        CurrentCapacitorPercentage = 1;
        EnergySource = GetComponentInParent<BaseMechMain>();
        base.Start();
    }

    protected override void Update()
    {
        if (ChargeDelayRemaining > 0)
            ChargeDelayRemaining -= Time.deltaTime;
        else
            Recharge();

        base.Update();
    }

    protected virtual void Recharge()
    {
        if (CurrentCapacitorPercentage < 1)
        {
            CurrentCapacitorPercentage += ChargePerSecond * Time.deltaTime*EnergySource.CurrentOutputEffiency;
            CurrentCapacitorPercentage = Mathf.Clamp(CurrentCapacitorPercentage, 0, 1);
            if (!WasCharging)
                EnergySource.CurrentPowerDraw += ChargePowerDraw;
            WasCharging = true;
        }
        else
        {
            if (WasCharging)
                EnergySource.CurrentPowerDraw -= ChargePowerDraw;
            WasCharging = false;
        }

    }

    public override void Trigger(bool Fire)
    {
        base.Trigger(Fire);
    }

    protected override void Fire1()
    {
        if (CurrentCapacitorPercentage > PercentageConsumedPerShot)
        {
            CurrentCapacitorPercentage -= PercentageConsumedPerShot;
            base.Fire1();
            ChargeDelayRemaining = ChargeDelay;
        }
    }

    public override float GetAmmoGauge()
    {
        return CurrentCapacitorPercentage;
    }

    public override string GetAmmoText()
    {
        return (CurrentCapacitorPercentage * 100).ToString("F2") + "%";
    }
}
