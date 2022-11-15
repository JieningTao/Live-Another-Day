using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOPMechArms : LOPMechPart
{
    [SerializeField]
    public BaseMechPart MyOtherPart;

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Health: ");
        Temp.Add(MyPart.GetHealth);

        Temp.Add("Weight: ");
        Temp.Add(MyPart.GetPartWeight);


        Temp.Add("EXG Slots: ");

        string a = "";

        a += MyPart.GetEXGSlots;
        a += " | ";
        a += MyOtherPart.GetEXGSlots;

        Temp.Add(a);

        if (MyPart.GetBIEXG != null)
        {
            Temp.Add("Left BIEXG: ");
            Temp.Add(MyPart.GetBIEXG);
        }

        if (MyOtherPart.GetBIEXG != null)
        {
            Temp.Add("Right BIEXG: ");
            Temp.Add(MyOtherPart.GetBIEXG);
        }

        return Temp;
    }
}
