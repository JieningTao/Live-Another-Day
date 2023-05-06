using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShield : BaseShield
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
    private float DeployedPowerDraw;
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

        if (ProjectionShield.gameObject.active&&DeployedPowerDraw>0)
        {
            
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
        if (Mech)
        {
            EnergySource = Mech.GetEnergySystem();
            gameObject.layer = Mech.gameObject.layer;

            if (gameObject.layer == 11) //bullet is in friendly projectile layer
                ProjectionShield.layer = LayerMask.NameToLayer("Friendly_Shields");
            else if (gameObject.layer == 9)
                ProjectionShield.layer = LayerMask.NameToLayer("Enemy_Shields");
        }

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

                if (DeployedPowerDraw > 0)
                    EnergySource.CurrentPowerDraw += DeployedPowerDraw;
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

                if (DeployedPowerDraw > 0)
                    EnergySource.CurrentPowerDraw -= DeployedPowerDraw;
            }
        }
    }

    public override void Hit(float Damage, DamageSystem.DamageType Type, List<DamageSystem.DamageTag> Tags, IDamageSource Source)
    {
        if (!IsDestroied)
        {
            float ActualDamage = Damage * DamageSystem.GetDamageMultiplier(MyArmorType, Type, Tags);
            CurrentHealth -= ActualDamage;

            base.PingDamageable(this, "Damage", ActualDamage, Source);

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Destroied();

                base.PingDamageable(this, "Destroied", 0, Source);
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


    #region InfoAccess

    public string GetRechargeRate
    { get { return RechargeRate+"/s"; } }

    public string GetRechargeDelay
    { get { return RechargeDelay + ""; } }

    public string GetChargeWhileDeployed
    { get {
            if(ChargeWhileDeployed)
            return "Y";
            return "N";
        } }

    public string GetOverLoadRecharge
    { get { return OverloadDelay + "s"; } }

    public string GetChargePowerDraw
    { get { return ChargePowerDraw + "/s"; } }

    public string GetDeployedPowerDraw
    { get { return DeployedPowerDraw + "/s"; } }

    #endregion

}
