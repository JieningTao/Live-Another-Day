using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDamageable : MonoBehaviour
{


    [SerializeField]
    protected float MaxHealth = 100;
    [SerializeField]
    protected DamageSystem.ArmorType MyArmorType;
    [SerializeField]
    protected List<MonoBehaviour> StuffToDisableAfterDestroy;
    [SerializeField]
    protected ParticleSystem DestroyEffect;
    [Tooltip("Negative value for don''t destroy")]
    [SerializeField]
    protected float DestroyDelay=0;


    protected float CurrentHealth;
    protected bool IsDestroied = false;


    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public virtual void Hit(float Damage ,DamageSystem.DamageType Type, List<DamageSystem.DamageTag> Tags)
    {
        if (!IsDestroied)
        {
            CurrentHealth -= Damage * DamageSystem.GetDamageMultiplier(MyArmorType,Type,Tags);

            if (CurrentHealth <= 0 )
            {
                CurrentHealth = 0;
                IsDestroied = true;
                Destroied();
            }
        }
    }

    protected virtual void Destroied()
    {
        if (DestroyDelay != 0)
        {
            foreach (MonoBehaviour a in StuffToDisableAfterDestroy)
                a.enabled = false;

            DestroyEffect.Play();
        }

        if (DestroyDelay >= 0)
        {
            Destroy(this.gameObject, DestroyDelay);
        }

        
    }

    public string GetHealthText()
    {
        return (int)CurrentHealth + "";
    }

    public float GetHealthPercent()
    {
        return CurrentHealth / MaxHealth;
    }
}
