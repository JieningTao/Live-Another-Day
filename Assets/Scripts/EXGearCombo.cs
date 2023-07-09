using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearCombo: BaseEXGear
{
    [SerializeField]
    protected BaseEXGear TapGear;
    [SerializeField]
    protected BaseEXGear HoldGear;
    [SerializeField]
    protected float HoldTime = 0.18f;
    protected float TimeHeld;
    protected bool Held;


    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);

        TapGear.InitializeGear(Mech, transform, Right);
        HoldGear.InitializeGear(Mech, transform, Right);
    }

    protected override void Update()
    {
        if (Held)
        {
            if (TimeHeld < HoldTime && TimeHeld + Time.deltaTime > HoldTime) // if this is the frame that holding down trigger will cross to hold control time
            {
                HoldGear.TriggerGear(true);
            }
            TimeHeld += Time.deltaTime;
        }
    }

    public override void Equip(bool a)
    {
        base.Equip(a);

        TapGear.Equip(a);
        HoldGear.Equip(a);
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        if (Down)
            Held = true;
        else
        {
            if (TimeHeld < HoldTime)
                TapGear.TriggerGear(true);
            else
                HoldGear.TriggerGear(false);

            Held = false;
            TimeHeld = 0;

        }
    }

    public override float GetReadyPercentage()
    {
        return TapGear.GetReadyPercentage();
    }

    public override float GetSubReadyPercentage()
    {
        return HoldGear.GetReadyPercentage();
    }

    public override string GetBBMainText()
    {
        return TapGear.GetBBMainText();
    }

    public override string GetBBSubText()
    {
        return HoldGear.GetBBMainText();
    }

    public override void ReSupply(float Percentage)
    {
        TapGear.ReSupply(Percentage);
        HoldGear.ReSupply(Percentage);
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();
        Temp.AddRange(TapGear.GetStats());
        Temp.AddRange(HoldGear.GetStats());

        return  Temp;
    }

    public override bool IsAimed
    {
        get { return TapGear.IsAimed || HoldGear.IsAimed; }
    }
}
