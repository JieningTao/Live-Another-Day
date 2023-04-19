using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearGenerator : BaseEXGear
{
    [SerializeField]
    float MaxFuel;
    float CurrentFuel;
    [SerializeField]
    float FuelConsumedPerSecond;
    [SerializeField]
    float EnergyGenerationPerSecond;

    bool GeneratorOn;




    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        CurrentFuel = MaxFuel;
    }

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

        if (Down) // check to see it only activates on trigger doen dumbass! it's not a hold ability
        {
            if (GeneratorOn)
                GeneratorTurn(false);
            else
            {
                if (CurrentFuel > 0)
                    GeneratorTurn(true);
            }
        }


    }

    public override float GetReadyPercentage()
    {
        return (float)(CurrentFuel/MaxFuel);
    }

    public override string GetBBMainText()
    {
        if (GeneratorOn)
            return "Active\n" + EnergyGenerationPerSecond + "EU/s";
        else
            return "Standby";
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Output: ");
        Temp.Add(EnergyGenerationPerSecond+"/s");

        Temp.Add("BurnTime: ");
        Temp.Add((MaxFuel/FuelConsumedPerSecond).ToString("F2")+"s");


        return Temp;
    }
}
