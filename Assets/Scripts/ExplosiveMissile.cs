using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMissile : BaseMissile
{


    //public virtual void InitializeProjectile(float _Damage, int Layer, DamageSystem.DamageType DamageType, List<DamageSystem.DamageTag> DamageTags, float _TrackingSpeed, float _ActivationDelay, float ExplosiveDamage, float ExplosiveForce, BaseExplosion _MyExplosion)
    //{
    //    base.InitializeProjectile(_Damage, Layer, DamageType, DamageTags, _TrackingSpeed, _ActivationDelay);

    //    if (_MyExplosion)
    //    {
    //        MyExplosion = _MyExplosion;
    //        MyExplosion.InitializeExplosion(ExplosiveDamage, DamageType, DamageTags, ExplosiveForce, HitMask);
    //    }

    //}

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
            Temp.Hit(Damage, MyDamageType, MyDamageTags);
            //Debug.Log(Target.name + " Was hit by " + gameObject.name);
        }

        if (MyExplosion)
        {
            MyExplosion.transform.parent = null;
            MyExplosion.gameObject.SetActive(true);
        }

        Destroy(this.gameObject);
    }

    public override string GetDamage
    { get { return MyExplosion.GetDamage; } }

}
