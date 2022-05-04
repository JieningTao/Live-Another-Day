using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDamageable : MonoBehaviour
{

    [SerializeField]
    protected float Health;
    [SerializeField]
    protected List<MonoBehaviour> StuffToDisableAfterDestroy;
    [SerializeField]
    protected ParticleSystem DestroyEffect;
    [Tooltip("Negative value for don''t destroy")]
    [SerializeField]
    protected float DestroyDelay=0;


    protected bool IsDestroied = false;


    public virtual void Hit(float Damage)
    {
        Health -= Damage;
        if (Health <= 0 && !IsDestroied)
        {
            IsDestroied = true;
            Destroied();
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
}
