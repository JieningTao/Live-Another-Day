using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VDAirBurstMissile : VerticalDropMissile
{
    [SerializeField]
    ParticleSystem BurstEffect;
    [SerializeField]
    float BlastDistance = 10;
    [SerializeField]
    GameObject FragmentPrefab;
    [SerializeField]
    float SpreadAngle = 30;
    [SerializeField]
    int FragAmount = 20;

    protected override void Update()
    {
        if (Vector3.Distance(Target.transform.position, transform.position) < BlastDistance && Vector3.Angle(transform.forward, Target.transform.position - transform.position) < SpreadAngle / 2)
            Burst();

        base.Update();
    }

    public override void SetLayerAndMask(int Layer)
    {
        base.SetLayerAndMask(Layer);
        FragmentPrefab.GetComponent<BaseBullet>().SetLayerAndMask(Layer);
    }

    private void Burst()
    {
        for (int i = 0; i < FragAmount; i++)
        {
            GameObject NewProjectile = Instantiate(FragmentPrefab, transform.position, transform.rotation);
            NewProjectile.transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Random.Range(-SpreadAngle / 2, SpreadAngle / 2));
            NewProjectile.SetActive(true);
        }
        if (BurstEffect)
        {
            BurstEffect.transform.parent = null;
            BurstEffect.Play();
        }
        Destroy(gameObject);
    }
}
