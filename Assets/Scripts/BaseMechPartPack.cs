using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechPartPack : BaseMechPart
{

    [SerializeField]
    BaseEXGear BackPackBuiltInEXG;



    public BaseEXGear GetBuilInEXG()
    {
        return BackPackBuiltInEXG;
    }
}
