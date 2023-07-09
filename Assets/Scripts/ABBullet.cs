using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABBullet : BaseBullet
{
    [Space(30)]
    [SerializeField]
    BaseBullet FragmentPrefab;
    [SerializeField]
    float SpreadAngle = 30;
    [SerializeField]
    int BurstAmount = 10;


    protected override void Start()
    {
    }

    protected override void Update()
    {
        FlightCheck();

        Lifetime += Time.deltaTime;

        if(Timer <= Lifetime)
        Burst();
    }

    public override void InitBullet(BaseShoot Source)
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

        if (FragmentPrefab)
            FragmentPrefab.InitBullet(Source);

    }

    public override void SetDamageSource()
    {
        base.SetDamageSource();

        if (FragmentPrefab)
            FragmentPrefab.SetDamageSource();
    }

    private void Burst()
    {
        for (int i = 0; i < BurstAmount; i++)
        {
            GameObject NewProjectile = Instantiate(FragmentPrefab.gameObject, transform.position, transform.rotation);
            NewProjectile.transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Random.Range(-SpreadAngle / 2, SpreadAngle / 2));
            NewProjectile.SetActive(true);
        }
        if (HitEffect)
        {
            HitEffect.transform.parent = null;
            HitEffect.Play();
        }
        Destroy(gameObject);

    }


}
