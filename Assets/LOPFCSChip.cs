using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOPFCSChip : LoadOutPart
{
    [SerializeField]
    public BaseFCSChip MyChip;




    public override List<string> GetStats()
    {

        List<string> Temp = new List<string>();

        Temp.Add("Lock: ");
        Temp.Add(MyChip.GetPerLockCount+"/"+MyChip.GetLockTime+"s");

        Temp.Add("Max Lock: ");
        Temp.Add(MyChip.GetMaxLock+"");


        Temp.Add("LockRange: ");
        Temp.Add(MyChip.GetLockRange+"");

        Temp.Add("Radar Range: ");
        Temp.Add(MyChip.GetRadarRange + "");


        Temp.Add("Aim Angle: ");
        Temp.Add(MyChip.GetAimAngle+"");


        return Temp;

    }


}
