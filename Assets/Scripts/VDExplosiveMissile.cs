﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VDExplosiveMissile : VerticalDropMissile
{


    public override void SetLayerAndMask(int Layer)
    {
        base.SetLayerAndMask(Layer);
        MyExplosion.SetLayerAndMask(Layer, HitMask);
    }

    protected override void DealDamageTo(GameObject Target)
    {
        //Debug.Log("boom");

        IDamageable Temp = Target.GetComponent<IDamageable>();

        if (Temp != null)
        {
            Temp.Hit(Damage, MyDamageType, MyDamageTags, (IDamageSource)DamageSource);
            //Debug.Log(Target.name + " Was hit by " + gameObject.name);
        }

        if (MyExplosion)
        {
            MyExplosion.transform.parent = null;
            MyExplosion.gameObject.SetActive(true);
        }

        Destroy(this.gameObject);
    }
}
