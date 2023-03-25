using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearFlares : BaseEXGear
{
    [SerializeField]
    float DisruptionChance = 0;

    [SerializeField]
    int Charges = 5;

    [SerializeField]
    float ReloadTime =2;
    float ReloadRemaining = 0;

    [SerializeField]
    ParticleSystem Effect;

    private EnergySignal MyEnergySignal;

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyEnergySignal = Mech.GetEnergySignal();
    }

    protected override void Update()
    {
        base.Update();

        if (ReloadRemaining > 0)
            ReloadRemaining -= Time.deltaTime;
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        Debug.Log(Down);

        if (Down && ReloadRemaining <= 0 && Charges > 0)
        {
            FireFlares();
        }

    }

    protected virtual void FireFlares()
    {
        Effect.Play();
        MyEnergySignal.Distrupt(DisruptionChance,null);
        Charges--;
        ReloadRemaining = ReloadTime;
    }


    public override float GetReadyPercentage()
    {
        if (Charges <= 0)
            return 0;

        if (ReloadRemaining <= 0)
            return 1;
        else
            return 1 - ReloadRemaining / ReloadTime;
    }

    public override string GetBBMainText()
    {
        if (Charges <= 0)
            return "Empty";

        return Charges + " Chrges\nRemaining";
    }


}
