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

    [HideInInspector]
    [SerializeField] //for some reason this doessnt work unless serialized, F**K!
    protected int HitMask;

    [SerializeField]
    protected ParticleSystem HitEffect;
    [SerializeField]
    protected TrailRenderer MyTR;

    [SerializeField]
    protected BaseExplosion MyExplosion;

    [SerializeField]
    protected bool ExplodeOnTimerExpire = false;

    [HideInInspector]
    //[SerializeField]
    //protected BaseShoot BulletOrigin;
    [SerializeField]
    protected Object DamageSource;

    [SerializeField]
    protected float Lifetime;

    protected virtual void Start()
    {
        if(!ExplodeOnTimerExpire)
        Destroy(this.gameObject, Timer);
    }

    public virtual void InitBullet(BaseShoot Source)
    {
        int SetLayer = 0;

        if (Source.gameObject.layer == 9)
            SetLayer = 10;
        else if (Source.gameObject.layer == 11)
            SetLayer = 12;

        SetLayerAndMask(SetLayer);
        SetDamageSource();

        if (MyExplosion)
        {
            MyExplosion.SetLayerAndMask(SetLayer, HitMask);
            MyExplosion.SetDamageSource();
        }
        if(MyTR)
        MyTR.widthMultiplier = transform.localScale.x * 1.5f;
    }

    protected virtual void Update()
    {
        FlightCheck();

        Lifetime += Time.deltaTime;

        if(Timer <= Lifetime)
        {
            if (ExplodeOnTimerExpire && MyExplosion)
            {
                MyExplosion.transform.parent = null;
                MyExplosion.gameObject.SetActive(true);
            }

                Destroy(this.gameObject);
        }
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
        FlightCheck(1);
    }

    protected virtual void FlightCheck(float SpeedMod)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, SpeedMod * Speed * Time.deltaTime, ~HitMask))
        {
            transform.Translate(Vector3.forward * hit.distance);
            DealDamageTo(hit.collider.gameObject);
        }
        else
        {
            transform.Translate(Vector3.forward * SpeedMod * Speed * Time.deltaTime);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        DealDamageTo(collision.gameObject);
    }


    public virtual void SetLayerAndMask(int Layer)
    {
        gameObject.layer = Layer;
        SetMask();
    }

    public virtual void SetDamageSource()
    {
        DamageSource = (Object)GetComponentInParent<IDamageSource>();
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

            Temp.Hit(Damage, MyDamageType, MyDamageTags, (IDamageSource)DamageSource);
            //Debug.Log(Target.name + " Was hit by " + gameObject.name);
        }

        if (MyExplosion)
        {
            MyExplosion.transform.parent = null;
            MyExplosion.gameObject.SetActive(true);
        }

        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (HitEffect)
        {
            HitEffect.transform.parent = null;
            HitEffect.transform.localScale = new Vector3(1, 1, 1);
            HitEffect.gameObject.SetActive(true); //for some reason on impact explosives, the game obejct keeps getting set to false
            HitEffect.Play();
            //Debug.Log("HitEffect Play", HitEffect);
        }

    }

    public virtual string GetDamage
    {
        get {
            if(MyExplosion)
                return MyExplosion.GetDamage;
            return Damage + "";
        }
    }
}
