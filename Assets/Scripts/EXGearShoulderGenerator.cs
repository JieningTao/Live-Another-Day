using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearShoulderGenerator : EXGearShoulder
{
    [SerializeField]
    float MaxFuel;
    float CurrentFuel;
    [SerializeField]
    float FuelConsumedPerSecond;
    [SerializeField]
    float EnergyGenerationPerSecond;

    bool GeneratorOn;






    protected override void Update()
    {
        base.Update();

        if (GeneratorOn)
        {
            CurrentFuel -= FuelConsumedPerSecond * Time.deltaTime;

            if (CurrentFuel <= 0)
                GeneratorTurn(false);
        }
    }


    private void GeneratorTurn(bool On)
    {
        if (On&&!GeneratorOn)
        {
            GeneratorOn = true;
            MechEnergySystem.CurrentPowerDraw -= EnergyGenerationPerSecond;
        }
        else if(!On && GeneratorOn)
        {
            GeneratorOn = false;
            MechEnergySystem.CurrentPowerDraw += EnergyGenerationPerSecond;
        }
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        if (GeneratorOn)
            GeneratorTurn(false);
        else
        {
            if (CurrentFuel > 0)
                GeneratorTurn(true);
        }
    }

    public override float GetReadyPercentage()
    {
        return CurrentFuel/MaxFuel;
    }
}
