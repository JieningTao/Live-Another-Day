using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOPMainGear : LoadOutPart
{
    [SerializeField]
    public BaseMainSlotEquipment MyEquipment;

    public override List<string> GetStats()
    {
        return MyEquipment.GetStats();
    }
}
