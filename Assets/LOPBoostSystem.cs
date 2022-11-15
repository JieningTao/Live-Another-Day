using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOPBoostSystem : LoadOutPart
{
    [SerializeField]
    public BaseBoostSystem MyBS;




    public override List<string> GetStats()
    {

        List<string> Temp = new List<string>();

        Temp.Add("Capacity: ");
        Temp.Add(MyBS.GetBoostCapacity);

        Temp.Add("Regen: ");
        Temp.Add(MyBS.GetRecovery);

        Temp.Add("RegenCD: ");
        Temp.Add(MyBS.GetRecoveryCD);


        Temp.Add("Boost: ");
        Temp.Add(MyBS.GetBoostForce+" at "+MyBS.GetBoostCost);

        Temp.Add("Impulse: ");
        Temp.Add(MyBS.GetImpulseForce + " at " + MyBS.GetImpulseCost);

        Temp.Add("Float: ");
        Temp.Add(MyBS.GetFloatForce + " at " + MyBS.GetFloatCost);


        return Temp;

    }

}
