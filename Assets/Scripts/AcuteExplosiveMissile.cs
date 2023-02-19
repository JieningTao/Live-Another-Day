using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcuteExplosiveMissile : AcuteMissile
{
    [SerializeField]
    protected BaseExplosion MyExplosion;

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
