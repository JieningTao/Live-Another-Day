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


    //[SerializeField]
    protected float CurrentHealth;
    protected bool IsDestroied = false;


    protected virtual void Start()
    {
        InitializeIDamageable();
    }

    protected virtual void InitializeIDamageable()
    {
        CurrentHealth = MaxHealth;
        GetStuffToDisable();
        if(DestroyEffect)
            DestroyEffect.gameObject.SetActive(false);
        
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
            {
            if(a)
                a.enabled = false;
            }

            if (DestroyEffect)
            {
                DestroyEffect.gameObject.SetActive(true);
                DestroyEffect.Play();
            }
        }
        else
        {
            if (DestroyEffect)
            {
                DestroyEffect.transform.parent = null;
                DestroyEffect.transform.localScale = new Vector3(1, 1, 1);
                DestroyEffect.gameObject.SetActive(true);
                DestroyEffect.Play();
            }
        }

        if (DestroyDelay >= 0)
        {
            Destroy(this.gameObject, DestroyDelay);
        }


        
    }

    public virtual void GetStuffToDisable()
    {
        StuffToDisableAfterDestroy.AddRange(GetComponentsInChildren<MonoBehaviour>());

        //StuffToDisableAfterDestroy.AddRange(GetComponentsInChildren<BaseShoot>());
        //StuffToDisableAfterDestroy.AddRange(GetComponentsInChildren<BaseMechMovement>());
        //StuffToDisableAfterDestroy.AddRange(GetComponentsInChildren<BaseMechFCS>());

    }

    public void Heal(float Amount)
    {
        CurrentHealth += Amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public bool HealthFull()
    {
        if (CurrentHealth >= MaxHealth)
            return true;
        return false;
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
