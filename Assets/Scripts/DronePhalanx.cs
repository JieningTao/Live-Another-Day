using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePhalanx : BaseDrone
{
    [Space(20)]
    [SerializeField]
    Transform AIBody;
    [SerializeField]
    float SelfTurnSpeed;
    [SerializeField]
    float SelfTurnSpeedShieldActive;
    [SerializeField]
    EnergyShield FrontShield;
    [SerializeField]
    float ShieldActivateRange;
    [SerializeField]
    float ShieldActiveSpeed;
    [SerializeField]
    float NormalSpeed;
    [SerializeField]
    UnityEngine.AI.NavMeshAgent MyNMA;

    bool FrontShieldOn;

    protected void Update()
    {
        //if (MTargetSignal)
            //HandleReposition();

        //AimWeapons();
        AimSelf();
    }

    private void MoveToTarget()
    {

    }

    private void AimSelf()
    {
        if (MTargetSignal)
        {
            AimWeapon(AIBody, MTargetSignal.transform.position - transform.position, new Vector3(0, 360, 0), GetCurrentTurnSpeed);
        }
        else
        {
            AimWeapon(AIBody, transform.forward, new Vector3(0, 360, 0), GetCurrentTurnSpeed);
        }
    }

    private float GetCurrentTurnSpeed
    {
        get
        {
            if (FrontShield.GetDeployed)
                return SelfTurnSpeedShieldActive;
            else
                return SelfTurnSpeed;

        }
    }

    private void ToggleShield(bool ShieldOn)
    {
        FrontShield.ToggleShield(ShieldOn);
        FrontShieldOn = ShieldOn;
        if (ShieldOn)
        {
            MyNMA.speed = ShieldActiveSpeed;
        }
        else
        {
            MyNMA.speed = NormalSpeed;
        }
    }

    private void HandleShieldEvent(IDamageable a,string b, float c, IDamageSource d )
    {
        if (a == FrontShield && b == "Destroied")
        {
            ToggleShield(false);
        }
    }

    private void OnEnable()
    {
        IDamageable.DamageablePing += HandleShieldEvent;
    }

    private void OnDisable()
    {
        IDamageable.DamageablePing -= HandleShieldEvent;
    }
}
