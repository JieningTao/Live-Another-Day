using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearHealthPack : BaseEXGear
{
    [SerializeField]
    float HealAmount =100;
    [SerializeField]
    float HealTime =1;
    [SerializeField]
    int MaxCharge =6;
    int CurrentCharge;

    float HealPerSecond { get { return HealAmount / HealTime; } }

    float HealLeft;


    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        CurrentCharge = MaxCharge;
    }


    protected override void Update()
    {
        if (HealLeft > 0)
        {
            float Temp = HealPerSecond * Time.deltaTime;
            if (HealLeft > Temp)
            {
                HealLeft -= Temp;
                if (!MyMech.HealthFull())
                    MyMech.Heal(Temp);
            }
            else
            {
                if (!MyMech.HealthFull())
                    MyMech.Heal(HealLeft);
                HealLeft = 0;
            }
        }
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        if (Down)
        {
            if (CurrentCharge > 0&&HealLeft <=0)
            {
                CurrentCharge--;

                if (HealTime > 0)
                    HealLeft = HealAmount;
                else
                {
                    if (!MyMech.HealthFull())
                        MyMech.Heal(HealAmount);
                }
            }
        }

    }

    public override float GetReadyPercentage()
    {
        return (float)CurrentCharge/(float)MaxCharge;
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Repair: ");
        Temp.Add(HealAmount+"");

        Temp.Add("Time: ");
        Temp.Add(HealTime+"s");

        Temp.Add("Charges: ");
        Temp.Add(MaxCharge+"");

        return Temp;
    }

}
