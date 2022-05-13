using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechMain : ICoatedDamagable
{
    [SerializeField]
    protected BaseMechMovement MyMovement;
    [SerializeField]
    protected BaseMechFCS MyFCS;

    [Space(20)]

    [SerializeField]
    protected float MaxEnergy;
    [SerializeField]
    protected float NaturalEnergyRegen;


    protected float CurrentEnergy;
    public float CurrentPowerDraw;

    public float CurrentOutputEffiency;//{ get; protected set; }


    protected override void Start()
    {
        
        base.Start();
        CurrentEnergy = MaxEnergy;
    }
    private void Update()
    {
        UpdateEnergy();
    }
    private void UpdateEnergy()
    {
        float EnergyChange = NaturalEnergyRegen - CurrentPowerDraw;

        if (!((EnergyChange > 0 && CurrentEnergy == MaxEnergy) || (EnergyChange < 0 && CurrentEnergy == 0)))
        {
            CurrentEnergy += EnergyChange*Time.deltaTime;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
        }

        if (EnergyChange >= 0)
            CurrentOutputEffiency = 1;
        else
            CurrentOutputEffiency = GetOutputEfficiency();

    }

    private float GetOutputEfficiency()
    {
        if (CurrentPowerDraw <= NaturalEnergyRegen||CurrentEnergy>0)
            return 1;
        else
        {
            return NaturalEnergyRegen / CurrentPowerDraw;
        }
    }

    public string GetEnergyText()
    {
        return CurrentEnergy.ToString("F2");
    }

    public float GetEnergyPercent()
    {
        return CurrentEnergy / MaxEnergy;
    }


}
