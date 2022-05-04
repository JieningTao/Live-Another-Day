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

    protected void Start()
    {
        Destroy(this.gameObject, Timer);
    }

    protected virtual void Update()
    {
        FlightCheck();
    }

    public virtual void InitializeProjectile(float _Damage,int Layer, DamageSystem.DamageType DamageType, List<DamageSystem.DamageTag> DamageTags)
    {
        MyDamageType = DamageType;
        MyDamageTags = DamageTags;
        gameObject.layer = Layer;
        Damage = _Damage;

        SetMask();
    }

    protected virtual void FlightCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime, HitMask))
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

    protected void SetMask()
    {

        HitMask = 1 << (gameObject.layer-1);
        HitMask = ~HitMask;

        //Debug.Log(gameObject.name+ " Mask set "+HitMask);
    }

    protected virtual void DealDamageTo(GameObject Target)
    {

        IDamageable Temp = Target.GetComponentInParent<IDamageable>();

        if (Temp != null)
        {
            Temp.Hit(Damage);
            //Debug.Log(Target.name + " Was hit by " + gameObject.name);
        }

        Destroy(this.gameObject);
    }
}
