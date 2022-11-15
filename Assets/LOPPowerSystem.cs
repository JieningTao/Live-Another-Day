using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOPPowerSystem : LoadOutPart
{
    [SerializeField]
    public BasePowerSystem MyPS;




    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Capacity: ");
        Temp.Add(MyPS.GetCapacity);

        Temp.Add("Regen: ");
        Temp.Add(MyPS.GetRegen);

        return Temp;
    }
}
