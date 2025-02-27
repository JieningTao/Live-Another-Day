﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalShield : BaseShield
{


    public void Equip(BaseMechMain Mech)
    {
        if (Mech)
            gameObject.layer = Mech.gameObject.layer;
        else
            return;

        List<Collider> AllColliders = new List<Collider>();

        AllColliders.AddRange(GetComponentsInChildren<Collider>());
        foreach (Collider a in AllColliders)
        {
            a.gameObject.layer = gameObject.layer;
        }
    }

    protected override void Destroied()
    {
        base.Destroied();
    }
}
