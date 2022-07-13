using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICoatedDamagable : IDamageable
{
    [SerializeField]
    DamageSystem.DefensiveCoating MyCoatingType;

    protected float CurrentCoatingLeft;
    protected float MaxCoating;


    protected override void InitializeIDamageable()
    {
        base.InitializeIDamageable();
        MaxCoating = MaxHealth * 0.2f;
        CurrentCoatingLeft = MaxCoating;
    }

    public override void Hit(float Damage, DamageSystem.DamageType Type, List<DamageSystem.DamageTag> Tags)
    {
        if (CurrentCoatingLeft > 0)
        {
            float AbsorbedDamage = Damage * DamageSystem.GetCoatingAbsorbRate(MyCoatingType, Type, Tags);

            CurrentCoatingLeft -= AbsorbedDamage;

            Damage -= AbsorbedDamage;
            CurrentCoatingLeft = Mathf.Clamp(CurrentCoatingLeft, 0, MaxCoating);
        }

        base.Hit(Damage, Type, Tags);
    }




    public string GetCoatingText()
    {
        if (MyCoatingType == DamageSystem.DefensiveCoating.NoCoating)
            return "";
        return (int)CurrentCoatingLeft + "";
    }

    public float GetCoatingPercent()
    {
        if (MyCoatingType == DamageSystem.DefensiveCoating.NoCoating)
            return 0;
        return CurrentCoatingLeft / MaxCoating;
    }
}
