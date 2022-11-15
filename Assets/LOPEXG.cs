using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOPEXG : LoadOutPart
{


    [SerializeField]
    public BaseEXGear MyEXG;

    public override List<string> GetStats()
    {
        return MyEXG.GetStats();
    }
}
