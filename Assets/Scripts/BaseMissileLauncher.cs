using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMissileLauncher : BaseKineticShoot
{


    protected BaseMissile MissileScript
    { get { return (ProjectileScript as BaseMissile); } }
    protected List<EnergySignal> Targets = new List<EnergySignal>();
   
    //public override void InitializeGear(BaseMechFCS FCS)
    //{
    //    base.InitializeGear(FCS);

    //    GetSpawns();
    //    InitializeMissile();

    //}

    protected override void Update()
    {
        base.Update();

        if (Firing)
        {
            if (FireCooldown <= 0)
            {
                Fire1();
                if (Targets.Count <= 0)
                    Firing = false;
            }
            else if (Targets.Count <= 0)
                Firing = false;
        }
    }

    protected override void InitializeBullet()
    {

        int SetLayer = 0;
        if (gameObject.layer == 9)
            SetLayer = 10;
        else if (gameObject.layer == 11)
            SetLayer = 12;

        MissileScript.SetLayerAndMask(SetLayer);
        MissileScript.SetDamageSource();
        //MissileScript.InitializeProjectile(PerShotDamage, SetLayer, MyDamageType, MyDamageTags, TrackingSpeed, ActivationDelay, ExplosiveDamage, explosiveForce, ExplosionScript);
    }



    public override void Trigger(bool Fire)
    {


        //something is using trigger to stop the volley fire
        //base.Trigger(Fire);
        //haven't decided what trigger does for this yet
    }

    public void FireVolley(List<EnergySignal> NewTargets)
    {
        //Targets.AddRange(NewTargets);

        if (NewTargets.Count > 0)
        {
            for (int i = 0; i < MagazineRemaining; i++)
            {
                Targets.Add(NewTargets[i % NewTargets.Count]);
            }

            Firing = true;
        }

    }

    public void FireFocusedVolley(EnergySignal Target, int VollyAmount)
    {
        for (int i = 0; i < VollyAmount; i++)
        {
            Targets.Add(Target);
        }
            Firing = true;
    }

    public void FireCustomVolley(List<EnergySignal> NewTargets,int VollyAmount)
    {
        if (NewTargets.Count > 0)
        {
            for (int i = 0; i < MagazineRemaining; i++)
            {
                for (int j = 0; j < VollyAmount; j++)
                {
                    Targets.Add(NewTargets[i % NewTargets.Count]);
                }
            }
            Firing = true;
        }

    }

    public void FireSingleShot(EnergySignal Target)
    {
        Targets.Add(Target);
        Firing = true;
    }

    protected override void Fire1()
    {
        if (Targets.Count > 0)
        {
            Fire1(Targets[0]);
            Targets.RemoveAt(0);
        }
    }

    private void Fire1(EnergySignal Target) //public for testing
    {

        if (MagazineRemaining > 0 && ReloadTimeRemaining <= 0)
        {
            MissileScript.RecieveTarget(Target);
            base.Fire1();
            //MissileScript.RecieveTarget(null);

        }
        else
        {
            Reload();
        }
    }



    public string GetTracking
    { get { return MissileScript.GetTracking; } }

}
