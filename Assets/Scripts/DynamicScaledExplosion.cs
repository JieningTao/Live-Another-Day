using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicScaledExplosion : BaseExplosion
{
    [SerializeField]
    List<ParticleSystem> SizeScale;
    [SerializeField]
    List<ParticleSystem> AmountScale;

    private void Start()
    {
        ScaleExplosion(ExplosionRadius);
    }


    public void InitializeExplosion(int Layer, int Mask)
    {
        SetLayerAndMask(Layer, Mask);
    }

    public void ScaleExplosion(float Radius)
    {
        foreach (ParticleSystem a in SizeScale)
        {
            a.transform.localScale = Vector3.one * Radius;
        }

        foreach (ParticleSystem a in AmountScale)
        {
            a.transform.localScale = Vector3.one * Radius;
            ParticleSystem.Burst b =a.emission.GetBurst(0);
            b = new ParticleSystem.Burst(0, b.count.constant * ExplosionRadius);
            a.emission.SetBurst(0, b);
        }
    }
}
