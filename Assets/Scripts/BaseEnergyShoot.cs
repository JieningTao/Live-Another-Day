using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnergyShoot : BaseShoot
{
    [SerializeField]
    protected float PercentageConsumedPerShot;
    [SerializeField]
    protected float ChargePerSecond;
    [SerializeField]
    protected float ChargeDelay;

    private float CurrentCapacitorPercentage;
    private float ChargeDelayRemaining;


    protected override void Start()
    {
        CurrentCapacitorPercentage = 1;
        base.Start();
    }

    protected override void Update()
    {
        if (ChargeDelayRemaining > 0)
            ChargeDelayRemaining -= Time.deltaTime;
        else
        {
            if (CurrentCapacitorPercentage < 1)
            {
                CurrentCapacitorPercentage += ChargePerSecond * Time.deltaTime;
                CurrentCapacitorPercentage = Mathf.Clamp(CurrentCapacitorPercentage, 0, 1);
            }
        }

        base.Update();
    }

    public virtual void Trigger(bool Fire)
    {

        if (Fire && MyFireMode == FireMode.SemiAuto && FireCooldown <= 0)
            Fire1();
        else
            Firing = Fire;
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
