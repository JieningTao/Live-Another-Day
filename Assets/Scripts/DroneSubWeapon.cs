using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSubWeapon : IDamageable, IDamageSource
{
    [SerializeField] //SFT
    protected EnergySignal MyTarget;




    public virtual void GetDestroied()
    {
        base.Destroied();
    }

    public virtual void RecieveTarget(EnergySignal Target)
    {
        MyTarget = Target;
    }

    public virtual bool CanTargetPosition(Vector3 Pos)
    {
        return true;
    }

    public virtual bool CurrentlyTargeting()
    {
        if (MyTarget)
            return true;

        return false;
    }

    public virtual bool WeaponReady()
    {
        return true;
    }

    public IDamageSource DamageSource()
    {
        return this;
    }
}
