using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOPMechPart : LoadOutPart
{
    [SerializeField]
    public BaseMechPart MyPart;


    public override List<string> GetStats()
    {
        List<string> Temp  = new List<string>();

        Temp.Add("Health: ");
        Temp.Add(MyPart.GetHealth);

        Temp.Add("Weight: ");
        Temp.Add(MyPart.GetPartWeight);

        if (MyPart.GetBIEXG!=null)
        {
            Temp.Add("BIEXG: ");
            Temp.Add(MyPart.GetBIEXG);
        }

        if (MyPart.GetEXGSlots != null) 
        {
            Temp.Add("EXG Slots: ");
            Temp.Add(MyPart.GetEXGSlots);
        }

        return Temp;
    }
}
