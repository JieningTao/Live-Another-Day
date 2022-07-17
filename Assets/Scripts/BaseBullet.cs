using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField]
    protected float Speed = 200;

    [SerializeField]
    protected float Timer = 2f;

    [SerializeField]
    protected float Damage = 10;

    [SerializeField]
    protected DamageSystem.DamageType MyDamageType;

    [SerializeField]
    protected List<DamageSystem.DamageTag> MyDamageTags;

     
    [SerializeField] //for some reason this doessnt work unless serialized, F**K!
    protected int HitMask;

    [SerializeField]
    protected ParticleSystem HitEffect;

    protected void Start()
    {
        Destroy(this.gameObject, Timer);
    }

    protected virtual void Update()
    {
        FlightCheck();
    }

    //public virtual void InitializeProjectile(float _Damage,int Layer, DamageSystem.DamageType DamageType, List<DamageSystem.DamageTag> DamageTags)
    //{
    //    MyDamageType = DamageType;
    //    MyDamageTags = DamageTags;
    //    gameObject.layer = Layer;
    //    Damage = _Damage;

    //    SetMask();
    //}

    protected virtual void FlightCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime, ~HitMask))
        {
            transform.Translate(Vector3.forward * hit.distance);
            DealDamageTo(hit.collider.gameObject);
        }
        else
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        DealDamageTo(collision.gameObject);
    }

    public virtual void SetLayerAndMask(int Layer)
    {
        //Debug.Log("Ping");
        gameObject.layer = Layer;
        SetMask();
    }

    public virtual float GetSpeed()
    {
        return Speed;
    }

    public virtual void SetSpeed(float _Speed)
    {
        Speed = _Speed;
    }

    public virtual void SetDamage(float _Damage)
    {
        Damage = _Damage;
    }

    protected void SetMask()
    {
        if (gameObject.layer == 12) //bullet is in friendly projectile layer
            HitMask = LayerMask.GetMask("Friendly", "Friendly_Shields");
        else if (gameObject.layer == 10)
            HitMask = LayerMask.GetMask("Enemy", "Enemy_Shields");

        if (MyDamageType != DamageSystem.DamageType.Energy)
            HitMask = HitMask | (1 << 15);

        //Debug.Log(gameObject.name+ " Mask set "+HitMask);
    }

    public void AddLayerToMask(int Layer)
    {
        HitMask = HitMask | ~(1 << Layer);
    }

    protected virtual void DealDamageTo(GameObject Target)
    {

        IDamageable Temp = Target.GetComponentInParent<IDamageable>();

        if (Temp != null)
        {
            Temp.Hit(Damage, MyDamageType, MyDamageTags);
            //Debug.Log(Target.name + " Was hit by " + gameObject.name);
        }

        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (HitEffect)
        {
            HitEffect.transform.parent = null;
            HitEffect.transform.localScale = new Vector3(1, 1, 1);
            HitEffect.Play();
        }

    }
}
