using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseExplosion : MonoBehaviour
{
    [SerializeField]
    private float ExplosiveDamage;
    [SerializeField]
    public DamageSystem.DamageType MyDamageType;
    [SerializeField]
    public List<DamageSystem.DamageTag> MyDamageTags;
    [SerializeField]
    float ExplosionRadius;
    [SerializeField]
    float Delay;
    [SerializeField]
    float ExplosiveForce;
    [SerializeField]
    float DestroyTimer;
    [SerializeField]
    int HitMask;

    private float ScaledExplosionRadius
    {
        get
        {
            return ExplosionRadius * transform.localScale.x;
        }
    }

    public virtual void InitializeExplosion(float Damage, DamageSystem.DamageType DT, List<DamageSystem.DamageTag> Tags , float Force,int _HitMask )
    {
        ExplosiveDamage = Damage;
        MyDamageType = DT;
        MyDamageTags = Tags;
        ExplosiveForce = Force;
        HitMask = _HitMask;
    }

    public virtual void SetLayerAndMask(int Layer)
    {
        gameObject.layer = Layer;
        SetMask();
    }

    protected void SetMask()
    {
        HitMask = 1 << (gameObject.layer - 1);
        HitMask = ~HitMask;
    }

    private void Update()
    {
        Delay -= Time.deltaTime;

        if (Delay <= 0)
        {
            Explode();
            this.enabled = false;
            Destroy(gameObject, DestroyTimer);
        }

        
    }

    private void Explode()
    {
        ///Debug.Log("Boom");

        Collider[] objects = UnityEngine.Physics.OverlapSphere(transform.position, ScaledExplosionRadius,HitMask);
        foreach (Collider h in objects)
        {
            IDamageable D = h.GetComponentInParent<IDamageable>();
            if (D != null)
                D.Hit(ExplosiveDamage, MyDamageType, MyDamageTags);

            Rigidbody r = h.GetComponent<Rigidbody>();
            if (r != null)
                r.AddExplosionForce(ExplosiveForce, transform.position, ScaledExplosionRadius);

            //Debug.Log(h.gameObject.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, ScaledExplosionRadius);
    }
}
