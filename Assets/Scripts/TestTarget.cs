using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : IDamageable
{
    [SerializeField]
    MeshRenderer MyMR;
    [SerializeField]
    Material HitMaterial;
    [SerializeField]
    float IndicatorTime = 0.3f;

    private float IndicatorTimeLeft = 0;
    private Material OriginalMaterial;
    private bool IndicatorOn;

    protected override void Start()
    {
        OriginalMaterial = MyMR.material;
        IndicatorOn = false;
    }

    private void Update()
    {
        if (IndicatorTimeLeft > 0)
            IndicatorTimeLeft -= Time.deltaTime;
        else if (IndicatorOn)
        {
            MyMR.material = OriginalMaterial;
            IndicatorOn = false;
        }
    }

    public override void Hit(float Damage, DamageSystem.DamageType Type, List<DamageSystem.DamageTag> Tags)
    {
        if (!IndicatorOn)
        {
            IndicatorOn = true;
            MyMR.material = HitMaterial;
        }
        IndicatorTimeLeft = IndicatorTime;
    }
}
