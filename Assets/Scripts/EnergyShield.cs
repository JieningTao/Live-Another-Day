using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShield : IDamageable
{
    [SerializeField]
    GameObject ProjectionShield;
    [SerializeField]
    float RechargeDelay;
    [SerializeField]
    float RechargeRate;
    [SerializeField]
    bool ChargeWhileDeployed;
    [SerializeField]
    private float ChargePowerDraw;
    [SerializeField]
    float OverloadDelay;

    float DelayRemaining;
    BaseEnergySource EnergySource;
    bool WasCharging;

    bool Charging;

    private void Update()
    {
        if (Charging)
        {
            Recharge();
        }

        CheckCharge();
    }




    private void StartCharging(bool a)
    {
        if (a && !Charging)
        {
            EnergySource.CurrentPowerDraw += ChargePowerDraw;
            Charging = true;
        }
        else if (!a && Charging)
        {
            EnergySource.CurrentPowerDraw -= ChargePowerDraw;
            Charging = false;
        }
    }

    protected virtual void CheckCharge()
    {
        if ((ChargeWhileDeployed && !ProjectionShield.active) || !ChargeWhileDeployed)
        {
            if (DelayRemaining > 0)
                DelayRemaining -= Time.deltaTime;
            else
            {
                if (!HealthFull()&&!Charging)
                {
                    StartCharging(true);
                }
            }
        }
    }





    protected virtual void Recharge()
    {
        if (!HealthFull())
            Heal(RechargeRate * Time.deltaTime * EnergySource.CurrentOutputEffiency);
        else
            StartCharging(false);
    }

    public void Equip(BaseMechMain Mech)
    {
        EnergySource = Mech.GetEnergySystem();
        gameObject.layer = Mech.gameObject.layer;

        if (gameObject.layer == 11) //bullet is in friendly projectile layer
            ProjectionShield.layer = LayerMask.NameToLayer("Friendly_Shields");
        else if (gameObject.layer == 9)
            ProjectionShield.layer = LayerMask.NameToLayer("Enemy_Shields");
    }

    public void ToggleShield(bool On)
    {
        if (On)
        {
            if (!ProjectionShield.active && CurrentHealth > 0)
            {
                ProjectionShield.SetActive(true);

                if (ChargeWhileDeployed)
                    StartCharging(false);
            }
        }
        else
        {
            if (ProjectionShield.active)
            {
                ProjectionShield.SetActive(false);
                if (ChargeWhileDeployed)
                    if (DelayRemaining < RechargeDelay)
                        DelayRemaining = RechargeDelay;
            }
        }
    }

    public override void Hit(float Damage, DamageSystem.DamageType Type, List<DamageSystem.DamageTag> Tags)
    {
        if (!IsDestroied)
        {
            CurrentHealth -= Damage * DamageSystem.GetDamageMultiplier(MyArmorType, Type, Tags);

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Destroied();
            }
        }

        if (WasCharging)
            EnergySource.CurrentPowerDraw -= ChargePowerDraw;
        WasCharging = false;

        DelayRemaining = RechargeDelay;
    }

    protected override void Destroied()
    {
        ToggleShield(false);
       DelayRemaining = OverloadDelay;
        //base.Destroied();
    }



}
