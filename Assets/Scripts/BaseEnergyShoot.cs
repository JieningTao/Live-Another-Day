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

    protected float CurrentCapacitorPercentage;
    protected float ChargeDelayRemaining;
    protected BaseEnergySource EnergySource;
    bool WasCharging = false;

    protected override void Start()
    {
        CurrentCapacitorPercentage = 1;

        BaseMechMain Temp = GetComponentInParent<BaseMechMain>();
        if(Temp)
        EnergySource = Temp.GetEnergySystem();
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

    public void GetPowerSource(BaseMechMain a)
    {
        if (a)
            EnergySource = a.GetEnergySystem();

        //Debug.Log(EnergySource);
    }

    public virtual void ConsumeCharge(float Amount)
    {
        CurrentCapacitorPercentage -= Amount;
        if (CurrentCapacitorPercentage < 0)
            CurrentCapacitorPercentage = 0;

        ChargeDelayRemaining = ChargeDelay;

        if (WasCharging)
            EnergySource.CurrentPowerDraw -= ChargePowerDraw;
        WasCharging = false;
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

    public override bool GetFirable()
    {
        return CurrentCapacitorPercentage >= PercentageConsumedPerShot;
    }

    public override void Trigger(bool Fire)
    {
        base.Trigger(Fire);
    }

    protected override void Fire1()
    {
        if (CurrentCapacitorPercentage >= PercentageConsumedPerShot)
        {
            CurrentCapacitorPercentage -= PercentageConsumedPerShot;
            base.Fire1();
            ChargeDelayRemaining = ChargeDelay;

            if (WasCharging)
                EnergySource.CurrentPowerDraw -= ChargePowerDraw;
            WasCharging = false;
        }
        else
        {
            //if not enough energy to fire, clears remaining burst
            MyBurstSettings.BurstRemaining = 0;
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

    public override bool LowEnergyWarning()
    {
        if (CurrentCapacitorPercentage < 0.2)
            return true;
        return false;
    }

    public override string GetMag
    { get { return PercentageConsumedPerShot*100+"%/shot"; } }

    public override string GetReload
    { get { return ChargePerSecond*100+"%/s"; } }
}
