using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMissileLauncher : BaseShoot
{


    protected BaseMissile MissileScript;
    protected List<EnergySignal> Targets = new List<EnergySignal>();

    //public override void InitializeGear(BaseMechFCS FCS)
    //{
    //    base.InitializeGear(FCS);

    //    GetSpawns();
    //    InitializeMissile();

    //}

    protected override void Update()
    {

        if (FireCooldown > 0)
            FireCooldown -= Time.deltaTime;
        else
        {
            if (Firing)
            {


                if (Targets.Count > 0)
                {
                    Fire1(Targets[0]);
                    FireCooldown = TBS;
                    Targets.RemoveAt(0);
                }
                else
                {
                    Firing = false;
                    Targets.Clear();
                }
            }
        }
        
    }

    protected override void InitializeBullet()
    {

        MissileScript = ProjectilePrefab.GetComponent<BaseMissile>();

        int SetLayer = 0;
        if (gameObject.layer == 9)
            SetLayer = 10;
        else if (gameObject.layer == 11)
            SetLayer = 12;

        MissileScript.SetLayerAndMask(SetLayer);

        //MissileScript.InitializeProjectile(PerShotDamage, SetLayer, MyDamageType, MyDamageTags, TrackingSpeed, ActivationDelay, ExplosiveDamage, explosiveForce, ExplosionScript);
    }

    public void FireVolly(List<EnergySignal> NewTargets)
    {
        Targets.AddRange(NewTargets);
        Firing = true;
    }

    public virtual void Trigger(bool Fire)
    {
        //haven't decided what trigger does for this yet
    }

    public void Fire1(EnergySignal Target) //public for testing
    {
        MissileScript.RecieveTarget(Target);
        base.Fire1();
        MissileScript.RecieveTarget(null);
    }


}
