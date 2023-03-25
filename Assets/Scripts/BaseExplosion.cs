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
    protected float ExplosionRadius;
    [SerializeField]
    float Delay;
    [SerializeField]
    float ExplosiveForce;
    [SerializeField]
    protected float DestroyTimer;
    [SerializeField]
    int HitMask;


    private float ScaledExplosionRadius
    {
        get
        {
            return ExplosionRadius * transform.localScale.x;
        }
    }

    // remenant system where explosions are instantiated at play and requires all info about themseles
    //public virtual void InitializeExplosion(float Damage, DamageSystem.DamageType DT, List<DamageSystem.DamageTag> Tags , float Force,int _HitMask )
    //{
    //    ExplosiveDamage = Damage;
    //    MyDamageType = DT;
    //    MyDamageTags = Tags;
    //    ExplosiveForce = Force;
    //    HitMask = _HitMask;
    //}

    public virtual void SetLayerAndMask(int Layer,int Mask)
    {
        gameObject.layer = Layer;
        HitMask = Mask;
    }

    protected void SetMask()
    {
        if (gameObject.layer == 12) //bullet is in friendly projectile layer
            HitMask = LayerMask.GetMask("Friendly", "Friendly_Shields");
        else if (gameObject.layer == 10)
            HitMask = LayerMask.GetMask("Enemy", "Enemy_Shields");

        if (MyDamageType != DamageSystem.DamageType.Energy)
            HitMask = HitMask | (1 << 15);
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

        Collider[] objects = UnityEngine.Physics.OverlapSphere(transform.position, ScaledExplosionRadius,~HitMask);
        List < IDamageable > HitObjects = new List<IDamageable>();
        foreach (Collider h in objects)
        {
            IDamageable D = h.GetComponentInParent<IDamageable>();
            if (D != null)
            {
                if (!HitObjects.Contains(D))
                    HitObjects.Add(D);
            }

            Rigidbody r = h.GetComponent<Rigidbody>();
            if (r != null)
                r.AddExplosionForce(ExplosiveForce, transform.position, ScaledExplosionRadius);
        }

        foreach (IDamageable D in HitObjects)
        {
            if(LOSCheck(D))
            D.Hit(ExplosiveDamage, MyDamageType, MyDamageTags);
        }
    }

    private bool LOSCheck(IDamageable a)
    {
        //checking LOS makes sure shields defend against AOE explosions
        RaycastHit Hit;
        if (Physics.Raycast(transform.position,   a.transform.position - transform.position, out Hit))
        {
            //Debug.Log(Hit.collider,Hit.collider);
            if (Hit.collider.GetComponentInParent<IDamageable>() == a)
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, ScaledExplosionRadius);
    }

    public virtual string GetDamage
    { get { return "EXP-"+ExplosiveDamage; } }
}
