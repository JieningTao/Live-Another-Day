using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalShield : BaseShield
{


    public void Equip(BaseMechMain Mech)
    {
        gameObject.layer = Mech.gameObject.layer;
    }

    protected override void Destroied()
    {
        base.Destroied();
    }
}
