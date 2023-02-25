using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearPassiveAttributeProvider : BaseEXGear
{

    [SerializeField]
    public List<AttributeManager.BaseMechAttribute> MechAttributes = new List<AttributeManager.BaseMechAttribute>();
    [SerializeField]
    public List<AttributeManager.ExtraAttribute> EAttributes = new List<AttributeManager.ExtraAttribute>();

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);

        Mech.ApplyMechAttributes(MechAttributes);
        Mech.ApplyExtraAttributes(EAttributes);
    }
}
